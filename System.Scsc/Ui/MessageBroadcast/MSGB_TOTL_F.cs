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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.MessageBroadcast
{
   public partial class MSGB_TOTL_F : UserControl
   {
      public MSGB_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         MsgbBs1.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "001");
         MsgbBs4.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "002");
         MsgbBs2.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "003");
         MsgbBs3.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "004");         
         MsgbBs5.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "005");
         MsgbBs6.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "006");
         MsgbBs7.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "007");
         MsgbBs8.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "008");
         MsgbBs9.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "009");
         MsgbBs10.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "010");
         MsgbBs11.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "011");
         MsgbBs12.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "012");
         MsgbBs13.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "013");
         MsgbBs14.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "014");
         MsgbBs15.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "015");
         MsgbBs16.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "016");
         MsgbBs17.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "017");
         MsgbBs18.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "018");
         MsgbBs19.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "019");
         MsgbBs20.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "020");
         MsgbBs21.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "021");
         MsgbBs22.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "022");
         MsgbBs23.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "023");
         MsgbBs24.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "024");
         MsgbBs25.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "025");
         MsgbBs26.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "026");
         MsgbBs27.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "027");
         MsgbBs28.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "028");
         MsgbBs29.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "029");
         MsgbBs30.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "030");
         MsgbBs31.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "031");
         MsgbBs32.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "032");
         MsgbBs33.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "033");
         MsgbBs34.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "034");
         MsgbBs35.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "035");
         MsgbBs36.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "036");
         MsgbBs37.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "037");
         MsgbBs38.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "038");
         MsgbBs39.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "039");
         MsgbBs40.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "040");

         requery = false;
      }

      private void Msgb_Text00X_TextChanged(object sender, EventArgs e)
      {
         RichTextBox obj = sender as RichTextBox;

         switch (obj.Name)
         {
            case "Msgb_Text001":
               Msg_Count_Char001_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text002":
               Msg_Count_Char002_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text003":
               Msg_Count_Char003_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text004":
               Msg_Count_Char004_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text005":
               Msg_Count_Char005_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text006":
               Msg_Count_Char006_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text007":
               Msg_Count_Char007_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text008":
               Msg_Count_Char008_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text009":
               Msg_Count_Char009_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text010":
               Msg_Count_Char010_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text011":
               Msg_Count_Char011_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text012":
               Msg_Count_Char012_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text013":
               Msg_Count_Char013_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text014":
               Msg_Count_Char014_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text015":
               Msg_Count_Char015_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text016":
               Msg_Count_Char016_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text017":
               Msg_Count_Char017_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text018":
               Msg_Count_Char018_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text019":
               Msg_Count_Char019_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text020":
               Msg_Count_Char020_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text021":
               Msg_Count_Char021_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text022":
               Msg_Count_Char022_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text023":
               Msg_Count_Char023_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text024":
               Msg_Count_Char024_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text025":
               Msg_Count_Char025_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text026":
               Msg_Count_Char026_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text027":
               Msg_Count_Char027_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text028":
               Msg_Count_Char028_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text029":
               Msg_Count_Char029_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text030":
               Msg_Count_Char030_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text031":
               Msg_Count_Char031_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text032":
               Msg_Count_Char032_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text033":
               Msg_Count_Char033_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text034":
               Msg_Count_Char034_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text035":
               Msg_Count_Char035_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text036":
               Msg_Count_Char036_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text037":
               Msg_Count_Char037_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text038":
               Msg_Count_Char038_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text039":
               Msg_Count_Char039_Txt.Text = obj.Text.Length.ToString();
               break;
            case "Msgb_Text040":
               Msg_Count_Char040_Txt.Text = obj.Text.Length.ToString();
               break;

            default:
               break;
         }
      }

      private void DayType_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "TIME_DESC" && e.IsGetData)
         {
            var h = ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "END_TIME")).Hours;
            e.Value = h >= 0 && h < 12 ? "صبح" : h >= 12 && h < 15 ? "ظهر" : h >= 15 && h < 18 ? "بعد ظهر" : h >= 18 ? "عصر" : "نام مشخص";
         }
      }

      private void Rb_Selected00X_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            RadioButton obj = sender as RadioButton;
            if (!obj.Checked) return;
            string sex = "000";
            switch (tb_master.SelectedTab.Name)
            {
               case "tp_002":
                  sex = Rb_Men002.Checked ? "001" : (Rb_Women002.Checked ? "002" : "000");
                  break;
               case "tp_003":
                  sex = Rb_Men003.Checked ? "001" : (Rb_Women003.Checked ? "002" : "000");
                  break;
               case "tp_004":
                  sex = Rb_Men004.Checked ? "001" : (Rb_Women004.Checked ? "002" : "000");
                  break;
               case "tp_005":
                  sex = Rb_Men005.Checked ? "001" : (Rb_Women005.Checked ? "002" : "000");
                  break;
            }
            switch (obj.Name)
            {
               case "Rb_All002":
                  FLP_002.Visible = true;
                  Pn_Step2002.Visible = false;
                  Pn_Step3002.Visible = true;
                  FighBs2.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000");
                  Figh_CheckedComboBoxEdit002.CheckAll();
                  Figh_CheckedComboBoxEdit002.Enabled = false;
                  AllFigh_Count002_Txt.Text = FighBs2.Count.ToString();
                  break;
               case "Rb_Deactive002":
                  FLP_002.Visible = true;
                  Pn_Step2002.Visible = false;
                  Pn_Step3002.Visible = true;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit002.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit002.Enabled = true;
                  AllFigh_Count002_Txt.Text = FighBs2.Count.ToString();
                  break;
               case "Rb_SpecFighter002":
                  FLP_002.Visible = true;
                  Pn_Step2002.Visible = true;
                  Pn_Step3002.Visible = true;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit002.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit002.Enabled = true;
                  AllFigh_Count002_Txt.Text = FighBs2.Count.ToString();
                  break;
               case "Rb_ManualSelection002":
                  FLP_002.Visible = true;
                  Pn_Step2002.Visible = false;
                  Pn_Step3002.Visible = true;
                  FighBs2.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000");
                  //Figh_CheckedComboBoxEdit002.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => c.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit002.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit002.Enabled = true;
                  AllFigh_Count002_Txt.Text = FighBs2.Count.ToString();
                  break;
               case "Rb_All003":
                  FLP_003.Visible = true;
                  Pn_Step2003.Visible = false;
                  Pn_Step3003.Visible = true;
                  FighBs3.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000");
                  Figh_CheckedComboBoxEdit003.CheckAll();
                  Figh_CheckedComboBoxEdit003.Enabled = false;
                  AllFigh_Count003_Txt.Text = FighBs3.Count.ToString();
                  break;
               case "Rb_Deactive003":
                  FLP_003.Visible = true;
                  Pn_Step2003.Visible = false;
                  Pn_Step3003.Visible = true;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit003.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit003.Enabled = true;
                  AllFigh_Count003_Txt.Text = FighBs3.Count.ToString();
                  break;
               case "Rb_SpecFighter003":
                  FLP_003.Visible = true;
                  Pn_Step2003.Visible = true;
                  Pn_Step3003.Visible = true;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit003.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit003.Enabled = true;
                  AllFigh_Count003_Txt.Text = FighBs3.Count.ToString();
                  break;
               case "Rb_ManualSelection003":
                  FLP_003.Visible = true;
                  Pn_Step2003.Visible = false;
                  Pn_Step3003.Visible = true;
                  FighBs3.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000");
                  //Figh_CheckedComboBoxEdit003.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => c.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit003.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit003.Enabled = true;
                  AllFigh_Count003_Txt.Text = FighBs3.Count.ToString();
                  break;

               case "Rb_All004":
                  FLP_004.Visible = true;
                  Pn_Step2004.Visible = false;
                  Pn_Step3004.Visible = true;
                  FighBs4.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000").Where(f => f.DEBT_DNRM >= Convert.ToInt64(Debt_Pric_TextEdit004.EditValue));
                  Figh_CheckedComboBoxEdit004.CheckAll();
                  Figh_CheckedComboBoxEdit004.Enabled = false;
                  AllFigh_Count004_Txt.Text = FighBs4.Count.ToString();
                  break;
               case "Rb_Deactive004":
                  FLP_004.Visible = true;
                  Pn_Step2004.Visible = false;
                  Pn_Step3004.Visible = true;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit004.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit004.Enabled = true;
                  AllFigh_Count004_Txt.Text = FighBs4.Count.ToString();
                  break;
               case "Rb_SpecFighter004":
                  FLP_004.Visible = true;
                  Pn_Step2004.Visible = true;
                  Pn_Step3004.Visible = true;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit004.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit004.Enabled = true;
                  AllFigh_Count004_Txt.Text = FighBs4.Count.ToString();
                  break;
               case "Rb_ManualSelection004":
                  FLP_004.Visible = true;
                  Pn_Step2004.Visible = false;
                  Pn_Step3004.Visible = true;
                  Debt_Pric_TextEdit004.EditValue = Debt_Pric_TextEdit004.EditValue == null || Debt_Pric_TextEdit004.EditValue.ToString() == "" ? "0" : Debt_Pric_TextEdit004.EditValue;
                  FighBs4.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000").Where(f => f.DEBT_DNRM >= Convert.ToInt64(Debt_Pric_TextEdit004.EditValue));
                  //Figh_CheckedComboBoxEdit004.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => c.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit004.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit004.Enabled = true;
                  AllFigh_Count004_Txt.Text = FighBs4.Count.ToString();
                  break;

               case "Rb_All005":
                  FLP_005.Visible = true;
                  Pn_Step2005.Visible = false;
                  Pn_Step3005.Visible = true;
                  Pn_Step4005.Visible = false;
                  FighBs5.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000");
                  Figh_CheckedComboBoxEdit005.CheckAll();
                  //Figh_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit005.Enabled = false;
                  AllFigh_Count005_Txt.Text = FighBs5.Count.ToString();
                  break;
               case "Rb_Deactive005":
                  FLP_005.Visible = true;
                  Pn_Step2005.Visible = false;
                  Pn_Step3005.Visible = true;
                  Pn_Step4005.Visible = false;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit005.Enabled = true;
                  AllFigh_Count005_Txt.Text = FighBs5.Count.ToString();
                  break;
               case "Rb_SpecFighter005":
                  FLP_005.Visible = true;
                  Pn_Step2005.Visible = true;
                  Pn_Step3005.Visible = true;
                  Pn_Step4005.Visible = false;
                  PrepareForExecuteQuery(null, null);
                  Figh_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit005.Enabled = true;
                  AllFigh_Count005_Txt.Text = FighBs5.Count.ToString();
                  break;
               case "Rb_ManualSelection005":
                  FLP_005.Visible = true;
                  Pn_Step2005.Visible = false;
                  Pn_Step3005.Visible = true;
                  Pn_Step4005.Visible = false;
                  FighBs5.DataSource = ExecuteQuery(new List<long?>(), new List<long?>(), new List<long?>(), sex, "000");
                  //Figh_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => c.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = CheckState.Unchecked);
                  Figh_CheckedComboBoxEdit005.Enabled = true;
                  AllFigh_Count005_Txt.Text = FighBs5.Count.ToString();
                  break;

               case "Rb_PhoneNumberFromFile":
                  FLP_005.Visible = true;
                  Pn_Step2005.Visible = false;
                  Pn_Step3005.Visible = false;
                  Pn_Step4005.Visible = true;
                  Figh_CheckedComboBoxEdit005.Enabled = true;
                  AllFigh_Count005_Txt.Text = PhonNumber_CheckedComboBoxEdit005.Properties.Items.Count.ToString();
                  SlctFigh_Count005_Txt.Text = PhonNumber_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Count().ToString();
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private IEnumerable<Data.Fighter> ExecuteQuery(List<long?> cbmtcodes, List<long?> cochs, List<long?> mtods, string sex = "000", string stat = "000")
      {
         try
         {
            if (iScsc == null) return null;

            var phonnumb = new Regex(@"^09\d{2}\s*?\d{3}\s*?\d{4}$");

            return
               iScsc.Fighters
               .Where(f =>
                  f.CONF_STAT == "002" &&
                  (sex == "000" || f.SEX_TYPE_DNRM == sex) &&
                  (f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Length > 0) &&
                  !(f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") &&
                  f.ACTV_TAG_DNRM.CompareTo("101") >= 0 &&
                  (
                     stat == "000" ||
                     (
                        // اعضا فعال
                        stat == "002" &&
                        f.Member_Ships
                        .Any(
                           ms => ms.TYPE == "001" &&
                                 ms.VALD_TYPE == "002" &&
                                 ms.RECT_CODE == "004" &&
                                 (
                                    (ms.NUMB_OF_ATTN_MONT == 0 && ms.END_DATE.Value.Date >= DateTime.Now.Date) ||
                                    (ms.NUMB_OF_ATTN_MONT != 0 && ms.SUM_ATTN_MONT_DNRM <= ms.NUMB_OF_ATTN_MONT && ms.END_DATE.Value.Date >= DateTime.Now.Date)
                                 ) &&
                                 (cbmtcodes == null || cbmtcodes.Count == 0 || cbmtcodes.Contains(ms.Fighter_Public.CBMT_CODE)) &&
                                 (cochs == null || cochs.Count == 0 || cochs.Contains(ms.Fighter_Public.COCH_FILE_NO)) &&
                                 (mtods == null || mtods.Count == 0 || mtods.Contains(ms.Fighter_Public.MTOD_CODE))
                        )
                     ) ||
                     (
                        // اعضا غیرفعال
                        stat == "001" &&
                        !f.Member_Ships
                        .Any(
                           ms => ms.TYPE == "001" &&
                                 ms.VALD_TYPE == "002" &&
                                 ms.RECT_CODE == "004" &&
                                 (
                                    (ms.NUMB_OF_ATTN_MONT == 0 && ms.END_DATE.Value.Date >= DateTime.Now.Date) ||
                                    (ms.NUMB_OF_ATTN_MONT != 0 && ms.SUM_ATTN_MONT_DNRM <= ms.NUMB_OF_ATTN_MONT && ms.END_DATE.Value.Date >= DateTime.Now.Date)
                                 ) /*&&
                                 (cbmtcodes == null || cbmtcodes.Count == 0 || cbmtcodes.Contains(ms.Fighter_Public.CBMT_CODE)) &&
                                 (cochs == null || cochs.Count == 0 || cochs.Contains(ms.Fighter_Public.COCH_FILE_NO)) &&
                                 (mtods == null || mtods.Count == 0 || mtods.Contains(ms.Fighter_Public.MTOD_CODE))*/
                        )
                     )
                  )
               ).ToList()
               .Where(r => phonnumb.IsMatch(r.CELL_PHON_DNRM))
               .OrderBy(f => f.NAME_DNRM);
         }
         catch { return null; }
      }

      private void PrepareForExecuteQuery(object sender, EventArgs e)
      {
         try
         {
            List<long?> cbmtcodes, cochs, mtods;
            cbmtcodes = new List<long?>();
            cochs = new List<long?>();
            mtods = new List<long?>();

            string sex = "000";
            string stat = "000";
            switch (tb_master.SelectedTab.Name)
            {
               case "tp_002":
                  sex = Rb_Men002.Checked ? "001" : (Rb_Women002.Checked ? "002" : "000");
                  stat = Rb_Deactive002.Checked ? "001" : "002";
                  break;
               case "tp_003":
                  sex = Rb_Men003.Checked ? "001" : (Rb_Women003.Checked ? "002" : "000");
                  stat = Rb_Deactive003.Checked ? "001" : "002";
                  break;
               case "tp_004":
                  sex = Rb_Men004.Checked ? "001" : (Rb_Women004.Checked ? "002" : "000");
                  stat = Rb_Deactive004.Checked ? "001" : "002";
                  break;
               case "tp_005":
                  sex = Rb_Men005.Checked ? "001" : (Rb_Women005.Checked ? "002" : "000");
                  stat = Rb_Deactive005.Checked ? "001" : "002";
                  break;
            }

            if (tb_master.SelectedTab == tp_002)
            {
               if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit002.EditValue) > 0)
                  if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit002.EditValue) > 0)
                     cbmtcodes.Add((long?)CBMT_CODE_GridLookUpEdit002.EditValue);

               Coch_CheckedComboBoxEdit002.Properties.Items.GetCheckedValues().ForEach(c => cochs.Add((long?)c));
               Mtod_CheckedComboBoxEdit002.Properties.Items.GetCheckedValues().ForEach(m => mtods.Add((long?)m));

               FighBs2.DataSource = ExecuteQuery(cbmtcodes, cochs, mtods, sex, stat);
            }
            else if (tb_master.SelectedTab == tp_003)
            {
               if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit003.EditValue) > 0)
                  if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit003.EditValue) > 0)
                     cbmtcodes.Add((long?)CBMT_CODE_GridLookUpEdit003.EditValue);

               Coch_CheckedComboBoxEdit003.Properties.Items.GetCheckedValues().ForEach(c => cochs.Add((long?)c));
               Mtod_CheckedComboBoxEdit003.Properties.Items.GetCheckedValues().ForEach(m => mtods.Add((long?)m));

               FighBs3.DataSource = ExecuteQuery(cbmtcodes, cochs, mtods, sex, stat);
            }
            else if (tb_master.SelectedTab == tp_004)
            {
               if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit004.EditValue) > 0)
                  if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit004.EditValue) > 0)
                     cbmtcodes.Add((long?)CBMT_CODE_GridLookUpEdit004.EditValue);

               Coch_CheckedComboBoxEdit004.Properties.Items.GetCheckedValues().ForEach(c => cochs.Add((long?)c));
               Mtod_CheckedComboBoxEdit004.Properties.Items.GetCheckedValues().ForEach(m => mtods.Add((long?)m));
               Debt_Pric_TextEdit004.EditValue = Debt_Pric_TextEdit004.EditValue == null || Debt_Pric_TextEdit004.EditValue.ToString() == "" ? 0 : Debt_Pric_TextEdit004.EditValue;
               long debtpric = Convert.ToInt64(Debt_Pric_TextEdit004.EditValue);
               FighBs4.DataSource = ExecuteQuery(cbmtcodes, cochs, mtods, sex, stat).Where(f => f.DEBT_DNRM >= debtpric);
            }
            else if (tb_master.SelectedTab == tp_005)
            {
               if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit005.EditValue) > 0)
                  if (Convert.ToInt64(CBMT_CODE_GridLookUpEdit005.EditValue) > 0)
                     cbmtcodes.Add((long?)CBMT_CODE_GridLookUpEdit005.EditValue);

               Coch_CheckedComboBoxEdit005.Properties.Items.GetCheckedValues().ForEach(c => cochs.Add((long?)c));
               Mtod_CheckedComboBoxEdit005.Properties.Items.GetCheckedValues().ForEach(m => mtods.Add((long?)m));

               FighBs5.DataSource = ExecuteQuery(cbmtcodes, cochs, mtods, sex, stat);
            }
         }
         catch { }
      }

      private void Ofd_Browse_Click(object sender, EventArgs e)
      {
         if (ofd_selector.ShowDialog() != Windows.Forms.DialogResult.OK)
            return;

         FileName_TextEdit005.Text = ofd_selector.FileName;

         DataTable dt = new DataTable("Data");
         dt.Columns.AddRange(
            new[] {
               new DataColumn("Value", typeof(object))
            });

         StreamReader sr = new StreamReader(ofd_selector.FileName);
         string line;
         while ((line = sr.ReadLine()) != null)
         {
            if ( line != "" )
            {
               DataRow dr = dt.NewRow();
               dr["Value"] = line;
               dt.Rows.Add(dr);
            }
         }

         PhonNumber_CheckedComboBoxEdit005.Properties.DataSource = dt;
         PhonNumber_CheckedComboBoxEdit005.Properties.DisplayMember = "Value";
         PhonNumber_CheckedComboBoxEdit005.Properties.ValueMember = "Value";
         PhonNumber_CheckedComboBoxEdit005.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = Windows.Forms.CheckState.Checked);
         AllFigh_Count005_Txt.Text = PhonNumber_CheckedComboBoxEdit005.Properties.Items.Count.ToString();
      }

      private void Btn_Save_Click(object sender, EventArgs e)
      {
         try
         {
            MsgbBs1.EndEdit();
            MsgbBs2.EndEdit();
            MsgbBs3.EndEdit();
            MsgbBs4.EndEdit();
            MsgbBs5.EndEdit();
            MsgbBs6.EndEdit();
            MsgbBs7.EndEdit();
            MsgbBs8.EndEdit();
            MsgbBs9.EndEdit();
            MsgbBs10.EndEdit();
            MsgbBs11.EndEdit();
            MsgbBs12.EndEdit();
            MsgbBs13.EndEdit();
            MsgbBs14.EndEdit();
            MsgbBs15.EndEdit();
            MsgbBs16.EndEdit();
            MsgbBs17.EndEdit();
            MsgbBs18.EndEdit();
            MsgbBs19.EndEdit();
            MsgbBs20.EndEdit();
            MsgbBs21.EndEdit();
            MsgbBs22.EndEdit();
            MsgbBs23.EndEdit();
            MsgbBs24.EndEdit();
            MsgbBs25.EndEdit();
            MsgbBs26.EndEdit();
            MsgbBs27.EndEdit();
            MsgbBs28.EndEdit();
            MsgbBs29.EndEdit();
            MsgbBs30.EndEdit();
            MsgbBs31.EndEdit();
            MsgbBs32.EndEdit();
            MsgbBs33.EndEdit();
            MsgbBs34.EndEdit();
            MsgbBs35.EndEdit();
            MsgbBs36.EndEdit();
            MsgbBs37.EndEdit();
            MsgbBs38.EndEdit();
            MsgbBs39.EndEdit();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ذخیره سازی اطلاعات");
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Btn_MsgSend002_Click(object sender, EventArgs e)
      {
         try
         {
            #region Precondiotion
            var crnt = MsgbBs2.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            string msg = "";

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            msg = crnt.MSGB_TEXT;

            string clubname = "";

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  clubname = "\n" + crnt.CLUB_NAME;
               else
                  clubname = "\n" + crnt.Club.NAME;
            }
            #endregion

            if (MessageBox.Show(this, "آیا با ارسال پیامک موافق هستین؟", "مجوز ارسال پیامک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            var phonnumbs = Figh_CheckedComboBoxEdit002.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked);

            iScsc.MSG_SEND_P(
               new XElement("Process",
                  new XElement("Contacts",
                     new XAttribute("subsys", 5),
                     new XAttribute("linetype", crnt.LINE_TYPE),
                     phonnumbs.Select(pn =>
                        new XElement("Contact",
                           new XAttribute("phonnumb", FighBs2.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).CELL_PHON_DNRM),
                           new XElement("Message",
                              new XAttribute("type", crnt.MSGB_TYPE),
                              crnt.INSR_FNAM_STAT == "002" ? (
                                 FighBs2.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).SEX_TYPE_DNRM == "001" ? (
                                    string.Format("{0} {1}\n{2}{3}", "آقای", pn.Description, msg, clubname)
                                 ) : (
                                    string.Format("{0} {1}\n{2}{3}", "خانم", pn.Description, msg, clubname)
                                 )
                              ) : (
                                 string.Format("{2}{3}", msg, clubname)
                              )
                           )
                        )
                     )
                  )
               )
            );

            MessageBox.Show(this, "پیامهای ارسالی در صف انتظار برای ارسال قرار گرفتند");
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); }
      }

      private void Btn_MsgSend003_Click(object sender, EventArgs e)
      {
         try
         {
            #region Precondiotion
            var crnt = MsgbBs3.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            string msg = "";

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            msg = crnt.MSGB_TEXT;

            string clubname = "";

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  clubname = "\n" + crnt.CLUB_NAME;
               else
                  clubname = "\n" + crnt.Club.NAME;
            }
            #endregion

            if (MessageBox.Show(this, "آیا با ارسال پیامک موافق هستین؟", "مجوز ارسال پیامک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            var phonnumbs = Figh_CheckedComboBoxEdit003.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked);

            iScsc.MSG_SEND_P(
               new XElement("Process",
                  new XElement("Contacts",
                     new XAttribute("subsys", 5),
                     new XAttribute("linetype", crnt.LINE_TYPE),
                     phonnumbs.Select(pn =>
                        new XElement("Contact",
                           new XAttribute("phonnumb", FighBs3.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).CELL_PHON_DNRM),
                           new XElement("Message",
                              new XAttribute("type", crnt.MSGB_TYPE),
                              crnt.INSR_FNAM_STAT == "002" ? (
                                 FighBs3.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).SEX_TYPE_DNRM == "001" ? (
                                    string.Format("{0} {1}\n{2}{3}", "آقای", pn.Description, msg, clubname)
                                 ) : (
                                    string.Format("{0} {1}\n{2}{3}", "خانم", pn.Description, msg, clubname)
                                 )
                              ) : (
                                 string.Format("{2}{3}", msg, clubname)
                              )
                           )
                        )
                     )
                  )
               )
            );

            MessageBox.Show(this, "پیامهای ارسالی در صف انتظار برای ارسال قرار گرفتند");
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); }
      }

      private void Btn_MsgSend004_Click(object sender, EventArgs e)
      {
         try
         {
            #region Precondiotion
            var crnt = MsgbBs4.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            string msg = "";

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            msg = crnt.MSGB_TEXT;

            string clubname = "";

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  clubname = "\n" + crnt.CLUB_NAME;
               else
                  clubname = "\n" + crnt.Club.NAME;
            }
            #endregion

            if (MessageBox.Show(this, "آیا با ارسال پیامک موافق هستین؟", "مجوز ارسال پیامک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            var phonnumbs = Figh_CheckedComboBoxEdit004.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked);

            iScsc.MSG_SEND_P(
               new XElement("Process",
                  new XElement("Contacts",
                     new XAttribute("subsys", 5),
                     new XAttribute("linetype", crnt.LINE_TYPE),
                     phonnumbs.Select(pn =>
                        new XElement("Contact",
                           new XAttribute("phonnumb", FighBs4.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).CELL_PHON_DNRM),
                           new XElement("Message",
                              new XAttribute("type", crnt.MSGB_TYPE),
                              crnt.INSR_FNAM_STAT == "002" ? (
                                 FighBs4.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).SEX_TYPE_DNRM == "001" ? (
                                    string.Format("{0} {1}\n{2}{3}", "آقای", pn.Description, msg, clubname)
                                 ) : (
                                    string.Format("{0} {1}\n{2}{3}", "خانم", pn.Description, msg, clubname)
                                 )
                              ) : (
                                 string.Format("{2}{3}", msg, clubname)
                              )
                           )
                        )
                     )
                  )
               )
            );

            MessageBox.Show(this, "پیامهای ارسالی در صف انتظار برای ارسال قرار گرفتند");
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); }
      }

      private void Btn_MsgSend005_Click(object sender, EventArgs e)
      {
         try
         {
            #region Precondiotion
            var crnt = MsgbBs5.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            string msg = "";

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            msg = crnt.MSGB_TEXT;

            string clubname = "";

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام بخش مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  clubname = "\n" + crnt.CLUB_NAME;
               else
                  clubname = "\n" + crnt.Club.NAME;
            }
            #endregion

            if (MessageBox.Show(this, "آیا با ارسال پیامک موافق هستین؟", "مجوز ارسال پیامک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            int i = 0;

            if (Rb_PhoneNumberFromFile.Checked)
            {
               var phonnumbs = PhonNumber_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked);

               iScsc.MSG_SEND_P(
                  new XElement("Process",
                     new XElement("Contacts",
                        new XAttribute("subsys", 5),
                        new XAttribute("linetype", crnt.LINE_TYPE),
                        phonnumbs.Select(pn =>
                           new XElement("Contact",
                              new XAttribute("phonnumb", pn.Description),
                              new XElement("Message",
                                 new XAttribute("type", crnt.MSGB_TYPE),
                                 new XAttribute("scdldate", (crnt.SCDL_DATE == null ? DateTime.Now : (crnt.SCDL_DATE.Value.Date < DateTime.Now.Date ? DateTime.Now : (DateTime)crnt.SCDL_DATE))),
                                 new XAttribute("btchnumb", crnt.BTCH_NUMB ?? 0),
                                 new XAttribute("stepmin", crnt.STEP_MIN ?? 0),
                                 //new XAttribute("actndate", GetActnDate(ref i, (crnt.SCDL_DATE == null ? DateTime.Now : (crnt.SCDL_DATE.Value.Date < DateTime.Now.Date ? DateTime.Now : (DateTime)crnt.SCDL_DATE)), (int)crnt.BTCH_NUMB, (int)crnt.STEP_MIN)),
                                 new XAttribute("sendtype", "002"), // Bulk Send
                                 string.Format("{0}{1}", msg, clubname)
                              )
                           )
                        )
                     )
                  )
               );
            }
            else
            {
               var phonnumbs = Figh_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked);

               iScsc.MSG_SEND_P(
                  new XElement("Process",
                     new XElement("Contacts",
                        new XAttribute("subsys", 5),
                        new XAttribute("linetype", crnt.LINE_TYPE),
                        phonnumbs.Select(pn =>
                           new XElement("Contact",
                              new XAttribute("phonnumb", FighBs5.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).CELL_PHON_DNRM),
                              new XElement("Message",
                                 new XAttribute("type", crnt.MSGB_TYPE),
                                 new XAttribute("sendtype", crnt.INSR_FNAM_STAT == "002" ? "001" : "002"),
                                 new XAttribute("scdldate", (crnt.SCDL_DATE == null ? DateTime.Now : (crnt.SCDL_DATE.Value.Date < DateTime.Now.Date ? DateTime.Now : (DateTime)crnt.SCDL_DATE))),
                                 new XAttribute("btchnumb", crnt.BTCH_NUMB ?? 0),
                                 new XAttribute("stepmin", crnt.STEP_MIN ?? 0),
                                 //new XAttribute("actndate", GetActnDate(ref i, (crnt.SCDL_DATE == null ? DateTime.Now : (crnt.SCDL_DATE.Value < DateTime.Now ? DateTime.Now : (DateTime)crnt.SCDL_DATE)) , (int)crnt.BTCH_NUMB, (int)crnt.STEP_MIN)),
                                 crnt.INSR_FNAM_STAT == "002" ? (
                                    FighBs5.List.OfType<Data.Fighter>().First(f => (long)pn.Value == f.FILE_NO).SEX_TYPE_DNRM == "001" ? (
                                       string.Format("{0} {1}\n{2}{3}", "آقای", pn.Description, msg, clubname)
                                    ) : (
                                       string.Format("{0} {1}\n{2}{3}", "خانم", pn.Description, msg, clubname)
                                    )
                                 ) : (
                                    string.Format("{0}{1}", msg, clubname)
                                 )
                              )
                           )
                        )
                     )
                  )
               );
            }

            MessageBox.Show(this, "پیامهای ارسالی در صف انتظار برای ارسال قرار گرفتند");
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); }
      }
      
      #region Preview
      private void Btn_Preview001_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs1.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview002_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs2.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview003_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs3.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview004_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs4.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            if (crnt.DEBT_PRIC == 0)
            {
               MessageBox.Show("مبلغ بدهی وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview005_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs5.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }
      #endregion

      private void Btn_Preview006_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs6.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview007_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs7.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview008_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs8.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void Btn_Preview009_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs9.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }

      private void GridLookUpEditXXX_ButtonPressed(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            GridLookUpEdit lov = sender as GridLookUpEdit;
            if (e.Button.Kind == ButtonPredefines.Delete)
               lov.EditValue = null;
         }
         catch {}
      }

      private void Figh_CheckedComboBoxEditXXX_EditValueChanging(object sender, ChangingEventArgs e)
      {
         try
         {
            var lov = sender as CheckedComboBoxEdit;
            switch (tb_master.SelectedTab.Name)
            {
               case "tp_002":
                  SlctFigh_Count002_Txt.Text = lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Count().ToString();
                  break;
               case "tp_003":
                  SlctFigh_Count003_Txt.Text = lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Count().ToString();
                  break;
               case "tp_004":
                  SlctFigh_Count004_Txt.Text = lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Count().ToString();
                  break;
               case "tp_005":
                  if(Rb_PhoneNumberFromFile.Checked)
                     SlctFigh_Count005_Txt.Text = PhonNumber_CheckedComboBoxEdit005.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Count().ToString();
                  else
                     SlctFigh_Count005_Txt.Text = lov.Properties.Items.OfType<CheckedListBoxItem>().Where(i => i.CheckState == CheckState.Checked).Count().ToString();
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void Rb_SexType_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            switch (tb_master.SelectedTab.Name)
            {
               case "tp_002":
                  Rb_Selected00X_CheckedChanged(Pn_002.Controls.OfType<RadioButton>().First(i => i.Checked), null);
                  break;
               case "tp_003":
                  Rb_Selected00X_CheckedChanged(Pn_003.Controls.OfType<RadioButton>().First(i => i.Checked), null);
                  break;
               case "tp_004":
                  Rb_Selected00X_CheckedChanged(Pn_004.Controls.OfType<RadioButton>().First(i => i.Checked), null);
                  break;
               case "tp_005":
                  Rb_Selected00X_CheckedChanged(Pn_005.Controls.OfType<RadioButton>().First(i => i.Checked), null);
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void Btn_Preview010_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MsgbBs10.Current as Data.Message_Broadcast;

            if (crnt.STAT == "001")
            {
               MessageBox.Show("پیام در وضعیت غیرفعال می باشد");
               return;
            }

            if (crnt.MSGB_TEXT == null || crnt.MSGB_TEXT.Trim().Length == 0)
            {
               MessageBox.Show("متن پیام وارد نشده");
               return;
            }

            var sms = crnt.MSGB_TEXT;

            if (crnt.INSR_FNAM_STAT == "002")
               sms = "آقا / خانم" + "\n\r" + sms;

            if (crnt.INSR_CNAM_STAT == "002")
            {
               if (crnt.CLUB_NAME.Trim().Length == 0 && crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام باشگاه مشخص نشده است");
                  return;
               }
               if (crnt.CLUB_NAME.Trim().Length > 0)
                  sms = sms + "\n\r" + crnt.CLUB_NAME;
               else
                  sms = sms + "\n\r" + crnt.Club.NAME;
            }

            MessageBox.Show(this, sms, "خروجی پیامک");
         }
         catch { }
      }
   }
}
