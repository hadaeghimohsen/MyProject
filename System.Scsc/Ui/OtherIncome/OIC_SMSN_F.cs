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
using DevExpress.XtraGrid.Controls;

namespace System.Scsc.Ui.OtherIncome
{
   public partial class OIC_SMSN_F : UserControl
   {
      public OIC_SMSN_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);

      private void Execute_Query()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "001" &&
                     (rqst.RQTT_CODE == "008" || rqst.RQTT_CODE == "004") &&
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
                     (rqst.RQTT_CODE == "008" || rqst.RQTT_CODE == "004") &&
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

            // فرداخوانی اطلاعات کلاس ها
            ExpnBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "009" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "008" &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */
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
               SesnBs1.EndEdit();
               
               if (CLUB_CODELookUpEdit.ItemIndex == -1) { CLUB_CODELookUpEdit.Focus(); return; }

               var fp = FgpbBs1.Current as Data.Fighter_Public;
               var mb = MbspBs1.Current as Data.Member_Ship;

               // 1396/02/06
               SesnBs1.List.OfType<Data.Session>().Where(s => s.TOTL_SESN == null).ToList()
               .ForEach(s => 
                  s.TOTL_SESN = (short)ExpnBs1.List.OfType<Data.Expense>().FirstOrDefault(ex => ex.CODE == s.EXPN_CODE).NUMB_OF_ATTN_MONT
               );

