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
using System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Extensions;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors.Controls;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui
{
   public partial class Filter : UserControl
   {
      public Filter()
      {
         InitializeComponent();
      }
      private XElement xData;
      public XElement XmlTemplate 
      {
         get { return xData; }
         set 
         {
            xData = value;
            cb_filterdesc.Text = XmlTemplate.Element("Column").Attribute("faName").Value;
         }
      }

      public new string GetType
      {
         get
         {
            switch (xData.Element("Column").Attribute("type").Value)
            {
               case "CurrencyField":
               case "Int16sField":
               case "Int32sField":
               case "NumberField":
                  return "int";
               case "StringField":
               case "PersistentMemoField":
                  return xData.Element("StringDate").Attribute("checked").Value == "1" ? "stringdatetime" : "string";
               case "BooleanField":
                  return "domain";
               case "DateTimeField":
                  return "datetime";
               default:
                  return "unknown";
            }
         }
      }

      public void Init()
      {
         if (xData.Element("RefTable").Element("Schema").Attribute("checked").Value == "1" &&
            xData.Element("RefTable").Attribute("hac").Value == "2")
            Enabled = false;
         if (GetType != "datetime" && xData.Attribute("multiSelected").Value == "1")
            cbe_lookupvalue.Properties.AllowMultiSelect = true;
         if (xData.Element("MaxLen").Attribute("checked").Value == "1")
            te_minvalue.Properties.MaxLength = te_maxvalue.Properties.MaxLength = Convert.ToInt32(xData.Element("MaxLen").Attribute("value").Value);         

         switch (GetType)
         {
            case "int":
               #region int
               te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
               te_minvalue.Properties.Mask.PlaceHolder = te_maxvalue.Properties.Mask.PlaceHolder = '0';
               switch (xData.Element("DigitSep").Attribute("checked").Value)
	            {
                  case "0":
                     te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = "d";
                     break;
                  case "1":
                     te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = string.Format("n{0}", xData.Element("NumberPoint").Attribute("value").Value);
                     break;
	            }               
               #endregion
               break;
            case "string":
               #region string
               te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
               #endregion
               break;
            case "stringdatetime":
            case "datetime":
               #region datetime
               te_minvalue.Properties.Mask.MaskType = te_maxvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
               te_minvalue.Properties.Mask.EditMask = te_maxvalue.Properties.Mask.EditMask = @"([1234][0-9])[0-9][0-9]/(0[1-9]|1[012])/([012][1-9]|[123]0|31)(\s(0\d|1\d|2[0-3]):[0-5]\d(:[0-5]\d)?)?";
               te_minvalue.Properties.Mask.PlaceHolder = te_maxvalue.Properties.Mask.PlaceHolder = '0';
               #endregion
               break;
         }
         #region Min & Max
         if (xData.Element("MinValue").Attribute("checked").Value == "1")
         {
            te_minvalue.EditValue = xData.Element("MinValue").Attribute("value").Value;
         }
         if (xData.Element("MaxValue").Attribute("checked").Value == "1")
         {
            te_maxvalue.EditValue = xData.Element("MaxValue").Attribute("value").Value;
         }
         #endregion
      }

      public bool Checked
      {
         get { return cb_filterdesc.Checked; }
      }

      public bool HasFiltering
      {
         get
         {
            if ((xData.Element("RefTable").Attribute("hac").Value == "1" &&
            xData.Element("RefTable").Element("Schema").Attribute("checked").Value == "1") ||
               xData.Element("MinValue").Attribute("checked").Value == "1" ||
            xData.Element("MaxValue").Attribute("checked").Value == "1")
               return true;
            else
               return false;
         }
      }

      #region Events

      private void PostCheckedChangedFilterDesc(object sender, EventArgs e)
      {
         cbx_sourcepath.Visible = cb_filterdesc.Checked;
      }

      private void PostVisibleSourcePath(object sender, EventArgs e)
      {
         switch (cbx_sourcepath.Visible)
         {
            case true:
               cbx_compare.Visible = true;
               PostIndexChangedSourcePath(sender, e);
               break;
            case false:
               sb_filepath.Visible = false;
               cbx_compare.Visible = false;
               break;
         }
      }

      private void PostVisibleCompare(object sender, EventArgs e)
      {
         switch (cbx_compare.Visible)
         {
            case true:
               break;
            case false:
               te_minvalue.Visible = te_maxvalue.Visible = cbe_lookupvalue.Visible = false;
               break;
         }
      }

      private void PostIndexChangedSourcePath(object sender, EventArgs e)
      {         
         Action<string> LoadCompareData = new Action<string>(
            (sourcePath) =>
            {
               cbx_compare.Items.Clear();
               switch (GetType)
               {
                  case "int":
                     XmlUi.Element("Filtering").Elements("SourcePath")
                        .Where(sp => sp.Attribute("type").Value == sourcePath)
                        .FirstOrDefault()
                        .Elements("Field")
                        .Where(f => f.Attribute("type").Value == "int")
                        .FirstOrDefault()
                        .Element("Comparing")
                        .Elements("Item")
                        .ToList()
                        .ForEach(i =>
                        {
                           cbx_compare.Items.Add(i.Attribute("fa").Value);
                        });
                     break;
                  case "string":
                     XmlUi.Element("Filtering").Elements("SourcePath")
                        .Where(sp => sp.Attribute("type").Value == sourcePath)
                        .FirstOrDefault()
                        .Elements("Field")
                        .Where(f => f.Attribute("type").Value == "string")
                        .FirstOrDefault()
                        .Element("Comparing")
                        .Elements("Item")
                        .ToList()
                        .ForEach(i =>
                        {
                           cbx_compare.Items.Add(i.Attribute("fa").Value);
                        });
                     break;
                  case "stringdatetime":
                  case "datetime":
                     XmlUi.Element("Filtering").Elements("SourcePath")
                        .Where(sp => sp.Attribute("type").Value == sourcePath)
                        .FirstOrDefault()
                        .Elements("Field")
                        .Where(f => f.Attribute("type").Value == "datetime")
                        .FirstOrDefault()
                        .Element("Comparing")
                        .Elements("Item")
                        .ToList()
                        .ForEach(i =>
                        {
                           cbx_compare.Items.Add(i.Attribute("fa").Value);
                        });
                     break;
                  case "domain":
                     XmlUi.Element("Filtering").Elements("SourcePath")
                        .Where(sp => sp.Attribute("type").Value == sourcePath)
                        .FirstOrDefault()
                        .Elements("Field")
                        .Where(f => f.Attribute("type").Value == "domain")
                        .FirstOrDefault()
                        .Element("Comparing")
                        .Elements("Item")
                        .ToList()
                        .ForEach(i =>
                        {
                           cbx_compare.Items.Add(i.Attribute("fa").Value);
                        });
                     break;
               }
            });

         switch (cbx_sourcepath.SelectedIndex)
         {
            case 0:
               sb_filepath.Visible = false;
               LoadCompareData("*");
               break;
            case 1:
               sb_filepath.Visible = true;
               LoadCompareData("#");
               break;
            case 2:
               switch (GetType)
	            {
                  case "datetime":
                  case "stringdatetime":
                     cbx_sourcepath.SelectedIndex = 0;
                     return;
	            }
               if (xData.Element("RefTable").Element("Schema").Attribute("checked").Value == "0")
               {
                  cbx_sourcepath.SelectedIndex = 0;
                  return;
               }
               sb_filepath.Visible = false;
               LoadCompareData("@");
               switch (xData.Element("UseConStr").Attribute("retrieved").Value)
	            {
                  case "false":
                     LoadData();
                     xData.Element("UseConStr").Attribute("retrieved").SetValue("true");
                     break;
	            }
               break;
         }
      }

      private void PostIndexChangedCompare(object sender, EventArgs e)
      {
         switch (GetType)
         {
            case "int":
               switch (cbx_sourcepath.SelectedIndex)
	            {
                  case 0:
                     #region *
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                           te_maxvalue.Visible = cbe_lookupvalue.Visible = false;
                           te_minvalue.Visible = true;
                           break;
                        case 6:
                        case 7:
                           cbe_lookupvalue.Visible = false;
                           te_minvalue.Visible = te_maxvalue.Visible = true;
                           break;
                        case 8:
                        case 9:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = false;
                           break;
                     }
                     #endregion
                     break;
                  case 1:
                     #region #
                     switch (cbx_compare.SelectedIndex)
	                  {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = true;
                           break;
	                  }
                     #endregion
                     break;
                  case 2:
                     #region @
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = true;
                           break;
                     }
                     #endregion
                     break;
	            }               
               break;
            case "string":
               switch (cbx_sourcepath.SelectedIndex)
               {
                  case 0:
                     #region *
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                           te_maxvalue.Visible = cbe_lookupvalue.Visible = false;
                           te_minvalue.Visible = true;
                           break;
                        case 12:
                        case 13:
                           cbe_lookupvalue.Visible = false;
                           te_minvalue.Visible = te_maxvalue.Visible = true;
                           break;
                        case 14:
                        case 15:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = false;
                           break;
                     }
                     #endregion
                     break;
                  case 1:
                     #region #
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = true;
                           break;
                     }
                     #endregion
                     break;
                  case 2:
                     #region @
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = true;
                           break;
                     }
                     #endregion
                     break;
               }  
               break;
            case "stringdatetime":
            case "datetime":
               switch (cbx_sourcepath.SelectedIndex)
               {
                  case 0:
                     #region *
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                           te_maxvalue.Visible = cbe_lookupvalue.Visible = false;
                           te_minvalue.Visible = true;
                           break;
                        case 6:
                        case 7:
                           cbe_lookupvalue.Visible = false;
                           te_minvalue.Visible = te_maxvalue.Visible = true;
                           break;
                        case 8:
                        case 9:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = false;
                           break;
                     }
                     #endregion
                     break;
                  case 1:
                     #region #
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = true;
                           break;
                     }
                     #endregion
                     break;
               }
               break;
            case "domain":
               switch (cbx_sourcepath.SelectedIndex)
               {
                  case 0:
                     #region *
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = false;
                           break;
                     }
                     #endregion
                     break;
                  case 2:
                     #region @
                     switch (cbx_compare.SelectedIndex)
                     {
                        case 0:
                        case 1:
                           te_minvalue.Visible = te_maxvalue.Visible = false;
                           cbe_lookupvalue.Visible = true;
                           break;
                     }
                     #endregion
                     break;
               }
               break;
         }
      }

      private void PostMinValueValidation(object sender, EventArgs e)
      {
         te_minvalue.Text = getChekInptVald(te_minvalue.Text.Replace("," ,""), "min");
      }

      private void PostMaxValueValidation(object sender, EventArgs e)
      {
         te_maxvalue.Text = getChekInptVald(te_maxvalue.Text.Replace(",", ""), "max");
      }

      #endregion

      #region LoadData
      private void LoadData()
      {
         List<string> sql = new List<string>();
         xData
            .DoDistinct(sql)
            .DoShowMeaningDesc(sql)
            .DoWhere(sql)
            .DoHighAccessControl(sql)
            .DoAddScopeFrom(sql)
            .DoOrderBy(sql)
            .DoFetchRowCount(sql);

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
                  string.Format("Db_{0}",xData.Element("UseConStr").Attribute("id").Value),
                  xData.Element("UseConStr").Attribute("user").Value,
                  xData.Element("UseConStr").Attribute("password").Value
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                     {
                        cbe_lookupvalue.Properties.DataSource = (output as DataSet).Tables[0];
                        cbe_lookupvalue.Properties.DisplayMember = "DESCTEXT";
                        cbe_lookupvalue.Properties.ValueMember = "CODE";

                        EXEC_DFVL_X();
                     })
            });
      }

      private void EXEC_DFVL_X()
      {
         cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => xData.Descendants("Value").Where(v => c.Value.ToString() == v.Value).Count() > 0).ToList().ForEach(c => c.CheckState = CheckState.Checked);
      }
      #endregion

      partial void sb_filepath_Click(object sender, EventArgs e);
      
   }
}
