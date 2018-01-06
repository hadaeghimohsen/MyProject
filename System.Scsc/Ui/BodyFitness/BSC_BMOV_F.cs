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

namespace System.Scsc.Ui.BodyFitness
{
   public partial class BSC_BMOV_F : UserControl
   {
      public BSC_BMOV_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         if(tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            int _0 = BbfmBs1.Position;
            int _1 = PbfmBs1.Position;
            BbfmBs1.DataSource = iScsc.Basic_Body_Fitness_Movements;
            BbfmBs1.Position = _0;
            PbfmBs1.Position = _1;

            PmbfLookUpGridView.ActiveFilterString = "STAT = '002' AND BODY_TYPE IS NOT NULL AND EFCT_TYPE IS NOT NULL";
         }
      }


      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void BbfmBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = BbfmBs1.Current as Data.Basic_Body_Fitness_Movement;

            iScsc.Basic_Body_Fitness_Movements.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception )
         {
            MessageBox.Show("خطا در انجام عملیات");
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void BbfmBnSave1_Click(object sender, EventArgs e)
      {
         try
         {            
            BbfmBs1.EndEdit();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception )
         {
            MessageBox.Show("خطا در انجام عملیات");
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void PbfmBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = PbfmBs1.Current as Data.Premovement_Body_Fitness;

            iScsc.Premovement_Body_Fitnesses.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception )
         {
            MessageBox.Show("خطا در انجام عملیات");
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void PbfmBnSave1_Click(object sender, EventArgs e)
      {
         try
         {
            PbfmBs1.EndEdit();

            iScsc.SubmitChanges();

            requery = true;
         }
         catch (Exception )
         {
            MessageBox.Show("خطا در انجام عملیات");
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void BbfmBnNew1_Click(object sender, EventArgs e)
      {
         try
         {
            BbfmBs1.AddNew();

            var crnt = BbfmBs1.Current as Data.Basic_Body_Fitness_Movement;

            crnt.NUMB_OF_MOVE_IN_SET = 10;
            crnt.REST_TIME_IN_SET = new TimeSpan(0, 0, 45);
            crnt.TIME_PER_SET = new TimeSpan(0, 1, 0);
            crnt.STAT = "002";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void BbfmBs1_CurrentItemChanged(object sender, EventArgs e)
      {
         try
         {
            var crnt = BbfmBs1.Current as Data.Basic_Body_Fitness_Movement;

            if (crnt.BBFM_DESC != "" && crnt.BBFM_DESC != null) return;

            if(crnt.BODY_TYPE != null && crnt.BODY_TYPE != "" && crnt.EFCT_TYPE != null && crnt.EFCT_TYPE != "")
            {
               crnt.BBFM_DESC = string.Format("{1} اندازه قطر {0}", iScsc.D_BODies.First(b => b.VALU == crnt.BODY_TYPE).DOMN_DESC, iScsc.D_IDTPs.First(id => id.VALU == crnt.EFCT_TYPE).DOMN_DESC); 
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PmbfBnNew1_Click(object sender, EventArgs e)
      {
         try
         {
            PbfmBs1.AddNew();
            var crnt = PbfmBs1.Current as Data.Premovement_Body_Fitness;
            crnt.STAT = "002";
            crnt.ORDR = PbfmBs1.List.OfType<Data.Premovement_Body_Fitness>().Max(p => p.ORDR);
            crnt.ORDR = crnt.ORDR == null ? 0 : crnt.ORDR;
            crnt.ORDR++;
            var crntbbfm = BbfmBs1.Current as Data.Basic_Body_Fitness_Movement;

            if (crnt.PMBF_DESC != "" && crnt.PMBF_DESC != null) return;

            if (crnt.PRE_BBFM_BFID == 0)
            {
               crnt.PMBF_DESC = string.Format("حرکت پیش نیاز برای {0}", crntbbfm.BBFM_DESC);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Bbfm_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var crnt = PbfmBs1.Current as Data.Premovement_Body_Fitness;
            crnt.PRE_BBFM_BFID = (long)e.NewValue;
            PbfmBs1.EndEdit();
         }
         catch (Exception)
         {

         }
      }
   }
}
