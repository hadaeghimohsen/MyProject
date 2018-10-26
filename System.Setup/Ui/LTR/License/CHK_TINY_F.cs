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
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace System.Setup.Ui.LTR.License
{
   public partial class CHK_TINY_F : UserControl
   {
      public CHK_TINY_F()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         Application.Exit();
         Process.GetCurrentProcess().Kill();
      }

      private void Connect_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (TinySNInstaller_Txt.Text == "") { TinySNInstaller_Txt.Focus(); return; }

            // Install Tiny SDK
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            var tinypath = parentpath.FullName + "\\tools\\tinysdk";
            tinypath = @"D:\AnarSys\tools\tinysdk";
            if (Environment.Is64BitOperatingSystem)
            {               
               foreach (var dllfile in IO.Directory.GetFiles(tinypath).Where(f => f.Contains(".ocx")))
               {
                  IO.File.Copy(dllfile, Environment.ExpandEnvironmentVariables(@"%windir%\syswow64\") + new IO.FileInfo(dllfile).Name, true);
               }
               Process.Start(tinypath + "\\Just64bit_register_TinySDK.bat");
            }
            else
            {
               foreach (var dllfile in IO.Directory.GetFiles(tinypath).Where(f => f.Contains(".ocx")))
               {
                  IO.File.Copy(dllfile, Environment.ExpandEnvironmentVariables(@"%windir%\system32\") + new IO.FileInfo(dllfile).Name, true);
               }
               Process.Start(tinypath + "\\Just32bit_register_TinySDK.bat");
            }

            // Check Input Tiny Serial No With Tiny Lock
            var _CheckInstallTinyLock =
               new Job(SendType.External, "localhost", "DefaultGateway:DataGuard", 32 /* Execute DoWork4CheckInstallTinyLock */, SendType.Self) { Input = TinySNInstaller_Txt.Text };
            _DefaultGateway.Gateway(_CheckInstallTinyLock);

            if (_CheckInstallTinyLock.Output != null)
            {
               var _jobUnSecureHashCode =
                  new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 08 /* Execute DoWork4UnSecureHashCode  */, SendType.Self) { Input = (_CheckInstallTinyLock.Output as XElement).Value };
               _DefaultGateway.Gateway(_jobUnSecureHashCode);
               MessageBox.Show(_jobUnSecureHashCode.Output.ToString());
            }
            else
            {
               // Open First Page Setup
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute Frst_Page_F */)
                     }
                  )
               );
            }
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
