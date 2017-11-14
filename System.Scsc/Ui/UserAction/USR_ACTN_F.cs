using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.JobRouting.Jobs;
using System.Xml.Linq;

namespace System.Scsc.Ui.UserAction
{
   public partial class USR_ACTN_F : UserControl
   {
      public USR_ACTN_F()
      {
         InitializeComponent();
      }

      private List<string> rqtps = new List<string> { "001", "009", "016" };

      private void Execute_Query()
      {
         try
         {
            if (!AttnDate_Date.Value.HasValue)
               AttnDate_Date.Value = DateTime.Now;

            #region Set DateTime
            Lbl_CrntDate.Text = GetPersianDate(AttnDate_Date.Value.Value.Date);
            Lbl_NextDay.Text = GetPersianDate(AttnDate_Date.Value.Value.AddDays(1));
            Lbl_SecondDay.Text = GetPersianDate(AttnDate_Date.Value.Value.AddDays(2));
            Lbl_ThirdDay.Text = GetPersianDate(AttnDate_Date.Value.Value.AddDays(3));
            #endregion

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQST_STAT == "002" &&
                        rqst.RQTT_CODE == "001" &&
                        rqtps.Contains(rqst.RQTP_CODE) &&
                        rqst.SAVE_DATE.Value.Date == AttnDate_Date.Value.Value.Date &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var v_RqstsToday =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               );

            //var v_RqstsToday = iScsc.Requests.Where(r => rqtps.Contains(r.RQTP_CODE) && r.RQTT_CODE == "001" && r.RQST_STAT == "002" && r.SAVE_DATE.Value.Date == AttnDate_Date.Value.Value.Date);
            Lbl_NewMember.Text = v_RqstsToday.Where(r => r.RQTP_CODE == "001" && Fga_Uclb_U.Contains(r.Request_Rows.FirstOrDefault().Fighter.CLUB_CODE_DNRM) ).Count().ToString();
            Lbl_RetryMember.Text = v_RqstsToday.Where(r => r.RQTP_CODE == "009" && Fga_Uclb_U.Contains(r.Request_Rows.FirstOrDefault().Fighter.CLUB_CODE_DNRM)).Count().ToString();
            Lbl_Shopping.Text = v_RqstsToday.Where(r => r.RQTP_CODE == "016" && Fga_Uclb_U.Contains(r.Request_Rows.FirstOrDefault().Fighter.CLUB_CODE_DNRM)).Count().ToString();
            AttnBs1.DataSource = iScsc.Attendances.Where(a => a.ATTN_DATE.Date == AttnDate_Date.Value.Value.Date && Fga_Uclb_U.Contains(a.CLUB_CODE));
            Lbl_AllGetKeys.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.DERS_NUMB != null).Count().ToString();
            Lbl_AllLockKeys.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.DERS_NUMB != null && a.EXIT_TIME == null).Count().ToString();
            var v_PayAmnt = iScsc.Payment_Methods.Where(p => p.ACTN_DATE.Value.Date == AttnDate_Date.Value.Value.Date && Fga_Uclb_U.Contains(p.Request_Row.Fighter.CLUB_CODE_DNRM));
            if (v_PayAmnt.Count() > 0)
            {
               if (v_PayAmnt.Where(p => p.RCPT_MTOD == "001").Count() > 0)
               {
                  Lbl_CashAmnt.Text = string.Format("{0:n0}", v_PayAmnt.Where(p => p.RCPT_MTOD == "001").Sum(p => p.AMNT ?? 0));
                  if (v_PayAmnt.Where(p => p.RCPT_MTOD == "001" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "001").Count() > 0)
                     Lbl_TotalMenCashAmnt.Text = string.Format("{0:n0}", v_PayAmnt.Where(p => p.RCPT_MTOD == "001" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "001").Sum(p => p.AMNT ?? 0));
                  else
                     Lbl_TotalMenCashAmnt.Text = "0";
                  if (v_PayAmnt.Where(p => p.RCPT_MTOD == "001" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "002").Count() > 0)
                     Lbl_TotalWomenCashAmnt.Text = string.Format("{0:n0}", v_PayAmnt.Where(p => p.RCPT_MTOD == "001" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "002").Sum(p => p.AMNT ?? 0));
                  else
                     Lbl_TotalWomenCashAmnt.Text = "0";
               }
               else
               {
                  Lbl_CashAmnt.Text = "0";
                  Lbl_TotalMenCashAmnt.Text = "0";
                  Lbl_TotalWomenCashAmnt.Text = "0";
               }
               if (v_PayAmnt.Where(p => p.RCPT_MTOD == "003").Count() > 0)
               {
                  Lbl_PosAmnt.Text = string.Format("{0:n0}", v_PayAmnt.Where(p => p.RCPT_MTOD == "003").Sum(p => p.AMNT ?? 0));
                  if (v_PayAmnt.Where(p => p.RCPT_MTOD == "003" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "001").Count() > 0)
                     Lbl_TotalMenPosAmnt.Text = string.Format("{0:n0}", v_PayAmnt.Where(p => p.RCPT_MTOD == "003" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "001").Sum(p => p.AMNT ?? 0));
                  else
                     Lbl_TotalMenPosAmnt.Text = "0";
                  if (v_PayAmnt.Where(p => p.RCPT_MTOD == "003" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "002").Count() > 0)
                     Lbl_TotalWomenPosAmnt.Text = string.Format("{0:n0}", v_PayAmnt.Where(p => p.RCPT_MTOD == "003" && p.Request_Row.Fighter.SEX_TYPE_DNRM == "002").Sum(p => p.AMNT ?? 0));
                  else
                     Lbl_TotalWomenPosAmnt.Text = "0";
               }
               else
               {
                  Lbl_PosAmnt.Text = "0";
                  Lbl_TotalMenPosAmnt.Text = "0";
                  Lbl_TotalWomenPosAmnt.Text = "0";
               }
            }
            else
            {
               Lbl_CashAmnt.Text = "0";
               Lbl_TotalMenCashAmnt.Text = "0";
               Lbl_TotalWomenCashAmnt.Text = "0";
               Lbl_PosAmnt.Text = "0";
               Lbl_TotalMenPosAmnt.Text = "0";
               Lbl_TotalWomenPosAmnt.Text = "0";
            }
            var v_DiscountAmnt = iScsc.Payment_Discounts.Where(p => p.CRET_DATE.Value.Date == AttnDate_Date.Value.Value.Date && p.Request_Row.Request.RQST_STAT != "003" && Fga_Uclb_U.Contains(p.Request_Row.Fighter.CLUB_CODE_DNRM));
            if (v_DiscountAmnt.Count() > 0)
            {
               Lbl_DiscountAmnt.Text = string.Format("{0:n0}", v_DiscountAmnt.Sum(p => p.AMNT ?? 0));
               if (v_DiscountAmnt.Where(d => d.Request_Row.Request.RQST_STAT != "003" && d.Request_Row.Fighter.SEX_TYPE_DNRM == "001").Count() > 0)
                  Lbl_TotalMenDiscountAmnt.Text = string.Format("{0:n0}", v_DiscountAmnt.Where(d => d.Request_Row.Request.RQST_STAT != "003" && d.Request_Row.Fighter.SEX_TYPE_DNRM == "001").Sum(p => p.AMNT ?? 0));
               else
                  Lbl_TotalMenDiscountAmnt.Text = "0";
               if (v_DiscountAmnt.Where(d => d.Request_Row.Request.RQST_STAT != "003" && d.Request_Row.Fighter.SEX_TYPE_DNRM == "002").Count() > 0)
                  Lbl_TotalWomenDiscountAmnt.Text = string.Format("{0:n0}", v_DiscountAmnt.Where(d => d.Request_Row.Request.RQST_STAT != "003" && d.Request_Row.Fighter.SEX_TYPE_DNRM == "002").Sum(p => p.AMNT ?? 0));
               else
                  Lbl_TotalWomenDiscountAmnt.Text = "0";
            }
            else
            {
               Lbl_DiscountAmnt.Text = "0";
               Lbl_TotalMenDiscountAmnt.Text = "0";
               Lbl_TotalWomenDiscountAmnt.Text = "0";               
            }
            
