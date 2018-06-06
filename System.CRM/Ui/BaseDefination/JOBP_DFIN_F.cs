using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CRM.ExceptionHandlings;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;
using System.Xml.Linq;

namespace System.CRM.Ui.BaseDefination
{
   public partial class JOBP_DFIN_F : UserControl
   {
      public JOBP_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         int jobs = JobBs1.Position;
         int jbps = JbpsBs1.Position;
         int user = vUserBs1.Position;
         JobBs1.DataSource = iCRM.Jobs;
         JobBs1.Position = jobs;
         JbpsBs1.Position = jbps;
         vUserBs1.DataSource = iCRM.V_Users;
         vUserBs1.Position = user;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void JobAdd_Butn_Click(object sender, EventArgs e)
      {
         JobBs1.AddNew();
      }

      private void JobSave_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            JobBs1.EndEdit();

            iCRM.SubmitChanges();
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
               requery = false;
            }
         }
      }

      private void JobDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return; 

            var crnt = JobBs1.Current as Data.Job;

            iCRM.Jobs.DeleteOnSubmit(crnt);

            iCRM.SubmitChanges();
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
               requery = false;
            }
         }
      }

      private void JobpAdd_Butn_Click(object sender, EventArgs e)
      {
         JbpsBs1.AddNew();

         var crnt = JbpsBs1.Current as Data.Job_Personnel;

         var crntjob = JobBs1.Current as Data.Job;
         var crntusr = vUserBs1.Current as Data.V_User;

         crnt.Job = crntjob;
         crnt.USER_NAME = crntusr.USER_DB;
         crnt.STAT = "002";
         crnt.USER_DESC_DNRM = crntusr.USER_NAME;
      }

      private void JobpSave_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            JbpsBs1.EndEdit();

            iCRM.SubmitChanges();
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
               requery = false;
            }
         }
      }

      private void JobpDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var crnt = JbpsBs1.Current as Data.Job_Personnel;

            iCRM.Job_Personnels.DeleteOnSubmit(crnt);

            iCRM.SubmitChanges();

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
               requery = false;
            }
         }
      }

      private void Tg_ObjectTag_Toggled(object sender, EventArgs e)
      {
         try
         {
            var jobp = JbpsBs1.Current as Data.Job_Personnel;
            if (jobp == null) return;

            ToggleSwitch ts = sender as ToggleSwitch;

            switch (ts.Tag.ToString())
            {
               case "001":
                  jobp.SEND_EMAL_WHEN_TASK_ASGN_TO_ME = ts.IsOn ? "002" : "001";
                  break;
               case  "002":
                  jobp.SEND_EMAL_WHEN_LCAD_ASGN_TO_ME = ts.IsOn ? "002" : "001";
                  break;
               case "003":
                  jobp.SEND_EMAL_WHEN_MY_LCAD_ASGN_TO_SOME_ONE = ts.IsOn ? "002" : "001";
                  break;
               case "004":
                  jobp.SEND_NOTF_WHEN_TASK_ASGN_TO_ME = ts.IsOn ? "002" : "001";
                  break;
               case "005":
                  jobp.SEND_NOTF_WHEN_APPT_SCHD_FOR_ME = ts.IsOn ? "002" : "001";
                  break;
               case "006":
                  jobp.SEND_NOTF_WHEN_LCAD_ASGN_TO_ME = ts.IsOn ? "002" : "001";
                  break;
               case "007":
                  jobp.ALRM_DAY_STAT = ts.IsOn ? "002" : "001";
                  break;
               case "008":
                  jobp.ADD_LOGC_RLST_NOAN = ts.IsOn ? "002" : "001";
                  break;
               default:
                  break;
            }
         }
         catch (Exception)
         {}
      }

      private void JbpsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var jobp = JbpsBs1.Current as Data.Job_Personnel;
            if (jobp == null) return;            
            
         }
         catch (Exception)
         {  
         }
      }

      private void Cjbp_Butn_Click(object sender, EventArgs e)
      {
         var jbps = JbpsBs1.Current as Data.Job_Personnel;
         if(jbps == null)return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                  new Job(SendType.Self, 87 /* Execute Cjbp_Dfin_F */),                
                  new Job(SendType.SelfToUserInterface, "CJBP_DFIN_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Data",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("jobpcode", jbps.CODE)
                        )
                  }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Jbpd_Butn_Click(object sender, EventArgs e)
      {
         var jbps = JbpsBs1.Current as Data.Job_Personnel;
         if (jbps == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                  new Job(SendType.Self, 90 /* Execute Jbpd_Dfin_F */),                
                  new Job(SendType.SelfToUserInterface, "JBPD_DFIN_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Data",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("jobpcode", jbps.CODE)
                        )
                  }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SectFormAppBase_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "SECTION_FORM"),
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
   }
}