               iScsc.OIC_CROT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                        new XAttribute("rqtpcode", "001"),
                        new XAttribute("rqttcode", /*"008"*/Ts_RqttCode001.IsOn ? "008" : "004"),
                        //new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
                        //new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
                        new XAttribute("clubcode", CLUB_CODELookUpEdit.EditValue),
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                        new XAttribute("cardnumb", FNGR_PRNT_TextEdit.EditValue ?? ""),
                        new XElement("Sessions",
                           SesnBs1.List.OfType<Data.Session>().Select(s => 
                              new XElement("Session",
                                 new XAttribute("snid", s.SNID),
                                 new XAttribute("expncode", s.EXPN_CODE),
                                 new XAttribute("totlsesn", s.TOTL_SESN),
                                 new XAttribute("cbmtcode", s.CBMT_CODE)
                              )
                           )
                        ), 
                        new XElement("Fighter_Public",
                           new XAttribute("fighfileno", fp == null ? 0 : fp.FIGH_FILE_NO),
                           new XAttribute("frstname", FRST_NAME_TextEdit.Text),
                           new XAttribute("lastname", LAST_NAME_TextEdit.Text),
                           new XAttribute("brthdate", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
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
               if (FNGR_PRNT_TextEdit02.Text.Length == 0) { FNGR_PRNT_TextEdit02.Focus(); return; }
               if (!Dt_EndDate002.Value.HasValue) { /*Dt_EndDate002.Focus();*/ Dt_EndDate002.Value = DateTime.Now.AddMonths(1); }

               var r = RqstBs2.Current as Data.Request;

               // 1396/02/06
               SesnBs2.List.OfType<Data.Session>().Where(s => s.TOTL_SESN == null).ToList()
               .ForEach(s =>
                  s.TOTL_SESN = (short)ExpnBs1.List.OfType<Data.Expense>().FirstOrDefault(ex => ex.CODE == s.EXPN_CODE).NUMB_OF_ATTN_MONT
               );

               iScsc.OIC_ICRT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", r == null ? 0 : r.RQID),
                        new XAttribute("actntype", "003"), // More Session
                        new XAttribute("sendexpn", /*SendExpn_LookupEdit002.Text == "" ? "001" :SendExpn_LookupEdit002.EditValue*/ Ts_RqttCode002.IsOn ? "002" : "001"),
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_002_F"),
                        new XElement("Fighter",
                           new XAttribute("fngrprnt", FNGR_PRNT_TextEdit02.Text),
                           new XElement("Member_Ship",
                              new XAttribute("enddate", Dt_EndDate002.Value.Value.ToString("yyyy-MM-dd")),
                              new XElement("Sessions",
                                 SesnBs2.List.OfType<Data.Session>().Select(s =>
                                    new XElement("Session",
                                       new XAttribute("snid", s.SNID),
                                       new XAttribute("expncode", s.EXPN_CODE),
                                       new XAttribute("totlsesn", s.TOTL_SESN),
                                       new XAttribute("cbmtcode", s.CBMT_CODE)
                                    )
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

               iScsc.OIC_DCRT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", r == null ? 0 : r.RQID),
                        new XAttribute("actntype", "003"), // One Session
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_003_F"),
                        new XElement("Fighter",
                           new XAttribute("fngrprnt", FNGR_PRNT_TextEdit03.Text),
                           new XElement("Sessions",
                              SesnBs3.List.OfType<Data.Session>().Select(s =>
                                 new XElement("Session",
                                    new XAttribute("snid", s.SNID),
                                    new XAttribute("expncode", s.EXPN_CODE),
                                    new XAttribute("totlsesn", s.TOTL_SESN),
                                    new XAttribute("cbmtcode", s.CBMT_CODE)
                                 )
                              )
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

               iScsc.OIC_CSAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("setondebt", setOnDebt)
                        )
                     )
                  )
               );

               tc_submaster.SelectedTab = tp_sub001;

               // Save Card In Device
               if (CardNum_Txt.Text != "")
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
                     {
                        Input =
                        new XElement("User",
                           new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text),
                           new XAttribute("cardnumb", CardNum_Txt.Text),
                           new XAttribute("namednrm", FRST_NAME_TextEdit.Text + ", " + LAST_NAME_TextEdit.Text)
                        )
                     }
                  );

               if (SaveAttn_PkBt.PickChecked)
                  AutoAttn();

               CardNum_Txt.Text = "";
            }
            else if(tb_master.SelectedTab == tp_002)
            {
               Scsc.Data.Request Rqst = RqstBs2.Current as Scsc.Data.Request;
               if (Rqst == null) return;

               iScsc.OIC_ICSV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("setondebt", setOnDebt)
                        )
                     )
                  )
               );

               tb_submaster2.SelectedTab = tp_pblcinfo;

               if (SaveAttn002_Pkbt.PickChecked)
                  AutoAttn();
            }
            else if(tb_master.SelectedTab == tp_003)
            {
               Scsc.Data.Request Rqst = RqstBs3.Current as Scsc.Data.Request;
               if (Rqst == null) return;

               iScsc.OIC_DCSV_P(
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
               Gb_Sesn01.Visible = true;
               Gb_Sesn01.Enabled = false;
               //Btn_RqstDelete1.Visible = true;
               RqstBnDelete1.Enabled = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;

               Btn_PymtSave.Enabled = false;
               Gb_Info.Enabled = false;
               Ts_RqttCode001.IsOn = Rqst.RQTT_CODE == "008" ? true : false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               Gb_Sesn01.Visible = true;
               Gb_Sesn01.Enabled = true;
               //Btn_RqstDelete1.Visible = /*Btn_RqstSav1.Visible =*/ true;
               RqstBnDelete1.Enabled = true;
               RqstBnASav1.Enabled = true;

               Btn_PymtSave.Enabled = true;
               Gb_Info.Enabled = true;
               Ts_RqttCode001.IsOn = Rqst.RQTT_CODE == "008" ? true : false;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               Gb_Sesn01.Visible = false;
               RqstBnDelete1.Enabled = false;
               RqstBnASav1.Enabled = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               
               Btn_PymtSave.Enabled = false;
               Gb_Info.Enabled = true;
               Ts_RqttCode001.IsOn = true;
            }
            //Btn_ReLoadCbmt_Click(null, null);
            if(Rqst.RQTT_CODE == "004")
            {
               RqstBnASav1.Enabled = true;
               Btn_PymtSave.Enabled = false;
            }
            else if(Rqst.RQTT_CODE == "008")
            {
               RqstBnASav1.Enabled = false;
               Btn_PymtSave.Enabled = true;               
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            Gb_Sesn01.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            RqstBnDelete1.Enabled = false;
            RqstBnASav1.Enabled = false;
            
            Btn_PymtSave.Enabled = false;
            Gb_Info.Enabled = true;
            Ts_RqttCode001.IsOn = true;
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
               Gb_Sesn02.Visible = true;
               Gb_Sesn02.Enabled = false;

               RqstBnDelete2.Visible = true;
               //Btn_RqstSav1.Visible = false;

               Btn_PymtSave2.Enabled = false;
               Btn_PymtSave2.Enabled = true;
               Gb_Common002.Enabled = false;
               Ts_RqttCode002.IsOn = Rqst.RQTT_CODE == "008" ? true : false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense002.Visible = false;
               Gb_Sesn02.Visible = true;
               Gb_Sesn02.Enabled = true;
               RqstBnDelete2.Visible = true;

               Btn_PymtSave2.Enabled = true;
               Gb_Common002.Enabled = true;

               Ts_RqttCode002.IsOn = Rqst.RQTT_CODE == "008" ? true : false;

               if(Rqst.SEND_EXPN == "001")
               {
                  /* ثبت با هزینه */
                  /*colTotl_Sesn002.OptionsColumn.ReadOnly = false;
                  colTotl_Sesn002.OptionsColumn.AllowEdit = true;

                  colExpn_Code002.OptionsColumn.ReadOnly = false;
                  colExpn_Code002.OptionsColumn.AllowEdit = true;

                  AddInc_Butn.Enabled = DelInc_Butn.Enabled = true;*/

                  RqstBnASav2.Enabled = false;
                  Btn_PymtSave2.Enabled = true;
               }
               else if(Rqst.SEND_EXPN == "002")
               {
                  /* ثبت بدون هزینه */
                  /*colTotl_Sesn002.OptionsColumn.ReadOnly = true;
                  colTotl_Sesn002.OptionsColumn.AllowEdit = false;

                  colExpn_Code002.OptionsColumn.ReadOnly = true;
                  colExpn_Code002.OptionsColumn.AllowEdit = false;

                  AddInc_Butn.Enabled = DelInc_Butn.Enabled = false;*/

                  RqstBnASav2.Enabled = true;
                  Btn_PymtSave2.Enabled = false;
               }
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense002.Visible = false;
               Gb_Sesn02.Visible = false;
               RqstBnDelete2.Visible = /*Btn_RqstSav1.Visible =*/ false;

               Btn_PymtSave2.Enabled = false;
               Gb_Common002.Enabled = true;
               RqstBnASav2.Enabled = false;
            }
            if (Rqst.RQTT_CODE == "004")
            {
               RqstBnASav2.Enabled = true;
               Btn_PymtSave2.Enabled = false;
            }
            else if (Rqst.RQTT_CODE == "008")
            {
               RqstBnASav2.Enabled = false;
               Btn_PymtSave2.Enabled = true;
            }
         }
         catch
         {
            Gb_Expense002.Visible = false;
            Gb_Sesn02.Visible = false;
            RqstBnDelete2.Visible = /*Btn_RqstSav1.Visible =*/ false;

            Btn_PymtSave2.Enabled = false;
            Gb_Common002.Enabled = true;
            RqstBnASav2.Enabled = false;
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
               Gb_Sesn03.Visible = true;
               Gb_Sesn03.Enabled = false;

               Btn_RqstDelete3.Visible = true;

               Btn_PymtSave003.Enabled = true;
               Btn_RqstSav3.Visible = true;
               Gb_Common003.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 85 && Rqst.SSTT_CODE == 1) && Rqst.RQID > 0)
            {
               Gb_Expense003.Visible = false;
               Gb_Sesn03.Visible = true;
               Gb_Sesn03.Enabled = true;
               Btn_RqstDelete3.Visible = /*Btn_RqstSav1.Visible =*/ true;

               Btn_PymtSave003.Enabled = true;
               Btn_RqstSav3.Visible = false;
               Gb_Common003.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense003.Visible = false;
               Gb_Sesn03.Visible = false;
               Btn_RqstDelete3.Visible = false;

               Btn_PymtSave003.Enabled = false;
               Btn_RqstSav3.Visible = false;
               Gb_Common003.Enabled = true;
            }
         }
         catch
         {
            Gb_Expense003.Visible = false;
            Gb_Sesn03.Visible = false;
            Btn_RqstDelete3.Visible = false;

            Btn_PymtSave003.Enabled = false;
            Btn_RqstSav3.Visible = false;
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
               //var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
                  return;
               }*/

               foreach (Data.Payment pymt in PymtsBs1)
               {
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithoutPOS"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID)
                           )
                        )
                     )
                  );
               }

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
               //var pymt = PymtsBs2.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
                  return;
               }*/
               foreach (Data.Payment pymt in PymtsBs2)
               {
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
               }

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
      
      private void Btn_PymtSave_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "با انجام عملیات تسویه حساب دیگر قادر به تغییر نیستید؟ آیا عملیات تسویه حساب انجام شود", "انجام عملیات تسویه حساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_CPYM_P(
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

               iScsc.OIC_CPYM_P(
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
                        new XAttribute("actntype", "003")
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

      private void Btn_PymtDel_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "با انجام عملیات تغییر اطلاعات دوره و حذف هزینه موافق هستید؟ آیا عملیات انجام شود", "انجام عملیات تغییر دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_CPYM_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("pymttype", "002")
                     )
                  )
               );
               tc_submaster.SelectedTab = tp_sub001;
            }
            else if (tb_master.SelectedTab == tp_002)
            {
               if (MessageBox.Show(this, "با انجام عملیات تسویه حساب دیگر قادر به تغییر نیستید؟ آیا عملیات تسویه حساب انجام شود", "انجام عملیات تسویه حساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               Scsc.Data.Request Rqst = RqstBs2.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_CPYM_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("pymttype", "002")
                     )
                  )
               );
            }
            else if (tb_master.SelectedTab == tp_003)
            {
               Scsc.Data.Request Rqst = RqstBs3.Current as Scsc.Data.Request;

               if (Rqst.RQID == 0) return;

               iScsc.OIC_DPYM_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("actntype", "003")
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
         //CARD_NUMBTextEdit.EditValue = Guid.NewGuid().ToString();
      }

      private void DayType_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {

      }

      private void Btn_ReLoadCbmt_Click(object sender, EventArgs e)
      {
         try
         {
            //if (REGN_PRVN_CODELookUpEdit.ItemIndex == -1 && REGN_PRVN_CODELookUpEdit.EditValue == null) { REGN_PRVN_CODELookUpEdit.Focus(); return; }
            //if (REGN_CODELookUpEdit.ItemIndex == -1 && REGN_CODELookUpEdit.EditValue == null) { REGN_CODELookUpEdit.Focus(); return; }
            if (tb_master.SelectedTab == tp_001)
            {
               if (CLUB_CODELookUpEdit.ItemIndex == -1 && CLUB_CODELookUpEdit.EditValue == null) { CLUB_CODELookUpEdit.Focus(); return; }

               if (CLUB_CODELookUpEdit.Text == "") return;
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cb => cb.CLUB_CODE == (long?)CLUB_CODELookUpEdit.EditValue);
            }
            else if(tb_master.SelectedTab == tp_002)
            {
               if (ClubCode2_Lov.Text == "") return;
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cb => cb.CLUB_CODE == (long?)ClubCode2_Lov.EditValue);
            }
         }
         catch { }
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
               FNGR_PRNT_TextEdit.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt64(f.FNGR_PRNT_DNRM)) + 1;
            }
         }
         catch
         {
            if (tb_master.SelectedTab == tp_001)
            {
               FNGR_PRNT_TextEdit.EditValue = 1;
            }
         }
         //try
         //{
         //   if (tb_master.SelectedTab == tp_001)
         //   {
         //      FNGR_PRNT_TextEdit.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt32(f.FNGR_PRNT_DNRM)) + 1;
         //   }
         //}catch
         //{
         //   if (tb_master.SelectedTab == tp_001)
         //   {
         //      FNGR_PRNT_TextEdit.EditValue = 1;
         //   }
         //}
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

      //private void LL_MoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      //{
      //   Pn_MoreInfo.Visible = !Pn_MoreInfo.Visible;
      //   LL_MoreInfo.Text = Pn_MoreInfo.Visible ? "- کمتر ( F3 )" : "+ بیشتر ( F3 )";
      //   if (Pn_MoreInfo.Visible)
      //   {
      //      Gb_Info.Height = 405;
      //      //Gb_Expense.Top = 320;
      //   }
      //   else
      //   {
      //      Gb_Info.Height = 160;
      //      //Gb_Expense.Top = 170;
      //   }
      //}

      private void SesnAdd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtCode_Lov1.EditValue;
            var expn = ExpnCode_Lov1.EditValue;

            if (cbmt == null || cbmt.ToString() == "") { CbmtCode_Lov1.Focus(); return; }
            if (expn == null || expn.ToString() == "") { ExpnCode_Lov1.Focus(); return; }

            SesnBs1.AddNew();
            var sesn = SesnBs1.Current as Data.Session;
            sesn.CBMT_CODE = (long)cbmt;
            sesn.EXPN_CODE = (long)expn;
            sesn.TOTL_SESN = (short)ExpnBs1.List.OfType<Data.Expense>().FirstOrDefault(ex => ex.CODE == (long)expn).NUMB_OF_ATTN_MONT;

            Btn_RqstBnARqt1_Click(null, null);
         }
         catch (Exception exc) { }
      }

      private void SesnDel_Butn_Click(object sender, EventArgs e)
      {
         var Sesn = SesnBs1.Current as Data.Session;
         try
         {            
            if (Sesn == null) return;

            if (MessageBox.Show(this, "آیا با پاک کردن ساعت کلاسی موافقید", "حذف ساعت کلاسی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            iScsc.Sessions.DeleteOnSubmit(Sesn);

            iScsc.SubmitChanges();
            SesnBs1.Remove(Sesn);
         }
         catch
         { SesnBs1.Remove(Sesn); }         
      }

      private void MbspBsi_CurrentChanged(object sender, EventArgs e)
      {
         Btn_ReLoadCbmt_Click(null, null);
      }

      private void AddInc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtCode_Lov2.EditValue;
            var expn = ExpnCode_Lov2.EditValue;

            if (cbmt == null || cbmt.ToString() == "") { CbmtCode_Lov2.Focus(); return; }
            if (expn == null || expn.ToString() == "") { ExpnCode_Lov2.Focus(); return; }

            SesnBs2.AddNew();      
            var sesn = SesnBs2.Current as Data.Session;
            sesn.CBMT_CODE = (long)cbmt;
            sesn.EXPN_CODE = (long)expn;
            sesn.TOTL_SESN = (short)ExpnBs1.List.OfType<Data.Expense>().FirstOrDefault(ex => ex.CODE == (long)expn).NUMB_OF_ATTN_MONT;

            Btn_RqstBnARqt1_Click(null, null);
         }
         catch (Exception exc) { }
         
      }

      private void DelInc_Butn_Click(object sender, EventArgs e)
      {
         var Sesn = SesnBs2.Current as Data.Session;
         try
         {
            if (Sesn == null) return;

            if (iScsc.Sessions.Any(s => s.SNID == Sesn.SESN_SNID && s.MBSP_RECT_CODE == "004" && (s.TOTL_SESN - (s.SUM_MEET_HELD_DNRM ?? 0)) > 0))
            {
               MessageBox.Show(this, "شما قادر به حذف ساعت کلاسی نیستین. بخاطر اینکه تعداد جلسات هنوز به پایان نرسیده است. لطفا برای اینکه تعداد جلسات تغییر نکند عدد 0 را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }

            if (MessageBox.Show(this, "آیا با پاک کردن ساعت کلاسی موافقید", "حذف ساعت کلاسی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            iScsc.Sessions.DeleteOnSubmit(Sesn);

            iScsc.SubmitChanges();
            SesnBs2.Remove(Sesn);
         }
         catch
         { SesnBs2.Remove(Sesn); }         
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

      private void RqstBnQuery02_Click(object sender, EventArgs e)
      {         
         Execute_Query();
      }

      private void Cbmti_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if(tb_master.SelectedTab == tp_001)
            {
               var mtod = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == (long)e.NewValue).Method;
               //Expn1_GV.ApplyFindFilter( mtod.MTOD_DESC );
               var find = Expn1_GV.GridControl.Controls.Find("FindControl", true)[0] as FindControl;
               find.FindEdit.Text = mtod.MTOD_DESC;
            }
            else if(tb_master.SelectedTab == tp_002)
            {
               var mtod = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == (long)e.NewValue).Method;
               var find = Expn2_GV.GridControl.Controls.Find("FindControl", true)[0] as FindControl;
               find.FindEdit.Text = mtod.MTOD_DESC;
            }
         }
         catch (Exception exc)
         {

         }
      }

      private void AutoAttn()
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               var figh = FgpbBs1.Current as Data.Fighter_Public;

               if (figh.FNGR_PRNT == "" && !(figh.TYPE == "002" || figh.TYPE == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
               if (figh.COCH_FILE_NO == null && !(figh.TYPE == "009" || figh.TYPE == "002" || figh.TYPE == "003" || figh.TYPE == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                        
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "accesscontrol"), new XAttribute("fngrprnt", figh.FNGR_PRNT))}
                     })
               );
            }
            else if(tb_master.SelectedTab == tp_002)
            {
               var figh = FighBs2.Current as Data.Fighter;

               if (figh.FNGR_PRNT_DNRM == "" && !(figh.FGPB_TYPE_DNRM == "002" || figh.FGPB_TYPE_DNRM == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
               if (figh.COCH_FILE_NO_DNRM == null && !(figh.FGPB_TYPE_DNRM == "009" || figh.FGPB_TYPE_DNRM == "002" || figh.FGPB_TYPE_DNRM == "003" || figh.FGPB_TYPE_DNRM == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                        
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "accesscontrol"), new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM))}
                     })
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CBMT_CODE_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var cbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cb => cb.CODE == (long)e.NewValue);
            ExpnBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "001" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "008" &&
                  ex.MTOD_CODE == (long)cbmt.MTOD_CODE &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */
               );

         }
         catch (Exception exc)
         {

         }
      }

      private void Btn_Cbmt1_Click(object sender, EventArgs e)
      {
         try
         {
            long code = 0;
            if(tb_master.SelectedTab == tp_001)
               code = (long)CbmtCode_Lov1.EditValue;
            else if(tb_master.SelectedTab == tp_002)
               code = (long)CbmtCode_Lov2.EditValue;


            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
               {
                  new Job(SendType.Self, 149 /* Execute Bas_Wkdy_F */),
                  new Job(SendType.SelfToUserInterface,"BAS_WKDY_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("code", code), new XAttribute("showonly", "002"))}
               }
               )
            );
         }
         catch { }
      }

      private void CbmtCode_Lov2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var cbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cb => cb.CODE == (long)e.NewValue);
            ExpnBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "009" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "008" &&
                  ex.MTOD_CODE == (long)cbmt.MTOD_CODE &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */
               );

         }
         catch (Exception exc)
         {

         }
      }

      private void Cbmt_Gv1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "TIME_DESC" && e.IsGetData)
         {
            var h = ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "END_TIME")).Hours;
            e.Value = h >= 0 && h < 12 ? "صبح" : h >= 12 && h < 15 ? "ظهر" : h >= 15 && h < 18 ? "بعد ظهر" : h >= 18 ? "عصر" : "نام مشخص";
         }
      }
   }
}
