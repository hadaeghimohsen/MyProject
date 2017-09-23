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
   public partial class SettingsAccountGallery : UserControl
   {
      public SettingsAccountGallery()
      {
         InitializeComponent();
      }

      private bool requery = false;

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

      private void Sb_Profile1_Click(object sender, EventArgs e)
      {
         try
         {
            var sb = (SimpleButton)sender;
            var user = UserBs.Current as Data.User;
            
            Image img = sb.BackgroundImage;

            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            //img = Pb_FaceZone.Image;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            user.USER_IMAG = bytes;

            SubmitChange_Butn_Click(null, null);
            Back_Butn_Click(null, null);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {                     
                     new Job(SendType.SelfToUserInterface, FormCaller, 10 /* Execute ActionCallWindow */)
                  }
               )
            );
         }
         catch { }
      }

      private void SelectImage_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         if (ImageFile_Ofd.ShowDialog() == DialogResult.OK)
         {
            var img = Image.FromFile(ImageFile_Ofd.FileName);
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            //img = Pb_FaceZone.Image;
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
      }
   }
}
