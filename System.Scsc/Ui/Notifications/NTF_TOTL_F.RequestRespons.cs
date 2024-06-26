﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Notifications
{
   partial class NTF_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private bool isFirstLoaded = false;
      private string formStatus = "";
      private Data.Setting stng;

      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
               CheckSecurity(job);
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
               SendKeys.Send("{TAB}");
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         // 1402/11/21 * برای جناب صمیمی که درخواست کردن دیگه خطا نباشه
         stng = iScsc.Settings.FirstOrDefault(s => Fga_Uclb_U.Contains(s.CLUB_CODE));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         if (job.Input == null || (job.Input != null && (job.Input as XElement).Attribute("actntype").Value != "JustRunInBackground"))
         {
            formStatus = "RunInForeground";
            Job _Paint = new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
               });
            _DefaultGateway.Gateway(_Paint);
         }
         else
         {
            formStatus = "JustRunInBackground";
            Dt_CrntDate2R.Value = DateTime.Now;
         }

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (isFirstLoaded) goto finishcommand;

         if (InvokeRequired)
            Invoke(new Action(() => {
               Dt_CrntDate2R.Value = DateTime.Now;
               Dt_CrntDate4R.Value = DateTime.Now;
            }));
         else
         {
            Dt_CrntDate2R.Value = DateTime.Now;
            Dt_CrntDate4R.Value = DateTime.Now;
         }
         DSxtpBs1.DataSource = iScsc.D_SXTPs;
         DFgtpBs1.DataSource = iScsc.D_FGTPs;
         DAttpBs.DataSource = iScsc.D_ATTPs;
         DActvBs2.DataSource = iScsc.D_ACTVs;
         DcktpBs.DataSource = iScsc.D_CKTPs;
         DsmtpBs.DataSource = iScsc.D_SMTPs;
         DysnoBs.DataSource = iScsc.D_YSNOs;
         //DresBs2.DataSource = iScsc.Dressers.Where(d => Fga_Uclb_U.Contains(d.CLUB_CODE));

         isFirstLoaded = true;

         finishcommand:
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         //tb_master.Pages.Clear();
         var input = job.Input as XElement;
         switch (input.Attribute("type").Value)
         {
            case "attn":
               //tb_master.Pages.Add(tp_002);
               tb_master.SelectedTab = tp_002;
               if (input.Attribute("mbsprwno") != null)
                  mbsprwno = Convert.ToInt16(input.Attribute("mbsprwno").Value);
               else
                  mbsprwno = 0;

               if (input.Attribute("attnsystype") != null)
                  attnsystype = input.Attribute("attnsystype").Value;
               else
                  attnsystype = "002";

               if (input.Attribute("attnignrtype") != null)
                  attnignrtype = input.Attribute("attnignrtype").Value;
               else
                  attnignrtype = "001";

               if (input.Attribute("compname") != null)
                  compname = input.Attribute("compname").Value;
               else
                  compname = "";

               if (input.Attribute("chckattnalrm") != null)
                  chckattnalrm = input.Attribute("chckattnalrm").Value;
               else
                  chckattnalrm = "001";

               if (input.Attribute("barcodedata") != null && input.Attribute("barcodedata").Value != "")
               {
                  Pb_FileName2.PickChecked = true;
                  Lov_FileName2.EditValue = Convert.ToDecimal(input.Attribute("barcodedata").Value.Substring(14));
                  Btn_Attn_Click(null, null);
               }
               else if (input.Attribute("enrollnumber") != null && input.Attribute("enrollnumber").Value != "")
               {
                  Pb_FileName2.PickChecked = true;
                  Lov_FileName2.Tag = input.Attribute("enrollnumber").Value;
                  Lov_FileName2.EditValue = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == input.Attribute("enrollnumber").Value).FILE_NO;
                  Btn_Attn_Click(null, null);
               }
               break;
            case "endinsr":
               //tb_master.Pages.Add(tp_004);
               tb_master.SelectedTab = tp_004;
               break;
            case "reportcheck":
               tb_master.SelectedTab = tp_006;
               break;
         }
         Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
