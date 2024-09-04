using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Threading;
using System.Globalization;
using System.IO;

namespace MyProject.Programs.Code
{
    internal partial class Program
    {
       public Program()
       {
          _Wall = new Ui.Wall { _DefaultGateway = this };
          _readyToWork = false;

          _Commons = new Commons.Code.Commons(_Wall) { _DefaultGateway = this };
          _DataGuard = new System.DataGuard.Self.Code.DataGuard(_Commons, _Wall) { _DefaultGateway = this };
          _Setup = new System.Setup.Code.Setup(_Commons, _Wall) { _DefaultGateway = this };
       }

       private bool _readyToWork;

       private string _errorForNotInstallDll = "هسته راه اندازی و امنیتی سیستم قادر به بارگذاری و تنظیم کردن سطوح دسترسی نرم افزار نمی باشد. لطفا با پشیبانی تماس بگیرید";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       [STAThread]
       static void Main()
       {
          //Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR") ;
          //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
          //Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
          
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);

          // 1403/06/12
          bool _createdNew;
          Mutex _app = new Mutex(false, "UniTech", out _createdNew);
          if (!_createdNew)
          {
             Application.Exit();
             return;
          }

          Program _prg = new Program();
          #region Check iProject Dsn Name ODBC
          // 1397/07/24 * بررسی اینکه آیا راه ارتباطی در 
          // ODBC 
          // ثبت شده است یا خیر
          
          var checkDsniProject = StatusType.Failed;
          _prg.Gateway(
             new Job(SendType.External, "Program",
                new List<Job>
                {
                   new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.External, "Odbc",
                           new List<Job>
                           {
                              new Job(SendType.Self, 08 /* Execute DsnNameExists */){ Input = "iProject", AfterChangedStatus = new Action<StatusType>((s) => checkDsniProject = s) }
                           }
                        )
                     }
                  )
                }
             )             
          );

          if (checkDsniProject == StatusType.Failed)
          {
             _prg.Gateway(
                new Job(SendType.External, "Program",
                   new List<Job>
                   {
                     new Job(SendType.External, "Setup",
                        new List<Job>
                        {
                           //new Job(SendType.Self, 03 /* Execute Chk_Licn_F */)
                           //new Job(SendType.Self, 02 /* Execute Frst_Page_F */)
                           new Job(SendType.Self, 05 /* Execute Chk_Tiny_F */)
                        }
                     ),
                     new Job(SendType.Self, 01 /* Execute Startup*/)
                   }
                )
             );
          }
          #endregion
          else
          {
             Job _Startup = new Job(SendType.External, "Program",
                new List<Job>
                {
                   new Job(SendType.External, "DataGuard",
                   new List<Job>
                   {
                      new Job(SendType.Self, 02 /* Execute DoWork4Login */),
                   }),                
                   new Job(SendType.Self, 01 /* Execute Startup*/),
                });
             _prg.Gateway(_Startup);
          }          
       }
    }
}
