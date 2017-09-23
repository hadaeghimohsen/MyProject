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
using System.Xml.Linq;
using System.ISP.ExceptionHandlings;

namespace System.ISP.Ui.Admission
{
   public partial class ADM_ADSL_F : UserControl
   {
      public ADM_ADSL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int rqstindx = 0;
      private IEnumerable<Data.Expense> ExpnsCovr;
      private BindingSource ExpnsCovrBs = new BindingSource();

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iISP = new Data.iISPDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            iISP = new Data.iISPDataContext(ConnectionString);
            var Rqids = iISP.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "001" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.RQTT_CODE == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iISP.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               )
               .OrderByDescending(
                  rqst => 
                     rqst.RQST_DATE
               );

            RqstBs1.Position = rqstindx;
         }
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      #region Toolbar Buttons
      private void RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;

            if (rqst == null) return;
            if (rqst.RQID == 0) return;

            if (MessageBox.Show(this, "آیا از انصراف درخواست مطمئن هستید؟", "انصراف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iISP.CNCL_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("rqtpcode", rqst.RQTP_CODE)
                  )
               )
            );
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            RqstBs1.EndEdit();
            SrpbBs1.EndEdit();
            MbspBs1.EndEdit();

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            var rqst = RqstBs1.Current as Data.Request;
            var serv = ServBs1.Current as Data.Service;
            var srpb = SrpbBs1.Current as Data.Service_Public;
            var mbsp = MbspBs1.Current as Data.Member_Ship;

            rqstindx = RqstBs1.Position;

            iISP.ADM_ARQT_F(
               new XElement("Process", 
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("rqtpcode", "001"),
                     new XAttribute("rqttcode", rqst == null ? "001" : rqst.RQTT_CODE),
                     new XAttribute("prvncode", rqst == null || rqst.RQID == 0 ? PrvnCode_Lov.SelectedValue : rqst.REGN_PRVN_CODE),
                     new XAttribute("regncode", rqst == null || rqst.RQID == 0 ? RegnCode_Lov.SelectedValue : rqst.REGN_CODE),
                     new XElement("Service",
                        new XAttribute("fileno", serv == null ? 0 : serv.FILE_NO),
                        new XElement("Service_Public", 
                           new XElement("Agnc_Code", srpb == null ? AgncCode_Lov.SelectedValue : srpb.AGNC_CODE),
                           new XElement("Btrf_Code", srpb == null ? BtrfCode_Lov.SelectedValue : srpb.BTRF_CODE),
                           new XElement("Trfd_Code", srpb == null ? TrfdCode_Lov.SelectedValue : srpb.TRFD_CODE),
                           new XElement("Frst_Name", srpb == null ? FrstName_Txt.Text : srpb.FRST_NAME),
                           new XElement("Last_Name", srpb == null ? LastName_Txt.Text : srpb.LAST_NAME),
                           new XElement("Fath_Name", srpb == null ? FathName_Txt.Text : srpb.FATH_NAME),
                           new XElement("Natl_Code", srpb == null ? NatlCode_Txt.Text : srpb.NATL_CODE),
                           new XElement("Brth_Date", srpb == null ? BrthDate_Dat.Value.Value.ToString("yyyy-MM-dd") : srpb.BRTH_DATE.Value.ToString("yyyy-MM-dd")),
                           new XElement("Cell_Phon", srpb == null ? CellPhon_Txt.Text : srpb.CELL_PHON),
                           new XElement("Tell_Phon", srpb == null ? TellPhon_Txt.Text : srpb.TELL_PHON),
                           new XElement("Idty_Code", srpb == null ? IdtyCode_Txt.Text : srpb.IDTY_CODE),
                           new XElement("Ownr_Type", srpb == null ? OwnrType_Lov.SelectedValue : srpb.OWNR_TYPE),
                           new XElement("User_Name", srpb == null ? UserName_Txt.Text : srpb.USER_NAME),
                           new XElement("Pass_Word", srpb == null ? PassWord_Txt.Text : srpb.PASS_WORD),
                           new XElement("Telg_Chat_Code", srpb == null ? 0 : srpb.TELG_CHAT_CODE),
                           new XElement("Frst_Name_Ownr_Line", srpb == null ? FrstNameOwnrLine_Txt.Text : srpb.FRST_NAME_OWNR_LINE),
                           new XElement("Last_Name_Ownr_Line", srpb == null ? LastNameOwnrLine_Txt.Text : srpb.LAST_NAME_OWNR_LINE),
                           new XElement("Natl_Code_Ownr_Line", srpb == null ? NatlCodeOwnrLine_Txt.Text : srpb.NATL_CODE_OWNR_LINE),
                           new XElement("Line_Numb_Serv", srpb == null ? LineNumServ_Txt.Text : srpb.LINE_NUMB_SERV),
                           new XElement("Post_Adrs", srpb == null ? PostAdrs_Txt.Text : srpb.POST_ADRS),
                           new XElement("Emal_Adrs", srpb == null ? EmalAdrs_Txt.Text : srpb.EMAL_ADRS),
                           new XElement("Sunt_Bunt_Dept_Orgn_Code", srpb == null ? OrgnCode_Lov.SelectedValue : srpb.SUNT_BUNT_DEPT_ORGN_CODE),
                           new XElement("Sunt_Bunt_Dept_Code", srpb == null ? DeptCode_Lov.SelectedValue : srpb.SUNT_BUNT_DEPT_CODE),
                           new XElement("Sunt_Bunt_Code", srpb == null ? BuntCode_Lov.SelectedValue : srpb.SUNT_BUNT_CODE),
                           new XElement("Sunt_Code", srpb == null ? SuntCode_Lov.SelectedValue : srpb.SUNT_CODE)                           
                        ),
                        new XElement("Member_Ship", 
                           new XAttribute("strtdate", mbsp == null ? StrtDate_Dat.Value.Value.ToString("yyyy-MM-dd") : mbsp.STRT_DATE.Value.ToString("yyyy-MM-dd")),
                           new XAttribute("enddate", mbsp == null ? EndDate_Dat.Value.Value.ToString("yyyy-MM-dd") : mbsp.END_DATE.Value.ToString("yyyy-MM-dd")),
                           new XAttribute("numbmontofer", mbsp == null ? (int?)NumbMontOfer_Nud.Value : mbsp.NUMB_MONT_OFER),
                           new XAttribute("volmoftrfc", mbsp == null ? (int?)VolmOfTrfc_Nud.Value : mbsp.VOLM_OF_TRFC)
                        )
                     )
                  )
               )
            );

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)         
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            switch (exc.Message)
            {
               case "Object reference not set to an instance of an object.":
               case "Value cannot be null.\r\nParameter name: value":
                  MessageBox.Show("لطفا گزینه های قرمز که اجباری هستند را وارد کنید");
                  break;
               default:
                  MessageBox.Show(exc.Message);
                  break;
            }
            iISP.SaveException(exc);
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

      private void RqstBnDefaultPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnSettingPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnASav1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnAResn1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnADocPicProfile1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnADoc1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var Rg1 = iISP.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 08 /* Execute Rqrq_Dfin_F */){Input = Rg1},
                     new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", Rqtp_Lov.Tag)))}
                  })
               );
         }
      }
      #endregion      

      #region Binding Events
      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               Gb_Expense.Visible = true;
               Gb_ExpenseItem.Visible = true;
               ExpnBs1.DataSource = 
                  iISP.Expenses
                  .Where(
                     ex => 
                        ex.AUTO_ADD == "001" &&
                        ex.EXPN_STAT == "002" &&
                        ex.Expense_Type.Request_Requester.RQTP_CODE == "001" && 
                        ex.Expense_Type.Request_Requester.RQTT_CODE == "001"
                  );

               //Btn_RqstDelete1.Visible = true; Btn_RqstSav1.Visible = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               Gb_ExpenseItem.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
            }
            else if (Rqst.RQID == 0)
            {
               StatusSaving_Sic.StateIndex = 0; // New Request
               Gb_Expense.Visible = false;
               Gb_ExpenseItem.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false; DefaultTabPage001();
            }
         }
         catch
         {
            StatusSaving_Sic.StateIndex = 0; // Error On Request
            Gb_Expense.Visible = false;
            Gb_ExpenseItem.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false; DefaultTabPage001();
         }
      }

      private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
      {
         try
         {
            if (requery)
               return;

            var rqst = RqstBs1.Current as Data.Request;

            if (rqst.RQID == 0)
            {
               StatusSaving_Sic.StateIndex = 0;
            }
            else
            {
               StatusSaving_Sic.StateIndex = 2;
            }
         }
         catch (Exception exc)
         {
            iISP.SaveException(exc);
         }
      }

      private void BackMoveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ExpnsCovrBs.MovePrevious();

            var expn = ExpnsCovrBs.Current as Data.Expense;

            StrtDate_Dat.Value = DateTime.Now;
            EndDate_Dat.Value = DateTime.Now.AddMonths((int)expn.NUMB_OF_MONT);
            NumbMontOfer_Nud.Value = 0;
            VolmOfTrfc_Nud.Value = (decimal)expn.VOLM_OF_TRFC;
            MbspBs1.EndEdit();
            VolmOfTrfc_Nud.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NextMoveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ExpnsCovrBs.MoveNext();

            var expn = ExpnsCovrBs.Current as Data.Expense;

            StrtDate_Dat.Value = DateTime.Now;
            EndDate_Dat.Value = DateTime.Now.AddMonths((int)expn.NUMB_OF_MONT);
            NumbMontOfer_Nud.Value = 0;
            VolmOfTrfc_Nud.Value = (decimal)expn.VOLM_OF_TRFC;
            MbspBs1.EndEdit();
            VolmOfTrfc_Nud.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PydtsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var crnt = PydtsBs1.Current as Data.Payment_Detail;
            if(crnt.Expense.EXPN_TYPE == "002") // کالا
            {
               PdcsGc1.Visible = true;
               AddPdcs_Butn.Visible = true;
               DelPdcs_Butn.Visible = true;
               SavePdcs_Butn.Visible = true;
            }
            else if(crnt.Expense.EXPN_TYPE == "001") // خدمت
            {
               PdcsGc1.Visible = false;
               AddPdcs_Butn.Visible = false;
               DelPdcs_Butn.Visible = false;
               SavePdcs_Butn.Visible = false;
            }
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }
      #endregion

      #region Button Events
      private void AutoTarfSet_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long? btrfcode = (long?)BtrfCode_Lov.SelectedValue;
            long? trfdcode = (long?)TrfdCode_Lov.SelectedValue;

            ExpnsCovr = iISP.Expenses.Where(ex => ex.EXPN_STAT == "002" && ex.BTRF_CODE == btrfcode && ex.TRFD_CODE == trfdcode && ex.Expense_Type.Request_Requester.RQTP_CODE == "001" && ex.Expense_Type.Request_Requester.RQTT_CODE == "001" && ex.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && ex.Expense_Type.Request_Requester.Regulation.TYPE == "001");
            ExpnsCovrBs.DataSource = ExpnsCovr;

            if (ExpnsCovr.Count() <= 1) { Expns_Pnl.Visible = false; }
            else { Expns_Pnl.Visible = true; }

            var expn = ExpnsCovr.FirstOrDefault();
            StrtDate_Dat.Value = DateTime.Now;
            EndDate_Dat.Value = DateTime.Now.AddMonths((int)expn.NUMB_OF_MONT);
            NumbMontOfer_Nud.Value = 0;
            VolmOfTrfc_Nud.Value = (decimal)expn.VOLM_OF_TRFC;
            MbspBs1.EndEdit();
            VolmOfTrfc_Nud.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void AddExpnInPydt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            // اگر در جدول هزینه قبلا رکوردی درج شده باشد
            if (rqst == null) return;

            var expn = ExpnBs1.Current as Data.Expense;

            // چک میکنیم که قبلا از این آیتم هزینه در جدول ریز هزینه وجود نداشته باشد
            if (!PydtsBs1.List.OfType<Data.Payment_Detail>().Any(p => p.EXPN_CODE == expn.CODE))
            {
               PydtsBs1.AddNew();
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               iISP.Payment_Details.InsertOnSubmit(pydt);
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; pydt.PAY_STAT = "001"; pydt.PYMT_RQST_RQID = rqst.RQID; pydt.PYMT_CASH_CODE = ex.Expense_Type.Expense_Cashes.FirstOrDefault(ec => ec.REGN_PRVN_CODE == rqst.REGN_PRVN_CODE && ec.REGN_CODE == rqst.REGN_CODE).CASH_CODE; pydt.RQRO_RWNO = 1; pydt.RCPT_MTOD = "001"; });               
            }
            else
            {
               var pydt = PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.EXPN_CODE == expn.CODE).First();
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.QNTY += 1; });
            }
            //PydtsBs1.EndEdit();
            SaveOthrExpn_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void IncrsOthrQntyExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            
            /// اولین گام 
            /// اینکه آیتم هزینه مبلغ اتوماتیک برای درخواست نباشد.
            /// مبلغ های اتوماتیک به هیچ عنوان نباید تغییر کنند

            if (iISP.Expenses.FirstOrDefault(exp => exp.CODE == pydt.EXPN_CODE).AUTO_ADD == "002")
               throw new Exception("شما نمی توانید هزینه اتوماتیک محاسبه شده برای درخواست را تغییر دهید");

            pydt.QNTY += 1;
            PydtsBs1.EndEdit();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void DecrsOthrQntyExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pydt = PydtsBs1.Current as Data.Payment_Detail;

            /// اولین گام 
            /// اینکه آیتم هزینه مبلغ اتوماتیک برای درخواست نباشد.
            /// مبلغ های اتوماتیک به هیچ عنوان نباید تغییر کنند

            if (iISP.Expenses.FirstOrDefault(exp => exp.CODE == pydt.EXPN_CODE).AUTO_ADD == "002")
               throw new Exception("شما نمی توانید هزینه اتوماتیک محاسبه شده برای درخواست را تغییر دهید");

            pydt.QNTY = pydt.QNTY <= 1 ? (short)1 : (short)(pydt.QNTY - 1);

            PydtsBs1.EndEdit();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void SaveOthrExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            StatusSaving_Sic.StateIndex = 2; // Saving Request
            PydtsBs1.EndEdit();
            iISP.SubmitChanges();
            rqstindx = RqstBs1.Position;
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void DelOthrExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pydt = PydtsBs1.Current as Data.Payment_Detail;

            if (pydt != null && MessageBox.Show(this, "آیا با پاک کردن ردیف هزینه برای درخواست مطمئن هستید؟", "حذف ردیف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            /// اولین گام 
            /// اینکه آیتم هزینه مبلغ اتوماتیک برای درخواست نباشد.
            /// مبلغ های اتوماتیک به هیچ عنوان نباید تغییر کنند

            if (iISP.Expenses.FirstOrDefault(exp => exp.CODE == pydt.EXPN_CODE).AUTO_ADD == "002")
               throw new Exception("شما نمی توانید هزینه اتوماتیک محاسبه شده برای درخواست را حذف کنید");

            rqstindx = RqstBs1.Position;

            iISP.DEL_PYDT_P(pydt.CODE, pydt.PYMT_CASH_CODE, pydt.PYMT_RQST_RQID, pydt.RQRO_RWNO, pydt.EXPN_CODE);

            requery = true;

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void CmpsPymt_Butn_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs1.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 12 /* Execute Pymt_Mtod_F */, SendType.Self) { Input = pymt }
            );
         }
      }

      private void AddPdcs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PdcsBs1.AddNew();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void DelPdcs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = PdcsBs1.Current as Data.Payment_Detail_Commodity_Sale;

            if (crnt != null && MessageBox.Show(this, "آیا با پاک کردن ردیف کالا برای درخواست مطمئن هستید؟", "حذف ردیف کالا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            rqstindx = RqstBs1.Position;

            iISP.Payment_Detail_Commodity_Sales.DeleteOnSubmit(crnt);

            iISP.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void SavePdcs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            StatusSaving_Sic.StateIndex = 2; // Saving Request
            PdcsBs1.EndEdit();
            rqstindx = RqstBs1.Position;

            iISP.SubmitChanges();
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void CashPymt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void InDebt_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion
   }
}
