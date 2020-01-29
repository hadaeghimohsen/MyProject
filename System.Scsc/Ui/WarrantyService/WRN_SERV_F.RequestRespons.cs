using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.WarrantyService
{
   partial class WRN_SERV_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string CurrentUser;
      private string RegnLang = "054";


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

         if (keyData == Keys.F1)
         {

         }
         else if (keyData == Keys.Escape)
         {
            job.Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if(keyData == Keys.Enter)
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

         var GetUserAccount =
            new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);

         _DefaultGateway.Gateway(
            GetUserAccount
         );
         CurrentUser = GetUserAccount.Output.ToString();

         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());


         #region Set Localization
         var regnlang = iScsc.V_User_Localization_Forms.Where(rl => rl.FORM_NAME == GetType().Name);
         if (regnlang.Count() > 0 && regnlang.First().REGN_LANG != RegnLang)
         {
            RegnLang = regnlang.First().REGN_LANG;
            // Ready To Change Text Title
            foreach (var control in regnlang)
            {
               switch (control.CNTL_NAME.ToLower())
               {
                  default: break;
               }
            }
         }
         #endregion

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);
         

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
         //Job _InteractWithJob =
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.External, "Commons",
         //            new List<Job>
         //            {
         //               #region Access Privilege
         //               new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
         //               {
         //                  Input = new List<string> {"<Privilege>245</Privilege><Sub_Sys>5</Sub_Sys>", "DataGuard"},
         //                  AfterChangedOutput = new Action<object>((output) => {
         //                     if ((bool)output)
         //                        return;
         //                     #region Show Error
         //                     job.Status = StatusType.Failed;
         //                     MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 245 امنیتی", "خطا دسترسی");
         //                     #endregion                           
         //                  })
         //               },
         //               #endregion                        
         //            })                     
         //         });
         //_DefaultGateway.Gateway(_InteractWithJob);
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         DPsatBs.DataSource = iScsc.D_PSATs;
         DPmstBs.DataSource = iScsc.D_PMSTs;
         DPTypBs.DataSource = iScsc.D_PTYPs;
         DCyclBs.DataSource = iScsc.D_CYCLs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if(xinput != null)
         {
            if (xinput.Attribute("fileno") != null)
            {
               fileno = Convert.ToInt64(xinput.Attribute("fileno").Value);
               if (RqstBs.Count > 0 && (RqstBs.Current as Data.Request).RQID > 0)
                  RqstBs.AddNew();

               RqstBnARqt1_Click(null, null);

               // اولین گام این هست که ببینیم آیا ما توانسته ایم برای مشترک درخواست درآمد متفرقه ثبت کنیم یا خیر
               var fg = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno);
               if (!(fg.FIGH_STAT == "001" && fg.RQST_RQID != null && fg.Request.RQTP_CODE == "031"))
               {
                  MessageBox.Show("ثبت درخواست برای مشتری با مشکلی مواجه شده است، لطفا بررسی کنید");
                  return;
               }
               // 1398/10/17 * پیدا کردن درخواست برای مشتری
               RqstBs.Position = RqstBs.IndexOf(RqstBs.List.OfType<Data.Request>().FirstOrDefault(r => r.Request_Rows.Any(rr => rr.Fighter.FILE_NO == fileno)));
            }
            else
               fileno = null;
         }
         Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
