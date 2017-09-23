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
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsNetworkConnection : UserControl
   {
      public SettingsNetworkConnection()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int user;
      private int datasource;

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
            int subsys = SubSysBs.Position;
            SubSysBs.DataSource = iProject.Sub_Systems;
            SubSysBs.Position = subsys;
         }
         else if(Tb_Master.SelectedTab == tp_002)
         {
            //user = UserBs.Position;
            //datasource = DatasourceBs.Position;
            UserBs.DataSource = iProject.Users;
            DatasourceBs.DataSource = iProject.DataSources.Where(d => d.SUB_SYS != null);
            UserBs.Position = user;
            DatasourceBs.Position = datasource;
         }
      }

      private void SubSysBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            // نمایش اطلاعات ارتباط ها روی فرم
            Computer_User_Status_Txt.Text = string.Format("<color=Blue>{0}</color><br><color=Green>{1}</color>", HostInfo.Attribute("name").Value, CurrentUser);
            var datasource = iProject.DataSources.FirstOrDefault(s => s.SUB_SYS == subsys.SUB_SYS);
            Server_Database_Status_Txt.Text = string.Format("<color=Black>{0}</color><br><color=Gray>{1}</color>", datasource.IPAddress, datasource.Database_Alias);

            var result = (from a in iProject.Access_User_Datasources
                        join ug in iProject.User_Gateways on new {Host_Name = a.HOST_NAME, User_Id = (long)(a.USER_ID)} equals new {Host_Name = ug.GTWY_MAC_ADRS, User_Id = (long)(ug.USER_ID)}
                        join u in iProject.Users on ug.USER_ID equals u.ID
                        join d in iProject.DataSources on (long)a.DSRC_ID equals (long)d.ID
                        where a.STAT == "002" 
                           && ug.VALD_TYPE == "002"
                           && u.USERDB.ToUpper() == CurrentUser.ToUpper()
                           && d.SUB_SYS == subsys.SUB_SYS
                           && ug.GTWY_MAC_ADRS == HostInfo.Attribute("cpu").Value
                        select a).FirstOrDefault();

            if (result == null) return;

            RightConnectionStatus_Lbl.Appearance.Image = null;
            LeftConnectionStatus_Lbl.Appearance.Image = null;
         }
         catch 
         {
            Server_Database_Status_Txt.Text = "";
            RightConnectionStatus_Lbl.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1418;
            LeftConnectionStatus_Lbl.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1418;
         }
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;
            UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.VALD_TYPE == "002" && ug.USER_ID == user.ID);
            AccessUserDatasourceBs.DataSource = iProject.Access_User_Datasources.Where(a => a.USER_ID == user.ID);
            UserAccount_Lbl.Text = user.TitleFa;
            CreateAccessUserDataSourceMenu();
         }
         catch
         {
            
         }
      }

      private void CreateAccessUserDataSourceMenu()
      {
         AccessUserDatasourceList_Flp.Controls.Clear();
         foreach (var item in AccessUserDatasourceBs.List.OfType<Data.Access_User_Datasource>())
         {
            var simplebutton = new SimpleButton();
            simplebutton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            simplebutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            simplebutton.Appearance.BackColor = item.STAT == "002" ? System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))) : Color.WhiteSmoke;
            simplebutton.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            simplebutton.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            simplebutton.Appearance.Options.UseBackColor = true;
            simplebutton.Appearance.Options.UseFont = true;
            simplebutton.Appearance.Options.UseForeColor = true;
            simplebutton.Appearance.Options.UseTextOptions = true;
            simplebutton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            simplebutton.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            simplebutton.Image = global::System.DataGuard.Properties.Resources.IMAGE_1417;
            simplebutton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            simplebutton.Location = new System.Drawing.Point(378, 3);
            simplebutton.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            simplebutton.LookAndFeel.UseDefaultLookAndFeel = false;
            simplebutton.Name = "simpleButton1";
            simplebutton.Size = new System.Drawing.Size(215, 72);
            simplebutton.TabIndex = 2;
            simplebutton.Tag = item;
            simplebutton.Text = string.Format("<b>{0}</b><br><color=Gray><size=10>{1}@{2}</size></color><br><color=DimGray><size=10>{4}@{5}</size></color><br><b><color=Green>{3}</color></b>", item.DataSource.TitleFa, item.User.USERDB, item.Gateway.COMP_NAME_DNRM, item.ACES_TYPE == "001" ? "unlimited" : "limited", item.DataSource.IPAddress, item.DataSource.Database_Alias);
            simplebutton.Click += AccessUserConnection_Click;
            AccessUserDatasourceList_Flp.Controls.Add(simplebutton);
         }
      }

      private void AccessUserConnection_Click(object sender, EventArgs e)
      {
         user = UserBs.Position;
         datasource = DatasourceBs.Position;
         var aud = ((SimpleButton)sender).Tag as Data.Access_User_Datasource;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 29 /* Execute DoWork4SettingsOtherAccount */),
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnectionInfo", 10 /* Execute ActionCallWindow */){Input = aud}
               }
            )
         );
      }

      private void DatasourceBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var datasource = DatasourceBs.Current as Data.DataSource;
            if (datasource == null) return;
            Datasource_Lbl.Text = string.Format("<color=Black>{0}</color><br><color=Gray>{1}</color>", datasource.IPAddress, datasource.Database_Alias);
         }
         catch 
         {            
            throw;
         }
      }

      private void UserGatewayBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var client = UserGatewayBs.Current as Data.User_Gateway;
            if (client == null) return;
            ClientName_Lbl.Text = string.Format("<color=Black>{0}</color><br><color=Gray>{1}</color>", client.Gateway.COMP_NAME_DNRM, client.Gateway.IP_DNRM);
         }
         catch
         {
            throw;
         }
      }

      private void AddConnection_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;
            var usergateway = UserGatewayBs.Current as Data.User_Gateway;
            if (usergateway == null) return;
            var datasource = DatasourceBs.Current as Data.DataSource;
            if (datasource == null || datasource.SUB_SYS == null) return;

            if (AccessUserDatasourceBs.List.OfType<Data.Access_User_Datasource>().Any(a => a.USER_ID == user.ID && a.HOST_NAME == usergateway.GTWY_MAC_ADRS && a.DataSource.Database_Alias == datasource.Database_Alias)) return;

            AccessUserDatasourceBs.AddNew();
            var aud = AccessUserDatasourceBs.Current as Data.Access_User_Datasource;
            aud.USER_ID = user.ID;
            aud.HOST_NAME = usergateway.GTWY_MAC_ADRS;
            aud.DSRC_ID = datasource.ID;
            aud.STAT = "002";
            aud.ACES_TYPE = "001";

            iProject.Access_User_Datasources.InsertOnSubmit(aud);

            iProject.SubmitChanges();
            requery = true;
         }
         catch 
         {
            
            throw;
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

      private void ChangeNetwork_Butn_Click(object sender, EventArgs e)
      {
         var subsys = SubSysBs.Current as Data.Sub_System;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 30 /* Execute DoWork4SettingsChangeNetwork */),
                  new Job(SendType.SelfToUserInterface, "SettingsChangeNetwork", 10 /* Execute ActionCallWindow */){Input = new XElement("ChangeNetwork", new XAttribute("user", CurrentUser), new XAttribute("subsys", subsys.SUB_SYS), new XAttribute("hostname", HostInfo.Attribute("cpu").Value))}
               }
            )
         );
      }
   }
}
