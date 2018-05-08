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
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.JobRouting.Jobs;
using System.IO;

namespace System.Scsc.Ui.Settings
{
   public partial class CNF_STNG_F : UserControl
   {
      public CNF_STNG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if(tc_master.SelectedTab == tp_001)
         {
            ClubBs1.DataSource = iScsc.Clubs;
            int si = StngBs1.Position;
            StngBs1.DataSource = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE));
            StngBs1.Position = si;
         }
         else if(tc_master.SelectedTab == tp_002)
         {
            int c = CashBs1.Position;
            int e = EpitBs1.Position;
            CashBs1.DataSource = iScsc.Cashes;
            EpitBs1.DataSource = iScsc.Expense_Items;
            CashBs1.Position = c;
            EpitBs1.Position = e;
         }
      }

      private void StngBnReload1_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void StngBnASav1_Click(object sender, EventArgs e)
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
                     new XAttribute("hldycont", Stng.HLDY_CONT ?? 1)
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

      private void StngBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void CashBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         iScsc.CommandTimeout = 1800;
         CashBs1.EndEdit();
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
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
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
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
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               default:
                  MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
                  break;
            }
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

      private void EpitBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         iScsc.CommandTimeout = 1800;
         EpitBs1.EndEdit();
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن آیتم هزینه موافقید؟", "حذف آیتم هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "010"),
                        new XElement("Delete",
                           new XElement("Expense_Item",
                              new XAttribute("code", (EpitBs1.Current as Data.Expense_Item).CODE)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  var crnt = EpitBs1.Current as Data.Expense_Item;

                  if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "010"),
                        EpitBs1.List.OfType<Data.Expense_Item>().Where(c => c.CRET_BY == null).Select(c =>
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
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               default:
                  MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
                  break;
            }
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

      private void tYPELookUpEdit1_EditValueChanged(object sender, EventArgs e)
      {
         if (tYPELookUpEdit1.ItemIndex == -1) return;

         radGroupBox5.Enabled = tYPELookUpEdit1.EditValue.ToString() == "001" || tYPELookUpEdit1.EditValue.ToString() == "003" ? true : false;
      }

      private void StngBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var crnt = StngBs1.Current as Data.Setting;

            if (crnt == null) return;            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
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
   }
}
