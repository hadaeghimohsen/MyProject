using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.IO;
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.Scsc.Ui.Admission
{
   public partial class MBSP_MARK_F : UserControl
   {
      public MBSP_MARK_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? fileno;
      private short? rwno;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);

         MbsmBs1.DataSource = iScsc.Member_Ship_Marks.Where(m => m.MBSP_FIGH_FILE_NO == fileno && m.MBSP_RWNO == rwno && m.MBSP_RECT_CODE == "004");

         ApbsBs.DataSource = iScsc.App_Base_Defines.Where(a => a.ENTY_NAME == "Member_ship_Mark");
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
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Member_Ship_Mark"),
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

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsm = MbsmBs1.Current as Data.Member_Ship_Mark;
            if (mbsm == null) return;
            if (MessageBox.Show(this, "آیا با حذف برچسب و نشانه موافق هستید؟", "حذف برچسب و نشانه", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Member_Ship_Marks.DeleteOnSubmit(mbsm);

            iScsc.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {            
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void ApbsList_Lov_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (ApbsList_Lov.EditValue == null || ApbsList_Lov.EditValue.ToString() == "") { ApbsList_Lov.Focus(); return; }

                  if (MbsmBs1.List.OfType<Data.Member_Ship_Mark>().Any(t => t.MARK_CODE == (long)ApbsList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }
                  
                  MbsmBs1.AddNew();

                  var mbsm = MbsmBs1.Current as Data.Member_Ship_Mark;

                  if (fileno != 0 && rwno != 0)
                  {
                     mbsm.MBSP_FIGH_FILE_NO = fileno;
                     mbsm.MBSP_RWNO = rwno;
                     mbsm.MBSP_RECT_CODE = "004";
                  }
                  
                  mbsm.MARK_CODE = (long)ApbsList_Lov.EditValue;
                  Mbsm_Gv.PostEditor();
                  MbsmBs1.EndEdit();
                  
                  iScsc.SubmitChanges();
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

      private void ACTN_BUTN_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  Del_Butn_Click(null, null);
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
   }
}
