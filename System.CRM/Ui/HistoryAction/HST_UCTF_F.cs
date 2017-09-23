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
   public partial class HST_UCTF_F : UserControl
   {
      public HST_UCTF_F()
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
         
         ApbsBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "CONTACT_INFO" && a.REF_CODE == null);
      }

      private void ApbsList_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (ApbsList_Lov.EditValue == null || ApbsList_Lov.EditValue.ToString() == "") { ApbsList_Lov.Focus(); return; }
                  if (CtifBs.List.OfType<Data.Contact_Info>().Any(t => t.APBS_CODE == (long)ApbsList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }

                  CtifBs.AddNew();
                  var ctif = CtifBs.Current as Data.Contact_Info;
                  ctif.CODE = CtifBs.List.Count;
                  ctif.APBS_CODE = (long)ApbsList_Lov.EditValue;
                  Exif_Gv.PostEditor();
                  CtifBs.EndEdit();
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
                  var exif = CtifBs.Current as Data.Extra_Info;
                  CtifBs.Remove(exif);
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
            new XElement("Contact_Infos",
               new XAttribute("cont", CtifBs.Count),
               new XAttribute("type", "contactinfo"),
               new XAttribute("allany", Ckb_AllAny.Checked ? "ALL" : "ANY"),
               CtifBs.OfType<Data.Contact_Info>()
               .Select(i =>
                  new XElement("Contact_Info",
                     new XAttribute("apbscode", i.APBS_CODE),
                     new XAttribute("contdesc", string.Format("%{0}%",(i.CONT_DESC ?? "").Replace(' ', '%')))
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
