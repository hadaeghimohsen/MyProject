using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace MyProject.Programs.Code
{
   partial class Program : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "Commons":
               _Commons.Gateway(jobs);
               break;   
            case "DataGuard":
               _DataGuard.Gateway(jobs);
               break;
             case "ServiceDefinition":
               _ServiceDefinition.Gateway(jobs);
               break;
            case "Reporting":
               _Reporting.Gateway(jobs);
               break;
            case "Gas":
               if(_Gas != null)
                  _Gas.Gateway(jobs);
               break;
            case "Scsc":
               if (_Scsc != null)
                  _Scsc.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "Sas":
               if(_Sas != null)
                  _Sas.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "Msgb":
               if (_Msgb != null)
                  _Msgb.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "ISP":
               if (_ISP != null)
                  _ISP.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "CRM":
               if (_CRM != null)
                  _CRM.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "RoboTech":
               if (_RoboTech != null)
                  _RoboTech.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "Setup":
               if (_Setup != null)
                  _Setup.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            case "GateControl":
               if (_Setup != null)
                  _Setup.Gateway(jobs);
               else
               {
                  //System.Windows.Forms.MessageBox.Show(_errorForNotInstallDll);
                  jobs.Status = StatusType.Failed;
               }
               break;
            default:
               jobs.Status = StatusType.Failed;
               break;
         }
      }

      protected override void InternalAssistance(Job job)
      {
         switch (job.Method)
         {
            case 01:
               Startup(job);
               break;
            case 02:
               Start_Service_Component(job);
               break;
            case 03:
               Stop_Service_Compnent(job);
               break;
            case 04:
               ReadyToWorkStatus(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }

      protected override void RequestToUserInterface(Job job)
      {
         switch (job.Gateway)
         {
            case "Wall":
               _Wall.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
