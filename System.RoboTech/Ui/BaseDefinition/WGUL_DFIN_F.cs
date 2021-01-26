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

namespace System.RoboTech.Ui.BaseDefinition
{
   public partial class WGUL_DFIN_F : UserControl
   {
      public WGUL_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            int uo = UserOrgnBs.Position;

            UserOrgnBs.DataSource = iRoboTech.User_Organ_Fgacs;

            UserOrgnBs.Position = uo;
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            int urlf = UrlfBs.Position;

            UrlfBs.DataSource = iRoboTech.User_RobotListener_Fgacs;

            UrlfBs.Position = urlf;
         }         
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            UrlfBs.EndEdit();
            UserOrgnBs.EndEdit();

            UserOrgn_Gv.PostEditor();
            Urlf_Gv.PostEditor();

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

      private void Tsb_DelUrlf_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var urlf = UrlfBs.Current as Data.User_RobotListener_Fgac;

            iRoboTech.DEL_URLF_P(urlf.FGA_CODE);
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

      private void DelUserOrgn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var ugfa = UserOrgnBs.Current as Data.User_Organ_Fgac;

            iRoboTech.DEL_UGFA_P(ugfa.FGA_CODE);
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

      private void ExecuteQuery_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }      
   }
}
