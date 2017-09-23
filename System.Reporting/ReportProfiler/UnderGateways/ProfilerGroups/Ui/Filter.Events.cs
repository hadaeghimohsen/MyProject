using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DevExpress.XtraEditors.Controls;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui
{
   partial class Filter
   {
      partial void sb_filepath_Click(object sender, EventArgs e)
      {
         if (ofd_selector.ShowDialog() != Windows.Forms.DialogResult.OK)
            return;

         DataTable dt = new DataTable("Data");
         dt.Columns.AddRange(
            new[] {
               new DataColumn("Value", typeof(object))
            });

         StreamReader sr = new StreamReader(ofd_selector.FileName);
         string line;
         while((line = sr.ReadLine()) != null)
         {
            if (               
               line != "" &&
               getChekInptVald(line, "list") == line &&
               getChekInptVald(line, "min") == line &&
               getChekInptVald(line, "max") == line
            )
            {
               DataRow dr = dt.NewRow();
               dr["Value"] = line;
               dt.Rows.Add(dr);
            }
         }

         cbe_lookupvalue.Properties.DataSource = dt;
         cbe_lookupvalue.Properties.DisplayMember = "Value";
         cbe_lookupvalue.Properties.ValueMember = "Value";
         cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(i => i.CheckState = Windows.Forms.CheckState.Checked);

         xData.Element("UseConStr").Attribute("retrieved").SetValue("false");
      }
   }
}
