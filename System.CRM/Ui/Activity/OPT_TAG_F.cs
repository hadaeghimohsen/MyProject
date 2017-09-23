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
using System.MaxUi;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_TAG_F : UserControl
   {
      public OPT_TAG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         TagBs.DataSource = iCRM.Tags.Where(t => t.SERV_FILE_NO == fileno && t.RQRO_RQST_RQID == null);
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         TagBs.AddNew();

         var tag = TagBs.Current as Data.Tag;
         tag.SERV_FILE_NO = fileno;
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Invalidate();

            TagBs.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
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

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var tag = TagBs.Current as Data.Tag;
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
            if (requery)
            {
               Execute_Query();
            }
         }
      }
   }
}
