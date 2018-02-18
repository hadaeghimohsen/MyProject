using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_DFIN_F : UserControl
   {
      public BAS_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool fetchagine = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         fetchagine = true;
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if(Tb_Master.SelectedTab == tp_001)
         {
            CashBs1.DataSource = iScsc.Cashes;
         }
         else if(Tb_Master.SelectedTab == tp_002)
         {
            int income = InComeEpitBs1.Position;
            int outcome = OutComeEpitBs1.Position;
            InComeEpitBs1.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "001");
            OutComeEpitBs1.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "002");
            InComeEpitBs1.Position = income;
            OutComeEpitBs1.Position = outcome;
         }
         else if(Tb_Master.SelectedTab == tp_003)
         {
            int mtod = MtodBs1.Position;
            int ctgy = CtgyBs1.Position;
            InComeEpitBs1.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "001");
            MtodBs1.DataSource = iScsc.Methods;
            Mtod_Gv.TopRowIndex = mtod;
            MtodBs1.Position = mtod;
            Ctgy_Gv.TopRowIndex = ctgy;
            CtgyBs1.Position = ctgy;
         }
         else if(Tb_Master.SelectedTab == tp_004)
         {
            int c = CntyBs1.Position;
            int p = PrvnBs1.Position;
            int r = RegnBs1.Position;
            CntyBs1.DataSource = iScsc.Countries;
            CntyBs1.Position = c;
            PrvnBs1.Position = p;
            RegnBs1.Position = r;
         }
         else if(Tb_Master.SelectedTab == tp_005)
         {
            if (fetchagine)
            {
               CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003");
               MtodBs1.DataSource = iScsc.Methods;
               ClubBs1.DataSource = iScsc.Clubs;
               //CreateCoachMenu();
            }
            else
            {
               //CochCbmtInfo();
            }
         }
         else if(Tb_Master.SelectedTab == tp_006)
         {
            int club = ClubBs1.Position;
            int cbmt = CbmtBs2.Position;
            ClubBs1.DataSource = iScsc.Clubs;
            CochBs2.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003" && Convert.ToInt32(c.ACTV_TAG_DNRM) >= 101);
            MtodBs1.DataSource = iScsc.Methods.Where(m => m.MTOD_STAT == "002");
            ClubBs1.Position = club;
            Cbmt_Gv.TopRowIndex = cbmt;
            CbmtBs2.Position = cbmt;

            //CbmtBs1.List.Clear();
            //if (CbmtGv1.Tag != null)
            //{
               
            //   CbmtBs1.DataSource = iScsc.Club_Methods.Where(cb => cb.CLUB_CODE == ((Data.Club)CbmtGv1.Tag).CODE);
            //   Cbmt_Gv.TopRowIndex = cbmt;
            //   CbmtBs1.Position = cbmt;
            //}
            //CreateClubMenu();
         }
         else if(Tb_Master.SelectedTab == tp_007)
         {
            int hldy = HldyBs.Position;
            HldyBs.DataSource = iScsc.Holidays.Where(h => h.HLDY_DATE.Value.Year == DateTime.Now.Year && h.HLDY_DATE.Value.Date >= DateTime.Now.Date);
            HldyBs.Position = hldy;
         }

         requery = false;
      }

      #region TabPage001
      private void AddCash_Butn_Click(object sender, EventArgs e)
      {
         CashBs1.AddNew();
      }

      private void SaveCash_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = CashBs1.Current as Data.Cash;

            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "011"),
                  CashBs1.List.OfType<Data.Cash>().Where(c => c.CRET_BY == null).Select(c =>
                     new XElement("Insert",
                        new XElement("Cash",
                           new XAttribute("name", c.NAME ?? ""),
                           new XAttribute("type", c.TYPE ?? ""),
                           new XAttribute("cashstat", c.CASH_STAT ?? ""),
                           new XAttribute("bankname", c.BANK_NAME ?? ""),
                           new XAttribute("bankbrnccode", c.BANK_BRNC_CODE ?? ""),
                           new XAttribute("bankacntnumb", c.BANK_ACNT_NUMB ?? ""),
                           new XAttribute("shbaacnt", c.SHBA_ACNT ?? ""),
                           new XAttribute("cardnumb", c.CARD_NUMB ?? "")
                        )
                     )
                  ),
                  crnt.CRET_BY != null ?
                     new XElement("Update",
                        new XElement("Cash",
                           new XAttribute("code", crnt.CODE),
                           new XAttribute("name", crnt.NAME ?? ""),
                           new XAttribute("type", crnt.TYPE ?? ""),
                           new XAttribute("cashstat", crnt.CASH_STAT ?? ""),
                           new XAttribute("bankname", crnt.BANK_NAME ?? ""),
                           new XAttribute("bankbrnccode", crnt.BANK_BRNC_CODE ?? ""),
                           new XAttribute("bankacntnumb", crnt.BANK_ACNT_NUMB ?? ""),
                           new XAttribute("shbaacnt", crnt.SHBA_ACNT ?? ""),
                           new XAttribute("cardnumb", crnt.CARD_NUMB ?? "")
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
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void RemoveCash_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با پاک کردن حساب مالی موافقید؟", " حذف حساب مالی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "011"),
                  new XElement("Delete",
                     new XElement("Cash",
                        new XAttribute("code", (CashBs1.Current as Data.Cash).CODE)
                     )
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
      #endregion

      #region TabPage002
      private void AddInComeEpit_Butn_Click(object sender, EventArgs e)
      {
         InComeEpitBs1.AddNew();

         var incomeepit = InComeEpitBs1.Current as Data.Expense_Item;
         incomeepit.RQTT_CODE = "001";
         incomeepit.RQTP_CODE = "001";
      }

      private void SaveInComeEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = InComeEpitBs1.Current as Data.Expense_Item;

            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "010"),
                  InComeEpitBs1.List.OfType<Data.Expense_Item>().Where(c => c.CRET_BY == null).Select(c =>
                     new XElement("Insert",
                        new XElement("Expense_Item",
                           new XAttribute("type", c.TYPE ?? "001"),
                           new XAttribute("epitdesc", c.EPIT_DESC ?? ""),
                           new XAttribute("autognrt", c.AUTO_GNRT ?? "002"),
                           new XAttribute("rqtpcode", c.RQTP_CODE ?? ""),
                           new XAttribute("rqttcode", c.RQTT_CODE ?? "")
                        )
                     )
                  ),
                  crnt.CRET_BY != null ?
                     new XElement("Update",
                        new XElement("Expense_Item",
                           new XAttribute("code", crnt.CODE),
                           new XAttribute("type", crnt.TYPE ?? "001"),
                           new XAttribute("epitdesc", crnt.EPIT_DESC ?? ""),
                           new XAttribute("autognrt", crnt.AUTO_GNRT ?? "002"),
                           new XAttribute("rqtpcode", crnt.RQTP_CODE ?? ""),
                           new XAttribute("rqttcode", crnt.RQTT_CODE ?? "")
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
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeleteInComeEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با پاک کردن آیتم درآمد موافقید؟", "حذف آیتم درآمد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "010"),
                  new XElement("Delete",
                     new XElement("Expense_Item",
                        new XAttribute("code", (InComeEpitBs1.Current as Data.Expense_Item).CODE)
                     )
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

      private void AddOutComeEpit_Butn_Click(object sender, EventArgs e)
      {
         OutComeEpitBs1.AddNew();
      }

      private void SaveOutComeEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = OutComeEpitBs1.Current as Data.Expense_Item;

            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "010"),
                  OutComeEpitBs1.List.OfType<Data.Expense_Item>().Where(c => c.CRET_BY == null).Select(c =>
                     new XElement("Insert",
                        new XElement("Expense_Item",
                           new XAttribute("type", c.TYPE ?? "002"),
                           new XAttribute("epitdesc", c.EPIT_DESC ?? ""),
                           new XAttribute("autognrt", c.AUTO_GNRT ?? ""),
                           new XAttribute("rqtpcode", c.RQTP_CODE ?? ""),
                           new XAttribute("rqttcode", c.RQTT_CODE ?? "")
                        )
                     )
                  ),
                  crnt.CRET_BY != null ?
                     new XElement("Update",
                        new XElement("Expense_Item",
                           new XAttribute("code", crnt.CODE),
                           new XAttribute("type", crnt.TYPE ?? "002"),
                           new XAttribute("epitdesc", crnt.EPIT_DESC ?? ""),
                           new XAttribute("autognrt", crnt.AUTO_GNRT ?? ""),
                           new XAttribute("rqtpcode", crnt.RQTP_CODE ?? ""),
                           new XAttribute("rqttcode", crnt.RQTT_CODE ?? "")
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
               requery = false;
            }
         }
      }

      private void DeleteOutComeEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با پاک کردن آیتم هزینه موافقید؟", "حذف آیتم هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "010"),
                  new XElement("Delete",
                     new XElement("Expense_Item",
                        new XAttribute("code", (OutComeEpitBs1.Current as Data.Expense_Item).CODE)
                     )
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
      #endregion

      #region TabPage003
      private void AddMethod_Butn_Click(object sender, EventArgs e)
      {
         MtodBs1.AddNew();
         
         var mtod = MtodBs1.Current as Data.Method;
         mtod.DFLT_STAT = "001";
         mtod.MTOD_STAT = "002";
      }

      private void SaveMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            this.Focus();
            Validate();
            MtodBs1.EndEdit();

            iScsc.SubmitChanges();
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

      private void DeleteMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MtodBs1.Current as Data.Method;

            if (crnt != null && MessageBox.Show(this, "آیا با حذف سرگروه ورزشی موافق هستید؟", "حذف سرگروه ورزشی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Methods.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();
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

      private void AddCategory_Butn_Click(object sender, EventArgs e)
      {
         var oldctgy = CtgyBs1.Current as Data.Category_Belt;

         CtgyBs1.AddNew();
         
         var newctgy = CtgyBs1.Current as Data.Category_Belt;
         if(oldctgy == null)
         {
            newctgy.NUMB_MONT_OFER = 0;
            newctgy.NUMB_CYCL_DAY = 29;
            newctgy.NUMB_OF_ATTN_MONT = 24;
            newctgy.PRIC = 0;
            newctgy.DFLT_STAT = "001";
            newctgy.CTGY_DESC = "دوره جدید";
         }
         else
         {
            newctgy.NUMB_MONT_OFER = oldctgy.NUMB_MONT_OFER;
            newctgy.NUMB_CYCL_DAY = oldctgy.NUMB_CYCL_DAY;
            newctgy.NUMB_OF_ATTN_MONT = oldctgy.NUMB_OF_ATTN_MONT;
            newctgy.PRIC = oldctgy.PRIC;
            newctgy.CTGY_DESC = "دوره جدید";
            newctgy.DFLT_STAT = oldctgy.DFLT_STAT;
         }
         newctgy.CTGY_STAT = "002";
      }

      private void SaveCategory_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.CommandTimeout = 1800;
            this.Focus();
            Validate();
            CtgyBs1.EndEdit();

            iScsc.SubmitChanges();            
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
               SaveExpenseParam_Butn_Click(null, null);
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeleteCategory_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = CtgyBs1.Current as Data.Category_Belt;

            if (crnt != null && MessageBox.Show(this, "آیا با حذف زیر گروه ورزشی موافق هستید؟", "حذف زیر گروه ورزشی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Category_Belts.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();
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

      private void SaveExpenseParam_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mtod = MtodBs1.Current as Data.Method;
            var ctgy = CtgyBs1.Current as Data.Category_Belt;
            if(mtod == null || ctgy == null)return;

            var rqtp = ExpnRqtp_Lov.EditValue;
            var rqtt = ExpnRqtt_Lov.EditValue;
            long? epit = (long?)ExpnEpit_Lov.EditValue;

            if(rqtp == null || rqtp.ToString().Length != 3)rqtp = "";
            if(rqtt == null || rqtt.ToString().Length != 3)rqtt = "";
            if(epit == null || (long)(epit) == 0) epit = null;

            var expn =
               iScsc.Expenses.
               Where(ex => ex.Regulation.TYPE == "001"
                  && ex.Regulation.REGL_STAT == "002"
                  && (ex.Expense_Type.Request_Requester.RQTP_CODE == "001" || ex.Expense_Type.Request_Requester.RQTP_CODE == "009")
                  && ex.Expense_Type.Request_Requester.RQTT_CODE == "001"
                  && ex.Expense_Type.EPIT_CODE == epit
                  && ex.MTOD_CODE == mtod.CODE
                  && ex.CTGY_CODE == ctgy.CODE
               );

            

            if(expn.Count() > 0)
            {
               expn.ToList().ForEach(ex =>
               {
                  ex.PRIC = (int)ctgy.PRIC;
                  ex.NUMB_CYCL_DAY = ctgy.NUMB_CYCL_DAY;
                  ex.NUMB_OF_ATTN_MONT = ctgy.NUMB_OF_ATTN_MONT;
                  ex.NUMB_MONT_OFER = ctgy.NUMB_MONT_OFER;
                  ex.EXPN_STAT = ctgy.CTGY_STAT;
               });
               //MessageBox.Show(this, string.Format("تعداد رکورد ثبت شده {0} می باشد",  expn.Count()));
               iScsc.SubmitChanges();
               requery = true;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {               
               requery = false;
            }
         }
      }
      #endregion

      #region TabPage004
      private void AddCountry_Butn_Click(object sender, EventArgs e)
      {
         CntyBs1.AddNew();
      }

      private void SaveCountry_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CntyBs1.EndEdit();
            var cnty = CntyBs1.Current as Data.Country;
            if (cnty == null) return;

            if (cnty.CODE == null || cnty.CODE == "") { return; }
            if (cnty.NAME == null || cnty.NAME == "") { return; }

            //if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.SubmitChanges();
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void DeleteCountry_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cnty = CntyBs1.Current as Data.Country;
            if (cnty == null) return;

            if (cnty.CODE == null || cnty.CODE == "") { return; }
            if (cnty.NAME == null || cnty.NAME == "") { return; }

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.DEL_CNTY_P(cnty.CODE);
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void AddProvince_Butn_Click(object sender, EventArgs e)
      {
         PrvnBs1.AddNew();
      }

      private void SaveProvince_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PrvnBs1.EndEdit();
            var prvn = PrvnBs1.Current as Data.Province;
            if (prvn == null) return;

            if (prvn.CNTY_CODE == null || prvn.CNTY_CODE == "" ) {  return; }
            if (prvn.CODE == null || prvn.CODE == "" ) { return; }
            if (prvn.NAME == null || prvn.NAME == "") { return; }

            iScsc.SubmitChanges();
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void DeleteProvince_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PrvnBs1.EndEdit();
            var prvn = PrvnBs1.Current as Data.Province;
            if (prvn == null) return;

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (prvn.CNTY_CODE == null || prvn.CNTY_CODE == "") { return; }
            if (prvn.CODE == null || prvn.CODE == "") { return; }
            if (prvn.NAME == null || prvn.NAME == "") { return; }

            iScsc.DEL_PRVN_P(prvn.CNTY_CODE, prvn.CODE);
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void AddRegion_Butn_Click(object sender, EventArgs e)
      {
         RegnBs1.AddNew();
      }

      private void SaveRegion_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RegnBs1.EndEdit();
            var regn = RegnBs1.Current as Data.Region;
            if (regn == null) return;

            if (regn.PRVN_CNTY_CODE == null || regn.PRVN_CNTY_CODE == "") { return; }
            if (regn.PRVN_CODE == null || regn.PRVN_CODE == "") { return; }
            if (regn.CODE == null || regn.CODE == "") { return; }
            if (regn.NAME == null || regn.NAME == "") { return; }

            iScsc.SubmitChanges();
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void DeleteRegion_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RegnBs1.EndEdit();
            var regn = RegnBs1.Current as Data.Region;
            if (regn == null) return;

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (regn.CODE == null || regn.CODE == "") { return; }

            iScsc.DEL_REGN_P(regn.PRVN_CNTY_CODE, regn.PRVN_CODE, regn.CODE);
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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
      #endregion

      #region TabPage005
      //private void CreateCoachMenu()
      //{
      //   //NameDnrm_Lbl.Text = "";
      //   //SexType_Lbl.Text = "";
      //   //BrthDate_Lbl.Text = "";
      //   CoachList_Flp.Controls.Clear();
      //   CbmtBs1.List.Clear();
      //   foreach (Data.Fighter coch in CochBs1.List.OfType<Data.Fighter>())
      //   {
      //      var simplebutton = new SimpleButton();

      //      simplebutton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
      //      simplebutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
      //         | System.Windows.Forms.AnchorStyles.Right)));
      //      simplebutton.Appearance.BackColor = Convert.ToInt32(coch.ACTV_TAG_DNRM) <= 100 ? Color.Gainsboro : Color.SkyBlue;
      //      simplebutton.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      //      simplebutton.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      //      simplebutton.Appearance.Options.UseBackColor = true;
      //      simplebutton.Appearance.Options.UseFont = true;
      //      simplebutton.Appearance.Options.UseForeColor = true;
      //      simplebutton.Appearance.Options.UseTextOptions = true;
      //      simplebutton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
      //      simplebutton.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
      //      simplebutton.Image = coch.SEX_TYPE_DNRM == "001" ? global::System.Scsc.Properties.Resources.IMAGE_1076 : global::System.Scsc.Properties.Resources.IMAGE_1507;
      //      simplebutton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
      //      simplebutton.Location = new System.Drawing.Point(530, 3);
      //      simplebutton.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
      //      simplebutton.LookAndFeel.UseDefaultLookAndFeel = false;
      //      simplebutton.Name = "simpleButton2";
      //      simplebutton.Size = new System.Drawing.Size(169, 57);
      //      simplebutton.TabIndex = 3;
      //      simplebutton.Tag = coch;
      //      simplebutton.Text = string.Format("<b><u>{0}</u></b><br><color=DimGray><size=9>{1}</size></color><br><color=blue><size=10>{2}</size></color><br>", coch.NAME_DNRM, coch.Method.MTOD_DESC, coch.FNGR_PRNT_DNRM);
      //      simplebutton.Click += CochInfo_Click;
      //      CoachList_Flp.Controls.Add(simplebutton);
      //   }
      //}

      //Data.Fighter coch;
      //private void CochInfo_Click(object sender, EventArgs e)
      //{
      //   coch = ((SimpleButton)sender).Tag as Data.Fighter;

      //   CochCbmtInfo();
      //}

      //private void CochCbmtInfo()
      //{
      //   //try
      //   //{
      //   //   ImageProfile_Pb.ImageProfile = null;
      //   //   MemoryStream mStream = new MemoryStream();
      //   //   byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", coch.FILE_NO))).ToArray();
      //   //   mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
      //   //   Bitmap bm = new Bitmap(mStream, false);
      //   //   mStream.Dispose();

      //   //   ImageProfile_Pb.Visible = true;

      //   //   if (InvokeRequired)
      //   //      Invoke(new Action(() => ImageProfile_Pb.ImageProfile = bm));
      //   //   else
      //   //      ImageProfile_Pb.ImageProfile = bm;
      //   //}
      //   //catch
      //   //{
      //   //   ImageProfile_Pb.ImageProfile = coch.SEX_TYPE_DNRM == "001" ? global::System.Scsc.Properties.Resources.IMAGE_1076 : global::System.Scsc.Properties.Resources.IMAGE_1507;
      //   //}

      //   //NameDnrm_Lbl.Text = coch.NAME_DNRM;
      //   //SexType_Lbl.Text = coch.SEX_TYPE_DNRM == "001" ? "مربی آقایان" : "مربی خانم ها";
      //   //BrthDate_Lbl.Text = GetPersianDate(coch.BRTH_DATE_DNRM);

      //   var cbmt = CbmtBs1.Position;
      //   CbmtBs1.List.Clear();
      //   CbmtBs1.DataSource = iScsc.Club_Methods.Where(c => c.COCH_FILE_NO == coch.FILE_NO);
      //   gridView4.TopRowIndex = cbmt;
      //   CbmtBs1.Position = cbmt;
         
      //}

      private string GetPersianDate(DateTime? datetime)
      {
         PersianCalendar pc = new PersianCalendar();
         return
            string.Format("{0}/{1}/{2}",
               pc.GetYear((DateTime)datetime),
               pc.GetMonth((DateTime)datetime),
               pc.GetDayOfMonth((DateTime)datetime));
      }

      private void NewCoach_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 145 /* Execute Bas_Adch_F */, SendType.Self)
         );
      }

      #endregion

      #region TabPage006
      private void CreateClubMenu()
      {
         //Club_Flp.Controls.Clear();

         foreach (Data.Club club in ClubBs1.List.OfType<Data.Club>())
         {
            var simplebutton = new SimpleButton();

            simplebutton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            simplebutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            simplebutton.Appearance.BackColor = Color.SkyBlue;
            simplebutton.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            simplebutton.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            simplebutton.Appearance.Options.UseBackColor = true;
            simplebutton.Appearance.Options.UseFont = true;
            simplebutton.Appearance.Options.UseForeColor = true;
            simplebutton.Appearance.Options.UseTextOptions = true;
            simplebutton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            simplebutton.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            simplebutton.Image = System.Scsc.Properties.Resources.IMAGE_1122;
            simplebutton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            simplebutton.Location = new System.Drawing.Point(530, 3);
            simplebutton.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            simplebutton.LookAndFeel.UseDefaultLookAndFeel = false;
            simplebutton.Name = "simpleButton2";
            simplebutton.Size = new System.Drawing.Size(169, 57);
            simplebutton.TabIndex = 3;
            simplebutton.Tag = club;
            simplebutton.Text = string.Format("<b>{0}</b><br>", club.NAME);
            simplebutton.Click += ClubInfo_Click;
            //Club_Flp.Controls.Add(simplebutton);
         }
      }

      private void ClubInfo_Click(object sender, EventArgs e)
      {
         var club = ((SimpleButton)sender).Tag as Data.Club;
         CbmtGv1.Tag = club;
         ClubMethod_Splt.Tag = club;

         CbmtBs1.List.Clear();
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(c => c.CLUB_CODE == club.CODE);
      }

      #endregion

      private void AddClubMethod_Butn_Click(object sender, EventArgs e)
      {
         var club = ClubBs1.Current as Data.Club;
         var oldcbmt = CbmtBs2.Current as Data.Club_Method;

         CbmtBs2.AddNew();

         var newcbmt = CbmtBs2.Current as Data.Club_Method;
         
         newcbmt.CLUB_CODE = club.CODE;
         
         if(oldcbmt == null)
         {
            newcbmt.DFLT_STAT = "001";
            newcbmt.MTOD_STAT = "002";
         }
         else
         {
            //newcbmt.COCH_FILE_NO = oldcbmt.COCH_FILE_NO;
            //newcbmt.MTOD_CODE = oldcbmt.MTOD_CODE;
            newcbmt.DAY_TYPE = oldcbmt.DAY_TYPE;
            newcbmt.SEX_TYPE = oldcbmt.SEX_TYPE;
            newcbmt.MTOD_STAT = oldcbmt.MTOD_STAT;
            newcbmt.DFLT_STAT = oldcbmt.DFLT_STAT;
            newcbmt.STRT_TIME = oldcbmt.STRT_TIME;
            newcbmt.END_TIME = oldcbmt.END_TIME;
            newcbmt.CBMT_TIME = 0;
            newcbmt.CBMT_TIME_STAT = "001";
            newcbmt.CLAS_TIME = 90;
            newcbmt.CPCT_NUMB = 0;
            newcbmt.CPCT_STAT = "001";
         }
      }

      private void SaveClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) { return; }

               var club = ClubBs1.Current as Data.Club;
               if (cbmt.CLUB_CODE == null) { cbmt.CLUB_CODE = club.CODE; }

               if (cbmt.COCH_FILE_NO == null) return;
               if (cbmt.MTOD_CODE == null) return;
               if (cbmt.DAY_TYPE == null) return;

               if (cbmt.CODE == 0)
                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "005"),
                           new XElement("Insert",
                              new XElement("Club_Method",
                                 new XAttribute("clubcode", club.CODE),
                                 new XAttribute("mtodcode", cbmt.MTOD_CODE),
                                 new XAttribute("cochfileno", cbmt.COCH_FILE_NO),
                                 new XAttribute("daytype", cbmt.DAY_TYPE),
                                 new XAttribute("strttime", cbmt.STRT_TIME.ToString()),
                                 new XAttribute("endtime", cbmt.END_TIME.ToString()),
                                 new XAttribute("mtodstat", cbmt.MTOD_STAT ?? "002"),
                                 new XAttribute("sextype", cbmt.SEX_TYPE ?? "002"),
                                 new XAttribute("cbmtdesc", cbmt.CBMT_DESC ?? ""),
                                 new XAttribute("dfltstat", cbmt.DFLT_STAT ?? "001"),
                                 new XAttribute("cpctnumb", cbmt.CPCT_NUMB ?? 0),
                                 new XAttribute("cpctstat", cbmt.CPCT_STAT ?? "001"),
                                 new XAttribute("cbmttime", cbmt.CBMT_TIME ?? 0),
                                 new XAttribute("cbmttimestat", cbmt.CBMT_TIME_STAT ?? "001"),
                                 new XAttribute("clastime", cbmt.CLAS_TIME ?? 90)
                              )
                           )
                     )
                  );
               else
                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "005"),
                           new XElement("Update",
                              new XElement("Club_Method",
                                 new XAttribute("code", cbmt.CODE),
                                 new XAttribute("clubcode", club.CODE),
                                 new XAttribute("mtodcode", cbmt.MTOD_CODE),
                                 new XAttribute("cochfileno", cbmt.COCH_FILE_NO),
                                 new XAttribute("daytype", cbmt.DAY_TYPE),
                                 new XAttribute("strttime", cbmt.STRT_TIME.ToString()),
                                 new XAttribute("endtime", cbmt.END_TIME.ToString()),
                                 new XAttribute("mtodstat", cbmt.MTOD_STAT ?? "002"),
                                 new XAttribute("sextype", cbmt.SEX_TYPE ?? "002"),
                                 new XAttribute("cbmtdesc", cbmt.CBMT_DESC ?? ""),
                                 new XAttribute("dfltstat", cbmt.DFLT_STAT ?? "001"),
                                 new XAttribute("cpctnumb", cbmt.CPCT_NUMB ?? 0),
                                 new XAttribute("cpctstat", cbmt.CPCT_STAT ?? "001"),
                                 new XAttribute("cbmttime", cbmt.CBMT_TIME ?? 0),
                                 new XAttribute("cbmttimestat", cbmt.CBMT_TIME_STAT ?? "001"),
                                 new XAttribute("clastime", cbmt.CLAS_TIME ?? 90)
                              )
                           )
                     )
                  );
               requery = true;
            }
            else if(Tb_Master.SelectedTab == tp_005)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) { return; }

               if (cbmt.COCH_FILE_NO == null) return;
               if (cbmt.MTOD_CODE == null) return;
               if (cbmt.DAY_TYPE == null) return;

               iScsc.STNG_SAVE_P(
                  new XElement("Config",
                     new XAttribute("type", "005"),
                        new XElement("Update",
                           new XElement("Club_Method",
                              new XAttribute("code", cbmt.CODE),
                              new XAttribute("clubcode", cbmt.CLUB_CODE),
                              new XAttribute("mtodcode", cbmt.MTOD_CODE),
                              new XAttribute("cochfileno", cbmt.COCH_FILE_NO),
                              new XAttribute("daytype", cbmt.DAY_TYPE),
                              new XAttribute("strttime", cbmt.STRT_TIME.ToString()),
                              new XAttribute("endtime", cbmt.END_TIME.ToString()),
                              new XAttribute("mtodstat", cbmt.MTOD_STAT ?? "002"),
                              new XAttribute("sextype", cbmt.SEX_TYPE ?? "002"),
                              new XAttribute("cbmtdesc", cbmt.CBMT_DESC ?? ""),
                              new XAttribute("dfltstat", cbmt.DFLT_STAT ?? "001"),
                              new XAttribute("cpctnumb", cbmt.CPCT_NUMB ?? 0),
                              new XAttribute("cpctstat", cbmt.CPCT_STAT ?? "001"),
                              new XAttribute("cbmttime", cbmt.CBMT_TIME ?? 0),
                              new XAttribute("cbmttimestat", cbmt.CBMT_TIME_STAT ?? "001"),
                              new XAttribute("clastime", cbmt.CLAS_TIME ?? 90)
                           )
                        )
                  )
               );
               requery = true;
            }
            if (Tb_Master.SelectedTab == tp_005)
               fetchagine = false;
            else
               fetchagine = true;
            
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

      private void DeleteClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtBs1.Current as Data.Club_Method;
            if (cbmt == null) return;
            if (cbmt.CRET_BY == null)
            {
               CbmtBs1.RemoveCurrent();
               return;
            }

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                  new XElement("Delete",
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
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

      private void AddClub_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 146 /* Execute Bas_Adcl_F */, SendType.Self)
         );
      }

      private void DeleteClub_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs1.Current as Data.Club;
            if (club != null && MessageBox.Show(this, "آیا با حذف شیفت باشگاه موافق هستید؟", "حذف شیفت باشگاه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)!= DialogResult.Yes) return;

            iScsc.Clubs.DeleteOnSubmit(club);
            iScsc.SubmitChanges();
            CbmtGv1.Tag = null;

            requery = true;
         }
         catch (Exception )            
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

      private void UpdateClub_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs1.Current as Data.Club;
            if (club == null) return;

            _DefaultGateway.Gateway(               
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 146 /* Execute Bas_Adcl_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_ADCL_F",  10 /* Execute Actn_CalF_F */){Input = club}
                  }
               )
            );
         }
         catch (Exception )
         {
            throw;
         }
      }

      private void GrantUserToClub_Butn_Click(object sender, EventArgs e)
      {
         var club = ClubBs1.Current as Data.Club;
         if (club == null) return;

         _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 147 /* Execute Bas_Gruc_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_GRUC_F",  10 /* Execute Actn_CalF_F */){Input = club}
                  }
               )
            );
      }

      private void Regulation_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         /// Must Be Change
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
                              "<Privilege>2</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 03 /* Execute Mstr_Regl_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void WeekDay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;
            if (cbmt.CRET_BY == null) { MessageBox.Show("لطفا اطلاعات برنامه کلاسی رو ذخیره کنید"); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 149 /* Execute Bas_Wkdy_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_WKDY_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("code", cbmt.CODE))}
                  }
               )
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }         
      }

      #region PrintButn006
      private void PrintDefaultClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (!FromDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate006_Date.Focus(); return; }
            //if (!ToDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate006_Date.Focus(); return; }

            var crnt = CbmtBs2.Current as Data.Club_Method;
            if (crnt == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */)
                  {
                     Input = 
                        new XElement("Print", 
                           new XAttribute("type", "Default"), 
                           new XAttribute("modual", GetType().Name), 
                           new XAttribute("section", GetType().Name.Substring(0,3) + "_006_F"), 
                           //string.Format("<Club_Method code=\"{0}\" /><Request fromsavedate=\"{1}\" tosavedate=\"{2}\" />", crnt.CODE, FromDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd") )
                           string.Format("<Club_Method code=\"{0}\" />",crnt.CODE )
                        )
                  }
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (!FromDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate006_Date.Focus(); return; }
            //if (!ToDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate006_Date.Focus(); return; }

            var crnt = CbmtBs2.Current as Data.Club_Method;
            if (crnt == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selection"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_006_F"),
                              //string.Format("<Club_Method code=\"{0}\" /><Request fromsavedate=\"{1}\" tosavedate=\"{2}\" />",crnt.CODE , FromDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd") )
                              string.Format("<Club_Method code=\"{0}\" />",crnt.CODE )
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintSettingClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("type", "ModualReport"), 
                              new XAttribute("modul", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_006_F")
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }
      #endregion

      private void CbmtActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               //CbmtGv1.Tag = cbmt.Club;

               switch (e.Button.Index)
               {
                  case 0:
                     cbmt.MTOD_STAT = cbmt.MTOD_STAT == "002" ? "001" : "002";
                     SaveClubMethod_Butn_Click(null, null);
                     break;
                  case 1:
                     SaveClubMethod_Butn_Click(null, null);
                     break;
                  case 2:
                     WeekDay_Butn_Click(null, null);
                     break;
                  default:
                     break;
               }
            }
            else if(Tb_Master.SelectedTab == tp_005)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               //CbmtGv1.Tag = cbmt.Club;

               switch (e.Button.Index)
               {
                  case 0:
                     cbmt.MTOD_STAT = cbmt.MTOD_STAT == "002" ? "001" : "002";
                     SaveClubMethod_Butn_Click(null, null);
                     break;
                  case 1:
                     SaveClubMethod_Butn_Click(null, null);
                     break;
                  case 2:
                     WeekDay_Butn_Click(null, null);
                     break;
                  default:
                     break;
               }
            }
         }
         catch (Exception )
         {

         }
      }

      private void MtodActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var mtod = MtodBs1.Current as Data.Method;
            if (mtod == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  mtod.MTOD_STAT = mtod.MTOD_STAT == "002" ? "001" : "002";
                  SaveMethod_Butn_Click(null, null);
                  break;
               default:
                  break;
            }
            requery = true;
         }
         catch (Exception )
         {

         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void SetCbmtDesc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CbmtBs2.List.OfType<Data.Club_Method>().ToList().ForEach(x =>
               {
                  x.CBMT_DESC = "";
                  foreach (var item in (from w in x.Club_Method_Weekdays.Where(w => w.STAT == "002")
                                 join d in iScsc.D_WKDies on w.WEEK_DAY equals d.VALU
                                 select d.DOMN_DESC))
	               {
                     x.CBMT_DESC = x.CBMT_DESC + item + ", ";
	               }                  
               });
            CbmtBs2.EndEdit();
            iScsc.SubmitChanges();
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
            }
         }
      }

      private void PrintDefaultCoch_But_Click(object sender, EventArgs e)
      {
         try
         {
            //if (!FromDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate006_Date.Focus(); return; }
            //if (!ToDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate006_Date.Focus(); return; }

            var crnt = CbmtBs1.Current as Data.Club_Method;
            if (crnt == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */)
                  {
                     Input = 
                        new XElement("Print", 
                           new XAttribute("type", "Default"), 
                           new XAttribute("modual", GetType().Name), 
                           new XAttribute("section", GetType().Name.Substring(0,3) + "_005_F"), 
                           //string.Format("<Club_Method code=\"{0}\" /><Request fromsavedate=\"{1}\" tosavedate=\"{2}\" />", crnt.CODE, FromDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd") )
                           string.Format("<Club_Method cochfileno=\"{0}\" />",crnt.COCH_FILE_NO )
                        )
                  }
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintCoch_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (!FromDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromDate006_Date.Focus(); return; }
            //if (!ToDate006_Date.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToDate006_Date.Focus(); return; }

            var crnt = CbmtBs1.Current as Data.Club_Method;
            if (crnt == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selection"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_005_F"),
                              //string.Format("<Club_Method code=\"{0}\" /><Request fromsavedate=\"{1}\" tosavedate=\"{2}\" />",crnt.CODE , FromDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd") )
                              string.Format("<Club_Method cochfileno=\"{0}\" />",crnt.COCH_FILE_NO )
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintSettingCoch_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("type", "ModualReport"), 
                              new XAttribute("modul", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_005_F")
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void CochInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var coch = CochBs1.Current as Data.Fighter;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", coch.FILE_NO)) }
            );
         }
         catch { }
      }

      private void CochInfo_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         CochInfo_Butn_Click(null, null);
      }

      private void SaveFriday_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.SAVE_FRDY_P();
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
            }
         }
      }

      private void AddHldy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (HldyBs.List.OfType<Data.Holiday>().Any(h => h.CODE == 0)) return;

            HldyBs.AddNew();
            var hldy = HldyBs.Current as Data.Holiday;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveHldy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            HldyBs.EndEdit();
            Hldy_gv.PostEditor();

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
            }
         }
      }

      private void DelHldy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            HldyBs.EndEdit();
            Hldy_gv.PostEditor();

            var hldy = HldyBs.Current as Data.Holiday;
            if (hldy == null || hldy.CODE == 0) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.Holidays WHERE Code = {0}", hldy.CODE));
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

      private void AcptDuplClass_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sorccochfileno = CochBs1.Current as Data.Fighter;
            if (sorccochfileno == null) return;

            var trgtcochfileno = DupCoch_Lov.EditValue;
            if (trgtcochfileno == null || trgtcochfileno.ToString() == "") return;

            var trgtclubcode = Club_Lov.EditValue;
            if (trgtclubcode == null || trgtclubcode.ToString() == "") return;

            iScsc.DUP_CSCC_P(
               new XElement("Duplicate",
                  new XAttribute("sorccochfileno", sorccochfileno.FILE_NO),
                  new XAttribute("trgtcochfileno", trgtcochfileno),
                  new XAttribute("trgtclubcode", trgtclubcode)
               )
            );

            DuplicateClass_Pn.Visible = false;
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
            }
         }
      }

      private void CnclDuplClas_Butn_Click(object sender, EventArgs e)
      {
         DuplicateClass_Pn.Visible = false;
      }

      private void DuplicateClass_Butn_Click(object sender, EventArgs e)
      {
         DupCoch_Lov.EditValue = null;
         Club_Lov.EditValue = null;
         DuplicateClass_Pn.Visible = !DuplicateClass_Pn.Visible;
      }
   }
}
