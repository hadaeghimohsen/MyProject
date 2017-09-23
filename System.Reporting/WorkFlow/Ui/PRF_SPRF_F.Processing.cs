using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_SPRF_F
   {
      private void Processing()
      {
         /*Invoke(new Action(() =>
         {*/
            CLER_FAST_P();
            PROJ_XTMP_P();
            SET_RPRF_P();
         //}));
      }

      /* پاک کردن اطلاعات کنترل های فرم */
      private void CLER_FAST_P()
      {
         cbe_role.Properties.DataSource = null;

         Tile_Ctrl.Groups.Clear();
      }

      /* پر کردن اطلاعات گروه های دسترسی درون کنترل فرم */
      private void PROJ_XTMP_P()
      {
         cbe_role.Properties.DataSource = xAccessProfiler.Descendants("Role").
            Select(xRole => new { Role_ID = xRole.Attribute("id").Value, Role_Name = xRole.Attribute("faName").Value });

         cbe_role.Properties.DisplayMember = "Role_Name";
         cbe_role.Properties.ValueMember = "Role_ID";
         cbe_role.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(item => item.CheckState = Windows.Forms.CheckState.Checked);
         cbe_role.CheckAll();
      }

      /* ایجاد کردن آیتم های گروه و آیتم های گزارشات */
      private void SET_RPRF_P()
      {
         xAccessProfiler.Descendants("Role").ToList()
            .ForEach(xRole =>
            {
               TileGroup tGroup = SET_TGRP_F(xRole);
               Tile_Ctrl.Groups.Add(tGroup);

               xRole.Elements("Profiler").ToList()
                  .ForEach(xProfiler =>
                  {
                     tGroup.Items.Add(SET_TITM_F(xProfiler));
                  });
            });
      }

      private TileGroup SET_TGRP_F(XElement xGroup)
      {
         return new TileGroup() { Text = xGroup.Attribute("faName").Value, Tag = xGroup.Attribute("id").Value };
      }

      private TileItem SET_TITM_F(XElement xProfiler)
      {
         TileItem tItem = new TileItem() { IsLarge = Convert.ToBoolean(xProfiler.Attribute("isLarge").Value), Tag = xProfiler };
         tItem.AppearanceItem.Normal.BackColor = Convert.ToInt32(xProfiler.Attribute("rowNo").Value) % 2 == 0 ? Color.Blue : Color.Purple;
         tItem.AppearanceItem.Normal.ForeColor = Color.White;
         tItem.AppearanceItem.Normal.Options.UseBackColor = true;
         tItem.AppearanceItem.Normal.Options.UseForeColor = true;
         tItem.ItemClick += report_ItemClick;

         TileItemElement tItemElementTop = new TileItemElement();
         tItemElementTop.Text = xProfiler.Attribute("faName").Value;
         tItemElementTop.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 12F/*8.25F*/, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         tItemElementTop.TextAlignment = TileItemContentAlignment.TopRight;
         TileItemElement tItemElementBottom = new TileItemElement();
         tItemElementBottom.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         tItemElementBottom.Appearance.Normal.Options.UseFont = true;
         tItemElementBottom.Text = xProfiler.Attribute("rowNo").Value;
         tItemElementBottom.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.BottomRight;

         tItem.Elements.Add(tItemElementTop);
         tItem.Elements.Add(tItemElementBottom);

         return tItem;
      }

      private void report_ItemClick(object sender, TileItemEventArgs e)
      {
         if (xRunReport.Descendants("StepTwo").Count() >= 1)
            xRunReport.Descendants("StepTwo").Remove();

         xRunReport.Element("RunReport").Add(new XElement("StepTwo", (sender as TileItem).Tag));
         /* در این قسمت باید فرم بعدی اجرا شود */
         Next_Form = "WHR_SCON_F";
         next_Form_E();
      }

      private void next_Form_E()
      {
         switch (Next_Form)
         {
            case "WHR_SCON_F":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "", 05 /* Execute DoWork4WHR_SCON_F */, SendType.Self) { Input = xRunReport });
               break;
            default:
               break;
         }
      }
   }
}
