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
using System.Scsc.ExtCode;

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

            var cbmt = mbsp.Fighter_Public.Club_Method;
            CochName_Txt.EditValue = cbmt.Fighter.NAME_DNRM;
            MtodDesc_Txt.EditValue = mbsp.Fighter_Public.Method.MTOD_DESC;
            CtgyDesc_Txt.EditValue = mbsp.Fighter_Public.Category_Belt.CTGY_DESC;
            StrtTime_Txt.EditValue = cbmt.STRT_TIME.ToString().Substring(0, 5);
            EndTime_Txt.EditValue = cbmt.END_TIME.ToString().Substring(0, 5);

            CbmtBs1.DataSource = iScsc.Club_Methods.Where(cm => cm.MTOD_STAT == "002" /*&& cm.MTOD_CODE == mbsp.Fighter_Public.MTOD_CODE*/);

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

            if (!StrtDate_DateTime002.Value.HasValue) { StrtDate_DateTime002.Focus(); return; }
            if (!EndDate_DateTime002.Value.HasValue) { EndDate_DateTime002.Focus(); return; }

            if (StrtDate_DateTime002.Value.Value.Date > EndDate_DateTime002.Value.Value.Date)
            {
               throw new Exception("تاریخ شروع باید از تاریخ پایان کوچکتر با مساوی باشد");
            }

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
                           new XAttribute("numbmontofer", 0),
                           new XAttribute("numbofattnmont", NumbAttnMont_TextEdit002.Text ?? "0"),
                           new XAttribute("sumnumbattnmont", SumNumbAttnMont_TextEdit002.Text ?? "0"),
                           new XAttribute("strttime", StrtTimeN_Txt.Text),
                           new XAttribute("endtime", EndTimeN_Txt.Text)
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

            if (CBMT_CODE_GridLookUpEdit.EditValue == null || CBMT_CODE_GridLookUpEdit.EditValue.ToString() == "") { CBMT_CODE_GridLookUpEdit.Focus(); return; }
            if (CtgyCode_LookupEdit001.EditValue == null || CtgyCode_LookupEdit001.EditValue.ToString() == "") { CtgyCode_LookupEdit001.Focus(); return; }

            if (MessageBox.Show(this, "آیا با ذخیره کردن اطلاعات موافق هستید؟", "ذخیره اطلاعات", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.MBSP_SCHG_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", mbspnew.RQRO_RQST_RQID),
                     new XElement("Request_Row",
                        new XAttribute("fileno", mbspnew.FIGH_FILE_NO),
                        new XAttribute("rwno", mbspnew.RQRO_RWNO)                        
                     ),
                     new XElement("Member_Ship",
                        new XAttribute("cbmtcode", CBMT_CODE_GridLookUpEdit.EditValue),
                        new XAttribute("ctgycode", CtgyCode_LookupEdit001.EditValue),
                        new XAttribute("editpymt", TranExpn_Cb.Checked ? "002" : "001")
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

               #region Ignore Operation
               //if (TranExpn_Cb.Checked)
               //{
               //   bool checkOK = true;
               //   #region Check Security
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "Desktop",
               //         new List<Job>
               //         {
               //            new Job(SendType.External, "Commons",
               //               new List<Job>
               //               {
               //                  #region Access Privilege
               //                  new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
               //                  {
               //                     Input = new List<string> 
               //                     {
               //                        "<Privilege>226</Privilege><Sub_Sys>5</Sub_Sys>", 
               //                        "DataGuard"
               //                     },
               //                     AfterChangedOutput = new Action<object>((output) => {
               //                        if ((bool)output)
               //                           return;
               //                        checkOK = false;
               //                        MessageBox.Show(this, "عدم دسترسی به ردیف 226 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
               //                     })
               //                  }
               //                  #endregion                        
               //               })                     
               //            })
               //   );
               //   #endregion
               //   if (checkOK)
               //   {
               //      var mbspnew = Mbsp002Bs.Current as Data.Member_Ship;
               //      if (mbspnew != null)
               //      {
               //         long? rqid = 0;
               //         if (mbspnew.Request_Row.RQTT_CODE == "004")
               //            rqid = mbspnew.Request_Row.Request.RQST_RQID;
               //         else
               //            rqid = mbspnew.RQRO_RQST_RQID;

               //         var pydt = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == rqid);

               //         if (pydt.Count() > 1 || pydt.Count() == 0)
               //         {
               //            if (MessageBox.Show(this, "کاربر گرامی صورتحساب شما یا وجود ندارد یا بیش از یک آیتم در هزینه های صورتحساب وجود دارد. آیا مایل به بررسی صورتحساب هستید؟", "عدم اصلاح صورتحساب", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               //            {
               //               _DefaultGateway.Gateway(
               //                  new Job(SendType.External, "localhost",
               //                     new List<Job>
               //                     {
               //                        new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
               //                           Input = 
               //                              new XElement("Fighter",
               //                                 new XAttribute("fileno", fileno)                               
               //                              )
               //                        },
               //                        new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
               //                        {
               //                           Input = 
               //                           new XElement("Fighter",
               //                              new XAttribute("fileno", fileno),
               //                              new XAttribute("type", "refresh"),
               //                              new XAttribute("tabfocued", "tp_003")
               //                           )
               //                        }
               //                     })
               //               );
               //            }
               //         }
               //         else
               //         {
               //            _DefaultGateway.Gateway(
               //               new Job(SendType.External, "localhost",
               //                  new List<Job>
               //                  {
               //                     new Job(SendType.Self, 150 /* Execute Tran_Expn_F */),
               //                     new Job(SendType.SelfToUserInterface, "TRAN_EXPN_F", 10 /* execute Actn_CalF_F */)
               //                     {
               //                        Input = 
               //                           new XElement("Payment",
               //                              new XAttribute("pydtcode", pydt.FirstOrDefault().CODE),
               //                              new XAttribute("fileno", fileno),
               //                              new XAttribute("formcaller", GetType().Name),
               //                              new XAttribute("cbmtcode", CBMT_CODE_GridLookUpEdit.EditValue),
               //                              new XAttribute("ctgycode", CtgyCode_LookupEdit001.EditValue)
               //                           )
               //                     }
               //                  }
               //               )
               //            );
               //         }
               //      }
               //   }
               //}
               #endregion
            }
         }
      }

      private void MbspCopy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = Mbsp004Bs.Current as Data.Member_Ship;

            StrtDate_DateTime002.Value = mbsp.STRT_DATE;
            EndDate_DateTime002.Value = mbsp.END_DATE;
            NumbAttnMont_TextEdit002.EditValue = mbsp.NUMB_OF_ATTN_MONT;
            SumNumbAttnMont_TextEdit002.EditValue = mbsp.SUM_ATTN_MONT_DNRM;
            NumbOfDayDnrm_Txt.EditValue = mbsp.NUMB_OF_DAYS_DNRM;
            NumbOfMontDnrm_Txt.EditValue = mbsp.NUMB_OF_MONT_DNRM;

            CBMT_CODE_GridLookUpEdit.EditValue = mbsp.Fighter_Public.CBMT_CODE;
            CtgyCode_LookupEdit001.EditValue = mbsp.Fighter_Public.CTGY_CODE;
         }
         catch { }
      }

      private void CBMT_CODE_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var cbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cb => cb.CODE == (long)e.NewValue);
            MtodDescN_Txt.EditValue = cbmt.Method.MTOD_DESC;
            CtgyDescN_Txt.EditValue = CtgyDesc_Txt.Text;
            StrtTimeN_Txt.EditValue = cbmt.STRT_TIME.ToString().Substring(0, 5);
            EndTimeN_Txt.EditValue = cbmt.END_TIME.ToString().Substring(0, 5);
            CtgyBs1.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");
            CtgyCode_LookupEdit001.EditValue = null;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CBMT_CODE_GridLookUpEdit_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            //long code = (long)CbmtCode_Lov.EditValue;
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 159 /* Execute Bas_Cbmt_F */),
                        new Job(SendType.SelfToUserInterface,"BAS_CBMT_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("formcaller", GetType().Name))},
                        new Job(SendType.SelfToUserInterface, GetType().Name, 0 /* Execute ProccessCmdKey */){Input = Keys.Escape}
                     }
                  )
               );
            }
         }
         catch { }
      }

      private void SUNT_CODELookUpEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {

      }

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            // 1401/07/16 * روز سرنگونی حکومت کثیف آخوندی
            long ctgycode = (long)CtgyCode_LookupEdit001.EditValue;
            var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "001" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && /*exp.MTOD_CODE == mtodcode &&*/ exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

            /// سیستم تغییر تاریخ شروع و پایان
            /// Ctrl : تاریخ پایان بر اساس تاریخ شروع به تعداد دوره
            /// 

            if (ModifierKeys == Keys.Control)
            {
               // تاریخ پایان بر اساس تاریخ شروعی که وارد شده محاسبه گردد
               StrtDate_DateTime002.CommitChanges();
               var strtdate = StrtDate_DateTime002.Value;
               if (strtdate.HasValue)
                  EndDate_DateTime002.Value = strtdate.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               else
               {
                  StrtDate_DateTime002.Value = DateTime.Now;
                  EndDate_DateTime002.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
            }
            else if (ModifierKeys == Keys.Shift)
            {
               // تاریخ شروع به اولین روز همان ماه برگردد
               StrtDate_DateTime002.CommitChanges();
               var strtdate = StrtDate_DateTime002.Value;
               if (strtdate.HasValue)
               {
                  var day = StrtDate_DateTime002.GetText("dd").ToInt32();
                  if (day != 1)
                     StrtDate_DateTime002.Value = StrtDate_DateTime002.Value.Value.AddDays((day - 1) * -1);
                  EndDate_DateTime002.Value = StrtDate_DateTime002.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
               else
               {
                  StrtDate_DateTime002.Value = DateTime.Now;
                  var day = StrtDate_DateTime002.GetText("dd").ToInt32();
                  if (day != 1)
                     StrtDate_DateTime002.Value = StrtDate_DateTime002.Value.Value.AddDays((day - 1) * -1);
                  EndDate_DateTime002.Value = StrtDate_DateTime002.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
            }
            else
            {
               StrtDate_DateTime002.Value = DateTime.Now;
               EndDate_DateTime002.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
            }            
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }
      }
   }
}
