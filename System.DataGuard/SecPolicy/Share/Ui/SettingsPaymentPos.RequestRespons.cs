using DirectShowLib;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SettingsPaymentPos : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iProjectDataContext iProject;
      private string ConnectionString;
      private string CurrentUser;
      private XElement HostNameInfo;
      private XElement Xinput;
      private Data.Pos_Device Pos_Device;


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
            case 08:
               LoadDataAsync(job);
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
         else if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
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
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };

         _DefaultGateway.Gateway(
            GetConnectionString
         );

         var GetUserAccount =
            new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);

         _DefaultGateway.Gateway(
            GetUserAccount
         );
         CurrentUser = GetUserAccount.Output.ToString();

         ConnectionString = GetConnectionString.Output.ToString();
         iProject = new Data.iProjectDataContext(GetConnectionString.Output.ToString());

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);

         HostNameInfo = (XElement)GetHostInfo.Output;
         gtwymacadrs = HostNameInfo.Attribute("cpu").Value;

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
         //Execute_Query();
         DBankBs.DataSource = iProject.D_BANKs;      
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadDataAsync(Job job)
      {         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallWindow(Job job)
      {
         Xinput = job.Input as XElement;
         if (Xinput != null)
         {
            Pos_Device = iProject.Pos_Devices.FirstOrDefault(p => p.PSID == Convert.ToInt64(Xinput.Attribute("psid").Value));
            if (Xinput.Attribute("subsys") != null)
               subsys = Convert.ToInt32(Xinput.Attribute("subsys").Value);
            else
               subsys = null;            

            if (Xinput.Attribute("rqid") != null)
               rqid = Convert.ToInt64(Xinput.Attribute("rqid").Value);
            else
               rqid = null;

            if (Xinput.Attribute("rqtpcode") != null)
               rqtpcode = Xinput.Attribute("rqtpcode").Value;
            else
               rqtpcode = null;

            if (Xinput.Attribute("router") != null)
               router = Xinput.Attribute("router").Value;
            else
               router = null;

            if (Xinput.Attribute("callback") != null)
               callback = Convert.ToInt32(Xinput.Attribute("callback").Value);
            else
               callback = null;

            if (Xinput.Attribute("amnt") != null)
               Amnt_Txt.EditValue = Xinput.Attribute("amnt").Value;
            else
               Amnt_Txt.EditValue = 1000;
         }
         else
         {
            Pos_Device = null;
            subsys = null;
            rqid = null;
            rqtpcode = null;
            router = null;
            callback = null;
            Amnt_Txt.EditValue = 0;
         }

         FromTranDate_Dt.Value = ToTranDate_Dt.Value = DateTime.Now;

         PayResult_Lb.Appearance.Image = null;
         //Execute_Query();
         RightButns_Click(PosPymt_Butn, null);

         if (Pos_Device != null && Pos_Device.AUTO_COMM == "002")
            PosPayment_Butn_Click(null, null);

         job.Status = StatusType.Successful;
      }
   }
}
