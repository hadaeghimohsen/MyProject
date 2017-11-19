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

      private void Execute_Query()
      {
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
                     (pm.ACTN_DATE.Value.Date >= FromDate1_Date.Value.Value.Date ) &&
                     (pm.ACTN_DATE.Value.Date <= ToDate1_Date.Value.Value.Date ) &&
                     (rqtps.Count == 0 || rqtps.Contains(pm.Request_Row.RQTP_CODE)) &&
                     (rcmts.Count == 0 || rcmts.Contains(pm.RCPT_MTOD)) &&
                     (users.Count == 0 || (users.Contains(pm.CRET_BY) || users.Contains(pm.MDFY_BY))) &&
                     (pm.Request_Row.FIGH_FILE_NO == (fileno ?? pm.Request_Row.FIGH_FILE_NO)) &&
                     (Fga_Uclb_U.Contains(pm.Payment.CLUB_CODE_DNRM))
                  );
            }
            else if(tc_master.SelectedTab == tp_002)
            {
               var rqtps = Rqtp_Lov2.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               var users = User_Lov2.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Select(i => i.Value).ToList();
               
               long? fileno = null, coch = null;
               
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
                     (pd.Request_Row.FIGH_FILE_NO == (fileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM))
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
                     (Fga_Uclb_U.Contains(pd.Payment.CLUB_CODE_DNRM))
                  );
            }
            else if(tc_master.SelectedTab == tp_004)
            {
               MiscExpenseBs4.DataSource =
                  iScsc.Misc_Expenses
                  .Where(me =>
                     me.VALD_TYPE == "002" &&
                     me.DELV_STAT == "002" &&
                     me.CRET_DATE.Value.Date >= FromDate4_Date.Value.Value.Date &&
                     me.CRET_DATE.Value.Date <= ToDate4_Date.Value.Value.Date  &&
                     (Fga_Uclb_U.Contains(me.CLUB_CODE))
                  );
            }
         }
         catch { }
      }

      private void Find_Butn_Click(object sender, EventArgs e)
      {
         if (tc_master.SelectedTab == tp_001)
         {
            if (!FromDate1_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate1_Date.Focus(); return; }
            if (!ToDate1_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate1_Date.Focus(); return; }

            FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate1_Date.Value;
            ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate1_Date.Value;

            Rqtp_Lov2.EditValue = Rqtp_Lov3.EditValue = Rqtp_Lov.EditValue;
            Figh_Lov2.EditValue = Figh_Lov3.EditValue = Figh_Lov.EditValue;
            User_Lov2.EditValue = User_Lov3.EditValue = User_Lov.EditValue;
         }
         else if(tc_master.SelectedTab == tp_002)
         {
            if (!FromDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate2_Date.Focus(); return; }
            if (!ToDate2_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate2_Date.Focus(); return; }

            FromDate1_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate2_Date.Value;
            ToDate1_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate2_Date.Value;
            
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

            FromDate1_Date.Value = FromDate2_Date.Value = FromDate4_Date.Value = FromDate3_Date.Value;
            ToDate1_Date.Value = ToDate2_Date.Value = ToDate4_Date.Value = ToDate3_Date.Value;

            Rqtp_Lov2.EditValue = Rqtp_Lov.EditValue = Rqtp_Lov3.EditValue;
            Figh_Lov2.EditValue = Figh_Lov.EditValue = Figh_Lov3.EditValue;
            User_Lov2.EditValue = User_Lov.EditValue = User_Lov3.EditValue;
         }
         else if (tc_master.SelectedTab == tp_004)
         {
            if (!FromDate4_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate4_Date.Focus(); return; }
            if (!ToDate4_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate4_Date.Focus(); return; }

            FromDate1_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value;
            ToDate1_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value;
         }
         Execute_Query();
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
         catch (Exception exc)
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
         catch (Exception exc)
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
         catch (Exception exc)
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selection"), 
                              new XAttribute("modual", /*GetType().Name*/formName), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"),
                              string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\"/></Request>", FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), User_Lov2.EditValue, Figh_Lov2.EditValue )
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Default"), 
                              new XAttribute("modual", /*GetType().Name*/ formName), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                              string.Format("<Request fromrqstdate=\"{0}\" torqstdate=\"{1}\" cretby=\"{2}\"><Club_Method cochfileno=\"{3}\"/></Request>", FromDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate2_Date.Value.Value.Date.ToString("yyyy-MM-dd"), User_Lov2.EditValue, Figh_Lov2.EditValue )
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
            var misxexpn = MiscExpenseBs4.Current as Data.Misc_Expense;
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
   }
}
