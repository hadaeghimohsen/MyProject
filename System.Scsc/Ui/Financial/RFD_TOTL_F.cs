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

namespace System.Scsc.Ui.Financial
{
   public partial class RFD_TOTL_F : UserControl
   {
      public RFD_TOTL_F()
      {
         InitializeComponent();
      }

      bool requery = false;

      private void Execute_Query(bool runAllQuery)
      {
         if (tb_master.SelectedTab == tp_001 || runAllQuery)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "019" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.RQTT_CODE == "004" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID) &&
                     rqst.MDUL_NAME == GetType().Name &&
                     rqst.SECT_NAME == GetType().Name.Substring(0, 3) + "_001_F"
               );
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               if(Rqst.Request_Letters.Any())
               {
                  Gb_Rqlt.Visible = Gb_Fidc.Visible = Gb_Rfpd.Visible = true;
               }
               else
               {
                  Gb_Rqlt.Visible = Gb_Fidc.Visible = Gb_Rfpd.Visible = false;
               }

               RqstRqidGridLookup.EditValue = Rqst.RQST_RQID;
            }
            else if (Rqst.RQID == 0)
            {
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               Gb_Rqlt.Visible = Gb_Fidc.Visible = Gb_Rfpd.Visible = false;
               VF_RqstChngBs1.List.Clear();
               RqstRqidGridLookup.EditValue = null;
            }
         }
         catch
         {
            Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            Gb_Rqlt.Visible = Gb_Fidc.Visible = Gb_Rfpd.Visible = false;
         }
      }

      private void RqroBs1_CurrentChanged(object sender, EventArgs e)
      {
         var crnt = RqroBs1.Current as Data.Request_Row;
         if(crnt == null)return;

         VF_RqstChngBs1.DataSource = iScsc.VF_Request_Changing(crnt.FIGH_FILE_NO).Where(r => r.RQTT_CODE != "004");
      }

      private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;

            iScsc.RFD_TRQT_P(
               new XElement("Process",
                  new XElement("Request", 
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XAttribute("rqstrqid", RqstRqidGridLookup.EditValue ?? 0),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", FILE_NO_LookUpEdit.EditValue)
                     ),
                     new XElement("Request_Letter",
                        new XAttribute("lettno", LETT_NOTextEdit.EditValue ?? ""),
                        new XAttribute("lettdate", Dt_Rqlt_LettDate.Value == null ? "" : Dt_Rqlt_LettDate.Value.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("rqltdesc", RQLT_DESCTextEdit.EditValue ?? ""),
                        new XElement("Finance_Document",
                           new XAttribute("dcmtprictype", DCMT_PRIC_TYPE_LookUpEdit.EditValue ?? ""),
                           new XAttribute("dcmtno", dCMT_NOTextEdit.EditValue ?? ""),
                           new XAttribute("dcmtdate", Dt_Fidc_DcmtDate.Value == null ? "" : Dt_Fidc_DcmtDate.Value.Value.ToString("yyyy-MM-dd")),
                           new XAttribute("dcmtpric", dCMT_PRICTextEdit.EditValue ?? ""),
                           new XAttribute("dcmtdesc", dCMT_DESCTextEdit.EditValue ?? ""),
                           new XElement("Refunds_Payment_Details",
                              RfpdBs1.List.OfType<Data.Refunds_Payment_Detail>()
                              .Select(r => 
                                 new XElement("Refunds_Payment_Detail", 
                                    new XAttribute("rfid", r.RFID),
                                    new XAttribute("rfndstat", r.RFND_STAT)
                                 )
                              )
                           )
                        )
                     )
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               /*
                * Requery Data From Database
                */
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_RqstDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انصراف درخواست مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst != null && Rqst.RQID > 0)
            {
               /*
                *  Remove Data From Tables
                */
               iScsc.ADM_TCNL_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );
               VF_RqstChngBs1.List.Clear();
               RqstRqidGridLookup.EditValue = null;
               //MessageBox.Show(this, "هنرجو حذف گردید!");
            }
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_RqstSav1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;

            iScsc.RFD_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID)                     
                  )
               )
            );
            requery = true;            
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_RqstExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void RqstBnADoc_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
            );
         }
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 94 /* Execute Cmn_Resn_F */){Input = rqst.Request_Rows.FirstOrDefault()},
                     //new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "019")))}
                  })
               );
         }
      }

   }
}
