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

namespace System.Emis.Sas.View
{
   public partial class MSTR_RQST_F : UserControl
   {
      public MSTR_RQST_F()
      {
         InitializeComponent();
      }

      private void WBtn_Search_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         e.Button.Properties.Enabled = false;
         string btn = e.Button.Properties.Tag.ToString();
         if (btn == "Srch")
         {
            string sqlQuery = "";
            if (Txt_Rqid.Text != "")
               sqlQuery += string.Format("AND Rqid = {0} ", Txt_Rqid.EditValue);
            if (Txt_RefCode.Text != "")
               sqlQuery += string.Format("AND Ref_Code = {0} ", Txt_RefCode.EditValue);
            if (Txt_RqstStat.Text != "")
               sqlQuery += string.Format("AND Rqst_Stat = '{0}' ", Txt_RqstStat.EditValue);
            if (Txt_RegnCode.Text != "")
               sqlQuery += string.Format("AND Regn_Code = '{0}' ", Txt_RegnCode.EditValue);
            if (Txt_RqtpCode.Text != "")
               sqlQuery += string.Format("AND Rqtp_Code = {0} ", Txt_RqtpCode.EditValue);
            if (Txt_RqttCode.Text != "")
               sqlQuery += string.Format("AND Rqtt_Code = {0} ", Txt_RqttCode.EditValue);
            if(Txt_FromRqstDate.Text != "")
               sqlQuery += string.Format("AND Rqst_Date >= TO_DATE('{0}', 'YYYY/MM/DD') ", Txt_FromRqstDate.EditValue);
            if (Txt_ToRqstDate.Text != "")
               sqlQuery += string.Format("AND Rqst_Date <= TO_DATE('{0}', 'YYYY/MM/DD') ", Txt_ToRqstDate.EditValue);
            if (Txt_FromSaveDate.Text != "")
               sqlQuery += string.Format("AND Save_Date >= TO_DATE('{0}', 'YYYY/MM/DD') ", Txt_FromSaveDate.EditValue);
            if (Txt_ToSaveDate.Text != "")
               sqlQuery += string.Format("AND Save_Date <= TO_DATE('{0}', 'YYYY/MM/DD') ", Txt_ToSaveDate.EditValue);
            if (Txt_LettNo.Text != "")
               sqlQuery += string.Format("AND Lett_No = '{0}' ", Txt_LettNo.EditValue);
            if (Txt_LettDate.Text != "")
               sqlQuery += string.Format("AND Lett_Date = TO_DATE('{0}', 'YYYY/MM/DD') ", Txt_LettDate.EditValue);
            if (Txt_LettOwnr.Text != "")
               sqlQuery += string.Format("AND Lett_Ownr LIKE '%{0}%' ", Txt_LettOwnr.EditValue);
            if (Txt_MsttRwno.Text != "")
               sqlQuery += string.Format("AND Sstt_Mstt_Row_No = {0} ", Txt_MsttRwno.EditValue);
            if (Txt_SsttRwno.Text != "")
               sqlQuery += string.Format("AND Sstt_Row_No = {0} ", Txt_SsttRwno.EditValue);
            if (Txt_SubSys.Text != "")
               sqlQuery += string.Format("AND Sub_Sys = '{0}' ", Txt_SubSys.EditValue);
            if (Txt_RqstRqid.Text != "")
               sqlQuery += string.Format("AND Rqst_Rqid = {0} ", Txt_RqstRqid.EditValue);

            if (OraPool.Tables.IndexOf("Request") > 0)
               OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Request"));

            OraDA.SelectCommand.CommandText = "SELECT * FROM Request WHERE 1 = 1 " + sqlQuery;
            OraDA.Fill(OraPool);
            OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Request";

            RequestBindingSource.DataSource = OraPool.Tables["Request"];
            RequestGridControl.DataSource = RequestBindingSource;
            Request_RowGridControl.DataSource = Request_RowBindingSource;
            Step_History_SummeryDataGridView.DataSource = Step_History_SummeryBindingSource;
            Step_History_DetailDataGridView.DataSource = Step_History_DetailBindingSource;
            if (OraPool.Tables["Request"].Rows.Count > 0)
            {
               Tc_Info.SelectTab(1);
               RequestGridControl.Focus();
            }
         }
         else if(btn == "Cler")
         {
            if (OraPool.Tables.IndexOf("Request") > 0)
               OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Request"));

            Tc_Info.SelectTab(0);
            Txt_Rqid.EditValue = Txt_RefCode.EditValue = Txt_RegnCode.EditValue = Txt_RqstStat.EditValue = Txt_RqtpCode.EditValue = Txt_RqttCode.EditValue = null;
            Txt_FromRqstDate.EditValue = Txt_ToRqstDate.EditValue = Txt_FromSaveDate.EditValue = Txt_ToSaveDate.EditValue = null;
            Txt_LettNo.EditValue = Txt_LettDate.EditValue = Txt_LettOwnr.EditValue = Txt_MsttRwno.EditValue = Txt_SsttRwno.EditValue = null;
            Txt_SubSys.EditValue = null;

            //RequestBindingSource = null;
            RequestGridControl.DataSource = null;
            //Request_RowBindingSource = null;
            Request_RowGridControl.DataSource = null;
            //Step_History_SummeryBindingSource = null;
            Step_History_SummeryDataGridView.DataSource = null;
            //Step_History_DetailBindingSource = null;
            Step_History_DetailDataGridView.DataSource = null;

            Txt_Rqid.Focus();
         }
         e.Button.Properties.Enabled = true;
      }

