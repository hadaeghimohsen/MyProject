using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using Telerik.WinControls.UI;


namespace System.Scsc.Ui.MasterPage
{
   internal partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();
      }

      #region sys menu
      private void rmi_sys_settings_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "userview"))}
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_sys_security_Click(object sender, EventArgs e)
      {

      }

      private void rmi_sys_datasource_Click(object sender, EventArgs e)
      {

      }

      private void rmi_sys_report_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
                 new Job(SendType.External, "Localhost",
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
                                    "<Privilege>1</Privilege><Sub_Sys>4</Sub_Sys>", 
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
                              #region Get Form Reports
                              new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                              {
                                 Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    true,
                                    "xml",
                                    string.Format(@"<Request type=""GetCurrentFormReports"">{0}</Request>", 
                                    new XDocument(
                                       new XElement("Form",
                                       new XAttribute("enname", "FRST_PAGE_F"),
                                       new XElement("Application",
                                          new XAttribute("enname", "System.Scsc"),
                                          new XAttribute("subsys", "5")),
                                       new XElement("Guid", "{140D3745-CCA0-4C59-A2C9-8EC0F132602B}"))
                                    ).ToString()),
                                    "{ CALL Global.GetFormReport(?) }",
                                    "iProject",
                                    "scott"
                                 }
                              },
                              #endregion
                              #region Show Form Settings
                              new Job(SendType.Self, 21 /* Execute DoWork4Form_Stng */){WhereIsInputData = WhereIsInputDataType.StepBack},
                              new Job(SendType.Self, 23 /* Execute DoWork4AFrm_Stng */){Input = 
                                 new XDocument(
                                       new XElement("Form",
                                       new XAttribute("enname", "FRST_PAGE_F"),
                                       new XElement("Application",
                                          new XAttribute("enname", "System.Scsc"),
                                          new XAttribute("subsys", "5")),
                                       new XElement("Guid", "{140D3745-CCA0-4C59-A2C9-8EC0F132602B}"))
                                    )}
                              #endregion
                           }
                        ){GenerateInputData = GenerateDataType.Dynamic}
                    })
               );
      }

      private void rmi_sys_fullscreennormal_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface,"Wall", 20 /* Execute FullScreenOrNormal */) 
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_sys_backup_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "BackupRestore"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sys_reloadreports_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                     {                        
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {                              
                              #region Get Form Reports
                              new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                              {
                                 Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    true,
                                    "xml",
                                    string.Format(@"<Request type=""GetCurrentFormReports"">{0}</Request>", 
                                    new XDocument(
                                       new XElement("Form",
                                       new XAttribute("enname", "FRST_PAGE_F"),
                                       new XElement("Application",
                                          new XAttribute("enname", "System.Scsc"),
                                          new XAttribute("subsys", "5")),
                                       new XElement("Guid", "{140D3745-CCA0-4C59-A2C9-8EC0F132602B}"))
                                    ).ToString()),
                                    "{ CALL Global.GetFormReport(?) }",
                                    "iProject",
                                    "scott"
                                 },
                                 AfterChangedOutput = new Action<object>(
                                    output => 
                                    {
                                       foreach (DataRow item in (output as DataSet).Tables["Form_Report"].Rows)
                                       {
                                          RadMenuItem rt = new RadMenuItem() { Tag = item["RWNO"], Text = item["RPRT_DESC"].ToString() };
                                          rt.Click += GotoPrint;
                                          rmi_rpt.Items.Add(rt);
                                       }
                                    })
                              },
                              #endregion
                           }
                        ){GenerateInputData = GenerateDataType.Dynamic}
                    })
               );
      }

      private void rmi_sys_quit_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region bas menu
      #region regl sub menu
      private void rmi_bas_regl_createexpense_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>3</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 03 /* Execute Mstr_Regl_F */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 10 /* Actn_CalF_P */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_regl_createaccount_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>3</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 03 /* Execute Mstr_Regl_F */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 10 /* Actn_CalF_P */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_regl_expenseaccount_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>2</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 03 /* Execute Mstr_Regl_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_regl_currentexpense_Click(object sender, EventArgs e)
      {
         var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
         if (Rg1 == null) return;

         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}}
                  })
            );
      }

      private void rmi_bas_regl_incdecexpense_Click(object sender, EventArgs e)
      {
         var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
         if (Rg1 == null) return;

         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 05 /* Execute Regl_Expn_F */){Input = new List<Data.Regulation>{Rg1, null}}
                  })
            );
      }

      private void rmi_bas_regl_createcash_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "cash"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_regl_createexpenseitem_Click(object sender, EventArgs e)
      {
         #region Old
         //         Job _InteractWithScsc =
         //            new Job(SendType.External, "Localhost",
         //               new List<Job>
         //               {
         //                  new Job(SendType.External, "Commons",
         //                     new List<Job>
         //                     {
         //                        #region Access Privilege
         //                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
         //                        {
         //                           Input = new List<string> 
         //                           {
         //                              "<Privilege>37</Privilege><Sub_Sys>5</Sub_Sys>", 
         //                              "DataGuard"
         //                           },
         //                           AfterChangedOutput = new Action<object>((output) => {
         //                              if ((bool)output)
         //                                 return;
         //                              #region Show Error
         //                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
         //                              {
         //                                 Input = @"<HTML>
         //                                             <body>
         //                                                <p style=""float:right"">
         //                                                   <ol>
         //                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
         //                                                      <ul>
         //                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
         //                                                      </ul>
         //                                                   </ol>
         //                                                </p>
         //                                             </body>
         //                                             </HTML>"
         //                              };
         //                              _DefaultGateway.Gateway(_ShowError);
         //                              #endregion                           
         //                           })
         //                        },
         //                        #endregion
         //                     }),
         //                  #region DoWork
         //                  new Job(SendType.Self, 10 /* Execute Mstr_Epit_F */)
         //                  #endregion

         //                  });
         //         _DefaultGateway.Gateway(_InteractWithScsc);
         #endregion
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "epit"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_organ_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>171</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا: عدم دسترسی به کد 171");
                              #endregion                           
                           })
                        },
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>175</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا: عدم دسترسی به کد 175");
                              #endregion                           
                           })
                        }
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 108 /* Execute Orgn_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 10 /* Actn_CalF_P */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region cal sub menu
      private void rmi_bas_cal_coachdegree_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 68 /* Execute Cal_Expn_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_cal_coach_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 68 /* Execute Cal_Expn_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      private void rmi_bas_club_Click(object sender, EventArgs e)
      {
         #region Old
//         Job _InteractWithScsc =
//            new Job(SendType.External, "Localhost",
//               new List<Job>
//               {
//                  new Job(SendType.External, "Commons",
//                     new List<Job>
//                     {
//                        #region Access Privilege
//                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
//                        {
//                           Input = new List<string> 
//                           {
//                              "<Privilege>41</Privilege><Sub_Sys>5</Sub_Sys>", 
//                              "DataGuard"
//                           },
//                           AfterChangedOutput = new Action<object>((output) => {
//                              if ((bool)output)
//                                 return;
//                              #region Show Error
//                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
//                              {
//                                 Input = @"<HTML>
//                                             <body>
//                                                <p style=""float:right"">
//                                                   <ol>
//                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
//                                                      <ul>
//                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
//                                                      </ul>
//                                                   </ol>
//                                                </p>
//                                             </body>
//                                             </HTML>"
//                              };
//                              _DefaultGateway.Gateway(_ShowError);
//                              #endregion                           
//                           })
//                        },
//                        #endregion
//                     }),
//                  #region DoWork
//                  new Job(SendType.Self, 11 /* Execute Mstr_Club_F */)
//                  #endregion

