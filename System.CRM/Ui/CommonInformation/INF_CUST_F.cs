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

namespace System.CRM.Ui.CommonInformation
{
   public partial class INF_CUST_F : UserControl
   {
      public INF_CUST_F()
      {
         InitializeComponent();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServsBs1.DataSource = iCRM.Services.FirstOrDefault(s => s.FILE_NO == fileno);
         var serv = ServsBs1.Current as Data.Service;
         SrpbBs1.DataSource = iCRM.Service_Publics.FirstOrDefault(p => p.RECT_CODE == "004" && p.SERV_FILE_NO == fileno && serv.SRPB_RWNO_DNRM == p.RWNO);
         vfLogcallsBs3.DataSource = iCRM.VF_LogCalls(new XElement("LogCall", new XAttribute("fileno", fileno)));
         vfTrackAttn.DataSource = iCRM.VF_TrackAttendances(new XElement("TrackAttendance", new XAttribute("fileno", fileno)));
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            SrctBs1.EndEdit();

            iCRM.SubmitChanges();
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

      private void Tsb_DelSrct_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            //var srvc = SrctBs1.Current as Data.Service_Contact;

            //iCRM.DEL_SRVC_P(srvc.CODE);
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

      private void Comp_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),                
                new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Query", 
                        new XAttribute("type", "companyzoom"),
                        new XAttribute("compcode", (SrpbBs1.Current as Data.Service_Public).COMP_CODE)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
   }
}