      private void RequestBindingSource_PositionChanged(object sender, EventArgs e)
      {
         var Current = RequestBindingSource.Current as DataRowView;

         if (Current == null || Current["Rqid"].ToString() == "") return;
         
         if (OraPool.Tables.IndexOf("Request_Row") > 0)
            OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Request_Row"));

         OraDA.SelectCommand.CommandText = string.Format("SELECT Rr.Rwno, Rr.Serv_File_No, S.Name_Dnrm, S.Serv_Addr_Dnrm, S.Tarf_Code_Dnrm, S.Phas_Dnrm, S.Ampr_Dnrm, Rr.Rec_Stat FROM Request_Row Rr, Service S WHERE Rr.Rqst_Rqid = {0} AND Rr.Serv_File_No = S.File_No", Current["Rqid"].ToString());
         OraDA.Fill(OraPool);
         OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Request_Row";

         Request_RowBindingSource.DataSource = OraPool.Tables["Request_Row"];
         if (OraPool.Tables["Request_Row"].Rows.Count > 0 && Tc_Info.SelectedTab != tabPage2)
         {
            Tc_Info.SelectTab(1);
            RequestGridControl.Focus();
         }

         if (OraPool.Tables.IndexOf("Step_History_Summery") > 0)
            OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Step_History_Summery"));

         OraDA.SelectCommand.CommandText = string.Format("SELECT RQST_RQID, Row_no, Shis_Desc, TO_CHAR(From_Date, 'YYYY/MM/DD') AS From_Date, TO_CHAR(To_Date, 'YYYY/MM/DD') AS To_Date, Cret_By, Mdfy_By FROM Step_History_Summery WHERE Rqst_Rqid = {0} ORDER BY Row_No", Current["Rqid"].ToString());
         OraDA.Fill(OraPool);
         OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Step_History_Summery";

         Step_History_SummeryBindingSource.DataSource = OraPool.Tables["Step_History_Summery"];
      }

