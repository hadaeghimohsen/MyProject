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

namespace System.Reporting.ReportUnitType.Ui
{
   partial class SpecifyAppDecision : ISendRequest
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
               LoadRolesOfUser(job);
               break;
            case 09:
               LoadUnitOfRoles(job);
               break;
            case 10:
               LoadWhatsFileType(job);
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
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "SpecifyAppDecision", 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "DefaultGateway",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportUnitType:SpecifyAppDecision", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
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
            new Job(SendType.SelfToUserInterface, "SpecifyAppDecision", 08 /* LoadRolesOfUser */);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUser(Job job)
      {
         Job _LoadRoleOfUser =
            new Job(SendType.External, "DefaultGateway",
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
                                    cb_roles.DataSource = ds.Tables["Roles"];
                                    cb_roles.DisplayMember = "RoleFaName";
                                    cb_roles.ValueMember = "RoleID";                                    
                                 })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "SpecifyAppDecision", 09 /* Exective LoadUnitOfRoles */)
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadUnitOfRoles(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "DefaultGateway",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 14 /* Execute ReadUnitTypeOfRoles*/)
                        {
                           Input = string.Format("<RoleID>{0}</RoleID><UnitType>0</UnitType><IsLinked>{1}</IsLinked>", cb_roles.SelectedValue, cb_islinked.Checked),
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 DataSet ds = output as DataSet;
                                 ccbe_servunit.Properties.DataSource = ds.Tables["UnitType"];
                                 ccbe_servunit.Properties.DisplayMember = "TitleFa";
                                 ccbe_servunit.Properties.ValueMember = "UnitTypeid";
                                 //ccbe_servunit.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(s => s.CheckState = CheckState.Checked);
                              })
                        }
                     }),
               }));
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadWhatsFileType(Job job)
      {
         ccbe_servunit.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(ft => ft.CheckState = CheckState.Unchecked);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
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
                           string.Format("<FileType><RoleID>{0}</RoleID>{1}</FileType>", cb_roles.SelectedValue, job.Input),
                           "{ Call Report.WhatsFileType(?) }",
                           "iProject",
                           "scott"
                        }, 
                        AfterChangedOutput = new Action<object>(
                           (output) => 
                           {
                              DataSet ds = output as DataSet;
                              ds.Tables["FileType"].Rows.OfType<DataRow>().ToList().ForEach(ft =>
                                 {
                                    ccbe_servunit.Properties.GetItems().OfType<CheckedListBoxItem>()
                                       .Where(su => Convert.ToInt64(su.Value) == Convert.ToInt64(ft["Uid"]))
                                       .ToList()
                                       .ForEach(sft => sft.CheckState = CheckState.Checked);
                                 });
                                 
                           })
                     }
                  })
               }));
      }

   }
}
