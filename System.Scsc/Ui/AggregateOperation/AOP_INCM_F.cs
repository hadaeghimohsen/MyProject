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
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.AggregateOperation
{
   public partial class AOP_INCM_F : UserControl
   {
      public AOP_INCM_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int agopindx = 0, aodtindx = 0;
      private long? cbmtcode;
      private DateTime? fromdate = null, todate = null;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         agopindx = AgopBs1.Position;
         aodtindx = AodtBs1.Position;
         AgopBs1.DataSource = iScsc.Aggregation_Operations.Where(a => a.OPRT_TYPE == "006" && (a.OPRT_STAT == "001" || a.OPRT_STAT == "002"));
         AgopBs1.Position = agopindx;
         AodtBs1.Position = aodtindx;

         ExpnBs1.DataSource =
            iScsc.Expenses.Where(ex =>
               ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
               ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
               ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
               ex.EXPN_STAT == "002" /* هزینه های فعال */
            );        

         Grop_FLP.Controls.Clear();
         var allItems = new Button();

         allItems.Text = "همه موارد";
         allItems.Tag = 0;

         allItems.Click += GropButn_Click;
         Grop_FLP.Controls.Add(allItems);

         ExpnBs1.List.OfType<Data.Expense>().OrderBy(e => e.GROP_CODE).GroupBy(e => e.Group_Expense).ToList().ForEach(
            g =>
            {
               var b = new Button();
               if (g.Key != null)
               {
                  b.Text = g.Key.GROP_DESC;
                  b.Tag = g.Key.CODE;
               }
               else
                  b.Text = "سایر موارد";
               b.Click += GropButn_Click;
               Grop_FLP.Controls.Add(b);
            }
         );
         requery = false;
      }

      void GropButn_Click(object sender, EventArgs e)
      {
         Button b = (Button)sender;
         if (b.Tag != null)
            if (Convert.ToInt64(b.Tag) != 0)
               Expn_Gv.ActiveFilterString = string.Format("GROP_CODE = {0}", b.Tag);
            else
               Expn_Gv.ActiveFilterString = "";
         else
            Expn_Gv.ActiveFilterString = "GROP_CODE IS NULL";
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

      private void ClerCbmt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         cBMT_CODEGridLookUpEdit.EditValue = null;
         crnt.CBMT_CODE = null;
      }

      private void ClerFromDate_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         crnt.FROM_DATE = null;
      }

      private void ClerToDate_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         crnt.TO_DATE = null;
      }

      private void ClerNumMontOfer_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         crnt.NUMB_MONT_OFFR = null;
         crnt.NUMB_OF_ATTN_MONT = null;
      }

      private void ClerRqtt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         crnt.RQTT_CODE = null;
      }

      private void ClerCndo_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;
         if (crnt == null) return;

         Cndo_Lov.EditValue = null;
         crnt.UNIT_BLOK_CNDO_CODE = null;
      }

      private void ClerBlok_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;
         if (crnt == null) return;

         Cblk_Lov.EditValue = null;
         crnt.UNIT_BLOK_CODE = null;
      }

      private void ClerUnit_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;
         if (crnt == null) return;

         Cunt_Lov.EditValue = null;
         crnt.UNIT_CODE = null;
      }

      private void ClearAll_Butn_Click(object sender, EventArgs e)
      {
         ClerRegn_Butn_Click(sender, e);
         ClerCtgy_Butn_Click(sender, e);
         ClerCoch_Butn_Click(sender, e);
         ClerCbmt_Butn_Click(sender, e);
         ClerFromDate_Butn_Click(sender, e);
         ClerToDate_Butn_Click(sender, e);
         ClerNumMontOfer_Butn_Click(sender, e);
         ClerRqtt_Butn_Click(sender, e);
         ClerCndo_Butn_Click(sender, e);
         ClerBlok_Butn_Click(sender, e);
         ClerUnit_Butn_Click(sender, e);
         // Clear All Detail Record
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            AgopBs1.EndEdit();
            var crnt = AgopBs1.Current as Data.Aggregation_Operation;

            if (crnt == null) return;

            iScsc.INS_AGOP_P(
               new XElement("Process",
                  new XElement("Aggregation_Operation",
                     new XAttribute("code", crnt.CODE),
                     new XAttribute("regnprvncntycode", crnt.REGN_PRVN_CNTY_CODE ?? "001"),
                     new XAttribute("regnprvncode", crnt.REGN_PRVN_CODE ?? "017"),
                     new XAttribute("regncode", crnt.REGN_CODE ?? "001"),
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "006"),
                     new XAttribute("oprtstat", crnt.OPRT_STAT ?? "001"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("unitblokcndocode", crnt.UNIT_BLOK_CNDO_CODE ?? ""),
                     new XAttribute("unitblokcode", crnt.UNIT_BLOK_CODE ?? ""),
                     new XAttribute("unitcode", crnt.UNIT_CODE ?? "")
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
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "006"),
                     new XAttribute("oprtstat", "003"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("unitblokcndocode", crnt.UNIT_BLOK_CNDO_CODE ?? ""),
                     new XAttribute("unitblokcode", crnt.UNIT_BLOK_CODE ?? ""),
                     new XAttribute("unitcode", crnt.UNIT_CODE ?? "")

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
               if (AgopBs1.List.Count == 0)
                  Back_Butn_Click(null, null);
               //requery = false;
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
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "006"),
                     new XAttribute("oprtstat", "002"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("unitblokcndocode", crnt.UNIT_BLOK_CNDO_CODE ?? ""),
                     new XAttribute("unitblokcode", crnt.UNIT_BLOK_CODE ?? ""),
                     new XAttribute("unitcode", crnt.UNIT_CODE ?? "")

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
            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if(aodt == null)return;

            RqstBs1.DataSource = aodt.Request;
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
                     new XAttribute("rqtpcode", crnt.RQTP_CODE ?? "016"),
                     new XAttribute("rqttcode", crnt.RQTT_CODE ?? "001"),
                     new XAttribute("mtodcode", crnt.MTOD_CODE ?? 0),
                     new XAttribute("ctgycode", crnt.CTGY_CODE ?? 0),
                     new XAttribute("cochfileno", crnt.COCH_FILE_NO ?? 0),
                     new XAttribute("cbmtcode", crnt.CBMT_CODE ?? 0),
                     new XAttribute("oprttype", crnt.OPRT_TYPE ?? "006"),
                     new XAttribute("oprtstat", "004"),
                     new XAttribute("fromdate", crnt.FROM_DATE.HasValue ? crnt.FROM_DATE.Value.ToString("yyyy-MM-dd") : ""),
                     new XAttribute("todate", crnt.TO_DATE.HasValue ? crnt.TO_DATE.Value.ToString("yyyy-MM-dd") : "")
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

      private void ClerNewCtgy_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         crnt.NEW_MTOD_CODE = crnt.NEW_CTGY_CODE = null;
      }

      private void ClerNewCbmt_Butn_Click(object sender, EventArgs e)
      {
         var crnt = AgopBs1.Current as Data.Aggregation_Operation;

         if (crnt == null) return;

         crnt.NEW_CBMT_CODE = null;
      }

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }
      }

      private void AgopBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            if (AodtBs1.List.Count > 0)
               Gb_Expense.Visible = true;
            else
               Gb_Expense.Visible = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CbmtCode_Lov_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
         }
         catch (Exception)
         {

         }
      }

      private void AutoAttn_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            
         }
         catch
         {

         }
      }

      long rqstindex = 0;

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (AllPydsRqro_Cb.Checked)
            {
               AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>()
                  .ToList()
                  .ForEach(aod =>
                  {
                     if(aod.RQST_RQID != null)
                     {
                        var pymt = aod.Request.Payments.FirstOrDefault();
                        if (pymt == null) return;

                        int? amnt = null;
                        switch (PydsType_Butn.Tag.ToString())
                        {
                           case "0":
                              if (!(Convert.ToInt32(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt32(PydsAmnt_Txt.EditValue) <= 100))
                              {
                                 PydsAmnt_Txt.EditValue = null;
                                 PydsAmnt_Txt.Focus();
                              }

                              amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt32(PydsAmnt_Txt.EditValue)) / 100;
                              break;
                           case "1":
                              amnt = Convert.ToInt32(PydsAmnt_Txt.EditValue);
                              if (amnt == 0) return;
                              break;
                        }

                        iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, amnt, "002", "002", PydsDesc_Txt.Text);
                     }
                  });
            }
            else
            {
               var pymt = PymtBs1.Current as Data.Payment;
               if (pymt == null) return;

               int? amnt = null;
               switch (PydsType_Butn.Tag.ToString())
               {
                  case "0":
                     if (!(Convert.ToInt32(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt32(PydsAmnt_Txt.EditValue) <= 100))
                     {
                        PydsAmnt_Txt.EditValue = null;
                        PydsAmnt_Txt.Focus();
                     }

                     amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt32(PydsAmnt_Txt.EditValue)) / 100;
                     break;
                  case "1":
                     amnt = Convert.ToInt32(PydsAmnt_Txt.EditValue);
                     if (amnt == 0) return;
                     break;
               }

               iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, amnt, "002", "002", PydsDesc_Txt.Text);

            }

            PydsAmnt_Txt.EditValue = null;
            PydsDesc_Txt.EditValue = null;
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

      private void DeltPyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pyds = PydsBs1.Current as Data.Payment_Discount;
            if (pyds == null) return;

            iScsc.DEL_PYDS_P(pyds.PYMT_CASH_CODE, pyds.PYMT_RQST_RQID, pyds.RQRO_RWNO, pyds.RWNO);

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

      private void PydsType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydsType_Butn.Text = PydsType_Butn.Tag.ToString() == "0" ? "مبلغی" : "درصدی";
            PydsType_Butn.Tag = PydsType_Butn.Tag.ToString() == "0" ? "1" : "0";

            if (PydsType_Butn.Tag.ToString() == "0")
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "درصد تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 3;
            }
            else
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "مبلغ تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 0;
            }
            PydsAmnt_Txt.Focus();

         }
         catch { }
      }

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {

      }

      private void AddItem_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            foreach (var rqst in AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Select(r => r.Request))
            {
               var pymt = rqst.Payments.FirstOrDefault();
               if (!pymt.Payment_Details.Any(pd => pd.EXPN_CODE == expn.CODE))
               {
                  long? pydtcode = 0;
                  iScsc.INS_PYDT_P(
                    pymt.CASH_CODE,
                    rqst.RQID,
                    1,
                    expn.CODE,
                    "001",
                    expn.PRIC,
                    expn.EXTR_PRCT,
                    null,
                    1, null, null, null, null, null, expn.EXPN_DESC, null, null, null, null,
                    ref pydtcode
                  );
               }
               else
               {
                  var pydt = pymt.Payment_Details.FirstOrDefault(pd => pd.EXPN_CODE == expn.CODE);
                  iScsc.UPD_SEXP_P(
                      new XElement("Request",
                        new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("code", pydt.CODE),
                              new XAttribute("expncode", pydt.EXPN_CODE),
                              new XAttribute("expnpric", pydt.EXPN_PRIC),
                              new XAttribute("pydtdesc", pydt.PYDT_DESC),
                              new XAttribute("qnty", (pydt.QNTY ?? 1) + 1),
                              new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0)
                           )
                        )
                     )
                  );
               }
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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void RemoveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            /* Do Delete Payment_Detail */
            var Crnt = PydtsBs1.Current as Data.Payment_Detail;
            var rqst = RqstBs1.Current as Data.Request;
            iScsc.DEL_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", rqst.RQID),
                  new XElement("Payment",
                     new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                     new XElement("Payment_Detail",
                        new XAttribute("code", Crnt.CODE)
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
               Execute_Query();
         }
      }

      private void SaveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Pydt_Gv.PostEditor();
            /* Do Something for insert or update Payment Detail Price */
            var rqst = RqstBs1.Current as Data.Request;

            PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.CRET_BY == null).ToList()
               .ForEach(pd =>
               {
                  rqst = RqstBs1.Current as Data.Request;
                  iScsc.INS_SEPD_P(
                     new XElement("Request",
                        new XAttribute("rqid", rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("expncode", pd.EXPN_CODE),
                              new XAttribute("expnpric", pd.EXPN_PRIC),
                              new XAttribute("pydtdesc", pd.PYDT_DESC ?? ""),
                              new XAttribute("qnty", pd.QNTY ?? 1),
                              new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
                           )
                        )
                     )
                  );
               }
            );

            PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.CODE != 0).ToList()
               .ForEach(pd =>
               {
                  rqst = RqstBs1.Current as Data.Request;
                  iScsc.UPD_SEXP_P(
                     new XElement("Request",
                        new XAttribute("rqid", rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("code", pd.CODE),
                              new XAttribute("expncode", pd.EXPN_CODE),
                              new XAttribute("expnpric", pd.EXPN_PRIC),
                              new XAttribute("pydtdesc", pd.PYDT_DESC),
                              new XAttribute("qnty", pd.QNTY ?? 1),
                              new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
                           )
                        )
                     )
                  );
               }
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
               Execute_Query();
         }
      }

      private void CBMT_CODE_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (requery) { requery = false; return; }
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

            //pydt.Club_Method = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == (long?)e.NewValue);
            iScsc.UPD_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                  new XElement("Payment",
                     new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                     new XElement("Payment_Detail",
                        new XAttribute("code", pydt.CODE),
                        new XAttribute("expncode", pydt.EXPN_CODE),
                        new XAttribute("expnpric", pydt.EXPN_PRIC),
                        new XAttribute("pydtdesc", pydt.PYDT_DESC),
                        new XAttribute("qnty", pydt.QNTY ?? 1),
                        new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                        new XAttribute("cbmtcodednrm", e.NewValue ?? 0)
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
               Execute_Query();
         }
      }

      private void SaveCbmtForAll_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtAll_Lov.EditValue;
            if (cbmt == null || cbmt.ToString() == "") return;

            foreach (var rqst in AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Select(r => r.Request))
            {
               var pymt = rqst.Payments.FirstOrDefault();

               foreach (var pydt in pymt.Payment_Details)
               {
                  iScsc.UPD_SEXP_P(
                     new XElement("Request",
                        new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("code", pydt.CODE),
                              new XAttribute("expncode", pydt.EXPN_CODE),
                              new XAttribute("expnpric", pydt.EXPN_PRIC),
                              new XAttribute("pydtdesc", pydt.PYDT_DESC),
                              new XAttribute("qnty", (pydt.QNTY ?? 1) + 1),
                              new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", cbmt ?? 0)
                           )
                        )
                     )
                  );
               }               
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Cndo_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            CblkBs1.DataSource = iScsc.Cando_Blocks.Where(b => b.CNDO_CODE == e.NewValue.ToString());
         }
         catch { }
      }

      private void Cblk_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            CuntBs1.DataSource = iScsc.Cando_Block_Units.Where(u => u.BLOK_CNDO_CODE == Cndo_Lov.EditValue.ToString() && u.BLOK_CODE == e.NewValue.ToString());
         }
         catch { }
      }   
   

   }
}
