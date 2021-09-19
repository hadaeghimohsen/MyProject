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
   public partial class AOP_BUFE_F : UserControl
   {
      public AOP_BUFE_F()
      {
         InitializeComponent();
      }

      private bool requery = false, setondebt = false;
      private int agopindx = 0, aodtindx = 0;
      private string stat, macadrs, fngrprnt;
      private bool isOnline = false;

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

         ExtpDeskBs1.DataSource =
            iScsc.Expense_Types.Where(et =>
               et.Request_Requester.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && et.Request_Requester.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
               et.Request_Requester.RQTP_CODE == "016" &&
               et.Request_Requester.RQTT_CODE == "007" &&
               et.Expenses.Any(ex => ex.EXPN_STAT == "002")
            );
         
         // 1398/12/17 * بخاطر اضافه شدن گزینه مربوط به نوع هزینه این گزینه اینجا بسته میشود
         //ExpnDeskBs1.DataSource =
         //   iScsc.Expenses.Where(ex =>
         //      ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
         //      ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
         //      ex.Expense_Type.Request_Requester.RQTT_CODE == "007" &&
         //      ex.EXPN_STAT == "002" /* هزینه های فعال */
         //   ).OrderBy(ed => ed.EXPN_DESC);

         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      #region Print Configuration
      #region Agop
      private void AgopBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AgopBnPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (AgopBs1.Current == null) return;
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Aggregation_Operation.Code = {0}", crnt.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AgopBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (AgopBs1.Current == null) return;
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Aggregation_Operation.Code = {0}", crnt.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AgopBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (AgopBs1.Current == null) return;
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Aggregation_Operation.Code = {0}", crnt.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }
      #endregion
      #region Figh
      private void FighBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void FighBnPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (Figh_Lov.EditValue == null || Figh_Lov.EditValue.ToString() == "") return;
            var crnt = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)Figh_Lov.EditValue);

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void FighBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (Figh_Lov.EditValue == null || Figh_Lov.EditValue.ToString() == "") return;
            var crnt = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)Figh_Lov.EditValue);

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void FighBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (Figh_Lov.EditValue == null || Figh_Lov.EditValue.ToString() == "") return;
            var crnt = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)Figh_Lov.EditValue);

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }
      #endregion
      #region Aodt
      private void AodtBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AodtBnPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (AodtBs1.Current == null) return;
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Aggregation_Operation_Detail.Agop_Code = {0} AND Aggregation_Operation_Detail.Rwno = {1}", crnt.AGOP_CODE, crnt.RWNO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AodtBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (AodtBs1.Current == null) return;
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Aggregation_Operation_Detail.Agop_Code = {0} AND Aggregation_Operation_Detail.Rwno = {1}", crnt.AGOP_CODE, crnt.RWNO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AodtBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         if (AodtBs1.Current == null) return;
         var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Aggregation_Operation_Detail.Agop_Code = {0} AND Aggregation_Operation_Detail.Rwno = {1}", crnt.AGOP_CODE, crnt.RWNO))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion
      #region Expn
      private void ExpnBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void ExpnBnPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (ExpnBufeBs1.Current == null) return;
            var crnt = ExpnBufeBs1.Current as Data.Expense;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Expense.Code = {0}", crnt.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void ExpnBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (ExpnBufeBs1.Current == null) return;
            var crnt = ExpnBufeBs1.Current as Data.Expense;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Expense.Code = {0}", crnt.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void ExpnBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (ExpnBufeBs1.Current == null) return;
            var crnt = ExpnBufeBs1.Current as Data.Expense;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Expense.Code = {0}", crnt.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }
      #endregion
      #endregion
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

            if (crnt == null || (crnt != null && MessageBox.Show(this, "آیا با انصراف دفتر فعلی موافق هستید؟", "انصراف دفتر", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)) return;

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
            if (crnt.STAT == "002") { MessageBox.Show("این رکورد قبلا در وضعیت نهایی قرار گرفته"); return; }
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
                  if (crnt.TOTL_AMNT_DNRM > ((crnt.CASH_AMNT ?? 0) + (crnt.POS_AMNT ?? 0) + (crnt.PYDS_AMNT ?? 0) + (crnt.DPST_AMNT ?? 0)))
                     setondebt = true;
                  else
                     setondebt = false;

                  if(crnt.STAT != "003")
                  {
                     MessageBox.Show(this, "میز بسته نشده لطفا دکمه بسته شدن میز را فشار دهید تا هزینه میز محاسبه شود");
                     return;
                     //if (MessageBox.Show(this, "میز بسته نشده! آیا میخواهید میز بسته شود؟", "میز بسته نشده", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                     //   return;
                     //DeskClose_Butn_Click(null, null);
                  }

                  // 1399/11/17 * اگر مشتری دارای مبلغ سپرده باشد اضافه بازی را به صورت تخفیف لحاظ میکنیم
                  if (crnt.Fighter.DPST_AMNT_DNRM > 0)
                     setondebt = false;

                  var unitamnt = iScsc.D_ATYPs.FirstOrDefault(d => d.VALU == crnt.Aggregation_Operation.Regulation.AMNT_TYPE);
                  // 1397/08/08 * برای حالت بدهکار شدن پیام هشدار نمایش داده شود
                  if(setondebt && 
                     MessageBox.Show(this, 
                        string.Format("مشترک هزینه میز به مبلغ" + " " + "{0}" + " " + "{1}" + " بدهکار می باشد! " + "\n\r" + 
                        "آیا عملیات ثبت بدهی انجام شود؟",
                        crnt.TOTL_AMNT_DNRM - (crnt.CASH_AMNT + crnt.POS_AMNT + crnt.PYDS_AMNT + crnt.DPST_AMNT),
                        unitamnt.DOMN_DESC
                        ), 
                        "ثبت هزینه در حساب دفتری مشترک", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                  {
                     return;
                  }
                  //if(setondebt && (crnt.CELL_PHON == null || crnt.CELL_PHON == ""))
                  //{
                  //   MessageBox.Show("برای هزینه هایی که در حالت بدهی ثبت می شوند باید شماره تلفن تماس ثبت گردد");
                  //   return;
                  //}
                  // اگر تغییری در اطلاعات به وجود آماده باشد تغییرات به صورت کلی ذخیره شود
                  iScsc.SubmitChanges();
                  iScsc.ENDO_RSBU_P(
                     new XElement("Aggregation_Operation_Detail",
                        new XAttribute("fileno", crnt.FIGH_FILE_NO),
                        new XAttribute("agopcode", crnt.AGOP_CODE),
                        new XAttribute("rwno", crnt.RWNO),
                        new XAttribute("setondebt", setondebt)
                     )
                  );

                  if(PrintExpnStat_Tsmi.CheckState == CheckState.Checked){
                     AodtBnPrintAfterPay_Click(null, null);
                  }

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

            // اگر میزی باز باشد که بسته نشده باشد
            // اگر میزی بسته شده باشد که برای مهمان آزاد باشد
            if(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(a => a.STAT == "001" || (a.STAT == "003" && a.Fighter.FGPB_TYPE_DNRM == "005" && a.CELL_PHON.Length == 0)))
            {
               MessageBox.Show("میزهای باز مانده یا میز های بسته شده که متعلق به مشتری آزاد میباشد که تسویه حساب نکرده اند باید تکلیف آنها را مشخص کنید");
               return;
            }

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
               if (Figh_Lov.EditValue == null || Figh_Lov.EditValue.ToString() == "") return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", Figh_Lov.EditValue), new XAttribute("auto", "true"))}
                     })
               );
            }

            var fileno = Convert.ToInt64(Figh_Lov.EditValue);

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
               var bufe = BufeBs1.List.OfType<Data.Buffet>().FirstOrDefault(b => b.EXPN_CODE == expn.CODE);
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
            }
         }
      }

      private void DeskClose_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            //aodt.END_TIME = DateTime.Now.TimeOfDay;
            aodt.END_TIME = DateTime.Now;
            aodt.STAT = "003";

            AodtBs1.EndEdit();

            iScsc.SubmitChanges();

            iScsc.CALC_APDT_P(aodt.AGOP_CODE, aodt.RWNO);
            requery = true;

            // ارسال پیام برای خاموش کردن دستگاه چراغ میز برای مشتری
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("ExpenseGame",
                         new XAttribute("type", "expnextr"),
                         new XAttribute("expncode", aodt.EXPN_CODE),
                         new XAttribute("cmndtext", "sp"),
                         new XAttribute("fngrprnt", aodt.Fighter.FNGR_PRNT_DNRM)
                     )
               }
            );

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
         }
         catch { }
         finally
         {
            if (requery)
            {
               var desk = AodtBs1.Current as Data.Aggregation_Operation_Detail;

               Execute_Query();

               // اگر سیستم به صورت انلاین اجرا شود
               if (isOnline)
               {
                  AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == desk.AGOP_CODE && a.RWNO == desk.RWNO));
                  desk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                  // اگر زمانی اتفاق بیوفتد که هزینه بازی برای مشتری از میزان مبلغ سپرده بیشتر شد کافیست که مبلغ هزینه را با مبلغ سپرده یکی کنیم
                  // که صورتحساب مشتری بدهکار نشود
                  if (desk.EXPN_PRIC >= desk.Fighter.DPST_AMNT_DNRM)
                     desk.EXPN_PRIC = (int)desk.Fighter.DPST_AMNT_DNRM; // مبلغ هزینه بازی را با میزان سپرده یکی قرار میدهیم
                  desk.DPST_AMNT = desk.EXPN_PRIC; // پرداخت هزینه میز را با سپرده انجام میدهیم
                  RecStat_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(RecStat_Butn.Buttons[3])); // تسویه حساب میز را انجام میدهیم
               }

               requery = false;
            }
         }
      }

      private void CalcDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            if (aodt.STAT.In("002")) { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }
            if (aodt.STAT.In("003")) { MessageBox.Show("میز بسته شده دیگر قادر به محاسبه نیست"); return; }

            // 1395/12/27 * اگر بخواهیم تا این مرحله از کار رو بررسی کنیم که چه میزان هزینه شده باید تاریخ ساعت پایان را داشته باشیم
            if (aodt.END_TIME == null || aodt.STRT_TIME > aodt.END_TIME)
            {
               aodt.END_TIME = DateTime.Now;
            }

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
         catch { }
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
            apdt_gv.PostEditor();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt.STAT.In("002")) { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستند"); Execute_Query(); return; }
            if (aodt.STAT.In("003")) { MessageBox.Show("میز بسته شده دیگر قادر به ویرایش نیست"); return; }

            AodtBs1.EndEdit();

            iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, aodt.FIGH_FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC, aodt.ATTN_TYPE, aodt.PYDS_AMNT, aodt.DPST_AMNT, aodt.BCDS_CODE);
            requery = true;

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
         }
         catch { }
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
            ServName_Tsmi.Text = "نامشخص";
            ServName_Tsmi.Enabled = false;
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            PosAmnt_Txt.EditValue = aodt.TOTL_AMNT_DNRM - ((aodt.CASH_AMNT ?? 0) + (aodt.POS_AMNT ?? 0) + (aodt.PYDS_AMNT ?? 0) + (aodt.DPST_AMNT ?? 0));

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

               Pos_Butn.Enabled = Cash_Butn.Enabled = false;
            }
            else
            {
               Pyrt_GridControl.Visible = Bufe_GridControl.Visible = true;
               Pos_Butn.Enabled = Cash_Butn.Enabled = true;
            }

            // 1398/08/19 * قرار دادن مبلغ و تایم بازی در منوی بازشو
            ExpnPric_Tsmi.Text = aodt.Expense.PRIC.ToString();
            ExpnMinTime_Tsmi.Text = aodt.Expense.MIN_TIME.Value.ToString("HH:mm");
            ServName_Tsmi.Text = aodt.Fighter.NAME_DNRM;
            ServName_Tsmi.Enabled = true;
            PayDebtAmnt1_Tsmi.Text = aodt.Fighter.DEBT_DNRM.ToString();
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

            if (Figh_Lov.EditValue != null && Figh_Lov.EditValue.ToString() != "")
            {
               fileno = Convert.ToInt64(Figh_Lov.EditValue);               
            }
            else
            {
               fileno = FighBs.OfType<Data.Fighter>().FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005").FILE_NO;
            }

            if (fileno == null) { MessageBox.Show("نام مشتری انتخاب نشده"); return; }

            // 1399/11/25 * اگر میز باز باشد دیگر اجازه باز کردن مجدد آن را نداشته یاشیم
            if(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(d => d.Aggregation_Operation == agop && d.EXPN_CODE == desk && d.REC_STAT == "002" && d.STAT == "001"))
            {
               MessageBox.Show("خطا - میزی که قصد باز کردن آن را دارید در حال حاضر باز می باشد، شما نمیتوانید دوباره همان میز را باز کنید");
               return;
            }

            if(TableCloseOpen)
            {
               // 1395/12/27 * میز هابه صورت پشت سر هم قرار میگیرند تا تسویه حساب شود
               var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;            
               iScsc.INS_AODT_P(agop.CODE, 1, aodt.AGOP_CODE, aodt.RWNO , fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null);
            }
            else
               iScsc.INS_AODT_P(agop.CODE, 1, null, null, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null);

            Figh_Lov.EditValue = null;
            Indpnd_Rb.Checked = true;
            requery = true;

            // ارسال پیام برای باز کردن دستگاه چراغ میز برای مشتری            
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("ExpenseGame",
                         new XAttribute("type", "expnextr"),
                         new XAttribute("expncode", desk),
                         new XAttribute("cmndtext", "st"),
                         new XAttribute("fngrprnt", FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == fileno).FNGR_PRNT_DNRM)
                     )
               }
            );            
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
            apdt_gv.PostEditor();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            TableCloseOpen = true;
            DeskClose_Butn_Click(null, null);
            ExpnDesk_GridLookUpEdit.EditValue = aodt.EXPN_CODE;
            Figh_Lov.EditValue = null;
            //Figh_Lov.EditValue = aodt.FIGH_FILE_NO;
            OpenDesk_Butn_Click(null, null);
            TableCloseOpen = false;
            requery = true;
         }
         catch { }
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
            apdt_gv.PostEditor();
            iScsc.SubmitChanges();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var amnt = Convert.ToInt64(PosAmnt_Txt.EditValue);
            if (amnt == 0) return;

            // 1400/05/24 * اگر مبلغ وارد شده از مبلغ کل بیشتر باشد
            if (amnt > (aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT))) { if (MessageBox.Show(this, "مبلغ کارتخوان شما از مبلغ کل هزینه میز بیشتر میباشد، ایا مایل به اصلاح قیمت میباشد؟", "مغایرت مالی در وصولی", MessageBoxButtons.YesNo) != DialogResult.Yes) return; else amnt = (long)(aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT)); }

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
            PosAmnt_Txt.EditValue = 0;
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

      private void AgopBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) { TotlRcptAmnt_Txt.Text = TotlDebtAmnt_Txt.Text = TotlCashAmnt_Txt.Text = TotlPosAmnt_Txt.Text = "0"; return; }

            TotlDebtAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.TOTL_AMNT_DNRM).ToString();
            TotlRcptAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.POS_AMNT + d.CASH_AMNT).ToString();
            TotlCashAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.CASH_AMNT).ToString();
            TotlPosAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.POS_AMNT).ToString();
            TotlRemnAmnt_Txt.Text = (agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.TOTL_AMNT_DNRM) - agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.POS_AMNT + d.CASH_AMNT + d.PYDS_AMNT + d.DPST_AMNT)).ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Lbl_Click(object sender, EventArgs e)
      {
         LabelControl lbl = (LabelControl)sender;
         AllRcrd_Lbl.Tag = lbl.Name;
         switch (lbl.Name)
         {
            case "TablOpen_Lbl":
               apdt_gv.ActiveFilterString = "STAT = '001' And Rec_Stat = '002'";
               break;
            case "TablClosDontCash_Lbl":
               apdt_gv.ActiveFilterString = "STAT = '003' And Rec_Stat = '002'";
               break;
            case "TablClosDoCash_Lbl":
               apdt_gv.ActiveFilterString = "STAT = '002' And Rec_Stat = '002'";
               break;
            case "AllRcrd_Lbl":
               apdt_gv.ActiveFilterString = "Rec_Stat = '002'";
               break;
         }
      }

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }

      private void Cash_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();
            iScsc.SubmitChanges();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var amnt = Convert.ToInt64(PosAmnt_Txt.EditValue);
            if (amnt == 0) return;

            // 1400/05/24 * اگر مبلغ وارد شده از مبلغ کل بیشتر باشد
            if (amnt > (aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT))) { if (MessageBox.Show(this, "مبلغ نقدی شما از مبلغ کل هزینه میز بیشتر میباشد، ایا مایل به اصلاح قیمت میباشد؟", "مغایرت مالی در وصولی", MessageBoxButtons.YesNo) != DialogResult.Yes) return; else amnt = (long)(aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT)); }

            aodt.CASH_AMNT += amnt;

            AodtBs1.EndEdit();

            iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, aodt.FIGH_FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC, aodt.ATTN_TYPE, aodt.PYDS_AMNT, aodt.DPST_AMNT, aodt.BCDS_CODE);
            requery = true;

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
            PosAmnt_Txt.EditValue = 0;
         }
         catch(Exception exc)
         { MessageBox.Show(exc.Message); }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void ExpnDesk_GridLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if (e.Button.Index == 1)
         {
            if (e.Button.Caption == "خودکار")
            {
               e.Button.Caption = "دستی";
               e.Button.Tag = "manual";
            }
            else
            {
               e.Button.Caption = "خودکار";
               e.Button.Tag = "auto";
            }
         }
      }

      private void ExpnDesk_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if(e.NewValue != null && ExpnDesk_GridLookUpEdit.Properties.Buttons[1].Tag.ToString() == "auto")
         {
            //OpenDesk_Butn_Click(null, null);

            try
            {
               var agop = AgopBs1.Current as Data.Aggregation_Operation;

               if (agop == null)
               {
                  if(MessageBox.Show(this, "دفتر لیست امروز باز نشده! آیا به صورت خودکار دفتر لیست باز شود؟", "دفتر لیست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)return;
                  AgopBs1.AddNew();
                  Edit_Butn_Click(null, null);
                  agop = AgopBs1.Current as Data.Aggregation_Operation;
               }

               if (agop.FROM_DATE.Value.Date != DateTime.Now.Date && MessageBox.Show(this, "ایا میز مورد نظر در تاریخ دیگری می خواهید باز کنید", "باز شدن میز در تاریخ غیر از امروز", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               //if (e.NewValue.ToString() == "") { MessageBox.Show("میزی انتخاب نشده"); return; }
               var desk = Convert.ToInt64(e.NewValue);

               long? fileno = null;

               if (Figh_Lov.EditValue != null && Figh_Lov.EditValue.ToString() != "")
               {
                  fileno = Convert.ToInt64(Figh_Lov.EditValue);
               }
               else
               {
                  fileno = FighBs.OfType<Data.Fighter>().FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005").FILE_NO;
               }

               if (fileno == null) { MessageBox.Show("نام مشتری انتخاب نشده"); return; }

               if (TableCloseOpen)
               {
                  // 1395/12/27 * میز هابه صورت پشت سر هم قرار میگیرند تا تسویه حساب شود
                  var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                  iScsc.INS_AODT_P(agop.CODE, 1, aodt.AGOP_CODE, aodt.RWNO, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null);

                  // ارسال پیام برای خاموش کردن دستگاه چراغ میز برای مشتری
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("ExpenseGame",
                               new XAttribute("type", "expnextr"),
                               new XAttribute("expncode", aodt.EXPN_CODE),
                               new XAttribute("cmndtext", "sp"),
                               new XAttribute("fngrprnt", aodt.Fighter.FNGR_PRNT_DNRM)
                           )
                     }
                  );
               }
               else
               {
                  iScsc.INS_AODT_P(agop.CODE, 1, null, null, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null);

                  // ارسال پیام برای باز کردن دستگاه چراغ میز برای مشتری                              
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("ExpenseGame",
                               new XAttribute("type", "expnextr"),
                               new XAttribute("expncode", desk),
                               new XAttribute("cmndtext", "st"),
                               new XAttribute("fngrprnt", FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == fileno).FNGR_PRNT_DNRM)
                           )
                     }
                  );
               }

               Figh_Lov.EditValue = null;
               requery = true;
            }
            catch {}
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
      }

      private void QuickFindCellPhon_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var fighs = iScsc.Fighters.Where(f => f.CELL_PHON_DNRM == aodt.CELL_PHON && f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0);
            if(fighs.Count() == 0)
            {
               MessageBox.Show("مشتری با این شماره تلفن یافت نشد");
               return;
            }
            else if(fighs.Count() > 1)
            {
               if(MessageBox.Show(this, "با این شماره تعدادی مشتری مشاهده شد! آیا مایل به بررسی هستید؟", "رکورد تکراری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               {
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */),
                           new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"), new XAttribute("filtering", "cellphon"), new XAttribute("filter_value", aodt.CELL_PHON))}
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);
               }
            }
            else
            {
               iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, fighs.FirstOrDefault().FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC, aodt.ATTN_TYPE, aodt.PYDS_AMNT, aodt.DPST_AMNT, aodt.BCDS_CODE);
            }

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

      private void ExpnEdit_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var expn = aodt.Expense;
            expn.PRIC = Convert.ToInt32(ExpnPric_Tsmi.Text);
            expn.MIN_TIME = DateTime.ParseExact(ExpnMinTime_Tsmi.Text, "hh:mm", System.Globalization.CultureInfo.CurrentCulture);

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

      private void ExpnDup_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var expn = aodt.Expense;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", expn.CODE),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", DupExpnDesc_Tsmi.Text),
                  new XAttribute("pric", DupExpnPric_Tsmi.Text),
                  new XAttribute("mintime", DupExpnMinTime_Tsmi.Text)
               )
            );

            DupExpnPric_Tsmi.Text = DupExpnDesc_Tsmi.Text = DupExpnMinTime_Tsmi.Text = "";

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

      private void NewExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var expn = aodt.Expense;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", ""),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", NewExpnDesc_Tsmi.Text),
                  new XAttribute("pric", NewExpnPric_Tsmi.Text),
                  new XAttribute("mintime", NewExpnMinTime_Tsmi.Text)
               )
            );

            NewExpnPric_Tsmi.Text = NewExpnDesc_Tsmi.Text = NewExpnMinTime_Tsmi.Text = "";

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

      private void FIGH_FILE_NOLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            ServInfo2_Tsmi.Text = "نامشخص";
            if (e.NewValue == null || e.NewValue.ToString() == "") return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)e.NewValue);

            ServInfo2_Tsmi.Text = figh.NAME_DNRM;
            PayDebtAmnt2_Tsmi.Text = figh.DEBT_DNRM.ToString();
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
         }
      }

      private void ServInfo1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", aodt.FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ServEdit1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", aodt.FIGH_FILE_NO), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region پرداخت بدهی
      private void PayCashDebt1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            // اگر مشترکی وجود نداشته باشد
            if (aodt == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (aodt.Fighter.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (aodt.Fighter.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt1_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > aodt.Fighter.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, aodt.FIGH_FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "001"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayPosDebt1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            // اگر مشترکی وجود نداشته باشد
            if (aodt == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (aodt.Fighter.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (aodt.Fighter.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt1_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > aodt.Fighter.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, aodt.FIGH_FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "003"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayDepositeDebt1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            // اگر مشترکی وجود نداشته باشد
            if (aodt == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (aodt.Fighter.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (aodt.Fighter.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt1_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > aodt.Fighter.DEBT_DNRM) return;
            // اگر مبلغ پرداخت از مبلغ سپرده بیشتر باشد
            if (paydebt > aodt.Fighter.DPST_AMNT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, aodt.FIGH_FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "005"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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
      #endregion

      #region افزایش سپرده
      private void PayCashDepositeAmnt1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (aodt.Fighter.FIGH_STAT == "001") return;
            var paydeposite = Convert.ToInt64(PayDepositeAmnt1_Tsmi.Text.Replace(",", ""));
            if (paydeposite == 0) return;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", aodt.FIGH_FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", 0),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", paydeposite),
                        new XAttribute("paiddate", ""),
                        new XAttribute("dpststat", "002"),
                        new XAttribute("resndesc", "افزایش سپرده در فرم کنترل میز"),
                        new XElement("Gain_Loss_Rial_Detials",                           
                           new XElement("Gain_Loss_Rial_Detial",
                              new XAttribute("rwno", 1),
                              new XAttribute("amnt", paydeposite),
                              new XAttribute("rcptmtod", "001")
                           )
                        )
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

             var Rqst =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               ).FirstOrDefault();

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );

            PayDepositeAmnt1_Tsmi.Text = "0";

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

      private void PayPosDepositeAmnt1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (aodt.Fighter.FIGH_STAT == "001") return;
            var paydeposite = Convert.ToInt64(PayDepositeAmnt1_Tsmi.Text.Replace(",", ""));
            if (paydeposite == 0) return;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", aodt.FIGH_FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", 0),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", paydeposite),
                        new XAttribute("paiddate", ""),
                        new XAttribute("dpststat", "002"),
                        new XAttribute("resndesc", "افزایش سپرده در فرم کنترل میز"),
                        new XElement("Gain_Loss_Rial_Detials",
                           new XElement("Gain_Loss_Rial_Detial",
                              new XAttribute("rwno", 1),
                              new XAttribute("amnt", paydeposite),
                              new XAttribute("rcptmtod", "003")
                           )
                        )
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
              iScsc.Requests
              .Where(
                 rqst =>
                    Rqids.Contains(rqst.RQID)
              ).FirstOrDefault();

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );

            PayDepositeAmnt1_Tsmi.Text = "0";

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
      #endregion

      private void ServPymt1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", aodt.FIGH_FILE_NO)
                           )
                     },
                     new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", aodt.FIGH_FILE_NO),
                           new XAttribute("type", "refresh"),
                           new XAttribute("tabfocued", "tp_003")
                        )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveServ1_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.BYR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "025"),
                     new XAttribute("rqttcode", "004"),
                     new XAttribute("prvncode", "017"),
                     new XAttribute("regncode", "001"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName1_Tsmi.Text),
                        new XElement("Last_Name", LastName1_Tsmi.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Sex_Type", "001"),
                        new XElement("Cell_Phon", CellPhon1_Tsmi.Text),
                        new XElement("Type", "001"),
                        new XElement("Fngr_Prnt", 
                           iScsc.Fighters
                            .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                            .Select(f => f.FNGR_PRNT_DNRM)
                            .ToList()
                            .Where(f => f.All(char.IsDigit))
                            .Max(f => Convert.ToInt64(f)) + 1)
                     ),
                     new XElement("Member_Ship",
                        new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst = 
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               ).FirstOrDefault();

            iScsc.BYR_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                        new XAttribute("regncode", Rqst.REGN_CODE),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );

            FrstName1_Tsmi.Text = LastName1_Tsmi.Text = CellPhon1_Tsmi.Text = "";

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

      #region Service Popup Menu 2
      private void ServInfo2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ServEdit2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", fileno), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region پرداخت بدهی قبلی
      private void PayCashDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "001"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayPosDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "003"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayDepositeDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "005"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
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
      #endregion

      #region سپرده گذاری سیستم
      private void PayCashDepositeAmnt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;
            var paydeposite = Convert.ToInt64(PayDepositeAmnt2_Tsmi.Text.Replace(",", ""));
            if (paydeposite == 0) return;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", figh.FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", 0),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", paydeposite),
                        new XAttribute("paiddate", ""),
                        new XAttribute("dpststat", "002"),
                        new XAttribute("resndesc", "افزایش سپرده در فرم کنترل میز"),
                        new XElement("Gain_Loss_Rial_Detials",
                           new XElement("Gain_Loss_Rial_Detial",
                              new XAttribute("rwno", 1),
                              new XAttribute("amnt", paydeposite),
                              new XAttribute("rcptmtod", "001")
                           )
                        )
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
              iScsc.Requests
              .Where(
                 rqst =>
                    Rqids.Contains(rqst.RQID)
              ).FirstOrDefault();

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );

            PayDepositeAmnt2_Tsmi.Text = "0";

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

      private void PayPosDepositeAmnt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;
            var paydeposite = Convert.ToInt64(PayDepositeAmnt2_Tsmi.Text.Replace(",", ""));
            if (paydeposite == 0) return;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", figh.FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", 0),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", paydeposite),
                        new XAttribute("paiddate", ""),
                        new XAttribute("dpststat", "002"),
                        new XAttribute("resndesc", "افزایش سپرده در فرم کنترل میز"),
                        new XElement("Gain_Loss_Rial_Detials",
                           new XElement("Gain_Loss_Rial_Detial",
                              new XAttribute("rwno", 1),
                              new XAttribute("amnt", paydeposite),
                              new XAttribute("rcptmtod", "003")
                           )
                        )
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
              iScsc.Requests
              .Where(
                 rqst =>
                    Rqids.Contains(rqst.RQID)
              ).FirstOrDefault();

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );

            PayDepositeAmnt2_Tsmi.Text = "0";

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
      #endregion

      private void ServPymt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            var figh = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                     },
                     new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", figh.FILE_NO),
                           new XAttribute("type", "refresh"),
                           new XAttribute("tabfocued", "tp_003")
                        )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveServ2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.BYR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "025"),
                     new XAttribute("rqttcode", "004"),
                     new XAttribute("prvncode", "017"),
                     new XAttribute("regncode", "001"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName2_Tsmi.Text),
                        new XElement("Last_Name", LastName2_Tsmi.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Sex_Type", "001"),
                        new XElement("Cell_Phon", CellPhon2_Tsmi.Text),
                        new XElement("Type", "001"),
                        new XElement("Fngr_Prnt",
                           iScsc.Fighters
                            .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                            .Select(f => f.FNGR_PRNT_DNRM)
                            .ToList()
                            .Where(f => f.All(char.IsDigit))
                            .Max(f => Convert.ToInt64(f)) + 1)
                     ),
                     new XElement("Member_Ship",
                        new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               ).FirstOrDefault();

            iScsc.BYR_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                        new XAttribute("regncode", Rqst.REGN_CODE),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );

            FrstName2_Tsmi.Text = LastName2_Tsmi.Text = CellPhon2_Tsmi.Text = "";

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
      #endregion

      private void ExpnBufeBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBufeBs1.Current as Data.Expense;
            if (expn == null) return;

            ExpnItem3_Tsmi.Text = ExpnDesc3_Tsmi.Text = expn.EXPN_DESC;
            ExpnPric3_Tsmi.Text = expn.PRIC.ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExpnItem3_Tsmi_Click(object sender, EventArgs e)
      {
         AddItem_Butn_ButtonClick(null, null);
         ExpnBufe_Gc.Focus();
      }

      private void ExpnEdit3_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBufeBs1.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_DESC = ExpnDesc3_Tsmi.Text;
            expn.PRIC = Convert.ToInt32(ExpnPric3_Tsmi.Text);

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

      private void DupExpn3_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBufeBs1.Current as Data.Expense;
            if (expn == null) return;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", expn.CODE),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", DupExpnText3_Tsmi.Text),
                  new XAttribute("pric", DupExpnPric3_Tsmi.Text)
               )
            );

            DupExpnPric3_Tsmi.Text = DupExpnText3_Tsmi.Text = "";

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

      private void OffExpn3_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBufeBs1.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_STAT = "001";
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

      private void NewExpn3_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBufeBs1.Current as Data.Expense;
            if (expn == null) return;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", ""),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", NewExpnText3_Tsmi.Text),
                  new XAttribute("pric", NewExpnPric3_Tsmi.Text)
               )
            );

            NewExpnPric3_Tsmi.Text = NewExpnText3_Tsmi.Text = "";

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

      private void RecalcDesks_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var crntdesk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (crntdesk == null) return;

            // 1399/11/25 * برای گزینه های میز باز باید محاسبه مجدد در نظر گرفته شود
            var desks = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(d => d.STAT == "001" /* میز های باز نیاز به محاسبه مجدد دارند */);
            // اگر میز بازی وجود نداشته باشد
            if (desks.Count() == 0) return;
            
            int value = 100 / desks.Count();
            RecalcDesk_Pgb.Visible = true;
            RecalcDesk_Pgb.Value = 0;
            foreach (var desk in desks)
            {
               AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == desk.AGOP_CODE && d.RWNO == desk.RWNO));
               CalcDesk_Butn_Click(null, null);
               RecalcDesk_Pgb.Value += value;
            }

            RecalcDesk_Pgb.Value = 100;
            AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == crntdesk.AGOP_CODE && d.RWNO == crntdesk.RWNO));
         }
         catch (Exception exc)
         {            
            MessageBox.Show(exc.Message);
         }
         finally
         {
            RecalcDesk_Pgb.Visible = false;
            
            // اگر سیستم به صورت انلاین کار کند که بر اساس اعتبار مشتریان در ارتباط باشد
            if (isOnline)
            {
               // آن دسته از مشتریانی که اعتبار آنها تمام شده است باید دستگاه را خاموش کنیم
               var desks = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(d => d.STAT != "002");
               foreach (var desk in desks)
               {
                  if (desk.EXPN_PRIC >= desk.Fighter.DPST_AMNT_DNRM)
                  {
                     AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a == desk));
                     DeskClose_Butn_Click(null, null);
                     //var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                     //// اگر زمانی اتفاق بیوفتد که هزینه بازی برای مشتری از میزان مبلغ سپرده بیشتر شد کافیست که مبلغ هزینه را با مبلغ سپرده یکی کنیم
                     //// که صورتحساب مشتری بدهکار نشود
                     //if (desk.EXPN_PRIC >= desk.Fighter.DPST_AMNT_DNRM)
                     //   desk.EXPN_PRIC = (int)desk.Fighter.DPST_AMNT_DNRM; // مبلغ هزینه بازی را با میزان سپرده یکی قرار میدهیم
                     //desk.DPST_AMNT = desk.EXPN_PRIC; // پرداخت هزینه میز را با سپرده انجام میدهیم
                     ////aodt.EXPN_PRIC = (int)aodt.Fighter.DPST_AMNT_DNRM; // مبلغ هزینه بازی را با میزان سپرده یکی قرار میدهیم
                     ////aodt.DPST_AMNT = aodt.EXPN_PRIC; // پرداخت هزینه میز را با سپرده انجام میدهیم
                     //RecStat_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(RecStat_Butn.Buttons[3])); // تسویه حساب میز را انجام میدهیم
                  }
               }
            }
         }
      }

      private void CloseDesk_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var crntdesk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (crntdesk == null) return;

            var desks = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(d => d.STAT != "002");
            foreach (var desk in desks)
            {
               AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == desk.AGOP_CODE && d.RWNO == desk.RWNO));
               DeskClose_Butn_Click(null, null);
            }

            AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == crntdesk.AGOP_CODE && d.RWNO == crntdesk.RWNO));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         } 
      }

      private void AutoRecalc_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            switch (AutoRecalc_Tsmi.CheckState)
            {
               case CheckState.Checked:
                  AutoRecalc_Tsmi.CheckState = CheckState.Unchecked;
                  AutoRecalc_Tmr.Enabled = IntervalRecalc_Tsmi.Enabled = false;
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
                  break;
               case CheckState.Unchecked:
                  AutoRecalc_Tsmi.CheckState = CheckState.Checked;
                  AutoRecalc_Tmr.Interval = Convert.ToInt32(IntervalRecalc_Tsmi.Text) * 60000;
                  AutoRecalc_Tmr.Enabled = IntervalRecalc_Tsmi.Enabled = true;
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.YellowGreen;
                  break;
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintExpnStat_Tsmi_Click(object sender, EventArgs e)
      {
         switch(PrintExpnStat_Tsmi.CheckState)
         {
            case CheckState.Checked:
               PrintExpnStat_Tsmi.CheckState = CheckState.Unchecked;
               break;
            case CheckState.Unchecked:
               PrintExpnStat_Tsmi.CheckState = CheckState.Checked;
               break;
         }
      }

      private void ExtpDesk_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (e.NewValue == null)
            {
               ExpnDeskBs1.Clear();
               return;
            }

            ExpnDeskBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "007" &&
                  ex.Expense_Type.CODE == (long)e.NewValue &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */ 

               ).OrderBy(ed => ed.EXPN_DESC);

            ExpnDesk_GridLookUpEdit.EditValue = null;
            //if (ExpnDeskBs1.List.Count > 0)
            //{
            //   ExpnDeskBs1.MoveFirst();
            //   ExpnDesk_GridLookUpEdit.EditValue = (ExpnDeskBs1.Current as Data.Expense).CODE;
            //}
            if(ExpnDeskBs1.List.Count == 1)
            {
               ExpnDeskBs1.MoveFirst();
               ExpnDesk_GridLookUpEdit.EditValue = (ExpnDeskBs1.Current as Data.Expense).CODE;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void IntervalRecalc_Tsmi_TextChanged(object sender, EventArgs e)
      {
         try
         {
            AutoRecalc_Tmr.Interval = Convert.ToInt32(IntervalRecalc_Tsmi.Text) * 60000;
         }
         catch 
         { 
            IntervalRecalc_Tsmi.Text = "1";
            AutoRecalc_Tmr.Interval = Convert.ToInt32(IntervalRecalc_Tsmi.Text) * 60000;
         }
      }

      private void Indpnd_Rb_CheckedChanged(object sender, EventArgs e)
      {
         // 1400/05/24 * اگر بخواهیم مشتریانی داشته باشیم که در ان واحد بازی های مختلفی را انجام میدهند
         if (AodtBs1.List.Count == 0) { TableCloseOpen = false; Indpnd_Rb.Checked = true; return; }
         TableCloseOpen = !Indpnd_Rb.Checked;
         if(TableCloseOpen)
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            Figh_Lov.EditValue = aodt.FIGH_FILE_NO;
         }
      }
   }
}
