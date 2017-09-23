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
using System.CRM.ExceptionHandlings;
using System.IO;
using System.MaxUi;
using DevExpress.XtraEditors;

namespace System.CRM.Ui.HistoryAction
{
   public partial class HST_UEXF_F : UserControl
   {
      public HST_UEXF_F()
      {
         InitializeComponent();         
      }

      private XElement xinput;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         
         ApbsBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "EXTRA_INFO" && a.REF_CODE == null);
      }

      private void ApbsList_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (ApbsList_Lov.EditValue == null || ApbsList_Lov.EditValue.ToString() == "") { ApbsList_Lov.Focus(); return; }
                  if (ExifBs.List.OfType<Data.Extra_Info>().Any(t => t.APBS_CODE == (long)ApbsList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }

                  ExifBs.AddNew();
                  var exif = ExifBs.Current as Data.Extra_Info;
                  exif.CODE = ExifBs.List.Count;
                  exif.APBS_CODE = (long)ApbsList_Lov.EditValue;
                  Exif_Gv.PostEditor();
                  ExifBs.EndEdit();

                  SapbBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "EXTRA_INFO" && a.App_Base_Define1.CODE == exif.APBS_CODE);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExifBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var exif = ExifBs.Current as Data.Extra_Info;
            if (exif == null) return;

            SapbBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "EXTRA_INFO" && a.App_Base_Define1.CODE == exif.APBS_CODE);
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void SapbList_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (SapbList_Lov.EditValue == null || SapbList_Lov.EditValue.ToString() == "") { SapbList_Lov.Focus(); return; }
                  if (ExidBs.List.OfType<Data.Extra_Info>().Any(t => t.APBS_CODE == (long)SapbList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }

                  var exif = ExifBs.Current as Data.Extra_Info;

                  ExidBs.AddNew();
                  var exid = ExidBs.Current as Data.Extra_Info;
                  
                  exid.EXIF_CODE = exif.CODE;
                  exid.APBS_CODE = (long)SapbList_Lov.EditValue;
                  Exid_Gv.PostEditor();
                  ExidBs.EndEdit();
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExifActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  var exif = ExifBs.Current as Data.Extra_Info;
                  ExifBs.Remove(exif);
                  break;
               default:
                  break;
            }            
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void ExidActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  var exid = ExidBs.Current as Data.Extra_Info;

                  ExidBs.Remove(exid);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void Approve_Butn_Click(object sender, EventArgs e)
      {
         var Qxml =
            new XElement("Extra_Infos",
               new XAttribute("cont", ExifBs.Count),
               new XAttribute("type", "extrainfo"),
               new XAttribute("allany", Ckb_AllAny.Checked ? "ALL" : "ANY"),
               ExifBs.OfType<Data.Extra_Info>()
               .Select(i =>
                  new XElement("Extra_Info",
                     new XAttribute("cont", i.Extra_Infos.Count),
                     i.Extra_Infos
                     .Select(ii =>
                        new XElement("Sub_Extra_Info",
                           new XAttribute("apbscode", i.APBS_CODE),
                           new XAttribute("sapbcode", ii.APBS_CODE)
                        )
                     )
                  )
               )
            );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, formCaller, 100 /* Execute SetFilterOnQuery */)
                  {
                     Input = Qxml
                  },
                  new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape}
               }
            )
         );
      }
   }
}
