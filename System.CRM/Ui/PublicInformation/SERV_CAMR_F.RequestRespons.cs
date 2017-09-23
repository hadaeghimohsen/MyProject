using DirectShowLib;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.CRM.Ui.PublicInformation
{
   partial class SERV_CAMR_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      private string CurrentUser;
      private Data.Receive_Document rcdc;


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
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iCRM</Database><Dbms>SqlServer</Dbms>" };

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
         iCRM = new Data.iCRMDataContext(GetConnectionString.Output.ToString());

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "CRM:" + GetType().Name, this }  },
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
         /* Load Video Capture */
         //-> Create a List to store for ComboCameras
         List<KeyValuePair<int, string>> ListCamerasData = new List<KeyValuePair<int, string>>();

         //-> Find systems cameras with DirectShow.Net dll 
         DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

         int _DeviceIndex = 0;
         foreach (DirectShowLib.DsDevice _Camera in _SystemCamereas)
         {
            ListCamerasData.Add(new KeyValuePair<int, string>(_DeviceIndex, _Camera.Name));
            _DeviceIndex++;
         }
                  
         //-> bind the combobox
         LOV_VideoSrc.Properties.DataSource = new BindingSource(ListCamerasData, null);
         LOV_VideoSrc.Properties.DisplayMember = "Value";
         LOV_VideoSrc.Properties.ValueMember = "Key";

         /* Emgu.CV */
         cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");
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
      private void Actn_CalF_P(Job job)
      {
         rcdc = job.Input as Data.Receive_Document;
         //Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
