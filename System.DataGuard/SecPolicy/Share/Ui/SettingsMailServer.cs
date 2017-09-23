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
using System.IO;
using System.Drawing.Imaging;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsMailServer : UserControl
   {
      public SettingsMailServer()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {         
         iProject = new Data.iProjectDataContext(ConnectionString);
         if (Tb_Master.SelectedTab == tp_001)
         {
            MailServerBs.DataSource = iProject.Mail_Servers.Where(ms => ms.EMAL_SRVR == "001");
         }
         else if (Tb_Master.SelectedTab == tp_002)
         {
            MailServerBs.DataSource = iProject.Mail_Servers.Where(ms => ms.EMAL_SRVR == "002");
         }
      }

      private void MailServerBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            MailServerBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch (Exception)
         {

         }         
      }

      private void TakeImage_Butn_Click(object sender, EventArgs e)
      {
         var user = MailServerBs.Current as Data.User;
         if (user == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 21 /* Execute DoWork4SettingsAccountCamera */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountCamera", 10 /* Execute ActionCallWindow */){Input = user}
               }
            )
         );
      }

      private void MailServerBs_CurrentChanged(object sender, EventArgs e)
      {
         var mailserver = MailServerBs.Current as Data.Mail_Server;
         if (mailserver == null) return;

         // Set Imap Ssl
         mailserver.IMAP_SSL_MODE = mailserver.IMAP_SSL_MODE == null ? "002" : mailserver.IMAP_SSL_MODE;
         switch (mailserver.IMAP_SSL_MODE)
         {
            case "001":
               if (mailserver.EMAL_SRVR == "001")
                  Ts_GoogleImapSsl.IsOn = false;
               else if (mailserver.EMAL_SRVR == "002")
                  Ts_YahooImapSsl.IsOn = false;
               break;
            case "002":
               if (mailserver.EMAL_SRVR == "001")
                  Ts_GoogleImapSsl.IsOn = true;
               else if (mailserver.EMAL_SRVR == "002")
                  Ts_YahooImapSsl.IsOn = true;
               break;
         }

         // Set Privacy Lock Screen Status
         mailserver.SMTP_SSL_MODE = mailserver.SMTP_SSL_MODE == null ? "002" : mailserver.SMTP_SSL_MODE;
         switch (mailserver.SMTP_SSL_MODE)
         {
            case "001":
               if (mailserver.EMAL_SRVR == "001")
                  Ts_GoogleSmtpSsl.IsOn = false;
               else if (mailserver.EMAL_SRVR == "002")
                  Ts_YahooSmtpSsl.IsOn = false;
               break;
            case "002":
               if (mailserver.EMAL_SRVR == "001")
                  Ts_GoogleSmtpSsl.IsOn = true;
               else if (mailserver.EMAL_SRVR == "002")
                  Ts_YahooSmtpSsl.IsOn = true;
               break;
         }
      }

      private void Ts_GoogleImapSsl_Toggled(object sender, EventArgs e)
      {
         var mailserver = MailServerBs.Current as Data.Mail_Server;
         if (mailserver == null) return;

         mailserver.IMAP_SSL_MODE = Ts_GoogleImapSsl.IsOn ? "002" : "001";
      }

      private void Ts_GoogleSmtpSsl_Toggled(object sender, EventArgs e)
      {
         var mailserver = MailServerBs.Current as Data.Mail_Server;
         if (mailserver == null) return;

         mailserver.SMTP_SSL_MODE = Ts_GoogleSmtpSsl.IsOn ? "002" : "001";
      }

      private void Ts_YahooImapSsl_Toggled(object sender, EventArgs e)
      {
         var mailserver = MailServerBs.Current as Data.Mail_Server;
         if (mailserver == null) return;

         mailserver.IMAP_SSL_MODE = Ts_YahooImapSsl.IsOn ? "002" : "001";
      }

      private void Ts_YahooSmtpSsl_Toggled(object sender, EventArgs e)
      {
         var mailserver = MailServerBs.Current as Data.Mail_Server;
         if (mailserver == null) return;

         mailserver.SMTP_SSL_MODE = Ts_YahooSmtpSsl.IsOn ? "002" : "001";
      }
   }
}
