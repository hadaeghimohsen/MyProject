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

      private async void Execute_Query()
      {
         bool isTab001 = tb_master.SelectedTab == tp_001;
         bool isTab002 = tb_master.SelectedTab == tp_002;
         int uo = isTab001 ? UserOrgnBs.Position : 0;
         int urlf = isTab002 ? UrlfBs.Position : 0;

         var result = await Task.Run(() =>
         {
            var db = new Data.iRoboTechDataContext(ConnectionString);
            var userOrgns = isTab001 ? db.User_Organ_Fgacs.ToList() : null;
            var urlfs = isTab002 ? db.User_RobotListener_Fgacs.ToList() : null;
            return new { userOrgns, urlfs };
         });

         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         if(isTab001)
         {
            UserOrgnBs.DataSource = result.userOrgns;
            UserOrgnBs.Position = uo;
         }
         else if (isTab002)
         {
            UrlfBs.DataSource = result.urlfs;
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
