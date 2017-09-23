using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Xml.Linq;

namespace MyProject.Commons.Form_Settings.Ui
{
   partial class FRPT_STNG_F
   {
      partial void gv_Lov_Role_Prof_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
      {
         if (Lov_ROLE_PROF.GetKeyValue(e.RowHandle) == null)
            return;

         ds_form_report.Tables["Form_Report"].Rows[gv_form_report.GetFocusedDataSourceRowIndex()]["PROF_ROLE_ID"] = Lov_ROLE_PROF.GetKeyValue(e.RowHandle);
         /* Other wise query List Of Profilers of Role */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "FRPT_STNG_F", 10 /* Execute LoadProfilersOfRoles */, SendType.SelfToUserInterface) { Input = Lov_ROLE_PROF.GetKeyValue(e.RowHandle) }
         );
      }

      partial void gv_Lov_Role_Serv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
      {
         if (Lov_ROLE_SERV.GetKeyValue(e.RowHandle) == null)
            return;

         ds_form_report.Tables["Form_Report"].Rows[gv_form_report.GetFocusedDataSourceRowIndex()]["SERV_ROLE_ID"] = Lov_ROLE_SERV.GetKeyValue(e.RowHandle);
         /* Other wise query List Of Reports of Role */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "FRPT_STNG_F", 11 /* Execute LoadReportsOfRoles */, SendType.SelfToUserInterface) { Input = Lov_ROLE_SERV.GetKeyValue(e.RowHandle) }
         );
      }

      partial void b_commit_Click(object sender, EventArgs e)
      {
         StringWriter sw = new StringWriter();
         ds_form_report.WriteXml(sw);

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
                                    "<Privilege>2</Privilege><Sub_Sys>4</Sub_Sys>", 
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
                              new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                              {
                                 Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    false,
                                    "xml",
                                    string.Format(@"<Request type=""SetCurrentFormReports"">{0}{1}</Request>", sw.ToString(), xdoc.ToString()),
                                    "{ CALL Global.SetFormReport(?) }",
                                    "iProject",
                                    "scott"
                                 }                                 
                              }
                           }
                        ),
                    })
                );
      }

      partial void b_addnewreport_Click(object sender, EventArgs e)
      {
         ds_form_report.Tables["Form_Report"].Rows.Add(ds_form_report.Tables["Form_Report"].NewRow());
      }

      partial void b_removereport_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از حذف گزارش برای فرم مطمئن هستید؟", "حذف گزارش", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No) return;

         DataRow dr = gv_form_report.GetFocusedDataRow();

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
                                    false,
                                    "xml",
                                    string.Format(@"<Request type=""DelCurrentFormReport"">{0}{1}</Request>", new XElement("iProject", new XElement("Form_Report", new XElement("ID", dr["ID"]), new XElement("RWNO", dr["RWNO"]))), xdoc.ToString()),
                                    "{ CALL Global.SetFormReport(?) }",
                                    "iProject",
                                    "scott"
                                 }, 
                                 AfterChangedStatus = new Action<StatusType>(
                                    (status) =>
                                       {
                                          try { 
                                             switch(status)
                                             {
                                                case StatusType.Successful:                                                   
                                                   ds_form_report.Tables["Form_Report"].Rows.Remove(dr);
                                                   break;
                                             }
                                          }
                                          catch { }
                                       }
                                 )
                              }
                           }
                        ),
                    })
                );
      }

      partial void b_exit_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
           new Job(SendType.External, "Localhost", "FRPT_STNG_F", 04 /* Execute UnPaint */, SendType.SelfToUserInterface)
         );
      }

   }
}
