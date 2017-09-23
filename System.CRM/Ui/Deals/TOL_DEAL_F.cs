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

namespace System.CRM.Ui.Deals
{
   public partial class TOL_DEAL_F : UserControl
   {
      public TOL_DEAL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool needclose = true;
      private long fileno;
      private long cashcode, rqid;
      private long rqstrqid;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.FILE_NO == fileno);
         ServsBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002" && Convert.ToInt32(s.ONOF_TAG_DNRM) >= 101);
         CompBs.DataSource = iCRM.Companies;
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.Payment;

            if (pymt.RQST_RQID == 0 || pymt.Request.RQST_STAT == "002")
            { ; }
            else if(pymt.Request.RQST_STAT == "001")
            {
               if (MessageBox.Show(this, "آیا با انصراف درخواست موافق هستین?", "انصراف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               iCRM.CNCL_RQST_P(
                  new XElement("Request",
                     new XAttribute("rqid", pymt.RQST_RQID)
                  )
               );
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
            );
         }
         catch { }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PymtBs.EndEdit();
            var pymt = PymtBs.Current as Data.Payment;

            if (pymt == null) return;
            if (pymt.SERV_FILE_NO == null || pymt.SERV_FILE_NO == 0) { Serv_Lov.Focus(); return; }
            if (pymt.PYMT_DATE == null) { Payment_Date.Focus(); return; }
            if (pymt.PYMT_DESC == null || pymt.PYMT_DESC == "") { Subject_Txt.Focus(); return; }
            if (pymt.PYMT_STAG == null || pymt.PYMT_STAG == "") { PymtStag_Lov.Focus(); return; }
            if (pymt.SINF_SORC_TYPE == null || pymt.SINF_SORC_TYPE == "") { SinfSorcType_Lov.Focus(); return; }


            iCRM.OPR_DRQT_F(
               new XElement("Payment",
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("rqid", pymt.RQST_RQID),
                  new XAttribute("cashcode", pymt.CASH_CODE),
                  new XAttribute("servfileno", pymt.SERV_FILE_NO),
                  new XAttribute("pymtdesc", pymt.PYMT_DESC),
                  new XAttribute("pymtstag", pymt.PYMT_STAG),
                  new XAttribute("sinfsorctype", pymt.SINF_SORC_TYPE),
                  new XAttribute("refservfileno", pymt.REF_SERV_FILE_NO ?? 0),
                  new XAttribute("refcompcode", pymt.REF_COMP_CODE ?? 0),
                  new XAttribute("refothrdesc", pymt.REF_OTHR_DESC ?? ""),
                  new XAttribute("pymtdate", GetDateTimeString(pymt.PYMT_DATE)),
                  new XAttribute("pymtlosttype", pymt.PYMT_LOST_TYPE ?? ""),
                  new XAttribute("pymtlostdesc", pymt.PYMT_LOST_DESC ?? "")
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
            if(requery && needclose)
            {
               var pymt = PymtBs.Current as Data.Payment;

               iCRM.OPR_DSAV_F(
                  new XElement("Payment",
                     new XAttribute("rqstrqid", pymt.RQST_RQID)
                  )
               );

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno)) }
               );
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
               );               
            }
         }
      }

      private string GetDateTimeString(DateTime? dt)
      {
         return
            string.Format("{0}-{1}-{2} {3}:{4}:{5}",
               dt.Value.Year,
               dt.Value.Month,
               dt.Value.Day,
               dt.Value.Hour,
               dt.Value.Minute,
               dt.Value.Second
            );
      }

      private void ShowInfoDeals_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.Payment;

            if (pymt.RQST_RQID == 0)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  PymtBs.DataSource =
                     iCRM.Payments.FirstOrDefault(em =>
                        em.SERV_FILE_NO == fileno &&
                        em.RQST_RQID == em.Service1.RQST_RQID);
               }
               else
                  return;
            }
            requery = true;
         }
         catch (Exception exc)
         { }
         finally
         {
            if (requery)
            {
               var pymt = PymtBs.Current as Data.Payment;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                       {                  
                         new Job(SendType.Self, 36 /* Execute Dtl_Deal-F */),
                         new Job(SendType.SelfToUserInterface, "DTL_DEAL_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", pymt.SERV_FILE_NO), 
                                 new XAttribute("rqstrqid", pymt.RQST_RQID),
                                 new XAttribute("cashcode", pymt.CASH_CODE),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void PymtStag_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (e.NewValue.ToString() == "007")
               LostType_Gb.Visible = true;
            else
               LostType_Gb.Visible = false;
         }
         catch (Exception exc)
         {}
      }

      private void SubRequest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.Payment;
            if (pymt.RQST_RQID == 0) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 56 /* Execute Opt_Rqst_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("fileno", pymt.SERV_FILE_NO),
                              new XAttribute("rqid", pymt.RQST_RQID)
                           )
                     }                     
                  }
               )
            );
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void PymtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.Payment;
            if (pymt == null) return;

            if(pymt.PYMT_STAG == "007")
               LostType_Gb.Visible = true;
            else
               LostType_Gb.Visible = false;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }
   }
}
