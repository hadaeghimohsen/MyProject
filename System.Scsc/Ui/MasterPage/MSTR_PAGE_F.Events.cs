using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.MasterPage
{
   partial class MSTR_PAGE_F
   {
      #region اطلاعات پایه سیستم
      partial void Btn_Regulation_Click(object sender, EventArgs e)
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

      partial void Btn_Region_Click(object sender, EventArgs e)
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
                              "<Privilege>27</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                  new Job(SendType.Self, 09 /* Execute Mstr_Regn_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Btn_Method_Click(object sender, EventArgs e)
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
                  new Job(SendType.Self, 08 /* Execute Mstr_Regl_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Btn_Club_Click(object sender, EventArgs e)
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
                              "<Privilege>41</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                  new Job(SendType.Self, 11 /* Execute Mstr_Club_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Btn_MainSubState_Click(object sender, EventArgs e)
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

      partial void Btn_RequestType_Click(object sender, EventArgs e)
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

      partial void Btn_RequesterType_Click(object sender, EventArgs e)
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
      #endregion

      #region
      private void Execute_Query()
      {
         requestTypeBindingSource.DataSource = iScsc.Request_Types;
         requesterTypeBindingSource.DataSource = iScsc.Requester_Types;
         dSUBBindingSource.DataSource = iScsc.D_SUBs;
         vFCashierBindingSource.DataSource = iScsc.VF_Cashiers;
         /*
         vF_AndRequestResultBindingSource.DataSource =
            iScsc.VF_AndRequest(
               new XElement("Process",
                  new XElement("Condition",
                     new XAttribute("subsys", ""),
                     new XAttribute("rqid", ""),
                     new XAttribute("rqtpcode", ""),
                     new XAttribute("cretby", ""),
                     new XAttribute("cretdate", ""),
                     new XAttribute("mdfyby", ""),
                     new XAttribute("mdfydate", "")
                  )
               )
            );
         vF_InComeResultBindingSource.DataSource = iScsc.VF_InCome(null, null, null, null);
         vF_All_Debt_FighterResultBindingSource.DataSource = iScsc.VF_All_Debt_Fighter();
         vF_InsuranceFighterResultBindingSource.DataSource = iScsc.VF_InsuranceFighter((Convert.ToDateTime(Pd_CrntDate.EditValue ?? DateTime.Now)).Date, Convert.ToInt16(Rb_Figh1.Checked));
          */
      }
      #endregion

      partial void TSM_AdmRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>65</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 65 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 15 /* Execute Adm_Rqst_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_AdmSumF_Click(object sender, EventArgs e)
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
                              "<Privilege>66</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 66 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 16 /* Execute Adm_FSum_F */){Input = x}
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_AdmSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>67</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 67 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 17 /* Execute Adm_Sexp_F */){Input = x}
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_AdmRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>68</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 68 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 18 /* Execute Adm_Rexp_F */){Input = x}
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_AdmFinalRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>70</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 70 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 19 /* Execute Adm_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_AttnSave_Click(object sender, EventArgs e)
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
                              "<Privilege>71</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 71 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 20 /* Execute Adm_Save_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_TstRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>74</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 74 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 21 /* Execute Tst_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_TestSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>75</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 75 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 22 /* Execute Tst_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_TstRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>76</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 76 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 23 /* Execute Tst_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_TstSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>77</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 77 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 24 /* Execute Tst_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmpRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>78</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 78 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 25 /* Execute Cmp_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmpSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>79</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 79 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 26 /* Execute Cmp_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmpReceiveF_Click(object sender, EventArgs e)
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
                              "<Privilege>80</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 80 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 27 /* Execute Cmp_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmpSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>81</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 81 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 28 /* Execute Cmp_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_PsfRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>82</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 82 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 29 /* Execute Psf_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_PsfSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>83</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 83 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 30 /* Execute Psf_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_PsfRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>84</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 84 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 31 /* Execute Psf_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_PsfSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>85</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 85 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 32 /* Execute Psf_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ClcRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>86</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 86 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 33 /* Execute Clc_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ClcSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>87</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 87 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 34 /* Execute Clc_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ClcRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>88</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 88 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 35 /* Execute Clc_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ClcSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>89</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 89 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 36 /* Execute Clc_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_HertRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>90</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 90 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 37 /* Execute Hrz_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_HertSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>91</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 91 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 38 /* Execute Hrz_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_HertRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>92</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 92 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 39 /* Execute Hrz_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_HertSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>93</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 93 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 40 /* Execute Hrz_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ExamRequestF_Click(object sender, EventArgs e)
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
                              "<Privilege>94</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 94 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 41 /* Execute Exm_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ExamSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>95</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 95 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 42 /* Execute Exm_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ExamRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>96</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 96 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 43 /* Execute Exm_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_ExamSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>97</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 97 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 44 /* Execute Exm_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Tsm_Folder_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */){Input = x}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_FgPbRqstF_Click(object sender, EventArgs e)
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
                              "<Privilege>98</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 98 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 47 /* Execute Pbl_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_FgPbSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>99</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 99 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 48 /* Execute Pbl_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_FgPbRecieveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>100</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 100 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 49 /* Execute Pbl_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_FgPbSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>101</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 101 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 50 /* Execute Pbl_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmcRqstF_Click(object sender, EventArgs e)
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
                              "<Privilege>102</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 102 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 51 /* Execute Cmc_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmcSendExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>103</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 103 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 52 /* Execute Cmc_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmcReceiveExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>104</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 104 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 53 /* Execute Cmc_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_CmcSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>105</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 105 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 54 /* Execute Cmc_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void BBI_INVSRQST_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         x = 
            iScsc.INVS_RQST_U(
               new XElement("Process",
                  new XElement("Request", 
                     new XAttribute("rqid", (vF_AndRequestResultBindingSource.Current as Data.VF_AndRequestResult).RQID)
                  )
               )
            );
         
         /* Check Condition For Go Next Form */
         if (x.Descendants("NextForm").Attributes("mtodnumb").First().Value == "-1")
         {
            MessageBox.Show(x.Descendants("NextForm").Attributes("mtoddesc").First().Value);
            return;
         }

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "MSTR_PAGE_F", 08 /* Execute Goto_NextForm */, SendType.SelfToUserInterface) { Input = x }
         );
      }

      partial void BBI_Requery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /*string subsys, rqid, rqtpcode, cretby, mdfyby;
         DateTime cretdate, mdfydate;
         subsys = (dSUBBindingSource.Current as Data.D_SUB).VALU;*/
         if ((BEI_Switch.EditValue == null) || BEI_Switch.EditValue.ToString() == "False")
         {
            vF_AndRequestResultBindingSource.DataSource =
            iScsc.VF_AndRequest(
               new XElement("Process",
                  new XElement("Condition",
                     new XAttribute("subsys", BEI_SUBSYS.EditValue ?? ""),
                     new XAttribute("rqid", BEI_RQID.EditValue ?? ""),
                     new XAttribute("rqtpcode", BEI_RQTPCODE.EditValue ?? ""),
                     new XAttribute("cretby", BEI_CRETBY.EditValue ?? ""),
                     new XAttribute("cretdate", BEI_CRETDATE.EditValue == null ? "" : Convert.ToDateTime(BEI_CRETDATE.EditValue).ToString("yyyy-MM-dd")),
                     new XAttribute("mdfyby", BEI_MDFYBY.EditValue ?? ""),
                     new XAttribute("mdfydate", BEI_MDFYDATE.EditValue == null ? "" : Convert.ToDateTime(BEI_MDFYDATE.EditValue).ToString("yyyy-MM-dd"))
                  )
               )
            );
         }
         else
         {
            vF_AndRequestResultBindingSource.DataSource =
            iScsc.VF_OrRequest(
               new XElement("Process",
                  new XElement("Condition",
                     new XAttribute("subsys", BEI_SUBSYS.EditValue ?? ""),
                     new XAttribute("rqid", BEI_RQID.EditValue ?? ""),
                     new XAttribute("rqtpcode", BEI_RQTPCODE.EditValue ?? ""),
                     new XAttribute("cretby", BEI_CRETBY.EditValue ?? ""),
                     new XAttribute("cretdate", BEI_CRETDATE.EditValue == null ? "" : Convert.ToDateTime(BEI_CRETDATE.EditValue).ToString("yyyy-MM-dd")),
                     new XAttribute("mdfyby", BEI_MDFYBY.EditValue ?? ""),
                     new XAttribute("mdfydate", BEI_MDFYDATE.EditValue == null ? "" : Convert.ToDateTime(BEI_MDFYDATE.EditValue).ToString("yyyy-MM-dd"))
                  )
               )
            );
         }
      }

      partial void TSM_UCCRqstF_Click(object sender, EventArgs e)
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
                              "<Privilege>106</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 106 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 55 /* Execute Ucc_Rqst_F */)
                  #endregion
                  });
          _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_UCCSExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>107</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 107 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 56 /* Execute Ucc_Sexp_F */){Input = x}
                  #endregion
                  });
          _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_UCCRExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>108</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 108 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 57 /* Execute Ucc_Rexp_F */){Input = x}
                  #endregion
                  });
          _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_UCCSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>109</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 109 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 58 /* Execute Ucc_Save_F */){Input = x}
                  #endregion
                  });
          _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Rb_Figh1_CheckedChanged(object sender, EventArgs e)
      {
         vF_InsuranceFighterResultBindingSource.DataSource = iScsc.VF_InsuranceFighter((Convert.ToDateTime(Pd_CrntDate.EditValue ?? DateTime.Now)).Date, Convert.ToInt16(Rb_Figh1.Checked));
      }

      partial void Pd_CrntDate_EditValueChanged(object sender, EventArgs e)
      {
         vF_InsuranceFighterResultBindingSource.DataSource = iScsc.VF_InsuranceFighter((Convert.ToDateTime(Pd_CrntDate.EditValue ?? DateTime.Now)).Date, Convert.ToInt16(Rb_Figh1.Checked));
      }

      partial void Bt_InComeQueryWithParm_Click(object sender, EventArgs e)
      {
         vF_InComeResultBindingSource.DataSource = iScsc.VF_InCome(Convert.ToDateTime(Pd_FromDate.EditValue), Convert.ToDateTime(Pd_ToDate.EditValue), Convert.ToString(LOV_RQTPCODE.EditValue) , Convert.ToString(LOV_RQTTCODE.EditValue));
         vF_All_Debt_FighterResultBindingSource.DataSource = iScsc.VF_All_Debt_Fighter();
      }

      partial void Bt_InComeQueryWithoutParm_Click(object sender, EventArgs e)
      {
         Pd_FromDate.EditValue = Pd_ToDate.EditValue = LOV_RQTPCODE.EditValue = LOV_RQTTCODE.EditValue = null;
         vF_InComeResultBindingSource.DataSource = iScsc.VF_InCome(null, null, null, null);
         vF_All_Debt_FighterResultBindingSource.DataSource = iScsc.VF_All_Debt_Fighter();
      }

      partial void TSM_MCCRqstF_Click(object sender, EventArgs e)
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
                              "<Privilege>110</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 110 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 60 /* Execute Mcc_Rqst_F */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_MCCSExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>111</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 111 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 61 /* Execute Mcc_Sexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_MCCRExpnF_Click(object sender, EventArgs e)
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
                              "<Privilege>112</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 112 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 62 /* Execute Mcc_Rexp_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void TSM_MCCSaveF_Click(object sender, EventArgs e)
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
                              "<Privilege>113</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show(this, "دسترسی به ردیف 113 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 63 /* Execute Mcc_Save_F */){Input = x}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Btn_CrntCashPayment_Click(object sender, EventArgs e)
      {
         try
         {
            //Te_CashPayment.EditValue = iScsc.VF_Payment_Delivers(new XElement("Process", new XElement("Payment", new XAttribute("fromdate", Convert.ToDateTime(Pdt_CashPaymentFromDate.EditValue).ToString("yyyy-MM-dd")), new XAttribute("todate", Convert.ToDateTime(Pdt_CashPaymentToDate.EditValue).ToString("yyyy-MM-dd")), new XAttribute("delvstat", "001")))).Sum(p => p.EXPN_PRIC) ?? 0;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      partial void Btn_PayDelv_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از بستن صندوق و تحویل درآمد صندوق مطمدن هستید؟", "بستن صندوق", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         
         /*
         if (Cb_CloseCrntCashDateNow.Checked || Pdt_CashPaymentFromDate.DateTime.ToString("yyyy-MM-dd") == "0001-01-01" && MessageBox.Show(this, "تاریخ شروع درآمد صندوق مشخص نشده میخواهید تاریخ امروز قرار گیرد؟", "تاریخ شروع بستن صندوق", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            Pdt_CashPaymentFromDate.DateTime = DateTime.Now;
         else
            Pdt_CashPaymentFromDate.EditValue = DateTime.Now.AddDays(-1);

         if (Cb_CloseCrntCashDateNow.Checked || Pdt_CashPaymentToDate.DateTime.ToString("yyyy-MM-dd") == "0001-01-01" && MessageBox.Show(this, "تاریخ پایان درآمد صندوق مشخص نشده میخواهید تاریخ امروز قرار گیرد؟", "تاریخ پایان بستن صندوق", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            Pdt_CashPaymentToDate.DateTime = DateTime.Now;
         else
            Pdt_CashPaymentToDate.DateTime = DateTime.MaxValue;
         */

         try
         {
            iScsc.PAY_DELV_P(
               new XElement("Process", 
                  new XElement("Payment", 
                     new XAttribute("cashby", CashBy_LookUpEdit.EditValue)
                  )
               )
            );
            MessageBox.Show(this, "درآمد صندوق تحویل داده شده");
            requery = true;            
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Execute_Query();
               requery = false;
            }
         }
      }

      partial void CashBy_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         if (CashBy_LookUpEdit.EditValue != null)
         {
            try
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
                                    "<Privilege>117</Privilege><Sub_Sys>5</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    MessageBox.Show(this, "دسترسی به ردیف 117 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                                 })
                              },
                              #endregion
                           })
                        });
               _DefaultGateway.Gateway(_InteractWithScsc);
               /*if(_InteractWithScsc.Status == StatusType.Successful)
                  Te_CashPayment.EditValue = iScsc.VF_Payment_Delivers(new XElement("Process", new XElement("Payment", new XAttribute("fromdate", Convert.ToDateTime(Pdt_CashPaymentFromDate.EditValue).ToString("yyyy-MM-dd")), new XAttribute("todate", Convert.ToDateTime(Pdt_CashPaymentToDate.EditValue).ToString("yyyy-MM-dd")), new XAttribute("delvstat", "001"), new XAttribute("cashby", CashBy_LookUpEdit.EditValue)))).Sum(p => p.EXPN_PRIC) ?? 0;
               */
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
         }
      }

      partial void Btn_AdmProccess_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

   }
}
