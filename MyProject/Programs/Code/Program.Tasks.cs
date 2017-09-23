using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace MyProject.Programs.Code
{
   partial class Program
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Startup(Job job)
      {
         try
         {
            job.Status = StatusType.Successful;
            _Wall.ShowDialog();
         }
         catch(Exception exc) 
         {
            System.Windows.Forms.MessageBox.Show(exc.Message + "\n\r" +  exc.StackTrace, "خطای ناشناخته");
            Application.Exit();
            Process.GetCurrentProcess().Kill();
         }
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Start_Service_Component(Job job)
      {
         _readyToWork = true;
         if (_ServiceDefinition == null)
         {
            _ServiceDefinition = new System.ServiceDefinition.Share.Code.Services(_Commons, _Wall) { _DefaultGateway = this };
            _Reporting = new System.Reporting.Self.Code.Reporting(_Commons, _Wall) { _DefaultGateway = this };
            //_Gas = new System.Gas.Self.Code.Gas(_Commons, _Wall) { _DefaultGateway = this };
            _Scsc = new System.Scsc.Code.Scsc(_Commons, _Wall) { _DefaultGateway = this };
            _Sas = new System.Emis.Sas.Controller.Sas(_Commons, _Wall) { _DefaultGateway = this };
            _Msgb = new System.MessageBroadcast.Code.Msgb(_Commons, _Wall) { _DefaultGateway = this };
            _ISP = new System.ISP.Code.ISP(_Commons, _Wall) { _DefaultGateway = this };
            _CRM = new System.CRM.Code.CRM(_Commons, _Wall) { _DefaultGateway = this };
            _RoboTech = new System.RoboTech.Code.RoboTech(_Commons, _Wall) { _DefaultGateway = this };
         }
         else
         {
            /* بازسازی و بازیابی پروسه های غیرفعال برای فعالیت مجدد */
            Job _StartComponent = new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Msgb", "", 03 /* Execute Actn_Extr_P */, SendType.Self)
                  {
                     Input = new XElement("Process", 
                                 new XElement("Action", 
                                    new XAttribute("type", "003"),
                                    new XAttribute("value", "true")                                 
                                 )
                              )
                  }
               });
            Gateway(_StartComponent);
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void Stop_Service_Compnent(Job job)
      {
         _readyToWork = false;
         /* در این قسمت بایستی به آن دسته از سرویس های پس زمینه در هر ماژول پیام غیرفعال شدن را ارسال کنیم */
         Job _StopComponent = new Job(SendType.External, "Localhost",
            new List<Job>
            {
               new Job(SendType.External, "Msgb", "", 03 /* Execute Actn_Extr_P */, SendType.Self)
               {
                  Input = new XElement("Process", 
                              new XElement("Action", 
                                 new XAttribute("type", "003"),
                                 new XAttribute("value", "false")                                 
                              )
                           )
               }
            });
         Gateway(_StopComponent);
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void ReadyToWorkStatus(Job job)
      {
         job.Output = _readyToWork;
         job.Status = StatusType.Successful;
      }
   }
}
