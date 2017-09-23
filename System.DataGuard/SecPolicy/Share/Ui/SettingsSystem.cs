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
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsSystem : UserControl
   {
      public SettingsSystem()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
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
            int subsys = SubSysBs.Position;
            SubSysBs.DataSource = iProject.Sub_Systems.Where(s => s.STAT == "002");
            SubSysBs.Position = subsys;
         }
         else if (Tb_Master.SelectedTab == tp_002 || Tb_Master.SelectedTab == tp_003)
         {
            int user = UserGatewayBs.Position;
            int packinstuser = PackageUserGatewayBs.Position;
            UsersBs.DataSource = iProject.Users.Where(u => u.USERDB == CurrentUser);
            UserGatewayBs.Position = user;
            PackageUserGatewayBs.Position = packinstuser;
         }
         else if(Tb_Master.SelectedTab == tp_005)
         {
            if(SubSysBs.Count == 0)
               SubSysBs.DataSource = iProject.Sub_Systems.Where(s => s.STAT == "002");
         }
         requery = false;
      }

      #region tp_001
      private string MtoS(DateTime dt)
      {
         PersianCalendar pc = new PersianCalendar();
         return string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
      }

      private string NumToUnit(double? byteCount)      
      {
         try
         {
            string[] suf = { "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
               return "0 " + suf[0];
            long bytes = Math.Abs((long)byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign((long)byteCount) * num).ToString() + " " + suf[place];
         }
         catch { return "0 MB"; }
      }

      private void SubSysBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            if (Tb_Master.SelectedTab == tp_001)
            {               
               INST_DATE_Lbl.Text = MtoS((DateTime)subsys.INST_DATE);
               LICN_TRIL_DATE_Lbl.Text = MtoS((DateTime)subsys.LICN_TRIL_DATE);
               LICN_TYPE_Lbl.Text = subsys.LICN_TYPE == "001" ? "آزمایشی محدود" : "اصلی";

               var dbfilesinfo = iProject.VF_DataBaseFileInfo(subsys.DB_NAME);
               LDF_Lbl.Text = NumToUnit(dbfilesinfo.Where(f => f.type == 1).Sum(f => f.size));
               MDF_Lbl.Text = NumToUnit(dbfilesinfo.Where(f => f.type == 0 && f.File_Type == "MDF").Sum(f => f.size));
               NDF_Lbl.Text = NumToUnit(dbfilesinfo.Where(f => f.type == 0 && f.File_Type == "NDF").Sum(f => f.size));
               SubSysDesc_Text.Text = subsys.SUB_DESC;

               InstallUninstall_Butn.Text = subsys.INST_STAT == "001" ? "Install" : "Uninstall";
            }
            else if(Tb_Master.SelectedTab == tp_005)
            {
               if (subsys.SUB_SYS == 0)
                  Pn_SubSys0.Visible = true;
               else
                  Pn_SubSys0.Visible = false;

               subsys.JOBS_STAT = subsys.JOBS_STAT == null ? "001" : subsys.JOBS_STAT;
               switch (subsys.JOBS_STAT)
               {
                  case "001":
                     Ts_JobsStat.IsOn = false;
                     break;
                  case "002":
                     Ts_JobsStat.IsOn = true;
                     break;
               }
            }
         }
         catch (Exception exc)
         {

         }
      }

      private void Emptydb_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Shrinkdb_Butn_Click(object sender, EventArgs e)
      {
         iProject.ShrinkLogFileDb();
         SubSysBs_CurrentChanged(null, null);
      }

      private void InstallUninstall_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaveSubSys_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            subsys.SUB_DESC = SubSysDesc_Text.Text;

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {

         }
      }

      private void ClearSubSysDesc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            subsys.SUB_DESC = "";

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {

         }
      }
      #endregion

      #region tp_002
      private void SaveDefaultApp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UsersBs.Current as Data.User;

            if (user == null) return;

            UsersBs.EndEdit();

            iProject.SubmitChanges();

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
               requery = false;
            }
         }
      }

      private void DisabledDefaultApp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UsersBs.Current as Data.User;

            if (user == null) return;

            user.DFLT_SUB_SYS = null;

            UsersBs.EndEdit();

            iProject.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {

         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }
      #endregion      

      private void PackageUserGatewayBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var packusergateway = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
            if (packusergateway == null) return;

            PackageActivityBs.DataSource = iProject.Package_Activities.Where(pa => pa.Package == packusergateway.Package_Instance.Package && pa.Sub_System_Item.Sub_System == packusergateway.Package_Instance.Package.Sub_System && pa.STAT == "002");
         }
         catch (Exception)
         {
            
            throw;
         }
      }

      private void UsersBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var user = UsersBs.Current as Data.User;

            if (user == null) return;

            UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.USER_ID == user.ID && ug.VALD_TYPE == "002");
         }
         catch (Exception exc)
         {

            throw;
         }
      }

      private void SystemPackage_Lnk_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 18 /* Execute DoWork4SettingsSystemPackage */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 10 /* Execute ActionCallWindow */)
               }
            )
         );
      }

      private void AddSaveUserQuickAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packactivity = PackageActivityBs.Current as Data.Package_Activity;
            if (packactivity == null) return;
            var packusergateway = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
            if (packusergateway == null) return;

            UserQiuckActionBs.AddNew();
            var userquickaction = UserQiuckActionBs.Current as Data.Package_Instance_User_Qiuck_Action;
            userquickaction.Package_Activity = packactivity;
            userquickaction.ORDR = UserQiuckActionBs.List.OfType<Data.Package_Instance_User_Qiuck_Action>().Max(q => q.ORDR) + 1;
            userquickaction.STAT = "002";
            PackageActivityBs.MoveNext();

            UserQiuckActionBs.EndEdit();
            iProject.SubmitChanges();

            requery = true;
         }
         catch { }
         finally 
         { 
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeleteUserQuickAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var userquickaction = UserQiuckActionBs.Current as Data.Package_Instance_User_Qiuck_Action;

            if (userquickaction == null || MessageBox.Show(this, "آیا با آیتم عملکردی سریع موافق هستید؟", "حذف آیتم عملکردی سریع", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.Package_Instance_User_Qiuck_Actions.DeleteOnSubmit(userquickaction);

            iProject.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {

         }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void ChangeStatUserQiuckAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var userquickaction = UserQiuckActionBs.Current as Data.Package_Instance_User_Qiuck_Action;

            if (userquickaction == null) return;

            userquickaction.STAT = userquickaction.STAT == "002" ? "001" : "002";

            iProject.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {

         }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void SHOW_NOTF_STAT_Ts_Toggled(object sender, EventArgs e)
      {
         try
         {
            var packusergateway = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
            if (packusergateway == null) return;

            packusergateway.SHOW_NOTF_STAT = SHOW_NOTF_STAT_Ts.IsOn ? "002" : "001";

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {
            
            throw;
         }
      }

      private void Ts_JobsStat_Toggled(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            subsys.JOBS_STAT = Ts_JobsStat.IsOn ? "002" : "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);            
         }
      }

      private void ClearSubSysJobs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            subsys.JOBS_STAT = "001";
            subsys.FREQ_INTR = null;

            iProject.UpdateSubSystem(subsys.SUB_SYS, subsys.STAT, subsys.INST_STAT, subsys.INST_DATE, subsys.LICN_TYPE, subsys.LICN_TRIL_DATE, subsys.LICN_DESC, subsys.SUB_DESC, subsys.JOBS_STAT, subsys.FREQ_INTR);
         }
         catch (Exception exc)
         {

         }
      }

      private void SetSubSysJobs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            iProject.UpdateSubSystem(subsys.SUB_SYS, subsys.STAT, subsys.INST_STAT, subsys.INST_DATE, subsys.LICN_TYPE, subsys.LICN_TRIL_DATE, subsys.LICN_DESC, subsys.SUB_DESC, subsys.JOBS_STAT, subsys.FREQ_INTR);
         }
         catch (Exception exc)
         {

         }
      }

      private void InstallUninstallJobs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            iProject.InstallOrUninstallJob(
               new XElement("JobScheduale",
                  new XAttribute("stat", subsys.JOBS_STAT ?? "001"),
                  new XAttribute("freqintr", subsys.FREQ_INTR ?? 10)
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
