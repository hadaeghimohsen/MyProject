using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace System.DataGuard.Login.Code
{
   partial class Login
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         if (iProject == null)
         {
            var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
            _DefaultGateway.Gateway(
               GetConnectionString
            );
            if (GetConnectionString.Output != null)
            {
               ConnectionString = GetConnectionString.Output.ToString();
               iProject = new Data.iProjectDataContext(ConnectionString);
            }
            else
            {
               job.Status = StatusType.Failed;
            }
         }
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Login(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "Login", 02 /* Execute Set */,
               new Job(SendType.SelfToUserInterface, "Login", 03 /* Execute Paint */));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SaveHostInfo(Job job)
      {
         try
         {
            #region Check Tiny Lock
            var _jobCheckTiny =
               new Job(SendType.External, "Localhost", "", 05 /* Execute DoWork4TinyLock  */, SendType.Self);
            _DefaultGateway.Gateway(_jobCheckTiny);

            if (_jobCheckTiny.Output != null)
            {
               var _jobUnSecureHashCode =
                  new Job(SendType.External, "Localhost", "", 08 /* Execute DoWork4UnSecureHashCode  */, SendType.Self) { Input = (_jobCheckTiny.Output as XElement).Value };
               _DefaultGateway.Gateway(_jobUnSecureHashCode);
               MessageBox.Show(_jobUnSecureHashCode.Output.ToString());
               Application.Exit();
               Process.GetCurrentProcess().Kill();
            }
            #endregion

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
               {
                  AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     iProject = null;
                     GetUi(null);
                     //iProject.SaveActiveSession(new XElement("ActiveSession", new XAttribute("database", "iProject")));
                     iProject.SaveHostInfo(output as XElement);
                     //var xparam = XDocument.Parse(string.Format("<Request Rqtp_Code=""ManualSaveHostInfo"">{0}</Request>", ));
                     if (iProject.CheckHostUserEntry(output as XElement) != "002")
                     {
                        Windows.Forms.MessageBox.Show("کاربر گرامی این سیستم برای شما تعریف نشده لطفا با پشتیبانی نرم افزار تماس بگیرید.");
                        Application.Exit();
                        Process.GetCurrentProcess().Kill();
                     }
                  })
               }
            );
            if (job.Status != StatusType.Failed)
               job.Status = StatusType.Successful;
         }
         catch (Exception ex)
         {
            MessageBox.Show((ex.Message == "Object reference not set to an instance of an object." ? "" : ex.Message) + "\r\n" + "کاربر گرامی این سیستم برای شما تعریف نشده لطفا با پشتیبانی نرم افزار تماس بگیرید.");
            Application.Exit();
            Process.GetCurrentProcess().Kill();
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LockScreen(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "LockScreen", 02 /* Execute Set */,
               new Job(SendType.SelfToUserInterface, "LockScreen", 03 /* Execute Paint */));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LastUserLogin(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "LastUserLogin", 02 /* Execute Set */,
               new Job(SendType.SelfToUserInterface, "LastUserLogin", 03 /* Execute Paint */,
                  new Job(SendType.SelfToUserInterface, "LastUserLogin", 08 /* Execute LoadDataAsync */) { Executive = ExecutiveType.Synchronize }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SelectedLastUserLogin(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "SelectedLastUserLogin", 02 /* Execute Set */,
               new Job(SendType.SelfToUserInterface, "SelectedLastUserLogin", 03 /* Execute Paint */,
                  new Job(SendType.SelfToUserInterface, "SelectedLastUserLogin", 08 /* Execute LoadDataAsync */) { Input = job.Input , Executive = ExecutiveType.Asynchronous }));
         job.Status = StatusType.Successful;
      }
   }
}
