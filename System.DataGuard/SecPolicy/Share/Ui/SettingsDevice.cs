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
using System.Globalization;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsDevice : UserControl
   {
      public SettingsDevice()
      {
         InitializeComponent();
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
         if(Tb_Master.SelectedTab == tp_001)
         {
            ActiveSessionBs.DataSource = iProject.Active_Sessions.Where(a => a.RWNO == iProject.Active_Sessions.Where(at => at.USGW_GTWY_MAC_ADRS == a.USGW_GTWY_MAC_ADRS && at.USGW_USER_ID == a.USGW_USER_ID && at.USGW_RWNO == a.USGW_RWNO && at.AUDS_ID == a.AUDS_ID && at.ACTN_DATE.Value.Date == a.ACTN_DATE.Value.Date).Max(at => at.RWNO));
            CreateActiveSessionMenu();
         }
      }

      private string MtoS(DateTime dt)
      {
         PersianCalendar pc = new PersianCalendar();
         return string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
      }

      private void CreateActiveSessionMenu()
      {
         ActiveSessionList_Flp.Controls.Clear();
         foreach (var item in ActiveSessionBs.List.OfType<Data.Active_Session>())
         {
            var activesession = new SimpleButton();
            activesession.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            activesession.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            activesession.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            activesession.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            activesession.Appearance.Options.UseFont = true;
            activesession.Appearance.Options.UseForeColor = true;
            activesession.Appearance.Options.UseTextOptions = true;
            activesession.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            activesession.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            activesession.Image = global::System.DataGuard.Properties.Resources.IMAGE_1415;
            activesession.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            activesession.Location = new System.Drawing.Point(146, 3);
            activesession.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            activesession.LookAndFeel.UseDefaultLookAndFeel = false;
            activesession.Name = "simpleButton1";
            activesession.Size = new System.Drawing.Size(215, 72);
            activesession.TabIndex = 1;
            activesession.Tag = "1";
            activesession.Text = string.Format("<b>{0}</b><br><color=Gray><size=-2>{1}</size></color><br><size=-3><color=Green>{2}</color></size><br><color=Red><size=-3>{3}</size></color>", item.User_Gateway.Gateway.COMP_NAME_DNRM, item.Access_User_Datasource.User.USERDB, item.Access_User_Datasource.DataSource.Database_Alias, MtoS((DateTime)item.ACTN_DATE));
            ActiveSessionList_Flp.Controls.Add(activesession);
         }

      }

   }
}