            Lbl_TotalMenAttn.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.Fighter1.SEX_TYPE_DNRM == "001").Count().ToString();
            Lbl_TotalWomenAttn.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.Fighter1.SEX_TYPE_DNRM == "002").Count().ToString();
            Lbl_TotalMenGetKeys.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.DERS_NUMB != null && a.Fighter1.SEX_TYPE_DNRM == "001").Count().ToString();
            Lbl_TotalMenLockKeys.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.DERS_NUMB != null && a.EXIT_TIME == null && a.Fighter1.SEX_TYPE_DNRM == "001").Count().ToString();
            Lbl_TotalWomenGetKeys.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.DERS_NUMB != null && a.Fighter1.SEX_TYPE_DNRM == "002").Count().ToString();
            Lbl_TotalWomenLockKeys.Text = AttnBs1.List.OfType<Data.Attendance>().Where(a => a.DERS_NUMB != null && a.EXIT_TIME == null && a.Fighter1.SEX_TYPE_DNRM == "002").Count().ToString();
            
            var v_Fighs = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM));
            if (v_Fighs.Count() > 0)
            {
               Lbl_AllPhoneSave.Text = v_Fighs.Where(f => f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count().ToString();
               Lbl_AllPhoneSavePercent.Text = (v_Fighs.Where(f => f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length >= 10).Count() * 100 / v_Fighs.Count()).ToString() + "%";
            }
            else
            {
               Lbl_AllPhoneSave.Text = "0";
               Lbl_AllPhoneSavePercent.Text = "0%";
            }
            Lbl_RqstNotEnd.Text = iScsc.Requests.Where(r => r.RQST_STAT == "001").Count().ToString();
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void sb_search_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void NextDay_Butn_Click(object sender, EventArgs e)
      {
         if (!AttnDate_Date.Value.HasValue)
               AttnDate_Date.Value = DateTime.Now;
         AttnDate_Date.Value = AttnDate_Date.Value.Value.AddDays(1);
         Execute_Query();
      }

      private void BackDay_Butn_Click(object sender, EventArgs e)
      {
         if (!AttnDate_Date.Value.HasValue)
            AttnDate_Date.Value = DateTime.Now;
         AttnDate_Date.Value = AttnDate_Date.Value.Value.AddDays(-1);
         Execute_Query();
      }

      private string GetPersianDate(DateTime dateTime)
      {
         PersianCalendar pc = new PersianCalendar();
         string year = pc.GetYear(dateTime).ToString();
         string weekdaydesc = "";
         string monthdesc = "";
         switch (pc.GetDayOfWeek(dateTime))
         {
            case DayOfWeek.Friday:
               weekdaydesc = "جمعه";
               break;
            case DayOfWeek.Saturday:
               weekdaydesc = "شنبه";
               break;
            case DayOfWeek.Sunday:
               weekdaydesc = "یکشنبه";
               break;
            case DayOfWeek.Monday:
               weekdaydesc = "دوشنبه";
               break;
            case DayOfWeek.Tuesday:
               weekdaydesc = "سه شنبه";
               break;
            case DayOfWeek.Wednesday:
               weekdaydesc = "چهارشنبه";
               break;
            case DayOfWeek.Thursday:
               weekdaydesc = "پنجشنبه";
               break;
         }
         string dayofmonthdesc = pc.GetDayOfMonth(dateTime).ToString();
         switch (pc.GetMonth(dateTime))
         {
            case 1:
               monthdesc = "فروردین";
               break;
            case 2:
               monthdesc = "اردیبهشت";
               break;
            case 3:
               monthdesc = "خرداد";
               break;
            case 4:
               monthdesc = "تیر";
               break;
            case 5:
               monthdesc = "مرداد";
               break;
            case 6:
               monthdesc = "شهریور";
               break;
            case 7:
               monthdesc = "مهر";
               break;
            case 8:
               monthdesc = "آبان";
               break;
            case 9:
               monthdesc = "آذر";
               break;
            case 10:
               monthdesc = "دی";
               break;
            case 11:
               monthdesc = "بهمن";
               break;
            case 12:
               monthdesc = "اسفند";
               break;
         }
         return string.Format("{0}, {1} {2} {3}", weekdaydesc, dayofmonthdesc, monthdesc, year);
      }

      private void DetailSRD_Butn_Click(object sender, EventArgs e)
      {
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
                              "<Privilege>218</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 218 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 135 /* Execute Rpt_Pmmt_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("formname", "RPT_PYM2_F"), new XAttribute("fromdate", AttnDate_Date.Value.Value.Date), new XAttribute("todate", AttnDate_Date.Value.Value.Date), new XAttribute("useraccount", "user"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void AttnDay_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 141 /* Execute WHO_ARYU_F */),
                  new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ ){Input = new XElement("Attendance", new XAttribute("attndate", AttnDate_Date.Value.Value))}
               })
         );
      }
   }
}
