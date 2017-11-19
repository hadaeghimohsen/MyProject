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

namespace System.Scsc.Ui.Cash
{
   public partial class TRAN_EXPN_F : UserControl
   {
      public TRAN_EXPN_F()
      {
         InitializeComponent();
      }

      bool requery = false;
      long fileno, pydtcode;
      bool checkOK = true;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query(bool runAllQuery)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         FighBs.DataSource = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno);
         PydtBs.DataSource = iScsc.Payment_Details.FirstOrDefault(pd => pd.CODE == pydtcode);
         requery = false;
      }

      private void RqstTran_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydtBs.EndEdit();
            var pydt = PydtBs.Current as Data.Payment_Detail;
            if (pydt == null) return;

            if (pydt.TRAN_CBMT_CODE == null) { Cbmt_Lov.Focus(); return; }
            if (pydt.TRAN_CTGY_CODE == null) { Ctgy_Lov.Focus(); return; }
            if (pydt.TRAN_EXPN_CODE == null) { Expn_Lov.Focus(); return; }

            iScsc.UPD_SEXP_P(
                  new XElement("Request",
                     new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                     new XElement("Payment",
                        new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                        new XElement("Payment_Detail",
                           new XAttribute("code", pydt.CODE),
                           new XAttribute("expncode", pydt.EXPN_CODE),
                           new XAttribute("expnpric", pydt.EXPN_PRIC),
                           new XAttribute("pydtdesc", pydt.PYDT_DESC ?? ""),
                           new XAttribute("qnty", pydt.QNTY ?? 1),
                           new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                           new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                           new XAttribute("mtodcodednrm", pydt.MTOD_CODE_DNRM ?? 0),
                           new XAttribute("ctgycodednrm", pydt.CTGY_CODE_DNRM ?? 0),
                           new XAttribute("tranby", pydt.TRAN_BY ?? CurrentUser),
                           new XAttribute("transtat", "001"),
                           new XAttribute("trandate", pydt.TRAN_DATE == null ? DateTime.Now.ToString("yyyy/MM/dd") : pydt.TRAN_DATE.Value.ToString("yyyy/MM/dd")),
                           new XAttribute("trancbmtcode", pydt.TRAN_CBMT_CODE),
                           //new XAttribute("tranmtodcode", pydt.TRAN_MTOD_CODE),
                           new XAttribute("tranctgycode", pydt.TRAN_CTGY_CODE),
                           new XAttribute("tranexpncode", pydt.TRAN_EXPN_CODE)
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
               Execute_Query(true);
            }
         }
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void RqstSave_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            #region Check Security            
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Desktop",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>227</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 checkOK = false;
                                 MessageBox.Show(this, "عدم دسترسی به ردیف 227 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                              })
                           }
                           #endregion                        
                        })                     
                  })
            );
            #endregion

            if (checkOK)
            {
               PydtBs.EndEdit();
               var pydt = PydtBs.Current as Data.Payment_Detail;
               if (pydt == null && MessageBox.Show(this, "آیا با انتقال هزینه موافق هستین؟", "انتقال هزینه کلاس", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               if (pydt.TRAN_CBMT_CODE == null) { Cbmt_Lov.Focus(); return; }
               if (pydt.TRAN_CTGY_CODE == null) { Ctgy_Lov.Focus(); return; }
               if (pydt.TRAN_EXPN_CODE == null) { Expn_Lov.Focus(); return; }

               iScsc.UPD_SEXP_P(
                     new XElement("Request",
                        new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("code", pydt.CODE),
                              new XAttribute("expncode", pydt.EXPN_CODE),
                              new XAttribute("expnpric", pydt.EXPN_PRIC),
                              new XAttribute("pydtdesc", pydt.PYDT_DESC ?? ""),
                              new XAttribute("qnty", pydt.QNTY ?? 1),
                              new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                              new XAttribute("mtodcodednrm", pydt.MTOD_CODE_DNRM ?? 0),
                              new XAttribute("ctgycodednrm", pydt.CTGY_CODE_DNRM ?? 0),
                              new XAttribute("tranby", pydt.TRAN_BY ?? CurrentUser),
                              new XAttribute("transtat", "002"),
                              new XAttribute("trandate", pydt.TRAN_DATE == null ? DateTime.Now.ToString("yyyy/MM/dd") : pydt.TRAN_DATE.Value.ToString("yyyy/MM/dd")),
                              new XAttribute("trancbmtcode", pydt.TRAN_CBMT_CODE),
                              new XAttribute("tranmtodcode", pydt.TRAN_MTOD_CODE),
                              new XAttribute("tranctgycode", pydt.TRAN_CTGY_CODE),
                              new XAttribute("tranexpncode", pydt.TRAN_EXPN_CODE)
                           )
                        )
                     )
                  );
               requery = true;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
            }
         }
      }

      private void RqstCncl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydtBs.EndEdit();
            var pydt = PydtBs.Current as Data.Payment_Detail;
            if (pydt == null) return;

            iScsc.UPD_SEXP_P(
                  new XElement("Request",
                     new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                     new XElement("Payment",
                        new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                        new XElement("Payment_Detail",
                           new XAttribute("code", pydt.CODE),
                           new XAttribute("expncode", pydt.EXPN_CODE),
                           new XAttribute("expnpric", pydt.EXPN_PRIC),
                           new XAttribute("pydtdesc", pydt.PYDT_DESC ?? ""),
                           new XAttribute("qnty", pydt.QNTY ?? 1),
                           new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                           new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                           new XAttribute("mtodcodednrm", pydt.MTOD_CODE_DNRM ?? 0),
                           new XAttribute("ctgycodednrm", pydt.CTGY_CODE_DNRM ?? 0),
                           new XAttribute("transtat", "003")                           
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
               Execute_Query(true);
            }
         }
      }

      private void Cbmt_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var cbmt = CbmtBs.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == (long)e.NewValue);
            CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.CTGY_STAT == "002" && c.MTOD_CODE == cbmt.MTOD_CODE);
         }
         catch (Exception exc){}
      }

      private void Ctgy_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var pydt = PydtBs.Current as Data.Payment_Detail;
            var rqst = iScsc.Requests.FirstOrDefault(r => r.RQID == pydt.PYMT_RQST_RQID);
            var ctgy = CtgyBs.List.OfType<Data.Category_Belt>().FirstOrDefault(c => c.CODE == (long)e.NewValue);
            ExpnBs.DataSource = 
               iScsc.Expenses.Where(ex => 
                  ex.Regulation.REGL_STAT == "002" && 
                  ex.Regulation.TYPE == "001" && 
                  ex.CTGY_CODE == ctgy.CODE &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == rqst.RQTP_CODE &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == rqst.RQTT_CODE &&
                  ex.EXPN_STAT == "002");
         }
         catch (Exception exc){}
      }

      private void FighBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            try
            {
               Pb_FighImg.ImageProfile = null;
               Pb_FighImg.ImageVisiable = true;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => Pb_FighImg.ImageProfile = bm));
               else
                  Pb_FighImg.ImageProfile = bm;
            }
            catch { Pb_FighImg.Visible = false; }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PydtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pydt = PydtBs.Current as Data.Payment_Detail;
            if (pydt == null) return;

            if(pydt.TRAN_BY != null)
            {
               CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == iScsc.Category_Belts.FirstOrDefault(ct => ct.CODE == pydt.TRAN_CTGY_CODE).MTOD_CODE);
               var rqst = iScsc.Requests.FirstOrDefault(r => r.RQID == pydt.PYMT_RQST_RQID);
               ExpnBs.DataSource =
                  iScsc.Expenses.Where(ex =>
                     ex.Regulation.REGL_STAT == "002" &&
                     ex.Regulation.TYPE == "001" &&
                     ex.CTGY_CODE == pydt.TRAN_CTGY_CODE &&
                     ex.Expense_Type.Request_Requester.RQTP_CODE == rqst.RQTP_CODE &&
                     ex.Expense_Type.Request_Requester.RQTT_CODE == rqst.RQTT_CODE &&
                     ex.EXPN_STAT == "002");
            }
         }
         catch (Exception exc)
         {

         }
      }  
   }
}
