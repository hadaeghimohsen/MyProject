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
using System.Threading;
using System.Scsc.ExtCode;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

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
      private List<Data.Aggregation_Operation_Detail> _listAodts = new List<Data.Aggregation_Operation_Detail>();

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         agopindx = AgopBs1.Position;
         aodtindx = AodtBs1.Position;
         if (SaveInfoStat_Rb.Checked)
            AgopBs1.DataSource = iScsc.Aggregation_Operations.Where(a => a.OPRT_TYPE == "005" && (a.OPRT_STAT == "001" || a.OPRT_STAT == "002")).OrderByDescending(ag => ag.FROM_DATE);
         else
         {
            RprtFromDate_Dt.CommitChanges();
            RprtToDate_Dt.CommitChanges();

            if (!RprtFromDate_Dt.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); RprtFromDate_Dt.Focus(); return; }
            if (!RprtToDate_Dt.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); RprtFromDate_Dt.Focus(); return; }

            AgopBs1.DataSource =
               iScsc.Aggregation_Operations
               .Where(a =>
                  a.OPRT_TYPE == "005" &&
                  a.OPRT_STAT == "004" &&
                  (!RprtFromDate_Dt.Value.HasValue || a.FROM_DATE.Value.Date >= RprtFromDate_Dt.Value.Value.Date) &&
                  (!RprtToDate_Dt.Value.HasValue || a.FROM_DATE.Value.Date <= RprtToDate_Dt.Value.Value.Date)
                )
               .OrderByDescending(ag => ag.FROM_DATE);
         }
         AgopBs1.Position = agopindx;
         AodtBs1.Position = aodtindx;

         FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);

         ClubBs1.DataSource = iScsc.Clubs.Where(c => Fga_Uclb_U.Contains(c.CODE));

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

         // 1401/01/03 * کنار مصطفی تو استخر هوابرد
         if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.RUN_QURY == "002"))
            SearchCustTell_Butn_Click(null, null);

         SunsBs.DataSource = iScsc.App_Base_Defines.Where(a => a.ENTY_NAME == "Suns_Group");
         StngBs.DataSource = iScsc.Settings.FirstOrDefault();

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

            // check admin wallet
            var _admnwlet = iScsc.V_Admin_Wallets.FirstOrDefault(w => w.WLET_TYPE == "001");
            if(_admnwlet != null)
            {
               if(_admnwlet.AMNT_DNRM <= 10000)
               {
                  MessageBox.Show(this, "شارژ کیف پول شما تمام شده، لطفا جهت شارژ کیف پول اقدام کنید", "شارژ کیف پول", MessageBoxButtons.OK);
                  return;
               }
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
            // 1401/01/18 - اگر فرم در وضعیت ثبت اطلاعات نباشد نبایستی  عملیاتی انجام شود
            if (!SaveInfoStat_Rb.Checked) return;

            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            
            // 1403/06/09 * IF sender object has data object
            if (sender is Data.Aggregation_Operation_Detail)
               crnt = sender as Data.Aggregation_Operation_Detail;

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

      private void RecStat_Butn_ButtonClick(Data.Aggregation_Operation_Detail desk)
      {
         try
         {
            var crnt = desk;

            if (crnt.STAT == "002") { MessageBox.Show("این رکورد قبلا در وضعیت نهایی قرار گرفته"); return; }
            
            if (crnt.TOTL_AMNT_DNRM > ((crnt.CASH_AMNT ?? 0) + (crnt.POS_AMNT ?? 0) + (crnt.PYDS_AMNT ?? 0) + (crnt.DPST_AMNT ?? 0)))
               setondebt = true;
            else
               setondebt = false;

            if (crnt.STAT != "003")
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
            if (setondebt &&
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

            if (PrintExpnStat_Tsmi.CheckState == CheckState.Checked)
            {
               AodtBnPrintAfterPay_Click(null, null);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
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
            // 1401/01/18 - اگر فرم در وضعیت ثبت اطلاعات نباشد نبایستی  عملیاتی انجام شود
            if (!SaveInfoStat_Rb.Checked) return;

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
            var _desk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            
            // 1403/06/11
            if (sender is Data.Aggregation_Operation_Detail)
               _desk = sender as Data.Aggregation_Operation_Detail;

            if (_desk == null) return;

            if (_desk.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            //aodt.END_TIME = DateTime.Now.TimeOfDay;
            _desk.END_TIME = DateTime.Now;
            _desk.STAT = "003";

            AodtBs1.EndEdit();

            iScsc.SubmitChanges();

            iScsc.CALC_APDT_P(_desk.AGOP_CODE, _desk.RWNO);
            requery = true;

            // ارسال پیام برای خاموش کردن دستگاه چراغ میز برای مشتری
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("ExpenseGame",
                         new XAttribute("type", "expnextr"),
                         new XAttribute("expncode", _desk.EXPN_CODE),
                         new XAttribute("cmndtext", "sp"),
                         new XAttribute("fngrprnt", _desk.Fighter.FNGR_PRNT_DNRM)
                     )
               }
            );

            if (_desk.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (_desk.END_TIME.Value - _desk.STRT_TIME.Value).TotalMinutes;
            }

            // 1403/06/10 * اگر انتخاب شود که بعد از بسته شدن فاکتور تسویه حساب شود می توانید
            if(FinlRec_Cbx.Checked)
            {
               CalcDesk_Butn_Click(null, null);
               // 1403/06/11 * بدلیل اینکه کاربر میتواند فقط میز های باز رو بررسی کند احتمال ایجاد خطا وجود دارن
               //AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == _desk.AGOP_CODE && a.RWNO == _desk.RWNO));
               //_desk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
               _desk = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == _desk.AGOP_CODE && a.RWNO == _desk.RWNO);
               _desk.CASH_AMNT = _desk.TOTL_AMNT_DNRM - (/*(aodt.CASH_AMNT ?? 0) +*/ (_desk.POS_AMNT ?? 0) + (_desk.PYDS_AMNT ?? 0) + (_desk.DPST_AMNT ?? 0));
               RecStat_Butn_ButtonClick(_desk, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(RecStat_Butn.Buttons[3])); // تسویه حساب میز را انجام میدهیم
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

      private void DeskClose_Butn_Click(Data.Aggregation_Operation_Detail aodt)
      {
         try
         {
            apdt_gv.PostEditor();
            if (aodt.STAT == "002") { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }

            //aodt.END_TIME = DateTime.Now.TimeOfDay;
            aodt.END_TIME = DateTime.Now;
            aodt.STAT = "003";

            AodtBs1.EndEdit();

            //iScsc.SubmitChanges();

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Aggregation_Operation_Detail SET End_Time = GETDATE(), Stat = '003' WHERE Agop_Code = {0} AND Rwno = {1}; EXEC dbo.Calc_Apdt_P @Agop_Code = {0}, @Rwno = {1};", aodt.AGOP_CODE, aodt.RWNO));

            //iScsc.CALC_APDT_P(aodt.AGOP_CODE, aodt.RWNO);
            //requery = true;

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
         //finally
         //{
         //   if (requery)
         //   {
         //      var desk = AodtBs1.Current as Data.Aggregation_Operation_Detail;

         //      Execute_Query();

         //      // اگر سیستم به صورت انلاین اجرا شود
         //      if (isOnline)
         //      {
         //         AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == desk.AGOP_CODE && a.RWNO == desk.RWNO));
         //         desk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
         //         // اگر زمانی اتفاق بیوفتد که هزینه بازی برای مشتری از میزان مبلغ سپرده بیشتر شد کافیست که مبلغ هزینه را با مبلغ سپرده یکی کنیم
         //         // که صورتحساب مشتری بدهکار نشود
         //         if (desk.EXPN_PRIC >= desk.Fighter.DPST_AMNT_DNRM)
         //            desk.EXPN_PRIC = (int)desk.Fighter.DPST_AMNT_DNRM; // مبلغ هزینه بازی را با میزان سپرده یکی قرار میدهیم
         //         desk.DPST_AMNT = desk.EXPN_PRIC; // پرداخت هزینه میز را با سپرده انجام میدهیم
         //         RecStat_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(RecStat_Butn.Buttons[3])); // تسویه حساب میز را انجام میدهیم
         //      }

         //      requery = false;
         //   }
         //}
      }

      private void CalcDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (sender is Data.Aggregation_Operation_Detail)
               aodt = sender as Data.Aggregation_Operation_Detail;

            if (aodt == null) return;

            if (aodt.STAT.In("002")) { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }
            //if (aodt.STAT.In("003")) { MessageBox.Show("میز بسته شده دیگر قادر به محاسبه نیست"); return; }

            // 1395/12/27 * اگر بخواهیم تا این مرحله از کار رو بررسی کنیم که چه میزان هزینه شده باید تاریخ ساعت پایان را داشته باشیم
            if (aodt.END_TIME == null || aodt.STRT_TIME > aodt.END_TIME || ChckCalcEndTime_Cbx.Checked)
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

      private void CalcDesk_Butn_Click(Data.Aggregation_Operation_Detail aodt)
      {
         try
         {
            apdt_gv.PostEditor();
            if (aodt.STAT.In("002")) { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستید"); return; }
            //if (aodt.STAT.In("003")) { MessageBox.Show("میز بسته شده دیگر قادر به محاسبه نیست"); return; }

            // 1395/12/27 * اگر بخواهیم تا این مرحله از کار رو بررسی کنیم که چه میزان هزینه شده باید تاریخ ساعت پایان را داشته باشیم
            if (aodt.END_TIME == null || aodt.STRT_TIME > aodt.END_TIME || ChckCalcEndTime_Cbx.Checked)
            {
               aodt.END_TIME = DateTime.Now;
            }

            AodtBs1.EndEdit();
            /*iScsc.SubmitChanges();

            iScsc.CALC_APDT_P(aodt.AGOP_CODE, aodt.RWNO);*/
            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Aggregation_Operation_Detail SET End_Time = '{2}' WHERE Agop_Code = {0} AND Rwno = {1}; EXEC dbo.Calc_Apdt_P @Agop_Code = {0}, @Rwno = {1};", aodt.AGOP_CODE, aodt.RWNO, aodt.END_TIME));
            //requery = true;

            if (aodt.END_TIME != null)
            {
               //TotlMint_Txt.EditValue = aodt.END_TIME.Value.TimeOfDay.TotalMinutes - aodt.STRT_TIME.Value.TimeOfDay.TotalMinutes;
               TotlMint_Txt.EditValue = (aodt.END_TIME.Value - aodt.STRT_TIME.Value).TotalMinutes;
            }
         }
         catch { }
         //finally
         //{
         //   if (requery)
         //   {
         //      Execute_Query();
         //      requery = false;
         //   }
         //}
      }

      private void SaveStrtEndTime_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            if (aodt.STAT.In("002")) { MessageBox.Show("میز هایی که بسته و تسویه یا دفتری حساب کرده اند دیگر قادر به ویرایش نیستند"); Execute_Query(); return; }
            //if (aodt.STAT.In("003")) { MessageBox.Show("میز بسته شده دیگر قادر به ویرایش نیست"); return; }

            AodtBs1.EndEdit();

            iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, aodt.FIGH_FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC, aodt.ATTN_TYPE, aodt.PYDS_AMNT, aodt.DPST_AMNT, aodt.BCDS_CODE, aodt.GROP_APBS_CODE, aodt.EXPR_MINT_NUMB);
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

            PosAmnt_Txt.EditValue = Math.Abs((decimal)(aodt.TOTL_AMNT_DNRM - ((aodt.CASH_AMNT ?? 0) + (aodt.POS_AMNT ?? 0) + (aodt.PYDS_AMNT ?? 0) + (aodt.DPST_AMNT ?? 0))));

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

            AodtTrgtBs1.DataSource = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(a => a.RWNO != aodt.RWNO);

            // 1401/02/22 * Show Infomation on record
            AodtInfo_Lb.Text = "";
            if (aodt.GROP_APBS_CODE != null)
               AodtInfo_Lb.Text = "سانس ( " + aodt.App_Base_Define.TITL_DESC + " ) - ";
            AodtInfo_Lb.Text += string.Format("ساعت خروج : " + "{0:HH:mm}", aodt.STRT_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue)));
         }
         catch { }
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

            // check admin wallet
            var _admnwlet = iScsc.V_Admin_Wallets.FirstOrDefault(w => w.WLET_TYPE == "001");
            if (_admnwlet != null)
            {
               if (_admnwlet.AMNT_DNRM <= 10000)
               {
                  MessageBox.Show(this, "شارژ کیف پول شما تمام شده، لطفا جهت شارژ کیف پول اقدام کنید", "شارژ کیف پول", MessageBoxButtons.OK);
                  return;
               }
            }

            if (OpenOnSelfDate_Cbx.Checked && agop.FROM_DATE.Value.Date != DateTime.Now.Date && MessageBox.Show(this, "ایا میز مورد نظر در تاریخ دیگری می خواهید باز کنید", "باز شدن میز در تاریخ غیر از امروز", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (ExpnDesks_Lov.EditValue == null || ExpnDesks_Lov.EditValue.ToString() == "") { MessageBox.Show("میزی انتخاب نشده"); return; }
            var desk = Convert.ToInt64(ExpnDesks_Lov.EditValue);

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
            if (!OpenMoreItem_Cbx.Checked)
            {
               if (AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(d => d.Aggregation_Operation == agop && d.EXPN_CODE == desk && d.REC_STAT == "002" && d.STAT == "001"))
               {
                  MessageBox.Show("خطا - میزی که قصد باز کردن آن را دارید در حال حاضر باز می باشد، شما نمیتوانید دوباره همان میز را باز کنید");
                  return;
               }
            }

            if(TableCloseOpen)
            {
               // 1395/12/27 * میز هابه صورت پشت سر هم قرار میگیرند تا تسویه حساب شود
               var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;            
               iScsc.INS_AODT_P(agop.CODE, 1, aodt.AGOP_CODE, aodt.RWNO , fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null, null, null);
               
               // 1401/01/05 * برای رکورد هایی که به صورت وابسته باز میکنیم سوال میکنیم که رکورد پدر باید بسته شود یا باز بماند
               if(aodt.STAT == "001" && MessageBox.Show(this, "آیا رکورد فعلی متوقف شود؟", "توقف ساعت رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                  DeskClose_Butn_Click(aodt);
               Indpnd_Rb.Checked = true;
            }
            else
               iScsc.INS_AODT_P(agop.CODE, 1, null, null, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null, null, null);

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
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
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
      Data.Aggregation_Operation_Detail _crntiDesk;
      private void CloseOpenTable_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            apdt_gv.PostEditor();

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            // 1403/07/09
            if (sender is Data.Aggregation_Operation_Detail)
               aodt = sender as Data.Aggregation_Operation_Detail;

            // 1403/06/09 * اگر میز بسته شده باشد به هیچ عنوان نمیتوان هیچ عملیاتی روی آن اجرا کرد
            if (aodt.STAT == "002") return;

            // 1403/06/11 * Save Index in Memory
            _crntiDesk = aodt;

            TableCloseOpen = true;
            // 1403/07/09
            if(sender is Data.Aggregation_Operation_Detail)
               DeskClose_Butn_Click(aodt);
            else
               DeskClose_Butn_Click(aodt, null);

            ExpnDesks_Lov.EditValue = null;
            ExpnDesks_Lov.EditValue = aodt.EXPN_CODE;
            Figh_Lov.EditValue = null;
            //Figh_Lov.EditValue = aodt.FIGH_FILE_NO;
            // 1403/06/08
            //OpenDesk_Butn_Click(null, null);
            TableCloseOpen = false;
            _crntiDesk = null;
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
            if (!SaveUpPymt_Cbx.Checked)
            {
               if (amnt > (aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT))) { if (MessageBox.Show(this, "مبلغ کارتخوان شما از مبلغ کل هزینه میز بیشتر میباشد، ایا مایل به اصلاح قیمت میباشد؟", "مغایرت مالی در وصولی", MessageBoxButtons.YesNo) != DialogResult.Yes) return; else amnt = (long)(aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT)); }
            }

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
            if (agop == null) 
            { 
               TotlRcptAmnt_Txt.Text = TotlDebtAmnt_Txt.Text = TotlCashAmnt_Txt.Text = TotlPosAmnt_Txt.Text = "0";
               InCashBs.List.Clear();
               OutCashBs.List.Clear();
               CashInAmnt_Txt.Text = PosInAmnt_Txt.Text = SumInAmnt_Txt.Text = "0";
               CashOutAmnt_Txt.Text = PosOutAmnt_Txt.Text = SumOutAmnt_Txt.Text = "0";
               CrntCashAmnt_Txt.Text = CrntPosAmnt_Txt.Text = CrntSumAmnt_Txt.Text = CrntDsctAmnt_Txt.Text = "0";
               return; 
            }

            TotlDebtAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.TOTL_AMNT_DNRM).ToString();
            TotlRcptAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.POS_AMNT + d.CASH_AMNT).ToString();
            TotlCashAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.CASH_AMNT).ToString();
            TotlPosAmnt_Txt.Text = agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.POS_AMNT).ToString();
            TotlRemnAmnt_Txt.Text = (agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.TOTL_AMNT_DNRM) - agop.Aggregation_Operation_Details.Where(d => d.REC_STAT == "002").Sum(d => d.POS_AMNT + d.CASH_AMNT + d.PYDS_AMNT + d.DPST_AMNT)).ToString();

            InCashBs.DataSource = iScsc.Payment_Row_Types.Where(a => a.APDT_AGOP_CODE == agop.CODE && a.APDT_RWNO == null && a.AMNT > 0);
            OutCashBs.DataSource = iScsc.Payment_Row_Types.Where(a => a.APDT_AGOP_CODE == agop.CODE && a.APDT_RWNO == null && a.AMNT < 0);

            CashInAmnt_Txt.EditValue = InCashBs.List.OfType<Data.Payment_Row_Type>().Where(p => p.RCPT_MTOD == "001").Sum(p => p.AMNT);
            PosInAmnt_Txt.EditValue = InCashBs.List.OfType<Data.Payment_Row_Type>().Where(p => p.RCPT_MTOD == "003").Sum(p => p.AMNT);
            SumInAmnt_Txt.EditValue = InCashBs.List.OfType<Data.Payment_Row_Type>().Sum(p => p.AMNT);

            CashOutAmnt_Txt.EditValue = OutCashBs.List.OfType<Data.Payment_Row_Type>().Where(p => p.RCPT_MTOD == "001").Sum(p => p.AMNT);
            PosOutAmnt_Txt.EditValue = OutCashBs.List.OfType<Data.Payment_Row_Type>().Where(p => p.RCPT_MTOD == "003").Sum(p => p.AMNT);
            SumOutAmnt_Txt.EditValue = OutCashBs.List.OfType<Data.Payment_Row_Type>().Sum(p => p.AMNT);

            CrntCashAmnt_Txt.EditValue = Convert.ToInt64(TotlCashAmnt_Txt.EditValue) + Convert.ToInt64(CashInAmnt_Txt.EditValue) + Convert.ToInt64(CashOutAmnt_Txt.EditValue);
            CrntPosAmnt_Txt.EditValue = Convert.ToInt64(TotlPosAmnt_Txt.EditValue) + Convert.ToInt64(PosInAmnt_Txt.EditValue) + Convert.ToInt64(PosOutAmnt_Txt.EditValue);
            CrntSumAmnt_Txt.EditValue = Convert.ToInt64(CrntCashAmnt_Txt.EditValue) + Convert.ToInt64(CrntPosAmnt_Txt.EditValue);
            CrntDsctAmnt_Txt.EditValue = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Sum(p => p.PYDS_AMNT);
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
            if (!SaveUpPymt_Cbx.Checked)
            {
               if (amnt > (aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT))) { if (MessageBox.Show(this, "مبلغ نقدی شما از مبلغ کل هزینه میز بیشتر میباشد، ایا مایل به اصلاح قیمت میباشد؟", "مغایرت مالی در وصولی", MessageBoxButtons.YesNo) != DialogResult.Yes) return; else amnt = (long)(aodt.TOTL_AMNT_DNRM - (aodt.POS_AMNT + aodt.CASH_AMNT + aodt.PYDS_AMNT + aodt.DPST_AMNT)); }
            }

            aodt.CASH_AMNT += amnt;

            AodtBs1.EndEdit();

            iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, aodt.FIGH_FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC, aodt.ATTN_TYPE, aodt.PYDS_AMNT, aodt.DPST_AMNT, aodt.BCDS_CODE, aodt.GROP_APBS_CODE, aodt.EXPR_MINT_NUMB);
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
         else if(e.Button.Index == 6)
         {
            // Trun On Glob
            if (ExpnDesks_Lov.EditValue == null || ExpnDesks_Lov.EditValue.ToString() == "") { MessageBox.Show("میزی انتخاب نشده"); return; }
            var desk = Convert.ToInt64(ExpnDesks_Lov.EditValue); 
            
            _DefaultGateway.Gateway(
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
                                 "<Privilege>255</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    // ارسال پیام برای باز کردن دستگاه چراغ میز برای مشتری            
                                    _DefaultGateway.Gateway(
                                       new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
                                       {
                                          Input =
                                             new XElement("ExpenseGame",
                                                 new XAttribute("type", "expnextr"),
                                                 new XAttribute("expncode", desk),
                                                 new XAttribute("cmndtext", "st"),
                                                 new XAttribute("fngrprnt", "")
                                             )
                                       }
                                    );
                                 }
                              })
                           },
                           #endregion
                        }),
                  })
            );
         }
         else if(e.Button.Index == 7)
         {
            // Trun Off Glob
            if (ExpnDesks_Lov.EditValue == null || ExpnDesks_Lov.EditValue.ToString() == "") { MessageBox.Show("میزی انتخاب نشده"); return; }
            var desk = Convert.ToInt64(ExpnDesks_Lov.EditValue); 
                        
            _DefaultGateway.Gateway(
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
                                 "<Privilege>256</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    // ارسال پیام برای باز کردن دستگاه چراغ میز برای مشتری            
                                    _DefaultGateway.Gateway(
                                       new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
                                       {
                                          Input =
                                             new XElement("ExpenseGame",
                                                 new XAttribute("type", "expnextr"),
                                                 new XAttribute("expncode", desk),
                                                 new XAttribute("cmndtext", "sp"),
                                                 new XAttribute("fngrprnt", "")
                                             )
                                       }
                                    );
                                 }
                              })
                           },
                           #endregion
                        }),
                  })
            );
         }
      }

      private void ExpnDesk_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         // check admin wallet
         var _admnwlet = iScsc.V_Admin_Wallets.FirstOrDefault(w => w.WLET_TYPE == "001");
         if (_admnwlet != null)
         {
            if (_admnwlet.AMNT_DNRM <= 10000)
            {
               MessageBox.Show(this, "شارژ کیف پول شما تمام شده، لطفا جهت شارژ کیف پول اقدام کنید", "شارژ کیف پول", MessageBoxButtons.OK);
               return;
            }
         }

         if(e.NewValue != null && ExpnDesks_Lov.Properties.Buttons[1].Tag.ToString() == "auto")
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

               if (OpenOnSelfDate_Cbx.Checked && agop.FROM_DATE.Value.Date != DateTime.Now.Date && MessageBox.Show(this, "ایا میز مورد نظر در تاریخ دیگری می خواهید باز کنید", "باز شدن میز در تاریخ غیر از امروز", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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

               // 1399/11/25 * اگر میز باز باشد دیگر اجازه باز کردن مجدد آن را نداشته یاشیم
               if (!OpenMoreItem_Cbx.Checked)
               {
                  if (AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(d => d.Aggregation_Operation == agop && d.EXPN_CODE == desk && d.REC_STAT == "002" && d.STAT == "001"))
                  {
                     MessageBox.Show("خطا - میزی که قصد باز کردن آن را دارید در حال حاضر باز می باشد، شما نمیتوانید دوباره همان میز را باز کنید");
                     return;
                  }
               }

               if (TableCloseOpen)
               {
                  // 1395/12/27 * میز هابه صورت پشت سر هم قرار میگیرند تا تسویه حساب شود
                  //var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                  var aodt = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == _crntiDesk.AGOP_CODE && a.RWNO == _crntiDesk.RWNO);
                  iScsc.INS_AODT_P(agop.CODE, 1, aodt.AGOP_CODE, aodt.RWNO, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null, null, null);

                  // 1401/01/05 * برای رکورد هایی که به صورت وابسته باز میکنیم سوال میکنیم که رکورد پدر باید بسته شود یا باز بماند
                  if (aodt.STAT == "001" && MessageBox.Show(this, "آیا رکورد فعلی متوقف شود؟", "توقف ساعت رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                     DeskClose_Butn_Click(aodt);
                  Indpnd_Rb.Checked = true;

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
                  iScsc.INS_AODT_P(agop.CODE, 1, null, null, fileno, null, null, null, "002", "001", desk, null, null, null, null, null, null, null, null, null, null);

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
               Indpnd_Rb.Checked = true;
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

         try
         {
            // 1400/11/01
            DfltExpnPricTp_Txt.Tag = DfltExpnPricTp_Txt.EditValue;//ExpnDeskBs1.List.OfType<Data.Expense>().FirstOrDefault(ex => ex.CODE == Convert.ToInt64(e.NewValue)).PRIC;
            DfltExpnPricTp_Txt.EditValue = Convert.ToInt64(DfltExpnPricTp_Txt.Tag) * Convert.ToInt64(Cont_Txt.EditValue);
            UpdatePrice();
         }
         catch { DfltExpnPricTp_Txt.Tag = DfltExpnPricTp_Txt.EditValue = 0; }
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
               iScsc.UPD_AODT_P(aodt.AGOP_CODE, aodt.RWNO, aodt.AODT_AGOP_CODE, aodt.AODT_RWNO, fighs.FirstOrDefault().FILE_NO, aodt.RQST_RQID, aodt.ATTN_CODE, aodt.COCH_FILE_NO, aodt.REC_STAT, aodt.STAT, aodt.EXPN_CODE, aodt.MIN_MINT_STEP, aodt.STRT_TIME, aodt.END_TIME, aodt.EXPN_PRIC, aodt.EXPN_EXTR_PRCT, aodt.CUST_NAME, aodt.CELL_PHON, aodt.CASH_AMNT, aodt.POS_AMNT, aodt.NUMB, aodt.AODT_DESC, aodt.ATTN_TYPE, aodt.PYDS_AMNT, aodt.DPST_AMNT, aodt.BCDS_CODE, aodt.GROP_APBS_CODE, aodt.EXPR_MINT_NUMB);
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
            // 1401/02/22 * IF Recption is enter data for sell ticket system must be wait for Ideal
            if (TimrStat_PkBt.PickChecked) return;

            var crntdesk = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (crntdesk == null) return;

            // 1399/11/25 * برای گزینه های میز باز باید محاسبه مجدد در نظر گرفته شود
            var desks = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(d => d.STAT == "001" /* میز های باز نیاز به محاسبه مجدد دارند */);
            // اگر میز بازی وجود نداشته باشد
            if (desks.Count() == 0) return;
            
            int value = 100 / desks.Count();
            RecalcDesk_Pgb.Visible = true;
            RecalcDesk_Pgb.Value = 0;

            List<string> evntLogs = new List<string>();

            foreach (var desk in desks)
            {
               // 1403/06/11 * IF Records for calc
               //AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == desk.AGOP_CODE && d.RWNO == desk.RWNO));
               CalcDesk_Butn_Click(desk, null);
               //CalcDesk_Butn_Click(desk);
               RecalcDesk_Pgb.Value += value;

               if(PlaySondAlrm_Cbx.Checked)
               {
                  if (evntLogs.Count == 0)
                     evntLogs.Add(DateTime.Now.ToString("HH:mm:ss =>"));
                  
                  // 1403/06/11 * Update and select correct record
                  crntdesk = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == desk.AGOP_CODE && a.RWNO == desk.RWNO);//AodtBs1.Current as Data.Aggregation_Operation_Detail;

                  if ((crntdesk.CASH_AMNT + crntdesk.POS_AMNT + crntdesk.PYDS_AMNT + crntdesk.DPST_AMNT) > 0 && (crntdesk.TOTL_AMNT_DNRM - (crntdesk.CASH_AMNT + crntdesk.POS_AMNT + crntdesk.PYDS_AMNT + crntdesk.DPST_AMNT) >= 0))
                  {
                     if (evntLogs.Count == 1)
                     {
                        new Thread(AlarmShow).Start();
                     }
                     evntLogs.Add(string.Format("{{ ({1}) : \"{0}\" - [ {2} ] }}", crntdesk.CUST_NAME, crntdesk.RWNO, crntdesk.NUMB));

                     if(AutoClos_Cbx.Checked)
                     {
                        DeskClose_Butn_Click(crntdesk, null);
                        //DeskClose_Butn_Click(desk);
                        FillEndTime_Butn_Click(crntdesk, null);

                        if(FinlRec_Cbx.Checked)
                        {
                           RecStat_Butn_ButtonClick(crntdesk, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(RecStat_Butn.Buttons[3]));
                        }
                     }
                  }

                  // 1403/06/11 * اگر تایم بازی برای مشتری که درخواست کرده تا فلان تایم تمام شده باشد بخواهیم 
                  if(crntdesk.EXPR_MINT_NUMB > 0 && crntdesk.STRT_TIME.Value.AddMinutes((double)crntdesk.EXPR_MINT_NUMB) <= DateTime.Now)
                  {
                     if (evntLogs.Count == 1)
                     {
                        new Thread(AlarmShow).Start();
                     }
                     evntLogs.Add(string.Format("{{ \"{4}\" ({1}) : \"{0}\" - {5}: [ {2:n0} {7} ] - {6}: [ {3:n0} {7} ] }}", crntdesk.Expense.EXPN_DESC, crntdesk.RWNO, crntdesk.EXPR_MINT_NUMB, crntdesk.END_TIME.Value.Subtract(crntdesk.STRT_TIME.Value).TotalMinutes, "هشدار یادآوری", "درخواست مدت زمان", "مدت زمانی که گذشته", "دقیقه"));
                  }
               }
            }

            RecalcDesk_Pgb.Value = 100;
            AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == crntdesk.AGOP_CODE && d.RWNO == crntdesk.RWNO));
            
            if (evntLogs.Count() > 1)
            {
               Evnt_Lbx.Items.Insert(0, Evnt_Lbx.Items.Count.ToString() + " ) " + string.Join(",", evntLogs).Replace("=>,", "=> "));
               MenuCtrl_Tc.SelectedTab = tabPage7;
            }

            requery = true;
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
                     //AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a == desk));
                     //DeskClose_Butn_Click(null, null);
                     DeskClose_Butn_Click(desk);
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

            if (requery)
               Execute_Query();
         }
      }

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      private void AlarmShow()
      {
         if (InvokeRequired)
         {
            try
            {
               wplayer.URL = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
               wplayer.controls.play();
            }
            catch { }

            var tempcolor = BackGrnd_Butn.NormalColorA;
            for (int i = 0; i < 5; i++)
            {
               if (i % 2 == 0)
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.YellowGreen;
               else
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.LimeGreen;

               Thread.Sleep(100);
            }
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = tempcolor;
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
               //AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(d => d.AGOP_CODE == desk.AGOP_CODE && d.RWNO == desk.RWNO));
               DeskClose_Butn_Click(desk, null);
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

            ExpnDesks_Lov.EditValue = null;
            //if (ExpnDeskBs1.List.Count > 0)
            //{
            //   ExpnDeskBs1.MoveFirst();
            //   ExpnDesk_GridLookUpEdit.EditValue = (ExpnDeskBs1.Current as Data.Expense).CODE;
            //}
            if(ExpnDeskBs1.List.Count == 1)
            {
               ExpnDeskBs1.MoveFirst();
               ExpnDesks_Lov.EditValue = (ExpnDeskBs1.Current as Data.Expense).CODE;
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

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            FngrPrnt_Txt.EditValue =
                iScsc.Fighters
                .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                .Select(f => f.FNGR_PRNT_DNRM)
                .ToList()
                .Where(f => f.All(char.IsDigit))
                .Max(f => Convert.ToInt64(f)) + 1;
         }
         catch
         {
            FngrPrnt_Txt.EditValue = 1;
         }
      }

      private void CellPhonFind_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _serv = FighBs.List.OfType<Data.Fighter>().Where(f => f.CELL_PHON_DNRM == CELL_PHON_TextEdit.Text);

            if (_serv.Count() == 1)
               Figh_Lov.EditValue = _serv.FirstOrDefault().FILE_NO;
            else
            {
               Figh_Lov.Focus();
               Figh_Gv.ActiveFilterString = string.Format("CELL_PHON_DNRM LIKE '%{0}%'", CELL_PHON_TextEdit.Text);
               Figh_Lov.ShowPopup();               
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FngrPrntFind_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _serv = FighBs.List.OfType<Data.Fighter>().Where(f => f.FNGR_PRNT_DNRM == FngrPrnt_Txt.Text);

            if (_serv.Count() == 1)
               Figh_Lov.EditValue = _serv.FirstOrDefault().FILE_NO;
            else
            {
               Figh_Lov.Focus();
               Figh_Gv.ActiveFilterString = string.Format("FNGR_PRNT_DNRM LIKE '{0}'", FngrPrnt_Txt.Text);
               Figh_Lov.ShowPopup();
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveServ_Butn_Click(object sender, EventArgs e)
      {
         #region Check Validation
         try
         {
            var _serv = FighBs.List.OfType<Data.Fighter>().Where(f => f.CELL_PHON_DNRM == CELL_PHON_TextEdit.Text);

            if (_serv.Count() == 1)
            {
               Figh_Lov.EditValue = _serv.FirstOrDefault().FILE_NO;
               Figh_Lov.Focus();
               return;
            }
            else
            {
               Figh_Lov.Focus();
               Figh_Gv.ActiveFilterString = string.Format("CELL_PHON_DNRM LIKE '%{0}%'", CELL_PHON_TextEdit.Text);
               Figh_Lov.ShowPopup();               
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         try
         {
            var _serv = FighBs.List.OfType<Data.Fighter>().Where(f => f.FNGR_PRNT_DNRM == FngrPrnt_Txt.Text);

            if (_serv.Count() == 1)
            {
               Figh_Lov.EditValue = _serv.FirstOrDefault().FILE_NO;
               Figh_Lov.Focus();
               return;
            }
            else
            {
               Figh_Lov.Focus();
               Figh_Gv.ActiveFilterString = string.Format("FNGR_PRNT_DNRM LIKE '{0}'", FngrPrnt_Txt.Text);
               Figh_Lov.ShowPopup();               
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         #endregion

         #region Save Record in Step One
         try
         {
            if (FngrPrnt_Txt.Text == "")
            {
               if (MessageBox.Show(this, "کد شناسایی خالی میباشد آیا مایل به ایجاد کد پیش فرض هستید؟", "هشدار کد شناسایی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                  MaxF_Butn001_Click(null, null);
               else
               {
                  FngrPrnt_Txt.Focus();
                  return;
               }
            }

            #region Save Request
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
                        new XElement("Frst_Name", FRST_NAME_TextEdit.Text),
                        new XElement("Last_Name", LAST_NAME_TextEdit.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue),
                        new XElement("Natl_Code", ""),
                        new XElement("Brth_Date", ""),
                        new XElement("Cell_Phon", CELL_PHON_TextEdit.Text),
                        new XElement("Tell_Phon", ""),
                        new XElement("Type", "001"),
                        new XElement("Post_Adrs", ""),
                        new XElement("Emal_Adrs", ""),
                        new XElement("Insr_Numb", ""),
                        new XElement("Insr_Date", ""),
                        new XElement("Educ_Deg", ""),
                        new XElement("Club_Code", Club_CodeLookUpEdit.EditValue ?? ""),
                        new XElement("Dise_Code", ""),
                        new XElement("Blod_Grop", ""),
                        new XElement("Fngr_Prnt", FngrPrnt_Txt.EditValue ?? ""),
                        new XElement("Sunt_Bunt_Dept_Orgn_Code", ""),
                        new XElement("Sunt_Bunt_Dept_Code", ""),
                        new XElement("Sunt_Bunt_Code", ""),
                        new XElement("Sunt_Code", ""),
                        new XElement("Cord_X", ""),
                        new XElement("Cord_Y", ""),
                        new XElement("Mtod_Code", ""),
                        new XElement("Ctgy_Code", ""),
                        new XElement("Most_Debt_Clng", ""),
                        new XElement("Serv_No", ""),
                        new XElement("Chat_Id", "")
                     ),
                     new XElement("Member_Ship",
                        new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
                     )
                  )
               )
            );
            #endregion

            #region Get Request Data
            var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", CurrentUser)))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst = iScsc.Requests.Where(r => Rqids.Contains(r.RQID) && r.Request_Rows.Any(rr => rr.Fighter_Publics.Any(fp => fp.CELL_PHON == CELL_PHON_TextEdit.Text && fp.FNGR_PRNT == FngrPrnt_Txt.Text))).FirstOrDefault();
            #endregion

            #region Final Request
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
            #endregion
            requery = true;

            FRST_NAME_TextEdit.Text = LAST_NAME_TextEdit.Text = CELL_PHON_TextEdit.Text = FngrPrnt_Txt.Text = "";
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();

         }
         #endregion         
      }

      private void ClerForm_Butn_Click(object sender, EventArgs e)
      {
         FRST_NAME_TextEdit.Text = LAST_NAME_TextEdit.Text = CELL_PHON_TextEdit.Text = FngrPrnt_Txt.Text = "";
         FRST_NAME_TextEdit.Focus();
      }

      private void AutoRecalc_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         //AutoRecalc_Tsmi.Checked = AutoRecalc_Cbx.Checked;         
         //IntervalRecalc_Txt.Enabled = AutoRecalc_Cbx.Checked;
         AutoRecalc_Tsmi_Click(null, null);
      }

      private void IntervalRecalc_Txt_TextChanged(object sender, EventArgs e)
      {
         IntervalRecalc_Tsmi.Text = IntervalRecalc_Txt.Text;
      }

      private void DresNumbFind_Txt_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            if (DratBs.List.OfType<Data.Dresser_Attendance>().Any(d => d.CODE == 0)) return;

            var drat = DratBs.AddNew() as Data.Dresser_Attendance;
            drat.Aggregation_Operation_Detail = aodt;
            drat.ATTN_TYPE = "001";

            iScsc.Dresser_Attendances.InsertOnSubmit(drat);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var drat = DratBs.Current as Data.Dresser_Attendance;
            if (drat == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Dresser_Attendances.DeleteOnSubmit(drat);
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

      private void SaveDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Drat_Gv.PostEditor();

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

      private void SaveNewRec_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            var lastaodt = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().OrderByDescending(a => a.RWNO).FirstOrDefault();
            
            // Check Payment Method With Payment Amount
            if (PayByPymt_Cbx.Checked && Convert.ToInt64(RmndAmntTp_Txt.EditValue) != 0)
            {
               MessageBox.Show(this, "مبلغ تسویه حساب شما از مبلغ فاکتور مغایرت دارد", "خطا پرداخت", MessageBoxButtons.OK);
               return;
            }

            #region 1st Step Find Guest
            var _guest = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005");
            if(_guest == null)
            {
               MessageBox.Show(this, "برای سیستم شما مهمان آزاد تعریف نشده، لطفا مهمان آزاد را تعریف کنید.");
               return;
            }

            Figh_Lov.EditValue = _guest.FILE_NO;
            #endregion

            // Call Open Desk
            OpenDesk_Butn_Click(null, null);

            // Get Last Record
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            if(lastaodt != null)
               if (lastaodt.RWNO == aodt.RWNO)
                  return;

            aodt.CUST_NAME = CustName_Txt.Text;
            aodt.NUMB = (int)Cont_Txt.Value;
            aodt.PYDS_AMNT = Convert.ToInt64(PydsAmntTp_Txt.EditValue);

            // مشخص کردن سانس برای مشتریان
            if (AutoSunsSlct_Cbx.Checked)
            {
               var _suns = SunsBs.List.OfType<Data.App_Base_Define>().Where(a => DateTime.Now.TimeOfDay >= a.PRT1_TIME.Value.TimeOfDay && DateTime.Now.TimeOfDay <= a.PRT3_TIME.Value.TimeOfDay).OrderBy(a => a.PRT1_TIME).FirstOrDefault();
               if (_suns != null)
               {
                  SlctSuns_Lov.EditValue = _suns.CODE;
                  SunsBs.Position = SunsBs.IndexOf(_suns);
               }
               else
               {
                  _suns = SunsBs.List.OfType<Data.App_Base_Define>().Where(a => a.PRT2_TIME.Value.TimeOfDay <= DateTime.Now.TimeOfDay && a.PRT6_TIME.Value.TimeOfDay >= DateTime.Now.TimeOfDay).FirstOrDefault();
                  if (_suns != null)
                  {
                     SlctSuns_Lov.EditValue = _suns.CODE;
                     SunsBs.Position = SunsBs.IndexOf(_suns);
                  }
                  else
                     SlctSuns_Lov.EditValue = null;
               }
            }

            // Suns Information
            var _sunsInfo = SunsBs.Current as Data.App_Base_Define;
            if (_sunsInfo == null)
            {
               SunsInfo_Lb.Text = "";
               return;
            }

            SunsInfo_Lb.Text = string.Format("زمان باقیمانده : " + "{0}" + " دقیقه" + ", ظرفیت باقیمانده ورودی سانس : " + "{1}" + " نفر", (int)((_sunsInfo.PRT5_TIME.Value.TimeOfDay - DateTime.Now.TimeOfDay).TotalMinutes), (_sunsInfo.NUMB - AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(a => a.GROP_APBS_CODE == _sunsInfo.CODE).Count()));

            // مشخص کردن سانس برای رکورد
            aodt.GROP_APBS_CODE = (long?)SlctSuns_Lov.EditValue;

            SaveStrtEndTime_Butn_Click(null, null);

            // Save Pos Amnt
            if(PosAmntTp_Txt.Text != "" && Convert.ToInt64(PosAmntTp_Txt.EditValue) != 0)
            {
               PosAmnt_Txt.EditValue = PosAmntTp_Txt.EditValue;
               Pos_Butn_Click(null, null);
            }

            // Save Cash Amnt
            if(CashAmntTp_Txt.Text != "" && Convert.ToInt64(CashAmntTp_Txt.EditValue) != 0)
            {
               PosAmnt_Txt.EditValue = CashAmntTp_Txt.EditValue;
               Cash_Butn_Click(null, null);
            }

            if (DresItem_Lbx.Items.Count > 0)
            {
               string _cmnd = "INSERT INTO dbo.Dresser_Attendance (AODT_AGOP_CODE, AODT_RWNO, CODE, DERS_NUMB) VALUES ";
               // Save Dresses Number
               int i = 0;
               foreach (var item in DresItem_Lbx.Items)
               {
                  _cmnd += string.Format("({0}, {1}, {2}, {3}),", aodt.AGOP_CODE, aodt.RWNO, ++i, item);
               }
               _cmnd += ";";
               _cmnd = _cmnd.Replace(",;", ";");

               iScsc.ExecuteCommand(_cmnd);
            }

            CustName_Txt.Text = "";
            PosAmntTp_Txt.EditValue = CashAmntTp_Txt.EditValue = PydsAmntTp_Txt.EditValue = null;
            DresItem_Lbx.Items.Clear();
            Cont_Txt.Value = 1;
            CustName_Txt.Focus();
            TimrStat_PkBt.PickChecked = false;
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

      private void AddDresItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            if (DresNum_Txt.EditValue == null || DresNum_Txt.Text == "") return;
            if (DresItem_Lbx.Items.Contains(DresNum_Txt.Text)) return;

            // Checked Not Exists For another members
            if(agop.Aggregation_Operation_Details.Any(a => a.REC_STAT == "002" && a.STAT.In("001") && a.Dresser_Attendances.Any(da => da.DERS_NUMB.ToString() == DresNum_Txt.Text)))
            {
               if (MessageBox.Show(this, "این شماره کمد در اختیار فرد دیگری میباشد، آیا مطمئن هستید؟", "شماره کمد تکراری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) { DresNum_Txt.Focus(); return; }
            }

            DresItem_Lbx.Items.Add(DresNum_Txt.Text);

            DresNum_Txt.Text = "";
            DresNum_Txt.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelDresItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (DresItem_Lbx.Items.Count == 0) return;
            if (DresItem_Lbx.SelectedIndex == -1) DresItem_Lbx.SelectedIndex = 0;

            DresItem_Lbx.Items.RemoveAt(DresItem_Lbx.SelectedIndex);

            DresNum_Txt.Text = "";
            DresNum_Txt.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FindDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.REC_STAT == "002" && a.STAT.In("001", "003") && a.Dresser_Attendances.Any(da => da.DERS_NUMB.ToString() == DresNum_Txt.Text));
            if (aodt == null) return;

            AodtBs1.Position = AodtBs1.IndexOf(aodt);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ColxVisible_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         apdt_gv.Columns.OfType<DevExpress.XtraGrid.Columns.GridColumn>().Where(gc => gc.Tag == ((CheckBox)sender).Tag).ToList().ForEach(gc => gc.Visible = ((CheckBox)sender).Checked);
      }

      private void Cancel_Butn_Click(object sender, EventArgs e)
      {
         CustName_Txt.Text = "";
         PosAmntTp_Txt.EditValue = CashAmntTp_Txt.EditValue = PydsAmntTp_Txt.EditValue = null;
         DresItem_Lbx.Items.Clear();
         Cont_Txt.Value = 1;
         CustName_Txt.Focus();
      }

      private void DelEvntLogs_Butn_Click(object sender, EventArgs e)
      {
         Evnt_Lbx.Items.Clear();
      }

      private void FillEndTime_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            // 1403/06/11
            if (sender is Data.Aggregation_Operation_Detail)
               aodt = sender as Data.Aggregation_Operation_Detail;

            aodt.END_TIME = aodt.STRT_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));

            var val = ChckCalcEndTime_Cbx.Checked;
            ChckCalcEndTime_Cbx.Checked = false;

            CalcDesk_Butn_Click(null, null);
            
            ChckCalcEndTime_Cbx.Checked = val;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowSmplGrid_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         rollout4.Controls.OfType<CheckBox>().Where(c => c.Tag != null && c.Tag.In("1", "2", "3", "12")).OrderBy(c => c.Tag).ToList().ForEach(c => c.Checked = !ShowSmplGrid_Cbx.Checked);
      }

      private void Cont_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            DfltExpnPricTp_Txt.EditValue = Convert.ToInt64(DfltExpnPricTp_Txt.Tag) * Convert.ToInt64(e.NewValue);
         }
         catch { }
      }

      private void UpdatePrice()
      {
         try
         {
            if (PosAmntTp_Txt.EditValue == null || PosAmntTp_Txt.EditValue.ToString() == "") { PosAmntTp_Txt.EditValue = 0; }
            if (CashAmntTp_Txt.EditValue == null || CashAmntTp_Txt.EditValue.ToString() == "") { CashAmntTp_Txt.EditValue = 0; }
            if (PydsAmntTp_Txt.EditValue == null || PydsAmntTp_Txt.EditValue.ToString() == "") { PydsAmntTp_Txt.EditValue = 0; }
            RmndAmntTp_Txt.EditValue = (Convert.ToInt64(DfltExpnPricTp_Txt.Tag) * Cont_Txt.Value) - (Convert.ToInt64(PosAmntTp_Txt.EditValue) + Convert.ToInt64(CashAmntTp_Txt.EditValue) + Convert.ToInt64(PydsAmntTp_Txt.EditValue));
         }
         catch { }
      }

      private void xAmntTp_Txt_TextChanged(object sender, EventArgs e)
      {
         UpdatePrice();
      }

      private void xAmntInCash_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            if (InCashType_Lov.EditValue == null || InCashType_Lov.EditValue.ToString() == "" || Convert.ToInt64(InCashType_Lov.EditValue) == 0) { InCashType_Lov.Focus(); return; }
            if (AmntInCash_Txt.EditValue == null || AmntInCash_Txt.EditValue.ToString() == "" || Convert.ToInt64(AmntInCash_Txt.EditValue) == 0) { AmntInCash_Txt.Focus(); return; }            

            iScsc.ExecuteCommand("INSERT INTO dbo.Payment_Row_Type(Apdt_Agop_Code, Code, Amnt, Rcpt_Mtod, Bank, Amnt_Type_Code) VALUES({0}, 0, {1}, {2}, {3}, {4});", agop.CODE, AmntInCash_Txt.EditValue, ((SimpleButton)sender).Tag, DescInCash_Txt.EditValue, InCashType_Lov.EditValue);
            AmntInCash_Txt.EditValue = DescInCash_Txt.EditValue = "";
            InCashType_Lov.EditValue = null;
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

      private void xAmntOutCash_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            if (OutCashType_Lov.EditValue == null || OutCashType_Lov.EditValue.ToString() == "" || Convert.ToInt64(OutCashType_Lov.EditValue) == 0) { OutCashType_Lov.Focus(); return; }
            if (AmntOutCash_Txt.EditValue == null || AmntOutCash_Txt.EditValue.ToString() == "" || Convert.ToInt64(AmntOutCash_Txt.EditValue) == 0) { AmntOutCash_Txt.Focus(); return; }

            iScsc.ExecuteCommand("INSERT INTO dbo.Payment_Row_Type(Apdt_Agop_Code, Code, Amnt, Rcpt_Mtod, Bank, Amnt_Type_Code) VALUES({0}, 0, {1}, {2}, {3}, {4});", agop.CODE, Convert.ToInt64(AmntOutCash_Txt.EditValue) * -1, ((SimpleButton)sender).Tag, DescOutCash_Txt.EditValue, OutCashType_Lov.EditValue);
            AmntOutCash_Txt.EditValue = DescOutCash_Txt.EditValue = "";
            OutCashType_Lov.EditValue = null;
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

      private void CashInActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            // 1401/01/18 - اگر فرم در وضعیت ثبت اطلاعات نباشد نبایستی  عملیاتی انجام شود
            if (!SaveInfoStat_Rb.Checked) return;

            var pymt = InCashBs.Current as Data.Payment_Row_Type;
            if (pymt == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Payment_Row_Types.DeleteOnSubmit(pymt);
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

      private void CashOutActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            // 1401/01/18 - اگر فرم در وضعیت ثبت اطلاعات نباشد نبایستی  عملیاتی انجام شود
            if (!SaveInfoStat_Rb.Checked) return;

            var pymt = OutCashBs.Current as Data.Payment_Row_Type;
            if (pymt == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Payment_Row_Types.DeleteOnSubmit(pymt);
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

      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var agop = AgopBs1.Current as Data.Aggregation_Operation;
            if (agop == null) return;

            if (NoteBs.List.OfType<Data.Note>().Any(n => n.CODE == 0)) return;

            var note = NoteBs.AddNew() as Data.Note;
            note.Aggregation_Operation = agop;

            iScsc.Notes.InsertOnSubmit(note);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DeleteNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var note = NoteBs.Current as Data.Note;
            if (note == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Notes.DeleteOnSubmit(note);

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

      private void SaveNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            NoteGv.PostEditor();

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

      private void SaveCustTell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long? fileno = null, rqid = null;
            string cntycode, prvncode, regncode;
            var _conts = FgpbCellBs.List.OfType<Data.Fighter_Public>().FirstOrDefault();
            if (_conts == null)
            {
               var _gust = FighBs.OfType<Data.Fighter>().FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005");
               if (_gust == null)
                  throw new Exception("لطفا مشتری آزادی را درون سیستم تعریف کنید");
               else
               {
                  fileno = _gust.FILE_NO;
                  cntycode = _gust.REGN_PRVN_CNTY_CODE;
                  prvncode = _gust.REGN_PRVN_CODE;
                  regncode = _gust.REGN_CODE;
                  rqid = iScsc.VF_Request_Changing(fileno).Where(r => r.RQTP_CODE == "001" || r.RQTP_CODE == "025").FirstOrDefault().RQID;
               }
            }
            else
            {
               fileno = _conts.FIGH_FILE_NO;
               cntycode = _conts.REGN_PRVN_CNTY_CODE;
               prvncode = _conts.REGN_PRVN_CODE;
               regncode = _conts.REGN_CODE;
               rqid = _conts.RQRO_RQST_RQID;

            }

            // 1401/01/03 * Check NOT EXISTS Member in Contacts List
            if (CellPhon_Txt.Text == null || CellPhon_Txt.Text == "" || !CellPhon_Txt.Text.Length.IsBetween(10, 11)) { CellPhon_Txt.Focus(); return; }
            var _cont = FgpbCellBs.List.OfType<Data.Fighter_Public>().Any(fp => fp.CELL_PHON == CellPhon_Txt.Text);
            if(!_cont)
            {
               // Save Contact in Database
               iScsc.ExecuteCommand(string.Format("INSERT INTO dbo.Fighter_Public(Regn_Prvn_Cnty_Code, Regn_Prvn_Code, Regn_Code, Rqro_Rqst_Rqid, Rqro_Rwno, Figh_File_No, Rect_Code, Frst_Name, Last_Name, Cell_Phon) VALUES('{0}', '{1}', '{2}', {3}, 1, {4}, '002', N'{5}', N'{6}', '{7}');",cntycode, prvncode, regncode, rqid, fileno, FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text));
               FrstName_Txt.Text = LastName_Txt.Text = CellPhon_Txt.Text = "";
               FrstName_Txt.Focus();
               requery = true;
            }
            else
            {
               // Focus in list
               FgpbCellBs.Position = FgpbCellBs.IndexOf(FgpbCellBs.List.OfType<Data.Fighter_Public>().FirstOrDefault(fp => fp.CELL_PHON == CellPhon_Txt.Text));
               FrstName_Txt.Text = LastName_Txt.Text = CellPhon_Txt.Text = "";
               //MessageBox.Show(this, "اطلاعات درون سیستم وجود دارد لطفا بررسی بفرمایید");
            }
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

      private void CnclCustTell_Butn_Click(object sender, EventArgs e)
      {
         FrstName_Txt.Text = LastName_Txt.Text = CellPhon_Txt.Text = "";
         FrstName_Txt.Focus();
      }

      private void SearchCustTell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(CrntDateCustTell_Rb.Checked)
               FgpbCellBs.DataSource = iScsc.Fighter_Publics.Where(fb => fb.RECT_CODE == "002" && fb.CRET_DATE.Value.Date == DateTime.Now.Date);
            else if(AllDateCustTell_Rb.Checked)
               FgpbCellBs.DataSource = iScsc.Fighter_Publics.Where(fb => fb.RECT_CODE == "002");
            else if (SetDateCustTell_Rb.Checked)
            {
               if (SetDateCustTell_Dt.Value.HasValue)
                  FgpbCellBs.DataSource = iScsc.Fighter_Publics.Where(fb => fb.RECT_CODE == "002" && fb.CRET_DATE.Value.Date == SetDateCustTell_Dt.Value.Value.Date);
               else
               {
                  SetDateCustTell_Dt.Focus();
                  return;
               }
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DfltExpnPricTp_Txt_TextChanged(object sender, EventArgs e)
      {
         //DfltExpnPricTp_Txt.Tag = DfltExpnPricTp_Txt.Text.Replace(",", "");
      }

      private void GotoCustTell_Butn_Click(object sender, EventArgs e)
      {
         MenuCtrl_Tc.SelectedTab = tabPage10;
         FrstName_Txt.Focus();
      }

      private void UpdtCustTell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FgpbCustTell_Gv.PostEditor();

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

      private void FrstPric_Txt_TextChanged(object sender, EventArgs e)
      {
         DfltExpnPricTp_Txt.Tag = FrstPric_Txt.Text.Replace(",", "");
         //Cont_Txt_EditValueChanging(null, null);
         UpdatePrice();
      }

      private void InCashType_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Buffe_InCash"),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
                  );
                  break;
               case 2:
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OutCashType_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Buffe_OutCash"),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
                  );
                  break;
               case 2:
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CalcTreeDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            // 1403/07/08 * Find focused record;
            if (sender is Data.Aggregation_Operation_Detail)
               aodt = sender as Data.Aggregation_Operation_Detail;

            int crntPos = AodtBs1.Position;

            // Find root records
            while(true)
            {
               if (aodt.Aggregation_Operation_Detail1 != null)
                  aodt = aodt.Aggregation_Operation_Detail1;
               else
                  break;
            }

            long dynamnt = 0, fixamnt = 0;
            long cashamnt = 0, posamnt = 0, dsctamnt = 0;
            int cont = 0;
            ShowRsltCalcTreeDesk_Txt.Text = "محاسبه صورتحساب";

            _listAodts.Clear();
            TreeScroll(aodt);

            // Calculate Dynamic and Fix Amount
            foreach (var desk in _listAodts)
            {
               if(desk.STAT == "001")
                  CalcDesk_Butn_Click(desk);

               iScsc = new Data.iScscDataContext(ConnectionString);
               aodt = iScsc.Aggregation_Operation_Details.FirstOrDefault(a => a.AGOP_CODE == desk.AGOP_CODE && a.RWNO == desk.RWNO);
               dynamnt += (long)((aodt.EXPN_PRIC ?? 0) * aodt.NUMB);
               fixamnt += aodt.TOTL_BUFE_AMNT_DNRM ?? 0;
               // 1403/06/09
               cashamnt += aodt.CASH_AMNT ?? 0;
               posamnt += aodt.POS_AMNT ?? 0;
               dsctamnt += aodt.PYDS_AMNT ?? 0;
               ++cont;

               ShowRsltCalcTreeDesk_Txt.Text += Environment.NewLine +
                  string.Format("{0} ) " + "هزینه محاسبه شده : " + "{1:n0}   ***   هزینه ثابت : " + "{2:n0}", aodt.RWNO, ((aodt.EXPN_PRIC ?? 0) * aodt.NUMB), aodt.TOTL_BUFE_AMNT_DNRM);
            }

            ShowRsltCalcTreeDesk_Txt.Text += Environment.NewLine + "-----------------------------------------------------";

            ShowRsltCalcTreeDesk_Txt.Text += Environment.NewLine +
                  string.Format("تعداد کل ردیف ها : " + "{0} عدد" , cont) + Environment.NewLine +  
                  string.Format("جمع هزینه محاسبه شده : " + "{0:n0}   ***   جمع هزینه ثابت : " + "{1:n0}", dynamnt, fixamnt) + Environment.NewLine + 
                  string.Format("جمع کل هزینه : " + "{0:n0}", dynamnt + fixamnt) + Environment.NewLine +
                  string.Format("تخفیف ها : " + "{0:n0}", dsctamnt) + Environment.NewLine + 
                  string.Format("پرداختی ها : نقدی : " + "{0:n0}" + " کارتخوان : " + "{1:n0}", cashamnt, posamnt);

            TotlDeskDynmExpnAmnt_Txt.EditValue = dynamnt;
            TotlDeskFixExpnAmnt_Txt.EditValue = fixamnt;

            // 1403/06/08
            PayCashAmnt_Txt.EditValue = PayPosAmnt_Txt.EditValue = DsctAmnt_Txt.EditValue = 0;

            PayRmndAmnt_Txt.EditValue = dynamnt + fixamnt - (cashamnt + posamnt + dsctamnt);//(Convert.ToInt64(PayCashAmnt_Txt.EditValue) + Convert.ToInt64(PayPosAmnt_Txt.EditValue) + Convert.ToInt64(DsctAmnt_Txt.EditValue)); ;

            

            AodtBs1.Position = crntPos;
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

      private void PayxAmnt_Txt_TextChanged(object sender, EventArgs e)
      {
         try
         {
            PayRmndAmnt_Txt.EditValue = (Convert.ToInt64(TotlDeskDynmExpnAmnt_Txt.EditValue) + Convert.ToInt64(TotlDeskFixExpnAmnt_Txt.EditValue)) - (Convert.ToInt64(PayCashAmnt_Txt.EditValue) + Convert.ToInt64(PayPosAmnt_Txt.EditValue) + Convert.ToInt64(DsctAmnt_Txt.EditValue));
         }
         catch { }
      }

      private void SaveTotlDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            // 1403/07/08 * Ending and close all records
            if (sender is Data.Aggregation_Operation_Detail)
               aodt = sender as Data.Aggregation_Operation_Detail;

            int crntPos = AodtBs1.Position;

            // Find root records
            while (true)
            {
               if (aodt.Aggregation_Operation_Detail1 != null)
                  aodt = aodt.Aggregation_Operation_Detail1;
               else
                  break;
            }

            long? totlDebt = Convert.ToInt64(TotlDeskDynmExpnAmnt_Txt.EditValue) + Convert.ToInt64(TotlDeskFixExpnAmnt_Txt.EditValue);
            long? cashAmnt = Convert.ToInt64(PayCashAmnt_Txt.EditValue), posAmnt = Convert.ToInt64(PayPosAmnt_Txt.EditValue), dsctAmnt = Convert.ToInt64(DsctAmnt_Txt.EditValue);

            _listAodts.Clear();
            TreeScroll(aodt);

            // Calculate Dynamic and Fix Amount
            foreach (var desk in _listAodts)
            {
               // AodtBs1.Position = AodtBs1.IndexOf(recd);
               // IF Exists Open Desk All Operation Must Rollback
               if (desk.STAT == "001")
               {
                  throw new Exception("رکورد باز درون لیست شما وجود دارد لطفا دکمه بستن میز را بزنید");
               }               

               // اگر رکورد تخفیف داشته باشد
               if(dsctAmnt > 0)
               {
                  desk.PYDS_AMNT = desk.TOTL_AMNT_DNRM >= dsctAmnt ? dsctAmnt : desk.TOTL_AMNT_DNRM;
                  dsctAmnt -= (desk.PYDS_AMNT ?? 0);
               }

               var rmndAmnt = desk.TOTL_AMNT_DNRM - (desk.PYDS_AMNT ?? 0);
               if(rmndAmnt > 0)
               {
                  if(cashAmnt > 0)
                  {
                     desk.CASH_AMNT = rmndAmnt >= cashAmnt ? cashAmnt : rmndAmnt;
                     cashAmnt -= (desk.CASH_AMNT ?? 0);
                  }
               }

               rmndAmnt = desk.TOTL_AMNT_DNRM - ((desk.CASH_AMNT ?? 0) + (desk.POS_AMNT ?? 0) + (desk.PYDS_AMNT ?? 0));
               if(rmndAmnt > 0)
               {
                  if(posAmnt > 0)
                  {
                     var POS = rmndAmnt >= posAmnt ? posAmnt : rmndAmnt;
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "CheckoutWithPOS4Agop"),
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("apdtagopcode", desk.AGOP_CODE),
                                 new XAttribute("apdtrwno", desk.RWNO),
                                 new XAttribute("amnt", POS )
                              )
                           )
                        )
                     );
                     posAmnt -= POS;
                  }
               }               
            }

            iScsc.SubmitChanges();
            PayPosAmnt_Txt.EditValue = PayCashAmnt_Txt.EditValue = DsctAmnt_Txt.EditValue = 0;
            AodtBs1.Position = crntPos;
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

               foreach (var _desk in _listAodts.Where(t => t.STAT != "002"))
               {
                  var d = AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.AGOP_CODE == _desk.AGOP_CODE && a.RWNO == _desk.RWNO);
                  RecStat_Butn_ButtonClick(d, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(RecStat_Butn.Buttons[3]));
               }

               Execute_Query();
            }
         }
      }

      private void CnclPayTotlDesk_Butn_Click(object sender, EventArgs e)
      {
         PayCashAmnt_Txt.EditValue = PayPosAmnt_Txt.EditValue = DsctAmnt_Txt.EditValue = 0;
      }

      private void SetStrtNow_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            aodt.STRT_TIME = aodt.END_TIME = DateTime.Now;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CloseTreeDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            int crntPos = AodtBs1.Position;

            // Find root records
            while (true)
            {
               if (aodt.Aggregation_Operation_Detail1 != null)
                  aodt = aodt.Aggregation_Operation_Detail1;
               else
                  break;
            }

            long dynamnt = 0, fixamnt = 0;
            long cashamnt = 0, posamnt = 0, dsctamnt = 0;
            int cont = 0;
            ShowRsltCalcTreeDesk_Txt.Text = "محاسبه صورتحساب";

            _listAodts.Clear();
            TreeScroll(aodt);

            // Calculate Dynamic and Fix Amount
            foreach (var desk in _listAodts)
            {
               if (desk.STAT == "001")
                  DeskClose_Butn_Click(desk);

               iScsc = new Data.iScscDataContext(ConnectionString);
               aodt = iScsc.Aggregation_Operation_Details.FirstOrDefault(a => a.AGOP_CODE == desk.AGOP_CODE && a.RWNO == desk.RWNO);
               dynamnt += (long)((aodt.EXPN_PRIC ?? 0) * aodt.NUMB);
               fixamnt += aodt.TOTL_BUFE_AMNT_DNRM ?? 0;
               // 1403/06/09
               cashamnt += aodt.CASH_AMNT ?? 0;
               posamnt += aodt.POS_AMNT ?? 0;
               dsctamnt += aodt.PYDS_AMNT ?? 0;
               ++cont;

               ShowRsltCalcTreeDesk_Txt.Text += Environment.NewLine +
                  string.Format("{0} ) " + "هزینه محاسبه شده : " + "{1:n0}   ***   هزینه ثابت : " + "{2:n0}", aodt.RWNO, (aodt.EXPN_PRIC ?? 0) * aodt.NUMB, aodt.TOTL_BUFE_AMNT_DNRM);
            }

            ShowRsltCalcTreeDesk_Txt.Text += Environment.NewLine + "-----------------------------------------------------";

            ShowRsltCalcTreeDesk_Txt.Text += Environment.NewLine +
                  string.Format("تعداد کل ردیف ها : " + "{0} عدد", cont) + Environment.NewLine +
                  string.Format("جمع هزینه محاسبه شده : " + "{0:n0}   ***   جمع هزینه ثابت : " + "{1:n0}", dynamnt, fixamnt) + Environment.NewLine +
                  string.Format("جمع کل هزینه : " + "{0:n0}", dynamnt + fixamnt) + Environment.NewLine +
                  string.Format("تخفیف ها : " + "{0:n0}", dsctamnt) + Environment.NewLine +
                  string.Format("پرداختی ها : نقدی : " + "{0:n0}" + " کارتخوان : " + "{1:n0}", cashamnt, posamnt);

            TotlDeskDynmExpnAmnt_Txt.EditValue = dynamnt;
            TotlDeskFixExpnAmnt_Txt.EditValue = fixamnt;

            // 1403/06/08
            PayCashAmnt_Txt.EditValue = PayPosAmnt_Txt.EditValue = DsctAmnt_Txt.EditValue = 0;

            PayRmndAmnt_Txt.EditValue = dynamnt + fixamnt - (cashamnt + posamnt + dsctamnt);//(Convert.ToInt64(PayCashAmnt_Txt.EditValue) + Convert.ToInt64(PayPosAmnt_Txt.EditValue) + Convert.ToInt64(DsctAmnt_Txt.EditValue));

            AodtBs1.Position = crntPos;
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

      private void TreeScroll(Data.Aggregation_Operation_Detail aodt)
      {
         _listAodts.Add(aodt);
         foreach (var recd in aodt.Aggregation_Operation_Details)
         {
            TreeScroll(recd);
         }
      }

      private void SetPosAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DsctAmnt_Txt.EditValue = PayCashAmnt_Txt.EditValue = PayPosAmnt_Txt.EditValue = 0;

            PayPosAmnt_Txt.EditValue = (Convert.ToInt64(TotlDeskDynmExpnAmnt_Txt.EditValue) + Convert.ToInt64(TotlDeskFixExpnAmnt_Txt.EditValue));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SetCashAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DsctAmnt_Txt.EditValue = PayCashAmnt_Txt.EditValue = PayPosAmnt_Txt.EditValue = 0;

            PayCashAmnt_Txt.EditValue = (Convert.ToInt64(TotlDeskDynmExpnAmnt_Txt.EditValue) + Convert.ToInt64(TotlDeskFixExpnAmnt_Txt.EditValue));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }      

      private void MoveRow(int sourceRow, int targetRow)
      {
         if (sourceRow == targetRow)
            return;

         GridView view = apdt_gv;
         DataRow srcrow = view.GetDataRow(sourceRow);
         DataRow trgrow = view.GetDataRow(targetRow);

         if (MessageBox.Show(this, "آیا میز شماره " + srcrow["RWNO"].ToString() + " به عنوان زیر مجموعه میز شماره " + trgrow["RWNO"].ToString() + " قرار گیرد؟", "تغییر ئضعیت میز", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
         
         iScsc.ExecuteCommand(string.Format("UPDATE dbo.Aggregation_Operation_Detail SET Aodt_Agop_Code = {0}, Aodt_Rwno = {1} WHERE Agop_Code = {2} AND Rwno = {3};", trgrow["AGOP_CODE"], trgrow["RWNO"], srcrow["AGOP_CODE"], srcrow["RWNO"]));
         Execute_Query();
      }

      private void TrnsDesk_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var trgtaodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (trgtaodt == null) return;

            if(TrnsDesk_Lov.EditValue == null || TrnsDesk_Lov.EditValue.ToString() == "") return;

            switch (e.Button.Index)
            {
               case 0: break;
               case 1:
                  var srcaodt = AodtTrgtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.RWNO == Convert.ToInt32(TrnsDesk_Lov.EditValue));
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Aggregation_Operation_Detail SET Aodt_Agop_Code = {0}, Aodt_Rwno = {1} WHERE Agop_Code = {2} AND Rwno = {3};", srcaodt.AGOP_CODE, srcaodt.RWNO, trgtaodt.AGOP_CODE, trgtaodt.RWNO));
                  break;
               default:
                  break;
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

      private void IndpDesk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            if (aodt.Aggregation_Operation_Detail1 == null) return;

            if (MessageBox.Show(this, "آیا با مستقل کردن میز موافق هستین؟", "مستقل کردن میز", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Aggregation_Operation_Detail SET Aodt_Agop_Code = NULL, Aodt_Rwno = NULL WHERE Agop_Code = {0} AND Rwno = {1};", aodt.AGOP_CODE, aodt.RWNO));
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

      private void SaveInfoStat_Rb_CheckedChanged(object sender, EventArgs e)
      {
         if(SaveInfoStat_Rb.Checked)
         {
            AddAgop_Butn.Enabled = DelAgop_Butn.Enabled = SaveAgop_Butn.Enabled = CloseAgop_Butn.Enabled = true;
            RprtParm_Pn.Visible = false;
            OpenDesk_Butn.Enabled = true;
            ExtpDesk_Lov.Enabled = true;
            ExpnDesks_Lov.Enabled = true;
            Expn_Cms.Enabled = ExpnBufe_Cms.Enabled = RootMenu_Cms.Enabled = Serv_Cms.Enabled = true;
            FillEndTime_Butn.Enabled = true;
            SetStrtNow_Butn.Enabled = DeskClose_Butn.Enabled = CloseOpenTable_Butn.Enabled = CalcDesk_Butn.Enabled = SaveStrtEndTime_Butn.Enabled = CalcAllDesk_Butn.Enabled = true;
            TransExpense2Another_Lov.Enabled = true;
            AutoRecalc_Cbx.Enabled = true;
            NoteBn.Enabled = true;
            DeskTree_Spc.Enabled = true;
            SaveNewRec_Butn.Enabled = true;
            SaveServ_Butn.Enabled = true;
            Bufe_GridControl.Enabled = true;
            panel2.Enabled = panel3.Enabled = true;
            Param_Ro.Height = 60;
         }
         else
         {
            AddAgop_Butn.Enabled = DelAgop_Butn.Enabled = SaveAgop_Butn.Enabled = CloseAgop_Butn.Enabled = false;
            RprtParm_Pn.Visible = true;
            OpenDesk_Butn.Enabled = false;
            ExtpDesk_Lov.Enabled = false;
            ExpnDesks_Lov.Enabled = false;
            Expn_Cms.Enabled = ExpnBufe_Cms.Enabled = RootMenu_Cms.Enabled = Serv_Cms.Enabled = false;
            FillEndTime_Butn.Enabled = false;
            SetStrtNow_Butn.Enabled = DeskClose_Butn.Enabled = CloseOpenTable_Butn.Enabled = CalcDesk_Butn.Enabled = SaveStrtEndTime_Butn.Enabled = CalcAllDesk_Butn.Enabled = false;
            TransExpense2Another_Lov.Enabled = false;
            AutoRecalc_Cbx.Checked = AutoRecalc_Cbx.Enabled = false;
            NoteBn.Enabled = false;
            DeskTree_Spc.Enabled = false;
            SaveNewRec_Butn.Enabled = false;
            SaveServ_Butn.Enabled = false;
            Bufe_GridControl.Enabled = false;
            panel2.Enabled = panel3.Enabled = false;
            Param_Ro.Height = 172;
         }

         Execute_Query();
      }

      private void ClerRprtDate_Butn_Click(object sender, EventArgs e)
      {
         RprtFromDate_Dt.Value = RprtToDate_Dt.Value = null;
      }

      private void RprtShow_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void PosAmntStp_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         try
         {
            if (e.KeyData == (Keys.Enter | Keys.Control) || e.KeyData == Keys.Enter)
            {               
               if (PosAmntTp_Txt.EditValue == null || PosAmntTp_Txt.EditValue.ToString() == "") { PosAmntTp_Txt.EditValue = 0; }

               if (ZeroCont_Cbx.Checked && !(ModifierKeys == Keys.Control))
               {
                  for (int i = 0; i < Convert.ToInt32(ZeroCont_Txt.EditValue); i++)
                  {
                     PosAmntStp_Txt.EditValue =  Convert.ToInt32(PosAmntStp_Txt.EditValue) * 10;
                  }
               }

               PosAmntTp_Txt.EditValue = Convert.ToInt64(PosAmntTp_Txt.EditValue ?? 0) + Convert.ToInt64(PosAmntStp_Txt.EditValue ?? 0);
               PosAmntStp_Txt.EditValue = 0;
               PosAmntStp_Txt.SelectAll();
               e.Handled = true;
            }
            else if(e.KeyData == Keys.F6)
            {
               PosAmntStp_Txt.Focus();
            }
            else if(e.KeyData == Keys.F7)
            {
               CashAmntStp_Txt.Focus();
            }
            else if(e.KeyData == Keys.F8)
            {
               PydsAmntStp_Txt.Focus();
            }
            else if (e.KeyData == Keys.F3)
            {
               SaveNewRec_Butn_Click(null, null);
            }
         }
         catch { PosAmntStp_Txt.Focus(); }
      }

      private void CashAmntStp_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         try
         {
            if (e.KeyData == (Keys.Enter | Keys.Control) || e.KeyData == Keys.Enter)
            {               
               if (CashAmntTp_Txt.EditValue == null || CashAmntTp_Txt.EditValue.ToString() == "") { CashAmntTp_Txt.EditValue = 0; }

               if (ZeroCont_Cbx.Checked && !(ModifierKeys == Keys.Control))
               {
                  for (int i = 0; i < Convert.ToInt32(ZeroCont_Txt.EditValue); i++)
                  {
                     CashAmntStp_Txt.EditValue = Convert.ToInt32(CashAmntStp_Txt.EditValue) * 10;
                  }
               }

               CashAmntTp_Txt.EditValue = Convert.ToInt64(CashAmntTp_Txt.EditValue ?? 0) + Convert.ToInt64(CashAmntStp_Txt.EditValue ?? 0);
               CashAmntStp_Txt.EditValue = 0;
               CashAmntStp_Txt.SelectAll();
               e.Handled = true;
            }
            else if (e.KeyData == Keys.F6)
            {
               PosAmntStp_Txt.Focus();
            }
            else if (e.KeyData == Keys.F7)
            {
               CashAmntStp_Txt.Focus();
            }
            else if (e.KeyData == Keys.F8)
            {
               PydsAmntStp_Txt.Focus();
            }
            else if (e.KeyData == Keys.F3)
            {
               SaveNewRec_Butn_Click(null, null);
            }
         }
         catch { CashAmntStp_Txt.Focus(); }
      }

      private void PydsAmntStp_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         try
         {
            if (e.KeyData == (Keys.Enter | Keys.Control) || e.KeyData == Keys.Enter)
            {
               if (PydsAmntTp_Txt.EditValue == null || PydsAmntTp_Txt.EditValue.ToString() == "") { PydsAmntTp_Txt.EditValue = 0; }

               if (ZeroCont_Cbx.Checked && !(ModifierKeys == Keys.Control))
               {
                  for (int i = 0; i < Convert.ToInt32(ZeroCont_Txt.EditValue); i++)
                  {
                     PydsAmntStp_Txt.EditValue = Convert.ToInt32(PydsAmntStp_Txt.EditValue) * 10;
                  }
               }

               PydsAmntTp_Txt.EditValue = Convert.ToInt64(PydsAmntTp_Txt.EditValue ?? 0) + Convert.ToInt64(PydsAmntStp_Txt.EditValue ?? 0);
               PydsAmntStp_Txt.EditValue = 0;
               PydsAmntStp_Txt.SelectAll();
               e.Handled = true;
            }
            else if (e.KeyData == Keys.F6)
            {
               PosAmntStp_Txt.Focus();
            }
            else if (e.KeyData == Keys.F7)
            {
               CashAmntStp_Txt.Focus();
            }
            else if (e.KeyData == Keys.F8)
            {
               PydsAmntStp_Txt.Focus();
            }
            else if(e.KeyData == Keys.F3)
            {
               SaveNewRec_Butn_Click(null, null);
            }
         }
         catch { PydsAmntStp_Txt.Focus(); }
      }

      private void AddSuns_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            if (SunsBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

            var suns = SunsBs.AddNew() as Data.App_Base_Define;
            suns.ENTY_NAME = "Suns_Group";
            suns.RWNO = SunsBs.List.Count;
            suns.COLR = Color.YellowGreen.ToArgb();
            suns.SEX_TYPE = "001";

            iScsc.App_Base_Defines.InsertOnSubmit(suns);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelSuns_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            var suns = SunsBs.Current as Data.App_Base_Define;
            if (suns == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.App_Base_Defines.DeleteOnSubmit(suns);

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

      private void SaveSuns_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            Suns_Gv.PostEditor();
            SunsBs.EndEdit();

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

      private void SaveStng_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var stng = StngBs.Current as Data.Setting;
            if (stng == null) return;

            iScsc.ExecuteCommand(
               string.Format(@"
                  UPDATE dbo.Settings
                     SET Snd1_Path = N'{0}',
                         Snd2_Path = N'{1}',
                         Snd3_Path = N'{2}',
                         Snd4_Path = N'{3}',
                         Snd5_Path = N'{4}',
                         Snd6_Path = N'{5}'
               ", stng.SND1_PATH, stng.SND2_PATH, stng.SND3_PATH, stng.SND4_PATH,
                  stng.SND5_PATH, stng.SND6_PATH)
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SunsColr_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var suns = SunsBs.Current as Data.App_Base_Define;
            if (suns == null) return;

            suns.COLR = ((Color)e.NewValue).ToArgb();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SunName_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var suns = SunsBs.Current as Data.App_Base_Define;
            if (suns == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  suns.TITL_DESC = string.Format("{0:HH:mm} - {1:HH:mm}", suns.PRT2_TIME, suns.PRT6_TIME);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Cont_Txt_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  Diagnostics.Process.Start("calc");
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SlctSuns_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  // Goto Next
                  SunsBs.MovePrevious();
                  SlctSuns_Lov.EditValue = (SunsBs.Current as Data.App_Base_Define).CODE;
                  break;
               case 2:
                  // Goto Last
                  SunsBs.MoveFirst();
                  SlctSuns_Lov.EditValue = (SunsBs.Current as Data.App_Base_Define).CODE;
                  break;
               case 3:
                  // Set Null
                  SlctSuns_Lov.EditValue = null;
                  break;
               case 4:
                  // Goto Next
                  SunsBs.MoveNext();
                  SlctSuns_Lov.EditValue = (SunsBs.Current as Data.App_Base_Define).CODE;
                  break;
               case 5:
                  // Goto Last
                  SunsBs.MoveLast();
                  SlctSuns_Lov.EditValue = (SunsBs.Current as Data.App_Base_Define).CODE;
                  break;
               case 6:
                  // Refresh Suns Information                 
                  var _suns = SunsBs.Current as Data.App_Base_Define;
                  if (_suns == null)
                  {
                     SunsInfo_Lb.Text = "";
                     return;
                  }

                  SunsInfo_Lb.Text = string.Format("زمان باقیمانده : " + "{0}" + " دقیقه" + ", ظرفیت باقیمانده ورودی سانس : " + "{1}" + " نفر", (int)((_suns.PRT5_TIME.Value.TimeOfDay - DateTime.Now.TimeOfDay).TotalMinutes), (_suns.NUMB - AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Where(a => a.GROP_APBS_CODE == _suns.CODE).Count()));
                  break;
               default:
                  break;
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DupSuns_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            var _suns = SunsBs.Current as Data.App_Base_Define;
            if (_suns == null) return;

            if (SunsBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

            var _dupSuns = SunsBs.AddNew() as Data.App_Base_Define;
            _dupSuns.ENTY_NAME = _suns.ENTY_NAME;
            _dupSuns.RWNO = SunsBs.List.Count;
            _dupSuns.SEX_TYPE = _suns.SEX_TYPE;
            _dupSuns.COLR = _suns.COLR;
            _dupSuns.STAT = _suns.STAT;
            _dupSuns.NUMB = _suns.NUMB;
            _dupSuns.PRT1_TIME = _suns.PRT1_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));
            _dupSuns.PRT2_TIME = _suns.PRT2_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));
            _dupSuns.PRT3_TIME = _suns.PRT3_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));
            _dupSuns.PRT4_TIME = _suns.PRT4_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));
            _dupSuns.PRT5_TIME = _suns.PRT5_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));
            _dupSuns.PRT6_TIME = _suns.PRT6_TIME.Value.AddMinutes(Convert.ToDouble(EndTimeValu_Txt.EditValue));
            _dupSuns.TITL_DESC = string.Format("{0:HH:mm} - {1:HH:mm}", _dupSuns.PRT2_TIME, _dupSuns.PRT6_TIME);

            iScsc.App_Base_Defines.InsertOnSubmit(_dupSuns);
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

      private void MoveToSuns_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _suns = SunsBs.Current as Data.App_Base_Define;
            if (_suns == null) return;

            var _aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (_aodt == null) return;

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Aggregation_Operation_Detail SET Grop_Apbs_Code = {0} WHERE AGOP_CODE = {1} AND Rwno = {2};", _suns.CODE, _aodt.AGOP_CODE, _aodt.RWNO));
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

      private void SunsBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            if (SunsFltr_Cbx.Checked)
            {
               var _suns = SunsBs.Current as Data.App_Base_Define;
               if (_suns == null) return;

               apdt_gv.ActiveFilterString = string.Format("Grop_Apbs_Code = {0}", _suns.CODE);
            }
            else
               apdt_gv.ActiveFilterString = "";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SunsFltr_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         SunsBs_CurrentChanged(null, null);
      }

      private void SndiPath_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _sndi = sender as ButtonEdit;

            if (SondSlct_Ofd.ShowDialog() != DialogResult.OK) return;

            _sndi.Text = SondSlct_Ofd.FileName;

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Settings SET {0}_PATH = N'{1}'", _sndi.Tag, _sndi.Text));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CustName_Txt_TextChanged(object sender, EventArgs e)
      {
         TimrStat_PkBt.PickChecked = true;
      }

      private void SetDsctAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DsctAmnt_Txt.EditValue = PayCashAmnt_Txt.EditValue = PayPosAmnt_Txt.EditValue = 0;

            DsctAmnt_Txt.EditValue = (Convert.ToInt64(TotlDeskDynmExpnAmnt_Txt.EditValue) + Convert.ToInt64(TotlDeskFixExpnAmnt_Txt.EditValue));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
