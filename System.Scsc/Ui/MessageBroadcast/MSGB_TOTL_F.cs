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
         MsgbBs2.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "003");
         MsgbBs3.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "004");
         MsgbBs4.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "002");
         MsgbBs5.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "005");
         MsgbBs6.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "006");
         MsgbBs7.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "007");
         MsgbBs8.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "008");
         MsgbBs9.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "009");
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
   }
}
