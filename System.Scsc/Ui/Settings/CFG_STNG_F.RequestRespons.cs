using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Settings
{
   partial class CFG_STNG_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string Crnt_User;
      private string Modul_Name, Section_Name;

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
            case 08:
               LoadDataSource(job);
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
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            // 1396/10/23 * 
            switch (Modul_Name)
            {
               case "WHO_ARYU_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {                           
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),                           
                        })
                  );
                  break;
               default:
                  break;
            }
            Modul_Name = "";
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
         }
         else if (keyData == Keys.Enter)
         {
         }
         else if (keyData == Keys.F2)
         {
         }
         else if (keyData == Keys.F8)
         {
         }
         else if (keyData == Keys.F5)
         {
         }
         else if (keyData == Keys.F3)
         {
         }
         else if (keyData == Keys.F10)
         {
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
         Crnt_User = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

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
         Job _CheckSecurity = 
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>134</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 134 سطوح امینتی");
                           })
                        },
                        #endregion
                     }),
               });
         _DefaultGateway.Gateway(_CheckSecurity);
         job.Status = _CheckSecurity.Status;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         try
         {
            #region Settings
            //PrvnBs1.DataSource = iScsc.Provinces;
            UserBs1.DataSource = iScsc.V_Users;
            DCstpBs1.DataSource = iScsc.D_CSTPs;
            DActvBs1.DataSource = iScsc.D_ACTVs;
            DIttpBs1.DataSource = iScsc.D_ITTPs;
            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DYsnoBs1.DataSource = iScsc.D_YSNOs;
            DAtsmBs4.DataSource = iScsc.D_ATSMs;
            DBcdtBs4.DataSource = iScsc.D_BCDTs;

            

            MtodBs1.DataSource = iScsc.Methods;
            RqtpBs1.DataSource = iScsc.Request_Types;
            RqttBs1.DataSource = iScsc.Requester_Types;
            CochBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE));
            vCompBs1.DataSource = iScsc.V_Computers;
            Lb_ListPort.Items.Clear();
            foreach (var portName in SerialPort.GetPortNames())
	         {
               Lb_ListPort.Items.Add(portName);
	         } 
            //Execute_Query();
            #endregion
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         CochBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE));
         MtodBs1.DataSource = iScsc.Methods;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         //tc_Settings.TabPages.Remove(tp_backuprestore);
         //tc_Settings.TabPages.Remove(tp_base);
         //tc_Settings.TabPages.Remove(tp_printmodual);
         tc_Settings.TabPages.Clear();
         var input = job.Input as XElement;
         switch (input.Attribute("type").Value)
         {
            case "BackupRestore":
               tc_Settings.TabPages.Add(tp_backuprestore);
               tc_Settings.SelectedTab = tp_backuprestore;               
               break;
            case "EmerjncyBackup":
               tc_Settings.TabPages.Add(tp_backuprestore);
               tc_Settings.SelectedTab = tp_backuprestore;
               Execute_Query();
               Btn_TakeBackup_Click(null, null);
               return;
            case "UserRegionClub":
               tc_Settings.TabPages.Add(tp_base);
               tc_Settings.SelectedTab = tp_base;
               switch(input.Attribute("section").Value)
               {
                  case "userview":
                     tsm_bas_close_Click(null, null);
                     tsm_bas_R1_Click(null, null);
                     break;
                  case "cash":
                     tsm_bas_close_Click(null, null);
                     tsm_bas_R2_Click(null, null);
                  break;
                  case "epit":
                     tsm_bas_close_Click(null, null);
                     tsm_bas_R3_Click(null, null);
                  break;
                  case "regn":
                     tsm_bas_close_Click(null, null);
                     tsm_bas_R4_Click(null, null);
                  break;
               }
               break;
            case "ModualReport":
               tc_Settings.TabPages.Add(tp_printmodual);
               tc_Settings.SelectedTab = tp_printmodual;
               Modul_Name = input.Attribute("modul").Value;
               Section_Name = input.Attribute("section").Value;
               break;
            case "tp_004":
               tc_Settings.TabPages.Add(tp_004);
               tc_Settings.SelectedTab = tp_004;
               break;
            default:
               break;
         }
         Execute_Query();
         //Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
