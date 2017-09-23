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
   public partial class SettingsAccountChangePassword : UserControl
   {
      public SettingsAccountChangePassword()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, ImageAccount_Pb.Width, ImageAccount_Pb.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         ImageAccount_Pb.Region = rg;
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
            Back_Butn_Click(null, null);
         }
         catch { }
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
         CurrentPassword_Be.Text = NewPassword_Be.Text = ReenterNewPassword_Be.Text = "";

         var user = UserBs.Current as Data.User;
         if (user == null) return;

         if (user.USER_IMAG == null)
         {
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            Image img = global::System.DataGuard.Properties.Resources.IMAGE_1429;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            user.USER_IMAG = bytes;
            ImageAccount_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         }
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            ImageAccount_Pb.Image = Image.FromStream(stream);
         }

         if(CurrentUser.ToLower() != user.USERDB.ToLower())
         {
            CurrentPassword_Be.Text = user.Password;
            CurrentPassword_Be.Visible = CurrentPassword_Lb.Visible = StepOneCurrentPassword_Lb.Visible = false;
         }
         else
         {
            CurrentPassword_Be.Visible = CurrentPassword_Lb.Visible = StepOneCurrentPassword_Lb.Visible = true;
         }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;

            // First Step Check Current Password
            if(user.Password != CurrentPassword_Be.EditValue.ToString())
            {
               CurrentPassword_Be.Focus();
               return;
            }

            // Second Step Check New Password
            if(NewPassword_Be.EditValue.ToString() != ReenterNewPassword_Be.EditValue.ToString())
            {
               ReenterNewPassword_Be.Focus();
               return;
            }

            // Third Step Check Current Password not equal New Password
            if((CurrentPassword_Be.EditValue.ToString() == NewPassword_Be.EditValue.ToString()) ||
               (NewPassword_Be.EditValue.ToString() == "" || ReenterNewPassword_Be.EditValue.ToString() == ""))
            {
               NewPassword_Be.Focus();
               return;
            }

            // Last Step Check Password Policy

            user.Password = NewPassword_Be.EditValue.ToString();
            iProject.SubmitChanges();
            Back_Butn_Click(null, null);
         }
         catch (Exception)
         {

         }
      }
   }
}
