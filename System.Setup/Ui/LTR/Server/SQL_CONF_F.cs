using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Collections.Specialized;  

namespace System.Setup.Ui.LTR.Server
{
   public partial class SQL_CONF_F : UserControl
   {
      public SQL_CONF_F()
      {
         InitializeComponent();
      }      

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Clear_Butn_Click(object sender, EventArgs e)
      {
         Server_Txt.Text = Username_Txt.Text = Password_Txt.Text = "";
         SqlServerAuth_Rb.Checked = true;
      }

      private void Connect_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ConnectionStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1611;
            ConnectionStatus_Lb.Text = "Connection Check";
            Connect_Butn.Tag = null;

            if (Server_Txt.Text == "") { Server_Txt.Focus(); return; }
            
            if (WinAuth_Rb.Checked)
            {
               var con = new SqlConnection(string.Format("server={0};database=master;Integrated Security=True", Server_Txt.Text));
               con.Open();
               con.Close();
            }
            else
            {
               if (Username_Txt.Text == "") { Username_Txt.Focus(); return; }
               if (Password_Txt.Text == "") { Password_Txt.Focus(); return; }
               var con = new SqlConnection(string.Format("server={0};database=master;Persist Security Info=True;User ID={1};Password={2}", Server_Txt.Text, Username_Txt.Text, Password_Txt.Text));
               con.Open();
               con.Close();
            }

