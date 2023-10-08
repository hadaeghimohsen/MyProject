using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.CalculateExpense
{
   public partial class CAL_CEXC_F : UserControl
   {
      public CAL_CEXC_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if (tb_master.SelectedTab == tp_001)
         {
            MsexBs.DataSource = iScsc.Misc_Expenses.Where(m => Fga_Urgn_U.Split(',').Contains(m.REGN_PRVN_CODE + m.REGN_CODE) && Fga_Uclb_U.Contains(m.CLUB_CODE) && m.VALD_TYPE == "001" && m.CALC_EXPN_TYPE == "001");
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            MosxBs2.DataSource = iScsc.Misc_Expenses.Where(m => Fga_Urgn_U.Split(',').Contains(m.REGN_PRVN_CODE + m.REGN_CODE) && Fga_Uclb_U.Contains(m.CLUB_CODE) && m.VALD_TYPE == "001" && m.CALC_EXPN_TYPE == "002");
         }
      }

      private void Commit()
      {
         try
         {
            Validate();
            MsexBs.EndEdit();
            MosxBs2.EndEdit();
            Pyde_Gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
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

      private void bt_calcexpn_Click(object sender, EventArgs e)
      {
         try
         {
            DateTime firsttime = DateTime.Now;
            if (!Pde_FromDate.Value.HasValue) { Pde_FromDate.Focus(); return; }
            if (!Pde_ToDate.Value.HasValue) { Pde_ToDate.Focus(); return; }
            
            iScsc.CommandTimeout = int.MaxValue;

            iScsc.CALC_EXPN_P(
               new XElement("Process",
                  new XElement("Payment",
                     new XAttribute("fromdate", Pde_FromDate.Value.Value.Date.ToString("yyyy-MM-dd")),
                     new XAttribute("todate", Pde_ToDate.Value.Value.Date.ToString("yyyy-MM-dd")),
                     new XAttribute("cochfileno", Coch_Lov.EditValue ?? ""),
                     new XAttribute("decrprct", DecrPrct_Te.EditValue ?? ""),
                     new XAttribute("mtodcode", Mtod_Lov.EditValue ?? ""),
                     new XAttribute("ctgycode", Ctgy_Lov.EditValue ?? ""),
                     new XAttribute("cochdegr", Degr_Lov.EditValue ?? ""),
                     new XAttribute("epitcode", Epit_Lov.EditValue ?? ""),
                     new XAttribute("cetpcode", Cetp1_Lov.EditValue ?? ""),
                     new XAttribute("cxtpcode", Cxtp1_Lov.EditValue ?? ""),
                     new XAttribute("rqtpcode", Rqtp_Lov.EditValue ?? "")
                  )
               )
            );

            Stopwatch_Lb.Text = (DateTime.Now - firsttime).ToString();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
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

      private void bt_confexpn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با ذخیره شدن اطلاعات هزینه های وارد شده موافق هستید؟", "تایید نهایی هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (tb_master.SelectedTab == tp_001)
               iScsc.CONF_EXPN_P(
                  new XElement("Process",
                     new XElement("Misc_Expenses",
                        new XAttribute("calcexpntype", "001"),
                        MsexBs.List.OfType<Data.Misc_Expense>().ToList()
                        .Select(m =>
                           new XElement("Misc_Expense",
                              new XAttribute("code", m.CODE),
                              new XAttribute("cochfileno", m.COCH_FILE_NO),
                              new XAttribute("delvby", m.DELV_BY ?? ""),
                              new XAttribute("delvdate", m.DELV_DATE == null ? "0001-01-01" : m.DELV_DATE.Value.ToString("yyyy/MM/dd")),
                              new XElement("Expn_Desc", m.EXPN_DESC ?? "")
                           )
                        )
                     )
                  )
               );
            else if(tb_master.SelectedTab == tp_002)
               iScsc.CONF_EXPN_P(
                  new XElement("Process",
                     new XElement("Misc_Expenses",
                        new XAttribute("calcexpntype", "002"),
                        MosxBs2.List.OfType<Data.Misc_Expense>().ToList()
                        .Select(m =>
                           new XElement("Misc_Expense",
                              new XAttribute("code", m.CODE),
                              new XAttribute("delvby", m.DELV_BY ?? ""),
                              new XAttribute("delvdate", m.DELV_DATE == null ? "0001-01-01" : m.DELV_DATE.Value.ToString("yyyy/MM/dd")),
                              new XElement("Expn_Desc", m.EXPN_DESC ?? "")
                           )
                        )
                     )
                  )
               );
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

      private void msex_save_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               Commit();
            }
            else if (tb_master.SelectedTab == tp_002)
            {
               MosxBs2.EndEdit();
               Mosx_Gv.PostEditor();
               var mosx = MosxBs2.Current as Data.Misc_Expense;
               if (mosx.CLUB_CODE == null)
               {
                  MessageBox.Show("هزینه بایستی به یکی از شیفت باشگاه تعلق گیرد");
                  return;
               }
               if (mosx.EPIT_CODE == null)
               {
                  MessageBox.Show("نوع هزینه مشخص نیست");
                  return;
               }
               if (mosx.EXPN_AMNT == null || mosx.EXPN_AMNT <= 0)
               {
                  MessageBox.Show("مبلغ هزینه مشخص نیست");
                  return;
               }
               if (mosx.EXPN_DESC == null || mosx.EXPN_DESC == "")
               {
                  MessageBox.Show("شرح هزینه وارد نشده است");
                  return;
               }
            }
            Commit();
         }
         catch { }
      }

      private void tsb_oreload_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void PRVN_LOV_Enter(object sender, EventArgs e)
      {
         PrvnBs2.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));
      }

      private void REGN_LOV_Click(object sender, EventArgs e)
      {
         var Crnt = MosxBs2.Current as Data.Misc_Expense;

         if (Crnt == null) return;
         if (Crnt.REGN_PRVN_CODE == null) { return; }

         RegnBs2.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == Crnt.REGN_PRVN_CODE && Fga_Urgn_U.Split(',').Contains(r.PRVN_CODE + r.CODE));
      }

      private void CLUB_LOV_Click(object sender, EventArgs e)
      {
         /*var Crnt = MOSX_BindingSource.Current as Data.Misc_Expense;

         if (Crnt == null) return;
         if (Crnt.REGN_PRVN_CODE == null || Crnt.REGN_CODE == null) { return; }

         ClubBs2.DataSource = iScsc.Clubs.Where(r => r.REGN_PRVN_CODE == Crnt.REGN_PRVN_CODE && r.REGN_CODE == Crnt.REGN_CODE && Fga_Uclb_U.Contains(r.CODE));*/
      }

      private void AddMosx_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            MosxBs2.AddNew();
            var mosx = MosxBs2.Current as Data.Misc_Expense;
            mosx.CLUB_CODE = (ClubBs2.Current as Data.Club).CODE;
            mosx.EPIT_CODE = (EpitBs2.Current as Data.Expense_Item).CODE;
            mosx.DELV_DATE = DateTime.Now;

            iScsc.Misc_Expenses.InsertOnSubmit(mosx);
         }
         catch{ }
      }

      private void DelMosx_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mosx = MosxBs2.Current as Data.Misc_Expense;

            if (mosx != null && MessageBox.Show(this,"آیا با حذف هزینه مطمئن هستید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if (mosx.CODE == 0)
               MosxBs2.RemoveCurrent();
            else
            {
               iScsc.Misc_Expenses.DeleteOnSubmit(mosx);
               Commit();
            }
            requery = true;
         }
         catch 
         {
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

      private void CalcSalary_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Convert.ToInt64(cOCH_FILE_NOSearchLookUpEdit.EditValue);

            var pers = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && f.Fighter_Publics.Any(fp => fp.RWNO == f.FGPB_RWNO_DNRM && fp.RECT_CODE == "004" /*&& fp.CNTR_CODE != null*/));

            if(pers == null)
            {
               MessageBox.Show("شماره قرارداد برای پرسنل ثبت نشده");
               return;
            }

            MosxBs2.AddNew();

            long? cnrtcode = null;// pers.Fighter_Publics.FirstOrDefault(fp => fp.RWNO == pers.FGPB_RWNO_DNRM && fp.RECT_CODE == "004").CNTR_CODE;
            var crnt = MosxBs2.Current as Data.Misc_Expense;
            crnt.EXPN_AMNT = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == cnrtcode).Sum(pd => pd.EXPN_PRIC ?? 0);
            crnt.EXPN_DESC = "حقوق و دستمزد";
            crnt.COCH_FILE_NO = fileno;
            crnt.DELV_DATE = DateTime.Now;

         }
         catch 
         {}
      }

      private void BaseCalc_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 68 /* Execute Cal_Expn_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ReportCalc_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 90 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_P", 10 /* Execute Actn_Calf_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var pyde = PydeBs.Current as Data.Payment_Expense;
            if (pyde == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pyde.Payment_Detail.Request_Row.FIGH_FILE_NO)) }
            );
         }
         catch (Exception )
         {

         }
      }

      private void Clear_Butn_Click(object sender, EventArgs e)
      {
         Coch_Lov.EditValue = null;
         Mtod_Lov.EditValue = null;
         Ctgy_Lov.EditValue = null;
         Degr_Lov.EditValue = null;
         Epit_Lov.EditValue = null;
         Cxtp1_Lov.EditValue = null;
         Cetp1_Lov.EditValue = null;
         Rqtp_Lov.EditValue = null;
      }

      private void Mtod_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long)e.NewValue && c.CTGY_STAT == "002");
         }
         catch { }
      }

      private void Epit1_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var msex = MsexBs.Current as Data.Misc_Expense;
            if (msex == null) return;

            Msex_Gv.PostEditor();

            switch (e.Button.Index)
            {
               case 1:
                  MsexBs.List.OfType<Data.Misc_Expense>().ToList().ForEach(m => m.EPIT_CODE = msex.EPIT_CODE);
                  break;
               default:
                  return;
            }

            iScsc.SubmitChanges();
            requery = true;
         }
         catch ( Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void MsedBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            MPydtBs.DataSource = null;

            var _msed = MsedBs.Current as Data.Misc_Expense_Deduction;
            if (_msed == null) return;

            MPydtBs.DataSource = 
               from pm in iScsc.Payment_Methods
               join pd in iScsc.Payment_Details on pm.PYMT_RQST_RQID equals pd.PYMT_RQST_RQID
               where pm.CODE == _msed.PMTD_CODE
               select pd;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstBnDefaultPrint1_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnPrint1_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {
                 new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnSettingPrint1_Click(object sender, EventArgs e)
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

      private void AddMexm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            if (MexmBs.List.OfType<Data.Misc_Expense_Method>().Any(m => m.CODE == 0)) return;

            var _mexm = MexmBs.AddNew() as Data.Misc_Expense_Method;
            _mexm.MSEX_CODE = _msxc.CODE;
            _mexm.RCPT_MTOD = "003";
            _mexm.ACTN_DATE = DateTime.Now;
            _mexm.AMNT = _msxc.SUM_NET_AMNT_DNRM - (_msxc.SUM_RCPT_PYMT_DNRM + _msxc.SUM_COST_AMNT_DNRM + _msxc.SUM_DSCT_AMNT_DNRM);

            iScsc.Misc_Expense_Methods.InsertOnSubmit(_mexm);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelMexm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mexm = MexmBs.Current as Data.Misc_Expense_Method;
            if (_mexm == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Methods.DeleteOnSubmit(_mexm);
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

      private void SaveMexm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mexm_Gv.PostEditor();

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

      private void AddMeck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            if (MeckBs.List.OfType<Data.Misc_Expense_Check>().Any(m => m.CODE == 0)) return;

            var _meck = MeckBs.AddNew() as Data.Misc_Expense_Check;
            _meck.MSEX_CODE = _msxc.CODE;
            _meck.CHEK_DATE = DateTime.Now;

            iScsc.Misc_Expense_Checks.InsertOnSubmit(_meck);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelMeck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _meck = MeckBs.Current as Data.Misc_Expense_Check;
            if (_meck == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Checks.DeleteOnSubmit(_meck);
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

      private void SaveMeck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Meck_Gv.PostEditor();

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

      private void AddMexd_Butn_Click(object sender, EventArgs e)
      {
         
      }

      private void DelMexd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mexd = MexdBs.Current as Data.Misc_Expense_Discount;
            if (_mexd == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Discounts.DeleteOnSubmit(_mexd);
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

      private void SaveMexd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mexd_Gv.PostEditor();

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

      private void DsctDef_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Misc_Expense_Discount_INFO"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MexdActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            var _dmexd = DMexdBs.Current as Data.App_Base_Define;
            if (_dmexd == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _mexd = MexdBs.AddNew() as Data.Misc_Expense_Discount;
                  _mexd.MSEX_CODE = _msxc.CODE;
                  _mexd.DSCT_APBS_CODE = _dmexd.CODE;

                  iScsc.Misc_Expense_Discounts.InsertOnSubmit(_mexd);
                  iScsc.SubmitChanges();
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

      private void AddMexc_Butn_Click(object sender, EventArgs e)
      {

      }

      private void DelMexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mexc = MexcBs.Current as Data.Misc_Expense_Cost;
            if (_mexc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Costs.DeleteOnSubmit(_mexc);
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

      private void SaveMexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mexc_Gv.PostEditor();

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

      private void MexcDef_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Misc_Expense_Cost_INFO"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MexcActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            var _dmexc = DMexcBs.Current as Data.App_Base_Define;
            if (_dmexc == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _mexc = MexcBs.AddNew() as Data.Misc_Expense_Cost;
                  _mexc.MSEX_CODE = _msxc.CODE;
                  _mexc.COST_APBS_CODE = _dmexc.CODE;

                  iScsc.Misc_Expense_Costs.InsertOnSubmit(_mexc);
                  iScsc.SubmitChanges();
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

      private void PrntActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _msex = MsexBs.Current as Data.Misc_Expense;
            if (_msex == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  // Print Settings
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                           new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"))}
                        })
                  );                  

                  // string.Format("Misc_Expense.Code = {0}", _msex.CODE)
                  break;
               case 1:
                  // Select Print
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Misc_Expense.Code = {0}", _msex.CODE))}
                        })
                  );
                  break;
               case 2:
                  // Default Print
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Misc_Expense.Code = {0}", _msex.CODE))}
                        })
                  );
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
   }
}
