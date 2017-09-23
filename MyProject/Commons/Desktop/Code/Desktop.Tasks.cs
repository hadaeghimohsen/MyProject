using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.Desktop.Code
{
   partial class Desktop
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input as string;

         if (value.ToLower() == "desktop")
         {
            if (_Desktop == null)
               _Desktop = new Ui.Desktop { _DefaultGateway = this };
            job.Output = _Desktop;
         }
         else if(value.ToLower() == "startdrawer")
         {
            if (_StartDrawer == null)
               _StartDrawer = new Ui.StartDrawer { _DefaultGateway = this };
            job.Output = _StartDrawer;
         }
         else if (value.ToLower() == "settingsdrawer")
         {
            if (_SettingsDrawer == null)
               _SettingsDrawer = new Ui.SettingsDrawer { _DefaultGateway = this };
            job.Output = _StartDrawer;
         }
         else if (value.ToLower() == "startmenu")
         {
            if (_StartMenu == null)
               _StartMenu = new Ui.StartMenu { _DefaultGateway = this };
            job.Output = _StartMenu;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Desktop(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "Desktop", 02 /* Execute Set */)
            {
               Next = new Job(SendType.SelfToUserInterface, "Desktop", 03 /* Execute Paint */)
            };
         job.Status = StatusType.Successful;
      }
      
      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4StartDrawer(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            bool readToWork = false;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Program", 04 /* Execute ReadyToWorkStatus */, SendType.Self) 
               { 
                  AfterChangedOutput = (output) =>
                     {
                        readToWork = (bool)output;
                     }
               }
            );

            if (readToWork == false) return;

            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "startdrawer"},
                  new Job(SendType.SelfToUserInterface, "StartDrawer", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "StartDrawer", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "StartDrawer", 05 /* Execute OpenDrawer */){Input = job.Input, Executive = ExecutiveType.Asynchronous},
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsDrawer(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            bool readToWork = false;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Program", 04 /* Execute ReadyToWorkStatus */, SendType.Self)
               {
                  AfterChangedOutput = (output) =>
                  {
                     readToWork = (bool)output;
                  }
               }
            );

            if (readToWork == false) return;

            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "settingsdrawer"},
                  new Job(SendType.SelfToUserInterface, "SettingsDrawer", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SettingsDrawer", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "SettingsDrawer", 05 /* Execute OpenDrawer */){Input = job.Input, Executive = ExecutiveType.Asynchronous},
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4StartMenu(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            bool readToWork = false;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Program", 04 /* Execute ReadyToWorkStatus */, SendType.Self)
               {
                  AfterChangedOutput = (output) =>
                  {
                     readToWork = (bool)output;
                  }
               }
            );

            if (readToWork == false) return;

            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "startmenu"},
                  new Job(SendType.SelfToUserInterface, "StartMenu", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "StartMenu", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "StartMenu", 05 /* Execute OpenDrawer */){Input = job.Input, Executive = ExecutiveType.Asynchronous},
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
