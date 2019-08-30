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
         iScsc = new Data.iScscDataContext(ConnectionString);
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
            else if (tc_master.SelectedTab == tp_005)
            {
               var rqtps = Rqtp_Lov5.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               var users = User_Lov5.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();

               long? fileno = null;//, coch = null;

               if (Figh_Lov5.EditValue != null && Figh_Lov5.EditValue.ToString() != "")
                  fileno = (long?)Figh_Lov5.EditValue;

               PydtBs5.DataSource =
                  iScsc.Payment_Details
                  .Where(pd =>
                     pd.TRAN_DATE.Value.Date >= FromDate5_Date.Value.Value.Date &&
                     pd.TRAN_DATE.Value.Date <= ToDate5_Date.Value.Value.Date &&

                     /*pd.Request_Row.Request.RQST_DATE.Value.TimeOfDay >= FromTime2_Te.Time.TimeOfDay &&
                     pd.Request_Row.Request.RQST_DATE.Value.TimeOfDay <= ToTime2_Te.Time.TimeOfDay &&*/

                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (rqtps.Count == 0 || rqtps.Contains(pd.Request_Row.RQTP_CODE)) &&
                     (users.Count == 0 || (users.Contains(pd.CRET_BY) || users.Contains(pd.MDFY_BY))) &&
                     (pd.Request_Row.FIGH_FILE_NO == (fileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM))
                  );
            }
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
                     (Fga_Uclb_U.Contains(pc.Payment.CLUB_CODE_DNRM))
                  );
            }
            else if(tc_master.SelectedTab == tp_007)
            {
               VInvtBs1.DataSource = iScsc.V_Inventories;
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
         FromDate5_Date.CommitChanges(); ToDate5_Date.CommitChanges();
         FromDate6_Date.CommitChanges(); ToDate6_Date.CommitChanges();

         if (tc_master.SelectedTab == tp_001)
         {
            if (!FromDate1_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate1_Date.Focus(); return; }
            if (!ToDate1_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate1_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if(!checkValidateDate(FromDate1_Date.Value.Value.Date))return;

            FromDate6_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate5_Date.Value = FromDate1_Date.Value;
            ToDate6_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate5_Date.Value = ToDate1_Date.Value;

            Rqtp_Lov5.EditValue = Rqtp_Lov2.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov.EditValue;
            Figh_Lov5.EditValue = Figh_Lov2.EditValue = Figh_Lov3.EditValue = Figh_Lov.EditValue;
            User_Lov5.EditValue = User_Lov2.EditValue = User_Lov3.EditValue = User_Lov.EditValue;
         }
         else if(tc_master.SelectedTab == tp_002)
         {
            if (!FromDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate2_Date.Focus(); return; }
            if (!ToDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate2_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate2_Date.Value.Value.Date)) return;

            FromDate6_Date.Value = FromDate1_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate5_Date.Value = FromDate2_Date.Value;
            ToDate6_Date.Value = ToDate1_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate5_Date.Value = ToDate2_Date.Value;
            
            if(FromTime2_Te.EditValue == null)
               FromTime2_Te.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
            if(ToTime2_Te.EditValue == null)
               ToTime2_Te.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 00);

            Rqtp_Lov5.EditValue = Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov2.EditValue;
            Figh_Lov5.EditValue = Figh_Lov.EditValue = Figh_Lov3.EditValue = Figh_Lov2.EditValue;
            User_Lov5.EditValue = User_Lov.EditValue = User_Lov3.EditValue = User_Lov2.EditValue;
         }
         else if (tc_master.SelectedTab == tp_003)
         {
            if (!FromDate3_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate3_Date.Focus(); return; }
            if (!ToDate3_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate3_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate3_Date.Value.Value.Date)) return;

            FromDate6_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value;
            ToDate6_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value;

            Rqtp_Lov5.EditValue = Rqtp_Lov2.EditValue = Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue;
            Figh_Lov5.EditValue = Figh_Lov2.EditValue = Figh_Lov.EditValue = Figh_Lov3.EditValue;
            User_Lov5.EditValue = User_Lov2.EditValue = User_Lov.EditValue = User_Lov3.EditValue;
         }
         else if (tc_master.SelectedTab == tp_004)
         {
            if (!FromDate4_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate4_Date.Focus(); return; }
            if (!ToDate4_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate4_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate4_Date.Value.Value.Date)) return;

            FromDate6_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate5_Date.Value = FromDate4_Date.Value;
            ToDate6_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate5_Date.Value = ToDate4_Date.Value;
         }
         else if (tc_master.SelectedTab == tp_005)
         {
            if (!FromDate5_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate5_Date.Focus(); return; }
            if (!ToDate5_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate5_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate5_Date.Value.Value.Date)) return;

            FromDate6_Date.Value = FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate5_Date.Value;
            ToDate6_Date.Value = ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate5_Date.Value;

            Rqtp_Lov2.EditValue = Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov5.EditValue;
            Figh_Lov2.EditValue = Figh_Lov.EditValue = Figh_Lov3.EditValue = Figh_Lov5.EditValue;
            User_Lov2.EditValue = User_Lov.EditValue = User_Lov3.EditValue = User_Lov5.EditValue;
         }
         else if (tc_master.SelectedTab == tp_006)
         {
            if (!FromDate6_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate5_Date.Focus(); return; }
            if (!ToDate6_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate5_Date.Focus(); return; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(FromDate6_Date.Value.Value.Date)) return;

            FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value = FromDate5_Date.Value = FromDate6_Date.Value;
            ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value = ToDate5_Date.Value = ToDate6_Date.Value;
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
         //if (tc_master.SelectedTab == tp_001)
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
                              string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\" code=\"{4}\" ctgycode=\"{5}\"/></Request>", 
                                            FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                                            ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                                            User_Lov2.EditValue, 
                                            Figh_Lov2.EditValue/*cochfileno*/, 
                                            Cbmt_Lov2.EditValue/*cbmtcode*/, 
                                            Ctgy_lov2.EditValue )
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tc_master.SelectedTab == tp_001)
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
                              string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\" code=\"{4}\" ctgycode=\"{5}\"/></Request>", 
                                 FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                                 ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), 
                                 User_Lov2.EditValue, 
                                 Figh_Lov2.EditValue, 
                                 Cbmt_Lov2.EditValue/*cbmtcode*/, 
                                 Ctgy_lov2.EditValue 
                              )
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tc_master.SelectedTab == tp_001)
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
            Pydtran_Gv.PostEditor();
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
         }
         catch {}
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
            cochfileno = (long?)e.NewValue;
         }
         catch { }
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
   }
}
