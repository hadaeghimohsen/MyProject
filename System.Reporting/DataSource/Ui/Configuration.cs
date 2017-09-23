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

namespace System.Reporting.DataSource.Ui
{
   public partial class Configuration : UserControl
   {
      public Configuration()
      {
         InitializeComponent();
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "Service", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "Service", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }


      string rootTag;
      string functionCall;
      string privilege;
      Func<string> xmlData;

      #region Command Master
      private void sb_import2odbc_Click(object sender, EventArgs e)
      {
         switch (lv_datasources.CheckBoxes)
         {
            case false:
               pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);
               lv_datasources.CheckBoxes = true;               
               break;
            case true:
               #region Precondition
               if (lv_datasources.Items.OfType<ListViewItem>().Where(ds => ds.Checked).Count() == 0)
               {
                  pn_CommandMaster.Controls.OfType<Control>().Where(c => !(c.Enabled) ).ToList().ForEach(c => c.Enabled = true);
                  lv_datasources.CheckBoxes = false;
                  return;
               }
               #endregion
               #region Xml Tag
               rootTag = "<ConnectionStrings>{0}</ConnectionStrings>";
               functionCall = "Report.LoadConnectionStrings";
               privilege = "<Privilege>9</Privilege><Sub_Sys>2</Sub_Sys>";

               xmlData = new Func<string>(
               () =>
               {
                  string temp = "";
                  lv_datasources.Items
                     .OfType<ListViewItem>()
                     .Where(ds => ds.Checked)
                     .ToList()
                     .ForEach(ds =>
                     {
                        temp += string.Format("<DataSourceID>{0}</DataSourceID>", ds.Tag);
                     });
                  return temp ;
               });
               #endregion
               #region PostCondition
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Datasource",
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
                                    privilege,
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Error Fire
                                    Job _ShowError = new Job(SendType.External, "Datasource",
                                       new List<Job>
                                       {
                                          new Job(SendType.External, "Commons",
                                             new List<Job>
                                             {
                                                new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                                {
                                                   Input = new List<object>
                                                   {
                                                      "Not Imp",
                                                      new Action(() => 
                                                      {
                                                         _DefaultGateway.Gateway(new Job(SendType.External, "Datasource", "Configuration", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
                                                      })
                                                   }
                                                }
                                             })
                                       });
                                    _DefaultGateway.Gateway(_ShowError);
                                    #endregion
                                 })
                              },
                              #endregion
                              #region DoWork
                              new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                              {
                                 Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    true,
                                    "xml",
                                    string.Format(rootTag, xmlData()),
                                    string.Format("{{ Call {0}(?) }}", functionCall),
                                    "iProject",
                                    "scott"
                                 }
                              },
                              #endregion                        
                              #region Import2Odbc
                              new Job(SendType.Self, 13 /* Execute DoWork4Import2Odbc */)
                              {
                                 WhereIsInputData = WhereIsInputDataType.StepBack
                              },
                              #endregion
                           }){GenerateInputData = GenerateDataType.Dynamic},
                        new Job(SendType.SelfToUserInterface, "Configuration", 07 /* Execute LoadData */)
                     }));
               #endregion
               break;
         }
      }

      private void sb_createcstr_Click(object sender, EventArgs e)
      {
         rootTag = "<Create>{0}</Create>";
         functionCall = "Report.CreateNewConnectionString";
         privilege = "<Privilege>3</Privilege><Sub_Sys>2</Sub_Sys>";
         pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);
         gb_connectionstring.Visible = true;
         te_rw_f_ip.Focus();

         xmlData = new Func<string>(
            () =>
            {
               string temp = "";
               gb_connectionstring.Controls
                  .OfType<TextEdit>()
                  .ToList()
                  .ForEach(info =>
                  {
                     temp += string.Format("<{0}>{1}</{0}>", info.Tag, info.EditValue);
                  });
               return temp + string.Format("<DataBaseServer>{0}</DataBaseServer>", cb_databaseserver.SelectedIndex);
            });
      }

      private void sb_updatecstr_Click(object sender, EventArgs e)
      {
         if (lv_datasources.Items.Count == 0 || lv_datasources.SelectedItems.Count == 0)
            return;

         rootTag = "<Update>{0}</Update>";
         functionCall = "Report.UpdateConnectionString";
         privilege = "<Privilege>4</Privilege><Sub_Sys>2</Sub_Sys>";
         pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);

         xmlData = new Func<string>(
            () =>
            {
               string temp = "";
               gb_connectionstring.Controls
                  .OfType<TextEdit>()
                  .ToList()
                  .ForEach(info =>
                  {
                     temp += string.Format("<{0}>{1}</{0}>", info.Tag, info.EditValue);
                  });
               return temp + string.Format("<DataSourceID>{0}</DataSourceID><DataBaseServer>{1}</DataBaseServer>", lv_datasources.SelectedItems[0].Tag, cb_databaseserver.SelectedIndex);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource", "Configuration", 09 /* Execute LoadSingleDataSource */ , SendType.SelfToUserInterface){Input = string.Format("<DataSourceID>{0}</DataSourceID>", lv_datasources.SelectedItems[0].Tag)}
               );

      }

      private void sb_testexistscstr_Click(object sender, EventArgs e)
      {

      }

      private void sb_restorecstr_Click(object sender, EventArgs e)
      {
         rootTag = "<Restore>{0}</Restore>";
         functionCall = "Report.RestoreDataSource";
         privilege = "<Privilege>5</Privilege><Sub_Sys>2</Sub_Sys>";
         pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);
         pn_editcstr.Visible = true;

         xmlData = new Func<string>(
            () =>
            {
               string temp = "";
               lv_datasources.Items.OfType<ListViewItem>().Where(dbs => dbs.Checked).ToList().ForEach(dbs => temp += string.Format("<DataSourceID>{0}</DataSourceID>",dbs.Tag));
               return temp;
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource", "Configuration", 10 /* Execute LoadReminderDataSourceWithCondition */ , SendType.SelfToUserInterface) { Input = "<IsVisible>false</IsVisible>" }
               );
      }

      private void sb_removecstr_Click(object sender, EventArgs e)
      {
         rootTag = "<Remove>{0}</Remove>";
         functionCall = "Report.RemoveDataSource";
         privilege = "<Privilege>6</Privilege><Sub_Sys>2</Sub_Sys>";
         pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);
         pn_editcstr.Visible = true;

         xmlData = new Func<string>(
            () =>
            {
               string temp = "";
               lv_datasources.Items.OfType<ListViewItem>().Where(dbs => dbs.Checked).ToList().ForEach(dbs => temp += string.Format("<DataSourceID>{0}</DataSourceID>", dbs.Tag));
               return temp;
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource", "Configuration", 10 /* Execute LoadReminderDataSourceWithCondition */ , SendType.SelfToUserInterface) { Input = "<IsVisible>true</IsVisible>" }
               );
      }

      private void sb_deactivecstr_Click(object sender, EventArgs e)
      {
         rootTag = "<Deactive>{0}</Deactive>";
         functionCall = "Report.DeactiveDataSource";
         privilege = "<Privilege>7</Privilege><Sub_Sys>2</Sub_Sys>";
         pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);
         pn_editcstr.Visible = true;

         xmlData = new Func<string>(
            () =>
            {
               string temp = "";
               lv_datasources.Items.OfType<ListViewItem>().Where(dbs => dbs.Checked).ToList().ForEach(dbs => temp += string.Format("<DataSourceID>{0}</DataSourceID>", dbs.Tag));
               return temp;
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource", "Configuration", 10 /* Execute LoadReminderDataSourceWithCondition */ , SendType.SelfToUserInterface) { Input = "<IsActive>true</IsActive>" }
               );
      }

      private void sb_activecstr_Click(object sender, EventArgs e)
      {
         rootTag = "<Active>{0}</Active>";
         functionCall = "Report.ActiveDataSource";
         privilege = "<Privilege>8</Privilege><Sub_Sys>2</Sub_Sys>";
         pn_CommandMaster.Controls.OfType<Control>().Where(c => (Control)sender != c).ToList().ForEach(c => c.Enabled = false);
         pn_editcstr.Visible = true;

         xmlData = new Func<string>(
            () =>
            {
               string temp = "";
               lv_datasources.Items.OfType<ListViewItem>().Where(dbs => dbs.Checked).ToList().ForEach(dbs => temp += string.Format("<DataSourceID>{0}</DataSourceID>", dbs.Tag));
               return temp;
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource", "Configuration", 10 /* Execute LoadReminderDataSourceWithCondition */ , SendType.SelfToUserInterface) { Input = "<IsActive>false</IsActive>" }
               );
      }
      #endregion

      #region Edit Master
      private void sb_testnewcstr_Click(object sender, EventArgs e)
      {

      }

      private void sb_save_Click(object sender, EventArgs e)
      {
         if (gb_connectionstring.Controls.OfType<TextEdit>().Where(te => te.Name.Contains("_f_") && string.IsNullOrEmpty(te.Text)).Count() >= 1)
         {
            gb_connectionstring.Controls.OfType<TextEdit>().Where(te => te.Name.Contains("_f_") && string.IsNullOrEmpty(te.Text)).First().Focus();
            return;
         }

         _DefaultGateway.Gateway(
              new Job(SendType.External, "Datasource",
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
                                 privilege,
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 #region Error Fire
                                 Job _ShowError = new Job(SendType.External, "Datasource",
                                    new List<Job>
                                    {
                                       new Job(SendType.External, "Commons",
                                          new List<Job>
                                          {
                                             new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                             {
                                                Input = new List<object>
                                                {
                                                   "Not Imp",
                                                   new Action(() => 
                                                   {
                                                      _DefaultGateway.Gateway(new Job(SendType.External, "Datasource", "Configuration", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
                                                   })
                                                }
                                             }
                                          })
                                    });
                                 _DefaultGateway.Gateway(_ShowError);
                                 #endregion
                              })
                           },
                           #endregion
                           #region DoWork
                           new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                           {
                              Input = new List<object>
                              {
                                 false,
                                 "procedure",
                                 true,
                                 false,
                                 "xml",
                                 string.Format(rootTag, xmlData()),
                                 string.Format("{{ Call {0}(?) }}", functionCall),
                                 "iProject",
                                 "scott"
                              }
                           },
                           #endregion                        
                        }),
                     new Job(SendType.SelfToUserInterface, "Configuration", 07 /* Execute LoadData */)
                  }));
      }

      private void sb_cancel_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource",
               new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "Configuration", 07 /* Execute LoadData */)
                  }));
      }

      private void sb_retype_Click(object sender, EventArgs e)
      {
         gb_connectionstring.Controls.OfType<TextEdit>().Where(c => c.Name.Contains("_rw_")).ToList().ForEach(c => c.Text = "");
      }
      #endregion

      #region Command Connection String
      private void sb_rollback_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource",
               new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "Configuration", 07 /* Execute LoadData */)
                  }));
      }

      private void sb_commit_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Datasource",
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
                                 privilege,
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 #region Error Fire
                                 Job _ShowError = new Job(SendType.External, "Datasource",
                                    new List<Job>
                                    {
                                       new Job(SendType.External, "Commons",
                                          new List<Job>
                                          {
                                             new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                             {
                                                Input = new List<object>
                                                {
                                                   "Not Imp",
                                                   new Action(() => 
                                                   {
                                                      _DefaultGateway.Gateway(new Job(SendType.External, "Datasource", "Configuration", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
                                                   })
                                                }
                                             }
                                          })
                                    });
                                 _DefaultGateway.Gateway(_ShowError);
                                 #endregion
                              })
                           },
                           #endregion
                           #region DoWork
                           new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                           {
                              Input = new List<object>
                              {
                                 false,
                                 "procedure",
                                 true,
                                 false,
                                 "xml",
                                 string.Format(rootTag, xmlData()),
                                 string.Format("{{ Call {0}(?) }}", functionCall),
                                 "iProject",
                                 "scott"
                              }
                           }
                           #endregion                        
                        }),
                     new Job(SendType.SelfToUserInterface, "Configuration", 07 /* Execute LoadData */)
                  }));
      }

      private void sb_deselect_Click(object sender, EventArgs e)
      {
         lv_datasources.Items.OfType<ListViewItem>().ToList().ForEach(dbs => dbs.Checked = false);
      }

      private void sb_selectinvert_Click(object sender, EventArgs e)
      {
         lv_datasources.Items.OfType<ListViewItem>().ToList().ForEach(dbs => dbs.Checked = !dbs.Checked);
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         lv_datasources.Items.OfType<ListViewItem>().ToList().ForEach(dbs => dbs.Checked = true);
      }
      #endregion

      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "Datasource",
               new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                              })
                        });
         _DefaultGateway.Gateway(_RoleSettings4CurrentUser);
      }
   }
}
