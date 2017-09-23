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
   public partial class SettingsUpdateSecurity : UserControl
   {
      public SettingsUpdateSecurity()
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
         SecurityManagmentBs.DataSource = iProject.Security_Managments; ;
         UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.User.USERDB.ToUpper() == CurrentUser.ToUpper() && ug.VALD_TYPE == "002"); 
      }

      private void SecurityManagmentBs_CurrentChanged(object sender, EventArgs e)
      {
         var secm = SecurityManagmentBs.Current as Data.Security_Managment;
         if (secm == null) return;

         #region Tab001
         // Set Last Update
         #endregion
         #region Tab002
         // Set Active Filtering
         secm.PLCY_MAC_ADDR_FLTR = secm.PLCY_MAC_ADDR_FLTR == null ? "002" : "001";
         switch (secm.PLCY_MAC_ADDR_FLTR)
         {
            case "001":
               Ts_PolicyMacAddressFiltering.IsOn = false;
               break;
            case "002":
               Ts_PolicyMacAddressFiltering.IsOn = true;
               break;
         }

         // Set Enter Safe System Define
         secm.PLCY_FORC_SAFE_ENTR = secm.PLCY_FORC_SAFE_ENTR == null ? "002" : "001";
         switch (secm.PLCY_FORC_SAFE_ENTR)
         {
            case "001":
               Ts_EnterSafeSystemDefine.IsOn = false;
               break;
            case "002":
               Ts_EnterSafeSystemDefine.IsOn = true;
               break;
         }

         // Set Block System Status
         secm.PLCY_COMP_BLOK = secm.PLCY_COMP_BLOK == null ? "002" : "001";
         switch (secm.PLCY_COMP_BLOK)
         {
            case "001":
               Ts_BlockSystemStatus.IsOn = false;
               break;
            case "002":
               Ts_BlockSystemStatus.IsOn = true;
               break;
         }

         // Set Login User Block System 
         secm.LOGN_COMP_BLOK = secm.LOGN_COMP_BLOK == null ? "002" : "001";
         switch (secm.LOGN_COMP_BLOK)
         {
            case "001":
               Ts_LoginUserBlockSystem.IsOn = false;
               break;
            case "002":
               Ts_LoginUserBlockSystem.IsOn = true;
               break;
         }

         // Set Show Alarm
         secm.SHOW_ALRM_UNAT = secm.SHOW_ALRM_UNAT == null ? "002" : "001";
         switch (secm.SHOW_ALRM_UNAT)
         {
            case "001":
               Ts_ShowAlamUnauthorised.IsOn = false;
               break;
            case "002":
               Ts_ShowAlamUnauthorised.IsOn = true;
               break;
         }
         #endregion
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try {
            SecurityManagmentBs.EndEdit();
            PackageUserGatewayBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch { }
      }

      private void SecurityManagmentBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void Ts_PolicyMacAddressFiltering_Toggled(object sender, EventArgs e)
      {
         var secm = SecurityManagmentBs.Current as Data.Security_Managment;
         if (secm == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         secm.PLCY_MAC_ADDR_FLTR = ts.IsOn ? "002" : "001";
      }

      private void Ts_EnterSafeSystemDefine_Toggled(object sender, EventArgs e)
      {
         var secm = SecurityManagmentBs.Current as Data.Security_Managment;
         if (secm == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         secm.PLCY_FORC_SAFE_ENTR = ts.IsOn ? "002" : "001";
      }

      private void Ts_BlockSystemStatus_Toggled(object sender, EventArgs e)
      {
         var secm = SecurityManagmentBs.Current as Data.Security_Managment;
         if (secm == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         secm.PLCY_COMP_BLOK = ts.IsOn ? "002" : "001";
      }

      private void Ts_LoginUserBlockSystem_Toggled(object sender, EventArgs e)
      {
         var secm = SecurityManagmentBs.Current as Data.Security_Managment;
         if (secm == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         secm.LOGN_COMP_BLOK = ts.IsOn ? "002" : "001";
      }

      private void Ts_ShowAlamUnauthorised_Toggled(object sender, EventArgs e)
      {
         var secm = SecurityManagmentBs.Current as Data.Security_Managment;
         if (secm == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         secm.SHOW_ALRM_UNAT = ts.IsOn ? "002" : "001";
      }

      private void AuthGateway_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 31 /* Execute DoWork4SettingsAuthoriseGateway */),
                  new Job(SendType.SelfToUserInterface, "SettingsAuthoriseGateway", 10 /* Execute ActionCallWindow */)
               }
            )
         );
      }

      private void Block_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 32 /* Execute DoWork4SettingsBlockGateway */),
                  new Job(SendType.SelfToUserInterface, "SettingsBlockGateway", 10 /* Execute ActionCallWindow */)
               }
            )
         );
      }

      private void UnAuthGateway_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 33 /* Execute DoWork4SettingsUnAuthoriseGateway */),
                  new Job(SendType.SelfToUserInterface, "SettingsUnAuthoriseGateway", 10 /* Execute ActionCallWindow */)
               }
            )
         );
      }

   }
}
