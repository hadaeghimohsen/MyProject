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
   partial class PinCode : ISendRequest
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
         //else if (keyData == Keys.Escape)
         //{
         //   job.Next =
         //      new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         //}
         //else if (((int)keyData >= 48 && (int)keyData <= 57) || ((int)keyData >= 96 && (int)keyData <= 105))
         //{
         //   switch (keyData)
         //   {               
         //      case Keys.Back:
         //         if (PinCode_Txt.Text.Length > 0)
         //            PinCode_Txt.Text = PinCode_Txt.Text.Substring(0, PinCode_Txt.Text.Length - 1);
         //         break;               
         //      case Keys.D0:
         //      case Keys.NumPad0:
         //         PinCode_Txt.Text += "0";
         //         break;
         //      case Keys.D1:
         //      case Keys.NumPad1:
         //         PinCode_Txt.Text += "1";
         //         break;
         //      case Keys.D2:
         //      case Keys.NumPad2:
         //         PinCode_Txt.Text += "2";
         //         break;
         //      case Keys.D3:
         //      case Keys.NumPad3:
         //         PinCode_Txt.Text += "3";
         //         break;
         //      case Keys.D4:
         //      case Keys.NumPad4:
         //         PinCode_Txt.Text += "4";
         //         break;
         //      case Keys.D5:
         //      case Keys.NumPad5:
         //         PinCode_Txt.Text += "5";
         //         break;
         //      case Keys.D6:
         //      case Keys.NumPad6:
         //         PinCode_Txt.Text += "6";
         //         break;
         //      case Keys.D7:
         //      case Keys.NumPad7:
         //         PinCode_Txt.Text += "7";
         //         break;
         //      case Keys.D8:
         //      case Keys.NumPad8:
         //         PinCode_Txt.Text += "8";
         //         break;
         //      case Keys.D9:
         //      case Keys.NumPad9:
         //         PinCode_Txt.Text += "9";
         //         break;
         //      case Keys.Enter:
         //         break;               
         //   }
         //}
         //else if(keyData == (Keys.Control | Keys.Back))
         //{
         //}
         //else if(keyData == (Keys.Alt | Keys.F4))
         //{

         //}

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
         Enabled = true;

         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };

         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output as string;

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "DataGuard:Login:PinCode", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = /*new List<object> {this, "cntrhrz:default"}*/ this }
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
         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
         //      new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
         //         new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */,
               new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
                     new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                        new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
                     ) { Input = this }
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
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallForm(Job job)
      {
         var xinput = job.Input as XElement;

         iProject = new Data.iProjectDataContext(ConnectionString);
         SelectedUser = iProject.Users.FirstOrDefault(u => u.USERDB.ToLower() == xinput.Attribute("username").Value.ToLower());
         User_RondButn.ImageProfile = GetUserImage(SelectedUser);
         UserName_Lb.Text = SelectedUser.TitleFa;
         job.Status = StatusType.Successful;
      }
   }
}
