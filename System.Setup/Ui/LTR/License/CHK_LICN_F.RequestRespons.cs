using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Setup.Ui.LTR.License
{
   partial class CHK_LICN_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
     

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
               ActionCallWindow(job);
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
               //new Job(SendType.SelfToUserInterface, GetType().Name, 06 /* Execute CloseDrawer */)
               //{
               //   Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
               //};
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
         _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "DefaultGateway",
                        new List<Job>
                        {
                           new Job(SendType.External, "DataGuard", 
                              new List<Job>
                              {
                                 new Job(SendType.Self, 31 /* Execute DoWork4GetComputerName */ ){AfterChangedOutput = new Action<object>((output) => HostInstaller_Txt.Text = output.ToString() )},
                              }
                           )                           
                        }
                     )
                  }
               )
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "DataGuard:SecurityPolicy:" + GetType().Name, this }  },
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
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
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
      private void ActionCallWindow(Job job)
      {
         var xinput = job.Input as XElement;
         if(xinput != null)
         {
            if(xinput.Attribute("type") != null)
            {
               var subsys = xinput.Attribute("subsys").Value;
               switch (xinput.Attribute("type").Value)
               {
                  case "install":
                     switch (subsys)
	                  {
                        case "iproject":
                           job.Output = 
                              new XElement("License",
                                 new XAttribute("status", Kernel1_Cb.Checked ? "valid" : "notvalid"),
                                 new XAttribute("key", Kernel1_Cb.Checked ? ServerKey0_Text.Text : "")
                              );
                           break;
                        case "iscsc":
                           job.Output =
                              new XElement("License",
                                 new XAttribute("status", Arta1_Cb.Checked ? "valid" : "notvalid"),
                                 new XAttribute("key", Arta1_Cb.Checked ? ServerKey0_Text.Text : "")
                              );
                           break;
                        case "icrm":
                           job.Output =
                              new XElement("License",
                                 new XAttribute("status", CRM1_Cb.Checked ? "valid" : "notvalid"),
                                 new XAttribute("key", CRM1_Cb.Checked ? ServerKey0_Text.Text : "")
                              );
                           break;
                        case "irobotech":
                           job.Output =
                              new XElement("License",
                                 new XAttribute("status", Telegram1_Cb.Checked ? "valid" : "notvalid"),
                                 new XAttribute("key", Telegram1_Cb.Checked ? ServerKey0_Text.Text : "")
                              );
                           break;
                        default:
                           job.Output =
                              new XElement("License",
                                 new XAttribute("status", "notvalid"),
                                 new XAttribute("key", "")
                              );
                           break;
	                  }
                     break;
                  case "update":
                     break;
                  case "remove":
                     break;
                  default:
                     break;
               }
            }
         }
         job.Status = StatusType.Successful;
      }
   }
}
