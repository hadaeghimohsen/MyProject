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

          Program _prg = new Program();
          Job _Startup = new Job(SendType.External, "Program",
             new List<Job>
             {
                new Job(SendType.External, "DataGuard",
                new List<Job>
                {
                   new Job(SendType.Self, 02 /* Execute DoWork4Login */),
                }),
                new Job(SendType.Self, 01 /* Execute Startup*/)                
             });
          _prg.Gateway(_Startup);
       }       
    }
}
