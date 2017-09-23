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
   public partial class TSK_CKLS_F : UserControl
   {
      public TSK_CKLS_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long clid;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ColbBs.DataSource = iCRM.Collaborators.FirstOrDefault(c => c.CLID == clid);

         requery = false;
      }

      private void CklsBs1_CurrentChanged(object sender, EventArgs e)
      {
         var ckls = CklsBs1.Current as Data.Check_List;
         if (ckls == null) return;

         // Set Stat
         ckls.STAT = ckls.STAT == null ? "002" : ckls.STAT;
         switch (ckls.STAT)
         {
            case "001":
               Tg_Stat.IsOn = false;
               break;
            case "002":
               Tg_Stat.IsOn = true;
               break;
            default:
               break;
         }
      }

      private void Tg_Stat_Toggled(object sender, EventArgs e)
      {
         var ckls = CklsBs1.Current as Data.Check_List;
         if (ckls == null) return;

         var tg = sender as ToggleSwitch;

         ckls.STAT = tg.IsOn ? "002" : "001";
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         CklsBs1.AddNew();
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Invalidate();

            CklsBs1.EndEdit();

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

      private void ColbBs_CurrentChanged(object sender, EventArgs e)
      {
         var colb = ColbBs.Current as Data.Collaborator;
         if (colb == null) return;

         Jobp_Txt.EditValue = colb.Job_Personnel.USER_DESC_DNRM;
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ckls = CklsBs1.Current as Data.Check_List;
            if (ckls == null) return;
            if (MessageBox.Show(this, "آیا با حذف چک لیست موافق هستید؟", "حذف چک لیست", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.DEL_CKLS_P(ckls.CKID);

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

      private void Actn_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var ckls = CklsBs1.Current as Data.Check_List;
            if (ckls == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 52 /* Execute Tsk_Ckld_F */),
                     new Job(SendType.SelfToUserInterface, "TSK_CKLD_F", 10 /* Execute Actn_Calf_P */)
                     {
                        Input = 
                           new XElement("Check_List",
                              new XAttribute("ckid", ckls.CKID)
                           )
                     }
                  }
               )
            );
         }
         catch
         { }
      }
   }
}
