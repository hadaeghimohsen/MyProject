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

namespace MyProject.Commons.LifeTime.Ui
{
   public partial class ToolOperation : UserControl
   {
      public ToolOperation()
      {
         InitializeComponent();
      }

      private Action AfterSelectedAccepted;
      private string CommandName;
      private string XMLParameter;
      private string DsnName;
      private string UserName;
      private string RedoLog;
      private List<string> AccessPrivilege;

      private void btn_accept_Click(object sender, EventArgs e)
      {        

         Job _DoWork4Accept = new Job(SendType.External, "ToolOperation",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = AccessPrivilege,                        
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;

                           Job _ShowError = new Job(SendType.External, "ToolOperation", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                           {
                              Input = @".\Documents\DataGuard\SecPolicy\Privilege\AccessPrivilege.Error.html"
                           };
                           _DefaultGateway.Gateway(_ShowError);
                        })
                     },
                     #endregion
                     #region DoWork
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */){
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           false,
                           "xml",
                           XMLParameter,
                           string.Format("{{ Call {0}(?) }}", CommandName),
                           DsnName,
                           UserName
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint", RedoLog}}
                     #endregion
                  }),

                  new Job (SendType.SelfToUserInterface, "ToolOperation", 04 /* Execute UnPaint */),
                  new Job (SendType.SelfToUserInterface, "ToolOperation", 08 /* Execute Finalization */)
            });
         _DefaultGateway.Gateway(_DoWork4Accept);
      }

      private void btn_cancel_Click(object sender, EventArgs e)
      {

      }
   }
}
