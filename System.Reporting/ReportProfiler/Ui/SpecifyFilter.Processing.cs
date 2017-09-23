using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Extensions;
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.Ui
{
   partial class SpecifyFilter
   {
      #region Processing
      private void Processing()
      {
         CommonProcessingType();

         if (!cb_state.Checked)
            return;

         switch (GetType)
         {
            case "int":
               ProcessingInteger();
               break;
            case "string":
               ProcessingString();
               break;
            case "stringdatetime":
               ProcessingStringDateTime();
               break;
            case "datetime":
               ProcessingDateTime();
               break;
            case "boolean":
               ProcessingBoolean();
               break;
         }

         EXEC_XML_Q();
         EXEC_DBMS_Q();
         EXEC_APLY_E();
         
         APLY_QURY();
      }

      private void Masking()
      {
         switch (GetType)
         {
            case "int":
               MaskInteger();
               break;
            case "string":
               MaskString();
               break;
            case "stringdatetime":
               MaskStringDateTime();
               break;
            case "datetime":
               MaskDateTime();
               break;
         }
      }

      private void CommonProcessingType()
      {
         EXEC_XMLP_Q();

         nud_maxlen.Value = 10;
         nud_maxlen.Increment = 1;
         nud_maxlen.Minimum = 0;
         nud_maxlen.Maximum = 99999999;
         if (xData.Element("MaxLen").Attribute("checked").Value == "1")
            te_minvalue.Properties.MaxLength = te_maxvalue.Properties.MaxLength = Convert.ToInt32(xData.Element("MaxLen").Attribute("value").Value);
         
         cb_state.Checked = Convert.ToBoolean(Convert.ToInt32(XData.Attribute("state").Value));
         pn_filter.Visible = cb_state.Checked;
      }

      private void ProcessingInteger()
      {
         /* Mask */
         MaskInteger();               

         Enabling();
         Disabling();
      }

      private void MaskInteger()
      {
         te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         te_minvalue.Properties.Mask.PlaceHolder = te_maxvalue.Properties.Mask.PlaceHolder = '0';
         switch (xData.Element("DigitSep").Attribute("checked").Value)
         {
            case "false":
            case "0":
               te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = "d";
               break;
            case "true":
            case "1":
               te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = string.Format("n{0}", xData.Element("NumberPoint").Attribute("value").Value);
               break;
         }
      }

      private void ProcessingString()
      {
         /* Mask */
         MaskString();

         Enabling();
         Disabling();
      }

      private void MaskString()
      {
         te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
      }

      private void ProcessingStringDateTime()
      {
         /* Mask */
         MaskStringDateTime();

         Enabling();
         Disabling();
      }

      private void MaskStringDateTime()
      {
         nud_maxlen.Value = 10;
         nud_maxlen.Increment = 9;
         nud_maxlen.Minimum = 10;
         nud_maxlen.Maximum = 19;

         te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = @"([1234][0-9])[0-9][0-9]/(0[1-9]|1[012])/([012][1-9]|[123]0|31)(\s(0\d|1\d|2[0-3]):[0-5]\d(:[0-5]\d)?)?";
         te_minvalue.Properties.Mask.PlaceHolder = te_maxvalue.Properties.Mask.PlaceHolder = '0';
      }

      private void ProcessingDateTime()
      {
         /* Mask */
         MaskDateTime();
         
         Enabling();
         Disabling();
      }

      private void MaskDateTime()
      {
         nud_maxlen.Value = 10;
         nud_maxlen.Increment = 9;
         nud_maxlen.Minimum = 10;
         nud_maxlen.Maximum = 19;

         te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = @"([1234][0-9])[0-9][0-9]/(0[1-9]|1[012])/([012][1-9]|[123]0|31)(\s(0\d|1\d|2[0-3]):[0-5]\d(:[0-5]\d)?)?";
         te_minvalue.Properties.Mask.PlaceHolder = te_maxvalue.Properties.Mask.PlaceHolder = '0';
      }

      private void ProcessingBoolean()
      {
         Enabling();
         Disabling();
      }
      #endregion

      #region Action On Item Type
      private void DoAction(string getType, string actionType)
      {         
         pn_filter.Controls.OfType<Control>()
            .Where(ctrl =>
               xTemplate.Element("FilterTemplate")
               .Elements("Type")
               .Where(t => t.Attribute("name").Value == getType)
               .First()
               .Elements("Controls")
               .Where(c => c.Attribute("action").Value == actionType)
               .First()
               .Elements("Control")
               .Any(c => c.Attribute("tag").Value == ctrl.Tag.ToString()))
            .ToList()
            .ForEach(item =>
            {
               switch (actionType)
               {
                  case "enabling":
                     item.Enabled = true;
                     break;
                  case "disabling":
                     item.Enabled = false;
                     break;
               }
            });
      }

      private void DoAction(string getType, string tagID, string selectCase, string actionType)
      {
         try
         {
            pn_filter.Controls.OfType<Control>()
               .Where(ctrl =>
                  xTemplate.Element("FilterTemplate")
                     .Descendants("Type")
                     .First(t => t.Attribute("name").Value == getType)
                     .Elements("Controls")
                     .First(c => c.Attribute("action").Value == "enabling")
                     .Elements("Control")
                     .First(c => c.Attribute("tag").Value == tagID)
                     .Elements("Select")
                     .First(s => s.Attribute("case").Value == selectCase)
                     .Elements("Controls")
                     .First(c => c.Attribute("action").Value == actionType)
                     .Elements("Control")
                     .Any(c => c.Attribute("tag").Value == ctrl.Tag.ToString()))
               .ToList()
               .ForEach(rslt =>
               {
                  switch (actionType)
                  {
                     case "enabling":
                        rslt.Enabled = true;
                        break;
                     case "disabling":
                        rslt.Enabled = false;
                        break;
                  }
               });
         }
         catch { }
      }

      private void DoAction_IV(string tagID, string actionType)
      {
         pn_filter.Controls.OfType<Control>()
            .Where(ctrl =>
               xTemplate.Element("FilterTemplate")
                  .Element("ShowHide")
                  .Descendants("Control")
                  .First(c => c.Attribute("tag").Value == tagID)
                  .Descendants("Control")
                  .Any(c => c.Attribute("tag").Value == ctrl.Tag.ToString()))
            .ToList()
            .ForEach(rslt =>
            {
               switch (actionType)
               {
                  case "visibling":
                     rslt.Visible = true;
                     break;
                  case "invisibling":
                     rslt.Visible = false;
                     break;
               }
            });
      }
      #endregion

      #region All ActionType
      private void Enabling()
      {
         DoAction(GetType, "enabling");
      }

      private void Enabling(string tagID, string selectCase)
      {
         DoAction(GetType, tagID, selectCase.ToLower(), "enabling");
      }

      private void Disabling()
      {
         DoAction(GetType, "disabling");
      }

      private void Disabling(string tagID, string selectCase)
      {
         DoAction(GetType, tagID, selectCase.ToLower(), "disabling");
      }

      private void Visibling(string tagID)
      {
         DoAction_IV(tagID, "visibling");
      }

      private void Invisibling(string tagID)
      {
         DoAction_IV(tagID, "invisibling");
      }
      #endregion

      #region Set Attribute On Control
      /* از این تابع برای قرار دادن اطلاعات استاتیک روی کنترل های فرم استفاده می شود */
      private void EXEC_XML_Q()
      {         
         cb_schema.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("Schema").First().Attribute("checked").Value));
         cbx_accesscontrol.SelectedIndex = Convert.ToInt32(xData.Descendants("RefTable").First().Attribute("hac").Value);
         cb_tabledomain.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("RefTable").First().Attribute("type").Value));
         cbx_showmeaning.SelectedIndex = Convert.ToInt32(xData.Descendants("RefTable").First().Attribute("showmeaning").Value);
         cb_multiselected.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Attribute("multiSelected").Value));
         cb_distinct.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Attribute("distincted").Value));
         cb_defaultvalues.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("DefaultValues").First().Attribute("checked").Value));
         cb_nrowfetch.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("NRowFetched").First().Attribute("checked").Value));
         nud_nrowcount.Value = Convert.ToDecimal(xData.Descendants("NRowFetched").First().Attribute("value").Value);
         cb_ordered.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("IsOrdered").First().Attribute("checked").Value));
         cbx_ordercolumn.SelectedIndex = Convert.ToInt32(xData.Descendants("IsOrdered").First().Attribute("columns").Value);
         cbx_ascdesc.SelectedIndex = Convert.ToInt32(xData.Descendants("IsOrdered").First().Attribute("sorted").Value);
         cbx_nullsfirstorlast.SelectedIndex = Convert.ToInt32(xData.Descendants("IsOrdered").First().Attribute("nullsFsLs").Value);
         cb_stringdate.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("StringDate").First().Attribute("checked").Value));
         cb_digitsep.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("DigitSep").First().Attribute("checked").Value));
         cb_maxlen.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("MaxLen").First().Attribute("checked").Value));
         decimal maxLen = Convert.ToDecimal(xData.Descendants("MaxLen").First().Attribute("value").Value);
         nud_maxlen.Value = (nud_maxlen.Minimum <= maxLen && nud_maxlen.Maximum >= maxLen) ? maxLen : nud_maxlen.Minimum;
         cb_numberpoint.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("NumberPoint").First().Attribute("checked").Value));
         nud_numberpoint.Value = Convert.ToDecimal(xData.Descendants("NumberPoint").First().Attribute("value").Value);
         cb_minvalue.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("MinValue").First().Attribute("checked").Value));
         te_minvalue.Text = xData.Descendants("MinValue").First().Attribute("value").Value;
         cb_maxvalue.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("MaxValue").First().Attribute("checked").Value));
         te_maxvalue.Text = xData.Descendants("MaxValue").First().Attribute("value").Value;
         cb_nullvalue.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("NullValue").First().Attribute("checked").Value));
         cbx_nullvalue.SelectedIndex = Convert.ToInt32(xData.Descendants("NullValue").First().Attribute("value").Value);
         cb_where.Checked = Convert.ToBoolean(Convert.ToInt32(xData.Descendants("Where").First().Attribute("checked").Value));
         te_where.Text = xData.Descendants("Where").First().Value;
      }

      /* نمايش اطلاعات عمومی فیلتر */
      private void EXEC_XMLP_Q()
      {
         tb_TFaName.Text = xData.Descendants("Table").First().Attribute("faName").Value;
         tb_TEnName.Text = xData.Descendants("Table").First().Attribute("enName").Value;
         tb_CFaName.Text = xData.Descendants("Column").First().Attribute("faName").Value;
         tb_CEnName.Text = xData.Descendants("Column").First().Attribute("enName").Value;
      }
      
      /* از این تابع برای قرار دادن اطلاعات داینامیک روی کنترل های فرم مورد استفاده قرار میگرد */
      private void EXEC_DBMS_Q()
      {
         if (!cb_schema.Checked)
            return;

         EXEC_SCHM_Q();
         EXEC_SCHM_X();

         if (cb_tabledomain.Checked)
         {
            EXEC_TABL_Q();
            EXEC_TABL_X();
         }
         else
         {
            EXEC_DOMN_Q();
            EXEC_DOMN_X();
         }

         if (cbx_tables.SelectedIndex >= 0)
         {
            EXEC_CLMN_Q();
            EXEC_CLMN_X();
         }
      }

      /* نمایش تاثیرات برجای گذاشته شده بر روی کنترلهای صفحه */
      private void EXEC_APLY_E()
      {
         Schm_Stat_CheckedChanged(null, null);
         //HAC_SelectedIndexChanged(null, null);
         Dflt_Valu_CheckedChanged(null, null);
         Row_Cont_CheckedChanged(null, null);
         Ordr_By_CheckedChanged(null, null);
         if ((new string[] { "string", "stringdatetime" }).Any(v => v == GetType))
            Strg_Date_CheckedChanged(null, null);
         Max_Len_CheckedChanged(null, null);
         Numb_Pont_CheckedChanged(null, null);
         Min_Valu_CheckedChanged(null, null);
         Max_Valu_CheckedChanged(null, null);
         Wher_Stmt_CheckedChanged(null, null);
      }

      #region EXEC_SCHM
      private void EXEC_SCHM_Q()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  @"<Request type=""Normal""/>",
                  "{ Call Report.GetSchemas(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
               (output) =>
                  {
                     OperationComplete = false;

                     DataSet ds = output as DataSet;
                     cbx_schema.DataSource = ds.Tables["Schema"];
                     cbx_schema.DisplayMember = "SEnName";
                     cbx_schema.ValueMember = "ID";
                     
                     OperationComplete = true;
                  })
            });
      }
      private void EXEC_SCHM_X()
      {
         /* انتخاب کردن اندیسی از شما که قبلا انتخاب شده است */
         cbx_schema.Text = xData.Descendants("Schema").First().Attribute("value").Value;
      }
      #endregion

      #region EXEC_TABL
      private void EXEC_TABL_Q()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  @"<Request type=""Tables""/>",
                  "{ Call Report.GetTableUsage(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
               (output) =>
               {
                  OperationComplete = false;

                  DataSet ds = output as DataSet;
                  cbx_tables.DataSource = ds.Tables["TableUsage"];
                  cbx_tables.DisplayMember = "TEnName";
                  cbx_tables.ValueMember = "ID";

                  OperationComplete = true;
               })
            });
      }
      private void EXEC_TABL_X()
      {
         cbx_tables.SelectedValue = xData.Descendants("RefTable").First().Attribute("tableID").Value;
         //cbx_tables.Text = xData.Descendants("RefTable").First().Attribute("enName").Value;
      }
      #endregion

      #region EXEC_DOMN
      private void EXEC_DOMN_Q()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  @"<Request type=""Domains""/>",
                  "{ Call Report.GetTableUsage(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
               (output) =>
               {
                  OperationComplete = false;

                  DataSet ds = output as DataSet;
                  cbx_tables.DataSource = ds.Tables["TableUsage"];
                  cbx_tables.DisplayMember = "TEnName";
                  cbx_tables.ValueMember = "ID";

                  OperationComplete = true;
               })
            });
      }
      private void EXEC_DOMN_X()
      {
         cbx_tables.SelectedValue = xData.Descendants("RefTable").First().Attribute("tableID").Value;
         //cbx_tables.Text = xData.Descendants("RefTable").First().Attribute("enName").Value;
      }
      #endregion

      #region EXEC_CLMN
      private void EXEC_CLMN_Q()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  string.Format(@"<Request type=""Normal""><Table>{0}</Table></Request>", cbx_tables.SelectedValue),
                  "{ Call Report.GetColumnUsage(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
               (output) =>
               {
                  OperationComplete = false;

                  DataSet dsCode = output as DataSet;
                  cbx_code.DataSource = dsCode.Tables["ColumnUsage"];
                  cbx_code.DisplayMember = "EnName";
                  cbx_code.ValueMember = "ID";

                  DataSet dsValue = dsCode.Copy();
                  cbx_value.DataSource = dsValue.Tables["ColumnUsage"];
                  cbx_value.DisplayMember = "EnName";
                  cbx_value.ValueMember = "ID";

                  OperationComplete = true;
               })
            });
      }
      private void EXEC_CLMN_X()
      {
         cbx_code.SelectedValue = xData.Descendants("RefTable").First().Attribute("codeID").Value;
         cbx_value.SelectedValue = xData.Descendants("RefTable").First().Attribute("valueID").Value;
      }
      #endregion

      #region EXEC_DFVL
      private void EXEC_DFVL_Q()
      {
         APLY_QURY();
      }
      private void EXEC_DFVL_X()
      {
         cbe_defaultvalues.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => xData.Descendants("Value").Where(v => c.Value.ToString() == v.Value).Count() > 0).ToList().ForEach(c => c.CheckState = CheckState.Checked);
      }
      #endregion

      #endregion

      #region Return Result Query From Database
      private void APLY_QURY()
      {
         if (!cb_schema.Checked) return;
         if (cbx_schema.SelectedIndex < 0) return;
         if (cbx_tables.SelectedIndex < 0) return;
         if (cbx_code.SelectedIndex < 0) return;
         if (cbx_value.SelectedIndex < 0) return;
         if (cbx_showmeaning.SelectedIndex < 0) return;
         if (!cb_defaultvalues.Checked) return;

         List<string> sql = new List<string>();
         xData
            .DoDistinct(sql)
            .DoShowMeaningDesc(sql)
            .DoWhere(sql)
            .DoHighAccessControl(sql)
            .DoAddScopeFrom(sql)
            .DoOrderBy(sql)
            .DoFetchRowCount(sql);

         string Dsn, user, pwd;
         if (cb_tabledomain.Checked)
         {
            Dsn = string.Format("Db_{0}", xData.Element("UseConStr").Attribute("id").Value);
            user = xData.Element("UseConStr").Attribute("user").Value;
            pwd = xData.Element("UseConStr").Attribute("password").Value;
         }
         else
         {
            Dsn = "iProject";
            user = "scott";
            pwd = "";
         }

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "text",
                  false,
                  true,
                  "",
                  "",
                  string.Join(" ", sql),
                  Dsn,
                  user,
                  pwd
                  /*
                  string.Format("Db_{0}",xData.Element("UseConStr").Attribute("id").Value),
                  xData.Element("UseConStr").Attribute("user").Value,
                  xData.Element("UseConStr").Attribute("password").Value
                   * */
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     if (InvokeRequired)
                     {
                        Invoke(new Action(() =>
                           {
                              cbe_defaultvalues.Visible = true;
                              cbe_defaultvalues.Properties.DataSource = (output as DataSet).Tables[0];
                              cbe_defaultvalues.Properties.DisplayMember = "DESCTEXT";
                              cbe_defaultvalues.Properties.ValueMember = "CODE";
                           }));
                     }
                     else
                     {
                        cbe_defaultvalues.Visible = true;
                        cbe_defaultvalues.Properties.DataSource = (output as DataSet).Tables[0];
                        cbe_defaultvalues.Properties.DisplayMember = "DESCTEXT";
                        cbe_defaultvalues.Properties.ValueMember = "CODE";

                        /* تنظیم کردن اطلاعات پیش فرض */
                        EXEC_DFVL_X();
                     }
                  })
            });
      }
      #endregion

      #region Prepare Xml Data
      private void EXEC_CLER_P()
      {
         /* ابتدا بایستی المنت مربوط به مقادیر پیش حذف شود */         
         if(xData.Descendants("DefaultValues").FirstOrDefault() != null)  
            xData.Descendants("DefaultValues").First().Remove();
      }

      private void EXEC_INSR_P()
      {     
         /* در این قسمت اطلاعات مقادیر پیش فرض جدید ایجاد میکنیم */
         xData.Descendants("RefTable").First()
            .Add(new XElement("DefaultValues",
               from v in cbe_defaultvalues.Properties.GetItems().OfType<CheckedListBoxItem>()
               where v.CheckState == CheckState.Checked
               select new XElement("Value", v.Value),
               new XAttribute("checked", "true")
               ));
      }
      #endregion
   }
}
