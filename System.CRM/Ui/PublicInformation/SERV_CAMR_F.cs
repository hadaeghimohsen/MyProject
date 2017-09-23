using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace System.CRM.Ui.PublicInformation
{
   public partial class SERV_CAMR_F : UserControl
   {
      public SERV_CAMR_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private Capture capture;
      private CascadeClassifier cascadeClassifier;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }            
         );
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Pb_FaceZone.Image == null) return;

            Image img;
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            img = Pb_FaceZone.Image;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            var CrntRcdc = rcdc;//(RcdcBs.Current as Data.Receive_Document);
            CrntRcdc.Image_Documents.Single().IMAG = Convert.ToBase64String(bytes);
            CrntRcdc.Image_Documents.Single().FILE_NAME = "تصویر گرفته شده از وب کم";
            CrntRcdc.Image_Documents.Single().MDFY_STAT = 1;

            iCRM.DCT_SAVE_P(
               new XElement("Prcess",                  
                  new XElement("Receive_Document",
                     new XAttribute("rcid", rcdc.RCID),
                     new XAttribute("delvdate", rcdc.DELV_DATE.Value.Date),
                     new XAttribute("rcdcstat", rcdc.RCDC_STAT),
                     new XAttribute("permstat", rcdc.PERM_STAT),
                     new XAttribute("strtdate", rcdc.STRT_DATE.Value.Date),
                     new XAttribute("enddate", rcdc.END_DATE.Value.Date),
                     new XAttribute("rcdcdesc", rcdc.RCDC_DESC ?? ""),
                     new XElement("Image_Document",
                        new XAttribute("filename", rcdc.Image_Documents.Single().FILE_NAME ?? ""),
                        rcdc.Image_Documents.Single().IMAG ?? ""
                     )
                  )
               )
            );
            requery = true;
         }
         catch { }
         finally
         {
            if(requery)
            {
               try
               {
                  var crnt = rcdc.Image_Documents.SingleOrDefault();
                  iCRM.SET_PIMG_P(
                     new XElement("Service",
                        new XAttribute("fileno", rcdc.Request_Row.SERV_FILE_NO),
                        new XElement("Image",
                           new XAttribute("rcdcrcid", crnt.RCDC_RCID),
                           new XAttribute("rwno", crnt.RWNO)
                        )
                     )
                  );
                  MessageBox.Show("پروفایل عکس شما با موفقیت تایید شد");

                  LOV_VideoSrc_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(LOV_VideoSrc.Properties.Buttons[2]));
               }
               catch (Exception ex)
               {
                  MessageBox.Show(ex.Message);
               }
            }
         }
      }

      private void UserBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void LOV_VideoSrc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  // Start Capture
                  if (LOV_VideoSrc.ItemIndex == -1) return;
                  if (capture == null)
                     capture = new Emgu.CV.Capture(LOV_VideoSrc.ItemIndex);
                  else
                     capture.Start();
                  Tm_NewFrameProcess.Enabled = true;
                  break;
               case 2:
                  // Stop Capture
                  if (capture != null)
                  {
                     Tm_NewFrameProcess.Enabled = false;
                     capture.Stop();
                     pb_source.Image = null;
                     pb_source.Invalidate();
                     Pb_FaceZone.Image = null;
                     Pb_FaceZone.Invalidate();
                  }
                  break;
               default:
                  break;
            }
         }
         catch { }
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
                  //LB_Result.Text = string.Format("X:{0}, Y:{1}, W:{2}, H:{3}", face.X, face.Y, face.Width, face.Height);
                  var newface = face;
                  newface.X -= 20;
                  newface.Y -= 60;
                  newface.Width += 40;
                  newface.Height += 100;

                  try
                  {
                     if(Ts_ChangeSizeButn.IsOn)
                        Pb_FaceZone.Image = imageFrame.Copy(face).Convert<Rgba, byte>().ToBitmap();
                     else
                        Pb_FaceZone.Image = imageFrame.Copy(newface).Convert<Rgba, byte>().ToBitmap();
                  }
                  catch (Exception ex)
                  {
                     //LB_Result.Text = ex.Message;
                  }


                  imageFrame.Draw(face, new Bgr(Color.Red), 1); //the detected face(s) is highlighted here using a box that is drawn around it/them

               }
            }
         }
      }

      private void TakeImage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Pb_FaceZone.Image == null) return;

            //Image img = Pb_FaceZone.Image;

            //byte[] bytes = null;
            //MemoryStream ms = new MemoryStream();
            //img = Pb_FaceZone.Image;
            //img.Save(ms, ImageFormat.Bmp);
            //bytes = ms.ToArray();

            ////serv.USER_IMAG = bytes;

            SubmitChange_Butn_Click(null, null);
            Back_Butn_Click(null, null);

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost",
            //      new List<Job>
            //      {                     
            //         new Job(SendType.SelfToUserInterface, "SettingsAccount", 10 /* Execute ActionCallWindow */)
            //      }
            //   )
            //);
         }
         catch { }
      }
   }
}
