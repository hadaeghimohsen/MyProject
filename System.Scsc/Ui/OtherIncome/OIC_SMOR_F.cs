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
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace System.Scsc.Ui.OtherIncome
{
   public partial class OIC_SMOR_F : UserControl
   {
      public OIC_SMOR_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);

      private void Execute_Query()
      {
         setOnDebt = false;
         if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "001" &&
                     rqst.RQTT_CODE == "008" &&
                     rqst.RQST_STAT == "001" &&
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
         else if (tb_master.SelectedTab == tp_002)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "009" &&
                     rqst.RQTT_CODE == "008" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs2.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID) &&
                     rqst.MDUL_NAME == GetType().Name &&
                     rqst.SECT_NAME == GetType().Name.Substring(0, 3) + "_002_F"
               );
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "019" &&
                     rqst.RQTT_CODE == "008" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs3.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID) &&
                     rqst.MDUL_NAME == GetType().Name &&
                     rqst.SECT_NAME == GetType().Name.Substring(0, 3) + "_003_F"
               );
         }
      }

      int RqstIndex;
      private void Get_Current_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Count >= 1)
               RqstIndex = RqstBs1.Position;
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            if (RqstBs2.Count >= 1)
               RqstIndex = RqstBs2.Position;
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            if (RqstBs3.Count >= 1)
               RqstIndex = RqstBs3.Position;
         }
      }

      private void Set_Current_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstIndex >= 0)
               RqstBs1.Position = RqstIndex;
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            if (RqstIndex >= 0)
               RqstBs2.Position = RqstIndex;
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            if (RqstIndex >= 0)
               RqstBs3.Position = RqstIndex;
         }
      }

      private void Create_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            RqstBs1.AddNew();
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            RqstBs2.AddNew();
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            RqstBs3.AddNew();
         }
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

               if (REGN_PRVN_CODELookUpEdit.ItemIndex == -1) { REGN_PRVN_CODELookUpEdit.Focus(); return; }
               if (REGN_CODELookUpEdit.ItemIndex == -1) { REGN_CODELookUpEdit.Focus(); return; }
               if (CLUB_CODELookUpEdit.ItemIndex == -1) { CLUB_CODELookUpEdit.Focus(); return; }

               var fp = FgpbBs1.Current as Data.Fighter_Public;
               var mb = MbspBs1.Current as Data.Member_Ship;

               iScsc.OIC_MROT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                        new XAttribute("rqtpcode", "001"),
                        new XAttribute("rqttcode", "008"),
                        new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
                        new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
                        new XAttribute("clubcode", CLUB_CODELookUpEdit.EditValue),
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                        new XElement("Session",
                           new XAttribute("cardnumb", CARD_NUMBTextEdit.Text ?? ""),
                           new XAttribute("expncode", EXPN_CODELookUpEdit.EditValue ?? ""),
                           new XAttribute("totlsesn", TOTL_SESNTextEdit.EditValue ?? "")
                        ),
                        new XElement("Fighter_Public",
                           new XAttribute("fighfileno", fp == null ? 0 : fp.FIGH_FILE_NO),
                           new XAttribute("frstname", FRST_NAME_TextEdit.Text),
                           new XAttribute("lastname", LAST_NAME_TextEdit.Text),
                           new XAttribute("brthdate", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                           new XAttribute("cbmtcode", CBMT_CODE_GridLookUpEdit.EditValue ?? ""),
                           new XAttribute("sextype", SEX_TYPE_LookUpEdit.EditValue ?? ""),
                           new XAttribute("fathname", FATH_NAME_TextEdit.Text),
                           new XAttribute("natlcode", NATL_CODE_TextEdit.Text),
                           new XAttribute("educdeg", EDUC_DEG_LookUpEdit.EditValue ?? ""),
                           new XAttribute("insrnumb", INSR_NUMB_TextEdit.Text),
                           new XAttribute("insrdate", INSR_DATE_PersianDateEdit.Value == null ? "" : INSR_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                           new XAttribute("disecode", DISE_CODE_LookUpEdit.EditValue ?? ""),
                           new XAttribute("cellphon", CELL_PHON_TextEdit.Text),
                           new XAttribute("tellphon", TELL_PHON_TextEdit.Text),
                           new XAttribute("blodgrop", BLOD_GROPLookUpEdit.EditValue ?? ""),
                           new XAttribute("emaladrs", EMAL_ADRS_TextEdit.Text),
                           new XAttribute("postadrs", POST_ADRS_TextEdit.Text),
                           new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text),
                           new XAttribute("suntbuntdeptorgncode", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
                           new XAttribute("suntbuntdeptcode", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
                           new XAttribute("suntbuntcode", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
                           new XAttribute("suntcode", SUNT_CODELookUpEdit.EditValue ?? ""),
                           new XAttribute("cordx", CORD_XTextEdit.Text),
                           new XAttribute("cordy", CORD_YTextEdit.Text)
                        ),
                        new XElement("Member_Ship",
                           new XAttribute("strtdate", MBSP_STRTDATE.Value == null ? "" : MBSP_STRTDATE.Value.Value.ToString("yyyy-MM-dd")),
                           new XAttribute("enddate", MBSP_ENDDATE.Value == null ? "" : MBSP_ENDDATE.Value.Value.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );
            }
            else if (tb_master.SelectedTab == tp_002)
            {
               if (Txt_CardNumb002.Text.Length == 0) { Txt_CardNumb002.Focus(); return; }
               if(!Dt_EndDate002.Value.HasValue){Dt_EndDate002.Focus(); return;}

               var r = RqstBs2.Current as Data.Request;

               iScsc.OIC_IRQT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", r == null ? 0 : r.RQID),
                        new XAttribute("actntype", "002"), // More Session
                        new XAttribute("sendexpn", SendExpn_LookupEdit002.Text == "" ? "001" : SendExpn_LookupEdit002.EditValue),
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_002_F"),
                        new XElement("Fighter",
                           new XAttribute("cardnumb", Txt_CardNumb002.Text),
                           new XAttribute("fngrprnt", FNGR_PRNT_TextEdit02.Text),
                           new XElement("Member_Ship",
                              new XAttribute("enddate", Dt_EndDate002.Value.Value.ToString("yyyy-MM-dd")),
                              SesnBs2.List.OfType<Data.Session>().Select(s =>
                                 new XElement("Session",
                                    new XAttribute("snid", s.SNID),
                                    new XAttribute("expncode", ExpnCode_LookupEdit002.EditValue ?? s.EXPN_CODE),
                                    new XAttribute("totlsesn", s.TOTL_SESN)
                                    //new XAttribute("cbmtcode", s.CBMT_CODE)
                                 )
                              )
                           )                           
                        )
                     )
                  )
               );
            }
            else if (tb_master.SelectedTab == tp_003)
            {
               var r = RqstBs3.Current as Data.Request;

               iScsc.OIC_DRQT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", r == null ? 0 : r.RQID),
                        new XAttribute("actntype", "002"), // One Session
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_003_F"),
                        new XElement("Fighter",
                           new XAttribute("cardnumb", Txt_CardNumb003.Text),
                           new XAttribute("fngrprnt", FNGR_PRNT_TextEdit03.Text),
                           new XElement("Session",
                              new XAttribute("totlsesn", Txt_TotlSesn003.Text)
                           )
                        )
                     )
                  )
               );
            }
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               Data.Request Rqst = RqstBs1.Current as Data.Request;
               if (Rqst == null) return;

               if (MessageBox.Show(this, "آیا با انصراف دادن درخواست موافق هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            else if (tb_master.SelectedTab == tp_002)
            {
               Data.Request Rqst = RqstBs2.Current as Data.Request;
               if (Rqst == null) return;

               if (MessageBox.Show(this, "آیا با انصراف دادن درخواست موافق هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            else if (tb_master.SelectedTab == tp_003)
            {
               Data.Request Rqst = RqstBs3.Current as Data.Request;
               if (Rqst == null) return;

               if (MessageBox.Show(this, "آیا با انصراف دادن درخواست موافق هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            //OldRecdBs1.List.Clear();
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
               if (Rqst == null) return;

               iScsc.OIC_MSAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Payment", 
                           new XAttribute("setondebt", setOnDebt)
                        )
                     )
                  )
               );
            }
            else if(tb_master.SelectedTab == tp_002)
            {
               Scsc.Data.Request Rqst = RqstBs2.Current as Scsc.Data.Request;
               if (Rqst == null) return;

               iScsc.OIC_ISAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("setondebt", setOnDebt)
                        )
                     )
                  )
               );
            }
            else if(tb_master.SelectedTab == tp_003)
            {
               Scsc.Data.Request Rqst = RqstBs3.Current as Scsc.Data.Request;
               if (Rqst == null) return;

               iScsc.OIC_DSAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            requery = true;
         }catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               Gb_Expense.Visible = true;
               Btn_RqstDelete1.Visible = true;
               Btn_RqstSav1.Visible = false;

               Btn_PymtSave.Enabled = false;
               Gb_Info.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               Btn_RqstDelete1.Visible = /*Btn_RqstSav1.Visible =*/ true;
               
               Btn_PymtSave.Enabled = true;
               Gb_Info.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               
               Btn_PymtSave.Enabled = false;
               Gb_Info.Enabled = true;
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            
            Btn_PymtSave.Enabled = false;
            Gb_Info.Enabled = true;
         }
      }

      private void RqstBs2_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs2.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               Gb_Expense002.Visible = true;
               Btn_RqstDelete2.Visible = true;
               //Btn_RqstSav1.Visible = false;

               Btn_PaymentSave2.Enabled = false;
               Gb_Common002.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense002.Visible = false;
               Btn_RqstDelete2.Visible = /*Btn_RqstSav1.Visible =*/ true;

               Btn_PaymentSave2.Enabled = true;
               Gb_Common002.Enabled = true;

               if (Rqst.SEND_EXPN == "001")
               {
                  /* ثبت با هزینه */
                  Txt_TotlSesn002.Properties.ReadOnly = false;
                  ExpnCode_LookupEdit002.Properties.ReadOnly = false;

                  Btn_RqstSave2.Visible = false;
                  Btn_PaymentSave2.Enabled = true;
               }
               else if (Rqst.SEND_EXPN == "002")
               {
                  /* ثبت بدون هزینه */
                  Txt_TotlSesn002.Properties.ReadOnly = true;
                  ExpnCode_LookupEdit002.Properties.ReadOnly = true;

                  Btn_RqstSave2.Visible = true;
                  Btn_PaymentSave2.Enabled = false;
               }
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense002.Visible = false;
               Btn_RqstDelete2.Visible = /*Btn_RqstSav1.Visible =*/ false;

               Btn_PaymentSave2.Enabled = false;
               Gb_Common002.Enabled = true;
               Btn_RqstSave2.Visible = false;
            }
         }
         catch
         {
            Gb_Expense002.Visible = false;
            Btn_RqstDelete2.Visible = /*Btn_RqstSav1.Visible =*/ false;

            Btn_PaymentSave2.Enabled = false;
            Gb_Common002.Enabled = true;
            Btn_RqstSave2.Visible = false;
         }
      }

      private void RqstBs3_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs3.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 85 && Rqst.SSTT_CODE == 1)
            {
               Gb_Expense003.Visible = true;
               Btn_RqstDelete3.Visible = true;

               Btn_PymtSave003.Enabled = false;
               Gb_Common003.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 85 && Rqst.SSTT_CODE == 1) && Rqst.RQID > 0)
            {
               Gb_Expense003.Visible = false;
               Btn_RqstDelete3.Visible = /*Btn_RqstSav1.Visible =*/ true;

               Btn_PymtSave003.Enabled = true;
               Gb_Common003.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense003.Visible = false;
               Btn_RqstDelete3.Visible = false;

               Btn_PymtSave003.Enabled = false;
               Gb_Common003.Enabled = true;
            }
         }
         catch
         {
            Gb_Expense003.Visible = false;
            Btn_RqstDelete3.Visible = false;

            Btn_PymtSave003.Enabled = false;
            Gb_Common003.Enabled = true;
         }
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
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
         else if(tb_master.SelectedTab == tp_002)
         {
            var rqst = RqstBs2.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
            );
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            var rqst = RqstBs3.Current as Data.Request;
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
         else if (tb_master.SelectedTab == tp_002)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"))}
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
         else if (tb_master.SelectedTab == tp_002)
         {
            if (RqstBs2.Current == null) return;
            var crnt = RqstBs2.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            if (RqstBs3.Current == null) return;
            var crnt = RqstBs3.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         else if (tb_master.SelectedTab == tp_002)
         {
            if (RqstBs2.Current == null) return;
            var crnt = RqstBs2.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            if (RqstBs3.Current == null) return;
            var crnt = RqstBs3.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         else if (tb_master.SelectedTab == tp_002)
         {
            if (RqstBs2.Current == null) return;
            var crnt = RqstBs2.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else if (tb_master.SelectedTab == tp_003)
         {
            if (RqstBs3.Current == null) return;
            var crnt = RqstBs3.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void bn_PaymentMethods_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs1.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) { Input = pymt }
            );
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            var rqst = RqstBs2.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs2.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) { Input = pymt }
            );
         }
      }

      private void bn_CashPayment_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
                  return;
               }*/

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithoutPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID)
                  //new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
                        )
                     )
                  )
               );

               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
            }
            else if (tb_master.SelectedTab == tp_002)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs2.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs2.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
                  return;
               }*/

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithoutPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID)
                  //new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
                        )
                     )
                  )
               );

               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
            }
            else if (tb_master.SelectedTab == tp_003)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;

               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  /* Do Delete Payment_Detail */
                  var Crnt  = PydtsBs1.Current as Data.Payment_Detail;
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
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  /* Do Something for insert or update Payment Detail Price */
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
                                       new XAttribute("pydtdesc", pd.PYDT_DESC)
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
                                    new XAttribute("expnpric", pd.EXPN_PRIC),
                                    new XAttribute("pydtdesc", pd.PYDT_DESC ?? "")
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
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
         }
         finally
         {
            if (requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }     

      private void ntb_POSPayment1_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var rqst = RqstBs1.Current as Data.Request;
            var pymt = PymtsBs1.Current as Data.Payment;

            var xSendPos =
               new XElement("Form",
                  new XAttribute("name", GetType().Name),
                  new XAttribute("tabpage", "tp_001"),
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("rqtpcode", rqst.RQTP_CODE),
                     new XAttribute("fileno", rqst.Fighters.FirstOrDefault().FILE_NO),
                     new XElement("Payment",
                        new XAttribute("cashcode", pymt.CASH_CODE),
                        new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
                     )
                  )
               );

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 93 /* Execute Pos_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            if (RqstBs2.Current == null) return;
            var rqst = RqstBs2.Current as Data.Request;
            var pymt = PymtsBs2.Current as Data.Payment;

            var xSendPos =
               new XElement("Form",
                  new XAttribute("name", GetType().Name),
                  new XAttribute("tabpage", "tp_002"),
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("rqtpcode", rqst.RQTP_CODE),
                     new XAttribute("fileno", rqst.Fighters.FirstOrDefault().FILE_NO),
                     new XElement("Payment",
                        new XAttribute("cashcode", pymt.CASH_CODE),
                        new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
                     )
                  )
               );

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 93 /* Execute Pos_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
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
         else if (tb_master.SelectedTab == tp_002)
         {
            var rqst = RqstBs2.Current as Data.Request;
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
         else if (tb_master.SelectedTab == tp_003)
         {
            var rqst = RqstBs3.Current as Data.Request;
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
      
      LookUpEdit lov_prvn;
      private void REGN_PRVN_CODELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         lov_prvn = sender as LookUpEdit;
         REGN_CODELookUpEdit.EditValue = null;
         RegnBs1.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == lov_prvn.EditValue.ToString() && Fga_Urgn_U.Split(',').Contains(r.PRVN_CODE + r.CODE));

         if (RegnBs1.Count == 1) REGN_CODELookUpEdit.EditValue = (RegnBs1.Current as Data.Region).CODE;
         REGN_CODELookUpEdit_EditValueChanged(REGN_CODELookUpEdit, e);
      }

      LookUpEdit lov_regn;
      private void REGN_CODELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            lov_regn = sender as LookUpEdit;
            if (lov_regn.EditValue == null || lov_regn.EditValue.ToString().Length != 3) return;
            ClubBs1.DataSource = iScsc.Clubs.Where(c => Fga_Uclb_U.Contains(c.CODE) &&  (c.REGN_PRVN_CODE + c.REGN_CODE).Contains(lov_prvn.EditValue.ToString() + lov_regn.EditValue.ToString()));

            if (ClubBs1.Count == 1) CLUB_CODELookUpEdit.EditValue = (ClubBs1.Current as Data.Club).CODE;
         }
         catch
         {

         }
      }

      private void Btn_PymtSave_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "با انجام عملیات تسویه حساب دیگر قادر به تغییر نیستید؟ آیا عملیات تسویه حساب انجام شود", "انجام عملیات تسویه حساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_MPYM_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            else if(tb_master.SelectedTab == tp_002)
            {
               if (MessageBox.Show(this, "با انجام عملیات تسویه حساب دیگر قادر به تغییر نیستید؟ آیا عملیات تسویه حساب انجام شود", "انجام عملیات تسویه حساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               Scsc.Data.Request Rqst = RqstBs2.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_OPYM_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            else if(tb_master.SelectedTab == tp_003)
            {
               Scsc.Data.Request Rqst = RqstBs3.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_DPYM_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("actntype", "002")
                     )
                  )
               );
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
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_NewId_Click(object sender, EventArgs e)
      {
         CARD_NUMBTextEdit.EditValue = Guid.NewGuid().ToString();
      }

      private void DayType_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "TIME_DESC" && e.IsGetData)
         {
            var h = ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "END_TIME")).Hours;
            e.Value = h >= 0 && h < 12 ? "صبح" : h >= 12 && h < 15 ? "ظهر" : h >= 15 && h < 18 ? "بعد ظهر" : h >= 18 ? "عصر" : "نام مشخص";
         }
      }

      private void Btn_ReLoadCbmt_Click(object sender, EventArgs e)
      {
         if (REGN_PRVN_CODELookUpEdit.ItemIndex == -1) { REGN_PRVN_CODELookUpEdit.Focus(); return; }
         if (REGN_CODELookUpEdit.ItemIndex == -1) { REGN_CODELookUpEdit.Focus(); return; }
         if (CLUB_CODELookUpEdit.ItemIndex == -1) { CLUB_CODELookUpEdit.Focus(); return; }

         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cb => cb.CLUB_CODE == (long?)CLUB_CODELookUpEdit.EditValue);
      }

      private void CLUB_CODELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         Btn_ReLoadCbmt_Click(null, null);
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "001")))}
                  })
               );
         }
         if (tb_master.SelectedTab == tp_002)
         {
            var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "009")))}
                  })
               );
         }
         if (tb_master.SelectedTab == tp_003)
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

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               FNGR_PRNT_TextEdit.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt32(f.FNGR_PRNT_DNRM)) + 1;
            }
         }
         catch {
            if (tb_master.SelectedTab == tp_001)
            {
               FNGR_PRNT_TextEdit.EditValue = 1;
            }
         }
      }

      private void sUNT_BUNT_DEPT_ORGN_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            OrgnBs1.Position = SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void sUNT_BUNT_DEPT_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            DeptBs1.Position = SUNT_BUNT_DEPT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void sUNT_BUNT_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            BuntBs1.Position = SUNT_BUNT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void LL_MoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         Pn_MoreInfo.Visible = !Pn_MoreInfo.Visible;
         LL_MoreInfo.Text = Pn_MoreInfo.Visible ? "- کمتر ( F3 )" : "+ بیشتر ( F3 )";
         if (Pn_MoreInfo.Visible)
         {
            Gb_Info.Height = 458;
            //Gb_Expense.Top = 320;
         }
         else
         {
            Gb_Info.Height = 216;
            //Gb_Expense.Top = 170;
         }
      }

      private void Txt_CardNumb002_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if (Txt_CardNumb002.Text != "")
            FNGR_PRNT_TextEdit02.Text = Txt_CardNumb002.Text;
      }

      private void MbspBs2_CurrentChanged(object sender, EventArgs e)
      {
         Btn_ReLoadCbmt_Click(null, null);
      }

      private void SesnBs2_CurrentChanged(object sender, EventArgs e)
      {
         var Sesn = SesnBs2.Current as Data.Session;

         if (Sesn == null) return;

         ExpnCode_LookupEdit002.EditValue = Sesn.EXPN_CODE;
      }

      private bool setOnDebt = false;
      private void Btn_InDebt001_Click(object sender, EventArgs e)
      {
         try
         {
            setOnDebt = true;

            _DefaultGateway.Gateway(
              new Job(SendType.External, "Localhost",
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
                                    "<Privilege>192</Privilege><Sub_Sys>5</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Show Error
                                    setOnDebt = false;
                                    MessageBox.Show("خطا - خطا - عدم دسترسی به ردیف 192 سطوح امینتی");
                                    #endregion                           
                                 })
                              },
                              #endregion
                           }
                        ){GenerateInputData = GenerateDataType.Dynamic}
                    })
            );

            if (setOnDebt == false) return;

            if (tb_master.SelectedTab == tp_001)
            {
               
               if (MessageBox.Show(this, "عملیات بدهکاری و ذخیره نهایی کردن انجام شود؟", "بدهکاری و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs1.Current as Data.Payment;


               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }
   }
}
