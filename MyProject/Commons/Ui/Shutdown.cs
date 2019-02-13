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
using System.Diagnostics;
using System.Xml.Linq;

namespace MyProject.Commons.Ui
{
   public partial class Shutdown : UserControl
   {
      public Shutdown()
      {
         InitializeComponent();
      }

      private void Cancl_Butn_Clicked(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Shutdown_Butn_Click(object sender, EventArgs e)
      {
         // 1397/11/24 * اگر که در زمان خروج نیاز به پشتیبان گیری باشد برنامه اتومات پشتیبان گیری انجام میدهد
         Job _BackupAfterShutdown = new Job(SendType.External, "Localhost",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */),
               new Job(SendType.External, "Program","DataGuard", 34 /* Execute DoWork4Backup */, SendType.Self)
               {
                  //Executive = ExecutiveType.Asynchronous, 
                  Input = 
                     new XElement("Backup",
                        new XAttribute("type", "exit")
                     )
               },
            });
         _DefaultGateway.Gateway(_BackupAfterShutdown);

         Application.Exit();
         Process.GetCurrentProcess().Kill();
      }

      private void Logout_Butn_Click(object sender, EventArgs e)
      {
         Job _Logout = new Job(SendType.External, "Localhost",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */),
               new Job(SendType.External, "Program","DataGuard", 10 /* Execute DoWork4TryLogin */, SendType.Self),
               new Job(SendType.External, "Program", "", 03 /* Execute Stop_Service_Component */, SendType.Self)               
            });
         _DefaultGateway.Gateway(_Logout);
      }

      private void Backup_Butn_Click(object sender, EventArgs e)
      {
         Job _ImmediateBackup = new Job(SendType.External, "Localhost",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */),
               new Job(SendType.External, "Program","DataGuard", 34 /* Execute DoWork4Backup */, SendType.Self)
               {
                  Executive = ExecutiveType.Asynchronous, 
                  Input = 
                     new XElement("Backup",
                        new XAttribute("type", "immediate")
                     )
               }               
            });
         _DefaultGateway.Gateway(_ImmediateBackup);

         //Shutdown_Butn_Click(null, null);
         Cancl_Butn_Clicked(null, null);
      }
   }
}
