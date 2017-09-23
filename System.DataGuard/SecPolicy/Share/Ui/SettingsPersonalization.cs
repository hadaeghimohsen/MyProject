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

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsPersonalization : UserControl
   {
      public SettingsPersonalization()
      {
         InitializeComponent();
         Tb_Master.Dock = DockStyle.Fill;
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages, Tab001BackgroundType;
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
         UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.User.USERDB.ToUpper() == CurrentUser.ToUpper() && ug.VALD_TYPE == "002"); 
      }

      private void PackageUserGatewayBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            #region Tab001
            var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
            if (personalization == null) return;

            // Background Type
            personalization.BACK_GRND_TYPE = personalization.BACK_GRND_TYPE == null ? "002" : personalization.BACK_GRND_TYPE;
            switch (personalization.BACK_GRND_TYPE)
            {
               case "001":
                  Rb_Tab001Picture.Checked = true;
                  break;
               case "002":
                  Rb_Tab001SimpleColor.Checked = true;
                  break;
            }

            // Picture Type
            personalization.IMAG_LYOT = personalization.IMAG_LYOT == null ? "003" : personalization.IMAG_LYOT;
            switch (personalization.IMAG_LYOT)
            {
               case "001":
                  Rb_Tab001Tile.Checked = true;
                  break;
               case "002":
                  Rb_Tab001Center.Checked = true;
                  break;
               case "003":
                  Rb_Tab001Stretch.Checked = true;
                  break;
               case "004":
                  Rb_Tab001Zoom.Checked = true;
                  break;
            }

            // Set Image On Background Panel Show
            if (Rb_Tab001Picture.Checked && personalization.IMAG_PATH != "")
            {
               try
               {
                  Pn_Tab001Background.BackgroundImage = Image.FromFile(personalization.IMAG_PATH);
                  Pn_Tab001Background.BackgroundImageLayout = Rb_Tab001Tile.Checked ? ImageLayout.Tile : (Rb_Tab001Center.Checked ? ImageLayout.Center : (Rb_Tab001Stretch.Checked ? ImageLayout.Stretch : (Rb_Tab001Zoom.Checked ? ImageLayout.Zoom : ImageLayout.None)));
               }
               catch { Pn_Tab001Background.BackgroundImage = null; }
            }

            // Set Color On Background Panel Show
            if (Rb_Tab001SimpleColor.Checked && personalization.BACK_GRND_COLR != null)
            {
               Pn_Tab001Background.BackgroundImage = null;
               try
               {
                  Pn_Tab001Background.BackColor = ColorTranslator.FromHtml(personalization.BACK_GRND_COLR);
               }
               catch
               {
                  Pn_Tab001Background.BackColor = Color.Transparent;
               }
            }
            #endregion
            #region Tab002
            // Set Theme Color on Panels Show
            try
            {
               Pn_ColorTheme1.BackColor = Pn_ColorTheme2.BackColor = Pn_ColorTheme3.BackColor = Pn_ColorTheme4.BackColor = ColorTranslator.FromHtml(personalization.THEM_COLR);
               Flp_Tab002ColorSelection.Controls.OfType<SimpleButton>().Where(b => b.Image != null).ToList().ForEach(b => b.Image = null);
               Flp_Tab002ColorSelection.Controls.OfType<SimpleButton>().Where(b => ColorTranslator.FromHtml(personalization.THEM_COLR) == b.Appearance.BackColor).ToList().ForEach(b => b.Image = global::System.DataGuard.Properties.Resources.IMAGE_1430);
            }
            catch
            {
               Pn_ColorTheme1.BackColor = Pn_ColorTheme2.BackColor = Pn_ColorTheme3.BackColor = Pn_ColorTheme4.BackColor = Color.Transparent;
            }
            #endregion
            #region Tab003
            // Set Lock Screen Color on Panel Show
            try
            {
               Pn_Tab003LockScreen.BackColor = ColorTranslator.FromHtml(personalization.LOCK_SCRN_BACK_GRND_COLR);
               Flp_Tab003ColorSelection.Controls.OfType<SimpleButton>().Where(b => b.Image != null).ToList().ForEach(b => b.Image = null);
               Flp_Tab003ColorSelection.Controls.OfType<SimpleButton>().Where(b => ColorTranslator.FromHtml(personalization.LOCK_SCRN_BACK_GRND_COLR) == b.Appearance.BackColor).ToList().ForEach(b => b.Image = global::System.DataGuard.Properties.Resources.IMAGE_1430);
            }
            catch
            {
               Pn_Tab003LockScreen.BackColor = Color.Transparent;
            }
            // Set Show Login Desc
            personalization.SHOW_LOG_IN_DESC = personalization.SHOW_LOG_IN_DESC == null ? "002" : personalization.SHOW_LOG_IN_DESC;
            switch (personalization.SHOW_LOG_IN_DESC)
            {
               case "001":
                  Ts_ShowLoginDesc.IsOn = false;
                  break;
               case "002":
                  Ts_ShowLoginDesc.IsOn = true;
                  break;
            }
            #endregion
            #region Tab004
            // Set Show Desktop Stat
            personalization.SHOW_DESK_STAT = personalization.SHOW_DESK_STAT == null ? "002" : personalization.SHOW_DESK_STAT;
            switch (personalization.SHOW_DESK_STAT)
            {
               case "001":
                  Ts_ShowDesktopStat.IsOn = false;
                  break;
               case "002":
                  Ts_ShowDesktopStat.IsOn = true;
                  break;
            }

            // Set Show Start Stat
            personalization.SHOW_STRT_STAT = personalization.SHOW_STRT_STAT == null ? "002" : personalization.SHOW_STRT_STAT;
            switch (personalization.SHOW_STRT_STAT)
            {
               case "001":
                  Ts_ShowStartStat.IsOn = false;
                  break;
               case "002":
                  Ts_ShowStartStat.IsOn = true;
                  break;
            }

            // Set Show Task Stat
            personalization.SHOW_TASK_STAT = personalization.SHOW_TASK_STAT == null ? "002" : personalization.SHOW_TASK_STAT;
            switch (personalization.SHOW_TASK_STAT)
            {
               case "001":
                  Ts_ShowTaskBarStat.IsOn = false;
                  break;
               case "002":
                  Ts_ShowTaskBarStat.IsOn = true;
                  break;
            }
            #endregion
            #region Tab005
            // Set Start Show More
            Pn_Tab005StartFullScreen.BackColor = Pn_Tab005StartMenu.BackColor = Pn_ColorTheme1.BackColor;
            Flp_Tab001StartMenu.BackColor = Pn_Tab001Taskbar.BackColor = Pn_ColorTheme1.BackColor;
            personalization.STRT_SHOW_MORE = personalization.STRT_SHOW_MORE == null ? "002" : personalization.STRT_SHOW_MORE;
            switch (personalization.STRT_SHOW_MORE)
            {
               case "001":
                  Ts_StartShowMore.IsOn = false;
                  break;
               case "002":
                  Ts_StartShowMore.IsOn = true;
                  break;
            }

            Flp_StartShowMore.Visible = Ts_StartShowMore.IsOn;

            // Set Start Show Fav
            personalization.STRT_SHOW_FAV = personalization.STRT_SHOW_FAV == null ? "002" : personalization.STRT_SHOW_FAV;
            switch (personalization.STRT_SHOW_FAV)
            {
               case "001":
                  Ts_StartShowFav.IsOn = false;
                  break;
               case "002":
                  Ts_StartShowFav.IsOn = true;
                  break;
            }

            Flp_StartShowFav.Visible = Ts_StartShowFav.IsOn;

            // Set Start Full Screen
            personalization.STRT_FULL_SCRN = personalization.STRT_FULL_SCRN == null ? "001" : personalization.STRT_FULL_SCRN;
            switch (personalization.STRT_FULL_SCRN)
            {
               case "001":
                  Ts_StartFullScreen.IsOn = false;
                  break;
               case "002":
                  Ts_StartFullScreen.IsOn = true;
                  break;
            }

            if (Ts_StartFullScreen.IsOn)
               Pn_Tab005StartFullScreen.Dock = DockStyle.Fill;
            else
               Pn_Tab005StartFullScreen.Dock = DockStyle.None;
            #endregion
            #region Tab006
            personalization.TASK_LOCT = personalization.TASK_LOCT == null ? "003" : personalization.TASK_LOCT;

            Pn_TaskbarLoc.BackColor = Pn_ColorTheme1.BackColor;
            switch (personalization.TASK_LOCT)
            {
               case "001":
                  Pn_TaskbarLoc.Dock = DockStyle.Top;
                  Pn_TaskbarLoc.Height = 15;
                  break;
               case "002":
                  Pn_TaskbarLoc.Dock = DockStyle.Right;
                  Pn_TaskbarLoc.Width = 15;
                  break;
               case "003":
                  Pn_TaskbarLoc.Dock = DockStyle.Bottom;
                  Pn_TaskbarLoc.Height = 15;
                  break;
               case "004":
                  Pn_TaskbarLoc.Dock = DockStyle.Left;
                  Pn_TaskbarLoc.Width = 15;
                  break;
            }

            personalization.TASK_SIZE = personalization.TASK_SIZE == null ? "002" : personalization.TASK_SIZE;
            switch (personalization.TASK_SIZE)
            {
               case "001":
                  Ts_TaskSize.IsOn = false;
                  break;
               case "002":
                  Ts_TaskSize.IsOn = true;
                  break;
            }
            #endregion
         }
         catch (Exception)
         {
            throw;
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PackageUserGatewayBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch { }
      }

      private void PackageUserGatewayBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      #region Tab Page 001
      private void FIELD_BACK_GRND_TYPE_CheckedChanged(object sender, EventArgs e)
      {
         RadioButton cb = (RadioButton)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.BACK_GRND_TYPE = cb.Tag.ToString();

         #region Action on TabControl
         if (Tab001BackgroundType == null)
            Tab001BackgroundType = Tb_Tab001BackgroundType.TabPages.OfType<TabPage>().ToList();

         var selectedtabpage = Tab001BackgroundType.Where(t => t.Tag == cb.Tag).First();
         Tb_Tab001BackgroundType.TabPages.Clear();
         Tb_Tab001BackgroundType.TabPages.Add(selectedtabpage);
         #endregion

         // Set Image On Background Panel Show
         if (Rb_Tab001Picture.Checked && personalization.IMAG_PATH != "")
         {
            try
            {
               Pn_Tab001Background.BackgroundImage = Image.FromFile(personalization.IMAG_PATH);
               Pn_Tab001Background.BackgroundImageLayout = Rb_Tab001Tile.Checked ? ImageLayout.Tile : (Rb_Tab001Center.Checked ? ImageLayout.Center : (Rb_Tab001Stretch.Checked ? ImageLayout.Stretch : (Rb_Tab001Zoom.Checked ? ImageLayout.Zoom : ImageLayout.None)));
            }
            catch { Pn_Tab001Background.BackgroundImage = null; }
         }

         // Set Color On Background Panel Show
         if (Rb_Tab001SimpleColor.Checked && personalization.BACK_GRND_COLR != null)
         {
            Pn_Tab001Background.BackgroundImage = null;
            try
            {
               Pn_Tab001Background.BackColor = ColorTranslator.FromHtml(personalization.BACK_GRND_COLR);
               Flp_Tab001ColorSelection.Controls.OfType<SimpleButton>().Where(b => b.Image != null).ToList().ForEach(b => b.Image = null);
               Flp_Tab001ColorSelection.Controls.OfType<SimpleButton>().Where(b => ColorTranslator.FromHtml(personalization.BACK_GRND_COLR) == b.Appearance.BackColor).ToList().ForEach(b => b.Image = global::System.DataGuard.Properties.Resources.IMAGE_1430);
            }
            catch
            {
               Pn_Tab001Background.BackColor = Color.Transparent;
            }
         }

      }

      private void FIELD_IMAG_LYOT_CheckedChanged(object sender, EventArgs e)
      {
         RadioButton cb = (RadioButton)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.IMAG_LYOT = cb.Tag.ToString();

         try
         {            
            Pn_Tab001Background.BackgroundImageLayout = Rb_Tab001Tile.Checked ? ImageLayout.Tile : (Rb_Tab001Center.Checked ? ImageLayout.Center : (Rb_Tab001Stretch.Checked ? ImageLayout.Stretch : (Rb_Tab001Zoom.Checked ? ImageLayout.Zoom : ImageLayout.None)));
         }
         catch { Pn_Tab001Background.BackgroundImage = null; }
      }

      private void BackgroundSelection_Click(object sender, EventArgs e)
      {
         Pn_Tab001Background.BackgroundImage = ((Panel)sender).BackgroundImage;
         
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.IMAG_PATH = ((Panel)sender).Tag.ToString(); 
      }

      private void Tab001Colori_Butn_Click(object sender, EventArgs e)
      {
         Pn_Tab001Background.BackColor = ((SimpleButton)sender).Appearance.BackColor;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.BACK_GRND_COLR = ColorTranslator.ToHtml(Pn_Tab001Background.BackColor);

         Flp_Tab001ColorSelection.Controls.OfType<SimpleButton>().Where(b => b.Image != null).ToList().ForEach(b => b.Image = null);
         ((SimpleButton)sender).Image = global::System.DataGuard.Properties.Resources.IMAGE_1430;
      }
      #endregion

      private void Tab002ColorThemei_Butn_Click(object sender, EventArgs e)
      {
         Pn_ColorTheme1.BackColor = ((SimpleButton)sender).Appearance.BackColor;
         Pn_ColorTheme2.BackColor = ((SimpleButton)sender).Appearance.BackColor;
         Pn_ColorTheme3.BackColor = ((SimpleButton)sender).Appearance.BackColor;
         Pn_ColorTheme4.BackColor = ((SimpleButton)sender).Appearance.BackColor;

         Pn_Tab001Taskbar.BackColor = Pn_Tab005StartFullScreen.BackColor = Pn_Tab005StartMenu.BackColor = Pn_TaskbarLoc.BackColor = Flp_Tab001StartMenu.BackColor = Pn_ColorTheme1.BackColor;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.THEM_COLR = ColorTranslator.ToHtml(Pn_ColorTheme1.BackColor);

         Flp_Tab002ColorSelection.Controls.OfType<SimpleButton>().Where(b => b.Image != null).ToList().ForEach(b => b.Image = null);
         ((SimpleButton)sender).Image = global::System.DataGuard.Properties.Resources.IMAGE_1430;
      }

      private void Tab003LockScreenColor_Butn_Click(object sender, EventArgs e)
      {
         Pn_Tab003LockScreen.BackColor = ((SimpleButton)sender).Appearance.BackColor;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.LOCK_SCRN_BACK_GRND_COLR = ColorTranslator.ToHtml(Pn_Tab003LockScreen.BackColor);

         Flp_Tab003ColorSelection.Controls.OfType<SimpleButton>().Where(b => b.Image != null).ToList().ForEach(b => b.Image = null);
         ((SimpleButton)sender).Image = global::System.DataGuard.Properties.Resources.IMAGE_1430;
      }

      private void Ts_ShowDesktopStat_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.SHOW_DESK_STAT = ts.IsOn ? "002" : "001";
      }

      private void Ts_ShowStartStat_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.SHOW_STRT_STAT = ts.IsOn ? "002" : "001";
      }

      private void Ts_ShowTaskBarStat_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.SHOW_TASK_STAT = ts.IsOn ? "002" : "001";
      }

      private void Ts_StartShowMore_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.STRT_SHOW_MORE = ts.IsOn ? "002" : "001";

         Flp_StartShowMore.Visible = ts.IsOn;
      }

      private void Ts_StartShowFav_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.STRT_SHOW_FAV = ts.IsOn ? "002" : "001";

         Flp_StartShowFav.Visible = ts.IsOn;
      }

      private void Ts_StartFullScreen_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.STRT_FULL_SCRN = ts.IsOn ? "002" : "001";

         if (ts.IsOn)
            Pn_Tab005StartFullScreen.Dock = DockStyle.Fill;
         else
            Pn_Tab005StartFullScreen.Dock = DockStyle.None;
      }

      private void TaskBarLoc_Butn_Click(object sender, EventArgs e)
      {
         SimpleButton sb = (SimpleButton)sender;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.TASK_LOCT = sb.Tag.ToString();

         switch (personalization.TASK_LOCT)
         {
            case "001":
               Pn_TaskbarLoc.Dock = DockStyle.Top;
               Pn_TaskbarLoc.Height = 15;
               break;
            case "002":
               Pn_TaskbarLoc.Dock = DockStyle.Right;
               Pn_TaskbarLoc.Width = 15;
               break;
            case "003":
               Pn_TaskbarLoc.Dock = DockStyle.Bottom;
               Pn_TaskbarLoc.Height = 15;
               break;
            case "004":
               Pn_TaskbarLoc.Dock = DockStyle.Left;
               Pn_TaskbarLoc.Width = 15;
               break;
         }
      }

      private void Lnk_DockStartMenuItem_Click(object sender, EventArgs e)
      {
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 19 /* Execute DoWork4SettingsPersonalizationDockStartMenuItem */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationDockStartMenuItem", 10 /* Execute ActionCallWindow */){Input = personalization}
               }
            )
         );
      }

      private void Ts_TaskSize_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.TASK_SIZE = ts.IsOn ? "002" : "001";
      }

      private void Lnk_SystemTryItem_Click(object sender, EventArgs e)
      {
         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 20 /* Execute DoWork4SettingsPersonalizationSystemTryItem */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationSystemTryItem", 10 /* Execute ActionCallWindow */){Input = personalization}
               }
            )
         );
      }

      private void Ts_ShowLoginDesc_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         personalization.SHOW_LOG_IN_DESC = ts.IsOn ? "002" : "001";
      }
   }
}
