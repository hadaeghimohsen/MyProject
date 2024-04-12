using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.JobRouting.Jobs;
using Emgu.CV.Structure;

namespace System.Scsc.Ui.Document
{
   public partial class CMN_DCMT_F : UserControl
   {
      public CMN_DCMT_F()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, pb_destination.Width, pb_destination.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         pb_destination.Region = rg;
      }

      private void Bt_SelectFile_Click(object sender, EventArgs e)
      {
         if (DG_SelectImage.ShowDialog() != DialogResult.OK) return;

         //FILE_NAME_TextBox.Text = DG_SelectImage.FileName;
         Image img = Image.FromFile(DG_SelectImage.FileName);

         switch (CB_ImageSize.SelectedIndex)
         {
            case 0:
               img = new Bitmap(img, new Size(169, 224));
               break;
            case 1:
               img = new Bitmap(img, new Size(200, 300));
               break;
            case 2:
               img = new Bitmap(img, new Size(300, 421));
               break;
            case 3:
               img = new Bitmap(img, new Size(597, 842));
               break;
            case 4:
               img = new Bitmap(img, new Size(842, 597));
               break;
            case 5: break;
            default:
               MessageBox.Show("ابعاد عکس مشخص نشده");
               return;
         }
         PE_ImageShow.Image = img;

         byte[] bytes = null;
         MemoryStream ms = new MemoryStream();
         img = PE_ImageShow.Image;
         img.Save(ms, ImageFormat.Bmp);
         bytes = ms.ToArray();

         var CrntRcdc = (RcvdBs.Current as Data.Receive_Document);
         CrntRcdc.Image_Documents.Single().IMAG = Convert.ToBase64String(bytes);
         CrntRcdc.Image_Documents.Single().FILE_NAME = FILE_NAME_TextBox.Text;
         CrntRcdc.Image_Documents.Single().MDFY_STAT = 1;
      }

