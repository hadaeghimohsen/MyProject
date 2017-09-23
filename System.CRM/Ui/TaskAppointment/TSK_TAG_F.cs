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
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.CRM.Ui.TaskAppointment
{
   public partial class TSK_TAG_F : UserControl
   {
      public TSK_TAG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long rqid, fileno, compcode;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         
         if(rqid != 0)
            TagBs1.DataSource = iCRM.Tags.Where(t => t.RQRO_RQST_RQID == rqid);
         else if(fileno != 0)
            TagBs1.DataSource = iCRM.Tags.Where(t => t.SERV_FILE_NO == fileno);
         else if(compcode != 0)
            TagBs1.DataSource = iCRM.Tags.Where(t => t.COMP_CODE_DNRM == compcode);

         ApbsBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "TAG");
         requery = false;
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         TagBs1.AddNew();

         var tag = TagBs1.Current as Data.Tag;

         if (rqid != 0)
         {
            tag.RQRO_RQST_RQID = rqid;
            tag.RQRO_RWNO = 1;
         }
         else if(fileno != 0)
         {
            tag.SERV_FILE_NO = fileno;
         }
         else if(compcode != 0)
         {
            tag.COMP_CODE_DNRM = compcode;
         }

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
                              new XAttribute("tablename", "TAG"),
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
            var tag = TagBs1.Current as Data.Tag;
            if (tag == null) return;
            if (MessageBox.Show(this, "آیا با حذف برچسب و نشانه موافق هستید؟", "حذف برچسب و نشانه", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.DEL_TAG_P(tag.TGID);

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
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

                  if (TagBs1.List.OfType<Data.Tag>().Any(t => t.APBS_CODE == (long)ApbsList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }
                  
                  TagBs1.AddNew();

                  var tag = TagBs1.Current as Data.Tag;

                  if (rqid != 0)
                  {
                     tag.RQRO_RQST_RQID = rqid;
                     tag.RQRO_RWNO = 1;
                  }
                  else if(fileno != 0)
                  {
                     tag.SERV_FILE_NO = fileno;
                  }
                  else if(compcode != 0)
                  {
                     tag.COMP_CODE_DNRM = compcode;
                  }
                  tag.APBS_CODE = (long)ApbsList_Lov.EditValue;
                  Tag_Gv.PostEditor();
                  TagBs1.EndEdit();
                  
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
