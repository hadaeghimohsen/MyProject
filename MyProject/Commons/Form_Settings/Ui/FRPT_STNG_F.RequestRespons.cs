using System;
using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MyProject.Commons.Form_Settings.Ui
{
   partial class FRPT_STNG_F : ISendRequest
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
               LoadRolesOfProfilers(job);
               break;
            case 09:
               LoadRolesOfReports(job);
               break;
            case 10:
               LoadProfilersOfRole(job);
               break;
            case 11:
               LoadReportsOfRole(job);
               break;
            case 12:
               LoadFristData(job);
               break;
            case 13:
               LoadAppForm(job);
               break;
            default:
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
               new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 04 /* Execute UnPaint */);
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
         ds_form_report = job.Input as DataSet;
         gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Commons:FORM_STNG:FRPT_STNG_F", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

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
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 08 /* Execute LoadRolesOfProfilers */),
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 09 /* Execute LoadRolesOfReports */),
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 12 /* Execute LoadFristData */)
               })
         );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfProfilers(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>(
                  (output1) =>
                  {
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost", "DefaultGateway", 04 /* Execute DoWork4Odbc */, SendType.Self)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format(@"<Request type=""ROLES""><User>{0}</User></Request>", output1),
                              "{ CALL [Report].[GetAccessProfilers](?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output2) =>
                              {
                                 if (InvokeRequired)
                                    Invoke(new Action(() =>
                                    {
                                       Lov_ROLE_PROF.DataSource = (output2 as DataSet).Tables["Roles"];
                                       Lov_ROLE_PROF.DisplayMember = "ROLE_NAME";
                                       Lov_ROLE_PROF.ValueMember = "ROLE_ID";
                                       /*****************/
                                       //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                    }));
                                 else
                                 {
                                    Lov_ROLE_PROF.DataSource = (output2 as DataSet).Tables["Roles"];
                                    Lov_ROLE_PROF.DisplayMember = "ROLE_NAME";
                                    Lov_ROLE_PROF.ValueMember = "ROLE_ID";
                                    /*****************/
                                    //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                 }
                              }
                           )
                        }
                     );
                  }
               )

            }
         );
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfReports(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>(
                  (output1) =>
                  {
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost", "DefaultGateway", 04 /* Execute DoWork4Odbc */, SendType.Self)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format(@"<Request type=""ROLES""><User>{0}</User></Request>", output1),
                              "{ CALL [Report].[GetAccessReports](?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output2) =>
                              {
                                 if (InvokeRequired)
                                    Invoke(new Action(() =>
                                    {
                                       Lov_ROLE_SERV.DataSource = (output2 as DataSet).Tables["Roles"];
                                       Lov_ROLE_SERV.DisplayMember = "ROLE_NAME";
                                       Lov_ROLE_SERV.ValueMember = "ROLE_ID";
                                       /*****************/
                                       //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                    }));
                                 else
                                 {
                                    Lov_ROLE_SERV.DataSource = (output2 as DataSet).Tables["Roles"];
                                    Lov_ROLE_SERV.DisplayMember = "ROLE_NAME";
                                    Lov_ROLE_SERV.ValueMember = "ROLE_ID";
                                    /*****************/
                                    //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                 }
                              }
                           )
                        }
                     );
                  }
               )}
         );
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadProfilersOfRole(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>(
                  (output1) =>
                  {
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost", "DefaultGateway", 04 /* Execute DoWork4Odbc */, SendType.Self)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format(@"<Request type=""USER-ROLES""><User>{0}</User><Role>{1}</Role></Request>", output1, job.Input),
                              "{ CALL [Report].[GetAccessProfilers](?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output2) =>
                              {
                                 if (InvokeRequired)
                                    Invoke(new Action(() =>
                                    {
                                       Lov_PROF.DataSource = (output2 as DataSet).Tables["Profilers"];
                                       Lov_PROF.DisplayMember = "PROF_NAME";
                                       Lov_PROF.ValueMember = "PROF_ID";
                                       /*****************/
                                       //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                    }));
                                 else
                                 {
                                    Lov_PROF.DataSource = (output2 as DataSet).Tables["Profilers"];
                                    Lov_PROF.DisplayMember = "PROF_NAME";
                                    Lov_PROF.ValueMember = "PROF_ID";
                                    /*****************/
                                    //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                 }
                              }
                           )}
                     );
                  }
               )}
         );
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadReportsOfRole(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>(
                  (output1) =>
                  {
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost", "DefaultGateway", 04 /* Execute DoWork4Odbc */, SendType.Self)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format(@"<Request type=""USER-ROLES""><User>{0}</User><Role>{1}</Role></Request>", output1, job.Input),
                              "{ CALL [Report].[GetAccessReports](?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output2) =>
                              {
                                 if (InvokeRequired)
                                    Invoke(new Action(() =>
                                    {
                                       Lov_SERV.DataSource = (output2 as DataSet).Tables["Reports"];
                                       Lov_SERV.DisplayMember = "SERV_NAME";
                                       Lov_SERV.ValueMember = "SERV_ID";
                                       /*****************/
                                       //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                    }));
                                 else
                                 {
                                    Lov_SERV.DataSource = (output2 as DataSet).Tables["Reports"];
                                    Lov_SERV.DisplayMember = "SERV_NAME";
                                    Lov_SERV.ValueMember = "SERV_ID";
                                    /*****************/
                                    //gc_frpt_stng.DataSource = ds_form_report.Tables["Form_Report"];                                       
                                 }
                              }
                           )
                        }
                     );
                  }
               )
            }
         );
         job.Status = StatusType.Successful;
      }
      
      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void LoadFristData(Job job)
      {
         if (ds_form_report.Tables["Form_Report"].Rows.Count == 0) return;

         foreach (DataRow item in ds_form_report.Tables["Form_Report"].Rows)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "FRPT_STNG_F", 10 /* Execute LoadProfilersOfRoles */, SendType.SelfToUserInterface) { Input = item["PROF_ROLE_ID"] }
            );

            _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "FRPT_STNG_F", 11 /* Execute LoadReportsOfRoles */, SendType.SelfToUserInterface) { Input = item["SERV_ROLE_ID"] }
         );

         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void LoadAppForm(Job job)
      {
         xdoc = job.Input as XDocument;
         job.Status = StatusType.Successful;
      }
   }
}
