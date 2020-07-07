using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   partial class ORGN_DVLP_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iRoboTechDataContext iRoboTech;
      private string ConnectionString;
      private List<long?> Fga_Ugov_U;

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
            case 40:
               CordinateGetSet(job);
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

         if (keyData == Keys.Enter)
         {
            if (!(Btn_Back.Focused))
               SendKeys.Send("{TAB}");
         }
         else if(keyData == Keys.F4)
         {
            Actn_Butn_ButtonClick(Actn_Butn, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Actn_Butn.Buttons[2]));
         }
         else if (keyData == (Keys.Shift | Keys.Delete))
         {
            Tsb_DelMenu_Click(null, null);
         }
         else if (keyData == (Keys.F5))
         {
            SubmitChanged_Clicked(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Insert))
         {
            AddSubMenu_Butn_Click(null, null);
         }
         else if(keyData == (Keys.Alt | Keys.Insert))
         {
            AddTopSubMenu_Butn_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.B))
         {
            Tsb_CreateBackMenu_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Up))
         {
            ConvertToStartMenu_Butn_Click(null, null);
         }
         else if (keyData == Keys.F9)
         {
            Tsb_SearchInMenu_Click(null, null);
         }
   
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iRoboTech</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output.ToString();
         iRoboTech = new Data.iRoboTechDataContext(GetConnectionString.Output.ToString());

         Fga_Ugov_U = (iRoboTech.FGA_UGOV_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         //var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         //_DefaultGateway.Gateway(GetHostInfo);

         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         //);

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
               //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("RoboTech:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
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
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>36</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 36 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
            _DefaultGateway.Gateway(_InteractWithJob);
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         DactvBs.DataSource = iRoboTech.D_ACTVs;
         CntyBs.DataSource = iRoboTech.Countries;
         DbuldBs.DataSource = iRoboTech.D_BULDs;
         vUserBs.DataSource = iRoboTech.V_Users;
         DordtBs.DataSource = iRoboTech.D_ORDTs;
         DcmtpBs.DataSource = iRoboTech.D_CMTPs;
         DysnoBs.DataSource = iRoboTech.D_YSNOs;
         DpktpBs.DataSource = iRoboTech.D_PKTPs;
         DbtypBs.DataSource = iRoboTech.D_BTYPs;
         DbdirBs.DataSource = iRoboTech.D_BDIRs;
         DAmutBs.DataSource = iRoboTech.D_AMUTs;
         DAcntBs.DataSource = iRoboTech.D_ACNTs;
         DMntpBs.DataSource = iRoboTech.D_MNTPs;
         DDstpBs.DataSource = iRoboTech.D_DSTPs;
         DShinBs.DataSource = iRoboTech.D_SHINs;
         UApbBs.DataSource = iRoboTech.App_Base_Defines.Where(r => r.ENTY_NAME == "PRODUCTUNIT_INFO");
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         Execute_Query();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void CordinateGetSet(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput != null)
         {
            var robo = RoboBs.Current as Data.Robot;

            if (xinput.Attribute("outputtype").Value == "robotpostadrs")
            {
               //MessageBox.Show(xinput.ToString());

               var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
               var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);

               if (cordx != robo.CORD_X && cordy != robo.CORD_Y)
               {
                  // Call Update Service_Public
                  try
                  {
                     robo.CORD_X = cordx;
                     robo.CORD_Y = cordy;
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     MessageBox.Show(exc.Message);
                  }
               }
            }            
         }

         job.Status = StatusType.Successful;
      }

   }
}
