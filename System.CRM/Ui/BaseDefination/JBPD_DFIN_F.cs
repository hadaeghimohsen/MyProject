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
   public partial class JBPD_DFIN_F : UserControl
   {
      public JBPD_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery;
      private string formcaller;
      private long jobpcode;
      private XElement xinput;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         JbpdBs1.DataSource = iCRM.Job_Personel_Dashboards;

         JobpBs1.DataSource = iCRM.Job_Personnels;
         MsttBs1.DataSource = iCRM.Main_States;
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
            var msttcode = MsttCode_Lov.EditValue;
            var ssttcode = SsttCode_Lov.EditValue;

            if ((jobpcode == null|| jobpcode == "") || (msttcode == null || msttcode == "") || (ssttcode == null || ssttcode == "")) return;

            iCRM.INS_JBPD_P((long?)jobpcode, 1, (short?)msttcode, (short?)ssttcode);

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
            var jbpd = JbpdBs1.Current as Data.Job_Personel_Dashboard;

            if (jbpd == null) return;

            iCRM.DEL_JBPD_P(jbpd.CODE);

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

      private void MsttCode_Lov_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 89 /* Execute Mstt_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "MSTT_DFIN_F", 10 /* Execute Actn_CalF_P */) 
                        {
                           Input = 
                              new XElement("MainSub_Stat",
                                 new XAttribute("formcaller", GetType().Name)                           
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MsttCode_Lov_EditValueChanging(object sender, ChangingEventArgs e)
      {
         try
         {
            SsttBs1.DataSource = iCRM.Sub_States.Where(ss => ss.MSTT_SUB_SYS == 1 && ss.MSTT_CODE == (short)e.NewValue);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RecStat_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var jbpd = JbpdBs1.Current as Data.Job_Personel_Dashboard;
            if (jbpd == null) return;

            jbpd.REC_STAT = jbpd.REC_STAT == "002" ? "001" : "002";
            iCRM.UPD_JBPD_P(jbpd.CODE, jbpd.REC_STAT);

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
