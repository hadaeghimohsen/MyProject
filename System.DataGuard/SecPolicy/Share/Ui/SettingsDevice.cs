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
         else if(Tb_Master.SelectedTab == tp_003)
         {
            PosBs.DataSource = iProject.Pos_Devices;
            CreatePosMenu();
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

      private void CreatePosMenu()
      {
         PosList_Flp.Controls.Clear();
         foreach (var item in PosBs.List.OfType<Data.Pos_Device>())
         {
            var pos = new SimpleButton();
            pos.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            pos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            if (item.POS_STAT == "002")
            {
               pos.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
               pos.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            }
            else
            {
               pos.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
               pos.Appearance.BorderColor = System.Drawing.Color.Silver;
            }
            pos.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pos.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            pos.Appearance.Options.UseBackColor = true;
            pos.Appearance.Options.UseBorderColor = true;
            pos.Appearance.Options.UseFont = true;
            pos.Appearance.Options.UseForeColor = true;
            pos.Appearance.Options.UseTextOptions = true;
            pos.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            pos.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            pos.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            pos.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
            pos.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            pos.Location = new System.Drawing.Point(306, 3);
            pos.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            pos.LookAndFeel.UseDefaultLookAndFeel = false;
            pos.Name = "simpleButton2";
            pos.Size = new System.Drawing.Size(215, 59);
            pos.TabIndex = 1;
            pos.Tag = item;
            pos.Click += Pos_Click;
            pos.Text = string.Format("{0}  {1}<br><color=Gray><size=9>{2}</size></color><br>" + "<color=Green><size=9>{3}</size></color><br>", item.POS_DESC, item.POS_DFLT == "002" ? "<b>*</b>" : "", iProject.D_BANKs.FirstOrDefault(b => item.BANK_TYPE == b.VALU).DOMN_DESC + " : " + item.BNKB_CODE, "شماره حساب : " + item.BNKA_ACNT_NUMB);
            
            PosList_Flp.Controls.Add(pos);
         }
      }

      void Pos_Click(object sender, EventArgs e)
      {
         try
         {
            var pos = (sender as SimpleButton).Tag as Data.Pos_Device;
            if (pos == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 38 /* Execute DoWork4SettingsPaymentPos */),
                     new Job(SendType.SelfToUserInterface, "SettingsPaymentPos", 10 /* Execute ActionCallWindow */){Input = pos}
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NewPos_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 37 /* Execute DoWork4SettingsNewPos */),
               }
            )
         );
      }

   }
}
