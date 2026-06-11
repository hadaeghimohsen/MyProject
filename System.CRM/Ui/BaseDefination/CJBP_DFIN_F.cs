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

namespace System.CRM.Ui.BaseDefination
{
   public partial class CJBP_DFIN_F : UserControl
   {
      public CJBP_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery;
      private string formcaller;
      private XElement xinput;

      private async void Execute_Query()
      {
         var result = await Task.Run(() =>
         {
            using (var ctx = new Data.iCRMDataContext(ConnectionString))
            {
               return new
               {
                  JobPersonnelRelations = ctx.Job_Personnel_Relations.ToList(),
                  JobPersonnels = ctx.Job_Personnels.ToList(),
                  AppBaseDefines = ctx.App_Base_Defines.Where(a => a.ENTY_NAME == "COMPANYCHART_INFO").ToList(),
               };
            }
         });

         iCRM = new Data.iCRMDataContext(ConnectionString);
         JbprBs1.DataSource = result.JobPersonnelRelations;
         JobpBs1.DataSource = result.JobPersonnels;
         ApbsBs1.DataSource = result.AppBaseDefines;
         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void InsJbpr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var jobpcode = JobpCode_Lov.EditValue;
            var rlatjobpcode = RlatJobpCode_Lov.EditValue;
            var apbscode = ApbsCode_Lov.EditValue;

            if ((jobpcode == null|| jobpcode == "") || (rlatjobpcode == null || rlatjobpcode == "") || (apbscode == null || apbscode == "")) return;

            iCRM.INS_JBPR_P((long?)jobpcode, (long?)rlatjobpcode, (long?)apbscode);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void DelJbpr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var jbpr = JbprBs1.Current as Data.Job_Personnel_Relation;

            if (jbpr == null) return;

            iCRM.DEL_JBPR_P(jbpr.CODE);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ApbsCode_Lov_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
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
                                 new XAttribute("tablename", "COMPANYCHART_INFO"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     })
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
