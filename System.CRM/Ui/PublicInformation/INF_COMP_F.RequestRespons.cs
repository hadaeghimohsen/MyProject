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

namespace System.CRM.Ui.PublicInformation
{
   partial class INF_COMP_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      int Sleeping = 5;
      int Step = 45;
      enum DetailShow { None, Minimum, Maximum };
      DetailShow detailShow = DetailShow.None;

      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      private string CurrentUser;
      private string formCaller;

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
               OpenDrawer(job);
               break;
            case 06:
               CloseDrawer(job);
               break;
            case 07:
               LoadData(job);
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

         if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.Escape)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 04 /* Execute UnPaint */, SendType.SelfToUserInterface)
            );
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iCRM</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output.ToString();
         iCRM = new Data.iCRMDataContext(GetConnectionString.Output.ToString());

         CurrentUser = iCRM.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));
         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);

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
         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("CRM:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
         //   });
         //_DefaultGateway.Gateway(_Paint);
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("CRM:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "left:in-screen:stretch:center"} }               
            });
         _DefaultGateway.Gateway(_Paint);

         this.Enabled = true;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
         //         new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
         //         //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
         //      })
         //   );
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
      private void OpenDrawer(Job job)
      {
         bool toggleMode = (bool)job.Input;

         switch (detailShow)
         {
            case DetailShow.None:
               break;
            case DetailShow.Minimum:
               for (int i = 0; i <= Width; i += Step)
               {
                  Invoke(new Action(() => { Left -= Step; }));
                  Thread.Sleep(Sleeping);
               }
               Invoke(new Action(() => { Left += toggleMode ? 35 : 45; }));
               detailShow = DetailShow.Maximum;
               break;
            case DetailShow.Maximum:
               for (int i = 0; i <= Height - 90; i += Step)
               {
                  Invoke(new Action(() => { Top -= Step; }));
                  Thread.Sleep(Sleeping);
               }
               detailShow = DetailShow.None;
               break;
            default:
               break;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void CloseDrawer(Job job)
      {
         for (int i = 0; i <= Width; i += Step)
         {
            Invoke(new Action(() => { Left += Step; }));
            Thread.Sleep(Sleeping);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         DcntpBs.DataSource = iCRM.D_CNTPs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         xinput = job.Input as XElement;
         if(xinput != null)
         {
            formCaller = xinput.Attribute("formcaller").Value;
            compcode = Convert.ToInt64(xinput.Attribute("compcode").Value);
            Execute_Query();
         }
         
         job.Status = StatusType.Successful;
      }

   }
}

