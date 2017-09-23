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

namespace System.DataGuard.Self.Ui
{
   public partial class Main : UserControl
   {
      public Main()
      {
         InitializeComponent();
      }

      private void sb_securitysettings_Click(object sender, EventArgs e)
      {
         Job _ShowSecuritySettings = new Job(SendType.External, "DataGuard",
            new List<Job>
            {
               #region Access Privilege
               new Job(SendType.External, "Commons",
                     new List<Job>
                     {                        
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>2</Privilege>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },                        
                     }),
               #endregion
               new Job(SendType.External,"SecurityPolicy", "", 03 /* Execute DoWork4ControlPanel */, SendType.Self)
               //new Job(SendType.External,"SecurityPolicy", "", 02 /* Execute DoWork4SecuritySettings */, SendType.Self)
            });            
         _DefaultGateway.Gateway(_ShowSecuritySettings);
      }

   }
}