//                  });
//         _DefaultGateway.Gateway(_InteractWithScsc);
         #endregion
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "regn"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_method_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>17</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 08 /* Execute Mstr_Mtod_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_requestertype_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>61</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 14 /* Execute Rqtr_Type_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_requestype_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>54</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 13 /* Execute Rqst_Type_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_mainsubstat_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>47</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                     }),
                  #region DoWork
                  new Job(SendType.Self, 12 /* Execute Main_Subs_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_bas_regions_Click(object sender, EventArgs e)
      {
         #region old
//         Job _InteractWithScsc =
//            new Job(SendType.External, "Localhost",
//               new List<Job>
//               {
//                  new Job(SendType.External, "Commons",
//                     new List<Job>
//                     {
//                        #region Access Privilege
//                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
//                        {
//                           Input = new List<string> 
//                           {
//                              "<Privilege>27</Privilege><Sub_Sys>5</Sub_Sys>", 
//                              "DataGuard"
//                           },
//                           AfterChangedOutput = new Action<object>((output) => {
//                              if ((bool)output)
//                                 return;
//                              #region Show Error
//                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
//                              {
//                                 Input = @"<HTML>
//                                             <body>
//                                                <p style=""float:right"">
//                                                   <ol>
//                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
//                                                      <ul>
//                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
//                                                      </ul>
//                                                   </ol>
//                                                </p>
//                                             </body>
//                                             </HTML>"
//                              };
//                              _DefaultGateway.Gateway(_ShowError);
//                              #endregion                           
//                           })
//                        },
//                        #endregion
//                     }),
//                  #region DoWork
//                  new Job(SendType.Self, 09 /* Execute Mstr_Regn_F */)
//                  #endregion

