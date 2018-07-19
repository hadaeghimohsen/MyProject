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

namespace System.Scsc.Ui.AggregateOperation
{
   public partial class AOP_BUFE_F : UserControl
   {
      public AOP_BUFE_F()
      {
         InitializeComponent();
      }

      private bool requery = false, setondebt = false;
      private int agopindx = 0, aodtindx = 0;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         agopindx = AgopBs1.Position;
         aodtindx = AodtBs1.Position;
         AgopBs1.DataSource = iScsc.Aggregation_Operations.Where(a => a.OPRT_TYPE == "005" && (a.OPRT_STAT == "001" || a.OPRT_STAT == "002")).OrderByDescending(ag => ag.FROM_DATE) ;
         AgopBs1.Position = agopindx;
         AodtBs1.Position = aodtindx;

         FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);

         ExpnBufeBs1.DataSource =
            iScsc.Expenses.Where(ex =>
               ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
               ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
               ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
               ex.EXPN_STAT == "002" /* هزینه های فعال */
            );

         ExpnDeskBs1.DataSource =
            iScsc.Expenses.Where(ex =>
               ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
               ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
               ex.Expense_Type.Request_Requester.RQTT_CODE == "007" &&
               ex.EXPN_STAT == "002" /* هزینه های فعال */
            ).OrderBy(ed => ed.EXPN_DESC);

         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void ClerFromDate_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         FromDate_Dt.Value = null;
         crnt.FROM_DATE = crnt.TO_DATE = null;
      }

      private void ClearAll_Butn_Click(object sender, EventArgs e)
      {
         ClerFromDate_Butn_Click(sender, e);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
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
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "005"),
                     new XAttribute("oprtstat", crnt.OPRT_STAT ?? "002"),
                     new XAttribute("agopdesc", crnt.AGOP_DESC ?? "")
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
               requery = false;
            }
         }
      }

      private void Cncl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt != null && MessageBox.Show(this, "آیا با انصراف دفتر فعلی موافق هستید؟", "انصراف دفتر", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "005"),
                     new XAttribute("oprtstat", "003")
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
               requery = false;
            }
         }
      }

      private void RecStat_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if(e.Button.Index == 4)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", crnt.FIGH_FILE_NO)) }
               );
               return;
            }
            if (crnt.STAT != "001") { MessageBox.Show("این رکورد قبلا در وضعیت نهایی قرار گرفته"); return; }
            switch (e.Button.Index)
            {
               case 0:
                  //crnt.REC_STAT = crnt.REC_STAT == "001" ? "002" : "001";
                  if (MessageBox.Show(this, "آیا با پاک کردن میز موافقید؟", "حذف میز", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iScsc.DEL_AODT_P(crnt.AGOP_CODE, crnt.RWNO);
                  break;
               case 2:
                  //crnt.END_TIME = DateTime.Now.TimeOfDay;
                  crnt.END_TIME = DateTime.Now;
                  break;
               case 3:
                  if (crnt.TOTL_AMNT_DNRM > /*crnt.Payment_Row_Types.Sum(a => a.AMNT)*/ ((crnt.CASH_AMNT ?? 0) + (crnt.POS_AMNT ?? 0)))
                     setondebt = true;
                  else
                     setondebt = false;

                  if(crnt.END_TIME == null)
                  {
                     MessageBox.Show(this, "میز بسته نشده لطفا دکمه بسته شدن میز را فشار دهید تا هزینه میز محاسبه شود");
                     return;
                  }
                  //if(setondebt && (crnt.CELL_PHON == null || crnt.CELL_PHON == ""))
                  //{
                  //   MessageBox.Show("برای هزینه هایی که در حالت بدهی ثبت می شوند باید شماره تلفن تماس ثبت گردد");
                  //   return;
                  //}

                  iScsc.ENDO_RSBU_P(
                     new XElement("Aggregation_Operation_Detail",
                        new XAttribute("fileno", crnt.FIGH_FILE_NO),
                        new XAttribute("agopcode", crnt.AGOP_CODE),
                        new XAttribute("rwno", crnt.RWNO),
                        new XAttribute("setondebt", setondebt)
                     )
                  );
                  break;               
               default:
                  break;
            }

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

      private void EndRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt != null && MessageBox.Show(this, "آیا با بستن دفتر فعلی موافق هستید؟", "بستن دفتر", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "005"),
                     new XAttribute("oprtstat", "004"),
                     new XAttribute("agopdesc", crnt.AGOP_DESC ?? "خدایا بابت روزی امروز ازت ممنونم")
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

      private void FIGH_FILE_NOLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 4) // بارگذاری لیست جدید
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
               return;
            }
            else if (e.Button.Index == 5) // تعریف کاربر جدید
            {
               Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
               _DefaultGateway.Gateway(_InteractWithScsc);
               return;
            }
            else if(e.Button.Index == 6) // مشخص کردن مشتری مهمان
            {
               if (FIGH_FILE_NOLookUpEdit.EditValue == null || FIGH_FILE_NOLookUpEdit.EditValue.ToString() == "") return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", FIGH_FILE_NOLookUpEdit.EditValue), new XAttribute("auto", "true"))}
                     })
               );
            }

            var fileno = Convert.ToInt64(FIGH_FILE_NOLookUpEdit.EditValue);

            switch (e.Button.Index)
            {
               case 1:
                  try
                  {
                     var agop = AgopBs1.Current as Data.Aggregation_Operation;
                     // اگر لیستی وجود نداشته باشد
                     if (agop == null)
                     {
                        MessageBox.Show(this, "اطلاعات یک لیست دفتری ایجاد کنید بعد میز را کرایه دهید", "لیست دفتری وجود ندارد");
                        if (MessageBox.Show(this, "آیا مایل به ایجاد کردن لیست دفتر امروز هستید؟", "ایجاد اتوماتیک لیست دفتر", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
                        // ایجاد کردن دفتر جدید
                        AgopBs1.AddNew();
                        agop = AgopBs1.Current as Data.Aggregation_Operation;
                        agop.FROM_DATE = agop.TO_DATE = DateTime.Now;
                        Edit_Butn_Click(null, null);
                     }
                     else if (agop.FROM_DATE.Value.Date != DateTime.Now.Date) // اگر لیست متعلق به روزهای قبل باشد
                     {
                        if (MessageBox.Show(this, "لیست دفتر جاری متعلق به تاریخ دیگری می باشد! آیا مایل به بستن لیست قبلی هستید؟", "بستن لیست دفتر قبل", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                        // پایانی کردن دفتر قبلی
                        EndRqst_Butn_Click(null, null);
                        // ایجاد کردن دفتر جدید
                        AgopBs1.AddNew();
                        agop = AgopBs1.Current as Data.Aggregation_Operation;
                        agop.FROM_DATE = agop.TO_DATE = DateTime.Now;
                        Edit_Butn_Click(null, null);
                     }
                     AodtBs1.AddNew();
                     var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                     aodt.Aggregation_Operation = agop;
                     aodt.FIGH_FILE_NO = fileno;
                     aodt.MIN_MINT_STEP = new TimeSpan(0, 15, 0);
                     //aodt.STRT_TIME = DateTime.Now.TimeOfDay;
                     aodt.STRT_TIME = DateTime.Now;
                     aodt.STAT = "001";
                     aodt.REC_STAT = "002";

                     iScsc.Aggregation_Operation_Details.InsertOnSubmit(aodt);

                     iScsc.SubmitChanges();

                     requery = true;
                  }
                  catch
                  { }
                  finally
                  {
                     if (requery)
                     {
                        Execute_Query();
                        requery = false;
                     }
                  }
                  break;
               case 2:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
                  );
                  break;
               case 3:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                          new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", fileno)))}
                        })
                  );
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void AddItem_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            // اگر لیستی وجود نداشته باشد
            if (aodt == null)
            {
               MessageBox.Show(this, "برای دفتر مربوطه شماره پرونده مشتری وجود ندارد", "ردیف لیست دفتری وجود ندارد");
               return;
            }

            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            requery = true;
            var expn = ExpnBufeBs1.Current as Data.Expense;

            // چک میکنیم که قبلا از این آیتم هزینه در جدول ریز هزینه وجود نداشته باشد
            if (!BufeBs1.List.OfType<Data.Buffet>().Any(b => b.EXPN_CODE == expn.CODE))
            {
               BufeBs1.AddNew();
               var bufe = BufeBs1.Current as Data.Buffet;
               bufe.EXPN_CODE = expn.CODE;
               bufe.QNTY = 1;
               iScsc.Buffets.InsertOnSubmit(bufe);
            }
            else
            {
               var bufe = BufeBs1.Current as Data.Buffet;
               bufe.QNTY += 1;
            }

            BufeBs1.EndEdit();
            iScsc.SubmitChanges();

            requery = true;
         }
         catch
         { }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void ActnBufeExpn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            var bufe = BufeBs1.Current as Data.Buffet;

            if (bufe == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "آیا با حذف آیتم بوفه موافق هستید؟", "حذف آیتم بوفه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iScsc.Buffets.DeleteOnSubmit(bufe);
                  break;
               default:
                  break;
            }

            BufeBs1.EndEdit();
            iScsc.SubmitChanges();

            requery = true;
         }
         catch
         { }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void ActnBufePyrt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }
            var pyrt = PyrtBs1.Current as Data.Payment_Row_Type;

            if (pyrt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "آیا با حذف آیتم پرداختی موافق هستید؟", "حذف آیتم پرداختی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  if (pyrt.CODE != 0)
                     iScsc.Payment_Row_Types.DeleteOnSubmit(pyrt);
                  else
                  {
                     PyrtBs1.Remove(pyrt);
                     return;
                  }
                  break;
               default:
                  break;
            }

            PyrtBs1.EndEdit();
            iScsc.SubmitChanges();

            requery = true;
         }
         catch (Exception ex)
         { MessageBox.Show(ex.Message); }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeskClose_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            //aodt.END_TIME = DateTime.Now.TimeOfDay;
            aodt.END_TIME = DateTime.Now;

            AodtBs1.EndEdit();

            iScsc.SubmitChanges();

            iScsc.CALC_APDT_P(aodt.AGOP_CODE, aodt.RWNO);
            requery = true;

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
         }
         catch (Exception exc)
         { MessageBox.Show(exc.Message); }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void CalcDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            // 1395/12/27 * اگر بخواهیم تا این مرحله از کار رو بررسی کنیم که چه میزان هزینه شده باید تاریخ ساعت پایان را داشته باشیم
            if (aodt.END_TIME == null)
            {
               aodt.END_TIME = DateTime.Now;
               AodtBs1.EndEdit();
               iScsc.SubmitChanges();
            }            

            iScsc.CALC_APDT_P(aodt.AGOP_CODE, aodt.RWNO);
            requery = true;

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
         }
         catch (Exception exc)
         { MessageBox.Show(exc.Message); }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void SaveStrtEndTime_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            AodtBs1.EndEdit();

            iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, aodt.FIGH_FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC);
            requery = true;

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
         }
         catch(Exception exc)
         { MessageBox.Show(exc.Message); }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void AodtBs1_PositionChanged(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            PosAmnt_Txt.EditValue = aodt.TOTL_AMNT_DNRM - ((aodt.CASH_AMNT ?? 0) + (aodt.POS_AMNT ?? 0));

            if (aodt.END_TIME == null)
               TotlMint_Txt.Text = "";
            else
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;

            if (aodt.STAT == "002")
            {
               if (aodt.Payment_Row_Types.Count > 0)
                  Pyrt_GridControl.Visible = true;
               else
                  Pyrt_GridControl.Visible = false;

               if (aodt.Buffets.Count > 0)
                  Bufe_GridControl.Visible = true;
               else
                  Bufe_GridControl.Visible = false;
            }
            else
            {
               Pyrt_GridControl.Visible = Bufe_GridControl.Visible = true;
            }
         }
         catch
         {
         }
      }

      private void OpenDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;

            if (agop == null)
            {
               MessageBox.Show("لیست دفتر امروز باز نشده");
               return;
            }

            if (agop.FROM_DATE.Value.Date != DateTime.Now.Date && MessageBox.Show(this, "ایا میز مورد نظر در تاریخ دیگری می خواهید باز کنید", "باز شدن میز در تاریخ غیر از امروز", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (ExpnDesk_GridLookUpEdit.EditValue.ToString() == "") { MessageBox.Show("میزی انتخاب نشده"); return; }
            var desk = Convert.ToInt64(ExpnDesk_GridLookUpEdit.EditValue);

            long? fileno = null;

            if (FIGH_FILE_NOLookUpEdit.EditValue != null && FIGH_FILE_NOLookUpEdit.EditValue.ToString() != "")
            {
               fileno = Convert.ToInt64(FIGH_FILE_NOLookUpEdit.EditValue);
            }
            if(TableCloseOpen)
            {
               // 1395/12/27 * میز هابه صورت پشت سر هم قرار میگیرند تا تسویه حساب شود
               var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;            
               iScsc.INS_AODT_P(agop.CODE, 1, aodt.AGOP_CODE, aodt.RWNO , fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null);
            }
            else
               iScsc.INS_AODT_P(agop.CODE, 1, null, null, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null);

            FIGH_FILE_NOLookUpEdit.EditValue = null;
            requery = true;
         }
         catch
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
               AodtBs1.MoveLast();
            }
         }
      }

      private void TransExpense2Another_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                  var newfileno = TransExpense2Another_Lov.EditValue;

                  if (newfileno == null || newfileno.ToString() == "") return;
                  if (crnt.FIGH_FILE_NO == (long)newfileno) return;
                  
                  if (MessageBox.Show(this, "آیا با انتقال هزینه به مشتری دیگر موافقید؟", "انتقال هزینه به مشتری دیگر", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                  crnt.Fighter = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == (long)newfileno);
                  iScsc.SubmitChanges();

                  requery = true;
                  if (crnt.RQST_RQID == null) return;
                  var rqro = iScsc.Request_Rows.FirstOrDefault(rr => rr.RQST_RQID == crnt.RQST_RQID);
                  rqro.Fighter = crnt.Fighter;
                  iScsc.SubmitChanges();
                  break;
               case 4:
                  iScsc = new Data.iScscDataContext(ConnectionString);
                  FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
                  return;
               case 5:
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  break;
               default:
                  break;
            }
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

      bool TableCloseOpen = false;
      private void CloseOpenTable_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;            
            if (aodt == null) return;

            TableCloseOpen = true;
            DeskClose_Butn_Click(null, null);
            ExpnDesk_GridLookUpEdit.EditValue = aodt.EXPN_CODE;
            FIGH_FILE_NOLookUpEdit.EditValue = null;
            OpenDesk_Butn_Click(null, null);
            TableCloseOpen = false;
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
               requery = true;
            }
         }
      }

      private void Regl_Butn_Click(object sender, EventArgs e)
      {
         var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
         if (Rg1 == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "016")))}
                  })
            );
      }

      private void AgopBs1_AddingNew(object sender, AddingNewEventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            agop.FROM_DATE = DateTime.Now;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Pos_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var amnt = Convert.ToInt64(PosAmnt_Txt.EditValue);
            if (amnt == 0) return;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            if (VPosBs1.List.Count == 0)
               UsePos_Cb.Checked = false;

            if (UsePos_Cb.Checked)
            {
               long psid;
               if (Pos_Lov.EditValue == null)
               {
                  var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                  if (posdflts.Count() == 1)
                     Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
                  else
                  {
                     Pos_Lov.Focus();
                     return;
                  }
               }
               else
               {
                  psid = (long)Pos_Lov.EditValue;
               }

               if (regl.AMNT_TYPE == "002")
                  amnt *= 10;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {
                              new Job(SendType.Self, 34 /* Execute PosPayment */)
                              {
                                 Input = 
                                    new XElement("PosRequest",
                                       new XAttribute("psid", psid),
                                       new XAttribute("subsys", 5),
                                       new XAttribute("rqid", aodt.AGOP_CODE),
                                       new XAttribute("rqtpcode", ""),
                                       new XAttribute("router", GetType().Name),
                                       new XAttribute("callback", 20),
                                       new XAttribute("amnt", amnt)
                                    )
                              }
                           }
                        )                     
                     }
                  )
               );
            }
            else
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithPOS4Agop"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("apdtagopcode", aodt.AGOP_CODE),
                           new XAttribute("apdtrwno", aodt.RWNO),
                           new XAttribute("amnt", amnt)
                        )
                     )
                  )
               );
            }
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
               Execute_Query();
         }

      }
   }
}