      private void CB_ShowScroll_CheckedChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.ShowScrollBars = !PE_ImageShow.Properties.ShowScrollBars;
      }

      private void CB_AllowMouseDrag_CheckedChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
         PE_ImageShow.Properties.AllowScrollViaMouseDrag = !PE_ImageShow.Properties.AllowScrollViaMouseDrag;
      }

      private void CB_Alignment_SelectedIndexChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.PictureAlignment = (ContentAlignment)(CB_Alignment.SelectedIndex + 1);
      }

      private void UD_Interpolation_ValueChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.PictureInterpolationMode = (Drawing.Drawing2D.InterpolationMode)(UD_Interpolation.Value);
      }

      private void ZC_ZoomImage_ValueChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.ZoomPercent = ZC_ZoomImage.Value;
      }

      private void TC_Dcmt_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (TC_Dcmt.SelectedIndex == 1)
         {
            try
            {
               var CrntRcdc = (RcvdBs.Current as Data.Receive_Document);
               byte[] bytes = null;
               bytes = Convert.FromBase64String(CrntRcdc.Image_Documents.Single().IMAG);
               MemoryStream ms = new MemoryStream(bytes);
               Image img = new Bitmap(ms);
               PE_ImageShow.Image = img;
            }
            catch { PE_ImageShow.Image = null; }
         }

         if (RcvdBs.List.OfType<Data.Receive_Document>().Any(rd => rd.Request_Document != null && rd.Request_Document.DCMT_DSID == 13980505495708))
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_Calf_P */){
                        Input = 
                           new XElement("ZktFingerPrint",
                              new XAttribute("type", "zktfngrprnt"),
                              new XAttribute("fngractn", (TC_Dcmt.SelectedIndex == 3 ? "enroll" : "attendance"))
                           )
                     }
                  }
               )
            );
         }
         else if (TC_Dcmt.SelectedTab == tp_004)
         {
            MessageBox.Show(this, "در این قسمت مدرک اثر انگشت تعریف نشده");
         }
      }

      private void Bt_RemvImage_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از پاک کردن تصویر مدرک مطمئن هستید؟", "حذف تصویر مدرک", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         try
         {
            var CrntRcdc = (RcvdBs.Current as Data.Receive_Document);
            iScsc.DEL_DIMG_P(
               new XElement("Process",
                  new XElement("Image_Document",
                     new XAttribute("rcid", CrntRcdc.Image_Documents.Single().RCDC_RCID),
                     new XAttribute("rwno", CrntRcdc.Image_Documents.Single().RWNO)
                  )
               )
            );
            PE_ImageShow.Image = null;
            requery = true;
         }
         catch (Exception Ex)
         {
            MessageBox.Show(Ex.Message);
         }
         finally
         {
            if (requery)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               int recdoc = RcvdBs.Position;
               RcvdBs.DataSource = iScsc.Receive_Documents.Where(rc => rc.Request_Row == Rqro);
               RcvdBs.Position = recdoc;
               requery = false;
            }
         }
      }

      private void receive_DocumentBindingNavigatorSaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            RcvdBs.EndEdit();
            iScsc.CommandTimeout = int.MaxValue;

            iScsc.DCT_SAVE_P(
               new XElement("Prcess",
                  RcvdBs.List.OfType<Data.Receive_Document>()
                  .ToList()
                  .Select(rc =>
                     new XElement("Receive_Document",
                        new XAttribute("rcid", rc.RCID),
                        new XAttribute("delvdate", rc.DELV_DATE.Value.Date),
                        new XAttribute("rcdcstat", rc.RCDC_STAT),
                        new XAttribute("permstat", rc.PERM_STAT),
                        new XAttribute("strtdate", rc.STRT_DATE.Value.Date),
                        new XAttribute("enddate", rc.END_DATE.Value.Date),
                        new XAttribute("rcdcdesc", rc.RCDC_DESC ?? "")
                     )
                  )
               )
            );

            if (RcvdBs.List.OfType<Data.Receive_Document>()
                  .Where(rd => rd.Image_Documents.Single().MDFY_STAT == 1).Count() >= 1)
               iScsc.DCT_SAVE_P(
                  new XElement("Prcess",
                     RcvdBs.List.OfType<Data.Receive_Document>()
                     .Where(rd => rd.Image_Documents.Single().MDFY_STAT == 1)
                     .ToList()
                     .Select(rc =>
                        new XElement("Receive_Document",
                           new XAttribute("rcid", rc.RCID),
                           new XAttribute("delvdate", rc.DELV_DATE.Value.Date),
                           new XAttribute("rcdcstat", rc.RCDC_STAT),
                           new XAttribute("permstat", rc.PERM_STAT),
                           new XAttribute("strtdate", rc.STRT_DATE.Value.Date),
                           new XAttribute("enddate", rc.END_DATE.Value.Date),
                           new XAttribute("rcdcdesc", rc.RCDC_DESC ?? ""),
                           new XElement("Image_Document",
                              new XAttribute("filename", rc.Image_Documents.Single().FILE_NAME ?? ""),
                              rc.Image_Documents.Single().IMAG ?? ""
                           )
                        )
                     )
                  )
               );
            requery = true;
         }
         catch (Exception Ex)
         {
            MessageBox.Show(Ex.Message);
         }
         finally
         {
            if (requery)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               int recdoc = RcvdBs.Position;
               RcvdBs.DataSource = iScsc.Receive_Documents.Where(rc => rc.Request_Row == Rqro);
               RcvdBs.Position = recdoc;
               requery = false;
            }
         }
      }

      private void Btn_SetProfileImage_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = ImdcBs1.Current as Data.Image_Document;
            iScsc.SET_PIMG_P(
               new XElement("Fighter",
                  new XAttribute("fileno", Rqro.FIGH_FILE_NO),
                  new XElement("Image",
                     new XAttribute("rcdcrcid", crnt.RCDC_RCID),
                     new XAttribute("rwno", crnt.RWNO)
                  )
               )
            );
            //MessageBox.Show("پروفایل عکس شما با موفقیت تایید شد");

            if (Tb_StartStopVideo.PickChecked)
               Tb_StartStopVideo.PickChecked = false;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Tb_StartStopVideo_PickCheckedChange(object sender)
      {
         try
         {
            #region Old Version
            //if (Tb_StartStopVideo.PickChecked)
            //{
            //   // دکمه وضعیت فعال
            //   if (LOV_VideoSrc.ItemIndex == -1) return;
            //   videoCaptureDevice = new AForge.Video.DirectShow.VideoCaptureDevice(filterInfoCollection[LOV_VideoSrc.ItemIndex].MonikerString);
            //   videoCaptureDevice.NewFrame += (object s, AForge.Video.NewFrameEventArgs e) => { pb_source.Image = (Bitmap)e.Frame.Clone(); };
            //   videoCaptureDevice.Start();
            //}
            //else
            //{
            //   // دکمه در وضعیت غیرفعال
            //   if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
            //   {
            //      videoCaptureDevice.Stop();
            //      pb_source.Image = null;
            //      pb_source.Invalidate();
            //   }
            //}
            #endregion
            #region New Version
            if (Tb_StartStopVideo.PickChecked)
            {
               // دکمه وضعیت فعال
               if (LOV_VideoSrc.ItemIndex == -1) return;
               if (capture == null)
                  capture = new Emgu.CV.Capture(LOV_VideoSrc.ItemIndex);
               else
                  capture.Start();
               Tm_NewFrameProcess.Enabled = true;
            }
            else
            {
               // دکمه در وضعیت غیرفعال
               if (capture != null)
               {
                  Tm_NewFrameProcess.Enabled = false;
                  capture.Stop();
                  pb_source.Image = null;
                  pb_source.Invalidate();
                  Pb_FaceZone.Image = null;
                  Pb_Face3x4Zone.Image = null;
                  Pb_FaceZone.Invalidate();
                  Pb_Face3x4Zone.Invalidate();
               }
            }
            #endregion
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_TakePicture_Click(object sender, EventArgs e)
      {
         if (pb_source.Image != null)
         {
            if (Npb_FaceZone.PickChecked)
            {
               pb_destination.Image = (Bitmap)Pb_FaceZone.Image.Clone();
            }
            else if (Npb_Face3x4Zone.PickChecked)
            {
               pb_destination.Image = (Bitmap)Pb_Face3x4Zone.Image.Clone();
            }
         }
      }

      private void mb_back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", GetType().Name, 00 /* Execute ProcessCmdKey Un_Paint */, SendType.SelfToUserInterface) { Input = Keys.Escape });
      }

      private void Btn_AcceptPicture_Click(object sender, EventArgs e)
      {
         if (pb_destination.Image == null) return;

         Image img = pb_destination.Image;

         //img = new Bitmap(img, new Size(169, 224));
         PE_ImageShow.Image = img;

         byte[] bytes = null;
         MemoryStream ms = new MemoryStream();
         img = PE_ImageShow.Image;
         img.Save(ms, ImageFormat.Bmp);
         bytes = ms.ToArray();

         //var CrntRcdc = (RcvdBs.Current as Data.Receive_Document);
         var CrntRcdc = RcvdBs.List.OfType<Data.Receive_Document>().FirstOrDefault(i => i.Request_Document.DCMT_DSID == 13930903120048833);
         if (CrntRcdc == null) { MessageBox.Show("رکورد عکس 3*4 تعریف نشده"); return; }

         CrntRcdc.Image_Documents.Single().IMAG = Convert.ToBase64String(bytes);
         CrntRcdc.Image_Documents.Single().FILE_NAME = FILE_NAME_TextBox.Text;
         CrntRcdc.Image_Documents.Single().MDFY_STAT = 1;
         TC_Dcmt.SelectedTab = tp_002;

         int _rcvd = RcvdBs.IndexOf(CrntRcdc);
         receive_DocumentBindingNavigatorSaveItem_Click(null, null);
         RcvdBs.Position = _rcvd;

         Btn_SetProfileImage_Click(null, null);
         mb_back_Click(null, null);
      }

      private void Tm_NewFrameProcess_Tick(object sender, EventArgs e)
      {
         using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
         {
            if (imageFrame != null)
            {
               pb_source.Image = imageFrame.ToBitmap();

               var grayframe = imageFrame.Convert<Gray, byte>();
               var faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
               foreach (var face in faces)
               {
                  // Additional Data
                  LB_Result.Text = string.Format("X:{0}, Y:{1}, W:{2}, H:{3}", face.X, face.Y, face.Width, face.Height);
                  var newface = face;
                  newface.X -= 20;
                  newface.Y -= 60;
                  newface.Width += 40;
                  newface.Height += 100;

                  try
                  {
                     Pb_FaceZone.Image = imageFrame.Copy(face).Convert<Rgba, byte>().ToBitmap();
                     Pb_Face3x4Zone.Image = imageFrame.Copy(newface).Convert<Rgba, byte>().ToBitmap();
                  }
                  catch (Exception ex)
                  {
                     LB_Result.Text = ex.Message;
                  }


                  imageFrame.Draw(face, new Bgr(Color.Red), 1); //the detected face(s) is highlighted here using a box that is drawn around it/them

               }
            }
         }
      }

      private void receive_DocumentBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         TC_Dcmt_SelectedIndexChanged(null, null);
      }

      private void CB_ImageSize1_SelectedIndexChanged(object sender, EventArgs e)
      {
         CB_ImageSize.SelectedIndex = CB_ImageSize1.SelectedIndex;
      }
   }
}