//                  });
//         _DefaultGateway.Gateway(_InteractWithScsc);
         #endregion
         //Job _InteractWithScsc =
         //  new Job(SendType.External, "Localhost",
         //     new List<Job>
         //      {
         //         new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //         new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "regn"))}
         //      });
         //_DefaultGateway.Gateway(_InteractWithScsc);
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 98 /* Execute Bas_Cpr_F */),
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_bas_cluboptionstool_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_004"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_bas_fingerprintreserved_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 100 /* Execute Fngr_Rsvd_F */),
                  new Job(SendType.SelfToUserInterface, "FNGR_RSVD_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_bas_bodymove_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 120 /* Execute Bsc_Bmov_F */),
                  new Job(SendType.SelfToUserInterface, "BSC_BMOV_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region adm menu
      private void rmi_adm_createfighter_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_createcoach_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "coach"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_renewcontract_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }      

      private void rmi_adm_guestfighter_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                     //new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Payment", new XAttribute("type", "Out"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_delete_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 82 /* Execute Adm_Ends_F */),
                  //new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_edit_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_recycle_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 83 /* Execute Adm_Dsen_F */),
                  //new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_listallfighter_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */){Input = x}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_changmethod_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 75 /* Execute Cmc_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_com_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 78 /* Execute Com_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_mcc_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 79 /* Execute Mcc_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_adm_ins_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 80 /* Execute Ins_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region pay menu
      private void rmi_pay_cashincome_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 67 /* Execute Pay_Cash_F */),
                  new Job(SendType.SelfToUserInterface, "PAY_CASH_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Pay_Cash_F", new XAttribute("type", "CashInCome"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_pay_cashoutcome_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 67 /* Execute Pay_Cash_F */),
                  new Job(SendType.SelfToUserInterface, "PAY_CASH_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Pay_Cash_F", new XAttribute("type", "CashOutCome"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_pay_income_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 87 /* Execute Mng_Rcan_F */){Input = new XElement("Payment", new XAttribute("type", "In"))},
                  new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Payment", new XAttribute("type", "In"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_pay_outcome_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 87 /* Execute Mng_Rcan_F */){Input = new XElement("Payment", new XAttribute("type", "Out"))},
                  new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Payment", new XAttribute("type", "Out"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_pay_misincome_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                  //new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Payment", new XAttribute("type", "Out"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_pay_changerials_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 113 /* Execute Glr_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "GLR_TOTL_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region exp menu
      private void rmi_exp_calculatecoachexpense_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_exp_othersexpense_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_exp_refunds_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 111 /* Execute Rfd_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "RFD_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      private void rt_exp_showexpense_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 90 /* Execute Cal_Cexc_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion

      #region Others
      private void GotoPrint(object sender, EventArgs e)
      {
         RadMenuItem rt = sender as RadMenuItem;

         #region Get Current User Name And Call GetAccessReports Procedure
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                        {
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 #region Prepare Execute Procedure
                                 _DefaultGateway.Gateway(
                                    new Job(SendType.External, "Localhost",
                                       new List<Job>
                                       {
                                          new Job(SendType.External, "Commons",
                                             new List<Job>
                                             {
                                                new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                                                {
                                                   #region Execute Procedure
                                                   Input = new List<object>
                                                   {
                                                      false,
                                                      "procedure",
                                                      true,
                                                      true,
                                                      "xml",
                                                      string.Format(@"<Request type=""GetReportRwno""><User>{0}</User><Report rwno=""{2}""/>{1}</Request>", output.ToString(), 
                                                      new XDocument(
                                                         new XElement("Form",
                                                         new XAttribute("enname", "FRST_PAGE_F"),
                                                         new XElement("Application",
                                                            new XAttribute("enname", "System.Scsc"),
                                                            new XAttribute("subsys", "5")),
                                                         new XElement("Guid", "{140D3745-CCA0-4C59-A2C9-8EC0F132602B}"))
                                                      ).ToString(),
                                                      rt.Tag),
                                                      "{ Call Global.[GetFormReport](?) }",
                                                      "iProject",
                                                      "scott"
                                                   }, 
                                                   AfterChangedOutput = new Action<object>(
                                                      (xoutput) =>
                                                         {  
                                                            try
                                                            {
                                                               DataSet ds = xoutput as DataSet;
                                                               _DefaultGateway.Gateway(
                                                                  new Job(SendType.External, "Localhost", "DefaultGateway:Reporting:WorkFlow", 05 /* DoWork4WHR_SCON_F */, SendType.Self)
                                                                  {
                                                                     Input = XDocument.Parse(ds.Tables["RunReport"].Rows[0][0].ToString().Replace("<RunReports>", string.Empty).Replace("</RunReports>", string.Empty))
                                                                  }
                                                               );
                                                            }
                                                            catch{}
                                                         })
                                                   #endregion
                                                }
                                             })
                                       }));                                       
                                 #endregion
                              })
                        }
                     })
               }));
         #endregion
      }
      #endregion

      #region Test
      private void rmi_tst_test_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 76 /* Execute Tst_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_tst_comp_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 77 /* Execute Cmp_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_tst_exam_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 71 /* Execute Exm_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_tst_psf_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 72 /* Execute Psf_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_tst_clc_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 73 /* Execute Clc_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rmi_tst_hrz_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 74 /* Execute Hrt_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region Notification
      private void rt_club_endfigh_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endfigh"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_club_attn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_mtod_endfigh_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endmtod"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void tr_ins_endfigh_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endinsr"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_club_remnsesn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endsesn"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_club_message_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 116 /* Execute Msgb_Totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_club_debts_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 117 /* Execute Debt_List_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion

      #region Reports
      private void rt_income_report_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>194</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 194 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 89 /* Execute Rpt_List_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_outcome_report_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>195</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 195 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 89 /* Execute Rpt_List_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_regulation_report_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>196</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 196 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 89 /* Execute Rpt_List_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_003"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_figters_report_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>197</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 197 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 89 /* Execute Rpt_List_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_004"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_income_statistics_Click(object sender, EventArgs e)
      {
         //Privilege 198
      }

      private void rt_outcome_statistics_Click(object sender, EventArgs e)
      {
         // Privilege 199
      }

      private void rt_fighter_statistics_Click(object sender, EventArgs e)
      {
         // Privilege 200
      }

      private void rt_rpt_accounting_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
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
                              "<Privilege>193</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 193 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                     #region DoWork
                  new Job(SendType.Self, 91 /* Execute Rpt_List_F */),
                  new Job(SendType.SelfToUserInterface, "ACN_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
         //Job _InteractWithScsc =
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.Self, 91 /* Execute Rpt_List_F */),
         //         new Job(SendType.SelfToUserInterface, "ACN_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
         //      });
         //_DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_rpt_rqsttrac_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 109 /* Execute Rqst_Trac_F */),
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion     

      #region short adm menu
      private void rt_sesn_saveontime_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 95 /* Execute Oic_Srzh_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SRZH_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_rentobjlist_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 95 /* Execute Oic_Srzh_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SRZH_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }


      private void rt_sesn_saveonetime_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 96 /* Execute Oic_Sone_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SONE_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_oinc_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 96 /* Execute Oic_Sone_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SONE_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_odec_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 96 /* Execute Oic_Sone_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SONE_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_003"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_olistclass_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 96 /* Execute Oic_Sone_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SONE_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_005"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_savemoretime_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 97 /* Execute Oic_Smor_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMOR_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_minc_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 97 /* Execute Oic_Smor_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMOR_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_mdec_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 97 /* Execute Oic_Smor_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMOR_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_003"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_mlistclass_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 97 /* Execute Oic_Smor_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMOR_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_005"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_multicoach_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_mcinc_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_mcdec_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_003"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_sesn_mclistclass_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_005"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion

      #region BarCode
      void Start_BarCode()
      {
         try
         {
            if(iScsc.Settings.Count(s => Fga_Uclb_U.Contains(s.CLUB_CODE)) > 1)
            {
               Tsp_AttnSys.Text = "***";
               return;
            }

            var barCodeSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();

            if (barCodeSetting == null) return;
            
            if (barCodeSetting.ATTN_SYST_TYPE != "001") return;

            Sp_Barcode.PortName = barCodeSetting.COMM_PORT_NAME;
            Sp_Barcode.BaudRate = (int)barCodeSetting.BAND_RATE;
            Sp_Barcode.Open();

            if (Sp_Barcode.IsOpen)
               Tsp_AttnSys.Text = "سیستم بارکد خوان فعال";
            else
               Tsp_AttnSys.Text = "سیستم بارکد خوان غیرفعال";
         }
         catch 
         {
            //MessageBox.Show(ex.Message);
            Tsp_AttnSys.Text = "سیستم بارکد خوان غیرفعال";
         }
      }

      void Stop_BarCode()
      {
         try
         {
            if (Sp_Barcode.IsOpen)
            {
               Sp_Barcode.Close();
               Tsp_AttnSys.Text = "سیستم بارکد خوان غیرفعال";
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Sp_Barcode_DataReceived(object sender, IO.Ports.SerialDataReceivedEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("barcodedata", Sp_Barcode.ReadExisting()))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region Finger Print
      public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
      bool bIsConnected = false;
      void Start_FingerPrint()
      {
         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
               {
                  AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     var host = output as XElement;

                     if(iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CONCT == host.Attribute("cpu").Value ))
                     {
                        var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CONCT == host.Attribute("cpu").Value).FirstOrDefault();

                        if (fingerPrintSetting == null) return;

                        if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                        if (fingerPrintSetting.IP_ADDR != null && fingerPrintSetting.PORT_NUMB != null)
                        {
                           Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                           Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                           if (!bIsConnected)
                           {
                              bIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                              // fire event for fetch 
                              axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                           }
                           if (bIsConnected == true)
                           {
                              Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                              Tsp_AttnSys.ForeColor = Color.Green;
                              int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                              axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                           }
                           else
                           {
                              Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                              Tsp_AttnSys.ForeColor = Color.Red;
                           }  
                        }
                     }
                     else
                     {
                        Tsp_AttnSys.Text = "***";
                     }
                  })
               }
            );
      }

      //private string enrollnumber;
      //private DateTime enrolldate;
      private void axCZKEM1_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
      {
         /*
          * در این قسمت بعد از اینکه کاربر درون دستگاه تعریف شد باید برای اولین بار در سیستم عمل ثبت نام صورت پذیرد
          * این حالت اولیه زمانی رخ میدهد که ما هیچ هنرجویی با این شماره اثر انگشت درون سیستم تعریف نشده باشد
          */
         //EnrollNumber = Microsof2t.VisualBasic.Interaction.InputBox("لطفا کد کاربری خود را وارد کنید");
         //if (enrollnumber == EnrollNumber && enrolldate.AddSeconds(5) <= DateTime.Now) { enrollnumber = ""; return; }
         //else { enrollnumber = EnrollNumber; enrolldate = DateTime.Now; }
         //ExtCode.ScreenSaver.KillScreenSaver();

         if (
            !iScsc.Fighters
            .Any(f => 
               //Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) && 
               f.FNGR_PRNT_DNRM == EnrollNumber && 
               Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 &&
               !(f.FGPB_TYPE_DNRM == "002" && f.FGPB_TYPE_DNRM == "003") 
               //(Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&                              
            )
         )
         {
            var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == EnrollNumber);

            if (figh != null &&  Convert.ToInt32(figh.ACTV_TAG_DNRM ?? "101") <= 100)
            {
               if (MessageBox.Show(this, "هنرجو مورد نظر در حالت حذف از سیستم قرار گرفته است. مایل به فعال کردن مجدد هنرجو هستید؟", "حضور مجدد هنرجوی غیرفعال", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               {
                  // این قسمت برنامه باید با واحد مربوطه انتقال یابد که پراکندگی کد وجود نداشته باشد
                  #region Disable To Enabled
                  iScsc.AET_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", 0),
                           new XAttribute("rqtpcode", "014"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO),
                              new XElement("Fighter_Public",
                                 new XElement("Actv_Tag", "101")
                              )
                           )
                        )
                     )
                  );

                  var Rqst = iScsc.Requests.FirstOrDefault(r => r.Request_Rows.Any(rr => rr.FIGH_FILE_NO == figh.FILE_NO) && r.RQTP_CODE == "014" && r.RQTT_CODE == "004");

                  iScsc.AET_SAVE_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqst.RQID),
                           new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                           new XAttribute("regncode", Rqst.REGN_CODE),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );
                  #endregion
               }
               else
                  return;
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                     new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("enrollnumber", EnrollNumber),
                           new XAttribute("isnewenroll", true)
                        )
                     }
                  })
            );
         }
         else
         {
            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", EnrollNumber))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }

         return;
      }

      void Stop_FingerPrint()
      {
         if (bIsConnected)
         {
            axCZKEM1.Disconnect();
            bIsConnected = false;            
            Tsp_AttnSys.Text = "دستگاه حضورغیاب غیرفعال شد";
         }
      }
      #endregion

      private void Tm_FingerPrintWorker_Tick(object sender, EventArgs e)
      {
         Start_FingerPrint();
         Tm_FingerPrintWorker.Enabled = false; ;
      }

      #region BodyFitness
      private void rt_bdfpros_Click(object sender, EventArgs e)
      {
         //Job _InteractWithScsc =
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {                    
         //         new Job(SendType.Self, 119 /* Execute Bdf_Pros_F */)
         //      });
         //_DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void rt_bdfnormal_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 119 /* Execute Bdf_Pros_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      private void AopMbspF_Butn_Click(object sender, EventArgs e)
      {
         MasterRbbv.HidePopup();
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 121 /* Execute Aop_Mbsp_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }      
   }
}
