using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.Scsc.ExtCode;

namespace System.Scsc.Ui.ReportManager
{
   public partial class RPT_PMMT_F : UserControl
   {
      public RPT_PMMT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formName = "";
      private long? cochfileno = null, cbmtcode = null;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString) { CommandTimeout = 18000 };
         try
         {
            if (tc_master.SelectedTab == tp_001)
            {
               var rqtps = Rqtp_Lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               var rcmts = Rcmt_Lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               var users = User_Lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               long? fileno = null;
               if (Figh_Lov.EditValue != null && Figh_Lov.EditValue.ToString() != "")
                  fileno = (long?)Figh_Lov.EditValue;
               
               PmmtBs1.DataSource =
                  iScsc.Payment_Methods
                  .Where(pm =>
                     (pm.RCPT_MTOD != "005") &&
                     (pm.Payment.PYMT_STAT != "002") &&
                     (pm.ACTN_DATE.Value.Date >= FromDate1_Date.Value.Value.Date ) &&
                     (pm.ACTN_DATE.Value.Date <= ToDate1_Date.Value.Value.Date ) &&
                     (rqtps.Count == 0 || rqtps.Contains(pm.Request_Row.RQTP_CODE)) &&
                     (rcmts.Count == 0 || rcmts.Contains(pm.RCPT_MTOD)) &&
                     (users.Count == 0 || (users.Contains(pm.CRET_BY) || users.Contains(pm.MDFY_BY))) &&
                     (pm.Request_Row.FIGH_FILE_NO == (fileno ?? pm.Request_Row.FIGH_FILE_NO)) &&
                     (Fga_Uclb_U.Contains(pm.Payment.CLUB_CODE_DNRM)) &&
                     // 1397/09/02 * اضافه شدن فیلتر مربوط به مربی
                     (cochfileno == null || pm.Payment.Payment_Details.Any(pd => pd.FIGH_FILE_NO == cochfileno)) &&
                     (cbmtcode == null || pm.Payment.Payment_Details.Any(pd => pd.CBMT_CODE_DNRM == cbmtcode))
                  );

               GlrdBs1.DataSource =
                  iScsc.Gain_Loss_Rail_Details
                  .Where(gd =>
                     gd.Gain_Loss_Rial.CONF_STAT == "002" &&
                     (gd.Gain_Loss_Rial.PAID_DATE.Value.Date >= FromDate1_Date.Value.Value.Date ) &&
                     (gd.Gain_Loss_Rial.PAID_DATE.Value.Date <= ToDate1_Date.Value.Value.Date ) &&
                     (users.Count == 0 || (users.Contains(gd.CRET_BY) || users.Contains(gd.MDFY_BY))) &&
                     (Fga_Uclb_U.Contains(gd.Gain_Loss_Rial.Fighter.CLUB_CODE_DNRM))
                  );
            }
            else if(tc_master.SelectedTab == tp_002)
            {
               var rqtps = Rqtp_Lov2.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               var users = User_Lov2.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();

               long? fileno = null;//, coch = null;               
               if (Figh_Lov2.EditValue != null && Figh_Lov2.EditValue.ToString() != "")
                  fileno = (long?)Figh_Lov2.EditValue;

               PydtBs2.DataSource =
                  iScsc.Payment_Details
                  .Where(pd =>
                     pd.Request_Row.Request.RQST_DATE.Value.Date >= FromDate2_Date.Value.Value.Date &&
                     pd.Request_Row.Request.RQST_DATE.Value.Date <= ToDate2_Date.Value.Value.Date &&
                     
                     pd.Payment.PYMT_STAT != "002" &&
                     /*pd.Request_Row.Request.RQST_DATE.Value.TimeOfDay >= FromTime2_Te.Time.TimeOfDay &&
                     pd.Request_Row.Request.RQST_DATE.Value.TimeOfDay <= ToTime2_Te.Time.TimeOfDay &&*/
                     
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (rqtps.Count == 0 || rqtps.Contains(pd.Request_Row.RQTP_CODE)) &&
                     (users.Count == 0 || (users.Contains(pd.CRET_BY) || users.Contains(pd.MDFY_BY))) &&                     
                     //(pd.Request_Row.FIGH_FILE_NO == (fileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM)) &&
                     // 1397/09/02 * اضافه شدن فیلتر مربوط به مربی
                     (fileno == null || pd.FIGH_FILE_NO == fileno) &&
                     (cochfileno == null || pd.FIGH_FILE_NO == cochfileno) &&
                     (cbmtcode == null || pd.CBMT_CODE_DNRM == cbmtcode)
                  );

               RpacBs.DataSource = iScsc.Report_Actions;
               MdrpBs.DataSource = iScsc.Modual_Reports.Where(m => m.MDUL_NAME == "RPT_PYM1_F");
            }
            else if (tc_master.SelectedTab == tp_003)
            {
               var rqtps = Rqtp_Lov3.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               var users = User_Lov3.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               long? fileno = null;
               if (Figh_Lov3.EditValue != null && Figh_Lov3.EditValue.ToString() != "")
                  fileno = (long?)Figh_Lov3.EditValue;

               PydsBs3.DataSource =
                  iScsc.Payment_Discounts
                  .Where(pd =>
                     /*pd.Request_Row.Request.RQST_DATE.Value.Date >= FromDate3_Date.Value.Value.Date &&
                     pd.Request_Row.Request.RQST_DATE.Value.Date <= ToDate3_Date.Value.Value.Date &&*/
                     pd.CRET_DATE.Value.Date >= FromDate3_Date.Value.Value.Date &&
                     pd.CRET_DATE.Value.Date <= ToDate3_Date.Value.Value.Date &&
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     pd.Payment.PYMT_STAT != "002" &&
                     (rqtps.Count == 0 || rqtps.Contains(pd.Request_Row.RQTP_CODE)) &&
                     (users.Count == 0 || (users.Contains(pd.CRET_BY) || users.Contains(pd.MDFY_BY))) &&
                     (pd.STAT == "002") &&
                     //(pd.AMNT_TYPE != "004" /* به جز ملبغ های مابه التفاوت */) &&
                     (pd.Request_Row.FIGH_FILE_NO == (fileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
                     ((Pyds004_Cb.Checked && pd.AMNT_TYPE == "004") || (Pyds004_Cb.Checked == false && pd.AMNT_TYPE != "004")) &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM)) &&
                     // 1397/09/02 * اضافه شدن فیلتر مربوط به مربی
                     (cochfileno == null || pd.Payment.Payment_Details.Any(pdt => pdt.FIGH_FILE_NO == cochfileno)) &&
                     (cbmtcode == null || pd.Payment.Payment_Details.Any(pdt => pdt.CBMT_CODE_DNRM == cbmtcode))
                  );
            }
            else if(tc_master.SelectedTab == tp_004)
            {
               MsexBs4.DataSource =
                  iScsc.Misc_Expenses
                  .Where(me =>
                     me.VALD_TYPE == "002" &&
                     me.DELV_STAT == "002" &&
                     me.DELV_DATE.Value.Date >= FromDate4_Date.Value.Value.Date &&
                     me.DELV_DATE.Value.Date <= ToDate4_Date.Value.Value.Date  &&
                     (Fga_Uclb_U.Contains(me.CLUB_CODE)) &&
                     // 1397/09/02 * اضافه شدن فیلتر مربوط به مربی
                     (cochfileno == null || me.COCH_FILE_NO == cochfileno)
                  );
            }
            //else if (tc_master.SelectedTab == tp_005)
            //{
            //   var rqtps = Rqtp_Lov5.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
            //   var users = User_Lov5.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();

            //   long? fileno = null;//, coch = null;

            //   if (Figh_Lov5.EditValue != null && Figh_Lov5.EditValue.ToString() != "")
            //      fileno = (long?)Figh_Lov5.EditValue;

            //   PydtBs5.DataSource =
            //      iScsc.Payment_Details
            //      .Where(pd =>
            //         pd.TRAN_DATE.Value.Date >= FromDate5_Date.Value.Value.Date &&
            //         pd.TRAN_DATE.Value.Date <= ToDate5_Date.Value.Value.Date &&
            //         pd.Payment.PYMT_STAT != "002" &&
            //         /*pd.Request_Row.Request.RQST_DATE.Value.TimeOfDay >= FromTime2_Te.Time.TimeOfDay &&
            //         pd.Request_Row.Request.RQST_DATE.Value.TimeOfDay <= ToTime2_Te.Time.TimeOfDay &&*/

            //         pd.Request_Row.Request.RQST_STAT == "002" &&
            //         (rqtps.Count == 0 || rqtps.Contains(pd.Request_Row.RQTP_CODE)) &&
            //         (users.Count == 0 || (users.Contains(pd.CRET_BY) || users.Contains(pd.MDFY_BY))) &&
            //         (pd.Request_Row.FIGH_FILE_NO == (fileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
            //         (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM))
            //      );
            //}
            else if(tc_master.SelectedTab == tp_006)
            {
               PmckBs.DataSource =
                  iScsc.Payment_Checks
                  .Where(pc =>
                     (
                        PyckExpireDate_Rb.Checked ? 
                        (pc.CHEK_DATE.Value.Date >= FromDate6_Date.Value.Value.Date && 
                        pc.CHEK_DATE.Value.Date <= ToDate6_Date.Value.Value.Date)
                        :
                        true
                     ) &&
                     (
                        PyckRcptDate_Rb.Checked ? 
                        (pc.RCPT_DATE.Value.Date >= FromDate6_Date.Value.Value.Date && 
                        pc.RCPT_DATE.Value.Date <= ToDate6_Date.Value.Value.Date)
                        :
                        true
                     ) &&
                     (PyckCheck_Rb.Checked ? 
                        pc.AMNT_TYPE == "001" : 
                        (PyckInstall_Rb.Checked ? 
                        pc.AMNT_TYPE == "002" :
                        true)
                     ) &&
                     pc.Request_Row.Request.RQST_STAT == "002" &&
                     pc.Payment.PYMT_STAT != "002" &&
                     (Fga_Uclb_U.Contains(pc.Payment.CLUB_CODE_DNRM))
                  );
            }
            else if (tc_master.SelectedTab == tp_007)
            {
               VSmsBs7.DataSource =
                  iScsc.V_Sms_Message_Boxes
                  .Where(s => 
                     s.ACTN_DATE.Value.Date >= FromDate7_Date.Value.Value.Date && 
                     s.ACTN_DATE.Value.Date <= ToDate7_Date.Value.Value.Date &&
                     (SmsSend003_Rb.Checked ? true : 
                        (SmsSend001_Rb.Checked ? s.MESG_ID != null : s.MESG_ID == null)
                     )
                  );
            }
            else if(tc_master.SelectedTab == tp_008)
            {
               GPymBs.DataSource =
                  iScsc.Payment_Methods
                  .Where(pm =>
                     (pm.RCPT_MTOD == "005") &&
                     (pm.Payment.PYMT_STAT != "002") &&
                     (pm.ACTN_DATE.Value.Date >= FromDate1_Date.Value.Value.Date) &&
                     (pm.ACTN_DATE.Value.Date <= ToDate1_Date.Value.Value.Date) &&
                     //(rqtps.Count == 0 || rqtps.Contains(pm.Request_Row.RQTP_CODE)) &&
                     //(rcmts.Count == 0 || rcmts.Contains(pm.RCPT_MTOD)) &&
                     //(users.Count == 0 || (users.Contains(pm.CRET_BY) || users.Contains(pm.MDFY_BY))) &&
                     //(pm.Request_Row.FIGH_FILE_NO == (fileno ?? pm.Request_Row.FIGH_FILE_NO)) &&
                     (Fga_Uclb_U.Contains(pm.Payment.CLUB_CODE_DNRM)) 
                        // 1397/09/02 * اضافه شدن فیلتر مربوط به مربی
                     //(cochfileno == null || pm.Payment.Payment_Details.Any(pd => pd.FIGH_FILE_NO == cochfileno)) &&
                     //(cbmtcode == null || pm.Payment.Payment_Details.Any(pd => pd.CBMT_CODE_DNRM == cbmtcode))
                  );
            }
            else if (tc_master.SelectedTab == tp_009)
            {
               StisBs.DataSource =
                  iScsc.Statistics
                  .Where( s => 
                     s.STIS_DATE.Value.Date >= FromDate9_Date.Value.Value.Date &&
                     s.STIS_DATE.Value.Date <= ToDate9_Date.Value.Value.Date &&
                     s.STIS_STAT == "002"
                  );

               RqroBs.DataSource = 
                  iScsc.Request_Rows
                  .Where(rr => 
                     rr.Request.RQST_STAT != "003" && 
                     rr.CRET_DATE.Value.Date >= FromDate9_Date.Value.Value.Date &&
                     rr.CRET_DATE.Value.Date <= ToDate9_Date.Value.Value.Date);

               LOptBs.DataSource =
                  iScsc.Log_Operations
                  .Where(lo => 
                     lo.CRET_DATE.Value.Date >= FromDate9_Date.Value.Value.Date &&
                     lo.CRET_DATE.Value.Date <= ToDate9_Date.Value.Value.Date
                  );
            }
            else if(tc_master.SelectedTab == tp_010)
            {
               PydtBs10.DataSource =
                  iScsc.Payment_Details
                  .Where(pd =>
                     pd.EXTS_RSRV_DATE.Value.Date >= FromDate10_Date.Value.Value.Date &&
                     pd.EXTS_RSRV_DATE.Value.Date <= ToDate10_Date.Value.Value.Date &&
                     pd.Payment.PYMT_STAT != "002" &&
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM)) && 
                     pd.EXTS_CODE != null
                  );
            }
            else if(tc_master.SelectedTab == tp_011)
            {
               PydtBs11.DataSource =
                  iScsc.Payment_Details
                  .Where(pd =>
                     pd.EXPR_DATE.Value.Date >= FromDate11_Date.Value.Value.Date &&
                     pd.EXPR_DATE.Value.Date <= ToDate11_Date.Value.Value.Date &&
                     pd.Payment.PYMT_STAT != "002" &&
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM))                     
                  );
            }
            else if(tc_master.SelectedTab == tp_012)
            {
               PydtBs12.DataSource =
                  iScsc.Payment_Details
                  .Where(pd =>
                     pd.Request_Row.Request.RQST_DATE.Value.Date >= FromDate12_Date.Value.Value.Date &&
                     pd.Request_Row.Request.RQST_DATE.Value.Date <= ToDate12_Date.Value.Value.Date &&
                     pd.Payment.PYMT_STAT != "002" &&
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM)) &&
                     pd.MBSP_RWNO != null
                  );
            }
         }
         catch { }
      }

      private void Find_Butn_Click(object sender, EventArgs e)
      {
         FromDate1_Date.CommitChanges(); ToDate1_Date.CommitChanges();
         FromDate2_Date.CommitChanges(); ToDate2_Date.CommitChanges();
         FromDate3_Date.CommitChanges(); ToDate3_Date.CommitChanges();
         FromDate4_Date.CommitChanges(); ToDate4_Date.CommitChanges();
         
         FromDate6_Date.CommitChanges(); ToDate6_Date.CommitChanges();
         FromDate7_Date.CommitChanges(); ToDate7_Date.CommitChanges();
         FromDate8_Date.CommitChanges(); ToDate8_Date.CommitChanges();
         FromDate9_Date.CommitChanges(); ToDate9_Date.CommitChanges();
         FromDate10_Date.CommitChanges(); ToDate10_Date.CommitChanges();
         FromDate11_Date.CommitChanges(); ToDate11_Date.CommitChanges();
         FromDate12_Date.CommitChanges(); ToDate12_Date.CommitChanges();

         // 1401/11/20 * بررسی اینکه آیا گزینه ها درست وار دشده یا خیر
         if (Figh_Lov2.EditValue == null || Figh_Lov2.EditValue.ToString() == "") { Figh_Lov2.EditValue = null; }
         if (Cbmt_Lov2.EditValue == null || Cbmt_Lov2.EditValue.ToString() == "") { Cbmt_Lov2.EditValue = null; }
         if (Sunt_Lov2.EditValue == null || Sunt_Lov2.EditValue.ToString() == "") { Sunt_Lov2.EditValue = null; }

         // مرگ بر دیکتاتور
         // 1401/08/26 * زن_زندگی_آزادی
         Save_Default_Report_Action_Parameter();

         if (tc_master.SelectedTab == tp_001)
         {
            if (!FromDate1_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate1_Date.Focus(); return; }
            if (!ToDate1_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate1_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if(!checkValidateDate(FromDate1_Date.Value.Value.Date))return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate9_Date.Value = FromDate8_Date.Value = FromDate6_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate1_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value     = ToDate10_Date.Value = ToDate7_Date.Value = ToDate9_Date.Value = ToDate8_Date.Value = ToDate6_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate1_Date.Value;

            Rqtp_Lov2.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov.EditValue;
            Figh_Lov2.EditValue = Figh_Lov3.EditValue = Figh_Lov.EditValue;
            User_Lov2.EditValue = User_Lov3.EditValue = User_Lov.EditValue;
         }
         else if(tc_master.SelectedTab == tp_002)
         {
            if (!FromDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate2_Date.Focus(); return; }
            if (!ToDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate2_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate2_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate9_Date.Value = FromDate8_Date.Value = FromDate6_Date.Value = FromDate1_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate2_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate7_Date.Value = ToDate9_Date.Value = ToDate8_Date.Value = ToDate6_Date.Value = ToDate1_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate2_Date.Value;
            
            if(FromTime2_Te.EditValue == null)
               FromTime2_Te.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
            if(ToTime2_Te.EditValue == null)
               ToTime2_Te.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 00);

            Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov2.EditValue;
            Figh_Lov.EditValue = Figh_Lov3.EditValue = Figh_Lov2.EditValue;
            User_Lov.EditValue = User_Lov3.EditValue = User_Lov2.EditValue;
         }
         else if (tc_master.SelectedTab == tp_003)
         {
            if (!FromDate3_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate3_Date.Focus(); return; }
            if (!ToDate3_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate3_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate3_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate9_Date.Value = FromDate8_Date.Value = FromDate6_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate7_Date.Value = ToDate9_Date.Value = ToDate8_Date.Value = ToDate6_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value;

            Rqtp_Lov2.EditValue = Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue;
            Figh_Lov2.EditValue = Figh_Lov.EditValue = Figh_Lov3.EditValue;
            User_Lov2.EditValue = User_Lov.EditValue = User_Lov3.EditValue;
         }
         else if (tc_master.SelectedTab == tp_004)
         {
            if (!FromDate4_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate4_Date.Focus(); return; }
            if (!ToDate4_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate4_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate4_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate8_Date.Value = FromDate9_Date.Value = FromDate6_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate7_Date.Value = ToDate8_Date.Value = ToDate9_Date.Value = ToDate6_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value;
         }
         //else if (tc_master.SelectedTab == tp_005)
         //{
         //   if (!FromDate5_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate5_Date.Focus(); return; }
         //   if (!ToDate5_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate5_Date.Focus(); return; }

         //   // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
         //   if (!checkValidateDate(FromDate5_Date.Value.Value.Date)) return;

         //   FromDate9_Date.Value = FromDate8_Date.Value = FromDate6_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate5_Date.Value;
         //   ToDate9_Date.Value = ToDate8_Date.Value = ToDate6_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate5_Date.Value;

         //   Rqtp_Lov2.EditValue = Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov5.EditValue;
         //   Figh_Lov2.EditValue = Figh_Lov.EditValue = Figh_Lov3.EditValue = Figh_Lov5.EditValue;
         //   User_Lov2.EditValue = User_Lov.EditValue = User_Lov3.EditValue = User_Lov5.EditValue;
         //}
         else if (tc_master.SelectedTab == tp_006)
         {
            if (!FromDate6_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate6_Date.Focus(); return; }
            if (!ToDate6_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate6_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate6_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate9_Date.Value = FromDate8_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate7_Date.Value = ToDate9_Date.Value = ToDate8_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value;
         }
         else if (tc_master.SelectedTab == tp_007)
         {
            if (!FromDate7_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate7_Date.Focus(); return; }
            if (!ToDate7_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate7_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate7_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate9_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value = FromDate8_Date.Value = FromDate7_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate9_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value = ToDate8_Date.Value = ToDate7_Date.Value;
         }
         else if(tc_master.SelectedTab == tp_008)
         {
            if (!FromDate8_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate8_Date.Focus(); return; }
            if (!ToDate8_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate8_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate8_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate9_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value = FromDate8_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate7_Date.Value = ToDate9_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value = ToDate8_Date.Value;
         }
         else if(tc_master.SelectedTab == tp_009)
         {
            if (!FromDate9_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate9_Date.Focus(); return; }
            if (!ToDate9_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate9_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate9_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate10_Date.Value = FromDate7_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value = FromDate8_Date.Value = FromDate9_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate10_Date.Value = ToDate7_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value = ToDate8_Date.Value = ToDate9_Date.Value;
         }
         else if(tc_master.SelectedTab == tp_010)
         {
            if (!FromDate10_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate10_Date.Focus(); return; }
            if (!ToDate10_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate10_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate10_Date.Value.Value.Date)) return;

            FromDate12_Date.Value = FromDate11_Date.Value = FromDate9_Date.Value = FromDate7_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value = FromDate8_Date.Value = FromDate10_Date.Value;
            ToDate12_Date.Value = ToDate11_Date.Value = ToDate9_Date.Value = ToDate7_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value = ToDate8_Date.Value = ToDate10_Date.Value;
         }
         else if(tc_master.SelectedTab == tp_011)
         {
            if (!FromDate11_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate11_Date.Focus(); return; }
            if (!ToDate11_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate11_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate11_Date.Value.Value.Date)) return;

            FromDate10_Date.Value = FromDate12_Date.Value = FromDate9_Date.Value = FromDate7_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value = FromDate8_Date.Value = FromDate11_Date.Value;
            ToDate10_Date.Value = ToDate12_Date.Value = ToDate9_Date.Value = ToDate7_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value = ToDate8_Date.Value = ToDate11_Date.Value;
         }
         else if(tc_master.SelectedTab == tp_012)
         {
            if (!FromDate12_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate12_Date.Focus(); return; }
            if (!ToDate12_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate12_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate12_Date.Value.Value.Date)) return;

            FromDate10_Date.Value = FromDate11_Date.Value = FromDate9_Date.Value = FromDate7_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate6_Date.Value = FromDate8_Date.Value = FromDate12_Date.Value;
            ToDate10_Date.Value = ToDate11_Date.Value = ToDate9_Date.Value = ToDate7_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate6_Date.Value = ToDate8_Date.Value = ToDate12_Date.Value;
         }

         Execute_Query();
      }

      private bool checkValidateDate(DateTime dateTime)
      {
         bool result = false;
         if (dateTime.Date == DateTime.Now.Date) return true;

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
                              "<Privilege>240</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output) {
                                 result = true;
                                 return;
                              }
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 240 سطوح امینتی", "عدم دسترسی");
                              result = false;
                           })
                        },
                        #endregion
                     })
               });
         _DefaultGateway.Gateway(_InteractWithScsc);

         return result;
      }

      private void Submit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Pydt_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var pydt = PydtBs2.Current as Data.Payment_Detail;
            switch (e.Button.Index)
            {
               case 0:                  
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pydt.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               case 1:
                  PmmtBs1.DataSource =
                     iScsc.Payment_Methods
                     .Where(pm =>
                        pm.RQRO_RQST_RQID == pydt.PYMT_RQST_RQID
                     );
                  tc_master.SelectedTab = tp_001;
                  break;
               case 2:
                  PydsBs3.DataSource =
                     iScsc.Payment_Discounts
                     .Where(pd =>
                        pd.PYMT_RQST_RQID == pydt.PYMT_RQST_RQID
                     );
                  tc_master.SelectedTab = tp_003;
                  break;
               default:
                  break;
            }
         }
         catch (Exception )
         {

         }
      }

      private void Pmmt_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var pmmt = PmmtBs1.Current as Data.Payment_Method;
            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pmmt.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               case 1:
                  PydtBs2.DataSource =
                     iScsc.Payment_Details
                     .Where(pm =>
                        pm.PYMT_RQST_RQID == pmmt.PYMT_RQST_RQID
                     );
                  tc_master.SelectedTab = tp_002;
                  break;
               case 2:
                  PydsBs3.DataSource =
                     iScsc.Payment_Discounts
                     .Where(pd =>
                        pd.PYMT_RQST_RQID == pmmt.PYMT_RQST_RQID
                     );
                  tc_master.SelectedTab = tp_003;
                  break;
               default:
                  break;
            }
         }
         catch (Exception )
         {

         }
      }

      private void Pyds_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var pyds = PydsBs3.Current as Data.Payment_Discount;
            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pyds.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               case 1:
                  PydtBs2.DataSource =
                     iScsc.Payment_Details
                     .Where(pm =>
                        pm.PYMT_RQST_RQID == pyds.PYMT_RQST_RQID
                     );
                  tc_master.SelectedTab = tp_002;
                  break;
               case 2:
                  PmmtBs1.DataSource =
                     iScsc.Payment_Methods
                     .Where(pd =>
                        pd.PYMT_RQST_RQID == pyds.PYMT_RQST_RQID
                     );
                  tc_master.SelectedTab = tp_001;
                  break;
               default:
                  break;
            }
         }
         catch (Exception )
         {

         }
      }

      private void RqstBnPrint_Click(object sender, EventArgs e)
      {
         Find_Butn_Click(null, null);

         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                  {
                     Input = 
                        new XElement("Print", 
                           new XAttribute("type", "Selection"), 
                           new XAttribute("modual", /*GetType().Name*/formName), 
                           new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"),
                           //string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\" code=\"{4}\" ctgycode=\"{5}\"/></Request>", FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), User_Lov2.EditValue, Figh_Lov2.EditValue, Cbmt_Lov2.EditValue, Ctgy_lov2.EditValue )
                           string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\" code=\"{4}\" ctgycode=\"{5}\"/><Organ code=\"{6}\"/></Request>", 
                                          FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                                          ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                                          User_Lov2.EditValue, 
                                          Figh_Lov2.EditValue/*cochfileno*/, 
                                          Cbmt_Lov2.EditValue/*cbmtcode*/, 
                                          Ctgy_lov2.EditValue,
                                          Sunt_Lov2.EditValue)
                        )
                  }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         Find_Butn_Click(null, null);

         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                  {
                     Input = 
                        new XElement("Print", 
                           new XAttribute("type", "Default"), 
                           new XAttribute("modual", /*GetType().Name*/ formName), 
                           new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                           string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\" code=\"{4}\" ctgycode=\"{5}\"/><Organ code=\"{6}\"/></Request>", 
                              FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                              ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                              User_Lov2.EditValue, 
                              Figh_Lov2.EditValue, 
                              Cbmt_Lov2.EditValue/*cbmtcode*/, 
                              Ctgy_lov2.EditValue,
                              Sunt_Lov2.EditValue
                           )
                        )
                  }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */)
                  {
                     Input = 
                        new XElement("Request", 
                           new XAttribute("type", "ModualReport"), 
                           new XAttribute("modul", /*GetType().Name*/ formName), 
                           new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F")
                        )
                  }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Expn_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var misxexpn = MsexBs4.Current as Data.Misc_Expense;
            if (misxexpn == null) return;

            misxexpn.VALD_TYPE = "001";

            iScsc.SubmitChanges();

            /// Must Be Change
            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                     new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
            }
         }
      }

      private void Pydt1_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            //Pydtran_Gv.PostEditor();
            var pydt = PydtBs5.Current as Data.Payment_Detail;
            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pydt.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               case 1:
                  try
                  {
                     bool checkOK = true;
                     Job _InteractWithScsc =
                        new Job(SendType.External, "Desktop",
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
                                          "<Privilege>221</Privilege><Sub_Sys>5</Sub_Sys>", 
                                          "DataGuard"
                                       },
                                       AfterChangedOutput = new Action<object>((output) => {
                                          if ((bool)output)
                                             return;
                                          checkOK = false;
                                          MessageBox.Show(this, "عدم دسترسی به ردیف 221 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                       })
                                    }
                                    #endregion                        
                                 })                     
                              });
                     _DefaultGateway.Gateway(_InteractWithScsc);
                     if (checkOK)
                     {
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                                       {
                                          new Job(SendType.Self, 86 /* Execute Pay_Mtod_F */){Input = pydt.Payment},
                                          //new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 10 /* Execute Actn_CalF_F*/)
                                          //{
                                          //   Input = 
                                          //      new XElement("Payment_Method",
                                          //         new XAttribute("callerform", GetType().Name),
                                          //         new XAttribute("tabfocued", "tp_003")
                                          //      )
                                          //}
                                       }
                           )
                        );
                     }
                  }
                  catch
                  {
                     requery = false;
                  }

                  break;
               case 2:
                  try
                  {
                     bool checkOK = true;
                     #region Check Security
                     Job _InteractWithScsc =
                        new Job(SendType.External, "Desktop",
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
                                                "<Privilege>224</Privilege><Sub_Sys>5</Sub_Sys>", 
                                                "DataGuard"
                                             },
                                             AfterChangedOutput = new Action<object>((output) => {
                                                if ((bool)output)
                                                   return;
                                                checkOK = false;
                                                MessageBox.Show(this, "عدم دسترسی به ردیف 224 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                             })
                                          }
                                          #endregion                        
                                       })                     
                                    });
                     _DefaultGateway.Gateway(_InteractWithScsc);
                     #endregion
                     if (checkOK)
                     {
                        if (MessageBox.Show(this, "آیا با ویرایش کردن هزینه درخواست موافقید؟", "ویرایش هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                        iScsc.UPD_SEXP_P(
                           new XElement("Request",
                              new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                              new XElement("Payment",
                                 new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                                 new XElement("Payment_Detail",
                                    new XAttribute("code", pydt.CODE),
                                    new XAttribute("expncode", pydt.EXPN_CODE),
                                    new XAttribute("expnpric", pydt.EXPN_PRIC),
                                    new XAttribute("pydtdesc", pydt.PYDT_DESC ?? ""),
                                    new XAttribute("qnty", pydt.QNTY ?? 1),
                                    new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                                    new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                                    new XAttribute("mtodcodednrm", pydt.MTOD_CODE_DNRM ?? 0),
                                    new XAttribute("ctgycodednrm", pydt.CTGY_CODE_DNRM ?? 0),
                                    new XAttribute("tranby", pydt.TRAN_BY),
                                    new XAttribute("transtat", "002"),
                                    new XAttribute("trandate", pydt.TRAN_DATE),
                                    new XAttribute("trancbmtcode", pydt.TRAN_CBMT_CODE),
                                    new XAttribute("tranmtodcode", pydt.TRAN_MTOD_CODE),
                                    new XAttribute("tranctgycode", pydt.TRAN_CTGY_CODE),
                                    new XAttribute("tranexpncode", pydt.TRAN_EXPN_CODE)
                                 )
                              )
                           )
                        );
                        requery = true;
                     }
                  }
                  catch (Exception )
                  { }
                  finally
                  {
                     if (requery)
                     {
                        Execute_Query();
                     }
                  }

                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void Cbmt_Lov2_EditValueChanging(object sender, ChangingEventArgs e)
      {
         try
         {
            var cbmt = CbmtBs.List.OfType<Data.Club_Method>().FirstOrDefault(cb => cb.CODE == (long)e.NewValue);
            CtgyBs.DataSource = iScsc.Category_Belts.Where(cg => cg.MTOD_CODE == cbmt.MTOD_CODE && cg.CTGY_STAT == "002");
            cbmtcode = (long?)e.NewValue;
         }
         catch { cbmtcode = null; }
      }

      private void Btn_Cbmt2_Click(object sender, EventArgs e)
      {
         try
         {
            long code = (long)Cbmt_Lov2.EditValue;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 149 /* Execute Bas_Wkdy_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_WKDY_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("code", code), new XAttribute("showonly", "002"))}
                  }
               )
            );
         }
         catch { }
      }

      private void Actn6_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var pmck = PmckBs.Current as Data.Payment_Check;
            if (pmck == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pmck.Request_Row.FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         { MessageBox.Show(exc.Message); }
      }

      private void Figh_Lov2_EditValueChanging(object sender, ChangingEventArgs e)
      {
         try
         {
            if (e.NewValue == null || e.NewValue.ToString() == "") {  cochfileno = null; return; }
            cochfileno = (long?)e.NewValue;
         }
         catch { cochfileno = null; }
      }

      private void Glrd_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var glrd = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", glrd.Gain_Loss_Rial.FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         {

            throw;
         }
      }

      private void CretStis_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromDate9_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate9_Date.Focus(); return; }
            if (!ToDate9_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate9_Date.Focus(); return; }

            //FromDate1_Date.Value = FromDate8_Date.Value = FromDate6_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate5_Date.Value = FromDate9_Date.Value;
            //ToDate1_Date.Value = ToDate8_Date.Value = ToDate6_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate5_Date.Value = ToDate9_Date.Value;

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate9_Date.Value.Value.Date)) return;

            iScsc.CRET_STIS_P(
               new XElement("Statistic", 
                   new XAttribute("fromdate", FromDate9_Date.Value.Value.ToString("yyyy-MM-dd")),
                   new XAttribute("todate", ToDate9_Date.Value.Value.ToString("yyyy-MM-dd"))
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void RemvStis_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromDate9_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate9_Date.Focus(); return; }
            if (!ToDate9_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate9_Date.Focus(); return; }

            //FromDate1_Date.Value = FromDate8_Date.Value = FromDate6_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate5_Date.Value = FromDate9_Date.Value;
            //ToDate1_Date.Value = ToDate8_Date.Value = ToDate6_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate5_Date.Value = ToDate9_Date.Value;

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate9_Date.Value.Value.Date)) return;

            iScsc.REMV_STIS_P(
               new XElement("Statistic",
                   new XAttribute("fromdate", FromDate9_Date.Value.Value.ToString("yyyy-MM-dd")),
                   new XAttribute("todate", ToDate9_Date.Value.Value.ToString("yyyy-MM-dd"))
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void UpdtStis_Butn_Click(object sender, EventArgs e)
      {
         RemvStis_Butn_Click(null, null);
         CretStis_Butn_Click(null, null);
      }

      private void StisBs_CurrentChanged(object sender, EventArgs e)
      {
         var stis = StisBs.Current as Data.Statistic;

         Stsd1Bs.DataSource = iScsc.Statistic_Details.Where(sd => sd.Statistic == stis && sd.RECT_TYPE == "001");
         Stsd2Bs.DataSource = iScsc.Statistic_Details.Where(sd => sd.Statistic == stis && sd.RECT_TYPE == "002");         
      }

      private void Rqst_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var rqro = RqroBs.Current as Data.Request_Row;
            if (rqro == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", rqro.FIGH_FILE_NO)) }
                  );
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqroBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqro = RqroBs.Current as Data.Request_Row;
            if (rqro == null) return;

            switch (rqro.RQTP_CODE)
            {
               case "001":
                  //RqstInfo_Lb.Text =
                  //   string.Format(
                  //      "ثبت نام جدید مشتری برای" + " {0} " + ""
                  //      ,
                  //      rqro.Fighter.NAME_DNRM
                  //   );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OneS_Rb_CheckedChanged(object sender, EventArgs e)
      {
         if (OneS_Rb.Checked)
            Pydts_Gv.OptionsSelection.MultiSelect = false;
         else if (MulS_Rb.Checked || AllS_Rb.Checked)
            Pydts_Gv.OptionsSelection.MultiSelect = true;
      }

      private void SaveCbmtEdit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (CbmtEdit_Lov.EditValue == null || CbmtEdit_Lov.EditValue.ToString() == "") { CbmtEdit_Lov.Focus(); return; }

            var _xdata =
               new XElement("Router_Command",
                   new XAttribute("subsys", 5),
                   new XAttribute("cmndcode", 109),
                   new XElement("Payment_Details",
                       Pydts_Gv.GetSelectedRows()
                       .ToList()
                       .Select(r => 
                          new XElement("Payment_Detail", 
                             new XAttribute("code", ((Data.Payment_Detail)Pydts_Gv.GetRow(r)).CODE),
                             new XAttribute("cbmtcode", CbmtEdit_Lov.EditValue)
                          )
                       )
                   )
               );

            iScsc.RunnerdbCommand(_xdata, ref _xdata);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveGustInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fp = FgpbBs1.Current as Data.Fighter_Public;
            if (fp == null) return;

            if (Pydts_Gv.OptionsSelection.MultiSelect) { MessageBox.Show("برای تغییر مشخصات اطلاعات مشتریان باید نوع انتخاب در حالت تک انتخابی باشد"); OneS_Rb.Checked = true; return; }

            var _xdata =
               new XElement("Router_Command",
                   new XAttribute("subsys", 5),
                   new XAttribute("cmndcode", 110),
                   new XElement("Fighter_Publics",
                       Pydts_Gv.GetSelectedRows()
                       .ToList()
                       .Select(r =>
                          new XElement("Fighter_Public",
                             new XAttribute("rqid", ((Data.Payment_Detail)Pydts_Gv.GetRow(r)).PYMT_RQST_RQID),
                             new XAttribute("frstname", fp.FRST_NAME ?? ""),
                             new XAttribute("lastname", fp.LAST_NAME ?? ""),
                             new XAttribute("cellphon", fp.CELL_PHON ?? ""),
                             new XAttribute("natlcode", fp.NATL_CODE ?? ""),
                             new XAttribute("servno", fp.SERV_NO ?? ""),
                             new XAttribute("suntcode", fp.SUNT_CODE ?? "")
                          )
                       )
                   )
               );

            iScsc.RunnerdbCommand(_xdata, ref _xdata);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void PydtBs2_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            FgpbBs1.List.Clear();

            var pydt = PydtBs2.Current as Data.Payment_Detail;
            if (pydt == null) return;

            if (pydt.Request_Row.RQTP_CODE == "016")
               FgpbBs1.DataSource = iScsc.Fighter_Publics.FirstOrDefault(fp => fp.RQRO_RQST_RQID == pydt.PYMT_RQST_RQID);
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
         }
      }

      private void RunRprt001_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (TiktNumb_Rb.Checked && (TiktNumb_Txt.EditValue == null || TiktNumb_Txt.EditValue.ToString() == "")) { TiktNumb_Txt.Focus(); return; }
            if (SuntTikt_Rb.Checked && (SuntTikt_Lov.EditValue == null || SuntTikt_Lov.EditValue.ToString() == "")) { SuntTikt_Lov.Focus(); return; }

            var _pydts =
               iScsc.Payment_Details.Where(pd => pd.FROM_NUMB != null).ToList();

            if(TiktNumb_Rb.Checked)
            {
               var _rslt = _pydts.Where(pd => Convert.ToInt64(TiktNumb_Txt.EditValue).IsBetween((long)pd.FROM_NUMB, (long)pd.TO_NUMB));
               PydtBs2.DataSource = _rslt;
            }
            else if(SuntTikt_Rb.Checked)
            {
               var _rslt = _pydts.Where(pd => pd.Request_Row.Fighter.SUNT_CODE_DNRM == SuntTikt_Lov.EditValue.ToString());
               PydtBs2.DataSource = _rslt;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveExprEdit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!ExprEdit_Dt.Value.HasValue) { ExprEdit_Dt.Focus(); return; }

            var _xdata =
               new XElement("Router_Command",
                   new XAttribute("subsys", 5),
                   new XAttribute("cmndcode", 111),
                   new XElement("Payment_Details",
                       Pydts_Gv.GetSelectedRows()
                       .ToList()
                       .Select(r =>
                          new XElement("Payment_Detail",
                             new XAttribute("code", ((Data.Payment_Detail)Pydts_Gv.GetRow(r)).CODE),
                             new XAttribute("exprdate", ExprEdit_Dt.Value.Value.ToString("yyyy-MM-dd"))
                          )
                       )
                   )
               );

            iScsc.RunnerdbCommand(_xdata, ref _xdata);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveMbspRwnoEdit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MbspRwnoPydtEdit_Lov.EditValue == null || MbspRwnoPydtEdit_Lov.EditValue.ToString() == "") { MbspRwnoPydtEdit_Lov.Focus(); return; }

            var _xdata =
               new XElement("Router_Command",
                   new XAttribute("subsys", 5),
                   new XAttribute("cmndcode", 112),
                   new XElement("Payment_Details",
                       Pydts_Gv.GetSelectedRows()
                       .ToList()
                       .Select(r =>
                          new XElement("Payment_Detail",
                             new XAttribute("code", ((Data.Payment_Detail)Pydts_Gv.GetRow(r)).CODE),
                             new XAttribute("mbsprwno", MbspRwnoPydtEdit_Lov.EditValue)
                          )
                       )
                   )
               );

            iScsc.RunnerdbCommand(_xdata, ref _xdata);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void MbspRwnoPydtEdit_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            MbspBs.List.Clear();

            var _pydt = PydtBs2.Current as Data.Payment_Detail;
            if (_pydt == null) return;


            MbspBs.DataSource = iScsc.Member_Ships.Where(m => m.FIGH_FILE_NO == _pydt.Request_Row.FIGH_FILE_NO && m.RECT_CODE == "004" && m.VALD_TYPE == "002");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void TiktChng_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _pydt = PydtBs2.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            if (Pydts_Gv.OptionsSelection.MultiSelect) { MessageBox.Show("برای ثبت اطلاعات شماره بلیط به صورت عمده فروشی باید نوع انتخاب در حالت تک انتخابی باشد"); OneS_Rb.Checked = true; return; }

            if (TiktFromNumbEdit_Txt.EditValue == null || TiktFromNumbEdit_Txt.EditValue.ToString() != "") { TiktFromNumbEdit_Txt.Focus(); return; }
            if (TiktToNumbEdit_Txt.EditValue == null || TiktToNumbEdit_Txt.EditValue.ToString() != "") { TiktToNumbEdit_Txt.Focus(); return; }

            var _xdata =
               new XElement("Router_Command",
                   new XAttribute("subsys", 5),
                   new XAttribute("cmndcode", 113),
                   new XElement("Fighter_Publics",
                       Pydts_Gv.GetSelectedRows()
                       .ToList()
                       .Select(r =>
                          new XElement("Payment_Detail",
                             new XAttribute("code", ((Data.Payment_Detail)Pydts_Gv.GetRow(r)).CODE),
                             new XAttribute("fromnumb", TiktFromNumbEdit_Txt.EditValue),
                             new XAttribute("tonumb", TiktToNumbEdit_Txt.EditValue)
                          )
                       )
                   )
               );

            iScsc.RunnerdbCommand(_xdata, ref _xdata);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void AddReportAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RpacBs.List.OfType<Data.Report_Action>().Any(ra => ra.CODE == 0)) return;

            var _rpac = RpacBs.AddNew() as Data.Report_Action;
            _rpac.STAT = "002";
            _rpac.RPRT_NAME = "عنوان گزارش";
            _rpac.ACTN_NAME = "Exec_Rpac_P";
            iScsc.Report_Actions.InsertOnSubmit(_rpac);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelReportAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rpac = RpacBs.Current as Data.Report_Action;
            if (_rpac == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.Report_Action WHERE Code = {0};", _rpac.CODE));
            requery = true;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveReportAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RpacBs.EndEdit();
            Rpac_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void CreateReportAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate2_Date.Focus(); return; }
            if (!ToDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate2_Date.Focus(); return; }

            var _rpac = RpacBs.Current as Data.Report_Action;
            if (_rpac == null) return;

            var _user = User_Lov2.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
            
            Data.Sub_Unit _orgn = null;
            if(Sunt_Lov2.EditValue != null && Sunt_Lov2.EditValue.ToString() != "")
               _orgn = SuntBs1.List.OfType<Data.Sub_Unit>().FirstOrDefault(o => o.CODE == Sunt_Lov2.EditValue);

            if (MessageBox.Show(this, "ایجاد گزارش فرآیند زمان بری میباشد، لطفا تا پایان اجرای فرایند صبر کنید. آیا فرایند انجام شود؟", "ایجاد گزارش", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(
               string.Format("EXEC dbo.{0} @X = N'{1}'", 
                  _rpac.ACTN_NAME, 
                  new XElement("Parameter",
                      new XAttribute("rpactype", _rpac.RPAC_TYPE),
                      new XAttribute("fromdate", FromDate2_Date.Value == null ? "" : FromDate2_Date.Value.Value.ToString("yyyy-MM-dd")),
                      new XAttribute("todate", ToDate2_Date.Value == null ? "" : ToDate2_Date.Value.Value.ToString("yyyy-MM-dd")),
                      new XAttribute("cochfileno", Figh_Lov2.EditValue ?? ""),
                      new XAttribute("cbmtcode", Cbmt_Lov2.EditValue ?? ""),
                      new XAttribute("recdownr", _user.Count == 0 ? "" : string.Join(":", _user)),
                      new XAttribute("suntcode", _orgn != null ? _orgn.CODE : ""),
                      new XAttribute("suntbuntcode", _orgn != null ? _orgn.BUNT_CODE : ""),
                      new XAttribute("suntbuntdeptcode", _orgn != null ? _orgn.BUNT_DEPT_CODE : ""),
                      new XAttribute("suntbuntdeptorgncode", _orgn != null ? _orgn.BUNT_DEPT_ORGN_CODE : "")
                  ).ToString()
               )
            );

            MessageBox.Show(this, "اجرای گزارش به پایان رسید.");

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowReportAction_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rpac = RpacBs.Current as Data.Report_Action;
            if (_rpac == null) return;

            var _rpap = iScsc.Report_Action_Parameters.FirstOrDefault(r => r.RPAC_TYPE == _rpac.RPAC_TYPE && r.CRET_BY == CurrentUser);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selected"), 
                              new XAttribute("mdrpcode", _rpac.MDRP_CODE),
                              new XAttribute("modual", /*GetType().Name*/formName), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"),
                              string.Format("Rpap_Code = {0}", _rpap.CODE)
                           )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowActnRprt_Lov_EditValueChanging(object sender, ChangingEventArgs e)
      {
         try
         {
            var _rpac = RpacBs.Current as Data.Report_Action;
            if (_rpac == null) return;

            _rpac.Modual_Report = iScsc.Modual_Reports.FirstOrDefault(m => m.CODE == (long)e.NewValue);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Actn10_Btn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtBs10.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _pydt.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Actn11_Btn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtBs11.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _pydt.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Actn12_Btn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtBs12.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _pydt.Request_Row.FIGH_FILE_NO)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Save_Default_Report_Action_Parameter()
      {
         try
         {
            var _rpac = iScsc.Report_Action_Parameters.FirstOrDefault(r => r.RPAC_TYPE == "000" && r.CRET_BY == CurrentUser);

            var _user = User_Lov2.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
            Data.Sub_Unit _orgn = null;
            if(Sunt_Lov2.EditValue != null && Sunt_Lov2.EditValue.ToString() != "")
               _orgn = SuntBs1.List.OfType<Data.Sub_Unit>().FirstOrDefault(o => o.CODE == Sunt_Lov2.EditValue);

            if(_rpac == null)
            {
               _rpac = new Data.Report_Action_Parameter() ;
               _rpac.RPAC_TYPE = "000";
               _rpac.FROM_DATE = FromDate2_Date.Value;
               _rpac.TO_DATE = ToDate2_Date.Value;
               _rpac.RECD_OWNR = _user.Count == 0 ? "" : string.Join(":", _user);
               _rpac.COCH_FILE_NO = (long?)Figh_Lov2.EditValue;
               _rpac.CBMT_CODE = (long?)Cbmt_Lov2.EditValue;
               _rpac.SUNT_BUNT_DEPT_ORGN_CODE = _orgn == null ? null : _orgn.BUNT_DEPT_ORGN_CODE;
               _rpac.SUNT_BUNT_DEPT_CODE = _orgn == null ? null : _orgn.BUNT_DEPT_CODE;
               _rpac.SUNT_BUNT_CODE = _orgn == null ? null : _orgn.BUNT_CODE;
               _rpac.SUNT_CODE = _orgn == null ? null : _orgn.CODE;
               iScsc.Report_Action_Parameters.InsertOnSubmit(_rpac);
            }
            else
            {
               _rpac.FROM_DATE = FromDate2_Date.Value;
               _rpac.TO_DATE = ToDate2_Date.Value;
               _rpac.RECD_OWNR = _user.Count == 0 ? null : string.Join(":", _user);
               _rpac.COCH_FILE_NO = (long?)Figh_Lov2.EditValue;
               _rpac.CBMT_CODE = (long?)Cbmt_Lov2.EditValue;
               _rpac.SUNT_BUNT_DEPT_ORGN_CODE = _orgn == null ? null : _orgn.BUNT_DEPT_ORGN_CODE;
               _rpac.SUNT_BUNT_DEPT_CODE = _orgn == null ? null : _orgn.BUNT_DEPT_CODE;
               _rpac.SUNT_BUNT_CODE = _orgn == null ? null : _orgn.BUNT_CODE;
               _rpac.SUNT_CODE = _orgn == null ? null : _orgn.CODE;
            }
            iScsc.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
