using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Controls;
using System.Xml.Linq;

namespace System.ServiceDefinition.GrpHdr.Ui
{
   partial class Update : ISendRequest
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
               new Job(SendType.SelfToUserInterface, "Update", 04 /* Execute UnPaint */);
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
         string value = job.Input as string;
         te_titlefa.Tag = XElement.Parse(value).Element("GroupHeaderID").Value;
         te_titlefa.Text = XElement.Parse(value).Element("GhFaName").Value;
         te_titleen.Text = XElement.Parse(value).Element("GhEnName").Value;         
         cb_ghactive.Checked = Convert.ToBoolean(XElement.Parse(value).Element("GhActive").Value);
         RoleID = Convert.ToInt64(XElement.Parse(value).Element("RoleID").Value);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "GroupHeader",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"ServiceDefinition:GroupHeader:Update", this}},
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
            new Job(SendType.SelfToUserInterface, "Update", 08 /* LoadRolesOfUser */);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUser(Job job)
      {
         Job _LoadRoleOfUser =
            new Job(SendType.External, "GroupHeader",
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
                                    ccbe_roles.Properties.GetItems()
                                       .OfType<CheckedListBoxItem>()
                                       .ToList()
                                       .ForEach(role => 
                                          {                                             
                                             role.CheckState = Convert.ToInt64(role.Value) == RoleID ? CheckState.Checked : CheckState.Unchecked;
                                          });
                                 })
                        }
                     })
                  #endregion
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
         job.Status = StatusType.Successful;
      }
   }
}