            Connect_Butn.Tag = true;
            ConnectionStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1609;
            ConnectionStatus_Lb.Text = "Connection Test OK!";
         }
         catch
         {
            ConnectionStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1608;
            ConnectionStatus_Lb.Text = "Connection Failed!";
         }
      }

      private void RestoreBackupFile_Butn_Click(object sender, EventArgs e)
      {
         Tb_Master.SelectedTab = tp_002;
      }

      private void AttachDatabaseFile_Butn_Click(object sender, EventArgs e)
      {
         Tb_Master.SelectedTab = tp_003;
      }

      private void CreateConnectionDatabaseInstance_Butn_Click(object sender, EventArgs e)
      {
         Tb_Master.SelectedTab = tp_004;
      }

      #region Restore
      private void SelectBackupFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;

            SelectBackupFile_Ofd.InitialDirectory = parentpath.FullName + "\\data\\backup";
            if (SelectBackupFile_Ofd.ShowDialog() != DialogResult.OK) return;

            BackupFile_Txt.Text = SelectBackupFile_Ofd.FileName;
            // D:\AnarSys\data\backup\iCRM.bak
            var destdb = new DirectoryInfo(BackupFile_Txt.Text);
            PathTargetDbFile_Txt.Tag = 
            PathTargetDbFile_Txt.Text = 
               string.Format(@"{0}\{1}", 
                  destdb.Parent.Parent.FullName,
                  destdb.Name.Substring(0, destdb.Name.LastIndexOf('.'))
               );

            RestoredbStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1611;
            RestoredbStatus_Lb.Text = "Ready For Restore Database!";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SelectPathTargetDbFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;

            SelectPathTargetDbFile_Fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (SelectPathTargetDbFile_Fbd.ShowDialog() != DialogResult.OK) return;

            PathTargetDbFile_Txt.Text = SelectPathTargetDbFile_Fbd.SelectedPath;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void InstallConfigDatabase_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Connect_Butn_Click(null, null);
            if (Connect_Butn.Tag == null || !(bool)Connect_Butn.Tag) { Tb_Master.SelectedTab = tp_001; Connect_Butn.Focus(); }

            if(BackupFile_Txt.Text == ""){BackupFile_Txt.Focus(); return;}
            // 1st Step * Check Subsysem can install on server
            var backupFileInfo = new IO.FileInfo( BackupFile_Txt.Text );
            var backName = backupFileInfo.Name.Substring(0, backupFileInfo.Name.LastIndexOf('.'));
            /// Check validation Subsystem in Anar LicenceKey
            //var _CheckSubSysInstallation =
            //   new Job(SendType.External, "localhost", "CHK_LICN_F", 10 /* Execute ActionCallWindow */, SendType.SelfToUserInterface) 
            //   { 
            //      Input = 
            //         new XElement("CheckValidation",
            //            new XAttribute("subsys", backName.ToLower()),
            //            new XAttribute("type", "install")
            //         )
            //   };
            //_DefaultGateway.Gateway(_CheckSubSysInstallation);
            //if ((_CheckSubSysInstallation.Output as XElement).Attribute("status").Value == "notvalid") { MessageBox.Show("The Application is not valid for install on Server"); return; }

            /// IF CUSTOMER CAN INSTALL SUBSYSTEM GOTO RESTORE BACKUP DATABASE
            SqlCommand cmd =
               new SqlCommand(
                  string.Format(
                  @"RESTORE DATABASE [{0}] FROM  DISK = N'{1}' WITH  FILE = 1,  
                    MOVE N'{2}' TO N'{3}\{0}.mdf',  
                    MOVE N'{2}_Blob' TO N'{3}\{0}_Blob.ndf',  
                    MOVE N'{2}_log' TO N'{3}\{0}_Log.ldf',  
                    NOUNLOAD,  REPLACE,  STATS = 10",
                       CreateTestDemoDatabase_Cb.Checked ? backName + "001" : backName,
                       BackupFile_Txt.Text,
                       backName,
                       CreateTestDemoDatabase_Cb.Checked ? PathTargetDbFile_Txt.Text : PathTargetDbFile_Txt.Text

                  ),
                  WinAuth_Rb.Checked ? 
                     new SqlConnection(string.Format("server={0};database=master;Integrated Security=True", Server_Txt.Text))
                     :
                     new SqlConnection(string.Format("server={0};database=master;Persist Security Info=True;User ID={1};Password={2}", Server_Txt.Text, Username_Txt.Text, Password_Txt.Text))
               );
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            // 2nd Step Empty Database for Ready to Use
            /// GET HOST INFO
            var _hostInfo =
               new Job(SendType.External, "localhost", "DefaultGateway:DataGuard", 04 /* Execute GetHostInfo */, SendType.Self);
            _DefaultGateway.Gateway(_hostInfo);
            
            /// Check Tiny Plus Serial No
            var _CheckTinyPlusSerialNoInstallation =
               new Job(SendType.External, "localhost", "CHK_TINY_F", 10 /* Execute ActionCallWindow */, SendType.SelfToUserInterface) 
               { 
                  Input = 
                     new XElement("TinyPlus",
                        new XAttribute("type", "get")
                     )
               };
            _DefaultGateway.Gateway(_CheckTinyPlusSerialNoInstallation);


            cmd.CommandText = "InstallDatabase";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(
               new SqlParameter("@X", SqlDbType.Xml)
               {
                  Value = 
                     new XElement("Params",
                        new XAttribute("databasetest", CreateTestDemoDatabase_Cb.Checked ? "002" : "001"),
                        new XAttribute("tinyserialno", _CheckTinyPlusSerialNoInstallation.Output.ToString()),
                        //new XAttribute("licensekey", (_CheckSubSysInstallation.Output as XElement).Attribute("key").Value),
                        _hostInfo.Output as XElement                        
                     ).ToString()
               }
            );
            cmd.Connection = 
               WinAuth_Rb.Checked ? 
                     new SqlConnection(string.Format("server={0};database={1};Integrated Security=True", Server_Txt.Text, CreateTestDemoDatabase_Cb.Checked ? backName + "001" : backName))
                     :
                     new SqlConnection(string.Format("server={0};database={1};Persist Security Info=True;User ID={2};Password={3}", Server_Txt.Text, CreateTestDemoDatabase_Cb.Checked ? backName + "001" : backName, Username_Txt.Text, Password_Txt.Text));

            cmd.CommandTimeout = 18000;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            RestoredbStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1609;
            RestoredbStatus_Lb.Text = "Restore db welldone!";
         }
         catch (Exception exc)
         {
            RestoredbStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1608;
            RestoredbStatus_Lb.Text = "Restore db Failed!";

            MessageBox.Show(exc.Message);
         }
      }
      #endregion

      #region Attach Database
      private void SelectDatabaseFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ChooseSubSys_Lov.SelectedItem.ToString() == "") { ChooseSubSys_Lov.Focus(); return; }

            if (SelectDatabaseFile_Ofd.ShowDialog() != DialogResult.OK) return;

            DatabaseFiles_Lst.Items.Clear();
            
            DatabaseFiles_Lst.Items.AddRange(SelectDatabaseFile_Ofd.FileNames);            
         }
         catch (Exception exc)
         {

         }
      }

      private void AttachDatabaseFile_Butn_Click_1(object sender, EventArgs e)
      {
         try
         {
            AttachRespons_Lb.Text = "Wait for respons action";
            if (ChooseSubSys_Lov.SelectedItem.ToString() == "") { ChooseSubSys_Lov.Focus(); return; }
            if (DatabaseFiles_Lst.Items.Count == 0) { DatabaseFiles_Lst.Focus(); return; }

            // 1st Step * Attach Database File to Sql Server Instance
            //ServerConnection sc =
            //   new ServerConnection(
            //         WinAuth_Rb.Checked ?
            //         new SqlConnection(string.Format("server={0};database=master;Integrated Security=True", Server_Txt.Text))
            //         :
            //         new SqlConnection(string.Format("server={0};database=master;Persist Security Info=True;User ID={1};Password={2}", Server_Txt.Text, Username_Txt.Text, Password_Txt.Text))
            //      );

            //Microsoft.SqlServer.Management.Smo.Server iServer = 
            //   new Microsoft.SqlServer.Management.Smo.Server(sc);

            //sc.Connect();

            //if (iServer.Databases.Contains(ChooseSubSys_Lov.SelectedItem.ToString())) { AttachRespons_Lb.Text = "Database is exists ready"; return; }

            //StringCollection dbFilePath = new StringCollection();
            //dbFilePath.AddRange(DatabaseFiles_Lst.Items.OfType<string>().ToArray());
            //iServer.AttachDatabase(ChooseSubSys_Lov.SelectedItem.ToString(), dbFilePath);
            AttachRespons_Lb.Text = "Attach database files successfully!";
         }
         catch (Exception exc)
         {
            AttachRespons_Lb.Text = "Attach database files failed!";
         }
      }
      #endregion

      #region Ceate Database Link
      private void TestInstanceCreateDatabaseLink_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            // First Step * connect to database iProject with scott user
            var con = new SqlConnection(string.Format("server={0};database=iProject;Persist Security Info=True;User ID=scott;Password=abcABC123!@#", Server_Txt.Text));
            con.Open();
            con.Close();

            // Second Step * Create Odbc Connection With iProject Dsn Name
            var _odbcConnection =
               new Job(SendType.External, "localhost", "Commons:Odbc:OdbcSettings", 12 /* Execute CreateDsnName */, SendType.SelfToUserInterface)
               {
                  Input = new List<object>
                  {
                     "iProject",
                     "No Description",
                     Server_Txt.Text,
                     "SQL Server",
                     "iProject",
                     "scott",
                     false
                  }
               };
            _DefaultGateway.Gateway(
               _odbcConnection
            );

            if (_odbcConnection.Status == StatusType.Successful)
            {
               OdbcConnectionStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1609;
               OdbcConnectionStatus_Lb.Text = "Create db connection welldone!";
            }
            else
            {
               OdbcConnectionStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1608;
               OdbcConnectionStatus_Lb.Text = "Create db connection Failed!";
            }
         }
         catch (Exception exc)
         {
            OdbcConnectionStatus_Lb.Appearance.Image = System.Setup.Properties.Resources.IMAGE_1608;
            OdbcConnectionStatus_Lb.Text = "Create db connection Failed!";

            MessageBox.Show(exc.Message);
         }
      }
      #endregion

      private void CreateTestDemoDatabase_Cb_CheckedChanged(object sender, EventArgs e)
      {
         PathTargetDbFile_Txt.Text =
            CreateTestDemoDatabase_Cb.Checked ?
            PathTargetDbFile_Txt.Tag.ToString() + "001" :
            PathTargetDbFile_Txt.Tag.ToString();
      }

      private void SetCurrentCompName_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self)
            {
               AfterChangedOutput =
                  new Action<object>(
                     (output) =>
                     {
                        var hostinfo = output as XElement;
                        Server_Txt.Text = hostinfo.Attribute("name").Value;
                     }
                  )
            }
         );
      }
   }
}
