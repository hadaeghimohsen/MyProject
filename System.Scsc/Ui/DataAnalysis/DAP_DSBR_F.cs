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
            FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "003") );
            
            if(InvokeRequired)
            {
               Invoke(new Action(() =>  TabPage001_Filling()));
            }
            else
            {
               TabPage001_Filling();
            }
         }
         catch {}
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
            AttnBs.DataSource = iScsc.Attendances.Where(a => a.ATTN_DATE.Date.Year == DateTime.Now.Date.Year || a.ATTN_DATE.Date.Year == (DateTime.Now.Date.Year - 1));

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
   }
}
