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
using System.Globalization;
using System.Xml.Linq;


namespace System.Emis.Sas.View
{
   public partial class MSTR_SERV_F : UserControl
   {
      public MSTR_SERV_F()
      {
         InitializeComponent();
      }

      private void WBtn_Search_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         e.Button.Properties.Enabled = false;
         if (e.Button.Properties.Tag.ToString() == "Srch")
         {
            string sqlQuery = "";
            if (Txt_FileNo.EditValue != null)
               sqlQuery += string.Format("AND File_No = {0} ", Txt_FileNo.EditValue);
            if(Txt_RegnCode.EditValue != null)
               sqlQuery += string.Format("AND Regn_Code = '{0}' ", Txt_RegnCode.EditValue);
            if(Txt_Rqid.EditValue != null)
               sqlQuery += string.Format("AND Efct_Rqid_Dnrm = {0} ", Txt_Rqid.EditValue);
            if (Cmb_RqtpCode.EditValue != null && Convert.ToInt32(Cmb_RqtpCode.EditValue) > 0)
               sqlQuery += string.Format("AND Efct_Rqst_Type_Dnrm = {0} ", Cmb_RqtpCode.EditValue);
            if(Txt_ServNo.EditValue != null)
               sqlQuery += string.Format("AND Serv_No_Dnrm = {0} ", Txt_ServNo.EditValue);
            if(Cmb_CyclStat.EditValue != null)
               sqlQuery += string.Format("AND Cycl_Stat = '{0}' ", Cmb_CyclStat.EditValue);
            if(Cbx_CustStat.Checked && Cbx_CustStat.CheckState == CheckState.Checked)
               sqlQuery += "AND Cust_Stat = '2' ";
            else if (!Cbx_CustStat.Checked && Cbx_CustStat.CheckState == CheckState.Unchecked)
               sqlQuery += "AND Cust_Stat = '1' ";
            if (Txt_TarfCode.EditValue != null)
               sqlQuery += string.Format("AND Tarf_Code_Dnrm = {0}", Txt_TarfCode.EditValue);
            if(Txt_WorkDay.EditValue != null)
               sqlQuery += string.Format("AND Work_Day_Dnrm = {0}", Txt_WorkDay.EditValue);
            if(Txt_BlokCode.EditValue != null)
               sqlQuery += string.Format("AND Blok_Dnrm = {0}", Txt_BlokCode.EditValue);
            if (Txt_SeqCode.EditValue != null)
               sqlQuery += string.Format("AND Seq_Dnrm = {0}", Txt_SeqCode.EditValue);
            if(Cmb_ServType.EditValue != null && Convert.ToInt32(Cmb_ServType.EditValue) > 0)
               sqlQuery += string.Format("AND Serv_Type_Dnrm = {0}", Cmb_ServType.EditValue);
            if (Txt_ServName.EditValue != null)
               sqlQuery += string.Format("AND Name_Dnrm LIKE '%{0}%'", Txt_ServName.EditValue);
            if (Txt_ServAddr.EditValue != null)
               sqlQuery += string.Format("AND Serv_Addr_Dnrm LIKE '%{0}%'", Txt_ServAddr.EditValue);
            if (Cmb_ZoneCode.EditValue != null && Convert.ToInt32(Cmb_ZoneCode.EditValue) > 0)
               sqlQuery += string.Format("AND Zone_Code_Dnrm = '{0}'", Cmb_ZoneCode.EditValue);
            if (Cmb_OnofTag.EditValue != null && Convert.ToInt32(Cmb_OnofTag.EditValue) > 0)
               sqlQuery += string.Format("AND Onof_Tag = '{0}'", Cmb_OnofTag.EditValue);
            if (Cmb_ReadStat.EditValue != null && Convert.ToInt32(Cmb_ReadStat.EditValue) > 0)
               sqlQuery += string.Format("AND Read_Stat = '{0}'", Cmb_ReadStat.EditValue);
            if (Cmb_ConsCode.EditValue != null && Convert.ToInt32(Cmb_ConsCode.EditValue) > 0)
               sqlQuery += string.Format("AND SUBSTR(Tarf_Code_Dnrm, 4, 1) = '{0}'", Cmb_ConsCode.EditValue);
            if (Cmb_ConfStat.EditValue != null && Convert.ToInt32(Cmb_ConfStat.EditValue) > 0)
               sqlQuery += string.Format("AND Conf_Stat = '{0}'", Cmb_ConfStat.EditValue);
            if (Cmb_Phas.EditValue != null && Convert.ToInt32(Cmb_Phas.EditValue) > 0)
               sqlQuery += string.Format("AND Phas_Dnrm = '{0}'", Cmb_Phas.EditValue);
            if (Cmb_BrncType.EditValue != null && Convert.ToInt32(Cmb_BrncType.EditValue) > 0)
               sqlQuery += string.Format("AND Brnc_Type_Dnrm = '{0}'", Cmb_BrncType.EditValue);
            if (Cb_PowrCodeNotTarf.Checked)
               sqlQuery += string.Format("AND ( NVL(Dlvr_Volt, '0') <> SUBSTR( Tarf_Code_Dnrm, 8, 1 ) OR NVL(Powr_Code, '0') <> SUBSTR( Tarf_Code_Dnrm, 9, 1 ) OR NVL(Item_Code, '0') <> SUBSTR( Tarf_Code_Dnrm, 10, 1 ) )");
            if (Cmb_RqstStat.EditValue != null && Convert.ToInt32(Cmb_RqstStat.EditValue) > 0)
               sqlQuery += string.Format(@"AND EXISTS(
                                                   SELECT *
                                                     FROM Request R
                                                    WHERE R.Rqid = Efct_Rqid_Dnrm
                                                      AND R.Sub_Sys = 3
                                                      AND R.Rqst_Stat = {0}
                                                      AND R.Sstt_Mstt_Row_No = {1}
                                               )", Cmb_RqstStat.EditValue
                                                 , Cb_RqstEnd99.Checked ? "99" : "R.Sstt_Mstt_Row_No");
            if (Cmb_MetrType.EditValue != null && Convert.ToInt32(Cmb_MetrType.EditValue) > 0)
               sqlQuery += string.Format(@"AND EXISTS(
                                                   SELECT *
                                                     FROM Meter_Spec B
                                                    WHERE B.Serv_File_No = File_No
                                                      AND B.Rect_Code = 6
                                                      AND B.Rwno = Metr_Seq_Dnrm
                                                      AND B.Metr_Type = {0}
                                               )", Cmb_MetrType.EditValue);

