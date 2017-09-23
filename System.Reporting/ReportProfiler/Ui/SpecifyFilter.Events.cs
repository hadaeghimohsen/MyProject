using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Reporting.ReportProfiler.Ui
{
   partial class SpecifyFilter
   {
      partial void Fltr_Stat_CheckedChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Attribute("state").SetValue(cb_state.Checked);

         pn_filter.Visible = cb_state.Checked;
         EXEC_APLY_E();
      }

      partial void Schm_Stat_CheckedChanged(object sender, EventArgs e)
      {
         Enabling(cb_schema.Tag.ToString(), cb_schema.Checked.ToString());
         Disabling(cb_schema.Tag.ToString(), cb_schema.Checked.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("Schema").First().Attribute("checked").SetValue(cb_schema.Checked);

         if (cb_schema.Checked)
         {
            EXEC_DBMS_Q();
            APLY_QURY();
         }
      }

      partial void Schm_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         if (cbx_schema.SelectedIndex < 0)
            return;
         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("Schema").First().Attribute("value").Value == cbx_schema.Text)
            return;

         xData.Descendants("Schema").First().Attribute("value").SetValue(cbx_schema.Text);

         APLY_QURY();
      }

      partial void HAC_SelectedIndexChanged(object sender, EventArgs e)
      {
         Enabling(cbx_accesscontrol.Tag.ToString(), cbx_accesscontrol.SelectedIndex.ToString());
         Disabling(cbx_accesscontrol.Tag.ToString(), cbx_accesscontrol.SelectedIndex.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("RefTable").First().Attribute("hac").Value == cbx_accesscontrol.SelectedIndex.ToString())
            return;

         xData.Descendants("RefTable").First().Attribute("hac").SetValue(cbx_accesscontrol.SelectedIndex);         
      }

      partial void Tabl_Domn_CheckedChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("RefTable").First().Attribute("type").SetValue(cb_tabledomain.Checked);

         if (cb_tabledomain.Checked)
         {
            cb_tabledomain.Text = "جدول";
            
            EXEC_TABL_Q();
            EXEC_TABL_X();
         }
         else
         {
            cb_tabledomain.Text = "دامنه";

            EXEC_DOMN_Q();
            EXEC_DOMN_X();
         }

         if (cbx_tables.SelectedIndex >= 0)
         {
            EXEC_CLMN_Q();
            EXEC_CLMN_X();
         }

         APLY_QURY();
      }

      partial void Tabl_Domn_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("RefTable").First().Attribute("enName").Value == cbx_tables.Text)
            return;

         if (cbx_tables.SelectedValue != null)
         {
            xData.Descendants("RefTable").First().Attribute("enName").SetValue(cbx_tables.Text);
            xData.Descendants("RefTable").First().Attribute("tableID").SetValue(cbx_tables.SelectedValue);
         }

         switch (xData.Descendants("UseConStr").First().Attribute("dsType").Value)
         {
            case "0":
               xData.Descendants("RefTable").First().Attribute("tabRealName").SetValue(string.Format(@"""{0}""", cbx_tables.Text));
               break;
            case "1":
               xData.Descendants("RefTable").First().Attribute("tabRealName").SetValue(string.Format(@"[{0}]", cbx_tables.Text));
               break;
         }
         if (cbx_tables.SelectedIndex >= 0)
         {
            EXEC_CLMN_Q();
            EXEC_CLMN_X();
         }

         APLY_QURY();
      }

      partial void Clmn_Code_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("RefTable").First().Attribute("code").Value == cbx_code.Text)
            return;

         if (cbx_code.SelectedValue == null) return;
         
         xData.Descendants("RefTable").First().Attribute("code").SetValue(cbx_code.Text);
         xData.Descendants("RefTable").First().Attribute("codeID").SetValue(cbx_code.SelectedValue);

         switch (xData.Descendants("UseConStr").First().Attribute("dsType").Value)
         {
            case "0":
               xData.Descendants("RefTable").First().Attribute("codeRealName").SetValue(string.Format(@"""{0}""", cbx_code.Text));
               break;
            case "1":
               xData.Descendants("RefTable").First().Attribute("codeRealName").SetValue(string.Format(@"[{0}]", cbx_code.Text));
               break;
         }
         
         APLY_QURY();
      }

      partial void Clmn_Valu_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("RefTable").First().Attribute("value").Value == cbx_value.Text)
            return;

         if (cbx_value.SelectedValue == null) return;

         xData.Descendants("RefTable").First().Attribute("value").SetValue(cbx_value.Text);
         xData.Descendants("RefTable").First().Attribute("valueID").SetValue(cbx_value.SelectedValue);

         switch (xData.Descendants("UseConStr").First().Attribute("dsType").Value)
         {
            case "0":
               xData.Descendants("RefTable").First().Attribute("valueRealName").SetValue(string.Format(@"""{0}""", cbx_value.Text));
               break;
            case "1":
               xData.Descendants("RefTable").First().Attribute("valueRealName").SetValue(string.Format(@"[{0}]", cbx_value.Text));
               break;
         }

         APLY_QURY();
      }

      partial void Show_Mean_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("RefTable").First().Attribute("showmeaning").Value == cbx_showmeaning.SelectedIndex.ToString())
            return;

         xData.Descendants("RefTable").First().Attribute("showmeaning").SetValue(cbx_showmeaning.SelectedIndex);

         APLY_QURY();
      }

      partial void Mult_Slct_CheckedChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Attribute("multiSelected").SetValue(cb_multiselected.Checked);
      }

      partial void Dstc_CheckedChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Attribute("distincted").SetValue(cb_distinct.Checked);
         APLY_QURY();
      }

      partial void Dflt_Valu_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_defaultvalues.Checked)
            Visibling(cb_defaultvalues.Tag.ToString());
         else
            Invisibling(cb_defaultvalues.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("DefaultValues").First().Attribute("checked").SetValue(cb_defaultvalues.Checked);

         if (cb_defaultvalues.Checked)
            APLY_QURY();
         else
            cbe_defaultvalues.Properties.Items.Clear();
      }

      partial void Row_Cont_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_nrowfetch.Checked)
            Visibling(cb_nrowfetch.Tag.ToString());
         else
            Invisibling(cb_nrowfetch.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("NRowFetched").First().Attribute("checked").SetValue(cb_nrowfetch.Checked);

         Row_Cont_Leave(null, null);
      }

      partial void Row_Cont_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("NRowFetched").First().Attribute("value").Value == nud_nrowcount.Value.ToString())
            return;

         xData.Descendants("NRowFetched").First().Attribute("value").SetValue(nud_nrowcount.Value);
         APLY_QURY();
      }

      partial void Ordr_By_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_ordered.Checked)
            Visibling(cb_ordered.Tag.ToString());
         else
            Invisibling(cb_ordered.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("IsOrdered").First().Attribute("checked").SetValue(cb_ordered.Checked);

         APLY_QURY();
      }

      partial void Ordr_ByCl_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("IsOrdered").First().Attribute("columns").Value == cbx_ordercolumn.SelectedIndex.ToString())
            return;

         xData.Descendants("IsOrdered").First().Attribute("columns").SetValue(cbx_ordercolumn.SelectedIndex);
         APLY_QURY();
      }

      partial void Asc_Desc_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("IsOrdered").First().Attribute("sorted").Value == cbx_ascdesc.SelectedIndex.ToString())
            return;

         xData.Descendants("IsOrdered").First().Attribute("sorted").SetValue(cbx_ascdesc.SelectedIndex);
         APLY_QURY();
      }

      partial void Null_FsLa_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("IsOrdered").First().Attribute("nullsFsLs").Value == cbx_nullsfirstorlast.SelectedIndex.ToString())
            return;

         xData.Descendants("IsOrdered").First().Attribute("nullsFsLs").SetValue(cbx_nullsfirstorlast.SelectedIndex);
         APLY_QURY();
      }

      partial void Strg_Date_CheckedChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("StringDate").First().Attribute("checked").SetValue(cb_stringdate.Checked);

         if (cb_stringdate.Checked)
            ProcessingStringDateTime();
         else
         {
            ProcessingString();
            APLY_QURY();
         }
      }

      partial void Digt_Sep_CheckedChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("DigitSep").First().Attribute("checked").SetValue(cb_digitsep.Checked);
         Masking();
      }

      partial void Max_Len_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_maxlen.Checked)
            Visibling(cb_maxlen.Tag.ToString());
         else
            Invisibling(cb_maxlen.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("MaxLen").First().Attribute("checked").SetValue(cb_maxlen.Checked);         

         Masking();
      }

      partial void Max_Len_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("MaxLen").First().Attribute("value").SetValue(nud_maxlen.Value);
         Masking();
      }

      partial void Numb_Pont_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_numberpoint.Checked)
            Visibling(cb_numberpoint.Tag.ToString());
         else
            Invisibling(cb_numberpoint.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("NumberPoint").First().Attribute("checked").SetValue(cb_numberpoint.Checked);
         
         Masking();
      }

      partial void Numb_Pont_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("NumberPoint").First().Attribute("value").SetValue(nud_numberpoint.Value);
         Masking();
      }

      partial void Min_Valu_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_minvalue.Checked)
            Visibling(cb_minvalue.Tag.ToString());
         else
            Invisibling(cb_minvalue.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;
                  
         xData.Descendants("MinValue").First().Attribute("checked").SetValue(cb_minvalue.Checked);

         Min_Valu_Leave(null, null);
      }

      partial void Min_Valu_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("MinValue").First().Attribute("value").Value == te_minvalue.Text.Replace(",", ""))
            return;

         xData.Descendants("MinValue").First().Attribute("value").SetValue(te_minvalue.Text.Replace(",", ""));         
         
         APLY_QURY();
      }

      partial void Max_Valu_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_maxvalue.Checked)
            Visibling(cb_maxvalue.Tag.ToString());
         else
            Invisibling(cb_maxvalue.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("MaxValue").First().Attribute("checked").SetValue(cb_maxvalue.Checked);

         Max_Valu_Leave(null, null);
      }

      partial void Max_Valu_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("MaxValue").First().Attribute("value").Value == te_maxvalue.Text.Replace(",", ""))
            return;
                  
         xData.Descendants("MaxValue").First().Attribute("value").SetValue(te_maxvalue.Text.Replace(",", ""));
         
         APLY_QURY();
      }

      partial void Null_Valu_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_nullvalue.Checked)
            Visibling(cb_nullvalue.Tag.ToString());
         else
            Invisibling(cb_nullvalue.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("NullValue").First().Attribute("checked").SetValue(cb_nullvalue.Checked);

         NullValue_SelectedIndexChanged(null, null);
      }

      partial void NullValue_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("NullValue").First().Attribute("value").SetValue(cbx_nullvalue.SelectedIndex);
                  
         APLY_QURY();
      }

      partial void Wher_Stmt_CheckedChanged(object sender, EventArgs e)
      {
         if (cb_where.Checked)
            Visibling(cb_where.Tag.ToString());
         else
            Invisibling(cb_where.Tag.ToString());

         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         xData.Descendants("Where").First().Attribute("checked").SetValue(cb_where.Checked);

         Wher_Stmt_Leave(null,null);
      }

      partial void Wher_Stmt_Leave(object sender, EventArgs e)
      {
         /* اگر کار بارگذاری تمام نشده باشد */
         if (!OperationComplete) return;

         /* اگر متن شرایط خالی باشد */
         if (te_where.Text.Replace(" ", string.Empty).Length == 0) return;

         /* اگر داده وارد شده و انتخاب شده توسط کاربر هیچ تفاوتی را ایجاد نکند دیگر نیازی به ادامه مراحل نیست */
         if (xData.Descendants("Where").First().Value.Replace(" ", string.Empty) == te_where.Text)
         {
            xData.Descendants("Where").First().SetValue(te_where.Text);
            return;
         }
         xData.Descendants("Where").First().SetValue(te_where.Text);

         APLY_QURY();
      }

      partial void Aply_Enbl_Chng(object sender, EventArgs e)
      {
         CheckBox cb = sender as CheckBox;

         if (cb.Enabled == false)
            cb.Checked = false;
         else
         {
            switch (cb.Tag.ToString())
            {
               case "8":
                  Dstc_CheckedChanged(cb, e);
                  break;
               case "9":
                  Dflt_Valu_CheckedChanged(cb, e);
                  break;
               case "11":
                  Row_Cont_CheckedChanged(cb, e);
                  break;
               case "13":
                  Ordr_By_CheckedChanged(cb, e);
                  break;
               case "17":
                  Max_Len_CheckedChanged(cb, e);
                  break;
               case "19":
                  Numb_Pont_CheckedChanged(cb, e);
                  break;
               case "21":
                  Min_Valu_CheckedChanged(cb, e);
                  break;
               case "23":
                  Max_Valu_CheckedChanged(cb, e);
                  break;
               case "25":
                  Null_Valu_CheckedChanged(cb, e);
                  break;
               case "26":
                  Wher_Stmt_CheckedChanged(cb, e);
                  break;
            }
         }
      }

      partial void Commit_Click(object sender, EventArgs e)
      {
         /* پاک کردن اطلاعات مقادیر پیش فرض قدیم */
         EXEC_CLER_P();

         /* اضافه کردن مقادیر پیش فرض جدیدی که کاربر انتخاب کرده باشد */
         if (cb_defaultvalues.Checked)
            EXEC_INSR_P();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              false,
                              "xml",
                              string.Format(@"<Request type=""Update"">{0}</Request>", xData.ToString()),
                              "{ Call Report.[SetItemsInGroup](?) }",
                              "iProject",
                              "scott"
                           }
                        }
                     }),
                  new Job(SendType.SelfToUserInterface, "SpecifyFilter", 04 /* Execute UnPaint */),
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 07 /* Execute LoadData */)
               }));

      }
   }
}
