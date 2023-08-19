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
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.Scsc.ExtCode;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.Warehouse
{
   public partial class DEF_PROD_F : UserControl
   {
      public DEF_PROD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            int _epit = EpitBs.Position;
            int _ulnf = UlnfBs.Position;
            int _ulns = UlnsBs.Position;
            int _whdt = WhdtBs.Position;
            int _wtag = WtagBs.Position;
            int _wexpn = WExpnBs.Position;

            EpitBs.DataSource = iScsc.Expense_Items.Where(i => i.TYPE == "001" && i.RQTP_CODE == "016" && i.RQTT_CODE == "001");
            ReglBs.DataSource = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001");

            if (Master_Tc.SelectedTab == tp_001)
            {
               FindWrhs_Butn_Click(null, null);
               ExpnStat_Cbx_CheckedChanged(null, null);
            }
            else if (Master_Tc.SelectedTab == tp_002)
            {

            }


            UlnfBs.DataSource = iScsc.User_Link_Fighters.Where(u => u.USER_DB == CurrentUser);
            UlnsBs.DataSource = iScsc.User_Link_Sections.Where(u => u.USER_DB == CurrentUser);

            UlnfBs.Position = _ulnf;
            UlnsBs.Position = _ulns;
            EpitBs.Position = _epit;
            WhdtBs.Position = _whdt;
            WtagBs.Position = _wtag;
            WExpnBs.Position = _wexpn;

            requery = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void AddEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (EpitBs.List.OfType<Data.Expense_Item>().Any(i => i.CODE == 0)) return;

            var _epit = EpitBs.AddNew() as Data.Expense_Item;

            iScsc.Expense_Items.InsertOnSubmit(_epit);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _epit = EpitBs.Current as Data.Expense_Item;
            if (_epit == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.DEL_EPIT_P(_epit.CODE);
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

      private void SaveEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            epit_gv.PostEditor();

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

      private void ReloadForm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Execute_Query();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void EpitBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _epit = EpitBs.Current as Data.Expense_Item;
            if (_epit == null) return;

            var _regl = ReglBs.Current as Data.Regulation;
            if (_regl == null) return;

            ExtpBs.DataSource =
               iScsc.Expense_Types.Where(et => et.Request_Requester.Regulation == _regl && et.Request_Requester.RQTP_CODE == "016" && et.Request_Requester.RQTT_CODE == "001" && et.Expense_Item == _epit);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void EpitActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _epit = EpitBs.Current as Data.Expense_Item;
            if (_epit == null) return;

            var _regl = ReglBs.Current as Data.Regulation;
            if (_regl == null) return;

            var _rqrq = _regl.Request_Requesters.FirstOrDefault(rr => rr.RQTP_CODE == "016" && rr.RQTT_CODE == "001");
            if (_rqrq == null) return;

            var crnt = ExtpBs.Current as Data.Expense_Type;
            iScsc.REGL_TOTL_P(
               new XElement("Config",
                  new XAttribute("type", "001"),
                  ExtpBs.List.OfType<Data.Expense_Type>().Where(c => c.CRET_BY == null).Select(c =>
                     new XElement("Insert",
                        new XElement("Expense_Type",
                           new XAttribute("rqrqcode", _rqrq.CODE),
                           new XAttribute("epitcode", _epit.CODE)
                        )
                     )
                  ),
                  crnt.CRET_BY != null ?
                     new XElement("Update",
                        new XElement("Expense_Type",
                           new XAttribute("code", crnt.CODE),
                           new XAttribute("extpdesc", crnt.EXTP_DESC)
                        )
                    ) : new XElement("Update")
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
            }
         }
      }

      private void SetDescExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (AllExpn_Cbx.Checked)
            {
               ExpnBs.MoveFirst();
               foreach (var expn in ExpnBs.List.OfType<Data.Expense>())
               {
                  expn.EXPN_DESC =
                     string.Format("{0} {1}، {2}",
                     ExtpDesc_Cbx.Checked ? expn.Expense_Type.Expense_Item.EPIT_DESC : "",
                     MtodDesc_Cbx.Checked ? expn.Method.MTOD_DESC : "",
                     CtgyDesc_Cbx.Checked ? expn.Category_Belt.CTGY_DESC : ""
                  );
               }
            }
            else
            {
               var expn = ExpnBs.Current as Data.Expense;
               if (expn == null) return;

               expn.EXPN_DESC =
                  string.Format("{0} {1}، {2}",
                  ExtpDesc_Cbx.Checked ? expn.Expense_Type.Expense_Item.EPIT_DESC : "",
                  MtodDesc_Cbx.Checked ? expn.Method.MTOD_DESC : "",
                  CtgyDesc_Cbx.Checked ? expn.Category_Belt.CTGY_DESC : ""
               );
            }
         }
         catch { }
      }

      private void SaveComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Expn_Gv.PostEditor();

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

      private void UnitAcnt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
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
                                    new XAttribute("tablename", "PRODUCTUNIT_INFO"),
                                    new XAttribute("formcaller", GetType().Name),
                                    new XAttribute("gototab", "tp_002")
                                 )
                           }
                        }
                     )
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

      private void AddWrhs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (WrhsBs.List.OfType<Data.Warehouse>().Any(w => w.CODE == 0)) return;

            var _wrhs = WrhsBs.AddNew() as Data.Warehouse;
            _wrhs.WRHS_TYPE = "001";
            _wrhs.WRHS_STAT = "001";
            _wrhs.WRHS_DATE = DateTime.Now;

            iScsc.Warehouses.InsertOnSubmit(_wrhs);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelWrhs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            if (_wrhs.WRHS_STAT == "002") return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            iScsc.Warehouses.DeleteOnSubmit(_wrhs);

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

      private void SaveWrhs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            WrhsBs.EndEdit();

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

      private void FindWrhs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromWrhsDate_Dt.Value.HasValue)
            {
               FromWrhsDate_Dt.Focus();
               FromWrhsDate_Dt.Value = DateTime.Now;
               var day = FromWrhsDate_Dt.GetText("dd").ToInt32();
               if (day != 1)
                  FromWrhsDate_Dt.Value = FromWrhsDate_Dt.Value.Value.AddDays((day - 1) * -1);
            }
            if (!ToWrhsDate_Dt.Value.HasValue) { ToWrhsDate_Dt.Focus(); ToWrhsDate_Dt.Value = DateTime.Now; }

            WrhsBs.DataSource =
               iScsc.Warehouses.
               Where(w =>
                  (WrhsStat_Lov.EditValue == null || WrhsStat_Lov.EditValue.ToString() == "" || w.WRHS_STAT == WrhsStat_Lov.EditValue.ToString()) &&
                  (WrhsType_Lov.EditValue == null || WrhsType_Lov.EditValue.ToString() == "" || w.WRHS_TYPE == WrhsType_Lov.EditValue.ToString()) &&
                  (!FromWrhsDate_Dt.Value.HasValue || FromWrhsDate_Dt.Value.Value.Date <= w.WRHS_DATE.Value.Date) &&
                  (!ToWrhsDate_Dt.Value.HasValue || ToWrhsDate_Dt.Value.Value.Date >= w.WRHS_DATE.Value.Date) &&
                  (WrhsNumb_Txt.EditValue == null || WrhsNumb_Txt.EditValue.ToString() == "" || w.WRHS_NUMB.Contains(WrhsNumb_Txt.Text)) &&
                  (WrhsSorcPostAdrs_Txt.EditValue == null || WrhsSorcPostAdrs_Txt.EditValue.ToString() == "" || w.SORC_POST_ADRS.Contains(WrhsSorcPostAdrs_Txt.Text)) &&
                  (!CretBy_Cbx.Checked || w.CRET_BY == CurrentUser)
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void FinlWrhs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.UNIT_APBS_CODE == null || wd.SECT_APBS_CODE == null || wd.FIGH_FILE_NO == null || wd.PRIC == 0 || (wd.WEGH == 0 && wd.QNTY == 0)))
            {
               MessageBox.Show("بعضی از آیتم و فیلدهای ردیف های فاکتور درست وارد نشده لطفا اطلاعات را وارد کنید");
               return;
            }
            else if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.QNTY != 0 && wd.WEGH != 0))
            {
               MessageBox.Show("هر ردیف فاکتور فقط اجازه ذخیره کردن تعداد یا وزن را دارید هر دو مورد اطلاعات وارد کنید بی معنی میباشد");
               return;
            }

            if (MessageBox.Show(this, "آیا با ذخیره کردن فاکتور موافق هستید؟", "نهایی کردن فاکتور", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            WrhsBs.EndEdit();
            _wrhs.WRHS_STAT = "002";

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

      private void ExpnActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.CODE == 0)) return;

            if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.WRHS_CODE == _wrhs.CODE && wd.EXPN_CODE == _expn.CODE)) return;

            var _whdt = WhdtBs.AddNew() as Data.Warehouse_Detail;
            if (_whdt == null) return;

            _whdt.WRHS_CODE = _wrhs.CODE;            
            _whdt.EXPN_CODE = _expn.CODE;
            _whdt.RECD_STAT = _wrhs.WRHS_TYPE;

            if (FighDflt_Cbx.Checked)
               _whdt.FIGH_FILE_NO = (UlnfBs.Current as Data.User_Link_Fighter).FIGH_FILE_NO;

            if(SectDflt_Cbx.Checked)
               _whdt.SECT_APBS_CODE = (UlnsBs.Current as Data.User_Link_Section).SECT_APBS_CODE;

            iScsc.Warehouse_Details.InsertOnSubmit(_whdt);
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

      private void NatlCode_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            RServBs.DataSource =
               iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.NATL_CODE_DNRM.Contains(NatlCode_Txt.Text));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CellPhon_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            RServBs.DataSource =
               iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.CELL_PHON_DNRM.Contains(CellPhon_Txt.Text));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Serv_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (Serv_Lov.EditValue == null || Serv_Lov.EditValue.ToString() == "") return;

            switch (e.Button.Index)
            {
               case 1:
                  if (iScsc.User_Link_Fighters.Any(u => u.FIGH_FILE_NO == Serv_Lov.EditValue.ToString().ToInt64() && u.USER_DB == CurrentUser)) return;

                  iScsc.ExecuteCommand(
                     string.Format("INSERT INTO dbo.USer_Link_Fighter (User_Db, Figh_File_No, Code) VALUES ('{0}', {1}, 0);",
                     CurrentUser, Serv_Lov.EditValue)
                  );
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

      private void UlnfActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            var _ulnf = UlnfBs.Current as Data.User_Link_Fighter;
            if (_ulnf == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _whdt = WhdtBs.Current as Data.Warehouse_Detail;
                  if (_whdt == null) return;

                  if((_whdt.FIGH_FILE_NO == null) || (_whdt.FIGH_FILE_NO != _ulnf.FIGH_FILE_NO && MessageBox.Show(this, "آیا با تغییر شخص ردیف فاکتور موافق هستید؟", "تغییر شخص ردیف فاکتور", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                  {
                     iScsc.ExecuteCommand(
                        string.Format("UPDATE dbo.Warehouse_Detail SET FIGH_FILE_NO = {0} WHERE CODE = {1};", _ulnf.FIGH_FILE_NO, _whdt.CODE)
                     );
                  }                  
                  break;
               case 1:
                  if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.FIGH_FILE_NO != null && wd.FIGH_FILE_NO != _ulnf.FIGH_FILE_NO) && MessageBox.Show(this, "آیا با تغییر تمام ردیف فاکتور با این شخص موافق هستید؟", "تغییر تمام ردیف فاکتور با شخص", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(
                     string.Format("UPDATE dbo.Warehouse_Detail SET FIGH_FILE_NO = {0} WHERE WRHS_CODE = {1};", _ulnf.FIGH_FILE_NO, _wrhs.CODE)
                  );
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

      private void ExpnStat_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         Expn_Gv.ActiveFilterString = string.Format("EXPN_STAT = '{0}'", ExpnStat_Cbx.Checked ? "002" : "001");
      }

      private void WdtlWidt_Butn_Click(object sender, EventArgs e)
      {
         if (Wrhs_Scc.SplitterPosition != 261)
            Wrhs_Scc.SplitterPosition = 261;
         else
            Wrhs_Scc.SplitterPosition = 0;
      }

      private void UnitDef_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "PRODUCTUNIT_INFO"),
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("gototab", "tp_002")
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

      private void SectDef_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "SECTION_INFO"),
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

      private void Sect_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (Sect_Lov.EditValue == null || Sect_Lov.EditValue.ToString() == "") return;

            switch (e.Button.Index)
            {
               case 1:
                  if (iScsc.User_Link_Sections.Any(u => u.SECT_APBS_CODE == Sect_Lov.EditValue.ToString().ToInt64() && u.USER_DB == CurrentUser)) return;

                  iScsc.ExecuteCommand(
                     string.Format("INSERT INTO dbo.USer_Link_Section (User_Db, Sect_Apbs_Code, Code) VALUES ('{0}', {1}, 0);",
                     CurrentUser, Sect_Lov.EditValue)
                  );
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

      private void UlnsActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            var _ulns = UlnsBs.Current as Data.User_Link_Section;
            if (_ulns == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _whdt = WhdtBs.Current as Data.Warehouse_Detail;
                  if (_whdt == null) return;

                  if ((_whdt.SECT_APBS_CODE == null) || (_whdt.SECT_APBS_CODE != _ulns.SECT_APBS_CODE && MessageBox.Show(this, "آیا با تغییر بخش ردیف فاکتور موافق هستید؟", "تغییر بخش ردیف فاکتور", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                  {
                     iScsc.ExecuteCommand(
                        string.Format("UPDATE dbo.Warehouse_Detail SET Sect_Apbs_Code = {0} WHERE CODE = {1};", _ulns.SECT_APBS_CODE, _whdt.CODE)
                     );
                  }
                  break;
               case 1:
                  if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.SECT_APBS_CODE != null && wd.SECT_APBS_CODE != _ulns.SECT_APBS_CODE) && MessageBox.Show(this, "آیا با تغییر تمام ردیف فاکتور با این بخش موافق هستید؟", "تغییر تمام ردیف فاکتور با بخش", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(
                     string.Format("UPDATE dbo.Warehouse_Detail SET Sect_Apbs_Code = {0} WHERE WRHS_CODE = {1};", _ulns.SECT_APBS_CODE, _wrhs.CODE)
                  );
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

      private void DelWhdt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _whdt = WhdtBs.Current as Data.Warehouse_Detail;
            if (_whdt == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Warehouse_Details.DeleteOnSubmit(_whdt);
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

      private void SaveWhdt_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            Whdt_Gv.PostEditor();

            if (WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.UNIT_APBS_CODE == null || wd.SECT_APBS_CODE == null || wd.FIGH_FILE_NO == null || wd.PRIC == 0 || (wd.WEGH == 0 && wd.QNTY == 0)))
            {
               MessageBox.Show("بعضی از آیتم و فیلدهای ردیف های فاکتور درست وارد نشده لطفا اطلاعات را وارد کنید");
               return;
            }
            else if(WhdtBs.List.OfType<Data.Warehouse_Detail>().Any(wd => wd.QNTY != 0 && wd.WEGH != 0))
            {
               MessageBox.Show("هر ردیف فاکتور فقط اجازه ذخیره کردن تعداد یا وزن را دارید هر دو مورد اطلاعات وارد کنید بی معنی میباشد");
               return;
            }

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

      private void WExpnActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            var _dwexpn = DWExpnBs.Current as Data.App_Base_Define;
            if (_dwexpn == null) return;

            if (WExpnBs.List.OfType<Data.Warehouse_Expense>().Any(wt => wt.CODE == 0)) return;

            var _wexpn = WExpnBs.AddNew() as Data.Warehouse_Expense;

            _wexpn.WRHS_CODE = _wrhs.CODE;
            _wexpn.RECD_APBS_CODE = _dwexpn.CODE;

            iScsc.Warehouse_Expenses.InsertOnSubmit(_wexpn);
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

      private void DelWExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _wexpn = WhdtBs.Current as Data.Warehouse_Expense;
            if (_wexpn == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Warehouse_Expenses.DeleteOnSubmit(_wexpn);
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

      private void SaveWExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            WExpn_Gv.PostEditor();

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

      private void WExpn_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "WarehouseExpense_INFO"),
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

      private void WTagActn_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _wrhs = WrhsBs.Current as Data.Warehouse;
            if (_wrhs == null) return;

            var _dwtag = DWTagBs.Current as Data.App_Base_Define;
            if (_dwtag == null) return;

            if (WtagBs.List.OfType<Data.Warehouse_Tag>().Any(wt => wt.CODE == 0 || wt.RECD_APBS_CODE == _dwtag.CODE)) return;

            var _wtag = WtagBs.AddNew() as Data.Warehouse_Tag;

            _wtag.WRHS_CODE = _wrhs.CODE;
            _wtag.RECD_APBS_CODE = _dwtag.CODE;

            iScsc.Warehouse_Tags.InsertOnSubmit(_wtag);
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

      private void DelWTag_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _wtag = WtagBs.Current as Data.Warehouse_Tag;
            if (_wtag == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Warehouse_Tags.DeleteOnSubmit(_wtag);
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

      private void SaveWTag_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            WTag_Gv.PostEditor();

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

      private void Wtag_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "WarehouseTag_INFO"),
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

      private void HistFind_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!HistFromWrhsDate_Dt.Value.HasValue)
            {
               HistFromWrhsDate_Dt.Focus();
               HistFromWrhsDate_Dt.Value = DateTime.Now;
               var day = HistFromWrhsDate_Dt.GetText("dd").ToInt32();
               if (day != 1)
                  HistFromWrhsDate_Dt.Value = HistFromWrhsDate_Dt.Value.Value.AddDays((day - 1) * -1);
            }
            if (!HistToWrhsDate_Dt.Value.HasValue) { HistToWrhsDate_Dt.Focus(); HistToWrhsDate_Dt.Value = DateTime.Now; }

            HistWrhsBs.DataSource =
               iScsc.Warehouses.Where(
                  w =>                     
                     (HistWrhsType_Cbx.Checked || (HistInvcType_Lov.EditValue == null || HistInvcType_Lov.EditValue.ToString() == "" || w.WRHS_TYPE == HistInvcType_Lov.EditValue.ToString())) &&
                     (!HistCrntEpit_Cbx.Checked || w.Warehouse_Details.Any(wd => wd.Expense.Expense_Type.EPIT_CODE == (EpitBs.Current as Data.Expense_Item).CODE)) &&
                     (!HistFromWrhsDate_Dt.Value.HasValue || HistFromWrhsDate_Dt.Value.Value.Date <= w.WRHS_DATE.Value.Date) &&
                     (!HistToWrhsDate_Dt.Value.HasValue || HistToWrhsDate_Dt.Value.Value.Date >= w.WRHS_DATE.Value.Date) &&
                     (HistWrhsNumb_Txt.EditValue == null || HistWrhsNumb_Txt.EditValue.ToString() == "" || w.WRHS_NUMB.Contains(HistWrhsNumb_Txt.Text)) &&
                     (HistWrhsSorcPost_Txt.EditValue == null || HistWrhsSorcPost_Txt.EditValue.ToString() == "" || w.SORC_POST_ADRS.Contains(HistWrhsSorcPost_Txt.Text)) &&
                     (!HistCrntUsrWrhs_Cbx.Checked || w.CRET_BY == CurrentUser) &&
                     (HistNatlCode_Txt.EditValue == null || HistNatlCode_Txt.EditValue.ToString() == "" || w.Warehouse_Details.Any(wd => wd.Fighter.NATL_CODE_DNRM == HistNatlCode_Txt.Text)) &&
                     (HistCellPhon_Txt.EditValue == null || HistCellPhon_Txt.EditValue.ToString() == "" || w.Warehouse_Details.Any(wd => wd.Fighter.CELL_PHON_DNRM == HistCellPhon_Txt.Text)) &&
                     (HistFighWrhs_Lov.EditValue == null || HistFighWrhs_Lov.EditValue.ToString() == "" || w.Warehouse_Details.Any(wd => wd.FIGH_FILE_NO == Convert.ToInt64(HistFighWrhs_Lov.EditValue))) &&
                     (HistSectWrhs_Lov.EditValue == null || HistSectWrhs_Lov.EditValue.ToString() == "" || w.Warehouse_Details.Any(wd => wd.FIGH_FILE_NO == Convert.ToInt64(HistSectWrhs_Lov.EditValue))) &&
                     w.WRHS_STAT == "002"
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void HistFighWrhs_Lov_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  HistFighWrhs_Lov.EditValue = null;
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

      private void HistSectWrhs_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  HistSectWrhs_Lov.EditValue = null;
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
