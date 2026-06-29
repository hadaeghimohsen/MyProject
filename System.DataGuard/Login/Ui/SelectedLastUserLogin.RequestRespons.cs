using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.DataGuard.Login.Ui
{
   partial class SelectedLastUserLogin : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private string ConnectionString;
      private Data.iProjectDataContext iProject;
      private string CurrentUser;
      private Data.User SelectedUser;
      private XElement HostInfo;

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
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               LoadDataAsync(job);
               break;
            case 10:
               ActionCallForm(job);
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
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Document\Login\Login.html"
                     }
                  });
         }
         else if (keyData == Keys.F2)
         {
            Password_Be.Focus();
            job.Next =
               new Job(SendType.SelfToUserInterface, "Login", 02 /* Execute Set */);
         }
         else if (keyData == Keys.Escape)
         {
            job.Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);            
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };

         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output as string;

         var _GetHostInfo =
            new Job(SendType.External, "localhost", "Commons", 24 /* Execute GetHostInfo */, SendType.Self);
         _DefaultGateway.Gateway(_GetHostInfo);

         HostInfo = _GetHostInfo.Output as XElement;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 09 /* Execute LangChangToEnglish */, SendType.Self)
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
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "DataGuard:Login:" + GetType().Name, this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){ Input = this }               
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
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         /* Get Mac Address And Send to database for save */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 03 /* Execute DoWork4SaveHostInfo */, SendType.Self)
         );         
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadDataAsync(Job job)
      {
         if(InvokeRequired)
         {            
            SelectedUser = job.Input as Data.User;
            Invoke(
               new Action(
                  () =>
                  {
                     Password_Be.Text = "";
                     Password_Be.Focus();
                     User_RondButn.ImageProfile = GetUserImage(SelectedUser);
                     User_Txt.Text = SelectedUser.USERDB;
                     User_RondButn.Tag = SelectedUser;

                     iProject = new Data.iProjectDataContext(ConnectionString);

                     try
                     {
                        Lb_ShowLoginDesc.Text =
                           iProject.Package_Instance_User_Gateways
                           .FirstOrDefault(piug => piug.User_Gateway.GTWY_MAC_ADRS == HostInfo.Attribute("cpu").Value
                                       && piug.User_Gateway.User.USERDB == SelectedUser.USERDB
                                       && piug.PKIN_PAKG_SUB_SYS == 0
                                       && piug.SHOW_LOG_IN_DESC == "002"
                           ).LOG_IN_DESC ?? "";
                     }
                     catch { Lb_ShowLoginDesc.Text = ""; }
                  }
               )
            );            
         }         
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallForm(Job job)
      {
         job.Status = StatusType.Successful;
      }
   }
}
