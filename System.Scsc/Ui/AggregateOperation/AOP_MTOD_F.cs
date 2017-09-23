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

namespace System.Scsc.Ui.AggregateOperation
{
   public partial class AOP_MTOD_F : UserControl
   {
      public AOP_MTOD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int agopindx = 0, aodtindx = 0;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         agopindx = AgopBs1.Position;
         aodtindx = AodtBs1.Position;
         AgopBs1.DataSource = iScsc.Aggregation_Operations.Where(a => a.OPRT_TYPE == "003" && (a.OPRT_STAT == "001" || a.OPRT_STAT == "002"));
         AgopBs1.Position = agopindx;
         AodtBs1.Position = aodtindx;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void ClerRegn_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         rEGN_CODEGridLookUpEdit.EditValue = null;
         crnt.REGN_CODE = crnt.REGN_PRVN_CODE = crnt.REGN_PRVN_CNTY_CODE = null;

      }

      private void ClerCtgy_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cTGY_CODEGridLookUpEdit.EditValue = null;
         crnt.MTOD_CODE = crnt.CTGY_CODE = null;
      }

      private void ClerCoch_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cOCH_FILE_NOGridLookUpEdit.EditValue = null;

         crnt.COCH_FILE_NO = null;
      }

      private void ClerNewCtgy_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         New_Ctgy_CodeGridLookUpEdit.EditValue = null;
         crnt.NEW_MTOD_CODE = crnt.NEW_CTGY_CODE = null;
      }

      private void ClerCbmt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cBMT_CODEGridLookUpEdit.EditValue = null;
         crnt.CBMT_CODE = null;
      }

      private void ClerNewCbmtCode_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         NEW_CBMT_CODEGridLookUpEdit.EditValue = null;
         crnt.NEW_CBMT_CODE = null;
      }

      private void ClearAll_Butn_Click(object sender, EventArgs e)
      {
         ClerRegn_Butn_Click(sender, e);
         ClerCtgy_Butn_Click(sender, e);
         ClerCoch_Butn_Click(sender, e);
         ClerCbmt_Butn_Click(sender, e);
         ClerNewCbmtCode_Butn_Click(sender, e);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "011"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "004"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "003"),
                     new XAttribute("oprtstat", crnt.OPRT_STAT ?? "001")
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
               requery = false;
            }
         }
      }

      private void Cncl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "011"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "004"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "003"),
                     new XAttribute("oprtstat", "003")
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
               requery = false;
            }
         }
      }

      private void CretRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "011"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "004"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "003"),
                     new XAttribute("oprtstat", "002")
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message.Substring(0, exc.Message.IndexOf("\r\n")));
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

      private void RecStat_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            crnt.REC_STAT = crnt.REC_STAT == "001" ? "002" : "001";

            AodtBs1.EndEdit();

            iScsc.SubmitChanges();
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

      private void AodtBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;

            RqstBs2.DataSource = iScsc.Requests.First(r => r == crnt.Request);
         }
         catch { }
      }

      private void EndRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "011"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "004"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("newcbmtcode", crnt.NEW_CBMT_CODE ?? 0),
                     new XAttribute("newmtodcode", crnt.NEW_MTOD_CODE ?? 0),
                     new XAttribute("newctgycode", crnt.NEW_CTGY_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "003"),
                     new XAttribute("oprtstat", "004")
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message.Substring(0, exc.Message.IndexOf("\r\n")));
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

      

   }
}
