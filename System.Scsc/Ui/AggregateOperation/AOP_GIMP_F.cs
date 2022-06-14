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
using System.Scsc.ExtCode;

namespace System.Scsc.Ui.AggregateOperation
{
   public partial class AOP_GIMP_F : UserControl
   {
      public AOP_GIMP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string fngrprnt = "";

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);

         AgopBs1.DataSource = iScsc.Aggregation_Operations.Where(a => a.OPRT_TYPE == "007" && (a.OPRT_STAT == "001" || a.OPRT_STAT == "002"));
         
         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void CnclGimp_Butn_Click(object sender, EventArgs e)
      {

      }

      private void NewGimp_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaveGimp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            AgopBs1.EndEdit();
            iScsc.SubmitChanges();
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

      private void AddFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            if (agop.Aggregation_Operation_Details.Any(d => d.FNGR_PRNT == FngrPrnt_Txt.Text)) return;

            var aodt = AodtBs1.AddNew() as Data.Aggregation_Operation_Detail;
            aodt.Aggregation_Operation = agop;
            aodt.FNGR_PRNT = FngrPrnt_Txt.Text;
            aodt.REC_STAT = "002";

            iScsc.Aggregation_Operation_Details.InsertOnSubmit(aodt);

            iScsc.SubmitChanges();
            //iScsc.INS_AODT_P(agop.CODE, 1, null, null, null, null, null, null, "002", "001", null, null, null, null, null, null, null, null, FngrPrnt_Txt.Text);
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

      private void DelFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            iScsc.Aggregation_Operation_Details.DeleteOnSubmit(aodt);
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

      private void TrncFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            iScsc.ExecuteCommand("DELETE dbo.Aggregation_Operation_Detail WHERE AGOP_CODE = {0};", agop.CODE);

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

