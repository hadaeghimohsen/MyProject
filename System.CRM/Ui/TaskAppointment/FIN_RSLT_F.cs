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
   public partial class FIN_RSLT_F : UserControl
   {
      public FIN_RSLT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long rqid;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         FinRBs1.DataSource = iCRM.Final_Results.FirstOrDefault(fr => fr.RQRO_RQST_RQID == rqid);
         requery = false;
      }

      private void FinRBs1_CurrentChanged(object sender, EventArgs e)
      {
         var finr = FinRBs1.Current as Data.Final_Result;
         if (finr == null) return;

         finr.FINR_STAT = finr.FINR_STAT == null ? "001" : finr.FINR_STAT;
         switch (finr.FINR_STAT)
         {
            case "001":
               Stat001_Butn.NormalColorA = Stat001_Butn.NormalColorB = Color.YellowGreen;
               Stat002_Butn.NormalColorA = Stat002_Butn.NormalColorB = Color.White;
               Stat003_Butn.NormalColorA = Stat003_Butn.NormalColorB = Color.White;
               break;
            case "002":
               Stat001_Butn.NormalColorA = Stat001_Butn.NormalColorB = Color.White;
               Stat002_Butn.NormalColorA = Stat002_Butn.NormalColorB = Color.Yellow;
               Stat003_Butn.NormalColorA = Stat003_Butn.NormalColorB = Color.White;
               break;
            case "003":
               Stat001_Butn.NormalColorA = Stat001_Butn.NormalColorB = Color.White;
               Stat002_Butn.NormalColorA = Stat002_Butn.NormalColorB = Color.White;
               Stat003_Butn.NormalColorA = Stat003_Butn.NormalColorB = Color.Gray;
               break;
            default:
               break;
         }
      }

      private void Stat00i_Butn_Click(object sender, EventArgs e)
      {
         var butn = sender as RoundedButton;
         var finr = FinRBs1.Current as Data.Final_Result;
         if(finr == null)
         {
            FinRBs1.AddNew();
            finr = FinRBs1.Current as Data.Final_Result;
            finr.RQRO_RQST_RQID = rqid;
            finr.RQRO_RWNO = 1;
         }
         finr.FINR_STAT = butn.Tag.ToString();

         switch (finr.FINR_STAT)
         {
            case "001":
               Stat001_Butn.NormalColorA = Stat001_Butn.NormalColorB = Color.YellowGreen;
               Stat002_Butn.NormalColorA = Stat002_Butn.NormalColorB = Color.White;
               Stat003_Butn.NormalColorA = Stat003_Butn.NormalColorB = Color.White;
               break;
            case "002":
               Stat001_Butn.NormalColorA = Stat001_Butn.NormalColorB = Color.White;
               Stat002_Butn.NormalColorA = Stat002_Butn.NormalColorB = Color.Yellow;
               Stat003_Butn.NormalColorA = Stat003_Butn.NormalColorB = Color.White;
               break;
            case "003":
               Stat001_Butn.NormalColorA = Stat001_Butn.NormalColorB = Color.White;
               Stat002_Butn.NormalColorA = Stat002_Butn.NormalColorB = Color.White;
               Stat003_Butn.NormalColorA = Stat003_Butn.NormalColorB = Color.Gray;
               break;
            default:
               break;
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FinRBs1.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               Back_Butn_Click(null, null);
            }
         }
      }
   }
}