      private void Step_History_SummeryBindingSource_PositionChanged(object sender, EventArgs e)
      {
         var Current = Step_History_SummeryBindingSource.Current as DataRowView;

         if (Current == null || Current["Rqst_Rqid"].ToString() == "") return;

         if (OraPool.Tables.IndexOf("Step_History_Detail") > 0)
            OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Step_History_Detail"));

         OraDA.SelectCommand.CommandText = string.Format("SELECT SHIS_RQST_RQID, Row_No, Shit_Desc, TO_CHAR(From_Date, 'YYYY/MM/DD') AS From_Date, TO_CHAR(To_Date, 'YYYY/MM/DD') AS To_Date, Cret_By, Mdfy_By FROM Step_History_Detail WHERE Shis_Rqst_Rqid = {0} AND Shis_Row_No = {1} ORDER BY Shis_Row_No, Row_No", Current["Rqst_Rqid"], Current["Row_No"]);
         OraDA.Fill(OraPool);
         OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Step_History_Detail";

         Step_History_DetailBindingSource.DataSource = OraPool.Tables["Step_History_Detail"];
      }

      private void Step_History_SummeryBindingSource_DataSourceChanged(object sender, EventArgs e)
      {
         var Current = Step_History_SummeryBindingSource.Current as DataRowView;

         if (Current == null || Current["Rqst_Rqid"].ToString() == "") return;

         if (OraPool.Tables.IndexOf("Step_History_Detail") > 0)
            OraPool.Tables.RemoveAt(OraPool.Tables.IndexOf("Step_History_Detail"));

         OraDA.SelectCommand.CommandText = string.Format("SELECT SHIS_RQST_RQID, Row_No, Shit_Desc, TO_CHAR(From_Date, 'YYYY/MM/DD') AS From_Date, TO_CHAR(To_Date, 'YYYY/MM/DD') AS To_Date, Cret_By, Mdfy_By FROM Step_History_Detail WHERE Shis_Rqst_Rqid = {0} AND Shis_Row_No = {1} ORDER BY Shis_Row_No, Row_No", Current["Rqst_Rqid"], Current["Row_No"]);
         OraDA.Fill(OraPool);
         OraPool.Tables[OraPool.Tables.Count - 1].TableName = "Step_History_Detail";

         Step_History_DetailBindingSource.DataSource = OraPool.Tables["Step_History_Detail"];
      }

