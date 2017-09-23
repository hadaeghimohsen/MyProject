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
   partial class RPT_SRPT_F
   {
      private void Processing()
      {
         if (InvokeRequired)
            Invoke(new Action(() =>
            {
               CLER_FAST_P();
               PROJ_XTMP_P();
               SET_RPTS_P();
            }));
         else
         {
            CLER_FAST_P();
            PROJ_XTMP_P();
            SET_RPTS_P();
         }
      }
      
      /* پاک کردن اطلاعات کنترل های فرم */
      private void CLER_FAST_P()
      {
         if (IsFirst)
         {
            cbe_role.Properties.DataSource = null;

            Tile_Ctrl.Groups.Clear();
         }
      }

      /* پر کردن اطلاعات گروه های دسترسی درون کنترل فرم */
      private void PROJ_XTMP_P()
      {
         cbe_role.Properties.DataSource = xAccessReport.Descendants("Role").
            Select(xRole => new { Role_ID = xRole.Attribute("id").Value, Role_Name = xRole.Attribute("faName").Value });

         cbe_role.Properties.DisplayMember = "Role_Name";
         cbe_role.Properties.ValueMember = "Role_ID";
         cbe_role.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(item => item.CheckState = Windows.Forms.CheckState.Checked);
         cbe_role.CheckAll();
      }

      /* ایجاد کردن آیتم های گروه و آیتم های گزارشات */
      private void SET_RPTS_P()
      {
         xAccessReport.Descendants("Role").ToList()
            .ForEach(xRole =>
            {
               TileGroup tGroup = SET_TGRP_F(xRole);
               Tile_Ctrl.Groups.Add(tGroup);

               xRole.Elements("Report").ToList()
                  .ForEach(xReport =>
                  {
                     tGroup.Items.Add(SET_TITM_F(xReport));
                  });
            });
      }

      private TileGroup SET_TGRP_F(XElement xGroup)
      {
         return new TileGroup() { Text = xGroup.Attribute("faName").Value, Tag = xGroup.Attribute("id").Value };
      }

      private TileItem SET_TITM_F(XElement xReport)
      {
         TileItem tItem = new TileItem(){IsLarge = Convert.ToBoolean(xReport.Attribute("isLarge").Value), Tag = xReport};
         tItem.AppearanceItem.Normal.BackColor = Convert.ToInt32(xReport.Attribute("rowNo").Value) % 2 == 0 ? Color.Blue : Color.Purple;
         tItem.AppearanceItem.Normal.ForeColor = Color.White;
         tItem.AppearanceItem.Normal.Options.UseBackColor = true;
         tItem.AppearanceItem.Normal.Options.UseForeColor = true;
         tItem.ItemClick += report_ItemClick;

         TileItemElement tItemElementTop = new TileItemElement();         
         tItemElementTop.Text = xReport.Element("LogicalName").Value;
         tItemElementTop.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 12F/*8.25F*/, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         tItemElementTop.TextAlignment = TileItemContentAlignment.TopRight;
         TileItemElement tItemElementBottom = new TileItemElement();
         tItemElementBottom.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         tItemElementBottom.Appearance.Normal.Options.UseFont = true;
         tItemElementBottom.Text = xReport.Attribute("rowNo").Value;
         tItemElementBottom.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.BottomRight;

         tItem.Elements.Add(tItemElementTop);
         tItem.Elements.Add(tItemElementBottom);

         return tItem;
      }

      private void report_ItemClick(object sender, TileItemEventArgs e)
      {
         xRunReport = XDocument.Parse(string.Format("<RunReport><StepOne>{0}</StepOne></RunReport>", (sender as TileItem).Tag));

         /* در این قسمت باید فرم بعدی اجرا شود */
         next_Form_E();
      }

      private void next_Form_E()
      {
         switch (Next_Form)
         {
            case "PRF_SPRF_F":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "PRF_SPRF_F"},
                        new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 02 /* Execute Set */){Input = xRunReport},
                        new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 07 /* Execute LoadData */),//{Executive = ExecutiveType.Asynchronous},
                        new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 03 /* Execute Paint */)
                     }));
               break;
            default:
               break;
         }
      }
   }
}
