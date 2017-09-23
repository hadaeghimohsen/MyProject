using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using System.Data;
using System.Xml.Linq;

namespace System.DataGuard.Login.Ui
{
   partial class Login : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.User SelectedUser;
      private string ConnectionString;
      private Data.iProjectDataContext iProject;


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
         else if(keyData == (Keys.Control | Keys.Alt | Keys.ControlKey))
         {
            te_username.Text = "artauser";
            te_password.Text = "hvjhuser1020";
            GotoValidation(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Alt | Keys.D))
         {
            te_username.Text = "demo";
            te_password.Text = "demo";
            GotoValidation(null, null);
         }
         else if (keyData == Keys.F2)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "Login", 02 /* Execute Set */);
         }
         job.Status = StatusType.Successful;
      }
      
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {         
         string value = job.Input as string;
         if (value.ToLower() == "username")
            job.Output = te_username.Text.ToLower();
         else if (value.ToLower() == "userfaname")
            job.Output = XElement.Parse(Tag.ToString()).Descendants("UserFaName").First().Value;
         else if (value.ToLower() == "both")
            job.Output = Tag;
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         Enabled = true;
         //te_username.Text = te_password.Text = "";
         te_username.Focus();

         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };

         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output as string;


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

         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
         //   {
         //      Next = new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */)
         //      {
         //         Input = new List<object> { "DataGuard:Login:Login", this },
         //         Next = new Job(SendType.SelfToUserInterface, "Wall", 00 /* PastManualOnWall */)
         //         {
         //            Input = new List<object> { this, "cntrhrz:default" },
         //            Next = new Job(SendType.SelfToUserInterface, "Wall", 19 /* Execute SetClearForm */)
         //         }
         //      }
         //   };

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
                 new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */,
                    new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
                       new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */)
                       {
                          Input = this
                       }
                    )
                 );

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



         if(te_username.Text.Trim() != "")
         {
            iProject = new Data.iProjectDataContext(ConnectionString);
            SelectedUser = iProject.Users.FirstOrDefault(u => u.USERDB.ToLower() == te_username.Text.ToLower());
            User_RondButn.ImageProfile = GetUserImage(SelectedUser);
         }

         Job _GetUserProfile =
            new Job(SendType.External, "Login",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format("<UserName>{0}</UserName>", te_username.Text.ToLower()),
                              "{ Call DataGuard.GetUserProfile(?) }",
                              "iProject",
                              "scott"
                           },
                           UpdateBeforeChangingOupput = new Func<object,object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 var _UsrPro = ds.Tables["User"].Rows[0];
                                 Tag = string.Format("<UserProfile><UserID>{2}</UserID><UserFaName>{0}</UserFaName><UserEnName>{1}</UserEnName></UserProfile>", _UsrPro["TitleFa"], te_username.Text.ToLower(), _UsrPro["ID"]);
                                 return Tag;
                              })
                        }
                     }){GenerateInputData = GenerateDataType.Dynamic},
                  new Job(SendType.SelfToUserInterface, "Wall", 18 /* Execute SetTextOnTitleForm */){WhereIsInputData = WhereIsInputDataType.StepBack}
               }) { GenerateInputData = GenerateDataType.Dynamic};
         _DefaultGateway.Gateway(_GetUserProfile);
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadDataAsync(Job job)
      {
         SelectedUser = job.Input as Data.User;
         if(InvokeRequired)
         {            
            Invoke(
               new Action(
                  () =>
                  {
                     te_username.Text = SelectedUser.TitleEn;
                     te_password.Text = SelectedUser.Password;
                     User_RondButn.ImageProfile = GetUserImage(SelectedUser);
                     GotoValidation(null, null);
                  }
               )
            );
         }
         else
         {
            te_username.Text = SelectedUser.TitleEn;
            te_password.Text = SelectedUser.Password;
            User_RondButn.ImageProfile = GetUserImage(SelectedUser);
            GotoValidation(null, null);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallForm(Job job)
      {
         var input = job.Input as XElement;
         switch (input.Attribute("type").Value)
         {
            case "changeusername":
               te_username.Text = input.Attribute("value").Value;
               break;
            case "changepassword":
               te_password.Text = input.Attribute("value").Value;
               break;
            default:
               break;
         }
      }
   }
}
