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
   public partial class ORGN_DFIN_F : UserControl
   {
      public ORGN_DFIN_F()
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

         int orgn = OrgnBs.Position;
         int robot = RoboBs.Position;
         int userorgn = UserOrgnBs.Position;
         int job = JobBs.Position;
         int prbt = PrbtBs.Position;
         int prjb = PrjbBs.Position;
         int pjsr = PjsrBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs;

         OrgnBs.Position = orgn;
         RoboBs.Position = robot;
         UserOrgnBs.Position = userorgn;
         JobBs.Position = job;
         PrbtBs.Position = prbt;
         PrjbBs.Position = prjb;
         PjsrBs.Position = pjsr;
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Orgn_Gv.PostEditor();
            Robo_Gv.PostEditor();
            Uorg_Gv.PostEditor();
            Job_Gv.PostEditor();
            Prbt_Gv.PostEditor();
            Pjsr_Gv.PostEditor();
            Prjb_Gv.PostEditor();

            OrgnBs.EndEdit();
            RoboBs.EndEdit();
            UserOrgnBs.EndEdit();
            JobBs.EndEdit();
            PrbtBs.EndEdit();
            PrjbBs.EndEdit();

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

      private void Tsb_DelOrgn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var orgn = OrgnBs.Current as Data.Organ;

            iRoboTech.DEL_ORGN_P(orgn.OGID);
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

      private void Tsb_DelRobo_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;

            iRoboTech.DEL_ROBO_P(robo.RBID);
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

      private void Tsb_DelUofg_Click(object sender, EventArgs e)
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

      private void Tsb_DelJob_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var job = JobBs.Current as Data.Job;

            iRoboTech.DEL_JOB_P(job.ROBO_RBID, job.CODE);
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

      private void Tsb_DelPrbt_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var prbt = PrbtBs.Current as Data.Personal_Robot;

            iRoboTech.DEL_PRBT_P(prbt.SERV_FILE_NO, prbt.ROBO_RBID);
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

      private void Tsb_DelPrjb_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var prjb = PrjbBs.Current as Data.Personal_Robot_Job;

            iRoboTech.DEL_PRJB_P(prjb.CODE);
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

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         var robot = RoboBs.Current as Data.Robot;
         ServPrbtBs.DataSource = iRoboTech.Services.Where(s => s.Service_Robots.Any(sr => sr.Robot == robot));
         ServPrjbBs.DataSource = iRoboTech.Services.Where(s => s.Personal_Robots.Any(pr => pr.Robot == robot));
      }

      private void AddPjsr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PjsrBs.AddNew();
            var pjsr = PjsrBs.Current as Data.Personal_Robot_Job_Service_Robot;
            var robo = RoboBs.Current as Data.Robot;
            pjsr.SRBT_ROBO_RBID = robo.RBID;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DeltPjsr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pjsr = PjsrBs.Current as Data.Personal_Robot_Job_Service_Robot;
            if(pjsr == null)return;
            if (MessageBox.Show(this, "آیا با حذف مشتری برای پرسنل موافق هستید؟", "حذف مشتری در لیست پرسنل", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iRoboTech.DEL_PJSR_P(pjsr.CODE);
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

      private void SavePjsr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PjsrBs.EndEdit();

            iRoboTech.SubmitChanges();

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
   }
}
