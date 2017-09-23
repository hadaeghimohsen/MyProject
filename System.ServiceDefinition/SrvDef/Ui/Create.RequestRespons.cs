using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data;

namespace System.ServiceDefinition.SrvDef.Ui
{
   partial class Create : ISendRequest
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
               LoadType(job);
               break;
            case 09:
               LoadUnit(job);
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
               new Job(SendType.SelfToUserInterface, "Create", 04 /* Execute UnPaint */);
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
         Controls.OfType<Control>().Where(c => c.Name.Contains("_rw_")).ToList().ForEach(c => c.Text = "");
         List<object> value = job.Input as List<object>;
         /*
          * 0 := Level
          * 1 := SFaName
          * 2 := ServiceID
          * 3 := <Level= {0 ,1}>
          * 4 := GroupHeadersComboBox
          */
         if(Convert.ToInt16(value[0]) == 1)
         {
            cb_servicetype.Enabled = sb_loadservicetype.Enabled = sb_defservicetype.Enabled = false;
            cb_serviceunit.Enabled = sb_loadserviceunit.Enabled = sb_defserviceunit.Enabled = false;
         }
         else
         {
            cb_servicetype.Enabled = sb_loadservicetype.Enabled = sb_defservicetype.Enabled = true;
            cb_serviceunit.Enabled = sb_loadserviceunit.Enabled = sb_defserviceunit.Enabled = true;
         }
         sb_save.Tag = value[0];
         te_parent.Text = value[1].ToString();
         te_parent.Tag = value[2].ToString();
         te_level.Text = ((int)value[0]) == 0 ? "تعریف خدمت جدید" : "تعریف گروه خدمت جدید";
         te_level.Tag = value[0];
         lb_title.Text = te_level.Text;
         ccbe_f_groupheaders.Properties.DataSource = (value[3] as CheckedComboBoxEdit).Properties.DataSource;
         ccbe_f_groupheaders.Properties.DisplayMember = (value[3] as CheckedComboBoxEdit).Properties.DisplayMember;
         ccbe_f_groupheaders.Properties.ValueMember = (value[3] as CheckedComboBoxEdit).Properties.ValueMember;         
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
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"ServiceDefinition:Service:Create", this}},
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
            new Job(SendType.SelfToUserInterface, "Create", 08 /* LoadServiceType */)
            {
               Next = new Job(SendType.SelfToUserInterface, "Create", 09 /* LoadServiceUnit */)
            };
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadType(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Service",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region DoWork4Odbc
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              false,
                              true,
                              "",
                              "",
                              "{ Call ServiceDef.LoadType }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 cb_servicetype.DataSource = ds;
                                 cb_servicetype.DisplayMember = "ServiceType.TitleFa";
                                 cb_servicetype.ValueMember = "ServiceType.ID";
                              })
                        }
                        #endregion
                     })
               }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadUnit(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Service",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region DoWork4Odbc
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              false,
                              true,
                              "",
                              "",
                              "{ Call ServiceDef.LoadUnit }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 cb_serviceunit.DataSource = ds;
                                 cb_serviceunit.DisplayMember = "ServiceUnit.TitleFa";
                                 cb_serviceunit.ValueMember = "ServiceUnit.ID";
                              })
                        }
                        #endregion
                     })
               }));
         job.Status = StatusType.Successful;
      }

   }
}
