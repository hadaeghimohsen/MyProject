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
using System.IO;
using System.MaxUi;

namespace System.Scsc.Ui.Notifications
{
   public partial class CHOS_MBSP_F : UserControl
   {
      public CHOS_MBSP_F()
      {
         InitializeComponent();
      }

      //private bool requery = false;

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            MbspBs.DataSource =
               iScsc.Member_Ships.Where(
                  mb => mb.FIGH_FILE_NO == Convert.ToInt64(fileno) &&
                        mb.TYPE == "001" &&
                        mb.RECT_CODE == "004" && 
                        mb.END_DATE.Value.Date >= DateTime.Now.Date &&
                        (mb.RWNO == 1 || mb.Request_Row.RQTT_CODE == "001") &&
                        (mb.NUMB_OF_ATTN_MONT > 0 && mb.NUMB_OF_ATTN_MONT > mb.SUM_ATTN_MONT_DNRM)
               );
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Butn_TakePicture_Click(object sender, EventArgs e)
      {
          try
          {
              if (true)
              {
                  var rqst = (from r in iScsc.Requests
                             join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                             where rr.FIGH_FILE_NO == Convert.ToInt64(fileno)
                                && r.RQTP_CODE == "001"
                            select r).FirstOrDefault();
                  if (rqst == null) return;

                  var result = (
                           from r in iScsc.Regulations
                           join rqrq in iScsc.Request_Requesters on r equals rqrq.Regulation
                           join rqdc in iScsc.Request_Documents on rqrq equals rqdc.Request_Requester
                           join rcdc in iScsc.Receive_Documents on rqdc equals rcdc.Request_Document
                           where r.TYPE == "001"
                              && r.REGL_STAT == "002"
                              && rqrq.RQTP_CODE == rqst.RQTP_CODE
                              && rqrq.RQTT_CODE == rqst.RQTT_CODE
                              && rqdc.DCMT_DSID == 13930903120048833 // عکس 4*3
                              && rcdc.RQRO_RQST_RQID == rqst.RQID
                              && rcdc.RQRO_RWNO == 1
                           select rcdc).FirstOrDefault();
                  if (result == null) return;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self,  59 /* Execute Cmn_Dcmt_F */){ Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() },
                        new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Action",
                                 new XAttribute("type", "001"),
                                 new XAttribute("typedesc", "Force Active Camera Picture Profile"),
                                 new XElement("Document",
                                    new XAttribute("rcid", result.RCID)
                                 )
                              )
                        }
                     }
                     )
                  );

              }
          }
          catch
          {

          }
      }

      private void Attn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                     new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", FngrPrnt_Lbl.Text), new XAttribute("mbsprwno", mbsp.RWNO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            RqstBnExit1_Click(null, null);
         }
      }
   }
}
