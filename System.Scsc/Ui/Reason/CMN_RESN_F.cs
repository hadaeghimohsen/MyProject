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
using System.Data.SqlClient;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Reason
{
   public partial class CMN_RESN_F : UserControl
   {
      public CMN_RESN_F()
      {
         InitializeComponent();
      }

      bool requery = false;
      long rqrorqstrqid;
      short rqrorwno;
      string rqtpcode;


      private void Execute_Query()
      {
         RsrqBs1.DataSource = iScsc.Reason_Requests.Where(rsrq => rsrq.Request_Row == RqroBs1.Current);
      }

      private void RsrqGv1_Bn_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن دلایل درخواست موافقید؟", "حذف دلایل درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  /* Do Delete Payment_Detail */
                  var Crnt = RsrqBs1.Current as Data.Reason_Request;
                  iScsc.DEL_RSRQ_P(
                     new XElement("Request",
                        new XAttribute("rqid", Crnt.RQRO_RQST_RQID),
                        new XElement("Request_Row",
                           new XAttribute("rwno", Crnt.RQRO_RWNO),
                           new XElement("Reason_Request",
                              new XAttribute("rwno", Crnt.RWNO)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  /* Do Something for insert or update Payment Detail Price */
                  RsrqBs1.List.OfType<Data.Reason_Request>().Where(p => p.CRET_BY == null).ToList()
                     .ForEach(pd =>
                     {                        
                        iScsc.INS_RSRQ_P(
                           new XElement("Request",
                              new XAttribute("rqid", rqrorqstrqid),
                              new XElement("Request_Row",
                                 new XAttribute("rwno", rqrorwno),
                                 new XElement("Reason_Request",
                                    new XAttribute("resnrqtpcode", rqtpcode),
                                    new XAttribute("resnrwno", pd.RESN_RWNO),
                                    new XAttribute("othrdesc", pd.OTHR_DESC ?? "")
                                 )
                              )
                           )
                        );
                     }
                  );

                  RsrqBs1.List.OfType<Data.Reason_Request>().Where(p => p.RWNO != 0).ToList()
                     .ForEach(pd =>
                     {
                        iScsc.UPD_RSRQ_P(
                           new XElement("Request",
                              new XAttribute("rqid", pd.RQRO_RQST_RQID),
                              new XElement("Request_Row",
                                 new XAttribute("rwno", pd.RQRO_RWNO),
                                 new XElement("Reason_Request",
                                    new XAttribute("resnrqtpcode", pd.RESN_RQTP_CODE),
                                    new XAttribute("resnrwno", pd.RESN_RWNO),
                                    new XAttribute("othrdesc", pd.OTHR_DESC ?? "")
                                 )
                              )
                           )
                        );
                     }
                  );
                  requery = true;
                  break;
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
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

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
