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

namespace System.CRM.Ui.Activity
{
   public partial class OPT_CLON_F : UserControl
   {
      public OPT_CLON_F()
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
         CompBs.DataSource = iCRM.Companies;
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
            if (Serv_Lov.EditValue == null || Serv_Lov.EditValue.ToString() == "") { Serv_Lov.Focus(); return; }
            if (FrstName_ButnText.EditValue == null || FrstName_ButnText.EditValue.ToString() == "") { FrstName_ButnText.Focus(); return; }
            if (LastName_ButnText.EditValue == null || LastName_ButnText.EditValue.ToString() == "") { LastName_ButnText.Focus(); return; }

            iCRM.CLON_SERV_P(
               new XElement("Service",
                  new XAttribute("fileno", Serv_Lov.EditValue),
                  new XAttribute("frstname", FrstName_ButnText.Text),
                  new XAttribute("lastname", LastName_ButnText.Text),
                  new XAttribute("emaladrs", EmailAddress_Txt.Text),
                  new XAttribute("cellphon", CellPhon_Txt.Text),
                  new XAttribute("tellphon", TellPhon_Txt.Text),
                  new XAttribute("postadrs", PostAddress_Txt.Text),
                  new XAttribute("compcode", Comp_Lov.EditValue)

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
               var serv = (from s in iCRM.Services
                          join rr in iCRM.Request_Rows on s.FILE_NO equals rr.SERV_FILE_NO
                          join r in iCRM.Requests on rr.RQST_RQID equals r.RQID
                          where r.RQTP_CODE == "001"
                             && r.RQTT_CODE == "004"
                             && r.RQST_STAT == "002"
                             && r.RQST_DATE.Value.Date == DateTime.Now.Date
                             && r.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby s.CONF_DATE descending
                          select s).FirstOrDefault();

               Btn_Back_Click(null, null);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        //new Job(SendType.SelfToUserInterface, FormCaller, 0 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.Self, serv.SRPB_TYPE_DNRM == "001" ? 24 : 34 /* Execute Inf_Lead_F OR Inf_Cont_F */),
                        new Job(SendType.SelfToUserInterface, serv.SRPB_TYPE_DNRM == "001" ? "INF_LEAD_F" : "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                     }
                  )
               );
            }
         }
      }

      private void Comp_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                       new List<Job>
                       {
                          new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdkey */){Input = Keys.Escape},
                          new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),
                          new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 10 /* Execute Actn_CalF_P */)
                          {
                             Input = 
                              new XElement("Region",
                                 new XAttribute("type", "public"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                          }
                       })
                  );
                  break;
               case 2:
                  Execute_Query();
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
         }
      }            
   }
}
