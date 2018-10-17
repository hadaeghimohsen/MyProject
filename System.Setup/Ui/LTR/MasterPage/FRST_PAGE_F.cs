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

namespace System.Setup.Ui.LTR.MasterPage
{
   public partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();
      }

      private string formCaller;

      private void Execute_Query()
      {
      }

      private void NewSqlServerInstance_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //MessageBox.Show(execpath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            //MessageBox.Show(parentpath.FullName);
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>{
                     new Job(SendType.External, "Commons",
                        new List<Job>{
                           new Job(SendType.Self, 36 /* Execute DoWork4GetWindowsPlatform */)
                           {
                              AfterChangedOutput = 
                                 new Action<object>(
                                    (output) => 
                                       {
                                          var sqlserverpath = parentpath.FullName + "\\tools\\sqlserver\\" + output.ToString() + "\\SQLEXPRWT_x64_ENU.exe";
                                          sqlserverpath = @"D:\iData\Utility\MicrosoftComponents\Sql Server Collection\Express\64\setup.exe /CONFIGURATIONFILE=ConfigurationFile.ini";
                                          Process.Start(sqlserverpath);//, configfile);                                          
                                       }
                                 )
                           }
                        }
                     )
                  }
               )
            );
            
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
   }
}
