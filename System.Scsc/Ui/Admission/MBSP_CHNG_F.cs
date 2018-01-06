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

namespace System.Scsc.Ui.Admission
{
   public partial class MBSP_CHNG_F : UserControl
   {
      public MBSP_CHNG_F()
      {
         InitializeComponent();
      }

      bool requery = false;
      long fileno;
      short mbsprwno;

      private void Back_Btn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            Mbsp004Bs.DataSource = iScsc.Member_Ships.FirstOrDefault(mb => mb.RWNO == mbsprwno && mb.RECT_CODE == "004" && mb.FIGH_FILE_NO == fileno);
            var mbsp = Mbsp004Bs.Current as Data.Member_Ship;

            Mbsp002Bs.DataSource = iScsc.Member_Ships.FirstOrDefault(mb => mb.RQRO_RQST_RQID == mbsp.RQRO_RQST_RQID && mb.RECT_CODE == "002" && mb.FIGH_FILE_NO == fileno);
         }
         catch (Exception ) { }
         requery = false;
      }

      private void RqstMbsp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbspnew = Mbsp004Bs.Current as Data.Member_Ship;
            /*if(mbspnew == null)
            {
               Mbsp002Bs.AddNew();
               mbspnew = Mbsp002Bs.Current as Data.Member_Ship;
               var mbspold = Mbsp004Bs.Current as Data.Member_Ship;
               mbspnew.RQRO_RQST_RQID = mbspold.RQRO_RQST_RQID;
               mbspnew.RQRO_RWNO = mbspold.RQRO_RWNO;
               mbspnew.FIGH_FILE_NO = mbspold.FIGH_FILE_NO;
               mbspnew.TYPE = mbspold.TYPE;
               mbspnew.RECT_CODE = "002";
               mbspnew.STRT_DATE = mbspold.STRT_DATE;
               mbspnew.END_DATE = mbspold.END_DATE;
               mbspnew.PRNT_CONT = mbspold.PRNT_CONT;
               mbspnew.NUMB_OF_ATTN_MONT = mbspold.NUMB_OF_ATTN_MONT;
               mbspnew.SUM_ATTN_MONT_DNRM = mbspold.SUM_ATTN_MONT_DNRM;
               mbspnew.NUMB_OF_ATTN_WEEK = mbspold.NUMB_OF_ATTN_WEEK;
               mbspnew.ATTN_DAY_TYPE = mbspold.ATTN_DAY_TYPE;
            }*/

            StrtDate_DateTime002.CommitChanges();
            EndDate_DateTime002.CommitChanges();

            iScsc.MBSP_TCHG_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", mbspnew.RQRO_RQST_RQID),                     
                     new XElement("Request_Row",
                        new XAttribute("fileno", mbspnew.FIGH_FILE_NO),
                        new XAttribute("rwno", mbspnew.RQRO_RWNO),
                        new XElement("Member_Ship",
                           new XAttribute("strtdate", StrtDate_DateTime002.Value.HasValue ? StrtDate_DateTime002.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("enddate", EndDate_DateTime002.Value.HasValue ? EndDate_DateTime002.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("prntcont", "1"),
                           new XAttribute("numbofattnmont", NumbAttnMont_TextEdit002.Text ?? "0"),
                           new XAttribute("sumnumbattnmont", SumNumbAttnMont_TextEdit002.Text ?? "0")
                        )
                     )
                  )
               )
            );

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

      private void SaveMbsp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbspnew = Mbsp002Bs.Current as Data.Member_Ship;
            if (mbspnew == null) return;

            iScsc.MBSP_SCHG_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", mbspnew.RQRO_RQST_RQID),
                     new XElement("Request_Row",
                        new XAttribute("fileno", mbspnew.FIGH_FILE_NO),
                        new XAttribute("rwno", mbspnew.RQRO_RWNO)                        
                     )
                  )
               )
            );

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
            }
         }
      }

      private void MbspCncl_Butn_Click(object sender, EventArgs e)
      {

      }
   }
}
