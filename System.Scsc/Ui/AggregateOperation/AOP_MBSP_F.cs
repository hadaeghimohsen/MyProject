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
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.AggregateOperation
{
   public partial class AOP_MBSP_F : UserControl
   {
      public AOP_MBSP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int agopindx = 0, aodtindx = 0;
      private long? cbmtcode;
      private DateTime? fromdate = null, todate = null;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         agopindx = AgopBs1.Position;
         aodtindx = AodtBs1.Position;
         AgopBs1.DataSource = iScsc.Aggregation_Operations.Where(a => a.OPRT_TYPE == "001" && (a.OPRT_STAT == "001" || a.OPRT_STAT == "002"));
         AgopBs1.Position = agopindx;
         AodtBs1.Position = aodtindx;
         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void ClerRegn_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         rEGN_CODEGridLookUpEdit.EditValue = null;
         crnt.REGN_CODE = crnt.REGN_PRVN_CODE = crnt.REGN_PRVN_CNTY_CODE = null;

      }

      private void ClerCtgy_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cTGY_CODEGridLookUpEdit.EditValue = null;
         crnt.MTOD_CODE = crnt.CTGY_CODE = null;
      }

      private void ClerCoch_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cOCH_FILE_NOGridLookUpEdit.EditValue = null;

         crnt.COCH_FILE_NO = null;
      }

      private void ClerCbmt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cBMT_CODEGridLookUpEdit.EditValue = null;
         crnt.CBMT_CODE = null;
      }

      private void ClerFromDate_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         FromDate_Dt.Value = null;
         crnt.FROM_DATE = null;
      }

      private void ClerToDate_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         ToDate_Dt.Value = null;

         crnt.TO_DATE = null;
      }

      private void ClerNumMontOfer_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         NumbMontOffr_Txt.EditValue = null;
         NumbOfAttnMont_Txt.EditValue = null;

         crnt.NUMB_MONT_OFFR = null;
         crnt.NUMB_OF_ATTN_MONT = null;
      }

      private void ClerRqtt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         RqttCode_Lov.EditValue = null;

         crnt.RQTT_CODE = null;
      }


      private void ClearAll_Butn_Click(object sender, EventArgs e)
      {
         ClerRegn_Butn_Click(sender, e);
         ClerCtgy_Butn_Click(sender, e);
         ClerCoch_Butn_Click(sender, e);
         ClerCbmt_Butn_Click(sender, e);
         ClerFromDate_Butn_Click(sender, e);
         ClerToDate_Butn_Click(sender, e);
         ClerNumMontOfer_Butn_Click(sender, e);
         ClerRqtt_Butn_Click(sender, e);
         // Clear All Detail Record
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            AgopBs1.EndEdit();
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "009"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "001"),
                     new XAttribute("oprtstat", crnt.OPRT_STAT ?? "001"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("numbmontoffr", crnt.NUMB_MONT_OFFR ?? 0),
                     new XAttribute("numbofattnmont", crnt.NUMB_OF_ATTN_MONT ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0)
                  )
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
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Cncl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "009"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "001"),
                     new XAttribute("oprtstat", "003"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("numbmontoffr", crnt.NUMB_MONT_OFFR ?? 0),
                     new XAttribute("numbofattnmont", crnt.NUMB_OF_ATTN_MONT ?? 0)
                  )
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
            {
               Execute_Query();
               if (AgopBs1.List.Count == 0)
                  Back_Butn_Click(null, null);
               //requery = false;
            }
         }
      }

      private void CretRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "009"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "001"),
                     new XAttribute("oprtstat", "002"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("numbmontoffr", crnt.NUMB_MONT_OFFR ?? 0),
                     new XAttribute("numbofattnmont", crnt.NUMB_OF_ATTN_MONT ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0)
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message.Substring(0, exc.Message.IndexOf("\r\n")));
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void RecStat_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            crnt.REC_STAT = crnt.REC_STAT == "001" ? "002" : "001";

            AodtBs1.EndEdit();

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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void AodtBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (crnt == null) { Pn_MbspInfo.Visible = Gb_Expense.Visible = false; return; }

            RqstBs2.DataSource = iScsc.Requests.First(r => r == crnt.Request);
         }
         catch { }
      }

      private void EndRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "009"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "001"),
                     new XAttribute("oprtstat", "004"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("numbmontoffr", crnt.NUMB_MONT_OFFR ?? 0),
                     new XAttribute("numbofattnmont", crnt.NUMB_OF_ATTN_MONT ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0)
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message.Substring(0, exc.Message.IndexOf("\r\n")));
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void ClerNewCtgy_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         CtgyNew_Lov.EditValue = null;
         crnt.NEW_MTOD_CODE = crnt.NEW_CTGY_CODE = null;
      }

      private void ClerNewCbmt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         CbmtNew_Lov.EditValue = null;
         crnt.NEW_CBMT_CODE = null;
      }

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            //if (tb_master.SelectedTab == tp_001)
            {
               //var rqst = RqstBs1.Current as Data.Request;
               //if (rqst == null) return;

               //long mtodcode = 0;//(long)MtodCode_LookupEdit001.EditValue;
               if (RqttCode_Lov.EditValue == null || RqttCode_Lov.EditValue.ToString() == "")
                  RqttCode_Lov.EditValue = "001";

               long ctgycode = (long)CtgyNew_Lov.EditValue;
               string rqttcode = (string)RqttCode_Lov.EditValue;
               var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "001" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && /*exp.MTOD_CODE == mtodcode &&*/ exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

               FromDate_Dt.Value = DateTime.Now;
               //if (MessageBox.Show(this, "تعداد جلسات با احتساب یک روز در میان می باشد؟", "مشخص شدن تاریخ پایان", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(2 * (expn.NUMB_OF_ATTN_MONT - 1)));
               //else
               //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_OF_ATTN_MONT ?? 30));
               ToDate_Dt.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               NumbOfAttnMont_Txt.Value = expn.NUMB_OF_ATTN_MONT ?? 0;
               NumbMontOffr_Txt.Value = expn.NUMB_MONT_OFER ?? 0;

               //crnt.RQTT_CODE = RqttCode_Lov.EditValue.ToString();
               //crnt.FROM_DATE = FromDate_Dt.Value.Value.Date;
               //crnt.TO_DATE = ToDate_Dt.Value.Value.Date;
               //crnt.NUMB_MONT_OFFR = Convert.ToInt32(NumbMontOffr_Txt.Value);
               //crnt.NUMB_OF_ATTN_MONT = Convert.ToInt32(NumbOfAttnMont_Txt.Value);
            }
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }
      }

      private void CbmtNew_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var cbmt = iScsc.Club_Methods.First(cm => cm.CODE == (long)CbmtNew_Lov.EditValue);
            if (cbmt == null) return;

            var cmwk = cbmt.Club_Method_Weekdays.ToList();

            if (cmwk.Count == 0)
            {
               ClubWkdy_Pn.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }

            foreach (var wkdy in cmwk)
            {
               var rslt = ClubWkdy_Pn.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
               rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
            }
         }
         catch { }
      }

      private void AgopBs1_CurrentChanged(object sender, EventArgs e)
      {
         CbmtNew_Lov_EditValueChanging(null, null);
      }

      private void CbmtCode_Lov_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtCode_Lov.EditValue;

            if (cbmt == null || cbmt.ToString() == "") return;

            var crntcbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(c => c.CODE == (long)cbmt);

            CtgyBs2.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == crntcbmt.MTOD_CODE && c.CTGY_STAT == "002");
         }
         catch (Exception)
         {

         }
      }

      private void AutoAttn_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var adot = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (adot == null) return;

            long ctgycode = (long)CtgyCode_LookupEdit003.EditValue;
            string rqttcode = adot.Request.RQTT_CODE;
            var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "009" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

            StrtDate_dt.Value = DateTime.Now;
            EndDate_dt.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
            NumbAttnMont_Txt.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;
            NumbMontOfer_Txt.EditValue = expn.NUMB_MONT_OFER ?? 0;
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }
      }

      private void RqstBs2_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs2.Current as Data.Request;
            if (rqst == null) { Pn_MbspInfo.Visible = Gb_Expense.Visible = false; return; }

            Pn_MbspInfo.Visible = true;

            if (rqst.RQTT_CODE == "001")
               Gb_Expense.Visible = true;
            else
               Gb_Expense.Visible = false;

         }
         catch
         {

         }
      }

      long rqstindex = 0;

      private void SaveData_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs2.Current as Data.Request;
            rqstindex = RqstBs2.Position;

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            
            StrtDate_dt.CommitChanges();
            EndDate_dt.CommitChanges();

            if (!StrtDate_dt.Value.HasValue) { StrtDate_dt.Value = DateTime.Now; }
            if (!EndDate_dt.Value.HasValue) { EndDate_dt.Value = DateTime.Now.AddDays(29); }

            if (StrtDate_dt.Value.Value.Date > EndDate_dt.Value.Value.Date)
            {
               throw new Exception("تاریخ شروع باید از تاریخ پایان کوچکتر با مساوی باشد");
            }

            iScsc.UCC_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("rqtpcode", "009"),
                     new XAttribute("rqttcode", rqst.RQTT_CODE),
                     new XElement("Request_Row",
                        new XAttribute("fileno", aodt.FIGH_FILE_NO),
                        new XElement("Fighter",
                           new XAttribute("ctgycodednrm", CtgyCode_LookupEdit003.EditValue ?? ""),
                           new XAttribute("cbmtcodednrm", CbmtCode_Lov.EditValue ?? "")
                        ),
                        new XElement("Member_Ship",
                           new XAttribute("strtdate", StrtDate_dt.Value.HasValue ? StrtDate_dt.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("enddate", EndDate_dt.Value.HasValue ? EndDate_dt.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("prntcont", "1"),
                           new XAttribute("numbmontofer", NumbMontOfer_Txt.Text ?? "0"),
                           new XAttribute("numbofattnmont", NumbOfAttnMont_Txt.Text ?? "0"),
                           new XAttribute("numbofattnweek", "0"),
                           new XAttribute("attndaytype", "")
                        )
                     )
                  )
               )
            );
            //tabControl1.SelectedTab = tabPage3;
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }
   }
}