      private void SaveDpstAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SaveGimp_Butn_Click(null, null);

            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", agop.CODE),
                     new XAttribute("regnprvncntycode", agop.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", agop.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", agop.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", agop.RQTP_CODE ?? "020"),
                     new XAttribute("rqttcode", agop.RQTT_CODE ?? "004"),
                     //new XAttribute("mtodcode", agop.MTOD_CODE ?? 0),
                     //new XAttribute("ctgycode", agop.CTGY_CODE ?? 0),
                     //new XAttribute("cochfileno", agop.COCH_FILE_NO ?? 0),
                     //new XAttribute("cbmtcode", agop.CBMT_CODE ?? 0),
                     new XAttribute("fromdate", agop.FROM_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("todate", agop.FROM_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("oprttype", agop.OPRT_TYPE ?? "007"),
                     new XAttribute("oprtstat", "004"),
                     new XAttribute("agopdesc", agop.AGOP_DESC ?? "خدایا بابت روزی امروز ازت ممنونم"),
                     new XAttribute("suntbuntdeptorgncode", agop.SUNT_BUNT_DEPT_ORGN_CODE ?? "00"),
                     new XAttribute("suntbuntdeptcode", agop.SUNT_BUNT_DEPT_CODE ?? "00"),
                     new XAttribute("suntbuntcode", agop.SUNT_BUNT_CODE ?? "00"),
                     new XAttribute("suntcode", agop.SUNT_CODE ?? "0000"),
                     new XAttribute("lettno", agop.LETT_NO ?? ""),
                     new XAttribute("lettownr", agop.LETT_OWNR ?? ""),
                     new XAttribute("lettdate", agop.LETT_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("expnamnt", agop.EXPN_AMNT ?? 0),
                     new XAttribute("rcptmtod", agop.RCPT_MTOD ?? "003")
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
               Execute_Query();
         }
      }

      private void CbmtCode_Lov_Popup(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtCode_Lov.EditValue;

            if (cbmt == null || cbmt.ToString() == "") return;

            var crntcbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(c => c.CODE == (long)cbmt);

            CtgyBs1.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == crntcbmt.MTOD_CODE && c.CTGY_STAT == "002");

            var cbmtt = iScsc.Club_Methods.First(cm => cm.CODE == (long)CbmtCode_Lov.EditValue);
            if (cbmtt == null) return;

            var cmwk = cbmtt.Club_Method_Weekdays.ToList();

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

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            long ctgycode = (long)CtgyCode_Lov.EditValue;
            var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "001" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && /*exp.MTOD_CODE == mtodcode &&*/ exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

            /// سیستم تغییر تاریخ شروع و پایان
            /// Ctrl : تاریخ پایان بر اساس تاریخ شروع به تعداد دوره
            /// 

            if (ModifierKeys == Keys.Control)
            {
               // تاریخ پایان بر اساس تاریخ شروعی که وارد شده محاسبه گردد
               StrtDate_DateTime001.CommitChanges();
               var strtdate = StrtDate_DateTime001.Value;
               if (strtdate.HasValue)
                  EndDate_DateTime001.Value = strtdate.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               else
               {
                  StrtDate_DateTime001.Value = DateTime.Now;
                  EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
            }
            else if (ModifierKeys == Keys.Shift)
            {
               // تاریخ شروع به اولین روز همان ماه برگردد
               StrtDate_DateTime001.CommitChanges();
               var strtdate = StrtDate_DateTime001.Value;
               if (strtdate.HasValue)
               {
                  var day = StrtDate_DateTime001.GetText("dd").ToInt32();
                  if (day != 1)
                     StrtDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((day - 1) * -1);
                  EndDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
               else
               {
                  StrtDate_DateTime001.Value = DateTime.Now;
                  var day = StrtDate_DateTime001.GetText("dd").ToInt32();
                  if (day != 1)
                     StrtDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((day - 1) * -1);
                  EndDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
            }
            else
            {
               StrtDate_DateTime001.Value = DateTime.Now;
               EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
            }

            NumbOfAttnMont_TextEdit001.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;

            SaveGimp_Butn_Click(null, null);
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }         

      }

      private void SaveCycl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SaveGimp_Butn_Click(null, null);

            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            if (agop.LETT_NO == null) { MessageBox.Show("شماره نامه قرارداد وارد نشد است"); return; }
            if (agop.LETT_DATE == null) { MessageBox.Show("تاریخ تنظیم قرارداد وارد نشده است"); return; }
            if (agop.LETT_OWNR == null) { MessageBox.Show("نام طرف قرارداد وارد نشده است"); return; }

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", agop.CODE),
                     new XAttribute("regnprvncntycode", agop.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", agop.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", agop.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", agop.RQTP_CODE ?? "001"),
                     new XAttribute("rqttcode", agop.RQTT_CODE ?? "001"),
                     //new XAttribute("mtodcode", agop.MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", agop.NEW_CTGY_CODE ?? 0),
                     new XAttribute("numbofattnmont", agop.NUMB_OF_ATTN_MONT ?? 0),
                     new XAttribute("newcbmtcode", agop.NEW_CBMT_CODE ?? 0),
                     new XAttribute("fromdate", agop.FROM_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("todate", agop.TO_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("oprttype", agop.OPRT_TYPE ?? "007"),
                     new XAttribute("oprtstat", "004"),
                     new XAttribute("agopdesc", agop.AGOP_DESC ?? "خدایا بابت روزی امروز ازت ممنونم"),
                     new XAttribute("suntbuntdeptorgncode", agop.SUNT_BUNT_DEPT_ORGN_CODE ?? "00"),
                     new XAttribute("suntbuntdeptcode", agop.SUNT_BUNT_DEPT_CODE ?? "00"),
                     new XAttribute("suntbuntcode", agop.SUNT_BUNT_CODE ?? "00"),
                     new XAttribute("suntcode", agop.SUNT_CODE ?? "0000"),
                     new XAttribute("lettno", agop.LETT_NO ?? ""),
                     new XAttribute("lettownr", agop.LETT_OWNR ?? ""),
                     new XAttribute("lettdate", agop.LETT_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("expnamnt", agop.EXPN_AMNT ?? 0),
                     new XAttribute("rcptmtod", agop.RCPT_MTOD ?? "003")
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
               Execute_Query();
         }
      }

      private void AddRangFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var from = FromFngrPrnt_Txt.Text.ToInt64();
            var to = ToFngrPrnt_Txt.Text.ToInt64();

            for (long i = from; i <= to; i++)
            {
               FngrPrnt_Txt.EditValue = i;
               AddFngrPrnt_Butn_Click(null, null);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
