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
   public partial class SettingsSystemPackage : UserControl
   {
      public SettingsSystemPackage()
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
         catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
         finally { Execute_Query(); }
      }      

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

       private async void Execute_Query()
       {
          var currentTab = Tb_Master.SelectedTab;
          
          if (currentTab == tp_001)
          {
             int subsys = SubSysBs.Position;
             int package = PackageBs.Position;
             
             var result = await Task.Run(() =>
             {
                using (var db = new Data.iProjectDataContext(ConnectionString))
                {
                   return new
                   {
                      SubSystems = db.Sub_Systems.Where(s => s.STAT == "002").ToList(),
                      SubSysIndex = subsys,
                      PackageIndex = package
                   };
                }
             });

             iProject = new Data.iProjectDataContext(ConnectionString);
             SubSysBs.DataSource = result.SubSystems;
             SubSysBs.Position = result.SubSysIndex;
             PackageBs.Position = result.PackageIndex;
          }
          else if (currentTab == tp_002)
          {
             int subsys = SubSysBs.Position;
             int package = PackageBs.Position;
             int packageactivity = PackageActivityBs.Position;
             int subitem = SubSysItemBs.Position;
             
             var result = await Task.Run(() =>
             {
                using (var db = new Data.iProjectDataContext(ConnectionString))
                {
                   return new
                   {
                      SubSystems = db.Sub_Systems.Where(s => s.STAT == "002").ToList(),
                      SubSysIndex = subsys,
                      PackageIndex = package,
                      PackageActivityIndex = packageactivity,
                      SubSysItemIndex = subitem
                   };
                }
             });

             iProject = new Data.iProjectDataContext(ConnectionString);
             SubSysBs.DataSource = result.SubSystems;
             SubSysBs.Position = result.SubSysIndex;
             PackageBs.Position = result.PackageIndex;
             PackageActivityBs.Position = result.PackageActivityIndex;
             SubSysItemBs.Position = result.SubSysItemIndex;
          }
          else if (currentTab == tp_003)
          {
             int subsys = SubSysBs.Position;
             int package = PackageBs.Position;
             int packageinstance = PackageInstanceBs.Position;
             int packinstusergateway = PackageUserGatewayBs.Position;
             
             var result = await Task.Run(() =>
             {
                using (var db = new Data.iProjectDataContext(ConnectionString))
                {
                   return new
                   {
                      SubSystems = db.Sub_Systems.Where(s => s.STAT == "002").ToList(),
                      UserGateways = db.User_Gateways.Where(ug => ug.VALD_TYPE == "002").ToList(),
                      SubSysIndex = subsys,
                      PackageIndex = package,
                      PackageInstanceIndex = packageinstance,
                      PackageUserGatewayIndex = packinstusergateway
                   };
                }
             });

             iProject = new Data.iProjectDataContext(ConnectionString);
             SubSysBs.DataSource = result.SubSystems;
             UserGatewayBs.DataSource = result.UserGateways;
             SubSysBs.Position = result.SubSysIndex;
             PackageBs.Position = result.PackageIndex;
             PackageInstanceBs.Position = result.PackageInstanceIndex;
             PackageUserGatewayBs.Position = result.PackageUserGatewayIndex;
          }
       }

      private void AddPackage_Butn_Click(object sender, EventArgs e)
      {
         PackageBs.AddNew();
      }

      private void SavePackage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var package = PackageBs.Current as Data.Package;

            if (package == null) return;

            PackageBs.EndEdit();

            iProject.SubmitChanges();
            requery = true;
         }
         catch { }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void DeletePackage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var package = PackageBs.Current as Data.Package;

            if (package == null || MessageBox.Show(this, "??? ?? ??? ???? ????? ??????", "??? ????", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.Packages.DeleteOnSubmit(package);

            iProject.SubmitChanges();
            requery = true;
         }
         catch { }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void AddPackageActivity_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subi = SubSysItemBs.Current as Data.Sub_System_Item;
            if (subi == null) return;
            var package = PackageBs.Current as Data.Package;
            if (package == null) return;

            SubSysItemBs.MoveNext();

            //??? ???? ????? ???? ???? ????? ?? ????? ???? ????
            if (PackageActivityBs.List.OfType<Data.Package_Activity>().Any(pa => pa.Sub_System_Item == subi)) return;

            PackageActivityBs.AddNew();
            var packageactivity = PackageActivityBs.Current as Data.Package_Activity;
            packageactivity.SSIT_SUB_SYS = subi.SUB_SYS;
            packageactivity.SSIT_RWNO = subi.RWNO;
            packageactivity.STAT = "002";

            PackageActivityBs.EndEdit();

            iProject.SubmitChanges();
            requery = true;
         }
         catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void ChangePackageActivityStat_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageactivity = PackageActivityBs.Current as Data.Package_Activity;
            if (packageactivity == null) return;

            packageactivity.STAT = packageactivity.STAT == "002" ? "001" : "002";
            PackageActivityBs.EndEdit();

            iProject.SubmitChanges();

            requery = true;
         }
         catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeletePackageActivity_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageactivity = PackageActivityBs.Current as Data.Package_Activity;

            if (packageactivity == null || MessageBox.Show(this, "??? ?? ??? ???? ???? ????? ??????", "??? ???? ????", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.Package_Activities.DeleteOnSubmit(packageactivity);

            iProject.SubmitChanges();
            requery = true;
         }
         catch { }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void AddPackageInstance_Butn_Click(object sender, EventArgs e)
      {
         PackageInstanceBs.AddNew();         
      }

      private void SavePackageInstance_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageinstance = PackageInstanceBs.Current as Data.Package_Instance;
            packageinstance.STAT = "002";
            if (packageinstance.INST_DESC == null || packageinstance.INST_DESC == "") return;

            PackageInstanceBs.EndEdit();
            iProject.SubmitChanges();

            requery = true;
         }
         catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeletePackageInstance_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageinstance = PackageInstanceBs.Current as Data.Package_Instance;

            if (packageinstance == null || MessageBox.Show(this, "??? ?? ??? ????? ???? ????? ????? ??????", "??? ????? ???? ?????", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.Package_Instances.DeleteOnSubmit(packageinstance);

            iProject.SubmitChanges();
            requery = true;
         }
         catch { }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void ChangeStatPackageInstance_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageinstance = PackageInstanceBs.Current as Data.Package_Instance;
            packageinstance.STAT = packageinstance.STAT == "002" ? "001" : "002";

            PackageInstanceBs.EndEdit();
            iProject.SubmitChanges();

            requery = true;
         }
         catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void AddSavePackageInstanceUserGateway_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packinstance = PackageInstanceBs.Current as Data.Package_Instance;
            if (packinstance == null) return;
            var usergateway = UserGatewayBs.Current as Data.User_Gateway;
            if (usergateway == null) return;

            PackageUserGatewayBs.AddNew();
            var packusrgateway = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
            packusrgateway.Package_Instance = packinstance;
            packusrgateway.STAT = "002";

            PackageUserGatewayBs.EndEdit();
            iProject.SubmitChanges();
         }
         catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeletePackageInstanceUserGateway_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageinstanceusergateway = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;

            if (packageinstanceusergateway == null || MessageBox.Show(this, "??? ?? ??? ????? ???? ????? ????? ????? ??????", "??? ????? ???? ????? ?????", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.Package_Instance_User_Gateways.DeleteOnSubmit(packageinstanceusergateway);

            iProject.SubmitChanges();
            requery = true;
         }
         catch { }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }

      private void ChangeStatPackageInstanceUserGateway_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var packageinstanceusergateway = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;

            if (packageinstanceusergateway == null) return;

            packageinstanceusergateway.STAT = packageinstanceusergateway.STAT == "002" ? "001" : "002";

            iProject.SubmitChanges();
            requery = true;
         }
         catch { }
         finally { if (requery) { Execute_Query(); requery = false; } }
      }
   }
}