      private void Wbtn_Rqst_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         if (MessageBox.Show(this, "آیا مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         string btn = e.Button.Properties.Tag.ToString();
         e.Button.Properties.Enabled = false;
         var Rqid = RequestBindingSource.Current as DataRowView;
         Job _InteractWithSas;
         if (btn == "CRqd")
         {
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
                                    "<Privilege>7</Privilege><Sub_Sys>6</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;                                    
                                 })
                              },
                              #endregion
                           })
                     });
            _DefaultGateway.Gateway(_InteractWithSas);

            if (_InteractWithSas.Status == StatusType.Successful && Pre_Cncl_Rqst(Rqid))
               Exec_Cncl_Rqst(Rqid);
            else if (_InteractWithSas.Status == StatusType.Successful)
               Roll_Back("درخواست قابل انصراف نیست");
            else
               InsufficientPrivilege("خطا - عدم دسترسی به ردیف 7 امنیتی : شما مجوز انصراف درخواست را ندارید");
         }
         else if (btn == "RExp")
         {
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
                                    "<Privilege>8</Privilege><Sub_Sys>6</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;                                    
                                 })
                              },
                              #endregion
                           })
                     });
            _DefaultGateway.Gateway(_InteractWithSas);

            if (_InteractWithSas.Status == StatusType.Successful && Pre_Remv_Expn(Rqid))
               Exec_Remv_Expn(Rqid);
            else if (_InteractWithSas.Status == StatusType.Successful)
               Roll_Back("هزینه های درخواست قابل حذف نیستند");
            else
               InsufficientPrivilege("خطا - عدم دسترسی به ردیف 8 امنیتی : شما مجوز حذف هزینه درخواست را ندارید");
         }
         else if (btn == "FRqd")
         {
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
                                    "<Privilege>9</Privilege><Sub_Sys>6</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;                                    
                                 })
                              },
                              #endregion
                           })
                     });
            _DefaultGateway.Gateway(_InteractWithSas);

            if (_InteractWithSas.Status == StatusType.Successful && Pre_Finl_Rqst(Rqid))
               Exec_Finl_Rqst(Rqid);
            else if (_InteractWithSas.Status == StatusType.Successful)
               Roll_Back("درخواست قابل پایانی شدن نیست");
            else
               InsufficientPrivilege("خطا - عدم دسترسی به ردیف 9 امنیتی : شما مجوز پایانی کردن درخواست را ندارید");
         }
         e.Button.Properties.Enabled = true;
      }

      private bool Pre_Cncl_Rqst(DataRowView Rqid)
      {
         if (Rqid["Sub_Sys"].ToString() != "3") return false;
         return true;
      }

      private void Exec_Cncl_Rqst(DataRowView Rqid)
      {
         string CurrRqid = Rqid["Rqid"].ToString();
         OraDA.SelectCommand.CommandText = string.Format(@"DECLARE X NUMBER; BEGIN X := Exec_Cncl_Rqst(XMLTYPE('<Process><Request rqid=""{0}""/></Process>')); END;", CurrRqid);
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount > 0)
            {
               Commit("درخواست در وضعیت انصراف قرار گرفت");
               WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((DevExpress.XtraBars.Docking2010.IButton)WBtn_Search.Buttons[0]));
            }
            else
               Roll_Back("درخواست در وضعیت انصراف قرار نگرفت");
         }
         catch (System.Data.OracleClient.OracleException ex)
         {
            Roll_Back(ex.Message);
         }
      }

      private bool Pre_Remv_Expn(DataRowView Rqid)
      {
         if (Rqid["Sub_Sys"].ToString() != "3") return false;
         return true;
      }

      private void Exec_Remv_Expn(DataRowView Rqid)
      {
         string CurrRqid = Rqid["Rqid"].ToString();
         OraDA.SelectCommand.CommandText = string.Format(@"DECLARE X NUMBER; BEGIN X := Exec_Remv_Expn(XMLTYPE('<Process><Request rqid=""{0}""/></Process>')); END;", CurrRqid);
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount > 0)
               Commit("هزینه های درخواست از سیستم حذف گردید");
            else
               Roll_Back("برای درخواست جاری هیچ هزینه ای حذف نشد.");
         }
         catch (System.Data.OracleClient.OracleException ex)
         {
            Roll_Back(ex.Message);
         }
      }

      private bool Pre_Finl_Rqst(DataRowView Rqid)
      {
         if (Rqid["Sub_Sys"].ToString() != "3") return false;
         return true;
      }

      private void Exec_Finl_Rqst(DataRowView Rqid)
      {
         string CurrRqid = Rqid["Rqid"].ToString();
         OraDA.SelectCommand.CommandText = string.Format(@"DECLARE X NUMBER; BEGIN X := Exec_Finl_Rqst(XMLTYPE('<Process><Request rqid=""{0}""/></Process>')); END;", CurrRqid);
         try
         {
            OraDA.SelectCommand.Connection.Open();
            OraTra = OraDA.SelectCommand.Connection.BeginTransaction();
            OraDA.SelectCommand.Transaction = OraTra;
            int rowCount = OraDA.SelectCommand.ExecuteNonQuery();
            if (rowCount > 0)
            {
               Commit("درخواست در وضعیت پایانی قرار گرفت");
               WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((DevExpress.XtraBars.Docking2010.IButton)WBtn_Search.Buttons[0]));
            }
            else
               Roll_Back("درخواست در وضعیت پایانی قرار نگرفت");
         }
         catch (System.Data.OracleClient.OracleException ex)
         {
            Roll_Back(ex.Message);
         }
      }

      private void Roll_Back(string message)
      {
         OraTra.Rollback();
         OraDA.SelectCommand.Connection.Close();
         MessageBox.Show(this, message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      private void Commit(string message)
      {
         OraTra.Commit();
         OraDA.SelectCommand.Connection.Close();
         MessageBox.Show(this, message, "پایان عملیات", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private void InsufficientPrivilege(string message)
      {
         MessageBox.Show(this, message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      private void RequestBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         RequestBindingSource_PositionChanged(null, null);
      }

   }
}
