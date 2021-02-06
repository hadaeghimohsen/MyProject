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
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace System.Scsc.Ui.ChangeRials
{
   public partial class GLR_INDC_F : UserControl
   {
      public GLR_INDC_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private int rqstindex = default(int);

      private void Execute_Query()
      {         
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);

               rqstindex = RqstBs1.Position;

               var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

               RqstBs1.DataSource =
                  iScsc.Requests
                  .Where(
                     rqst =>
                        Rqids.Contains(rqst.RQID)
                  );

               RqstBs1.Position = rqstindex;

               FighBs1.DataSource = iScsc.Fighters.Where(f => Rqids.Contains((long)f.RQST_RQID));
            }
         }
         catch { }
         finally { requery = false; }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               //Gb_Expense.Visible = true;
               //Btn_RqstDelete1.Visible = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               //Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnASav1.Enabled = true;
               Glrd_Pn.Visible = true;
            }
            else if (Rqst.RQID == 0)
            {
               //Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               //DefaultTabPage001();
               RqstBnASav1.Enabled = false;
               Glrd_Pn.Visible = false;
            }
         }
         catch
         {
            //Gb_Expense.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            //DefaultTabPage001();
            RqstBnASav1.Enabled = false;
            Glrd_Pn.Visible = false;
         }
      }

      private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      {
         try
         {
            Glrd_gv.PostEditor();

            var rqst = RqstBs1.Current as Data.Request;
            var glrl = GlrlBs1.Current as Data.Gain_Loss_Rial;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", FIGH_FILE_NOLookUpEdit.EditValue ?? "")
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", glrl == null ? 0 : glrl.GLID),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", Amnt_Txt.EditValue ?? ""),
                        new XAttribute("paiddate", PaidDate_DateTime.Value == null ? "" : PaidDate_DateTime.Value.Value.ToString("yyyy-MM-dd")),                        
                        new XAttribute("dpststat", IncDpst_Rb.Checked ? "002" : "001"),
                        new XAttribute("resndesc", ResnDesc_Txt.EditValue ?? ""),
                        new XElement("Gain_Loss_Rial_Detials",
                           GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>()
                           .Select(gd =>
                              new XElement("Gain_Loss_Rial_Detial",
                                 new XAttribute("rwno", gd.RWNO),
                                 new XAttribute("amnt", gd.AMNT ?? 0),
                                 new XAttribute("rcptmtod", gd.RCPT_MTOD ?? "001"),
                                 new XAttribute("termno", gd.TERM_NO ?? ""),
                                 new XAttribute("tranno", gd.TRAN_NO ?? ""),
                                 new XAttribute("cardno", gd.CARD_NO ?? ""),
                                 new XAttribute("flowno", gd.FLOW_NO ?? ""),
                                 new XAttribute("refno", gd.REF_NO ?? ""),
                                 new XAttribute("actndate", gd.ACTN_DATE.HasValue ? gd.ACTN_DATE.Value.ToString("yyyy-MM-dd") : "")
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
               Execute_Query();
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
               //MessageBox.Show(this, "مشتری حذف گردید!");
            }
            requery = true;
            //tc_pblc.SelectedTab = tp_pblcinfo;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
               //Create_Record();
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs1.List.Count == 0)
                  Btn_RqstExit1_Click(null, null);               
               requery = false;
            }
         }
      }

      private void Btn_RqstSav1_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            if (Rqst != null && Rqst.RQST_STAT == "001")
            {
               var glrl = GlrlBs1.Current as Data.Gain_Loss_Rial;
               if(glrl.AMNT > GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>().Sum(g => g.AMNT))
                  throw (new Exception("مبلغ کل وارد شده با مبلغ های پرداختی یکسان نمی باشد، لطفا بررسی و اصلاح نمایید"));

               if (MessageBox.Show(this, "آیا با ذخیره کردن تغییرات ریالی موافق هستید؟", "ذخیره کردن تغییرات ریالی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               iScsc.GLR_TSAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
               requery = true;            
            }
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

               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs1.List.Count == 0)
                  Btn_RqstExit1_Click(null, null);
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
         //if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
            );
         }
      }

      private void RqstBnADocPicProfile1_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               var rqst = RqstBs1.Current as Data.Request;
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

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
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
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
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
         //if (tb_master.SelectedTab == tp_001)
         {
            var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "025")))}
                  })
               );
         }         
      }

      private void AddAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>().Any(g => g.RWNO == 0))return;

            var glrl = GlrlBs1.Current as Data.Gain_Loss_Rial;
            if(glrl == null)return;

            if (GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>().Sum(g => g.AMNT) >= glrl.AMNT) return;

            GlrdBs1.AddNew();
            var glrd = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;
            glrd.GLRL_GLID = glrl.GLID;
            glrd.RCPT_MTOD = "003";
            glrd.AMNT = glrl.AMNT - (GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>().Sum(g => g.AMNT));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var Glrd = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;
            if (Glrd == null) return;

            if (MessageBox.Show(this, "آیا با حذف ردیف پرداختی سپرده موافق هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.DEL_GLRD_P(
               new XElement("Gain_Loss_Rail_Detail",
                  new XAttribute("glrlglid", Glrd.GLRL_GLID),
                  new XAttribute("rwno", Glrd.RWNO)
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

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }

      private void tbn_POSPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (VPosBs1.List.Count == 0)
               UsePos_Cb.Checked = false;

            if (UsePos_Cb.Checked)
            {
               //foreach (Data.Gain_Loss_Rail_Detail pymt in GlrdBs1)
               var pymt = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;
               {
                  var amnt = (pymt.AMNT);
                  if (amnt == 0) return;

                  var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

                  long psid;
                  if (Pos_Lov.EditValue == null)
                  {
                     var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                     if (posdflts.Count() == 1)
                        Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
                     else
                     {
                        Pos_Lov.Focus();
                        return;
                     }
                  }
                  else
                  {
                     psid = (long)Pos_Lov.EditValue;
                  }

                  if (regl.AMNT_TYPE == "002")
                     amnt *= 10;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                           {
                              new Job(SendType.External, "Commons",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 34 /* Execute PosPayment */)
                                    {
                                       Input = 
                                          new XElement("PosRequest",
                                             new XAttribute("psid", psid),
                                             new XAttribute("subsys", 5),
                                             new XAttribute("rqid", rqst.RQID),
                                             new XAttribute("rqtpcode", ""),
                                             new XAttribute("router", GetType().Name),
                                             new XAttribute("callback", 20),
                                             new XAttribute("amnt", amnt)
                                          )
                                    }
                                 }
                              )                     
                           }
                     )
                  );
               }
            }
            else
            {
               Btn_RqstRqt1_Click(null, null);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
