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

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsAccountCamera : UserControl
   {
      public SettingsAccountCamera()
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

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         UserBs.DataSource = iProject.Users.Where(u => u == User).ToList(); 
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            UserBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch { }
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
            var user = UserBs.Current as Data.User;
            if (Pb_FaceZone.Image == null) return;

            Image img = Pb_FaceZone.Image;

            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            img = Pb_FaceZone.Image;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            user.USER_IMAG = bytes;

            SubmitChange_Butn_Click(null, null);
            Back_Butn_Click(null, null);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {                     
                     new Job(SendType.SelfToUserInterface, "SettingsAccount", 10 /* Execute ActionCallWindow */)
                  }
               )
            );
         }
         catch { }
      }
   }
}