            if (OraPool.Tables.IndexOf("Service") > 0)
               OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Service"));

            OraDA.SelectCommand.CommandText = "SELECT * FROM SERVICE WHERE 1 = 1 " + sqlQuery;
            OraDA.Fill(OraPool);
            OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Service";

            ServiceBindingSource.DataSource = OraPool.Tables["Service"];
            if (OraPool.Tables["Service"].Rows.Count > 0)
            {               
               Tc_Info.SelectTab(1);
               ServiceDataGridView.Focus();
            }
         }
         else if (e.Button.Properties.Tag.ToString() == "Cler")
         {
            Txt_FileNo.EditValue = null;
            Txt_RegnCode.EditValue = null;
            Txt_Rqid.EditValue = null;
            Txt_ServNo.EditValue = null;
            Cmb_CyclStat.EditValue = null;
            Cbx_CustStat.Checked = true;
            Cbx_CustStat.CheckState = CheckState.Indeterminate;
            Cmb_RqtpCode.EditValue = null;
            Cmb_RqstStat.EditValue = null;
            Txt_TarfCode.EditValue = null;
            Txt_WorkDay.EditValue = null;
            Txt_BlokCode.EditValue = null;
            Txt_SeqCode.EditValue = null;
            Cmb_ServType.EditValue = null;
            Txt_ServName.EditValue = null;
            Txt_ServAddr.EditValue = null;
            Cmb_ZoneCode.EditValue = null;
            Cmb_OnofTag.EditValue = null;
            Cmb_ReadStat.EditValue = null;
            Cmb_ConsCode.EditValue = null;
            Cmb_ConfStat.EditValue = null;
            Cmb_Phas.EditValue = null;
            Cmb_BrncType.EditValue = null;
            Cmb_MetrType.EditValue = null;
            Cb_RqstEnd99.Checked = Cb_PowrCodeNotTarf.Checked = false;

            Tc_Info.SelectTab(0);
            Txt_FileNo.Focus();

            if (OraPool.Tables.Count == 4)
            {
               OraPool.Tables.RemoveAt(3);
            }
         }
         e.Button.Properties.Enabled = true;
      }

      private void Rqid_EditValueChanged(object sender, EventArgs e)
      {
         TextEdit edit = sender as TextEdit;
         if (edit.EditValue == null || string.IsNullOrEmpty(edit.EditValue.ToString()))
         {
            edit.Properties.Appearance.BackColor = Color.Transparent;
            return;
         }

         edit.Properties.Appearance.BackColor = Color.GreenYellow;
      }

      private void WBtn_FIR_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         if (MessageBox.Show(this, "آیا از انجام کار مطمئن هستید؟", "پیام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No) return;

         StrtLoopActn:

         var Current = ServiceBindingSource.Current as DataRowView;

         Job _InteractWithSas;
         switch (e.Button.Properties.Tag.ToString())
         {
            case "Free_Serv":
               _InteractWithSas = 
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
                                    "<Privilege>3</Privilege><Sub_Sys>6</Sub_Sys>", 
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
                           })
                     });
               _DefaultGateway.Gateway(_InteractWithSas);

               if (_InteractWithSas.Status == StatusType.Successful && Pre_Free_Serv(Current))
                  Exec_Free_Serv(Current);
               else
                  Roll_Back("مشترک قابل آزاد کردن نیست");
               break;
            case "Inst_Date":
               _InteractWithSas = 
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
                                    "<Privilege>4</Privilege><Sub_Sys>6</Sub_Sys>", 
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
                           })
                     });
               _DefaultGateway.Gateway(_InteractWithSas);
               if (_InteractWithSas.Status == StatusType.Successful && Pre_Inst_Date(Current))
                  Exec_Inst_Date(Current);
               else
                  Roll_Back("اصلاح تاریخ نصب کنتور قابل انجام نیست");
               break;
            case "Read_Date":
               _InteractWithSas = 
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
                                    "<Privilege>5</Privilege><Sub_Sys>6</Sub_Sys>", 
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
                           })
                     });
               _DefaultGateway.Gateway(_InteractWithSas);
               if (_InteractWithSas.Status == StatusType.Successful && Pre_Read_Date(Current))
                  Exec_Read_Date(Current);
               else
                  Roll_Back("اصلاح تاریخ آخرین قرائت کنتور قابل انجام نیست");
               break;
            case "Chng_Powr_Tarf":
               if (Pre_Powr_Code(Current))
                  Exec_Powr_Code(Current);
               else
                  Roll_Back("اصلاح کد قدرت قابل انجام شدن نیست");
               break;
            default:
               break;
         }

         if(TCmb_Refresh.SelectedIndex == 1)
            WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((DevExpress.XtraBars.Docking2010.IButton)WBtn_Search.Buttons[0]));         

         if (TCmb_Refresh.SelectedIndex == 2)
            ServiceBindingSource.MoveNext();

         if (ServiceBindingSource.Position == ServiceBindingSource.Count - 1)
         {
            if (TCmb_Refresh.SelectedIndex == 2)
               WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((DevExpress.XtraBars.Docking2010.IButton)WBtn_Search.Buttons[0]));
            return;
         }

         goto StrtLoopActn;
      }

      private void Roll_Back(string message)
      {
         L_Result.Text = message;
         L_Result.ForeColor = Color.Red;
      }

      private bool Pre_Free_Serv(DataRowView Current)
      {
         string CyclStat = Current.Row["Cycl_Stat"].ToString();
         if (CyclStat == "3" || CyclStat == "4" || CyclStat == "5") return false;
         else
         {
            string FileNo = Current.Row["File_No"].ToString();
            string RqstRqid = Current.Row["Efct_Rqid_Dnrm"].ToString();
            if (!string.IsNullOrEmpty(RqstRqid))
            {
               OraDA.SelectCommand.CommandText =
                  string.Format(
                     @"SELECT Rqid 
                         FROM Request, Service 
                        WHERE File_No = {0} 
                          AND Rqid = Efct_Rqid_Dnrm 
                          AND Sstt_Mstt_Row_No = 99
                          AND Sstt_Row_No = 99
                          AND Rqst_Stat IN (3, 2)"
                     , FileNo
                  );
               OraDA.Fill(OraPool);
               OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Result";

               int rowCount = OraPool.Tables["Result"].Rows.Count;
               OraPool.Tables.RemoveAt(OraPool.Tables.Count - 1);

               if (rowCount != 1) return false;
            }
         }
         return true;
      }

      private void Exec_Free_Serv(DataRowView Current)
      {
         string FileNo = Current.Row["File_No"].ToString();
         string RqstRqid = Current.Row["Efct_Rqid_Dnrm"].ToString();

         if (!string.IsNullOrEmpty(RqstRqid))
            OraDA.SelectCommand.CommandText =
                  string.Format(
                     @"UPDATE Service Su
                       SET Su.Cust_Stat = '2',
                           Su.Efct_Rqid_Dnrm = NULL,
                           Su.Efct_Rqst_Type_Dnrm = NULL,
                           Su.Efct_Rqno_Dnrm = NULL,
                           Su.Efct_Regn_Code_Dnrm = NULL,
                           Su.Efct_Cycl_Dnrm = NULL
                     WHERE Su.File_No = {0}
                       AND EXISTS
                              (SELECT S.File_No
                                 FROM Request R/*, Request_Row Rr*/, Service S
                              WHERE     /*Rr.Serv_File_No = {0}
                                    AND R.Rqid = Rr.Rqst_Rqid
                                    AND*/ R.Rqst_Stat IN (2, 3)
                                    AND R.Sstt_Mstt_Row_No = 99
                                    AND R.Sstt_Row_No = 99
                                    /*AND Rr.Serv_File_No = S.File_No*/
                                    AND R.Sub_Sys = 3
                                    AND S.Efct_Rqid_Dnrm = R.Rqid
                                    AND S.File_No = Su.File_No)
                    "
                     , FileNo
                  );
         else
            OraDA.SelectCommand.CommandText =
                  string.Format(
                     @"UPDATE Service Su
                       SET Su.Cust_Stat = '2',
                           Su.Efct_Rqid_Dnrm = NULL,
                           Su.Efct_Rqst_Type_Dnrm = NULL,
                           Su.Efct_Rqno_Dnrm = NULL,
                           Su.Efct_Regn_Code_Dnrm = NULL,
                           Su.Efct_Cycl_Dnrm = NULL
                     WHERE Su.File_No = {0}
                    ", FileNo
                  );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount == 1)
            {
               L_Result.ForeColor = Color.Green;
               L_Result.Text = "مشترک آزاد شد";
               OraTra.Commit();
            }
            else
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
               OraTra.Rollback();
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch
         {            
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }
      }

      private bool Pre_Inst_Date(DataRowView Current)
      {
         string CyclStat = Current.Row["Cycl_Stat"].ToString();
         string CustStat = Current.Row["Cust_Stat"].ToString();
         if (CyclStat == "3" || CyclStat == "4" || CyclStat == "5") return false;
         else if (CustStat == "1") return false;
         else
         {
            string FileNo = Current.Row["File_No"].ToString();
            OraDA.SelectCommand.CommandText =
               string.Format(
                  @"SELECT Bill_No
                      FROM bill
                     WHERE serv_file_no = {0}
                       AND bill_vlid = '2'
                       AND bill_type <> '0'
                       AND bill_stat >= '4'
                       AND bill_stat NOT IN ('8', 'A')
                       AND prcd_code IN (
                           SELECT code
                             FROM bil_prevent_code
                            WHERE actn_code IN ('1', '2', '5'))"
                  , FileNo
               );
            OraDA.Fill(OraPool);
            OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Result";

            int rowCount = OraPool.Tables["Result"].Rows.Count;
            OraPool.Tables.RemoveAt(OraPool.Tables.Count - 1);

            if (rowCount != 0) return false;
         }
         return true;
      }

      private void Exec_Inst_Date(DataRowView Current)
      {
         string FileNo = Current.Row["File_No"].ToString();
         PersianCalendar pc = new PersianCalendar();
         OraDA.SelectCommand.CommandText =
               string.Format(
                  @"UPDATE Base_Meter_Spec B
                       SET B.Inst_Date = TO_DATE ('{1}/{2,2:0#}/{3,2:0#}', 'YYYY/MM/DD')
                          ,B.Last_Read_Date = TO_DATE ('{1}/{2,2:0#}/{3,2:0#}', 'YYYY/MM/DD')
                     WHERE B.Rect_Code = 6
                       AND EXISTS (
                            SELECT *
                              FROM Service
                             WHERE File_No = {0}
                               AND B.Serv_File_No = File_No
                               AND B.Rwno = Base_Seq_Dnrm)"
                  , FileNo
                  , pc.GetYear(Pdat_InstDate.DateTime)
                  , pc.GetMonth(Pdat_InstDate.DateTime)
                  , pc.GetDayOfMonth(Pdat_InstDate.DateTime)
               );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount == 1)
            {
               L_Result.ForeColor = Color.Green;
               L_Result.Text = "تاریخ نصب کنتور و آخرین قرائت مشترک بروزرسانی شد ";
               OraTra.Commit();
            }
            else
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
               OraTra.Rollback();
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch 
         {            
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }

         OraDA.SelectCommand.CommandText =
               string.Format(
                  @"UPDATE Service
                       SET Inst_Date_Dnrm = TO_DATE ('{1}/{2,2:0#}/{3,2:0#}', 'YYYY/MM/DD')
                          ,Last_Read_Date = TO_DATE ('{1}/{2,2:0#}/{3,2:0#}', 'YYYY/MM/DD')
                     WHERE File_No = {0}"
                  , FileNo
                  , pc.GetYear(Pdat_InstDate.DateTime)
                  , pc.GetMonth(Pdat_InstDate.DateTime)
                  , pc.GetDayOfMonth(Pdat_InstDate.DateTime)
               );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount == 1)
            {
               L_Result.ForeColor = Color.Green;
               L_Result.Text = "تاریخ نصب کنتور و آخرین قرائت مشترک بروزرسانی شد ";
               OraTra.Commit();
            }
            else
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
               OraTra.Rollback();
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch
         {            
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }
      }

      private bool Pre_Read_Date(DataRowView Current)
      {
         string CyclStat = Current.Row["Cycl_Stat"].ToString();
         string CustStat = Current.Row["Cust_Stat"].ToString();

         if (CyclStat == "3" || CyclStat == "4" || CyclStat == "5") return false;
         else if (CustStat == "1") return false;
         return true;
      }

      private void Exec_Read_Date(DataRowView Current)
      {
         string FileNo = Current.Row["File_No"].ToString();
         string LastReadDate = "";
         OraDA.SelectCommand.CommandText =
               string.Format(
                 @"SELECT Crnt_Date
                     FROM (  SELECT Crnt_Date
                              FROM Bill
                              WHERE Serv_File_No = {0}
                                 AND Bill_Type <> '0'
                                 AND Bill_Stat >= '4'
                                 AND Bill_Stat NOT IN ('8', 'A')
                                 AND Prcd_Code IN (SELECT Code
                                                   FROM Bil_Prevent_Code
                                                   WHERE Actn_Code IN ('1', '2', '5'))
                           ORDER BY Bill_No DESC)
                     WHERE ROWNUM = 1"
                  , FileNo
               );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            LastReadDate = OraDA.SelectCommand.ExecuteScalar().ToString();
            if (LastReadDate == null)
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "مشترک هیچ قبضی  ندارد";
               OraTra.Rollback();
               OraDA.SelectCommand.Connection.Close();
               return;
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch
         {            
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در جمع آوری اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }

         PersianCalendar pc = new PersianCalendar();
         OraDA.SelectCommand.CommandText =
               string.Format(
                  @"UPDATE Base_Meter_Spec B
                       SET B.Last_Read_Date = TO_DATE ('{1}/{2,2:0#}/{3,2:0#}', 'YYYY/MM/DD')
                     WHERE B.Rect_Code = 6
                       AND B.Serv_File_No = {0}
                       AND EXISTS (
                            SELECT *
                              FROM Service
                             WHERE File_No = {0}
                               AND B.Serv_File_No = File_No
                               AND B.Rwno = Base_Seq_Dnrm)"
                  , FileNo
                  , pc.GetYear(Convert.ToDateTime(LastReadDate))
                  , pc.GetMonth(Convert.ToDateTime(LastReadDate))
                  , pc.GetDayOfMonth(Convert.ToDateTime(LastReadDate))
               );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount == 1)
            {
               L_Result.ForeColor = Color.Green;
               L_Result.Text = "تاریخ نصب کنتور و آخرین قرائت مشترک بروزرسانی شد ";
               OraTra.Commit();
            }
            else
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
               OraTra.Rollback();
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch
         {
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }

         OraDA.SelectCommand.CommandText =
               string.Format(
                  @"UPDATE Service
                       SET Last_Read_Date = TO_DATE ('{1}/{2,2:0#}/{3,2:0#}', 'YYYY/MM/DD')
                     WHERE File_No = {0}"
                  , FileNo
                  , pc.GetYear(Convert.ToDateTime(LastReadDate))
                  , pc.GetMonth(Convert.ToDateTime(LastReadDate))
                  , pc.GetDayOfMonth(Convert.ToDateTime(LastReadDate))
               );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount == 1)
            {
               L_Result.ForeColor = Color.Green;
               L_Result.Text = "تاریخ نصب کنتور و آخرین قرائت مشترک بروزرسانی شد ";
               OraTra.Commit();
            }
            else
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
               OraTra.Rollback();
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch
         {
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }
      }

      private bool Pre_Powr_Code(DataRowView Current)
      {
         string ConfStat = Current.Row["Conf_Stat"].ToString();         

         if (ConfStat == "1") return false;
         return true;
      }

      private void Exec_Powr_Code(DataRowView Current)
      {
         string FileNo = Current.Row["File_No"].ToString();

         OraDA.SelectCommand.CommandText =
               string.Format(
                  @"UPDATE Service
                       SET Dlvr_Volt = SUBSTR( Tarf_Code_Dnrm, 8, 1 ),
                           Powr_Code = SUBSTR( Tarf_Code_Dnrm, 9, 1 ),
                           Item_Code = SUBSTR( Tarf_Code_Dnrm, 10, 1 )
                     WHERE Conf_Stat = '2'
                       AND File_No = {0}"
                  , FileNo         
               );
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount == 1)
            {
               L_Result.ForeColor = Color.Green;
               L_Result.Text = "اطلاعات کد قدرت و تعرفه اصلاح شد ";
               OraTra.Commit();
            }
            else
            {
               L_Result.ForeColor = Color.Red;
               L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
               OraTra.Rollback();
            }
            OraDA.SelectCommand.Connection.Close();
         }
         catch
         {
            L_Result.ForeColor = Color.Red;
            L_Result.Text = "بروز اشکال در بروزرسانی اطلاعات مشترک";
            OraTra.Rollback();
            OraDA.SelectCommand.Connection.Close();
         }
      }

      private void Btn_InvsRqid_Click(object sender, EventArgs e)
      {
         if (string.IsNullOrEmpty(Txt_RqidDnrm.EditValue.ToString())) return;

         if (OraPool.Tables.IndexOf("Request") > 0)
            OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Request"));

         OraDA.SelectCommand.CommandText = "SELECT * FROM Request WHERE Rqid = " + Txt_RqidDnrm.EditValue;
         OraDA.Fill(OraPool);
         OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Request";

         RequestBindingSource.DataSource = OraPool.Tables["Request"];
         if (OraPool.Tables["Request"].Rows.Count > 0)
         {
            Tc_Info.SelectTab(2);            
         }
      }

      private void Btn_PblcInfo_Click(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrWhiteSpace(fILE_NOSpinEdit.EditValue.ToString())) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  #region DoWork
                  new Job(SendType.Self, 06 /* Execute Serv_Info_F */){Input = new XElement("Sas_Public", new XAttribute("datasource", ConnectionString), new XAttribute("fileno", fILE_NOSpinEdit.EditValue.ToString())) }
                  #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_Bill_Click(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrWhiteSpace(fILE_NOSpinEdit.EditValue.ToString())) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  #region DoWork
                  new Job(SendType.Self, 05 /* Execute Serv_Bill_F */){Input = new XElement("Bill", new XAttribute("datasource", ConnectionString), new XAttribute("fileno", fILE_NOSpinEdit.EditValue.ToString())) }
                  #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_Dart_Click(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrWhiteSpace(fILE_NOSpinEdit.EditValue.ToString())) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  #region DoWork
                  new Job(SendType.Self, 07 /* Execute Serv_Dart_F */){Input = new XElement("Bill", new XAttribute("datasource", ConnectionString), new XAttribute("fileno", fILE_NOSpinEdit.EditValue.ToString())) }
                  #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_RqstChng_Click(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrWhiteSpace(fILE_NOSpinEdit.EditValue.ToString())) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  #region DoWork
                  new Job(SendType.Self, 08 /* Execute Serv_Rqst_F */){Input = new XElement("Bill", new XAttribute("datasource", ConnectionString), new XAttribute("fileno", fILE_NOSpinEdit.EditValue.ToString())) }
                  #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_SaveChangeService_Click(object sender, EventArgs e)
      {
         if (Dlg_SaveFile.ShowDialog() != DialogResult.OK) return;

         ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook();
         wb.Worksheets.Add(OraPool.Tables["Service"], "Result");
         wb.SaveAs(Dlg_SaveFile.FileName);
      }
   }
}
