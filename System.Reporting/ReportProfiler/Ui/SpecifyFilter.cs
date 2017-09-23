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
using DevExpress.XtraEditors.Controls;

namespace System.Reporting.ReportProfiler.Ui
{
   public partial class SpecifyFilter : UserControl
   {
      public SpecifyFilter()
      {
         InitializeComponent();
      }

      private bool OperationComplete = true;

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
                  var values = new []{ "1", "true" };
                  return values.Any<string>(v => v == xData.Element("StringDate").Attribute("checked").Value) ? "stringdatetime" : "string";
               case "BooleanField":
                  return "boolean";
               case "DateTimeField":
                  return "datetime";
               default:
                  return "unknown";
            }
         }
      }

      #region Events
      partial void Fltr_Stat_CheckedChanged(object sender, EventArgs e);

      partial void Schm_Stat_CheckedChanged(object sender, EventArgs e);

      partial void Schm_SelectedIndexChanged(object sender, EventArgs e);

      partial void HAC_SelectedIndexChanged(object sender, EventArgs e);

      partial void Tabl_Domn_CheckedChanged(object sender, EventArgs e);

      partial void Tabl_Domn_SelectedIndexChanged(object sender, EventArgs e);

      partial void Clmn_Code_SelectedIndexChanged(object sender, EventArgs e);

      partial void Clmn_Valu_SelectedIndexChanged(object sender, EventArgs e);

      partial void Show_Mean_SelectedIndexChanged(object sender, EventArgs e);

      partial void Mult_Slct_CheckedChanged(object sender, EventArgs e);

      partial void Dstc_CheckedChanged(object sender, EventArgs e);

      partial void Dflt_Valu_CheckedChanged(object sender, EventArgs e);

      partial void Row_Cont_CheckedChanged(object sender, EventArgs e);

      partial void Row_Cont_Leave(object sender, EventArgs e);

      partial void Ordr_By_CheckedChanged(object sender, EventArgs e);

      partial void Ordr_ByCl_Leave(object sender, EventArgs e);

      partial void Asc_Desc_Leave(object sender, EventArgs e);

      partial void Null_FsLa_Leave(object sender, EventArgs e);

      partial void Strg_Date_CheckedChanged(object sender, EventArgs e);

      partial void Digt_Sep_CheckedChanged(object sender, EventArgs e);

      partial void Max_Len_CheckedChanged(object sender, EventArgs e);

      partial void Max_Len_Leave(object sender, EventArgs e);

      partial void Numb_Pont_CheckedChanged(object sender, EventArgs e);

      partial void Numb_Pont_Leave(object sender, EventArgs e);

      partial void Min_Valu_CheckedChanged(object sender, EventArgs e);

      partial void Min_Valu_Leave(object sender, EventArgs e);

      partial void Max_Valu_CheckedChanged(object sender, EventArgs e);

      partial void Max_Valu_Leave(object sender, EventArgs e);

      partial void Null_Valu_CheckedChanged(object sender, EventArgs e);

      partial void NullValue_SelectedIndexChanged(object sender, EventArgs e);

      partial void Wher_Stmt_CheckedChanged(object sender, EventArgs e);

      partial void Wher_Stmt_Leave(object sender, EventArgs e);

      partial void Aply_Enbl_Chng(object sender, EventArgs e);

      partial void Commit_Click(object sender, EventArgs e);
      #endregion
   }
}
