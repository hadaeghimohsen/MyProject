using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;

namespace System.Scsc.Ui.Document
{
   partial class CMN_DCMT_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private Data.Request_Row Rqro;
      private bool requery = default(bool);
      private string RegnLang = "054";

      /// <summary>
      ///  AForge.Net
      /// </summary>
      private FilterInfoCollection filterInfoCollection;
      //private VideoCaptureDevice videoCaptureDevice;

      /// <summary>
      /// Emgu.CV
      /// </summary>
      private Capture capture;
      private CascadeClassifier cascadeClassifier;
      
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
         else if (keyData == Keys.Escape)
         {
            Tb_StartStopVideo.PickChecked = false;
            if (capture != null)
            {
               capture.Stop();
               capture.Dispose();
               capture = null;
            }

            job.Next =
               new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //Tsb_SaveItem_Click(null, null);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         #region Set Localization
         var regnlang = iScsc.V_User_Localization_Forms.Where(rl => rl.FORM_NAME == GetType().Name);
         if (regnlang.Count() > 0 && regnlang.First().REGN_LANG != RegnLang)
         {
            RegnLang = regnlang.First().REGN_LANG;
            // Ready To Change Text Title
            foreach (var control in regnlang)
            {
               switch (control.CNTL_NAME.ToLower())
               {
                  case "rcdcstat_lb":
                     RcdcStat_Lb.Text = control.LABL_TEXT;
                     //RcdcStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RcdcStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_001":
                     tp_001.Text = control.LABL_TEXT;
                     //tp_001.Text = control.LABL_TEXT; // ToolTip
                     //tp_001.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_002":
                     tp_002.Text = control.LABL_TEXT;
                     //tp_002.Text = control.LABL_TEXT; // ToolTip
                     //tp_002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_003":
                     tp_003.Text = control.LABL_TEXT;
                     //tp_003.Text = control.LABL_TEXT; // ToolTip
                     //tp_003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dcmt_gb":
                     Dcmt_Gb.Text = control.LABL_TEXT;
                     //Dcmt_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Dcmt_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strtdate_lb":
                     StrtDate_Lb.Text = control.LABL_TEXT;
                     //StrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //StrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_lb":
                     EndDate_Lb.Text = control.LABL_TEXT;
                     //EndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "delvdate_lb":
                     DelvDate_Lb.Text = control.LABL_TEXT;
                     //DelvDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DelvDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rcdcdesc_lb":
                     RcdcDesc_Lb.Text = control.LABL_TEXT;
                     //RcdcDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RcdcDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rwno_lb":
                     Rwno_Lb.Text = control.LABL_TEXT;
                     //Rwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "filename_lb":
                     FileName_Lb.Text = control.LABL_TEXT;
                     //FileName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dimsn_lb":
                     Dimsn_Lb.Text = control.LABL_TEXT;
                     //Dimsn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Dimsn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bt_selectfile":
                     Bt_SelectFile.Text = control.LABL_TEXT;
                     //Bt_SelectFile.Text = control.LABL_TEXT; // ToolTip
                     //Bt_SelectFile.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "permstat_lb":
                     PermStat_Lb.Text = control.LABL_TEXT;
                     //PermStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PermStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode_clm":
                     RqtpCode_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpdesc_clm":
                     RqtpDesc_Clm.Caption = control.LABL_TEXT;
                     //RqtpDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_clm":
                     RqttCode_Clm.Caption = control.LABL_TEXT;
                     //RqttCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqttCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttdesc_clm":
                     RqttDesc_Clm.Caption = control.LABL_TEXT;
                     //RqttDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqttDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "dcmtdesc_clm":
                     DcmtDesc_Clm.Caption = control.LABL_TEXT;
                     //DcmtDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DcmtDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "needtype_clm":
                     NeedType_Clm.Caption = control.LABL_TEXT;
                     //NeedType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //NeedType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "origtype_clm":
                     OrigType_Clm.Caption = control.LABL_TEXT;
                     //OrigType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OrigType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstneed_clm":
                     FrstNeed_Clm.Caption = control.LABL_TEXT;
                     //FrstNeed_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FrstNeed_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "image_gb":
                     Image_Gb.Text = control.LABL_TEXT;
                     //Image_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Image_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cb_showscroll":
                     CB_ShowScroll.Text = control.LABL_TEXT;
                     //CB_ShowScroll.Text = control.LABL_TEXT; // ToolTip
                     //CB_ShowScroll.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cb_allowmousedrag":
                     CB_AllowMouseDrag.Text = control.LABL_TEXT;
                     //CB_AllowMouseDrag.Text = control.LABL_TEXT; // ToolTip
                     //CB_AllowMouseDrag.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "imagealign_lb":
                     ImageAlign_Lb.Text = control.LABL_TEXT;
                     //ImageAlign_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ImageAlign_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "imagequlity_lb":
                     ImageQulity_Lb.Text = control.LABL_TEXT;
                     //ImageQulity_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ImageQulity_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bt_remvimage":
                     Bt_RemvImage.Text = control.LABL_TEXT;
                     //Bt_RemvImage.Text = control.LABL_TEXT; // ToolTip
                     //Bt_RemvImage.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "selectcamera_lb":
                     SelectCamera_Lb.Text = control.LABL_TEXT;
                     //SelectCamera_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SelectCamera_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
               }
            }
         }
         #endregion

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
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
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
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (job.Input == null) return;
         Rqro = job.Input as Data.Request_Row;
         try
         {
            iScsc.CMN_DCMT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqro.Request.RQID),
                     new XAttribute("rqtpcode", Rqro.Request.RQTP_CODE),
                     new XAttribute("rqttcode", Rqro.Request.RQTT_CODE),
                     new XElement("Request_Row",
                        new XAttribute("rwno", Rqro.RWNO)
                     )
                  )
               )
            );

            receive_DocumentBindingSource.DataSource = iScsc.Receive_Documents.Where(rd => rd.Request_Row == Rqro);
            dDCNDBindingSource.DataSource = iScsc.D_DCNDs;
            dDCTPBindingSource.DataSource = iScsc.D_DCTPs;
            dYSNOBindingSource.DataSource = iScsc.D_YSNOs;
            dDCMTBindingSource.DataSource = iScsc.D_DCMTs;
            dPRSTBindingSource.DataSource = iScsc.D_PRSTs;

            /* Load Video Capture */
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            LOV_VideoSrc.Properties.DataSource = filterInfoCollection.OfType<FilterInfo>().ToList();
            LOV_VideoSrc.Properties.DisplayMember = "MonikerString";
            LOV_VideoSrc.Properties.ValueMember = "Name";

            /* Emgu.CV */
            cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");
            job.Status = StatusType.Successful;
         }
         catch (Exception Ex)
         {
            MessageBox.Show(Ex.Message);
            //_DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "CMN_DCMT_F", 04 /* Execute UnPaint */, SendType.SelfToUserInterface));
            job.Status = StatusType.Failed;
         }         
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xdata = job.Input as XElement;
         var rcid = Convert.ToInt64(xdata.Element("Document").Attribute("rcid").Value);
         for (int i = 0; i < receive_DocumentBindingSource.Count; i++)
         {
            if(rcid == (receive_DocumentBindingSource.List[i] as Data.Receive_Document).RCID)
            {
               receive_DocumentBindingSource.Position = i;
               break;
            }
         }
         switch (xdata.Attribute("type").Value)
         {
            case "001": // فعال سازی عکس برای مشتری
               TC_Dcmt.SelectedTab = tp_003;
               if(filterInfoCollection.Count > 0)
               {
                  LOV_VideoSrc.ItemIndex = 0;
                  Tb_StartStopVideo.PickChecked = true;
               }
               break;
            default:
               break;
         }
         job.Status = StatusType.Successful;
      }
   }
}
