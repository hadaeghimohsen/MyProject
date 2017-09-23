using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.Globalization;

namespace MyProject.Commons.Desktop.Ui
{
   public partial class Desktop : UserControl
   {
      public Desktop()
      {
         InitializeComponent();
      }

      #region Completed
      private void sb_shutdown_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }

      private void sb_logout_Click(object sender, EventArgs e)
      {
         Job _Logout = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Desktop", 04 /* Execute UnPaint */),
               new Job(SendType.External, "Commons","Program:DataGuard", 10 /* Execute DoWork4TryLogin */, SendType.Self),
               new Job(SendType.External, "Commons", "Program", 03 /* Execute Stop_Service_Component */, SendType.Self)               
            });
         _DefaultGateway.Gateway(_Logout);
      }
      #endregion

      private void Secu_Man_ItemClick(object sender, EventArgs e)
      {
         Job _ShowSecuritySettings = new Job(SendType.External, "Desktop", "Commons:Program:DataGuard", 03 /* Execute DoWork4SecuritySettings */, SendType.Self);
         _DefaultGateway.Gateway(_ShowSecuritySettings);
      }

      private void Serv_Def_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithServiceDef =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>1</Privilege><Sub_Sys>1</Sub_Sys>", "DataGuard"},
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "ServiceDefinition", 02 /* Execute DoWork4InteractWithServiceDef */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithServiceDef);
      }

      private void Rprt_App_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithServiceDef =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>2</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "Reporting:WorkFlow", 02 /* Execute DoWork4RPT_SRPT_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithServiceDef);
      }

      private void Bill_App_ItemClick(object sender, TileItemEventArgs e)
      {
         Job _InteractWithGas =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>3</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "Gas", 02 /* Execute DoWork4STRT_MTRO_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithGas);
      }

      private void Scsc_App_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "Scsc", 122 /* Execute Main_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Sas_App_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>6</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "Sas", 02 /* Execute Mstr_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void LNK_SMS_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>7</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "Msgb", 02 /* Execute Mstr_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void sb_startdrawer_Click(object sender, EventArgs e)
      {
         Job _GetToggleMode =
            new Job(SendType.External, "Program", "Wall", 21 /* Execute GetToggleStat */, SendType.SelfToUserInterface);

         _DefaultGateway.Gateway(_GetToggleMode);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Program", "Commons:Desktop", 03 /* Execute DoWork4StartDrawer */, SendType.Self) { Input = _GetToggleMode.Output }
         );
      }

      private void LNK_ISP_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>10</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "ISP", 02 /* Execute Frst_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void LNK_CRM_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>11</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "CRM", 02 /* Execute Frst_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void LNK_ROBOTECH_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Desktop",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>12</Sub_Sys>", 
                              "DataGuard"
                           },
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
                        #endregion
                        #region DoWork
                        new Job(SendType.External, "Program", "RoboTech", 02 /* Execute Frst_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Tm_ShowTime_Tick(object sender, EventArgs e)
      {
         PersianCalendar pc = new PersianCalendar();
         AdjustDateTime_Butn.Text =
            string.Format("{0}\n\r{1}/{2}/{3}",
               DateTime.Now.ToString("HH:mm:ss"),
               pc.GetYear(DateTime.Now),
               pc.GetMonth(DateTime.Now),
               pc.GetDayOfMonth(DateTime.Now));
      }

      private void AdjustDateTime_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 26 /* Execute DoWork4DateTimes */, SendType.Self));
      }

      private void StartMenu_Butn_Click(object sender, EventArgs e)
      {
         Job _GetToggleMode =
            new Job(SendType.External, "Program", "Wall", 21 /* Execute GetToggleStat */, SendType.SelfToUserInterface);

         _DefaultGateway.Gateway(_GetToggleMode);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Program", "Commons:Desktop", 05 /* Execute DoWork4StartMenu */, SendType.Self) { Input = _GetToggleMode.Output }
         );
      }
   }
}
