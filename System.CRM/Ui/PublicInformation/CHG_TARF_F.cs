using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;
using System.CRM.ExceptionHandlings;

namespace System.CRM.Ui.PublicInformation
{
   public partial class CHG_TARF_F : UserControl
   {
      public CHG_TARF_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno;
      private string srpbtype = "001";
      private bool islock = false;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002" && Convert.ToInt32(s.ONOF_TAG_DNRM) >= 101 && s.FILE_NO == fileno);
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;

            if (Serv_Lov.EditValue == null || Serv_Lov.EditValue.ToString() == "") { Serv_Lov.Focus(); return; }
            if (FrstName_ButnText.EditValue == null || FrstName_ButnText.EditValue.ToString() == "") { FrstName_ButnText.Focus(); return; }
            if (LastName_ButnText.EditValue == null || LastName_ButnText.EditValue.ToString() == "") { LastName_ButnText.Focus(); return; }
            
            //if (EmailAddress_Txt.EditValue == null || EmailAddress_Txt.EditValue.ToString() == "") { EmailAddress_Txt.Focus(); return; }

            //if (Comp_Lov.EditValue == null || Comp_Lov.EditValue.ToString() == "") { Comp_Lov.Focus(); return; }
            if (Btrf_Lov.EditValue == null || Btrf_Lov.EditValue.ToString() == "") { Btrf_Lov.Focus(); return; }
            if (Trfd_Lov.EditValue == null || Trfd_Lov.EditValue.ToString() == "") { Trfd_Lov.Focus(); return; }


            iCRM.CHNG_SRTP_P(
               new XElement("Service",
                  new XAttribute("fileno", serv.FILE_NO),
                  new XAttribute("type", serv.SRPB_TYPE_DNRM),
                  
                  new XAttribute("frstname", FrstName_ButnText.Text),
                  new XAttribute("lastname", LastName_ButnText.Text),
                  
                  new XAttribute("emaladdr", EmailAddress_Txt.Text),
                  new XAttribute("compcode", Comp_Lov.EditValue),
                  new XAttribute("btrfcode", Btrf_Lov.EditValue),
                  new XAttribute("trfdcode", Trfd_Lov.EditValue)
               )
            );

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
               //_DefaultGateway.Gateway(
               //   new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno)) }
               //);
               Btn_Back_Click(null, null);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, FormCaller, 0 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),
                        new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", fileno))},
                     }
                  )
               );
            }
         }
      }

      private void Btrf_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if(e.NewValue != null && e.NewValue.ToString() != "")
            {
               TrfdBs.DataSource = iCRM.Base_Tariff_Details.Where(t => t.BTRF_CODE == (long?)e.NewValue);
            }
         }
         catch 
         {}
      }      
   }
}
