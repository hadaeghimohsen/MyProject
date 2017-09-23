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
   public partial class SettingsAccountSetEmailAddress : UserControl
   {
      public SettingsAccountSetEmailAddress()
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
         requery = false;
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
         EmailPassword_Txt.Text = EmailPassword_Txt.Text = "";

         var user = UserBs.Current as Data.User;
         if (user == null) return;

         // Mail Server Stat
         user.MAIL_SRVR_STAT = user.MAIL_SRVR_STAT == null ? "001" : user.MAIL_SRVR_STAT;
         switch (user.MAIL_SRVR_STAT)
         {
            case "001":
               MailSrvrStat_Tg.IsOn = false;
               break;
            case "002":
               MailSrvrStat_Tg.IsOn = true;
               break;
            default:
               break;
         }

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

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;


            if (MailServer_Lov.EditValue.ToString() == "")
            {
               MailServer_Lov.Focus();
               return;
            }
            
            if(EmailAddress_Txt.EditValue.ToString() == "")
            {
               EmailAddress_Txt.Focus();
               return;
            }

            
            if (EmailPassword_Txt.EditValue.ToString() == "")
            {
               EmailPassword_Txt.Focus();
               return;
            }

            if(user.MAIL_SRVR_STAT == "002")
            {
               if (
                  ProfileName_Txt.Text == "" ||
                  iProject.CHK_MALS_U(
                     new XElement("MailServer",
                        new XAttribute("type", "001"),
                        new XAttribute("username", CurrentUser),
                        new XElement("Profile",
                           new XAttribute("name", ProfileName_Txt.Text ?? "")
                        )
                     )
                  ) != 0
               )
               {
                  ProfileName_Txt.Focus();
                  return;
               }

               if (AccountName_Txt.Text == "" ||
                     iProject.CHK_MALS_U(
                        new XElement("MailServer",
                           new XAttribute("type", "002"),
                           new XAttribute("username", CurrentUser),
                           new XElement("Account",
                              new XAttribute("name", AccountName_Txt.Text ?? "")
                           )
                        )
                     ) != 0
                  )
               {
                  AccountName_Txt.Focus();
                  return;
               }               
            }

            //iProject.SubmitChanges();
            iProject.ConfigurationUserMailServer(
               new XElement("MailServer",
                  new XAttribute("mailsrvr", user.MAIL_SRVR),
                  new XAttribute("emaladdr", user.EMAL_ADRS),
                  new XAttribute("pswd", user.EMAL_PASS),
                  new XAttribute("userid", user.ID),
                  new XAttribute("username", user.USERDB),
                  new XElement("SqlServerMailServer",
                     new XAttribute("stat", user.MAIL_SRVR_STAT),
                     new XAttribute("type", "001"),
                     new XAttribute("profname", user.MAIL_SRVR_PROF),
                     new XAttribute("acntname", user.MAIL_SRVR_ACNT)
                  )
               )
            );
            Back_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void TestEmailAddress_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MailServer_Lov.EditValue.ToString() == "")
            {
               MailServer_Lov.Focus();
               return;
            }

            if (EmailAddress_Txt.EditValue.ToString() == "")
            {
               EmailAddress_Txt.Focus();
               return;
            }


            if (EmailPassword_Txt.EditValue.ToString() == "")
            {
               EmailPassword_Txt.Focus();
               return;
            }

            var user = UserBs.Current as Data.User;

            var fromAddress = new MailAddress(EmailAddress_Txt.Text, user.TitleFa);
            var toAddress = new MailAddress(EmailAddress_Txt.Text, user.TitleFa);

            var mailserver = iProject.Mail_Servers.FirstOrDefault(ms => ms.EMAL_SRVR == user.MAIL_SRVR);

            var smtp = new SmtpClient
            {
               Host = mailserver.SMTP_ADRS, //"smtp.gmail.com",
               Port = (int)mailserver.SMTP_PORT, //587,
               EnableSsl = mailserver.SMTP_SSL_MODE == "002" ? true : false,//true,
               DeliveryMethod = SmtpDeliveryMethod.Network,
               UseDefaultCredentials = true,
               Credentials = new NetworkCredential(fromAddress.Address, EmailPassword_Txt.Text)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
               Subject = "تست ارتباط نرم افزار انار با ایمیل شما کاربر عزیز",
               Body = "با سلام، این پیام یک پیام تستی برای بررسی صحت ارتباط نرم افزار انار با سیستم ایمیل شما می باشد",
               IsBodyHtml = false
            })
            {
               smtp.Send(message);
            }
            MessageBox.Show("پیام با موفقیت از درگاه اینترنت عبور کرد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CheckProfileName_Butn_Click(object sender, EventArgs e)
      {
         if(
            iProject.CHK_MALS_U(
               new XElement("MailServer",
                  new XAttribute("type", "001"),
                  new XAttribute("username", CurrentUser),
                  new XElement("Profile",
                     new XAttribute("name", ProfileName_Txt.Text ?? "")
                  )
               )
            ) == 0
         )
            ProfileName_Txt.BackColor = Color.WhiteSmoke;
         else
            ProfileName_Txt.BackColor = Color.Pink;
      }

      private void CheckAcountName_Butn_Click(object sender, EventArgs e)
      {
         if (
            iProject.CHK_MALS_U(
               new XElement("MailServer",
                  new XAttribute("type", "002"),
                  new XAttribute("username", CurrentUser),
                  new XElement("Account",
                     new XAttribute("name", AccountName_Txt.Text ?? "")
                  )
               )
            ) == 0
         )
            AccountName_Txt.BackColor = Color.WhiteSmoke;
         else
            AccountName_Txt.BackColor = Color.Pink;
      }

      private void SugestProfileName_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if(user == null)return;

         user.MAIL_SRVR_PROF = string.Format("{0}_{1}_profile", EmailAddress_Txt.Text.Substring(0, EmailAddress_Txt.Text.IndexOf('@')), iProject.Mail_Servers.FirstOrDefault(ms => ms.EMAL_SRVR == user.MAIL_SRVR).EMAL_SRVR_NAME);
      }

      private void SugestAcountName_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         user.MAIL_SRVR_ACNT = string.Format("{0}_{1}_account", EmailAddress_Txt.Text.Substring(0, EmailAddress_Txt.Text.IndexOf('@')), iProject.Mail_Servers.FirstOrDefault(ms => ms.EMAL_SRVR == user.MAIL_SRVR).EMAL_SRVR_NAME);
      }

      private void MailSrvrStat_Tg_Toggled(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;

            user.MAIL_SRVR_STAT = MailSrvrStat_Tg.IsOn ? "002" : "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Delete_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;

            //iProject.SubmitChanges();
            iProject.ConfigurationUserMailServer(
               new XElement("MailServer",
                  new XAttribute("mailsrvr", user.MAIL_SRVR),
                  new XAttribute("emaladdr", user.EMAL_ADRS),
                  new XAttribute("pswd", user.EMAL_PASS),
                  new XAttribute("userid", user.ID),
                  new XAttribute("username", user.USERDB),
                  new XElement("SqlServerMailServer",
                     new XAttribute("stat", "001"),
                     new XAttribute("type", "002"),
                     new XAttribute("profname", user.MAIL_SRVR_PROF),
                     new XAttribute("acntname", user.MAIL_SRVR_ACNT)
                  )
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }
   }
}
