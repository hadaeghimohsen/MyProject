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

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class RBKN_DVLP_F : UserControl
   {
      public RBKN_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private async void Execute_Query()
      {
         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int rbkn = RoboKnowlgeBs.Position;

         var result = await Task.Run(() =>
         {
            var db = new Data.iRoboTechDataContext(ConnectionString);
            var organs = db.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID)).ToList();
            return new { organs };
         });

         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         OrgnBs.DataSource = result.organs;
         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         RoboKnowlgeBs.Position = rbkn;
      }

      private void Tsb_DelRbkn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rbkn = RoboKnowlgeBs.Current as Data.Robot_Import;

            iRoboTech.DEL_RBKN_P(rbkn.ROBO_RBID, rbkn.ID);
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
               requery = false;
            }
         }
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Invalidate();

            RoboKnowlgeBs.EndEdit();

            iRoboTech.SubmitChanges();
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
               requery = false;
            }
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
