using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Controls;

namespace System.ServiceDefinition.SrvDef.UnderGateways.SrvUnitType.Ui
{
   partial class UnitTypeMenus : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               LoadItemUnitType(job);
               break;
            case 09:
               LoadRoles(job);
               break;
            case 10:
               LoadRolesOfUnitType(job);
               break;
            case 11:
               LoadActiveUnitTypeWithCondition(job);
               break;
            case 12:
               LoadVisibleUnitTypeWithCondition(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Documents\DataGuard\SecPolicy\Role\CreateNewRole.html"
                     }
                  });
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "SUTM", 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         Enabled = true;
         cb_unittypeshow.SelectedIndex = (int)job.Input;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Service",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"ServiceDefinition:Service:UnitType:SUTM", this}},
                  new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */){Input = new List<object>{this , "cntrhrz:normal"}},
              });
         _DefaultGateway.Gateway(_Paint);
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "SUTM", 08 /* Execute LoadItemUnitType */)
            {
               Next = new Job(SendType.SelfToUserInterface, "SUTM", 09 /* Execute LoadRoles */)
            };

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadItemUnitType(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format("<UnitType>{0}</UnitType>",cb_unittypeshow.SelectedIndex),
                              "{ Call ServiceDef.LoadUnitType(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 DataSet ds = output as DataSet;
                                 lv_unittype.Items.Clear();

                                 pn_editgrpsrv.Visible = false;
                                 pn_desc.Enabled = false;
                                 lv_unittype.Enabled = true;
                                 lv_unittype.CheckBoxes = false;
                                 pn_commandgrpsrv.Controls.OfType<Control>().ToList().ForEach(c => c.Enabled = true);
                                 te_rw_f_titlefa.Text = "";

                                 Func<object, int> GetImageIndex = new Func<object, int>(
                                    (isactive) =>
                                    {
                                       return Convert.ToBoolean(isactive) ? 2 : 5;
                                    });

                                 ds.Tables["UnitType"].Rows
                                    .OfType<DataRow>()
                                    .ToList()
                                    .ForEach(item =>
                                    {
                                       var lvi = new ListViewItem(item["TitleFa"].ToString()) { Tag = item["ID"] , ImageIndex = GetImageIndex(item["IsActive"])};
                                       lvi.SubItems.Add(item["UnitTypeDesc"].ToString());
                                       lv_unittype.Items.Add(lvi);
                                    });
                              })
                        }
                     })
               })
          );
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadRoles(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  #region Read All Access Roles
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute ReadAllAccessRoles */)
                        {
                           AfterChangedOutput = 
                              new Action<object>(
                                 (output) =>
                                 {
                                    DataSet ds = output as DataSet;
                                    ccbe_roles.Properties.DataSource = ds.Tables["Roles"];
                                    ccbe_roles.Properties.DisplayMember = "RoleFaName";
                                    ccbe_roles.Properties.ValueMember = "RoleID";
                                    ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(role => role.CheckState = CheckState.Checked);
                                 })
                        }
                     }),
                  #endregion
               }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUnitType(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              job.Input,
                              "{ Call ServiceDef.LoadRolesOfUnitType(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 var roles = ds.Tables["Roles"].Rows.OfType<DataRow>().ToList();
                                 ccbe_roles.Properties.GetItems()
                                    .OfType<CheckedListBoxItem>()
                                    .ToList()
                                    .ForEach(role => 
                                    {
                                       if (roles.Where(r => Convert.ToInt64(r["RoleID"]) == Convert.ToInt64(role.Value)).Count() == 1)
                                          role.CheckState = CheckState.Checked;
                                       else
                                          role.CheckState = CheckState.Unchecked;
                                    });
                              })
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadActiveUnitTypeWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              job.Input,
                              "{ Call ServiceDef.LoadActiveUnitTypeWithCondition(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 Func<object, int> GetImageIndex = new Func<object, int>(
                                    (value) =>
                                    {
                                       return Convert.ToBoolean(value) ? 2 : 5;
                                    });
                                 lv_unittype.Items.Clear();

                                 ds.Tables["UnitType"].Rows
                                    .OfType<DataRow>()
                                    .ToList()
                                    .ForEach(ut =>
                                    {
                                       ListViewItem lvi = new ListViewItem(ut["TitleFa"].ToString()) { Tag = ut["ID"] , ImageIndex = GetImageIndex(ut["IsActive"])};
                                       lvi.SubItems.Add(ut["Desc"].ToString());
                                       lv_unittype.Items.Add(lvi);
                                    });
                              })
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void LoadVisibleUnitTypeWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              job.Input,
                              "{ Call ServiceDef.LoadVisibleUnitTypeWithCondition(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 Func<object, int> GetImageIndex = new Func<object, int>(
                                    (value) =>
                                    {
                                       return Convert.ToBoolean(value) ? 24 : 23;
                                    });
                                 lv_unittype.Items.Clear();

                                 ds.Tables["UnitType"].Rows
                                    .OfType<DataRow>()
                                    .ToList()
                                    .ForEach(ut =>
                                    {
                                       ListViewItem lvi = new ListViewItem(ut["TitleFa"].ToString()) { Tag = ut["ID"] , ImageIndex = GetImageIndex(ut["IsVisible"])};
                                       lvi.SubItems.Add(ut["Desc"].ToString());
                                       lv_unittype.Items.Add(lvi);
                                    });
                              })
                        }
                     })
               }));
      }
   }
}
