﻿using System;
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
using System.Data.Linq;
using System.Diagnostics;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

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
         if (Tb_Master.SelectedTab == tp_001)
         {
            CashBs1.DataSource = iScsc.Cashes;
         }
         else if (Tb_Master.SelectedTab == tp_002)
         {
            int income = InComeEpitBs1.Position;
            int outcome = OutComeEpitBs1.Position;
            InComeEpitBs1.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "001");
            OutComeEpitBs1.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "002");
            InComeEpitBs1.Position = income;
            OutComeEpitBs1.Position = outcome;
         }
         else if (Tb_Master.SelectedTab == tp_003)
         {
            int mtod = MtodBs1.Position;
            int ctgy = CtgyBs1.Position;
            InComeEpitBs1.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "001");
            if (InComeEpitBs1.List.OfType<Data.Expense_Item>().Any(ei => ei.EPIT_DESC.Contains("شهریه")))
               ExpnEpit_Lov.EditValue = InComeEpitBs1.List.OfType<Data.Expense_Item>().FirstOrDefault(ei => ei.EPIT_DESC.Contains("شهریه")).CODE;

            MtodBs1.DataSource = iScsc.Methods;
            Mtod_Gv.TopRowIndex = mtod;
            MtodBs1.Position = mtod;
            Ctgy_Gv.TopRowIndex = ctgy;
            CtgyBs1.Position = ctgy;

            ClubBs1.DataSource = iScsc.Clubs;
            CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003");
         }
         else if (Tb_Master.SelectedTab == tp_004)
         {
            int c = CntyBs1.Position;
            int p = PrvnBs1.Position;
            int r = RegnBs1.Position;
            int d = CndoBs1.Position;
            int b = CblkBs1.Position;
            int u = CuntBs1.Position;
            CntyBs1.DataSource = iScsc.Countries;
            CntyBs1.Position = c;
            PrvnBs1.Position = p;
            RegnBs1.Position = r;
            CndoBs1.Position = d;
            CblkBs1.Position = b;
            CuntBs1.Position = u;
         }
         else if (Tb_Master.SelectedTab == tp_005)
         {
            if (fetchagine)
            {
               int cbmt = CbmtBs1.Position;
               CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003");
               CbmtBs1.Position = cbmt;
               MtodBs1.DataSource = iScsc.Methods;
               ClubBs1.DataSource = iScsc.Clubs;
               //CreateCoachMenu();
            }
            else
            {
               //CochCbmtInfo();
            }
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            int club = ClubBs1.Position;
            int cbmt = CbmtBs2.Position;
            ClubBs1.DataSource = iScsc.Clubs;
            CochBs2.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003" && Convert.ToInt32(c.ACTV_TAG_DNRM) >= 101);
            MtodBs1.DataSource = iScsc.Methods.Where(m => m.MTOD_STAT == "002");
            ClubBs1.Position = club;
            //Cbmt_Gv.TopRowIndex = cbmt;
            CbmtBs2.Position = cbmt;
         }
         else if (Tb_Master.SelectedTab == tp_007)
         {
            int hldy = HldyBs.Position;
            HldyBs.DataSource = iScsc.Holidays.Where(h => h.HLDY_DATE.Value.Date >= DateTime.Now.Date);
            HldyBs.Position = hldy;
         }
         else if (Tb_Master.SelectedTab == tp_008)
         {
            int coma = ComaBs.Position;
            ComaBs.DataSource = iScsc.Computer_Actions;
            ComaBs.Position = coma;

            MtodBs1.DataSource = iScsc.Methods;
         }
         else if (Tb_Master.SelectedTab == tp_009)
         {
            ClubBs3.DataSource = iScsc.Clubs;
         }
         else if (Tb_Master.SelectedTab == tp_010)
         {
            int exdv = ExdvBs.Position;
            ExdvBs.DataSource = iScsc.External_Devices;
            ExdvBs.Position = exdv;

            DDevcBs.DataSource = iScsc.D_DEVCs;
            DConnBs.DataSource = iScsc.D_CONNs;
            DCycrBs.DataSource = iScsc.D_CYCRs;
            MtodBs1.DataSource = iScsc.Methods.Where(m => m.MTOD_STAT == "002");

            ExpnBs.DataSource =
            iScsc.Expenses.Where(ex =>
               ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
               ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                  //ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
               ex.EXPN_STAT == "002" /* هزینه های فعال */
            );
         }
         else if (Tb_Master.SelectedTab == tp_011)
         {
            TmplBs.DataSource = iScsc.Templates;
            TmpiBs.DataSource = iScsc.Template_Items.Where(ti => ti.RECD_STAT == "002");
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
            if (requery)
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
            if (requery)
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
         if (InComeEpitBs1.List.OfType<Data.Expense_Item>().Any(ei => ei.CODE == 0)) return;

         InComeEpitBs1.AddNew();

         var incomeepit = InComeEpitBs1.Current as Data.Expense_Item;
         incomeepit.RQTT_CODE = "001";
         incomeepit.RQTP_CODE = "001";
         incomeepit.TYPE = "001";
         incomeepit.AUTO_GNRT = "002";

         iScsc.Expense_Items.InsertOnSubmit(incomeepit);
      }

      private void SaveInComeEpit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = InComeEpitBs1.Current as Data.Expense_Item;

            InComeEpitBs1.EndEdit();
            incepit_gv.PostEditor();
            //if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            /*iScsc.STNG_SAVE_P(
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
            );*/
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
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void AddOutComeEpit_Butn_Click(object sender, EventArgs e)
      {
         if (OutComeEpitBs1.List.OfType<Data.Expense_Item>().Any(ei => ei.CODE == 0)) return;

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
         if (MtodBs1.List.OfType<Data.Method>().Any(m => m.CODE == 0)) return;

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

            if (crnt != null && MessageBox.Show(this, "آیا با حذف سرگروه موافق هستید؟", "حذف سرگروه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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

      private void ActvMtodExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long? epit = (long?)ExpnEpit_Lov.EditValue;
            if (epit == null || (long)(epit) == 0) { ExpnEpit_Lov.Focus(); return; }

            var expn =
               iScsc.Expenses.
               Where(ex => ex.Regulation.TYPE == "001"
                  && ex.Regulation.REGL_STAT == "002"
                  && (ex.Expense_Type.Request_Requester.RQTP_CODE == "001" || ex.Expense_Type.Request_Requester.RQTP_CODE == "009")
                  && ex.Expense_Type.Request_Requester.RQTT_CODE == "001"
                  && ex.Expense_Type.EPIT_CODE == epit
                  && ex.Method.MTOD_STAT == "002"
               //&& ex.MTOD_CODE == mtod.CODE
               //&& ex.CTGY_CODE == ctgy.CODE
               );

            if (expn.Count() > 0)
            {
               expn.ToList().ForEach(ex =>
               {
                  var ctgy = iScsc.Category_Belts.FirstOrDefault(cb => cb.CODE == ex.CTGY_CODE);
                  ex.PRIC = (int)ctgy.PRIC;
                  ex.NUMB_CYCL_DAY = ctgy.NUMB_CYCL_DAY;
                  ex.NUMB_OF_ATTN_MONT = ctgy.NUMB_OF_ATTN_MONT;
                  ex.NUMB_MONT_OFER = ctgy.NUMB_MONT_OFER;
                  ex.EXPN_STAT = ctgy.CTGY_STAT;
               });

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
            if (requery)
            {
               requery = false;
            }
         }
      }

      private void AddCategory_Butn_Click(object sender, EventArgs e)
      {
         var mtod = MtodBs1.Current as Data.Method;
         if (mtod.CODE == 0) return;

         if (CtgyBs1.List.OfType<Data.Category_Belt>().Any(cb => cb.CODE == 0)) return;

         var oldctgy = CtgyBs1.Current as Data.Category_Belt;

         CtgyBs1.AddNew();

         var newctgy = CtgyBs1.Current as Data.Category_Belt;
         if (oldctgy == null)
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
            iScsc.CommandTimeout = int.MaxValue;
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
            if (requery)
            {
               ActvCtgyExpn_Butn_Click(null, null);
               //SaveExpenseParam_Butn_Click(null, null);
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

            if (crnt != null && MessageBox.Show(this, "آیا با حذف زیر گروه موافق هستید؟", "حذف زیر گروه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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
            if (mtod == null || ctgy == null) return;

            var rqtp = ExpnRqtp_Lov.EditValue;
            var rqtt = ExpnRqtt_Lov.EditValue;
            long? epit = (long?)ExpnEpit_Lov.EditValue;

            if (rqtp == null || rqtp.ToString().Length != 3) rqtp = "";
            if (rqtt == null || rqtt.ToString().Length != 3) rqtt = "";
            if (epit == null || (long)(epit) == 0) epit = null;

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



            if (expn.Count() > 0)
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
            if (requery)
            {
               requery = false;
            }
         }
      }

      private void ActvCtgyExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mtod = MtodBs1.Current as Data.Method;
            if (mtod == null) return;
            long? epit = (long?)ExpnEpit_Lov.EditValue;
            if (epit == null || (long)(epit) == 0) { ExpnEpit_Lov.Focus(); return; }

            var expn =
               iScsc.Expenses.
               Where(ex => ex.Regulation.TYPE == "001"
                  && ex.Regulation.REGL_STAT == "002"
                  && (ex.Expense_Type.Request_Requester.RQTP_CODE == "001" || ex.Expense_Type.Request_Requester.RQTP_CODE == "009")
                  && ex.Expense_Type.Request_Requester.RQTT_CODE == "001"
                  && ex.Expense_Type.EPIT_CODE == epit
                  && ex.MTOD_CODE == mtod.CODE
               //&& ex.CTGY_CODE == ctgy.CODE
               );

            if (expn.Count() > 0)
            {
               expn.ToList().ForEach(ex =>
               {
                  var ctgy = iScsc.Category_Belts.FirstOrDefault(cb => cb.CODE == ex.CTGY_CODE);
                  ex.PRIC = (long)(ctgy.PRIC ?? 0);
                  ex.NUMB_CYCL_DAY = ctgy.NUMB_CYCL_DAY ?? 0;
                  ex.NUMB_OF_ATTN_MONT = ctgy.NUMB_OF_ATTN_MONT ?? 0;
                  ex.NUMB_MONT_OFER = ctgy.NUMB_MONT_OFER ?? 0;
                  ex.EXPN_STAT = ctgy.CTGY_STAT ?? "002";
               });

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
            if (requery)
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

            if (prvn.CNTY_CODE == null || prvn.CNTY_CODE == "") { return; }
            if (prvn.CODE == null || prvn.CODE == "") { return; }
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
         try
         {
            if (CbmtBs2.List.OfType<Data.Club_Method>().Any(cm => cm.CODE == 0)) return;

            var club = ClubBs1.Current as Data.Club;
            var oldcbmt = CbmtBs2.Current as Data.Club_Method;

            CbmtBs2.AddNew();

            var newcbmt = CbmtBs2.Current as Data.Club_Method;

            newcbmt.CLUB_CODE = club.CODE;

            if (oldcbmt == null)
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
               newcbmt.AMNT = 0;
               newcbmt.CPCT_STAT = oldcbmt.CPCT_STAT;
               newcbmt.CPCT_NUMB = oldcbmt.CPCT_NUMB;
               newcbmt.CBMT_DESC = oldcbmt.CBMT_DESC;
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void SaveClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Tb_Master.SelectedTab == tp_006)
            {
               CbmtBs2.EndEdit();
               Cbmt_Gv.PostEditor();

               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (cbmt.CODE == 0)
                  iScsc.INS_CBMT_P(cbmt.CLUB_CODE, cbmt.MTOD_CODE, cbmt.COCH_FILE_NO, cbmt.DAY_TYPE, cbmt.STRT_TIME, cbmt.END_TIME, cbmt.SEX_TYPE, cbmt.CBMT_DESC, cbmt.DFLT_STAT, cbmt.CPCT_NUMB, cbmt.CPCT_STAT, cbmt.CBMT_TIME, cbmt.CBMT_TIME_STAT, cbmt.CLAS_TIME, cbmt.AMNT, cbmt.NATL_CODE, cbmt.DURT_ATTN_SOND_PATH);
               else
                  iScsc.UPD_CBMT_P(cbmt.CODE, cbmt.CLUB_CODE, cbmt.MTOD_CODE, cbmt.COCH_FILE_NO, cbmt.MTOD_STAT, cbmt.DAY_TYPE, cbmt.STRT_TIME, cbmt.END_TIME, cbmt.SEX_TYPE, cbmt.CBMT_DESC, cbmt.DFLT_STAT, cbmt.CPCT_NUMB, cbmt.CPCT_STAT, cbmt.CBMT_TIME, cbmt.CBMT_TIME_STAT, cbmt.CLAS_TIME, cbmt.AMNT, cbmt.NATL_CODE, cbmt.DURT_ATTN_SOND_PATH);
               requery = true;
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               CbmtBs1.EndEdit();
               CbmtCoch_Gv.PostEditor();  
               //iScsc.SubmitChanges();
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;
               iScsc.UPD_CBMT_P(cbmt.CODE, cbmt.CLUB_CODE, cbmt.MTOD_CODE, cbmt.COCH_FILE_NO, cbmt.MTOD_STAT, cbmt.DAY_TYPE, cbmt.STRT_TIME, cbmt.END_TIME, cbmt.SEX_TYPE, cbmt.CBMT_DESC, cbmt.DFLT_STAT, cbmt.CPCT_NUMB, cbmt.CPCT_STAT, cbmt.CBMT_TIME, cbmt.CBMT_TIME_STAT, cbmt.CLAS_TIME, cbmt.AMNT, cbmt.NATL_CODE, cbmt.DURT_ATTN_SOND_PATH);
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
            var cbmt = CbmtBs2.Current as Data.Club_Method;
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
            if (club != null && MessageBox.Show(this, "آیا با حذف شیفت باشگاه موافق هستید؟", "حذف شیفت باشگاه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Clubs.DeleteOnSubmit(club);
            iScsc.SubmitChanges();
            CbmtGv1.Tag = null;

            requery = true;
         }
         catch (Exception)
         {

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
         catch (Exception)
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

            Data.Club_Method crnt = null;
            var section = "";
            if (Tb_Master.SelectedTab == tp_006)
            {
               crnt = CbmtBs2.Current as Data.Club_Method;
               section = GetType().Name.Substring(0, 3) + "_006_F";
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               crnt = CbmtBs1.Current as Data.Club_Method;
               section = GetType().Name.Substring(0, 3) + "_005_F";
            }

            if (crnt == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Default"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", section),
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

            Data.Club_Method crnt = null;
            var section = "";
            if (Tb_Master.SelectedTab == tp_006)
            {
               crnt = CbmtBs2.Current as Data.Club_Method;
               section = GetType().Name.Substring(0, 3) + "_006_F";
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               crnt = CbmtBs1.Current as Data.Club_Method;
               section = GetType().Name.Substring(0, 3) + "_005_F";
            }

            if (crnt == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selection"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", section),
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
            var section = "";
            if (Tb_Master.SelectedTab == tp_006)
            {
               section = GetType().Name.Substring(0, 3) + "_006_F";
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               section = GetType().Name.Substring(0, 3) + "_005_F";
            }

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
                              new XAttribute("section", section)
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
                     PrintDefaultClubMethod_Butn_Click(null, null);
                     break;
                  default:
                     break;
               }
            }
            else if (Tb_Master.SelectedTab == tp_005)
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
                     //WeekDay_Butn_Click(null, null);
                     PrintDefaultClubMethod_Butn_Click(null, null);
                     break;
                  case 3:
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
                     break;
                  default:
                     break;
               }
            }
         }
         catch (Exception)
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
               case 1:
                  mtod.SHOW_STAT = mtod.SHOW_STAT == "002" ? "001" : "002";
                  SaveMethod_Butn_Click(null, null);
                  break;
               case 2:
                  long? epit = (long?)ExpnEpit_Lov.EditValue;
                  if (epit == null || (long)(epit) == 0) { ExpnEpit_Lov.Focus(); return; }

                  var expn =
                     iScsc.Expenses.
                     Where(ex => ex.Regulation.TYPE == "001"
                        && ex.Regulation.REGL_STAT == "002"
                        && ex.Expense_Type.Request_Requester.RQTP_CODE == "016"
                        && ex.Expense_Type.Request_Requester.RQTT_CODE == "001"
                        && ex.Expense_Type.EPIT_CODE == epit
                        && ex.MTOD_CODE == mtod.CODE                        
                     );

                  if (expn.Count() > 0)
                  {

                     expn.ToList().ForEach(ex =>
                     {
                        var ctgy = iScsc.Category_Belts.FirstOrDefault(cb => cb.CODE == ex.CTGY_CODE);
                        ex.PRIC = (long)(ctgy.PRIC ?? 0);
                        ex.NUMB_CYCL_DAY = ctgy.NUMB_CYCL_DAY ?? 0;
                        ex.NUMB_OF_ATTN_MONT = ctgy.NUMB_OF_ATTN_MONT ?? 0;
                        ex.NUMB_MONT_OFER = ctgy.NUMB_MONT_OFER ?? 0;
                        ex.EXPN_STAT = ctgy.CTGY_STAT ?? "002";
                        ex.EXPN_DESC = mtod.MTOD_DESC + ", " + ctgy.CTGY_DESC;
                     });

                     iScsc.SubmitChanges();
                  }
                  break;
               default:
                  break;
            }
            requery = true;
         }
         catch (Exception)
         {

         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void CtgyActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var ctgy = CtgyBs1.Current as Data.Category_Belt;
            if (ctgy == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  ctgy.CTGY_STAT = ctgy.CTGY_STAT == "002" ? "001" : "002";
                  SaveCategory_Butn_Click(null, null);
                  break;
               case 1:
                  ctgy.SHOW_STAT = ctgy.SHOW_STAT == "002" ? "001" : "002";
                  SaveCategory_Butn_Click(null, null);
                  break;
               case 2:
                  long? epit = (long?)ExpnEpit_Lov.EditValue;
                  if (epit == null || (long)(epit) == 0) { ExpnEpit_Lov.Focus(); return; }

                  var expn =
                     iScsc.Expenses.
                     Where(ex => ex.Regulation.TYPE == "001"
                        && ex.Regulation.REGL_STAT == "002"
                        && ex.Expense_Type.Request_Requester.RQTP_CODE == "016"
                        && ex.Expense_Type.Request_Requester.RQTT_CODE == "001"
                        && ex.Expense_Type.EPIT_CODE == epit
                        && ex.MTOD_CODE == ctgy.MTOD_CODE
                        && ex.CTGY_CODE == ctgy.CODE
                     );

                  if (expn.Count() > 0)
                  {
                     expn.ToList().ForEach(ex =>
                     {
                        //var ctgy = iScsc.Category_Belts.FirstOrDefault(cb => cb.CODE == ex.CTGY_CODE);
                        ex.PRIC = (int)ctgy.PRIC;
                        ex.NUMB_CYCL_DAY = ctgy.NUMB_CYCL_DAY;
                        ex.NUMB_OF_ATTN_MONT = ctgy.NUMB_OF_ATTN_MONT;
                        ex.NUMB_MONT_OFER = ctgy.NUMB_MONT_OFER;
                        ex.EXPN_STAT = "002";
                        ex.EXPN_DESC = ctgy.CTGY_DESC;
                     });

                     iScsc.SubmitChanges();
                  }
                  break;
               default:
                  break;
            }
            requery = true;
         }
         catch (Exception)
         {

         }
         finally
         {
            if (requery)
               Execute_Query();
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
            if (requery)
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

            //var crnt = CbmtBs1.Current as Data.Club_Method;
            var crnt = CochBs1.Current as Data.Fighter;
            if (crnt == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                  {
                     Input = 
                        new XElement("Print", 
                           new XAttribute("type", "Default"), 
                           new XAttribute("modual", GetType().Name), 
                           new XAttribute("section", GetType().Name.Substring(0,3) + "_005_F"), 
                           //string.Format("<Club_Method code=\"{0}\" /><Request fromsavedate=\"{1}\" tosavedate=\"{2}\" />", crnt.CODE, FromDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd") )
                           string.Format("<Club_Method cochfileno=\"{0}\" />",crnt.FILE_NO )
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

            //var crnt = CbmtBs1.Current as Data.Club_Method;
            var crnt = CochBs1.Current as Data.Fighter;
            if (crnt == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selection"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_005_F"),
                              //string.Format("<Club_Method code=\"{0}\" /><Request fromsavedate=\"{1}\" tosavedate=\"{2}\" />",crnt.CODE , FromDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToDate006_Date.Value.Value.Date.ToString("yyyy-MM-dd") )
                              string.Format("<Club_Method cochfileno=\"{0}\" />",crnt.FILE_NO )
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
            if (requery)
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
                              "<Privilege>230</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                              {
                                 iScsc.ExecuteCommand(string.Format("DELETE dbo.Holidays WHERE Code = {0}", hldy.CODE));
                                 requery = true;
                                 return;
                              }
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 230 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),                  
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
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
            if (requery)
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

      private void CochBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            if (Tb_Master.SelectedTab == tp_005)
            {
               RqstBnCochName_Butn.Text = coch.NAME_DNRM;
               RqstBnCochName_Butn.Text += " ( " + coch.FNGR_PRNT_DNRM + " )";
            }

            switch (coch.ACTV_TAG_DNRM)
            {
               case "101":
                  DelUnDelCoch_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1609;
                  break;
               case "001":
                  DelUnDelCoch_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1608;
                  break;
            }

            tb_cbmt1_SelectedIndexChanged(null, null);

            try
            {
               CochProFile1_Rb.ImageVisiable = true;
               CochProFile1_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", coch.FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => CochProFile1_Rb.ImageProfile = bm));
               else
                  CochProFile1_Rb.ImageProfile = bm;
            }
            catch
            { //Pb_FighImg.Visible = false;
               CochProFile1_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
            }
         }
         catch (Exception exc) { }
      }

      private void DelUnDelCoch_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            switch (coch.ACTV_TAG_DNRM)
            {
               case "101":
                  if (MessageBox.Show(this, "آیا با حذف مربی موافق هستید؟", "عملیات حذف موقت مربی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", coch.FILE_NO), new XAttribute("auto", "true"))},
                           //new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute Load_Data */){Input = new XElement("LoadData", new XAttribute("requery", "1"))},
                        })
                  );
                  break;
               case "001":
                  if (MessageBox.Show(this, "آیا با بازیابی مربی موافق هستید؟", "عملیات بازیابی مربی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                  // 1396/09/04 * بازیابی کد انگشتی یا کارتی مشتری
                  //var fighhist = iScsc.Fighter_Publics.Where(fp => fp.FIGH_FILE_NO == coch.FILE_NO && fp.RECT_CODE == "004" && (fp.FNGR_PRNT ?? "") != "").OrderByDescending(fp => fp.RWNO).FirstOrDefault();
                  //if (fighhist != null && MessageBox.Show(this, string.Format("آخرین وضعیت کد انگشتی یا کارت مربی {0} می باشد آیا مایل به جای گیزینی مجدد هستید؟", fighhist.FNGR_PRNT), "بازیابی کد انگشتی یا کارت مربی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                  //   fighhist.FNGR_PRNT = "";

                  //if(fighhist.FNGR_PRNT == "" && MessageBox.Show(this, "آیا می خواهید که کد انگشتی یا کارت جدیدی به مربی اختصاص دهید", "الحاق انگشتی یا کارت جدید به مربی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                  //{
                  //getfngrprnt:
                  //   fighhist.FNGR_PRNT = Microsoft.VisualBasic.Interaction.InputBox("EnrollNumber", "Input EnrollNumber");
                  //if (fighhist.FNGR_PRNT == "")
                  //   goto getfngrprnt;
                  //}

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_dsen_f"},
                           new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 02 /* Execute Set */),
                           new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 07 /* Execute Load_Data */),                        
                           new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", coch.FILE_NO), new XAttribute("auto", "true"), new XAttribute("fngrprnt", ""))},
                           //new Job(SendType.SelfToUserInterface, "LSI_FDLF_F", 07 /* Execute Load_Data */){Input = new XElement("LoadData", new XAttribute("requery", "1"))},
                        })
                  );
                  break;
               default:
                  break;
            }
            requery = true;
         }
         catch (Exception exc) { }
         finally
         {
            if (requery)
               Execute_Query();
         }

      }

      private void AddComa_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ComaBs.List.OfType<Data.Computer_Action>().Any(ca => ca.CODE == 0)) return;

            ComaBs.AddNew();
            var coma = ComaBs.Current as Data.Computer_Action;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveComa_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ComaBs.EndEdit();
            Coma_Gv.PostEditor();

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
                                 "<Privilege>234</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    //iScsc.ExecuteCommand(string.Format("DELETE dbo.Computer_Action WHERE Code = {0}", coma.CODE));
                                    iScsc.SubmitChanges();
                                    requery = true;
                                    return;
                                 }
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 234 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),                  
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

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

      private void DelComa_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ComaBs.EndEdit();
            Coma_Gv.PostEditor();

            var coma = ComaBs.Current as Data.Computer_Action;
            if (coma == null || coma.CODE == 0) return;

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
                                 "<Privilege>235</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    iScsc.ExecuteCommand(string.Format("DELETE dbo.Computer_Action WHERE Code = {0}", coma.CODE));
                                    requery = true;
                                    return;
                                 }
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 235 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),                  
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
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

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            ComaBs.EndEdit();
            Coma_Gv.PostEditor();

            var coma = ComaBs.Current as Data.Computer_Action;
            if (coma == null || coma.CODE == 0) return;

            switch (e.Button.Index)
            {
               case 0:
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
                                       "<Privilege>234</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                       {
                                          iScsc.ExecuteCommand(string.Format("Update dbo.Computer_Action SET Mtod_Code = NULL WHERE Code = {0}", coma.CODE));
                                          requery = true;
                                          return;
                                       }
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 234 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),                  
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  break;
               case 1:
                  if(coma.ANY_DESK_PATH != "")
                  {
                     ProcessStartInfo startInfo = new ProcessStartInfo();
                     startInfo.FileName = coma.ANY_DESK_PATH;
                     startInfo.Arguments = string.Format("{0} --with-password", coma.ANY_DESK_ID/*, (coma.ANY_DESK_PSWD == null || coma.ANY_DESK_PSWD.Length == 0 ? "" : "--pasword " + coma.ANY_DESK_PSWD)*/);
                     startInfo.UseShellExecute = false;
                     startInfo.RedirectStandardInput = true;

                     Process process = new Process();
                     process.StartInfo = startInfo;
                     process.Start();

                     process.StandardInput.WriteLine("password");
                     process.StandardInput.Flush();
                     process.StandardInput.Close();
                  }
                  break;
               case 2:
                  if (coma.ANY_DESK_PATH != "")
                  {
                     ProcessStartInfo startInfo = new ProcessStartInfo();
                     startInfo.FileName = coma.ANY_DESK_PATH;
                     startInfo.Arguments = string.Format("{0} --with-password", coma.IP_ADRS);
                     startInfo.UseShellExecute = false;
                     startInfo.RedirectStandardInput = true;

                     Process process = new Process();
                     process.StartInfo = startInfo;
                     process.Start();

                     process.StandardInput.WriteLine("password");
                     process.StandardInput.Flush();
                     process.StandardInput.Close();
                  }
                  break;
               case 3:
                  Process.Start("explorer.exe", @"\\" + coma.IP_ADRS);
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
            }
         }
      }

      private void CbmtBs2_CurrentChanged(object sender, EventArgs e)
      {
         var cbmt = CbmtBs2.Current as Data.Club_Method;
         if (cbmt == null) return;

         if (Tb_Master.SelectedTab == tp_006)
         {
            CbmtwkdyBs1.DataSource = cbmt.Club_Method_Weekdays.ToList();

            if (CbmtwkdyBs1.List.Count == 0)
            {
               ClubWkdy_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }

            foreach (var wkdy in CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>())
            {
               var rslt = ClubWkdy_Spn.Panel2.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
               rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
            }

            CochName_Lb.Text = cbmt.Fighter.NAME_DNRM;
            FngrPrnt_Lb.Text = cbmt.Fighter.FNGR_PRNT_DNRM == "" ? "نامشخص" : cbmt.Fighter.FNGR_PRNT_DNRM;


            RqstBnCochName_Butn.Text = CochName_Lb.Text;
            RqstBnCochName_Butn.Text += " ( " + FngrPrnt_Lb.Text + " )";

            CtgyBs2.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");

            var listMbsp =
               iScsc.Member_Ships
               .Where(ms =>
                  ms.RECT_CODE == "004" &&
                  ms.VALD_TYPE == "002" &&
                  ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                  ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                  (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                  ms.Fighter_Public.CBMT_CODE == cbmt.CODE
               );

            ActvMembCount_Lb.Text = listMbsp.Count().ToString();
            AgeMemb_Lb.Text = string.Join(", ", listMbsp.Select(ms => (DateTime.Now.Year - ms.Fighter.BRTH_DATE_DNRM.Value.Year).ToString()).Distinct().OrderBy(f => f).ToList());

            try
            {
               CochProFile_Rb.ImageVisiable = true;
               CochProFile_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", cbmt.COCH_FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => CochProFile_Rb.ImageProfile = bm));
               else
                  CochProFile_Rb.ImageProfile = bm;
            }
            catch
            { //Pb_FighImg.Visible = false;
               CochProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
            }
         }

         tb_cbmt2_SelectedIndexChanged(null, null);
      }

      private void SaveWkdy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Data.Club_Method c = null;
            if (Tb_Master.SelectedTab == tp_006)
               c = CbmtBs2.Current as Data.Club_Method;
            else if (Tb_Master.SelectedTab == tp_005)
               c = CbmtBs1.Current as Data.Club_Method;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                     new XElement("Update",
                        new XElement("Club_Method",
                           new XAttribute("code", c.CODE),
                           new XAttribute("clubcode", c.CLUB_CODE),
                           new XAttribute("mtodcode", c.MTOD_CODE),
                           new XAttribute("cochfileno", c.COCH_FILE_NO),
                           new XAttribute("daytype", c.DAY_TYPE),
                           new XAttribute("strttime", c.STRT_TIME.ToString()),
                           new XAttribute("endtime", c.END_TIME.ToString()),
                           new XAttribute("mtodstat", c.MTOD_STAT),
                           new XAttribute("sextype", c.SEX_TYPE),
                           new XAttribute("cbmtdesc", c.CBMT_DESC ?? ""),
                           new XAttribute("dfltstat", c.DFLT_STAT ?? "001"),
                           new XAttribute("cpctnumb", c.CPCT_NUMB ?? 0),
                           new XAttribute("cpctstat", c.CPCT_STAT ?? "001"),
                           new XAttribute("cbmttime", c.CBMT_TIME ?? 0),
                           new XAttribute("cbmttimestat", c.CBMT_TIME_STAT ?? "001"),
                           new XAttribute("clastime", c.CLAS_TIME ?? 90),
                           new XElement("Club_Method_Weekdays",
                              CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().Select(cbmw =>
                                 new XElement("Club_Method_Weekday",
                                    new XAttribute("code", cbmw.CODE),
                                    new XAttribute("weekday", cbmw.WEEK_DAY),
                                    new XAttribute("stat", cbmw.STAT)
                                 )
                              )
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
            }
         }
      }

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SimpleButton sb = sender as SimpleButton;

            if (CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT == "001")
            {
               CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT = "002";
               sb.Appearance.BackColor = Color.GreenYellow;
            }
            else
            {
               CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT = "001";
               sb.Appearance.BackColor = Color.LightGray;
            }
         }
         catch { }
      }

      private void Cbmt_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            if (e.Column.FieldName == "colUbStrtTime")
            {
               var row = e.Row as Data.Club_Method;
               if (e.IsGetData)
               {
                  if (row.STRT_TIME == null)
                     e.Value = new DateTime();
                  else
                     e.Value = new DateTime(((TimeSpan)row.STRT_TIME).Ticks);
               }
               if (e.IsSetData)
               {
                  if (e.Value is DateTime)
                     row.STRT_TIME = new TimeSpan(((DateTime)e.Value).Ticks);
               }
            }
            else if (e.Column.FieldName == "colUbEndTime")
            {
               var row = e.Row as Data.Club_Method;
               if (e.IsGetData)
               {
                  if (row.END_TIME == null)
                     e.Value = new DateTime();
                  else
                     e.Value = new DateTime(((TimeSpan)row.END_TIME).Ticks);
               }
               if (e.IsSetData)
               {
                  if (e.Value is DateTime)
                     row.END_TIME = new TimeSpan(((DateTime)e.Value).Ticks);
               }
            }
         }
         catch { }
      }

      private void Cbmt_Gv_InitNewRow(object sender, InitNewRowEventArgs e)
      {
         Cbmt_Gv.SetRowCellValue(e.RowHandle, colUbStrtTime, new DateTime());
         Cbmt_Gv.SetRowCellValue(e.RowHandle, colUbEndTime, new DateTime());
      }

      private void QWkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SimpleButton sb = sender as SimpleButton;

            if (sb.Appearance.BackColor == Color.LightGray)
            {
               sb.Appearance.BackColor = Color.GreenYellow;
            }
            else
            {
               sb.Appearance.BackColor = Color.LightGray;
            }
         }
         catch { }
      }

      private void ClubBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs1.Current as Data.Club;
            if (club == null) return;

            var weekdays = new List<string>();
            //CommandCbmt_Pnl.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow).ToList().ForEach(sb => weekdays.Add(string.Format("'{0}'", sb.Tag.ToString())));
            CommandCbmt_Pnl.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow).ToList().ForEach(sb => weekdays.Add(sb.Tag.ToString()));

            if (weekdays.Count == 0)
            {
               CbmtBs2.DataSource =
                  iScsc.Club_Methods.Where(cm =>
                     cm.CLUB_CODE == club.CODE &&
                     (CbmtStat_Butn.Tag.ToString() == "001" || (cm.MTOD_STAT == "002" && cm.Method.MTOD_STAT == "002" && Convert.ToInt32(cm.Fighter.ACTV_TAG_DNRM) >= 101)) 
                  );
               ClubWkdy_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }
            else
            {
               var strttime = TimeSpan.Parse(QStrtTime_Tim.Text);
               var endtime = TimeSpan.Parse(QEndTime_Tim.Text);

               CbmtBs2.DataSource =
                  iScsc.Club_Methods.Where(cm =>
                     cm.CLUB_CODE == club.CODE &&
                     (CbmtStat_Butn.Tag.ToString() == "001" || (cm.MTOD_STAT == "002" && cm.Method.MTOD_STAT == "002" && Convert.ToInt32(cm.Fighter.ACTV_TAG_DNRM) >= 101)) &&
                     cm.Club_Method_Weekdays.Any(cmw => cmw.STAT == "002" && weekdays.Contains(cmw.WEEK_DAY)) &&
                        //((cm.STRT_TIME >= strttime && cm.END_TIME <= endtime) ||
                        // (cm.STRT_TIME <= strttime && cm.END_TIME >= endtime) ||
                        // ((cm.STRT_TIME <= strttime && cm.END_TIME >= strttime) && cm.END_TIME <= endtime) ||
                        // ((cm.STRT_TIME >= strttime && cm.STRT_TIME <= endtime)))
                     ((cm.STRT_TIME.CompareTo(strttime) >= 0 && cm.END_TIME.CompareTo(endtime) <= 0) ||
                      (cm.STRT_TIME.CompareTo(strttime) <= 0 && cm.END_TIME.CompareTo(endtime) >= 0) ||
                      (cm.STRT_TIME.CompareTo(strttime) <= 0 && cm.END_TIME.CompareTo(strttime) >= 0 && cm.END_TIME.CompareTo(endtime) <= 0) ||
                      (cm.STRT_TIME.CompareTo(strttime) >= 0 && cm.STRT_TIME.CompareTo(endtime) <= 0))
                  );
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null)
               {
                  //CbmtBs2.List.Clear();
                  ClubWkdy_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
                  return;
               }
            }
            tb_cbmt2_SelectedIndexChanged(null, null);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void CochProFile_Rb_Click(object sender, EventArgs e)
      {
         try
         {
            Data.Club_Method cbmt = null;
            if (Tb_Master.SelectedTab == tp_006)
               cbmt = CbmtBs2.Current as Data.Club_Method;
            else if (Tb_Master.SelectedTab == tp_005)
               cbmt = CbmtBs1.Current as Data.Club_Method;

            if (cbmt == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", cbmt.COCH_FILE_NO)) }
            );
         }
         catch { }
      }

      private void AttnComPortName_Lov_SelectedIndexChanged(object sender, EventArgs e)
      {
         AttnComPortName_Txt.Text = AttnComPortName_Lov.Text;
      }

      private void GateComPortName_Lov_SelectedIndexChanged(object sender, EventArgs e)
      {
         GateComPortName_Txt.Text = GateComPortName_Lov.Text;
      }

      private void ExpnComPortName_Lov_SelectedIndexChanged(object sender, EventArgs e)
      {
         ExpnComPortName_Txt.Text = ExpnComPortName_Lov.Text;
      }

      private void ATIP_REST_Butn_Click(object sender, EventArgs e)
      {
         if (ModifierKeys.HasFlag(Keys.Control))
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
                                    "<Privilege>225</Privilege><Sub_Sys>5</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    MessageBox.Show("خطا - عدم دسترسی به ردیف 225 سطوح امینتی", "عدم دسترسی");
                                 })
                              },
                              #endregion
                           }),
                        #region DoWork
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.1"))}
                        #endregion
                     });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else
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
                                 "<Privilege>225</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 225 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region DoWork
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.5.1"), new XAttribute("funcdesc", "ClearAdministrators"))}
                     #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void DeleteComputer1_Butn_Click(object sender, EventArgs e)
      {
         var stng = StngBs1.Current as Data.Setting;

         stng.ATTN_COMP_CONCT = null;
         lookUpEdit3.EditValue = null;
      }

      private void DeleteComputer2_Butn_Click(object sender, EventArgs e)
      {
         var stng = StngBs1.Current as Data.Setting;

         stng.ATTN_COMP_CNC2 = null;
         lookUpEdit4.EditValue = null;
      }

      private void SettingSubmitChange_butn_Click(object sender, EventArgs e)
      {
         try
         {
            StngBs1.EndEdit();
            Data.Setting Stng = StngBs1.Current as Data.Setting;

            iScsc.STNG_SAVE_P(
               new XElement("Request",
                  new XElement("Settings",
                     new XAttribute("clubcode", Stng.CLUB_CODE),
                     new XAttribute("dfltstat", Stng.DFLT_STAT ?? "001"),
                     new XAttribute("backup", Stng.BACK_UP ?? false),
                     new XAttribute("backupappexit", Stng.BACK_UP_APP_EXIT ?? false),
                     new XAttribute("backupintred", Stng.BACK_UP_IN_TRED ?? false),
                     new XAttribute("backupoptnpath", Stng.BACK_UP_OPTN_PATH ?? false),
                     new XAttribute("backupoptnpathadrs", Stng.BACK_UP_OPTN_PATH_ADRS ?? ""),
                     new XAttribute("backuprootpath", Stng.BACK_UP_ROOT_PATH ?? ""),
                     new XAttribute("dresstat", Stng.DRES_STAT ?? "002"),
                     new XAttribute("dresauto", Stng.DRES_AUTO ?? "001"),
                     new XAttribute("morefighonedres", Stng.MORE_FIGH_ONE_DRES ?? "001"),
                     new XAttribute("moreattnsesn", Stng.MORE_ATTN_SESN ?? "002"),
                     new XAttribute("notfstat", Stng.NOTF_STAT ?? "001"),
                     new XAttribute("notfexpday", Stng.NOTF_EXP_DAY ?? 3),
                     new XAttribute("attnsysttype", Stng.ATTN_SYST_TYPE ?? "000"),
                     new XAttribute("commportname", /*AttnComPortName_Lov.Text*/Stng.COMM_PORT_NAME ?? ""),
                     new XAttribute("bandrate", Stng.BAND_RATE ?? 0),
                     new XAttribute("barcodedatatype", Stng.BAR_CODE_DATA_TYPE ?? "000"),
                     new XAttribute("atn3evntactntype", Stng.ATN3_EVNT_ACTN_TYPE ?? "001"),

                     new XAttribute("ipaddr", Stng.IP_ADDR ?? ""),
                     new XAttribute("portnumb", Stng.PORT_NUMB ?? 0),
                     new XAttribute("attncompconct", Stng.ATTN_COMP_CONCT ?? ""),
                     new XAttribute("atn1evntactntype", Stng.ATN1_EVNT_ACTN_TYPE ?? "001"),

                     new XAttribute("ipadr2", Stng.IP_ADR2 ?? ""),
                     new XAttribute("portnum2", Stng.PORT_NUM2 ?? 0),
                     new XAttribute("attncompcnc2", Stng.ATTN_COMP_CNC2 ?? ""),
                     new XAttribute("atn2evntactntype", Stng.ATN2_EVNT_ACTN_TYPE ?? "001"),

                     new XAttribute("attnnotfstat", Stng.ATTN_NOTF_STAT ?? "002"),
                     new XAttribute("attnnotfclostype", Stng.ATTN_NOTF_CLOS_TYPE ?? ""),
                     new XAttribute("attnnotfclosintr", Stng.ATTN_NOTF_CLOS_INTR ?? 0),
                     new XAttribute("debtclngstat", Stng.DEBT_CLNG_STAT ?? "001"),
                     new XAttribute("mostdebtclngamnt", Stng.MOST_DEBT_CLNG_AMNT ?? 0),
                     new XAttribute("exprdebtday", Stng.EXPR_DEBT_DAY ?? 7),
                     new XAttribute("tryvaldsbmt", Stng.TRY_VALD_SBMT ?? "002"),
                     new XAttribute("debtchckstat", Stng.DEBT_CHCK_STAT ?? "002"),
                     new XAttribute("permentrdebtservnumb", Stng.PERM_ENTR_DEBT_SERV_NUMB ?? 0),

                     new XAttribute("gateattnstat", Stng.GATE_ATTN_STAT ?? "001"),
                     new XAttribute("gatecommportname", /*GateComPortName_Lov.Text*/Stng.GATE_COMM_PORT_NAME ?? ""),
                     new XAttribute("gatebandrate", Stng.GATE_BAND_RATE ?? 9600),
                     new XAttribute("gatetimeclos", Stng.GATE_TIME_CLOS ?? 5),
                     new XAttribute("gateentropen", Stng.GATE_ENTR_OPEN ?? "002"),
                     new XAttribute("gateexitopen", Stng.GATE_EXIT_OPEN ?? "002"),

                     new XAttribute("expnextrstat", Stng.EXPN_EXTR_STAT ?? "001"),
                     new XAttribute("expncommportname", /*GateComPortName_Lov.Text*/Stng.EXPN_COMM_PORT_NAME ?? ""),
                     new XAttribute("expnbandrate", Stng.EXPN_BAND_RATE ?? 9600),

                     new XAttribute("runqury", Stng.RUN_QURY ?? "001"),
                     new XAttribute("attnprntstat", Stng.ATTN_PRNT_STAT ?? "001"),
                     new XAttribute("sharmbspstat", Stng.SHAR_MBSP_STAT ?? "001"),
                     new XAttribute("runrbot", Stng.RUN_RBOT ?? "001"),
                     new XAttribute("clerzero", Stng.CLER_ZERO ?? "001"),
                     new XAttribute("hldycont", Stng.HLDY_CONT ?? 1),
                     new XAttribute("duplnatlcode", Stng.DUPL_NATL_CODE ?? "002"),
                     new XAttribute("duplcellphon", Stng.DUPL_CELL_PHON ?? "002"),
                     new XAttribute("inptnatlcodestat", Stng.INPT_NATL_CODE_STAT ?? "002"),
                     new XAttribute("inptcellphonstat", Stng.INPT_CELL_PHON_STAT ?? "002"),

                     new XAttribute("ipadr3", Stng.IP_ADR3 ?? ""),
                     new XAttribute("portnum3", Stng.PORT_NUM3 ?? 0),
                     new XAttribute("attncompcnc3", Stng.ATTN_COMP_CNC3 ?? ""),
                     new XAttribute("atn4evntactntype", Stng.ATN4_EVNT_ACTN_TYPE ?? "001"),

                     new XAttribute("dev4stat", Stng.DEV4_STAT ?? ""),
                     new XAttribute("ipadr4", Stng.IP_ADR4 ?? ""),
                     new XAttribute("portnum4", Stng.PORT_NUM4 ?? 0),
                     new XAttribute("attncompcnc4", Stng.ATTN_COMP_CNC4 ?? ""),

                     new XAttribute("dev5stat", Stng.DEV5_STAT ?? ""),
                     new XAttribute("ipadr5", Stng.IP_ADR5 ?? ""),
                     new XAttribute("portnum5", Stng.PORT_NUM5 ?? 0),
                     new XAttribute("attncompcnc5", Stng.ATTN_COMP_CNC5 ?? ""),

                     new XAttribute("dev6stat", Stng.DEV6_STAT ?? ""),
                     new XAttribute("ipadr6", Stng.IP_ADR6 ?? ""),
                     new XAttribute("portnum6", Stng.PORT_NUM6 ?? 0),
                     new XAttribute("attncompcnc6", Stng.ATTN_COMP_CNC6 ?? ""),

                     new XAttribute("attngustnumbtype", Stng.ATTN_GUST_NUMB_TYPE ?? "002"),
                     new XAttribute("attndelytime", Stng.ATTN_DELY_TIME ?? 0),

                     new XAttribute("snd7path", Stng.SND7_PATH ?? ""),
                     new XAttribute("snd9path", Stng.SND9_PATH ?? ""),

                     new XAttribute("restattnnumbbyyear", Stng.REST_ATTN_NUMB_BY_YEAR ?? "002"),
                     new XAttribute("attnnotinsrstat", Stng.ATTN_NOT_INSR_STAT ?? "002"),

                     new XAttribute("showrbonmnui", Stng.SHOW_RBON_MNUI ?? "002"),
                     new XAttribute("showslidmnui", Stng.SHOW_SLID_MNUI ?? "001"),

                     new XAttribute("negdpstamnt", Stng.NEG_DPST_AMNT ?? "001"),
                     new XAttribute("dontshoweror", Stng.DONT_SHOW_EROR ?? "001"),
                     new XAttribute("showerorlog", Stng.SHOW_EROR_LOG ?? "001")
                  )
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            string FngrPrnt = "";
            if (Tb_Master.SelectedTab == tp_005)
            {
               var coch = CochBs1.Current as Data.Fighter;
               if (coch == null) return;
               FngrPrnt = coch.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }
            else if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;
               FngrPrnt = cbmt.Fighter.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", FngrPrnt))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            string FngrPrnt = "";
            if (Tb_Master.SelectedTab == tp_005)
            {
               var coch = CochBs1.Current as Data.Fighter;
               if (coch == null) return;
               FngrPrnt = coch.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }
            else if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;
               FngrPrnt = cbmt.Fighter.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", FngrPrnt))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            string FngrPrnt = "";
            if (Tb_Master.SelectedTab == tp_005)
            {
               var coch = CochBs1.Current as Data.Fighter;
               if (coch == null) return;
               FngrPrnt = coch.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }
            else if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;
               FngrPrnt = cbmt.Fighter.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", FngrPrnt))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            string FngrPrnt = "";
            if (Tb_Master.SelectedTab == tp_005)
            {
               var coch = CochBs1.Current as Data.Fighter;
               if (coch == null) return;
               FngrPrnt = coch.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }
            else if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;
               FngrPrnt = cbmt.Fighter.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FngrPrnt)
                     )
               }
            );
         }
         catch { }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            string FngrPrnt = "";
            if (Tb_Master.SelectedTab == tp_005)
            {
               var coch = CochBs1.Current as Data.Fighter;
               if (coch == null) return;
               FngrPrnt = coch.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }
            else if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;
               FngrPrnt = cbmt.Fighter.FNGR_PRNT_DNRM;

               if (FngrPrnt == "") return;
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FngrPrnt)
                     )
               }
            );
         }
         catch { }
      }
      #endregion

      private void RqstBnCochName_Butn_Click(object sender, EventArgs e)
      {
         if (Tb_Master.SelectedTab == tp_005)
         {
            CochInfo_Lnk_LinkClicked(null, null);
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            CochProFile_Rb_Click(null, null);
         }
      }

      private void RqstBnEditPblc_Click(object sender, EventArgs e)
      {
         long fileno = 0;
         if (Tb_Master.SelectedTab == tp_005)
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            fileno = coch.FILE_NO;
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;

            fileno = (long)cbmt.COCH_FILE_NO;
         }

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", fileno), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void RqstBnInComeDay_Butn_Click(object sender, EventArgs e)
      {
         long? fileno = 0,
              cbmtcode = null;

         if (Tb_Master.SelectedTab == tp_005)
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            fileno = coch.FILE_NO;
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;

            fileno = (long)cbmt.COCH_FILE_NO;
            cbmtcode = cbmt.CODE;
         }

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
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */)
                  {
                     Input = 
                        new XElement("Request", 
                           new XAttribute("type", "tp_001"), 
                           new XAttribute("formname", "RPT_PYM1_F"), 
                           new XAttribute("fromdate", DateTime.Now), 
                           new XAttribute("todate", DateTime.Now), 
                           new XAttribute("useraccount", "manager"), 
                           new XAttribute("cochfileno", fileno),
                           cbmtcode != null ? new XAttribute("cbmtcode", cbmtcode) : null
                        )
                  }
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnInComeWeek_Butn_Click(object sender, EventArgs e)
      {
         long? fileno = 0,
              cbmtcode = null;

         if (Tb_Master.SelectedTab == tp_005)
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            fileno = coch.FILE_NO;
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;

            fileno = (long)cbmt.COCH_FILE_NO;
            cbmtcode = cbmt.CODE;
         }

         var strtdate = DateTime.Now;
         switch (strtdate.DayOfWeek)
         {
            case DayOfWeek.Friday:
               strtdate = strtdate.AddDays(-6);
               break;
            case DayOfWeek.Monday:
               strtdate = strtdate.AddDays(-2);
               break;
            case DayOfWeek.Saturday:
               break;
            case DayOfWeek.Sunday:
               strtdate = strtdate.AddDays(-1);
               break;
            case DayOfWeek.Thursday:
               strtdate = strtdate.AddDays(-5);
               break;
            case DayOfWeek.Tuesday:
               strtdate = strtdate.AddDays(-3);
               break;
            case DayOfWeek.Wednesday:
               strtdate = strtdate.AddDays(-4);
               break;
         }
         var enddate = DateTime.Now;
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
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */)
                  {
                     Input = 
                        new XElement("Request", 
                           new XAttribute("type", "tp_001"), 
                           new XAttribute("formname", "RPT_PYM1_F"), 
                           new XAttribute("fromdate", strtdate), 
                           new XAttribute("todate", enddate), 
                           new XAttribute("useraccount", "manager"), 
                           new XAttribute("cochfileno", fileno),
                           cbmtcode != null ? new XAttribute("cbmtcode", cbmtcode) : null
                        )
                  }
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnInComeMonth_Butn_Click(object sender, EventArgs e)
      {
         long? fileno = 0,
              cbmtcode = null;

         if (Tb_Master.SelectedTab == tp_005)
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            fileno = coch.FILE_NO;
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;

            fileno = (long)cbmt.COCH_FILE_NO;
            cbmtcode = cbmt.CODE;
         }

         var strtdate = DateTime.Now;
         PersianCalendar pc = new PersianCalendar();
         strtdate = strtdate.AddDays(-(pc.GetDayOfMonth(strtdate) - 1));
         var enddate = DateTime.Now;
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
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */)
                  {
                     Input = 
                        new XElement("Request", 
                           new XAttribute("type", "tp_001"), 
                           new XAttribute("formname", "RPT_PYM1_F"), 
                           new XAttribute("fromdate", strtdate), 
                           new XAttribute("todate", enddate), 
                           new XAttribute("useraccount", "manager"), 
                           new XAttribute("cochfileno", fileno),
                           cbmtcode != null ? new XAttribute("cbmtcode", cbmtcode) : null
                        )
                  }
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnInComeYear_Butn_Click(object sender, EventArgs e)
      {
         long? fileno = 0,
              cbmtcode = null;

         if (Tb_Master.SelectedTab == tp_005)
         {
            var coch = CochBs1.Current as Data.Fighter;
            if (coch == null) return;

            fileno = coch.FILE_NO;
         }
         else if (Tb_Master.SelectedTab == tp_006)
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;

            fileno = (long)cbmt.COCH_FILE_NO;
            cbmtcode = cbmt.CODE;
         }

         var strtdate = DateTime.Now;
         PersianCalendar pc = new PersianCalendar();
         strtdate = strtdate.AddMonths(-(pc.GetMonth(strtdate) - 1));
         strtdate = strtdate.AddDays(-(pc.GetDayOfMonth(strtdate) - 1));
         var enddate = DateTime.Now;
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
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */)
                  {
                     Input = 
                        new XElement("Request", 
                           new XAttribute("type", "tp_001"), 
                           new XAttribute("formname", "RPT_PYM1_F"), 
                           new XAttribute("fromdate", strtdate), 
                           new XAttribute("todate", enddate), 
                           new XAttribute("useraccount", "manager"), 
                           new XAttribute("cochfileno", fileno),
                           cbmtcode != null ? new XAttribute("cbmtcode", cbmtcode) : null
                        )
                  }
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnAttnMonth_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Organ_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
                              "<Privilege>171</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا: عدم دسترسی به کد 171");
                              #endregion                           
                           })
                        },
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>175</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا: عدم دسترسی به کد 175");
                              #endregion                           
                           })
                        }
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 108 /* Execute Orgn_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 10 /* Actn_CalF_P */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void GropAttn_Butn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            long? cbmtcode = null;
            DateTime? date = null;
            if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (!AttnDate6_Dt.Value.HasValue)
                  date = AttnDate6_Dt.Value = DateTime.Now;
               else
                  date = AttnDate6_Dt.Value;
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (!AttnDate5_Dt.Value.HasValue)
                  date = AttnDate5_Dt.Value = DateTime.Now;
               else
                  date = AttnDate5_Dt.Value;
            }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                    
                     new Job(SendType.Self, 126 /* Execute Aop_Attn_F */),
                     new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Attendance",
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("attndate", date.Value.Date)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void GropMbsp_Butn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            long? cbmtcode = null;
            DateTime? date = null;
            if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (!AttnDate6_Dt.Value.HasValue)
                  date = AttnDate6_Dt.Value = DateTime.Now;
               else
                  date = AttnDate6_Dt.Value;
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (!AttnDate5_Dt.Value.HasValue)
                  date = AttnDate5_Dt.Value = DateTime.Now;
               else
                  date = AttnDate5_Dt.Value;
            }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                    
                     new Job(SendType.Self, 121 /* Execute Aop_Mbsp_F */),
                     new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Member_Ship",
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("attndate", date.Value.Date)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void AddNewMbsp_Butn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            long? cbmtcode = null, ctgycode = null;
            if (Tb_Master.SelectedTab == tp_006)
            {
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               var ctgy = CtgyBs2.Current as Data.Category_Belt;
               if (ctgy == null) return;

               cbmtcode = cbmt.CODE;
               ctgycode = ctgy.CODE;
            }
            else if (Tb_Master.SelectedTab == tp_005)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               var ctgy = CtgyBs2.Current as Data.Category_Belt;
               if (ctgy == null) return;

               cbmtcode = cbmt.CODE;
               ctgycode = ctgy.CODE;
            }



            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("type", "admcbmt"),
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("ctgycode", ctgycode)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void CbmtBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtBs1.Current as Data.Club_Method;
            if (cbmt == null) return;

            if (Tb_Master.SelectedTab == tp_005)
            {
               CbmtwkdyBs1.DataSource = cbmt.Club_Method_Weekdays.ToList();

               if (CbmtwkdyBs1.List.Count == 0)
               {
                  ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
                  return;
               }

               foreach (var wkdy in CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>())
               {
                  var rslt = ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
                  rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
               }

               FngrPrnt1_Lb.Text = cbmt.Fighter.FNGR_PRNT_DNRM == "" ? "نامشخص" : cbmt.Fighter.FNGR_PRNT_DNRM;
               CtgyBs2.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");

               var listMbsp =
                  iScsc.Member_Ships
                  .Where(ms =>
                     ms.RECT_CODE == "004" &&
                     ms.VALD_TYPE == "002" &&
                     ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                     ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                     (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                     ms.Fighter_Public.CBMT_CODE == cbmt.CODE
                  );

               ActvMembCount1_Lb.Text = listMbsp.Count().ToString();
               AgeMemb1_Lb.Text = string.Join(", ", listMbsp.Select(ms => (DateTime.Now.Year - ms.Fighter.BRTH_DATE_DNRM.Value.Year).ToString()).Distinct().OrderBy(f => f).ToList());

               tb_cbmt1_SelectedIndexChanged(null, null);
            }
         }
         catch { }
      }

      private void tb_cbmt1_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (tb_cbmt1.SelectedTab == tp_0052)
            {
               Attn5Bs.List.Clear();
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (!ReloadAttn5_Cb.Checked) return;

               iScsc.CommandTimeout = int.MaxValue;

               var actvmbsp =
                  iScsc.VF_Coach_MemberShip(
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
                  );

               Attn5Bs.DataSource =
                  iScsc.Attendances
                  .Where(a => actvmbsp.Any(am => am.FILE_NO == a.FIGH_FILE_NO && am.RWNO == a.MBSP_RWNO_DNRM));
            }
            else if (tb_cbmt1.SelectedTab == tp_0053)
            {
               VCochMbsp5Bs.List.Clear();
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (!ReloadMbsp5_Cb.Checked) return;

               iScsc.CommandTimeout = int.MaxValue;

               VCochMbsp5Bs.DataSource =
                  iScsc.VF_Coach_MemberShip(
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
                  );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NextCbmt1_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs1.MoveNext();
      }

      private void BackCbmt1_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs1.MovePrevious();
      }

      private void tb_cbmt2_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (tb_cbmt2.SelectedTab == tp_0062)
            {
               Attn6Bs.List.Clear();
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (!ReloadAttn6_Cb.Checked) return;

               iScsc.CommandTimeout = int.MaxValue;

               var actvmbsp =
                  iScsc.VF_Coach_MemberShip(
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
                  );

               Attn6Bs.DataSource =
                  iScsc.Attendances
                  .Where(a => actvmbsp.Any(am => am.FILE_NO == a.FIGH_FILE_NO && am.RWNO == a.MBSP_RWNO_DNRM));
            }
            else if (tb_cbmt2.SelectedTab == tp_0063)
            {
               Attn6Bs.List.Clear();
               var cbmt = CbmtBs2.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (!ReloadAttn6_Cb.Checked) return;

               iScsc.CommandTimeout = int.MaxValue;

               VCochMbsp6Bs.DataSource =
                  iScsc.VF_Coach_MemberShip(
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
                  );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NextCbmt2_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs2.MoveNext();
      }

      private void BackCbmt2_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs2.MovePrevious();
      }

      private void AddSys_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self)
            {
               AfterChangedOutput =
                  new Action<object>(
                     (output) =>
                     {
                        if (ComaBs.List.OfType<Data.Computer_Action>().Any(ca => ca.CODE == 0)) return;

                        var hostinfo = output as XElement;

                        if (ComaBs.List.OfType<Data.Computer_Action>().Any(ca => ca.COMP_NAME == hostinfo.Attribute("name").Value)) return;

                        ComaBs.AddNew();
                        var coma = ComaBs.Current as Data.Computer_Action;
                        coma.COMP_NAME = hostinfo.Attribute("name").Value;
                        coma.IP_ADRS = hostinfo.Attribute("ip").Value;
                        coma.CHCK_ATTN_ALRM = "001";
                        coma.CHCK_DOBL_ATTN_STAT = "001";

                        SaveComa_Butn_Click(null, null);
                     }
                  )
            }
         );
      }

      private void AddCndo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var regn = RegnBs1.Current as Data.Region;
            if (regn == null) return;

            if (CndoBs1.List.OfType<Data.Cando>().Any(c => c.CRET_BY == null)) return;

            CndoBs1.AddNew();

            var cndo = CndoBs1.Current as Data.Cando;
            iScsc.Candos.InsertOnSubmit(cndo);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveCndo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CndoBs1.EndEdit();
            Cndo_Gv.PostEditor();

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

      private void DelCndo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cndo = CndoBs1.Current as Data.Cando;
            if (cndo == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.DEL_CNDO_P(cndo.REGN_PRVN_CNTY_CODE, cndo.REGN_PRVN_CODE, cndo.REGN_CODE, cndo.CODE);
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

      private void AddCndoBlok_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cndo = CndoBs1.Current as Data.Cando;
            if (cndo == null) return;

            if (CblkBs1.List.OfType<Data.Cando_Block>().Any(b => b.CRET_BY == null)) return;

            var cblk = CblkBs1.AddNew() as Data.Cando_Block;
            iScsc.Cando_Blocks.InsertOnSubmit(cblk);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveCndoBlok_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CblkBs1.EndEdit();
            Cblk_Gv.PostEditor();

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

      private void DelCndoBlok_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cblk = CblkBs1.Current as Data.Cando_Block;
            if (cblk == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.DEL_CBLK_P(cblk.CNDO_CODE, cblk.CODE);
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

      private void AddCndoBlokUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cblk = CblkBs1.Current as Data.Cando_Block;
            if (cblk == null) return;

            if (CuntBs1.List.OfType<Data.Cando_Block_Unit>().Any(u => u.CRET_BY == null)) return;

            var cunt = CuntBs1.AddNew() as Data.Cando_Block_Unit;
            iScsc.Cando_Block_Units.InsertOnSubmit(cunt);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveCndoBlokUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CuntBs1.EndEdit();
            Cunt_Gv.PostEditor();

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

      private void DelCndoBlokUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cunt = CuntBs1.Current as Data.Cando_Block_Unit;
            if (cunt == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.DEL_CUNT_P(cunt.BLOK_CNDO_CODE, cunt.BLOK_CODE, cunt.CODE);
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

      private void BlokSubmit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cndo = CndoBs1.Current as Data.Cando;
            if (cndo == null) return;

            if (Blok_Txt.Text == "") { Blok_Txt.Focus(); return; }
            /*if(CblkBs1.List.OfType<Data.Cando_Block>().Any(b => Convert.ToDecimal(b.CODE) >= MinBlok_Nud.Value && Convert.ToDecimal(b.CODE) <= MaxBlok_Nud.Value))
            {
               MessageBox.Show(this, "بازه ای که برای تعریف بلوک انتخاب کرده اید قبلا ثبت شده");
            }*/

            for (decimal i = MinBlok_Nud.Value; i < MaxBlok_Nud.Value; i++)
            {
               iScsc.INS_CBLK_P(cndo.CODE, i.ToString(), Blok_Txt.Text + " " + i.ToString(), null, null, null, null);
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

      private void UnitSubmit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
               if (Unit_Txt.Text == "") { Unit_Txt.Focus(); return; }

               CblkBs1.MoveFirst();
               foreach (var cblk in CblkBs1.List.OfType<Data.Cando_Block>())
               {
                  for (decimal i = MinUnit_Nud.Value; i < MaxUnit_Nud.Value; i++)
                  {
                     iScsc.INS_CUNT_P(cblk.CNDO_CODE, cblk.CODE, i.ToString(), Unit_Txt.Text + " " + i.ToString(), null, null, null, null);
                  }
               }
            }
            else
            {
               var cblk = CblkBs1.Current as Data.Cando_Block;
               if (cblk == null) return;

               if (Unit_Txt.Text == "") { Unit_Txt.Focus(); return; }

               for (decimal i = MinUnit_Nud.Value; i < MaxUnit_Nud.Value; i++)
               {
                  iScsc.INS_CUNT_P(cblk.CNDO_CODE, cblk.CODE, i.ToString(), Unit_Txt.Text + " " + i.ToString(), null, null, null, null);
               }
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

      private void FighMbsp5_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            dynamic vcochmbsp = null;
            if (Tb_Master.SelectedTab == tp_005)
               vcochmbsp = VCochMbsp5Bs.Current as Data.VF_Coach_MemberShipResult;
            else
               vcochmbsp = VCochMbsp6Bs.Current as Data.VF_Coach_MemberShipResult;

            if (vcochmbsp == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", vcochmbsp.FILE_NO)) }
                  );
                  break;
               case 1:
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
                                       "<Privilege>231</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 231 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region DoWork
                              new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                              new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Fighter",
                                       new XAttribute("fileno", vcochmbsp.FILE_NO),
                                       new XAttribute("mbsprwno", vcochmbsp.RWNO),
                                       new XAttribute("formcaller", GetType().Name)
                                    )
                              }
                           #endregion
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
      }

      private void SelectedImage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var epit = InComeEpitBs1.Current as Data.Expense_Item;
            if (epit == null) return;

            OpnFil_Ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;...";
            if (OpnFil_Ofd.ShowDialog() != DialogResult.OK) return;

            var img = Image.FromFile(OpnFil_Ofd.FileName);

            IncEpitImag_Pb.Image = img;

            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
               img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
               arr = ms.ToArray();
            }

            epit.IMAG = arr;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void InComeEpitBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            IncEpitImag_Pb.Image = null;
            var epit = InComeEpitBs1.Current as Data.Expense_Item;
            if (epit == null) return;

            if (epit.IMAG != null)
            {
               Byte[] img = ((System.Data.Linq.Binary)epit.IMAG).ToArray();

               MemoryStream ms = new MemoryStream();
               int offset = 0;
               ms.Write(img, offset, img.Length - offset);
               Bitmap bmp = new Bitmap(ms);
               ms.Close();

               IncEpitImag_Pb.Image = bmp;
            }
         }
         catch (Exception)
         {

         }
      }

      private void RunInsTime_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtBs1.Current as Data.Club_Method;
            if (cbmt == null) return;

            var strt = ((DateTime)StrtTime_Tspn.EditValue).TimeOfDay;
            var end = ((DateTime)EndTime_Tspn.EditValue).TimeOfDay;
            var priod = Convert.ToInt32(Time_Tspn.EditValue);

            var i = strt;

            while (i <= end)
            {
               if (!CbmtBs1.List.OfType<Data.Club_Method>().Any(t => t.MTOD_CODE == cbmt.MTOD_CODE && t.DAY_TYPE == cbmt.DAY_TYPE && t.STRT_TIME == i && t.END_TIME == i.Add(new TimeSpan(0, priod, 0))))
               {
                  iScsc.INS_CBMT_P(cbmt.CLUB_CODE, cbmt.MTOD_CODE, cbmt.COCH_FILE_NO, cbmt.DAY_TYPE, i, i.Add(new TimeSpan(0, priod, 0)), cbmt.SEX_TYPE, cbmt.CBMT_DESC, cbmt.DFLT_STAT, cbmt.CPCT_NUMB, cbmt.CPCT_STAT, cbmt.CBMT_TIME, cbmt.CBMT_TIME_STAT, cbmt.CLAS_TIME, cbmt.AMNT, cbmt.NATL_CODE, cbmt.DURT_ATTN_SOND_PATH);
               }

               i = i.Add(new TimeSpan(0, priod, 0));
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

      private void A0FC_REST_Butn_Click(object sender, EventArgs e)
      {
         try
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
                                 "<Privilege>225</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 225 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region DoWork
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_Calf_p */){Input = new XElement("DeviceControlFunction", new XAttribute("type", "fngrprntdev"), new XAttribute("fngractn", "truncate"))}
                     #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExcpAttnActv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var coch = CochBs1.Current as Data.Fighter;

            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '001' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '002', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '002';", coch.FILE_NO
               )
            );
            MessageBox.Show("عملیات استثناء ورود با موفقیت فعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExcpAttnDact_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var coch = CochBs1.Current as Data.Fighter;

            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '001' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '001', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '001';", coch.FILE_NO
               )
            );
            MessageBox.Show("عملیات استثناء ورود با موفقیت غیرفعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region TabPage 010
      private void AddNew_Exdv_Butn_Click(object sender, EventArgs e)
      {
         if (ExdvBs.List.OfType<Data.External_Device>().Any(ex => ex.CODE == 0)) return;

         var exdv = ExdvBs.AddNew() as Data.External_Device;

         iScsc.External_Devices.InsertOnSubmit(exdv);
      }

      private void Save_Exdv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ExdvBs.EndEdit();
            Exdv_Gv.PostEditor();

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

      private void Refresh_Exdv_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void ExdvBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var exdv = ExdvBs.Current as Data.External_Device;
            if (exdv == null) return;

            DevName_Tsm.Text = exdv.DEV_NAME;

            if (exdv.DEV_TYPE == "001")
            {
               Gate_Tsm.Enabled = false;
            }
            else if (exdv.DEV_TYPE == "006")
            {
               Gate_Tsm.Enabled = true;
            }

            EdlmBs.DataSource = iScsc.External_Device_Link_Methods.Where(x => x.External_Device == exdv);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Gate_Test_Tsm_Click(object sender, EventArgs e)
      {
         var dev = ExdvBs.Current as Data.External_Device;
         if (dev == null && dev.DEV_TYPE != "006") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "extdev"),
                           new XAttribute("devtype", "006"),
                           new XAttribute("contype", "002"),
                           new XAttribute("cmdtype", "test"),
                           new XAttribute("cmdsend", ""),
                           new XAttribute("ip", dev.IP_ADRS),
                           new XAttribute("sendport", dev.PORT_SEND)
                        )
                  }
               }
            )
         );
      }

      private void Gate_OnLineMode_Tsm_Click(object sender, EventArgs e)
      {
         var dev = ExdvBs.Current as Data.External_Device;
         if (dev == null && dev.DEV_TYPE != "006") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "extdev"),
                           new XAttribute("devtype", "006"),
                           new XAttribute("contype", "002"),
                           new XAttribute("cmdtype", "gotoonline"),
                           new XAttribute("cmdsend", ""),
                           new XAttribute("ip", dev.IP_ADRS),
                           new XAttribute("sendport", dev.PORT_SEND)
                        )
                  }
               }
            )
         );
      }

      private void Gate_OffLineMode_Tsm_Click(object sender, EventArgs e)
      {
         var dev = ExdvBs.Current as Data.External_Device;
         if (dev == null && dev.DEV_TYPE != "006") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "extdev"),
                           new XAttribute("devtype", "006"),
                           new XAttribute("contype", "002"),
                           new XAttribute("cmdtype", "gotooffline"),
                           new XAttribute("cmdsend", ""),
                           new XAttribute("ip", dev.IP_ADRS),
                           new XAttribute("sendport", dev.PORT_SEND)
                        )
                  }
               }
            )
         );
      }

      private void Gate_Open_Tsm_Click(object sender, EventArgs e)
      {
         var dev = ExdvBs.Current as Data.External_Device;
         if (dev == null && dev.DEV_TYPE != "006") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "extdev"),
                           new XAttribute("devtype", "006"),
                           new XAttribute("contype", "002"),
                           new XAttribute("cmdtype", "open"),
                           new XAttribute("cmdsend", ""),
                           new XAttribute("ip", dev.IP_ADRS),
                           new XAttribute("sendport", dev.PORT_SEND)
                        )
                  }
               }
            )
         );
      }

      private void Gate_Close_Tsm_Click(object sender, EventArgs e)
      {
         var dev = ExdvBs.Current as Data.External_Device;
         if (dev == null && dev.DEV_TYPE != "006") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "extdev"),
                           new XAttribute("devtype", "006"),
                           new XAttribute("contype", "002"),
                           new XAttribute("cmdtype", "close"),
                           new XAttribute("cmdsend", ""),
                           new XAttribute("ip", dev.IP_ADRS),
                           new XAttribute("sendport", dev.PORT_SEND)
                        )
                  }
               }
            )
         );
      }

      private void Gate_Error_Tsm_Click(object sender, EventArgs e)
      {
         var dev = ExdvBs.Current as Data.External_Device;
         if (dev == null && dev.DEV_TYPE != "006") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "extdev"),
                           new XAttribute("devtype", "006"),
                           new XAttribute("contype", "002"),
                           new XAttribute("cmdtype", "error"),
                           new XAttribute("cmdsend", ""),
                           new XAttribute("ip", dev.IP_ADRS),
                           new XAttribute("sendport", dev.PORT_SEND)
                        )
                  }
               }
            )
         );
      }
      #endregion

      private void ProdList_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var epit = InComeEpitBs1.Current as Data.Expense_Item;
            if (epit == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 161 /* Execute Bas_Prod_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_PROD_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Product", new XAttribute("epitcode", epit.CODE), new XAttribute("formstat", "show"))}
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ResetAllDevice_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var devs = ExdvBs.List.OfType<Data.External_Device>().Where(d => d.DEV_COMP_TYPE == "002" && d.DEV_TYPE == "008" && d.STAT == "002");

            foreach (var dev in devs)
            {
               var expn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == dev.EXPN_CODE);
               // ارسال پیام برای باز کردن دستگاه چراغ میز برای مشتری                              
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
                  {
                     Input =
                        new XElement("ExpenseGame",
                            new XAttribute("type", "expnextr"),
                            new XAttribute("macadrs", dev.DEV_NAME),
                            new XAttribute("expncode", dev.EXPN_CODE),
                            new XAttribute("cmndtext",
                               "er:" + "0".PadLeft(10, ' ') +
                               "&" + expn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                               ":" + expn.PRIC.ToString("n0").PadLeft(9, ' ')),
                            new XAttribute("fngrprnt", "")
                        )
                  }
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ResetSelectedDevice_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var dev = ExdvBs.Current as Data.External_Device;
            if (dev == null) return;

            if (!(dev.DEV_COMP_TYPE == "002" && dev.DEV_TYPE == "008" && dev.STAT == "002")) return;

            var expn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == dev.EXPN_CODE);
            // ارسال پیام برای باز کردن دستگاه چراغ میز برای مشتری                              
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Call_Actn_P */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("ExpenseGame",
                         new XAttribute("type", "expnextr"),
                         new XAttribute("macadrs", dev.DEV_NAME),
                         new XAttribute("expncode", dev.EXPN_CODE),
                         new XAttribute("cmndtext",
                            "er:" + "0".PadLeft(10, ' ') +
                            "&" + expn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                            ":" + expn.PRIC.ToString("n0").PadLeft(9, ' ')),
                         new XAttribute("fngrprnt", "")
                     )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddEdlm_Butn_Click(object sender, EventArgs e)
      {
         var edev = ExdvBs.Current as Data.External_Device;
         if (edev == null) return;

         if (EdlmBs.List.OfType<Data.External_Device_Link_Method>().Any(x => x.CODE == 0)) return;

         var edlm = EdlmBs.AddNew() as Data.External_Device_Link_Method;
         edlm.External_Device = edev;

         iScsc.External_Device_Link_Methods.InsertOnSubmit(edlm);
      }

      private void DelEdlm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var edlm = EdlmBs.Current as Data.External_Device_Link_Method;
            if (edlm == null) return;

            if (MessageBox.Show(this, "آیا حذف لینک گروه موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.External_Device_Link_Methods.DeleteOnSubmit(edlm);

            iScsc.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveEdlm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Edlm_Gv.PostEditor();
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

      private void ClubBs3_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs3.Current as Data.Club;
            if (club == null) return;

            var s = club.Settings.FirstOrDefault();

            InptNatlCode_Cb.Checked = (s.INPT_NATL_CODE_STAT ?? "001") == "002" ? true : false;
            InptCellPhon_Cb.Checked = (s.INPT_CELL_PHON_STAT ?? "001") == "002" ? true : false;

            ShowRbonMnui_Pkb.PickChecked = (s.SHOW_RBON_MNUI ?? "001") == "002" ? true : false;
            ShowSlidMnui_Pkb.PickChecked = (s.SHOW_SLID_MNUI ?? "001") == "002" ? true : false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void InptNatlCode_Cb_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs3.Current as Data.Club;
            if (club == null) return;

            var s = club.Settings.FirstOrDefault();

            s.INPT_NATL_CODE_STAT = InptNatlCode_Cb.Checked ? "002" : "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void InptCellPhon_Cb_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs3.Current as Data.Club;
            if (club == null) return;

            var s = club.Settings.FirstOrDefault();

            s.INPT_CELL_PHON_STAT = InptCellPhon_Cb.Checked ? "002" : "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddPlacHldr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var tmpi = TmpiBs.Current as Data.Template_Item;
            if (tmpi == null) return;
            
            TempText_Txt.Text = TempText_Txt.Text.Insert(TempText_Txt.SelectionStart, tmpi.PLAC_HLDR);
            TempText_Txt.Focus();
            AddPlacHldr_Butn.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddTmp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (TmplBs.List.OfType<Data.Template>().Any(t => t.TMID == 0)) return;

            var tmp = TmplBs.AddNew() as Data.Template;
            iScsc.Templates.InsertOnSubmit(tmp);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveTmp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Tmpl_gv.PostEditor();

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

      private void DelTmp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tmp = TmplBs.Current as Data.Template;
            if (_tmp == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Templates.DeleteOnSubmit(_tmp);
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

      private void NextCbmt_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs2.MoveNext();
      }

      private void PervCbmt_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs2.MovePrevious();
      }

      private void SaveUpdtCbmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = CbmtBs2.Current as Data.Club_Method;
            if (_cbmt == null) return;

            //if (NewTarfCode_Txt.Text == "") { NewTarfCode_Txt.Focus(); return; }
            if (NewCochFileNo_Txt.EditValue == null || NewCochFileNo_Txt.EditValue.ToString() == "") { NewCochFileNo_Txt.Focus(); return; }
            if (NewMtodCode_Lov.EditValue == null || NewMtodCode_Lov.EditValue.ToString() == "") { NewMtodCode_Lov.Focus(); return; }
            if (NewStrtTime_Te.EditValue == null || NewStrtTime_Te.EditValue.ToString() == "") { NewStrtTime_Te.Focus(); return; }
            if (NewEndTime_Te.EditValue == null || NewEndTime_Te.EditValue.ToString() == "") { NewEndTime_Te.Focus(); return; }

            iScsc.ExecuteCommand(
               string.Format("UPDATE dbo.Club_Method SET Coch_File_No = {0}, Mtod_Code = {1}, Strt_Time = '{2}', End_Time = '{3}', Cbmt_Desc = N'{4}' WHERE Code = {5};",
                  NewCochFileNo_Txt.EditValue,
                  NewMtodCode_Lov.EditValue,
                  NewStrtTime_Te.EditValue,
                  NewEndTime_Te.EditValue,
                  NewCbmtDesc_Txt.Text,
                  _cbmt.CODE
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

      private void CopyCbmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = CbmtBs2.Current as Data.Club_Method;
            if (_cbmt == null) return;

            NewCochFileNo_Txt.EditValue = _cbmt.COCH_FILE_NO;
            NewMtodCode_Lov.EditValue = _cbmt.MTOD_CODE;
            NewStrtTime_Te.EditValue = _cbmt.STRT_TIME;
            NewEndTime_Te.EditValue = _cbmt.END_TIME;
            NewCbmtDesc_Txt.EditValue = _cbmt.CBMT_DESC;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CbmtStat_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SimpleButton sb = sender as SimpleButton;

            if (sb.Appearance.BackColor == Color.Goldenrod)
            {
               sb.Appearance.BackColor = Color.GreenYellow;
               sb.Tag = "002";
            }
            else
            {
               sb.Appearance.BackColor = Color.Goldenrod;
               sb.Tag = "001";
            }

            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void AddUlkm_Butn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelUlkm_Butn_Click(object sender, EventArgs e)
      {
         try
         {

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

      private void SaveUlkm_Butn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MergUsers4Mtod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            switch(MessageBox.Show(this, "آیا ثبت دسترسی کاربران به گروه خدمات جاری باشد یا تمام گروه ها؟", "نحوه دسترسی به گروه خدمات", MessageBoxButtons.YesNoCancel))
            {
               case DialogResult.Yes:
                  var _mtod = MtodBs1.Current as Data.Method;
                  if (_mtod == null) return;

                  iScsc.ExecuteCommand(
                     string.Format(
                        "Merge dbo.User_Link_Method T " + 
                        "Using (Select u.Id As User_Id, {0} As Mtod_Code from dbo.V#Users u) S " +
                        "On (T.Mtod_Code = S.Mtod_Code And T.User_Id = S.User_Id) " + 
                        "When Not Matched Then Insert (Mtod_Code, User_Id, Code, Stat) Values(S.Mtod_Code, S.User_Id, dbo.Gnrt_Nvid_U(), '002');",
                        _mtod.CODE
                     )
                  );
                  break;
               case DialogResult.No:
                  iScsc.ExecuteCommand(
                        "Merge dbo.User_Link_Method T " +
                        "Using (Select u.Id As User_Id, m.Code As Mtod_Code from dbo.V#Users u, dbo.Method m) S " +
                        "On (T.Mtod_Code = S.Mtod_Code And T.User_Id = S.User_Id) " +
                        "When Not Matched Then Insert (Mtod_Code, User_Id, Code, Stat) Values(S.Mtod_Code, S.User_Id, dbo.Gnrt_Nvid_U(), '002');"
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

      private void DupCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            iScsc.DUP_CEXC_P(
               new XElement("OpIran",
                   new XAttribute("cexccode", _cexc.CODE)
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

      private void ADCexc_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            CexcBs.EndEdit();
            Cexc_Gv.PostEditor();

            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            // 1401/08/20 * مرگ بر اصل ولایت فقیه
            // مرگ بر خامنه ای
            // اگر کلید کنترل گرفته شده باشد تمام گروه خدمات برای این پرسنل فعال یا غیر فعال میکنیم
            if(ModifierKeys == Keys.Control)
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach SET Stat = '{0}' " + 
                     "WHERE Coch_File_No = {1} AND Mtod_Code = {2} AND Rqtp_Code = {3} AND Extp_Code = {4};",
                     _cexc.STAT == "001" ? "002" : "001",
                     _cexc.COCH_FILE_NO,
                     _cexc.MTOD_CODE,
                     _cexc.RQTP_CODE,
                     _cexc.EXTP_CODE
                  )
               );
            }
            else if (ModifierKeys == Keys.Shift)
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach SET Stat = '{0}' " + 
                     "WHERE Mtod_Code = {1} AND Rqtp_Code = {2} AND Extp_Code = {3};",
                     _cexc.STAT == "001" ? "002" : "001",
                     _cexc.MTOD_CODE,
                     _cexc.RQTP_CODE,
                     _cexc.EXTP_CODE
                  )
               );
            }
            else
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach SET Stat = '{0}' " + 
                     "WHERE Code = {1};",
                     _cexc.STAT == "001" ? "002" : "001",
                     _cexc.CODE
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
            if (requery)
               Execute_Query();
         }
      }

      private void CopyCexc_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            CexcBs.EndEdit();
            Cexc_Gv.PostEditor();

            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            if(ModifierKeys == Keys.Control)
            {
               // برای سرگروه تمام پرسنل
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{3}', Calc_Type = '{4}', Prct_Valu = {5}, Stat = '{6}', Pymt_Stat = '{7}', Rduc_Amnt = {8}, Efct_Date_Type = '{9}', Fore_Givn_Attn_Numb = {10} " +
                     "WHERE Mtod_Code = {0} AND Rqtp_Code = {1} AND Extp_Code = {2};",
                     _cexc.MTOD_CODE,
                     _cexc.RQTP_CODE,
                     _cexc.EXTP_CODE,
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            else if (ModifierKeys == Keys.Shift)
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{1}', Calc_Type = '{2}', Prct_Valu = {3}, Stat = '{4}', Pymt_Stat = '{5}', Rduc_Amnt = {6}, Efct_Date_Type = '{7}', Fore_Givn_Attn_Numb = {8} " +
                     "WHERE Coch_File_No = {0};",
                     _cexc.COCH_FILE_NO,
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            else if (ModifierKeys == (Keys.Control | Keys.Shift))
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{0}', Calc_Type = '{1}', Prct_Valu = {2}, Stat = '{3}', Pymt_Stat = '{4}', Rduc_Amnt = {5}, Efct_Date_Type = '{6}', Fore_Givn_Attn_Numb = {7};",
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            else
            {
               // برای تمام آیتم های سرگروه خوده پرسنل
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{4}', Calc_Type = '{5}', Prct_Valu = {6}, Stat = '{7}', Pymt_Stat = '{8}', Rduc_Amnt = {9}, Efct_Date_Type = '{10}', Fore_Givn_Attn_Numb = {11} " +  
                     "WHERE Coch_File_No = {0} AND Mtod_Code = {1} AND Rqtp_Code = {2} AND Extp_Code = {3};",
                     _cexc.COCH_FILE_NO,
                     _cexc.MTOD_CODE,
                     _cexc.RQTP_CODE,
                     _cexc.EXTP_CODE,
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
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

      private void SaveCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CexcBs.EndEdit();
            Cexc_Gv.PostEditor();

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

      private void Ulkm_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _ulkm = UlkmBs1.Current as Data.User_Link_Method;
            if (_ulkm == null) return;

            _ulkm.STAT = _ulkm.STAT == "001" ? "002" : "001";

            iScsc.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void Msgt_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _tmpl = TmplBs.Current as Data.Template;
            if (_tmpl == null) return;

            if (Msgt_Lov.EditValue == null || Msgt_Lov.EditValue.ToString() == "") return;

            switch (e.Button.Index)
            {
               case 1:
                  iScsc.ExecuteCommand(
                     string.Format("UPDATE dbo.Message_Broadcast SET Msgb_Text = N'{0}' WHERE Msgb_Type = '{1}'", _tmpl.TEMP_TEXT, Msgt_Lov.EditValue)
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

      private void DurtAttnSondPath_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (DurtAttnSp_Ofd.ShowDialog() != DialogResult.OK) return;

            if (DurtAttnSp_Ofd.FileName == "") return;

            DurtAttnSp_Txt.Text = DurtAttnSp_Ofd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveDASP_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = CbmtBs2.Current as Data.Club_Method;
            if (_cbmt == null) return;

            //if (string.IsNullOrEmpty(DurtAttnSp_Txt.Text))
            //{
            //   DurtAttnSp_Txt.Focus();
            //   DurtAttnSondPath_Butn_Click(null, null);
            //}

            if(ModifierKeys == Keys.Control)
               iScsc.ExecuteCommand(string.Format("UPDATE Club_Method SET Durt_Attn_Sond_Path = N'{0}' WHERE Mtod_Code = {1};", DurtAttnSp_Txt.Text, _cbmt.MTOD_CODE));
            else
               iScsc.ExecuteCommand(string.Format("UPDATE Club_Method SET Durt_Attn_Sond_Path = N'{0}' WHERE Code = {1};", DurtAttnSp_Txt.Text, _cbmt.CODE));

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

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      private void PlayDASP_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = CbmtBs2.Current as Data.Club_Method;
            if (_cbmt == null) return;

            wplayer.URL = _cbmt.DURT_ATTN_SOND_PATH;
            wplayer.controls.play();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PlayDebtSond_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _stng = StngBs1.Current as Data.Setting;
            if (_stng == null) return;

            wplayer.URL = _stng.SND9_PATH;
            //wplayer.controls.play();
            switch(PlayDebtSond_Butn.Tag.ToString())
            {
               case "stop":
                  PlayDebtSond_Butn.Tag = "play";
                  new Threading.Thread(PlaySound).Start();
                  break;
               case "play":
                  PlayDebtSond_Butn.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PlayHbDaySond_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _stng = StngBs1.Current as Data.Setting;
            if (_stng == null) return;

            wplayer.URL = _stng.SND7_PATH;
            //wplayer.controls.play();

            switch (PlayHbDaySond_Butn.Tag.ToString())
            {
               case "stop":
                  PlayHbDaySond_Butn.Tag = "play";
                  new Threading.Thread(PlaySound).Start();
                  break;
               case "play":
                  PlayHbDaySond_Butn.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      void PlaySound()
      {
         wplayer.controls.play();
      }

      void StopSound()
      {
         wplayer.controls.stop();
      }

      private void DuplCtgy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _btn = (MaxUi.Button)sender;

            if(DuplCtgy_Fcb.Checked)
            {
               switch (_btn.Tag.ToString())
               {
                  case "char":
                     foreach (var _ctgy in CtgyBs1.List.OfType<Data.Category_Belt>())
                     {
                        iScsc.INS_CTGY_P(_ctgy.MTOD_CODE, _ctgy.CTGY_CODE, _ctgy.CTGY_DESC + DuplChar_Txt.Text, _ctgy.CTGY_DESC + DuplChar_Txt.Text, null, _ctgy.EPIT_TYPE, _ctgy.NUMB_OF_ATTN_MONT, _ctgy.NUMB_CYCL_DAY, _ctgy.NUMB_MONT_OFER, _ctgy.PRVT_COCH_EXPN, _ctgy.PRIC, _ctgy.DFLT_STAT, _ctgy.CTGY_STAT, _ctgy.GUST_NUMB, null, _ctgy.RWRD_ATTN_PRIC, _ctgy.SHOW_STAT, _ctgy.FEE_AMNT);
                     }
                     break;
                  case "month":
                     if (Convert.ToInt32(CtgyMont_Txt.EditValue) == 0) {CtgyMont_Txt.Focus(); return;}
                     if (CtgyTmpStr_Txt.Text == "") { CtgyTmpStr_Txt.Focus(); return; }

                     foreach (var _ctgy in CtgyBs1.List.OfType<Data.Category_Belt>())
                     {
                        iScsc.INS_CTGY_P(_ctgy.MTOD_CODE, _ctgy.CTGY_CODE, _ctgy.CTGY_DESC + DuplChar_Txt.Text, string.Format(CtgyTmpStr_Txt.Text, Convert.ToInt32(CtgyMont_Txt.EditValue) * _ctgy.NUMB_OF_ATTN_MONT, CtgyMont_Txt.Text), null, _ctgy.EPIT_TYPE, _ctgy.NUMB_OF_ATTN_MONT * Convert.ToInt32(CtgyMont_Txt.EditValue), _ctgy.NUMB_CYCL_DAY * Convert.ToInt32(CtgyMont_Txt.EditValue), _ctgy.NUMB_MONT_OFER, _ctgy.PRVT_COCH_EXPN, _ctgy.PRIC * Convert.ToInt32(CtgyMont_Txt.EditValue), _ctgy.DFLT_STAT, _ctgy.CTGY_STAT, _ctgy.GUST_NUMB * Convert.ToInt32(CtgyMont_Txt.EditValue), null, _ctgy.RWRD_ATTN_PRIC, _ctgy.SHOW_STAT, _ctgy.FEE_AMNT);
                     }
                     break;
               }               
            }
            else
            {
               switch (_btn.Tag.ToString())
               {
                  case "char":
                     var _ctgy = CtgyBs1.Current as Data.Category_Belt;
                     if (_ctgy == null) return;

                     iScsc.INS_CTGY_P(_ctgy.MTOD_CODE, _ctgy.CTGY_CODE, _ctgy.CTGY_DESC + DuplChar_Txt.Text, _ctgy.CTGY_DESC + DuplChar_Txt.Text, null, _ctgy.EPIT_TYPE, _ctgy.NUMB_OF_ATTN_MONT, _ctgy.NUMB_CYCL_DAY, _ctgy.NUMB_MONT_OFER, _ctgy.PRVT_COCH_EXPN, _ctgy.PRIC, _ctgy.DFLT_STAT, _ctgy.CTGY_STAT, _ctgy.GUST_NUMB , null, _ctgy.RWRD_ATTN_PRIC, _ctgy.SHOW_STAT, _ctgy.FEE_AMNT);
                     break;
                  case "month":
                     if (Convert.ToInt32(CtgyMont_Txt.EditValue) == 0) {CtgyMont_Txt.Focus(); return;}
                     if (CtgyTmpStr_Txt.Text == "") { CtgyTmpStr_Txt.Focus(); return; }

                     foreach (var i in Ctgy_Gv.GetSelectedRows())
                     {
                        _ctgy = Ctgy_Gv.GetRow(i) as Data.Category_Belt;
                        iScsc.INS_CTGY_P(_ctgy.MTOD_CODE, _ctgy.CTGY_CODE, _ctgy.CTGY_DESC + DuplChar_Txt.Text, string.Format(CtgyTmpStr_Txt.Text, Convert.ToInt32(CtgyMont_Txt.EditValue) * _ctgy.NUMB_OF_ATTN_MONT, CtgyMont_Txt.Text), null, _ctgy.EPIT_TYPE, _ctgy.NUMB_OF_ATTN_MONT * Convert.ToInt32(CtgyMont_Txt.EditValue), _ctgy.NUMB_CYCL_DAY * Convert.ToInt32(CtgyMont_Txt.EditValue), _ctgy.NUMB_MONT_OFER, _ctgy.PRVT_COCH_EXPN, _ctgy.PRIC * Convert.ToInt32(CtgyMont_Txt.EditValue), _ctgy.DFLT_STAT, _ctgy.CTGY_STAT, _ctgy.GUST_NUMB * Convert.ToInt32(CtgyMont_Txt.EditValue), null, _ctgy.RWRD_ATTN_PRIC, _ctgy.SHOW_STAT, _ctgy.FEE_AMNT);
                     }
                     break;
               }               
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

      private void FngrPrnt1_Lb_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = CochBs1.Current as Data.Fighter;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                     new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("mbsprwno", 1))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowRbonMnui_Pkb_PickCheckedChange(object sender)
      {
         var stng = StngBs1.Current as Data.Setting;
         if (stng == null) return;

         stng.SHOW_RBON_MNUI = ShowRbonMnui_Pkb.PickChecked ? "002" : "001";
      }

      private void ShowSlidMnui_Pkb_PickCheckedChange(object sender)
      {
         var stng = StngBs1.Current as Data.Setting;
         if (stng == null) return;

         stng.SHOW_SLID_MNUI = ShowSlidMnui_Pkb.PickChecked ? "002" : "001";
      }

      private void AddCEdev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var edev = ExdvBs.Current as Data.External_Device;
            if (edev == null) return;

            if (CExdvBs.List.OfType<Data.External_Device_Link_External_Device>().Any(ele => ele.CODE == 0)) return;

            var cedev = CExdvBs.AddNew() as Data.External_Device_Link_External_Device;
            iScsc.External_Device_Link_External_Devices.InsertOnSubmit(cedev);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelCEdev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cedev = CExdvBs.Current as Data.External_Device_Link_External_Device;
            if (cedev == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.External_Device_Link_External_Device WHERE Code = {0};", cedev.CODE));

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

      private void SaveCEdev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CExdvGv.PostEditor();
            CExdvBs.EndEdit();

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

      private void DupAnydeskPath_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var coma = ComaBs.Current as Data.Computer_Action;
            if (coma == null) return;

            ComaBs.List.OfType<Data.Computer_Action>().Where(c => c.CODE != coma.CODE).ToList().ForEach(c => c.ANY_DESK_PATH = coma.ANY_DESK_PATH);

            SaveComa_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveEdvw_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            EdvwGv.PostEditor();
            EdwtGv.PostEditor();

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

      private void AddEdvw_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edev = ExdvBs.Current as Data.External_Device;
            if (_edev == null) return;

            if (EdvwBs.List.OfType<Data.External_Device_Weekday>().Any(ex => ex.CODE == 0)) return;

            var _edvw = EdvwBs.AddNew() as Data.External_Device_Weekday;
            _edvw.External_Device = _edev;
            _edvw.STAT = "002";

            iScsc.External_Device_Weekdays.InsertOnSubmit(_edvw);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelEdvw_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edvw = EdvwBs.Current as Data.External_Device_Weekday;
            if (_edvw == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.External_Device_Weekdays.DeleteOnSubmit(_edvw);
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

      private void AddEdwt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edvw = EdvwBs.Current as Data.External_Device_Weekday;
            if (_edvw == null) return;

            if (EdwtBs.List.OfType<Data.External_Device_Weekday_Timing>().Any(ex => ex.CODE == 0)) return;

            var _edwt = EdwtBs.AddNew() as Data.External_Device_Weekday_Timing;
            _edwt.External_Device_Weekday = _edvw;
            _edwt.STRT_TIME = DateTime.Now;
            _edwt.END_TIME = DateTime.Now.AddHours(2);
            _edwt.STAT = "002";

            iScsc.External_Device_Weekday_Timings.InsertOnSubmit(_edwt);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelEdwt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edwt = EdwtBs.Current as Data.External_Device_Weekday_Timing;
            if (_edwt == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.External_Device_Weekday_Timings.DeleteOnSubmit(_edwt);
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

      private void EdevEven_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edev = ExdvBs.Current as Data.External_Device;
            if (_edev == null) return;

            iScsc.ExecuteCommand(
               string.Format(
               "MERGE dbo.External_Device_Weekday T " + 
               "USING (SELECT w.Valu FROM D$Wkdy w WHERE w.Valu IN ('002', '004', '007')) S " + 
               "ON (T.Edev_Code = {0} AND T.Week_Day = S.Valu) " +
               "WHEN NOT MATCHED THEN INSERT (Edev_Code, Week_Day, Stat, Code) VALUES ({0}, s.Valu, '002', dbo.GNRT_NVID_U());",
               _edev.CODE
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

      private void EdevOdd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edev = ExdvBs.Current as Data.External_Device;
            if (_edev == null) return;

            iScsc.ExecuteCommand(
               string.Format(
               "MERGE dbo.External_Device_Weekday T " +
               "USING (SELECT w.Valu FROM D$Wkdy w WHERE w.Valu NOT IN ('002', '004', '006', '007')) S " +
               "ON (T.Edev_Code = {0} AND T.Week_Day = S.Valu) " +
               "WHEN NOT MATCHED THEN INSERT (Edev_Code, Week_Day, Stat, Code) VALUES ({0}, s.Valu, '002', dbo.GNRT_NVID_U());",
               _edev.CODE
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

      private void EdevFridy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edev = ExdvBs.Current as Data.External_Device;
            if (_edev == null) return;

            iScsc.ExecuteCommand(
               string.Format(
               "MERGE dbo.External_Device_Weekday T " +
               "USING (SELECT w.Valu FROM D$Wkdy w WHERE w.Valu IN ('006')) S " +
               "ON (T.Edev_Code = {0} AND T.Week_Day = S.Valu) " +
               "WHEN NOT MATCHED THEN INSERT (Edev_Code, Week_Day, Stat, Code) VALUES ({0}, s.Valu, '002', dbo.GNRT_NVID_U());",
               _edev.CODE
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

      private DragDropEffects GetDragDropEffect(TreeList tl, TreeListNode dragNode)
      {
         TreeListNode targetNode;
         Point p = tl.PointToClient(MousePosition);
         targetNode = tl.CalcHitInfo(p).Node;

         if (dragNode != null && targetNode != null
             && dragNode != targetNode
            /*&& dragNode.ParentNode == targetNode.ParentNode*/)
            return DragDropEffects.Move;
         else
            return DragDropEffects.None;
      }      

      private void i_Tl_DragOver(object sender, DragEventArgs e)
      {
         TreeListNode dragNode = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
         e.Effect = GetDragDropEffect(sender as TreeList, dragNode);
      }

      private void i_Tl_CalcNodeDragImageIndex(object sender, DevExpress.XtraTreeList.CalcNodeDragImageIndexEventArgs e)
      {
         TreeList tl = sender as TreeList;
         if (GetDragDropEffect(tl, tl.FocusedNode) == DragDropEffects.None)
            e.ImageIndex = -1;  // no icon
         else
            e.ImageIndex = 1;  // the reorder icon (a curved arrow)
      }

      private void Mtod_Tl_DragDrop(object sender, DragEventArgs e)
      {
         try
         {
            TreeListNode dragNode, targetNode;
            TreeList tl = sender as TreeList;
            Point p = tl.PointToClient(new Point(e.X, e.Y));

            dragNode = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
            targetNode = tl.CalcHitInfo(p).Node;

            //iRoboTech.UPD_PMNU_P((long?)dragNode.GetValue("MUID"), (long?)targetNode.GetValue("MUID"));
            if ((long?)dragNode.GetValue("CODE") != 0)
            {
               iScsc.ExecuteCommand(
                  string.Format("UPDATE dbo.Method SET Mtod_Code = {0} WHERE Code = {1};", 
                     (long?)targetNode.GetValue("CODE"), 
                     (long?)dragNode.GetValue("CODE"))
               );
               requery = true;
            }

            tl.SetNodeIndex(dragNode, tl.GetNodeIndex(targetNode));
            e.Effect = DragDropEffects.None;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Ctgy_Tl_DragDrop(object sender, DragEventArgs e)
      {
         try
         {
            TreeListNode dragNode, targetNode;
            TreeList tl = sender as TreeList;
            Point p = tl.PointToClient(new Point(e.X, e.Y));

            dragNode = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
            targetNode = tl.CalcHitInfo(p).Node;

            //iRoboTech.UPD_PMNU_P((long?)dragNode.GetValue("MUID"), (long?)targetNode.GetValue("MUID"));
            if ((long?)dragNode.GetValue("CODE") != 0)
            {
               iScsc.ExecuteCommand(
                  string.Format("UPDATE dbo.Category_Belt SET Ctgy_Code = {0} WHERE Code = {1};",
                     (long?)targetNode.GetValue("CODE"),
                     (long?)dragNode.GetValue("CODE"))
               );
               requery = true;
            }

            tl.SetNodeIndex(dragNode, tl.GetNodeIndex(targetNode));
            e.Effect = DragDropEffects.None;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void UpLvlMtod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs1.Current as Data.Method;
            if (_mtod == null) return;

            var _upMtod = _mtod.Method1;
            
            if(_upMtod != null)
            {
               iScsc.ExecuteCommand(
                  string.Format("UPDATE dbo.Method SET Mtod_Code = {0} WHERE Code = {1};", 
                     _upMtod.MTOD_CODE == null ? "NULL" : _upMtod.MTOD_CODE.ToString(),
                     _mtod.CODE
                  )
               );
            }
            else
            {
               iScsc.ExecuteCommand(
                  string.Format("UPDATE dbo.Method SET Mtod_Code = NULL WHERE Code = {0};",
                     _mtod.CODE
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
            if (requery)
               Execute_Query();
         }
      }

      private void UpLvlCtgy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _ctgy = CtgyBs1.Current as Data.Category_Belt;
            if (_ctgy == null) return;

            var _upCtgy = _ctgy.Category_Belt1;

            if (_upCtgy != null)
            {
               iScsc.ExecuteCommand(
                  string.Format("UPDATE dbo.Category_Belt SET Ctgy_Code = {0} WHERE Code = {1};",
                     _upCtgy.CTGY_CODE == null ? "NULL" : _upCtgy.CTGY_CODE.ToString(),
                     _ctgy.CODE
                  )
               );
            }
            else
            {
               iScsc.ExecuteCommand(
                  string.Format("UPDATE dbo.Category_Belt SET Ctgy_Code = NULL WHERE Code = {0};",
                     _ctgy.CODE
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
            if (requery)
               Execute_Query();
         }
      }

      private void ClenCochCbmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CochStrtCbmt_Te.EditValue = CochEndCbmt_Te.EditValue = null;
            CochSex_Lov.EditValue = null;
            CochDayType_Lov.EditValue = "003";
            CochCapCbmt_Lov.EditValue = "002";
            CochCapNCbmt_Txt.EditValue = 0;
            CochTimeCbmt_Lov.EditValue = "002";
            CochTimNCbmt_Txt.EditValue = 90;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveCochCbmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _club = ClubBs1.Current as Data.Club;
            if (_club == null) return;

            var _coch = CochBs1.Current as Data.Fighter;
            if (_coch == null) return;

            var _mtod = MtodBs1.Current as Data.Method;
            if (_mtod == null) return;

            Data.Club_Method _cbmt = null;
            if (CbmtBs2.List.OfType<Data.Club_Method>().Any(c => c.CODE == 0))
            {
               _cbmt = CbmtBs2.List.OfType<Data.Club_Method>().FirstOrDefault(c => c.CODE == 0);
               _cbmt.CLUB_CODE = _club.CODE;
               _cbmt.COCH_FILE_NO = _coch.FILE_NO;
               _cbmt.MTOD_CODE = _mtod.CODE;
               _cbmt.STRT_TIME = ((DateTime)CochStrtCbmt_Te.EditValue).TimeOfDay;
               _cbmt.END_TIME = ((DateTime)CochEndCbmt_Te.EditValue).TimeOfDay;
               _cbmt.SEX_TYPE = (string)CochSex_Lov.EditValue;
               _cbmt.DAY_TYPE = (string)CochDayType_Lov.EditValue;
               _cbmt.CPCT_STAT = (string)CochCapCbmt_Lov.EditValue;
               _cbmt.CPCT_NUMB = Convert.ToInt32(CochCapNCbmt_Txt.EditValue);
               _cbmt.CBMT_TIME_STAT = (string)CochTimeCbmt_Lov.EditValue;
               _cbmt.CLAS_TIME = Convert.ToInt32(CochTimNCbmt_Txt.EditValue);
            }
            else
            {
               _cbmt =
                  new Data.Club_Method()
                  {
                     CLUB_CODE = _club.CODE,
                     COCH_FILE_NO = _coch.FILE_NO,
                     MTOD_CODE = _mtod.CODE,
                     STRT_TIME = ((DateTime)CochStrtCbmt_Te.EditValue).TimeOfDay,
                     END_TIME = ((DateTime)CochEndCbmt_Te.EditValue).TimeOfDay,
                     SEX_TYPE = (string)CochSex_Lov.EditValue,
                     DAY_TYPE = (string)CochDayType_Lov.EditValue,
                     CPCT_STAT = (string)CochCapCbmt_Lov.EditValue,
                     CPCT_NUMB = Convert.ToInt32(CochCapNCbmt_Txt.EditValue),
                     CBMT_TIME_STAT = (string)CochTimeCbmt_Lov.EditValue,
                     CLAS_TIME = Convert.ToInt32(CochTimNCbmt_Txt.EditValue)
                  };
            }

            if (_cbmt.CODE == 0)
               iScsc.INS_CBMT_P(_cbmt.CLUB_CODE, _cbmt.MTOD_CODE, _cbmt.COCH_FILE_NO, _cbmt.DAY_TYPE, _cbmt.STRT_TIME, _cbmt.END_TIME, _cbmt.SEX_TYPE, _cbmt.CBMT_DESC, _cbmt.DFLT_STAT, _cbmt.CPCT_NUMB, _cbmt.CPCT_STAT, _cbmt.CBMT_TIME, _cbmt.CBMT_TIME_STAT, _cbmt.CLAS_TIME, _cbmt.AMNT, _cbmt.NATL_CODE, _cbmt.DURT_ATTN_SOND_PATH);
            else
               iScsc.UPD_CBMT_P(_cbmt.CODE, _cbmt.CLUB_CODE, _cbmt.MTOD_CODE, _cbmt.COCH_FILE_NO, _cbmt.MTOD_STAT, _cbmt.DAY_TYPE, _cbmt.STRT_TIME, _cbmt.END_TIME, _cbmt.SEX_TYPE, _cbmt.CBMT_DESC, _cbmt.DFLT_STAT, _cbmt.CPCT_NUMB, _cbmt.CPCT_STAT, _cbmt.CBMT_TIME, _cbmt.CBMT_TIME_STAT, _cbmt.CLAS_TIME, _cbmt.AMNT, _cbmt.NATL_CODE, _cbmt.DURT_ATTN_SOND_PATH);

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

      private void ActvCoch_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            CochBs1.DataSource =
               iScsc.Fighters
               .Where(c => 
                  c.FGPB_TYPE_DNRM == "003" &&
                  (
                     ActvCoch_Cbx.Checked ?
                     c.Club_Methods
                     .Any(cm => 
                        cm.MTOD_STAT == "002" &&
                        cm.Method.MTOD_STAT == "002"
                     ) :
                     true
                  )
               );
         }
         catch (Exception exc)
         {

         }
      }
   }
}
