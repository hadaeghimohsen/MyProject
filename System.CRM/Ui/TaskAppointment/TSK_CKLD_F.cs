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
   public partial class TSK_CKLD_F : UserControl
   {
      public TSK_CKLD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long ckid;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         CklsBs.DataSource = iCRM.Check_Lists.FirstOrDefault(c => c.CKID == ckid);

         requery = false;
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         CkldBs.AddNew();
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Invalidate();

            CkldBs.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
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

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ckld = CkldBs.Current as Data.Check_List_Detial;
            if (ckld == null) return;
            if (MessageBox.Show(this, "آیا با حذف جزییات چک لیست موافق هستید؟", "حذف جزییات چک لیست", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.DEL_CKLD_P(ckld.CDID);

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
   }
}
