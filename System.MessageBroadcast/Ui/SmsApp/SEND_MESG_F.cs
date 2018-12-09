using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;

namespace System.MessageBroadcast.Ui.SmsApp
{
   public partial class SEND_MESG_F : UserControl
   {
      public SEND_MESG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

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
         if (Tb_Master.SelectedTab == tp_004)
         {
            int sms = SmsSendedBs.Position;
            SmsSendedBs.DataSource = iProject.Sms_Message_Boxes;
            SmsSendedBs.Position = sms;

            if(xinput.Attribute("filtering") != null)
            {
               switch(xinput.Attribute("filtering").Value)
               {
                  case "phonnumb":
                     SmsSended_Gv.ActiveFilterString = string.Format("PHON_NUMB = '{0}'", xinput.Attribute("valu").Value);
                     break;
                  default:
                     SmsSended_Gv.ActiveFilterString = "";
                     break;
               }
            }
         }
         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
