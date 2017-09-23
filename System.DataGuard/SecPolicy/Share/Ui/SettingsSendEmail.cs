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
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsSendEmail : UserControl
   {
      public SettingsSendEmail()
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
         UserBs.DataSource = iProject.Users.Where(u => u.USERDB.ToUpper() == CurrentUser.ToUpper()).ToList();
         if(emailtype == "feedback")
         {
            ToEmail_Lst.Items.Add(iProject.Sub_Systems.FirstOrDefault(s => s.SUB_SYS == subsys).SUPR_EMAL);
            ToEmail_Txt.Properties.ReadOnly = true;
            DeleteEmail_Butn.Enabled = false;
         }
         else if(emailtype == "normal")
         {
            ToEmail_Txt.Properties.ReadOnly = false;
            DeleteEmail_Butn.Enabled = true;
         }
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
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
      }

      private void DeleteEmail_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ToEmail_Lst.Items.Count == 0) return;

            ToEmail_Lst.Items.RemoveAt(ToEmail_Lst.SelectedIndex);
         }
         catch { }
      }

      private void DeleteFileAttach_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FileAttach_Lst.Items.Count == 0) return;

            FileAttach_Lst.Items.RemoveAt(FileAttach_Lst.SelectedIndex);
         }
         catch { }
      }

      private void ToEmail_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (ToEmail_Txt.Text.Trim() == "") return;

            if (ToEmail_Lst.Items.IndexOf(ToEmail_Txt.Text) >= 0) { ToEmail_Txt.Text = ""; return; }

            ToEmail_Lst.Items.Add(ToEmail_Txt.Text);
            ToEmail_Txt.Text = "";
         }
         catch { }
      }

      private void FileAttach_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (FileAttach_Txt.Text.Trim() == "") return;

                  if (FileAttach_Lst.Items.IndexOf(FileAttach_Txt.Text) >= 0) { FileAttach_Txt.Text = ""; return; }

                  FileAttach_Lst.Items.Add(FileAttach_Txt.Text);
                  break;
               case 0:
                  if (FileAttach_Ofd.ShowDialog() != DialogResult.OK) return;
                  foreach (var file in FileAttach_Ofd.FileNames)
                  {
                     FileAttach_Txt.Text = file;

                     if (FileAttach_Txt.Text.Trim() == "") return;

                     if (FileAttach_Lst.Items.IndexOf(FileAttach_Txt.Text) >= 0) { FileAttach_Txt.Text = ""; return; }

                     FileAttach_Lst.Items.Add(FileAttach_Txt.Text);                     
                  }
                  FileAttach_Txt.Text = "";
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void SendEmail_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Subject_Txt.Text.Trim() == "") { Subject_Txt.Focus(); return; }
            if (Comment_Txt.Text.Trim() == "") { Comment_Txt.Focus(); return; }
            if (ToEmail_Lst.Items.Count == 0) { ToEmail_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsmailserver"},
                     new Job(SendType.SelfToUserInterface, "SettingsMailServer", 02 /* Execute Set */),                  
                     //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                     new Job(SendType.SelfToUserInterface, "SettingsMailServer", 07 /* Execute LoadData */),
                     new Job(SendType.SelfToUserInterface, "SettingsMailServer", 100 /* Execute SendMail */)
                     {
                        Input = 
                           new XElement("SendEmail",
                              new XAttribute("username", (UserBs.Current as Data.User).USERDB),
                              new XElement("Comment",
                                 new XAttribute("subject", Subject_Txt.Text),
                                 Comment_Txt.Text
                              ),
                              new XElement("ToEmails", 
                                 ToEmail_Lst.Items.Cast<string>()
                                 .Select(i => 
                                    new XElement("To", 
                                       new XAttribute("email", i)
                                    )
                                 )
                              ),
                              new XElement("Files",
                                 FileAttach_Lst.Items.Cast<string>()
                                 .Select(i => 
                                    new XElement("File", 
                                       new XAttribute("path", i)
                                    )
                                 )
                              )
                           )
                     }
                  }
               )
            );
            
            Back_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
