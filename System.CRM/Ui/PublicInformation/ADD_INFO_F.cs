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

namespace System.CRM.Ui.PublicInformation
{
   public partial class ADD_INFO_F : UserControl
   {
      public ADD_INFO_F()
      {
         InitializeComponent();         
      }

      private XElement xinput;
      private bool requery = false;
      private long fileno, compcode;


      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if(fileno != 0)
         {
            ExifBs.DataSource =
               iCRM.Extra_Infos
               .Where(ai =>
                  ai.SERV_FILE_NO == fileno &&
                  ai.EXIF_CODE == null
               );
         }
         else if(compcode != 0)
         {
            ExifBs.DataSource =
               iCRM.Extra_Infos
               .Where(ai =>
                  ai.COMP_CODE == compcode &&
                  ai.EXIF_CODE == null
               );
         }
         ApbsBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "EXTRA_INFO" && a.REF_CODE == null);
         requery = false;
      }

      private void AddNewAppBase_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 79 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "EXTRA_INFO"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
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
                  if (fileno != 0)
                     exif.SERV_FILE_NO = fileno;
                  else if (compcode != 0)
                     exif.COMP_CODE = compcode;

                  exif.APBS_CODE = (long)ApbsList_Lov.EditValue;
                  exif.RWNO = ExifBs.List.Count == 1 ? 1 : ExifBs.List.OfType<Data.Extra_Info>().Max(a => a.RWNO) + 1;
                  Exif_Gv.PostEditor();
                  ExifBs.EndEdit();
                  
                  iCRM.SubmitChanges();
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
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
                  
                  if (fileno != 0)
                     exid.SERV_FILE_NO = fileno;
                  else if (compcode != 0)
                     exid.COMP_CODE = compcode;
                  
                  
                  exid.EXIF_CODE = exif.CODE;
                  exid.RWNO = ExidBs.List.Count == 1 ? 1 : ExidBs.List.OfType<Data.Extra_Info>().Max(a => a.RWNO) + 1;
                  exid.APBS_CODE = (long)SapbList_Lov.EditValue;
                  Exid_Gv.PostEditor();
                  ExidBs.EndEdit();

                  iCRM.SubmitChanges();
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void ExifActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  var exif = ExifBs.Current as Data.Extra_Info;

                  iCRM.DEL_EXIF_P(exif.CODE);
                  requery = true;
                  break;
               default:
                  break;
            }            
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void ExidActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  var exid = ExidBs.Current as Data.Extra_Info;

                  iCRM.DEL_EXIF_P(exid.CODE);
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }
   }
}
