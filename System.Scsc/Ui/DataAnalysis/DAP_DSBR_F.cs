using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess;
using System.JobRouting.Jobs;
using System.Globalization;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.DataAnalysis
{
   public partial class DAP_DSBR_F : UserControl
   {
      public DAP_DSBR_F()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void ExecuteQuery_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }      

      private void Execute_Query()
      {
         switch (Tc_Master.SelectedTabPage.Name)
         {
            case "tp_001":
               new Threading.Thread(TabPage001_Starting).Start();
               break;
            case "tp_002":
               new Threading.Thread(TabPage002_Starting).Start();
               break;
            case "tp_003":
               new Threading.Thread(TabPage003_Starting).Start();
               break;
            default:
               break;
         }
      }

      #region Tab Page 001
      private void TabPage001_Starting()
      {
         try
         {
            //FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "003") );
            
            if(InvokeRequired)
            {
               Invoke(new Action(() =>  TabPage001_Filling()));
            }
            else
            {
               TabPage001_Filling();
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void TabPage001_Filling()
      {
         #region Temp Query
         var freefigh =
            (from f in iScsc.Fighters
             join fp in iScsc.Fighter_Publics on f.FILE_NO equals fp.FIGH_FILE_NO
             where f.FGPB_TYPE_DNRM == "005" &&
                   fp.RECT_CODE == "001" &&
                   fp.CELL_PHON != null &&
                   fp.CELL_PHON.Length >= 10
             select new { fp.CELL_PHON, f.SEX_TYPE_DNRM }).Distinct();

         #endregion

         #region آمار مشترکین
         PermServ_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001").Count().ToString();
         FreeServ_Lb.Text = freefigh.Count().ToString();
            
         TotlServ_Lb.Text = (Convert.ToInt64(FreeServ_Lb.Text) + Convert.ToInt64(PermServ_Lb.Text)).ToString();
         TotlMan_Lb.Text = (FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001").Count() + freefigh.Where(f => f.SEX_TYPE_DNRM == "001").Count()).ToString();
         TotlMen_Lb.Text = (FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002").Count() + freefigh.Where(f => f.SEX_TYPE_DNRM == "002").Count()).ToString();
         TotlActvMan_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date).Count().ToString();
         TotlActvMen_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date).Count().ToString();
         TotlDactMan_Lb.Text = (Convert.ToInt64(TotlMan_Lb.Text) - Convert.ToInt64(TotlActvMan_Lb.Text)).ToString();
         TotlDactMen_Lb.Text = (Convert.ToInt64(TotlMen_Lb.Text) - Convert.ToInt64(TotlActvMen_Lb.Text)).ToString();
         #endregion

         #region آمار شماره موبایل مشترکین
         TotlPermCellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count().ToString();
         TotlFreeCellPhon_Lb.Text = freefigh.Count().ToString();
         TotlCellPhon_Lb.Text = (Convert.ToInt64(TotlPermCellPhon_Lb.Text) + Convert.ToInt64(TotlFreeCellPhon_Lb.Text)).ToString();

         TotlManCellPhon_Lb.Text = (FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count() + freefigh.Where(f => f.SEX_TYPE_DNRM == "001").Count()).ToString();
         TotlMenCellPhon_Lb.Text = (FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count() + freefigh.Where(f => f.SEX_TYPE_DNRM == "002").Count()).ToString();
         TotlActvManCellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date && f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count().ToString();
         TotlActvMenCellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date && f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count().ToString();
         TotlDactManCellPhon_Lb.Text = (Convert.ToInt64(TotlManCellPhon_Lb.Text) - Convert.ToInt64(TotlActvManCellPhon_Lb.Text)).ToString();
         TotlDactMenCellPhon_Lb.Text = (Convert.ToInt64(TotlMenCellPhon_Lb.Text) - Convert.ToInt64(TotlActvMenCellPhon_Lb.Text)).ToString();

         CellPhonServ_Cc.DataSource = new BindingSource(
               new BindingList<Data.Fighter>(
                  FighBs.List.OfType<Data.Fighter>()
                  .Where(f => f.FGPB_TYPE_DNRM == "001" 
                           && f.CELL_PHON_DNRM != null 
                           && f.CELL_PHON_DNRM.Length >= 10)
                  .ToList()
            ), null
         );
         #endregion

         #region آمار شماره ثابت مشترکین
         TotlManTellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.TELL_PHON_DNRM != null && f.TELL_PHON_DNRM.Length >= 4).Count().ToString();
         TotlMenTellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.TELL_PHON_DNRM != null && f.TELL_PHON_DNRM.Length >= 4).Count().ToString();
         TotlTellPhon_Lb.Text = (Convert.ToInt64(TotlManTellPhon_Lb.Text) + Convert.ToInt64(TotlMenTellPhon_Lb.Text)).ToString();

         TotlActvManTellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date && f.TELL_PHON_DNRM != null && f.TELL_PHON_DNRM.Length >= 4).Count().ToString();
         TotlActvMenTellPhon_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date && f.TELL_PHON_DNRM != null && f.TELL_PHON_DNRM.Length >= 4).Count().ToString();
         TotlDactManTellPhon_Lb.Text = (Convert.ToInt64(TotlManTellPhon_Lb.Text) - Convert.ToInt64(TotlActvManTellPhon_Lb.Text)).ToString();
         TotlDactMenTellPhon_Lb.Text = (Convert.ToInt64(TotlMenTellPhon_Lb.Text) - Convert.ToInt64(TotlActvMenTellPhon_Lb.Text)).ToString();

         TellPhonServ_Cc.DataSource = new BindingSource(
               new BindingList<Data.Fighter>(
                  FighBs.List.OfType<Data.Fighter>()
                  .Where(f => f.FGPB_TYPE_DNRM == "001"
                           && f.TELL_PHON_DNRM != null
                           && f.TELL_PHON_DNRM.Length >= 4)
                  .ToList()
            ), null
         );
         #endregion

         #region آمار سرپرستان مشترکین
         TotlManCoch_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "003" && f.SEX_TYPE_DNRM == "001").Count().ToString();
         TotlMenCoch_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "003" && f.SEX_TYPE_DNRM == "002").Count().ToString();
         TotlCoch_Lb.Text = (Convert.ToInt64(TotlManCoch_Lb.Text) + Convert.ToInt64(TotlMenCoch_Lb.Text)).ToString();

         TotlActvManCoch_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "003" && f.SEX_TYPE_DNRM == "001" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0).Count().ToString();
         TotlActvMenCoch_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "003" && f.SEX_TYPE_DNRM == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0).Count().ToString();
         TotlDactManCoch_Lb.Text = (Convert.ToInt64(TotlManCoch_Lb.Text) - Convert.ToInt64(TotlActvManCoch_Lb.Text)).ToString();
         TotlDactMenCoch_Lb.Text = (Convert.ToInt64(TotlMenCoch_Lb.Text) - Convert.ToInt64(TotlActvMenCoch_Lb.Text)).ToString();

         CochServ_Cc.DataSource = new BindingSource(
               new BindingList<Data.Fighter>(
                  FighBs.List.OfType<Data.Fighter>()
                  .Where(f => f.FGPB_TYPE_DNRM == "003"
                           && f.ACTV_TAG_DNRM.CompareTo("101") >= 0)
                  .ToList()
            ), null
         );
         #endregion

         #region آمار بیمه مشترکین
         TotlManInsr_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.INSR_DATE_DNRM != null && f.INSR_DATE_DNRM.Value.Date >= DateTime.Now.Date).Count().ToString();
         TotlMenInsr_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.INSR_DATE_DNRM != null && f.INSR_DATE_DNRM.Value.Date >= DateTime.Now.Date).Count().ToString();
         TotlInsr_Lb.Text = (Convert.ToInt64(TotlManInsr_Lb.Text) + Convert.ToInt64(TotlMenInsr_Lb.Text)).ToString();

         TotlActvManInsr_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "001" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date && f.INSR_DATE_DNRM != null && f.INSR_DATE_DNRM.Value.Date >= DateTime.Now).Count().ToString();
         TotlActvMenInsr_Lb.Text = FighBs.List.OfType<Data.Fighter>().Where(f => f.FGPB_TYPE_DNRM == "001" && f.SEX_TYPE_DNRM == "002" && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date && f.INSR_DATE_DNRM != null && f.INSR_DATE_DNRM.Value.Date >= DateTime.Now).Count().ToString();
         TotlDactManInsr_Lb.Text = (Convert.ToInt64(TotlManInsr_Lb.Text) - Convert.ToInt64(TotlActvManInsr_Lb.Text)).ToString();
         TotlDactMenInsr_Lb.Text = (Convert.ToInt64(TotlMenInsr_Lb.Text) - Convert.ToInt64(TotlActvMenInsr_Lb.Text)).ToString();

         InsrServ_Cc.DataSource = new BindingSource(
               new BindingList<Data.Fighter>(
                  FighBs.List.OfType<Data.Fighter>()
                  .Where(f => f.FGPB_TYPE_DNRM == "001"
                           && f.INSR_DATE_DNRM != null 
                           && f.INSR_DATE_DNRM.Value.Date >= DateTime.Now.Date)
                  .ToList()
            ), null
         );
         #endregion
      }
      #endregion

      #region Tab Page 002
      private void TabPage002_Starting()
      {
         try
         {
            //AttnBs.DataSource = iScsc.Attendances.Where(a => a.ATTN_DATE.Date.Year == DateTime.Now.Date.Year || a.ATTN_DATE.Date.Year == (DateTime.Now.Date.Year - 1));

            if (InvokeRequired)
            {
               Invoke(new Action(() => TabPage002_Filling()));
            }
            else
            {
               TabPage002_Filling();
            }
         }
         catch { }
      }

      private void TabPage002_Filling()
      {
         #region آمار حضور و غیاب
         TotlAttn_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Count().ToString();

         PersianCalendar pc = new PersianCalendar();

         TotlCrntYear_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Where(a => pc.GetYear(a.ATTN_DATE) == pc.GetYear(DateTime.Now)).Count().ToString();
         TotlPrvsYear_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Where(a => pc.GetYear(a.ATTN_DATE) == (pc.GetYear(DateTime.Now) - 1)).Count().ToString();
         TotlManCrntYear_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Where(a => a.Fighter1.SEX_TYPE_DNRM == "001" && pc.GetYear(a.ATTN_DATE) == pc.GetYear(DateTime.Now)).Count().ToString();
         TotlMenCrntYear_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Where(a => a.Fighter1.SEX_TYPE_DNRM == "002" && pc.GetYear(a.ATTN_DATE) == pc.GetYear(DateTime.Now)).Count().ToString();
         TotlManCrntDay_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Where(a => a.Fighter1.SEX_TYPE_DNRM == "001" && a.ATTN_DATE.Date == DateTime.Now.Date).Count().ToString();
         TotlMenCrntDay_Lb.Text = AttnBs.List.OfType<Data.Attendance>().Where(a => a.Fighter1.SEX_TYPE_DNRM == "002" && a.ATTN_DATE.Date == DateTime.Now.Date).Count().ToString();


         //TotlAttnLine_Cc.DataSource = new BindingSource(
         //      new BindingList<Data.Attendance>(
         //         AttnBs.List.OfType<Data.Attendance>()
         //         .ToList()
         //   ), null
         //);
         
         #endregion         
      }
      #endregion

      #region Tab Page 003
      string yearFltr = "*", monthFltr = "*";

      private void TabPage003_Starting()
      {
         try
         {
            vSaleBs.DataSource = iScsc.V_StatisticSales;
            vRcptBs.DataSource = iScsc.V_StatisticReceipts;
            vDscnBs.DataSource = iScsc.V_StatisticDiscounts;
            VGnLsBs.DataSource = iScsc.V_GainLosses;
            VExpnBs.DataSource = iScsc.Misc_Expenses.Where(me => me.VALD_TYPE == "002");

            if (InvokeRequired)
            {
               Invoke(new Action(() => TabPage003_Filling()));
            }
            else
            {
               TabPage003_Filling();
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void TabPage003_Filling()
      {
         #region Load All Sale
         var minYear = vSaleBs.List.OfType<Data.V_StatisticSale>().Min(s => s.YEAR);
         var maxYear = vSaleBs.List.OfType<Data.V_StatisticSale>().Max(s => s.YEAR);

         YearCyclFltr1_Lb.Text = YearCyclFltr2_Lb.Text = YearCyclFltr3_Lb.Text = "*/*";

         SaleYear_Flp.Controls.OfType<SimpleButton>().Where(y => y.Tag.ToString() != "*").ToList().ForEach(y => SaleYear_Flp.Controls.Remove(y));

         for (int i = (int)minYear; i <= maxYear; i++)
         {
            SimpleButton year = new SimpleButton();
            year.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            year.Appearance.Font = new System.Drawing.Font("Iranian Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            year.Appearance.Options.UseBackColor = true;
            year.Appearance.Options.UseFont = true;
            year.Appearance.Options.UseTextOptions = true;
            year.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            year.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            year.Location = new System.Drawing.Point(2, 2);
            year.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            year.LookAndFeel.UseDefaultLookAndFeel = false;
            year.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            year.Name = "Year_Butn";
            year.Size = new System.Drawing.Size(61, 35);
            year.TabIndex = 8;
            year.Text = i.ToString();
            year.Tag = i;
            year.Click += YearData_Butn_Click;

            SaleYear_Flp.Controls.Add(year);
         }

         YearData_Butn_Click(AllYear_Butn, null);
         #endregion
      }
      #endregion

      private void YearData_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var yb = (sender as SimpleButton).Tag;
            
            if(yb == null)
            {
               yearFltr = "*";
               // All Year Data
               CalculateData003(yearFltr, monthFltr);
            }
            else
            {
               yearFltr = yb.ToString();
               CalculateData003(yearFltr, monthFltr);
            }
            YearCyclFltr1_Lb.Text = YearCyclFltr2_Lb.Text = YearCyclFltr3_Lb.Text = string.Format("{0}/{1}", yearFltr, monthFltr);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MontData_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mb = (sender as SimpleButton).Tag;

            if (mb == null)
            {
               monthFltr = "*";
               // All Year Data
               CalculateData003(yearFltr, monthFltr);
            }
            else
            {
               monthFltr = mb.ToString();
               CalculateData003(yearFltr, monthFltr);
            }
            YearCyclFltr1_Lb.Text = YearCyclFltr2_Lb.Text = YearCyclFltr3_Lb.Text = string.Format("{0}/{1}", yearFltr, monthFltr);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CalculateData003(object yb, object mb)
      {
         try
         {
            DateTime firsttime = DateTime.Now;
            #region Group SRD
            // Record 1 # Count
            Rqtp001Cont_Lb.Text = vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString()) && s.RQTP_CODE == "001").Sum(s => s.CONT).ToString();
            Rqtp009Cont_Lb.Text = vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString()) && s.RQTP_CODE == "009").Sum(s => s.CONT).ToString();
            Rqtp016Cont_Lb.Text = vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString()) && s.RQTP_CODE == "016").Sum(s => s.CONT).ToString();
            SumHCont_Lb.Text = vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString())).Sum(s => s.CONT).ToString();

            // Record 2 # Sale
            Rqtp001SaleAmnt_Lb.Text = ((long)(vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString()) && s.RQTP_CODE == "001").Sum(s => s.AMNT))).ToString("#,0");
            Rqtp009SaleAmnt_Lb.Text = ((long)(vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString()) && s.RQTP_CODE == "009").Sum(s => s.AMNT))).ToString("#,0");
            Rqtp016SaleAmnt_Lb.Text = ((long)(vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString()) && s.RQTP_CODE == "016").Sum(s => s.AMNT))).ToString("#,0");
            SumHSaleAmnt_Lb.Text = ((long)(vSaleBs.List.OfType<Data.V_StatisticSale>().Where(s => s.YEAR == Convert.ToInt16(yb.ToString() == "*" ? s.YEAR : yb) && s.CYCL == (mb.ToString() == "*" ? s.CYCL : mb.ToString())).Sum(s => s.AMNT))).ToString("#,0");

            // Record 3 # Rcpt
            Rqtp001RcptAmnt_Lb.Text = ((long)(vRcptBs.List.OfType<Data.V_StatisticReceipt>().Where(r => r.YEAR == (yb.ToString() == "*" ? r.YEAR : yb.ToString()) && r.CYCL == (mb.ToString() == "*" ? r.CYCL : mb.ToString()) && r.RQTP_CODE == "001").Sum(r => r.AMNT))).ToString("#,0");
            Rqtp009RcptAmnt_Lb.Text = ((long)(vRcptBs.List.OfType<Data.V_StatisticReceipt>().Where(r => r.YEAR == (yb.ToString() == "*" ? r.YEAR : yb.ToString()) && r.CYCL == (mb.ToString() == "*" ? r.CYCL : mb.ToString()) && r.RQTP_CODE == "009").Sum(r => r.AMNT))).ToString("#,0");
            Rqtp016RcptAmnt_Lb.Text = ((long)(vRcptBs.List.OfType<Data.V_StatisticReceipt>().Where(r => r.YEAR == (yb.ToString() == "*" ? r.YEAR : yb.ToString()) && r.CYCL == (mb.ToString() == "*" ? r.CYCL : mb.ToString()) && r.RQTP_CODE == "016").Sum(r => r.AMNT))).ToString("#,0");
            SumHRcptAmnt_Lb.Text = ((long)(vRcptBs.List.OfType<Data.V_StatisticReceipt>().Where(r => r.YEAR == (yb.ToString() == "*" ? r.YEAR : yb.ToString()) && r.CYCL == (mb.ToString() == "*" ? r.CYCL : mb.ToString())).Sum(r => r.AMNT))).ToString("#,0");

            // Record 4 # %
            if (Convert.ToDouble(Rqtp001RcptAmnt_Lb.Text) != 0)
               Rqtp001PrctAmnt_Lb.Text = ((Convert.ToDouble(Rqtp001RcptAmnt_Lb.Text) / Convert.ToDouble(Rqtp001SaleAmnt_Lb.Text)) * 100).ToString("0.00");
            else
               Rqtp001PrctAmnt_Lb.Text = "0";
            
            if(Convert.ToDouble(Rqtp009RcptAmnt_Lb.Text) != 0)            
               Rqtp009PrctAmnt_Lb.Text = ((Convert.ToDouble(Rqtp009RcptAmnt_Lb.Text) / Convert.ToDouble(Rqtp009SaleAmnt_Lb.Text)) * 100).ToString("0.00");
            else
               Rqtp009PrctAmnt_Lb.Text = "0";

            if(Convert.ToDouble(Rqtp016RcptAmnt_Lb.Text) != 0)
               Rqtp016PrctAmnt_Lb.Text = ((Convert.ToDouble(Rqtp016RcptAmnt_Lb.Text) / Convert.ToDouble(Rqtp016SaleAmnt_Lb.Text)) * 100).ToString("0.00");
            else
               Rqtp016PrctAmnt_Lb.Text = "0";

            // Record 5 # Dscn
            Rqtp001DscnAmnt_Lb.Text = ((long)(vDscnBs.List.OfType<Data.V_StatisticDiscount>().Where(d => d.YEAR == (yb.ToString() == "*" ? d.YEAR : yb.ToString()) && d.CYCL == (mb.ToString() == "*" ? d.CYCL : mb.ToString()) && d.RQTP_CODE == "001").Sum(d => d.AMNT))).ToString("#,0");
            Rqtp009DscnAmnt_Lb.Text = ((long)(vDscnBs.List.OfType<Data.V_StatisticDiscount>().Where(d => d.YEAR == (yb.ToString() == "*" ? d.YEAR : yb.ToString()) && d.CYCL == (mb.ToString() == "*" ? d.CYCL : mb.ToString()) && d.RQTP_CODE == "009").Sum(d => d.AMNT))).ToString("#,0");
            Rqtp016DscnAmnt_Lb.Text = ((long)(vDscnBs.List.OfType<Data.V_StatisticDiscount>().Where(d => d.YEAR == (yb.ToString() == "*" ? d.YEAR : yb.ToString()) && d.CYCL == (mb.ToString() == "*" ? d.CYCL : mb.ToString()) && d.RQTP_CODE == "016").Sum(d => d.AMNT))).ToString("#,0");
            SumHDscnAmnt_Lb.Text = ((long)(vDscnBs.List.OfType<Data.V_StatisticDiscount>().Where(d => d.YEAR == (yb.ToString() == "*" ? d.YEAR : yb.ToString()) && d.CYCL == (mb.ToString() == "*" ? d.CYCL : mb.ToString())).Sum(d => d.AMNT))).ToString("#,0");
            #endregion

            #region Group Sepordeh
            Rqtp020ManCont_Lb.Text = VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.SEX_TYPE_DNRM == "001" && g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Count().ToString();
            Rqtp020MenCont_Lb.Text = VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.SEX_TYPE_DNRM == "002" && g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Count().ToString();
            Rqtp020ManAmnt_Lb.Text = ((long)(VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.SEX_TYPE_DNRM == "001" && g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Sum(g => g.CASH_AMNT + g.POS_AMNT))).ToString("#,0");
            Rqtp020MenAmnt_Lb.Text = ((long)(VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.SEX_TYPE_DNRM == "002" && g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Sum(g => g.CASH_AMNT + g.POS_AMNT))).ToString("#,0");
            Rqtp020CashAmnt_Lb.Text = ((long)(VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Sum(g => g.CASH_AMNT ))).ToString("#,0");
            Rqtp020PosAmnt_Lb.Text = ((long)(VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Sum(g => g.POS_AMNT))).ToString("#,0");
            SumHCont2_Lb.Text = VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Count().ToString();
            SumHRcptAmnt2_Lb.Text = ((long)(VGnLsBs.List.OfType<Data.V_GainLoss>().Where(g => g.YEAR == (yb.ToString() == "*" ? g.YEAR : yb.ToString()) && g.CYCL == (mb.ToString() == "*" ? g.CYCL : mb.ToString())).Sum(g => g.POS_AMNT + g.CASH_AMNT))).ToString("#,0");
            #endregion

            #region Group Expense
            PersianCalendar pc = new PersianCalendar();
            Expn002Cont_Lb.Text = VExpnBs.List.OfType<Data.Misc_Expense>().Where(me => pc.GetYear(me.DELV_DATE.Value.Date).ToString() == (yb.ToString() == "*" ? pc.GetYear(me.DELV_DATE.Value.Date).ToString() : yb.ToString()) && pc.GetMonth(me.DELV_DATE.Value.Date).ToString() == (mb.ToString() == "*" ? pc.GetMonth(me.DELV_DATE.Value.Date).ToString() : mb.ToString()) && me.CALC_EXPN_TYPE == "002" && me.VALD_TYPE == "002").Count().ToString();
            Expn001Cont_Lb.Text = VExpnBs.List.OfType<Data.Misc_Expense>().Where(me => pc.GetYear(me.DELV_DATE.Value.Date).ToString() == (yb.ToString() == "*" ? pc.GetYear(me.DELV_DATE.Value.Date).ToString() : yb.ToString()) && pc.GetMonth(me.DELV_DATE.Value.Date).ToString() == (mb.ToString() == "*" ? pc.GetMonth(me.DELV_DATE.Value.Date).ToString() : mb.ToString()) && me.CALC_EXPN_TYPE == "001" && me.VALD_TYPE == "002").Count().ToString();
            Expn002Amnt_Lb.Text = ((long)(VExpnBs.List.OfType<Data.Misc_Expense>().Where(me => pc.GetYear(me.DELV_DATE.Value.Date).ToString() == (yb.ToString() == "*" ? pc.GetYear(me.DELV_DATE.Value.Date).ToString() : yb.ToString()) && pc.GetMonth(me.DELV_DATE.Value.Date).ToString() == (mb.ToString() == "*" ? pc.GetMonth(me.DELV_DATE.Value.Date).ToString() : mb.ToString()) && me.CALC_EXPN_TYPE == "002" && me.VALD_TYPE == "002").Sum(me => me.EXPN_AMNT))).ToString("#,0");
            Expn001Amnt_Lb.Text = ((long)(VExpnBs.List.OfType<Data.Misc_Expense>().Where(me => pc.GetYear(me.DELV_DATE.Value.Date).ToString() == (yb.ToString() == "*" ? pc.GetYear(me.DELV_DATE.Value.Date).ToString() : yb.ToString()) && pc.GetMonth(me.DELV_DATE.Value.Date).ToString() == (mb.ToString() == "*" ? pc.GetMonth(me.DELV_DATE.Value.Date).ToString() : mb.ToString()) && me.CALC_EXPN_TYPE == "001" && me.VALD_TYPE == "002").Sum(me => me.EXPN_AMNT))).ToString("#,0");
            SumHCont3_Lb.Text = VExpnBs.List.OfType<Data.Misc_Expense>().Where(me => pc.GetYear(me.DELV_DATE.Value.Date).ToString() == (yb.ToString() == "*" ? pc.GetYear(me.DELV_DATE.Value.Date).ToString() : yb.ToString()) && pc.GetMonth(me.DELV_DATE.Value.Date).ToString() == (mb.ToString() == "*" ? pc.GetMonth(me.DELV_DATE.Value.Date).ToString() : mb.ToString()) && me.VALD_TYPE == "002").Count().ToString();
            SumHExpnAmnt3_Lb.Text = ((long)(VExpnBs.List.OfType<Data.Misc_Expense>().Where(me => pc.GetYear(me.DELV_DATE.Value.Date).ToString() == (yb.ToString() == "*" ? pc.GetYear(me.DELV_DATE.Value.Date).ToString() : yb.ToString()) && pc.GetMonth(me.DELV_DATE.Value.Date).ToString() == (mb.ToString() == "*" ? pc.GetMonth(me.DELV_DATE.Value.Date).ToString() : mb.ToString()) && me.VALD_TYPE == "002").Sum(me => me.EXPN_AMNT))).ToString("#,0");
            #endregion

            #region Group Summery
            SumSaleAmnt_Lb.Text = (Convert.ToInt64(SumHSaleAmnt_Lb.Text.Replace(",", ""))).ToString("#,0");
            SumRcptAmnt_Lb.Text = (Convert.ToInt64(SumHRcptAmnt_Lb.Text.Replace(",", "")) + Convert.ToInt64(SumHRcptAmnt2_Lb.Text.Replace(",", ""))).ToString("#,0");
            SumExpnAmnt_Lb.Text = Convert.ToInt64(SumHExpnAmnt3_Lb.Text.Replace(",", "")).ToString("#,0");
            SumNetAmnt_Lb.Text = ((Convert.ToInt64(SumHRcptAmnt_Lb.Text.Replace(",", "")) + Convert.ToInt64(SumHRcptAmnt2_Lb.Text.Replace(",", ""))) - Convert.ToInt64(SumHExpnAmnt3_Lb.Text.Replace(",", ""))).ToString("#,0");

            RcptPrct_Prg.EditValue = (Convert.ToDouble(SumRcptAmnt_Lb.Text.Replace(",", "")) / Convert.ToDouble(SumSaleAmnt_Lb.Text.Replace(",", "")) * 100);
            ExpnPrct_Prg.EditValue = (Convert.ToDouble(SumExpnAmnt_Lb.Text.Replace(",", "")) / Convert.ToDouble(SumRcptAmnt_Lb.Text.Replace(",", "")) * 100);
            if (Convert.ToDouble(ExpnPrct_Prg.EditValue) == 0)
               if (Convert.ToDouble(RcptPrct_Prg.EditValue) != 0)
                  NetPrct_Prg.EditValue = 100;
               else
                  NetPrct_Prg.EditValue = 0;
            else
               NetPrct_Prg.EditValue = Convert.ToDouble(RcptPrct_Prg.EditValue) - Convert.ToDouble(ExpnPrct_Prg.EditValue);            
            #endregion

            Stopwatch3_Lb.Text = (DateTime.Now - firsttime).ToString();
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Tc_Master_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
      {
         Execute_Query();
      }
   }
}
