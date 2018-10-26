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
using System.Xml.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace System.Setup.Ui.LTR.MasterPage
{
   public partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();
      }

      private void NewSqlServerInstance_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            MessageBox.Show(this, "Please install the instance name \"MSSQLSERVER\" before installing SQL SERVER.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            if (Environment.Is64BitOperatingSystem)
            {
               var sqlserverpath = parentpath.FullName + "\\tools\\sqlserver\\SQLEXPRWT_x64_ENU.exe";
               //sqlserverpath = @"D:\iData\Utility\MicrosoftComponents\Sql Server Collection\Express\64\setup.exe /CONFIGURATIONFILE=ConfigurationFile.ini";
               Process.Start(sqlserverpath);
            }
            else
            {
               var sqlserverpath = parentpath.FullName + "\\tools\\sqlserver\\SQLEXPRWT_x86_ENU.exe";
               //sqlserverpath = @"D:\iData\Utility\MicrosoftComponents\Sql Server Collection\Express\32\setup.exe /CONFIGURATIONFILE=ConfigurationFile.ini";
               Process.Start(sqlserverpath);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ConfigDatabase_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 04 /* Execute Sql_Conf_F */)
               }
            )
         );
      }

      [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
      public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
                                         string lpFileName);
      private void InstallComponent_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            // Install Fonts
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            var fontspath = parentpath.FullName + "\\tools\\fonts";

            foreach (var font in IO.Directory.GetFiles(fontspath))
            {
               var result = AddFontResource(font);
            }

            // Install Finger Print SDK
            var sdkpath = parentpath.FullName + "\\tools\\fpsdk";
            if (Environment.Is64BitOperatingSystem)
            {               
               foreach (var dllfile in IO.Directory.GetFiles(sdkpath).Where(f => f.Contains(".dll")))
               {
                  IO.File.Copy(dllfile, Environment.ExpandEnvironmentVariables(@"%windir%\syswow64\") + new IO.FileInfo(dllfile).Name, true);
               }
               Process.Start(sdkpath + "\\Just64bit_register_SDK.bat");
            }
            else
            {
               foreach (var dllfile in IO.Directory.GetFiles(sdkpath).Where(f => f.Contains(".dll")))
               {
                  IO.File.Copy(dllfile, Environment.ExpandEnvironmentVariables(@"%windir%\system32\") + new IO.FileInfo(dllfile).Name, true);
               }
               Process.Start(sdkpath + "\\Just32bit_register_SDK.bat");
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UsersRole_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.External, "DefaultGateway",
                     new List<Job>
                     {
                        new Job(SendType.External, "DataGuard",
                           new List<Job>
                           {
                              new Job(SendType.External, "SecurityPolicy",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 25 /* Execute DoWork4SettingsAccountChangePassword */),                  
                                    new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 10 /* Execute ActionCallWindows */)
                                 }
                              )
                           }
                        )
                     }
                  )
               }
            )
         );
      }

      private void Redgate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            var redgatepath = parentpath.FullName + "\\tools\\redgate\\SQLToolbeltEssentials.exe";

            Process.Start(redgatepath);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Shutdown_Butn_Click(object sender, EventArgs e)
      {
         Application.Exit();
         Process.GetCurrentProcess().Kill();
      }

      private void IntegrayionSystems_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _IntegrationSystems = 
               new Job(SendType.External, "localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
               {
                  Input = new List<object>
                     {
                        false, // 0- Command requires validation
                        "procedure", // 1- Store procedure or function or command text
                        false, // 2- object value input parameter
                        false, // 3- have result query
                        "sql query", // 4- (DataSet) or (xml string) or (sql query)
                        null, // 5- Dataset or xml string or sql query
                        "{call integrationsystems}", // 6- Store Procedure or function name or command text
                        "iProject", // 7 - Dsn Name
                        "scott" // 8 - User Name
                     }               
               };
            _DefaultGateway.Gateway(_IntegrationSystems);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AnyDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            var redgatepath = parentpath.FullName + "\\tools\\remote\\AnyDesk.exe";

            Process.Start(redgatepath);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
