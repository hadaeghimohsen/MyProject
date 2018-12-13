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
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace System.Scsc.Ui.Admission
{
   public partial class ADM_TOTL_F : UserControl
   {
      public ADM_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);
      private int rqstindex = default(int);

      private void Execute_Query()
      {
         setOnDebt = false;
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            //{
            //   iScsc = new Data.iScscDataContext(ConnectionString);
            //   var Rqids = iScsc.VF_Requests(new XElement("Request"))
            //      .Where(rqst =>
            //            rqst.RQTP_CODE == "001" &&
            //            rqst.RQST_STAT == "001" &&
            //            (rqst.RQTT_CODE == "001" || rqst.RQTT_CODE == "004" || rqst.RQTT_CODE == "005" || rqst.RQTT_CODE == "006") &&
            //            rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            //   RqstBs1.DataSource =
            //      iScsc.Requests
            //      .Where(
            //         rqst =>
            //            Rqids.Contains(rqst.RQID)
            //      )
            //      .OrderByDescending(
            //         rqst =>
            //            rqst.RQST_DATE
            //      ); 

            //   RqstBs1.Position = rqstindex;

            //   if (RqstBs1.Count == 0 || (RqstBs1.Count == 1 && RqstBs1.List.OfType<Data.Request>().FirstOrDefault().RQID == 0))
            //   {
            //      DefaultTabPage001();
            //   } 
            //}

            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "009" &&
                        (rqst.RQTT_CODE == "001" || rqst.RQTT_CODE == "004" || rqst.RQTT_CODE == "005" || rqst.RQTT_CODE == "006") &&
                        rqst.RQST_STAT == "001" &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

               RqstBs3.DataSource =
                  iScsc.Requests
                  .Where(
                     rqst =>
                        Rqids.Contains(rqst.RQID)
                  )
                  .OrderByDescending(
                     rqst =>
                        rqst.RQST_DATE
                  ); 

               RqstBs3.Position = rqstindex;

               // 1396/11/02 * بدست آوردن شماره پرونده های درگیر در تمدید
               FighBs3.DataSource = iScsc.Fighters.Where(f => Rqids.Contains((long)f.RQST_RQID));

               if (RqstBs3.Count == 0 || (RqstBs3.Count == 1 && RqstBs3.List.OfType<Data.Request>().FirstOrDefault().RQID == 0))
               {
                  DefaultTabPage003();
               }
            }
         }
         catch { }
      }

      private void DefaultTabPage003()
      {
         /* تنظیم کردن ناحیه و استان قابل دسترس */
         RQTT_CODE_LookUpEdit3.EditValue = "001";
      }

      private void DefaultTabPage002()
      {
         /* تنظیم کردن ناحیه و استان قابل دسترس */
         
      }

      //private void DefaultTabPage001()
      //{
      //   /* تنظیم کردن ناحیه و استان قابل دسترس */
      //   REGN_PRVN_CODELookUpEdit.EditValue = Fga_Uprv_U.Split(',')[0];
      //   REGN_CODELookUpEdit.EditValue = Fga_Urgn_U.Split(',')[0].Substring(3);
      //   RQTT_CODE_LookUpEdit1.EditValue = "001";
      //}

      int RqstIndex;
      private void Get_Current_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstBs1.Count >= 1)
         //      RqstIndex = RqstBs1.Position;
         //}
         
         {
            if (RqstBs3.Count >= 1)
               RqstIndex = RqstBs3.Position;
         }
      }

      private void Set_Current_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstIndex >= 0)
         //      RqstBs1.Position = RqstIndex;
         //}
         //if (tb_master.SelectedTab == tp_002)
         //{
         //   if (RqstIndex >= 0)
         //      RqstBs2.Position = RqstIndex;
         //}
         //else if (tb_master.SelectedTab == tp_003)
         {
            if (RqstIndex >= 0)
               RqstBs3.Position = RqstIndex;
         }
      }

      private void Create_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   RqstBs1.AddNew();
         //   RQTT_CODE_LookUpEdit1.Focus();
         //}
         //if (tb_master.SelectedTab == tp_002)
         //{
         //   RqstBs2.AddNew();
         //   RQTT_CODE_LookUpEdit2.Focus();
         //}
         //else if (tb_master.SelectedTab == tp_003)
         {
            RqstBs3.AddNew();
            RQTT_CODE_LookUpEdit3.Focus();
         }
      }

      //private void LL_MoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      //{
      //   Pn_MoreInfo.Visible = !Pn_MoreInfo.Visible;
      //   LL_MoreInfo.Text = Pn_MoreInfo.Visible ? "- کمتر ( F3 )" : "+ بیشتر ( F3 )";
      //   if (Pn_MoreInfo.Visible && LL_MoreInfo.Visible)
      //   {
      //      Gb_Info.Height = 449;
      //      //Gb_Expense.Top = 320;
      //   }
      //   else
      //   {
      //      Gb_Info.Height = 210;
      //      //Gb_Expense.Top = 170;
      //   }
      //}

      //private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var Rqst = RqstBs1.Current as Data.Request;

      //      if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
      //      {
      //         Gb_Expense.Visible = true;
      //         Btn_RqstDelete1.Visible = true;
      //         Btn_RqstSav1.Visible = false;
      //      }
      //      else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
      //      {
      //         Gb_Expense.Visible = false;
      //         Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
      //      }
      //      else if (Rqst.RQID == 0)
      //      {
      //         Gb_Expense.Visible = false;
      //         Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
      //         DefaultTabPage001();
      //      }
      //   }
      //   catch
      //   {
      //      Gb_Expense.Visible = false;
      //      Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
      //      DefaultTabPage001();
      //   }
      //}

      //private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var Rqst = RqstBs1.Current as Data.Request;
      //      rqstindex = RqstBs1.Position;

      //      if (Rqst == null || Rqst.RQID >= 0)
      //      {
      //         var FighExists =
      //            iScsc.Fighters
      //            .Where(f =>
      //               //(Rqst == null || Rqst.RQID == 0) &&
      //               (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "004" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006") &&
      //               (f.NAME_DNRM.Contains(FRST_NAME_TextEdit.Text) || f.NAME_DNRM.Contains(LAST_NAME_TextEdit.Text) || f.FNGR_PRNT_DNRM == FNGR_PRNT_TextEdit.Text &&
      //               (f.CONF_STAT == "002") &&
      //               Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
      //               Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM)
      //               ));
      //         FighBs1.DataSource = FighExists;
      //         if (FighBs1.Count >= 1)
      //         {
      //            fighterGridControl.Visible = true;
      //            if ((Rqst != null && Rqst.CRET_BY != null) || MessageBox.Show(this, "آیا مشترییی که وارد کرده اید در لیست پایین وجود دارد؟", "لیست مشترییان", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
      //            {
      //               /*
      //                * ثبت مشتریی جدید در سیستم
      //                */
      //               if (NumbOfMontDnrm_TextEdit001.Text.Trim() == "")
      //                  NumbOfMontDnrm_TextEdit001.Text = "1";

      //               iScsc.ADM_TRQT_F(
      //                  new XElement("Process",
      //                     new XElement("Request",
      //                        new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
      //                        new XAttribute("rqtpcode", "001"),
      //                        new XAttribute("rqttcode", RQTT_CODE_LookUpEdit1.EditValue),
      //                        new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
      //                        new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
      //                        new XElement("Fighter",
      //                           new XAttribute("fileno", Rqst == null ? 0 : Rqst.Fighters.FirstOrDefault() == null ? 0 : Rqst.Fighters.FirstOrDefault().FILE_NO),
      //                           new XElement("Frst_Name", FRST_NAME_TextEdit.Text ?? ""),
      //                           new XElement("Last_Name", LAST_NAME_TextEdit.Text ?? ""),
      //                           new XElement("Fath_Name", FATH_NAME_TextEdit.Text ?? ""),
      //                           new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue ?? ""),
      //                           new XElement("Natl_Code", NATL_CODE_TextEdit.Text ?? ""),
      //                           new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
      //                           new XElement("Cell_Phon", CELL_PHON_TextEdit.Text ?? ""),
      //                           new XElement("Tell_Phon", TELL_PHON_TextEdit.Text ?? ""),
      //                           new XElement("Type", RQTT_CODE_LookUpEdit1.EditValue.ToString() == "004" ? "001" : RQTT_CODE_LookUpEdit1.EditValue ?? ""),
      //                           new XElement("Post_Adrs", POST_ADRS_TextEdit.Text ?? ""),
      //                           new XElement("Emal_Adrs", EMAL_ADRS_TextEdit.Text ?? ""),
      //                           new XElement("Insr_Numb", INSR_NUMB_TextEdit.Text ?? ""),
      //                           new XElement("Insr_Date", INSR_DATE_PersianDateEdit.Value == null ? "" : INSR_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
      //                           new XElement("Educ_Deg", EDUC_DEG_LookUpEdit.EditValue ?? ""),
      //                           new XElement("Cbmt_Code", CBMT_CODE_GridLookUpEdit.EditValue ?? ""),
      //                           new XElement("Dise_Code", DISE_CODE_LookUpEdit.EditValue ?? ""),
      //                           new XElement("Blod_Grop", BLOD_GROPLookUpEdit.EditValue ?? ""),
      //                           new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.EditValue ?? ""),
      //                           new XElement("Sunt_Bunt_Dept_Orgn_Code", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
      //                           new XElement("Sunt_Bunt_Dept_Code", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
      //                           new XElement("Sunt_Bunt_Code", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
      //                           new XElement("Sunt_Code", SUNT_CODELookUpEdit.EditValue ?? ""),
      //                           new XElement("Cord_X", CORD_XTextEdit.EditValue ?? ""),
      //                           new XElement("Cord_Y", CORD_YTextEdit.EditValue ?? ""),
      //                           new XElement("Mtod_Code", MtodCode_LookupEdit001.EditValue ?? ""),
      //                           new XElement("Ctgy_Code", CtgyCode_LookupEdit001.EditValue ?? ""),
      //                           new XElement("Most_Debt_Clng", SE_MostDebtClngAmnt.Value)
      //                        ),
      //                        new XElement("Member_Ship",
      //                           new XAttribute("strtdate", StrtDate_DateTime001.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : StrtDate_DateTime001.Value.Value.ToString("yyyy-MM-dd")),
      //                           new XAttribute("enddate", NumbOfMontDnrm_RB001.Checked ? (StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd")) : (EndDate_DateTime001.Value == null ? StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(1).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd") : EndDate_DateTime001.Value.Value.ToString("yyyy-MM-dd"))),
      //                           new XAttribute("numbmontofer", NumbMontOfer_TextEdit001.Text ?? "0"),
      //                           new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit001.Text ?? "0"),
      //                           new XAttribute("numbofattnweek", NumbOfAttnWeek_TextEdit001.Text ?? "0"),
      //                           new XAttribute("attndaytype", /*AttnDayType_Lov001.EditValue ??*/ "7")
      //                        )
      //                     )
      //                  )
      //               );
      //               fighterGridControl.Visible = false;
      //               //MessageBox.Show(this, "مشتریی جدید در سیستم ثبت گردید");
      //               requery = true;
      //            }
      //            else
      //            {
      //               /*
      //                * مشتری قبلا در سیستم ثبت شده و عملیات تمدید انجام میدهیم
      //                */
      //               MessageBox.Show(this, "مشتری قبلا در سیستم ثبت شده است، پس عملیات تمدید را انجام بدهید");
      //            }
      //         }
      //         else
      //         {
      //            fighterGridControl.Visible = false;
      //            /*
      //             * ثبت مشتریی جدید در سیستم
      //             */
      //            if (NumbOfMontDnrm_TextEdit001.Text.Trim() == "")
      //               NumbOfMontDnrm_TextEdit001.Text = "1";

      //            if (Rqst == null || Rqst.RQST_STAT == null || Rqst.RQST_STAT == "001")
      //               iScsc.ADM_TRQT_F(
      //                     new XElement("Process",
      //                        new XElement("Request",
      //                           new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
      //                           new XAttribute("rqtpcode", "001"),
      //                           new XAttribute("rqttcode", RQTT_CODE_LookUpEdit1.EditValue),
      //                           new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
      //                           new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
      //                           new XElement("Fighter",
      //                              new XAttribute("fileno", Rqst == null ? 0 : Rqst.Fighters.FirstOrDefault() == null ? 0 : Rqst.Fighters.FirstOrDefault().FILE_NO),
      //                              new XElement("Frst_Name", FRST_NAME_TextEdit.Text),
      //                              new XElement("Last_Name", LAST_NAME_TextEdit.Text),
      //                              new XElement("Fath_Name", FATH_NAME_TextEdit.Text),
      //                              new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue),
      //                              new XElement("Natl_Code", NATL_CODE_TextEdit.Text),
      //                              new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
      //                              new XElement("Cell_Phon", CELL_PHON_TextEdit.Text),
      //                              new XElement("Tell_Phon", TELL_PHON_TextEdit.Text),
      //                              new XElement("Type", RQTT_CODE_LookUpEdit1.EditValue),
      //                              new XElement("Post_Adrs", POST_ADRS_TextEdit.Text),
      //                              new XElement("Emal_Adrs", EMAL_ADRS_TextEdit.Text),
      //                              new XElement("Insr_Numb", INSR_NUMB_TextEdit.Text),
      //                              new XElement("Insr_Date", INSR_DATE_PersianDateEdit.Value == null ? "" : INSR_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
      //                              new XElement("Educ_Deg", EDUC_DEG_LookUpEdit.EditValue ?? ""),
      //                              new XElement("Cbmt_Code", CBMT_CODE_GridLookUpEdit.EditValue ?? ""),
      //                              new XElement("Dise_Code", DISE_CODE_LookUpEdit.EditValue ?? ""),
      //                              new XElement("Blod_Grop", BLOD_GROPLookUpEdit.EditValue ?? ""),
      //                              new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.EditValue ?? ""),
      //                              new XElement("Sunt_Bunt_Dept_Orgn_Code", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
      //                              new XElement("Sunt_Bunt_Dept_Code", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
      //                              new XElement("Sunt_Bunt_Code", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
      //                              new XElement("Sunt_Code", SUNT_CODELookUpEdit.EditValue ?? ""),
      //                              new XElement("Cord_X", CORD_XTextEdit.EditValue ?? ""),
      //                              new XElement("Cord_Y", CORD_YTextEdit.EditValue ?? ""),
      //                              new XElement("Mtod_Code", MtodCode_LookupEdit001.EditValue ?? ""),
      //                              new XElement("Ctgy_Code", CtgyCode_LookupEdit001.EditValue ?? "")
      //                           ),
      //                           new XElement("Member_Ship",
      //                              new XAttribute("strtdate", StrtDate_DateTime001.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : StrtDate_DateTime001.Value.Value.ToString("yyyy-MM-dd")),
      //                              new XAttribute("enddate", NumbOfMontDnrm_RB001.Checked ? (StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd")) : (EndDate_DateTime001.Value == null ? StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(1).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd") : EndDate_DateTime001.Value.Value.ToString("yyyy-MM-dd"))),
      //                              new XAttribute("numbmontofer", NumbMontOfer_TextEdit001.Text ?? "0"),
      //                              new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit001.Text ?? "0"),
      //                              new XAttribute("numbofattnweek", NumbOfAttnWeek_TextEdit001.Text ?? "0"),
      //                              new XAttribute("attndaytype", /*AttnDayType_Lov001.EditValue ??*/ "7")
      //                           )
      //                        )
      //                     )
      //                  );
      //            else if (Rqst.RQST_STAT == "002") return;
      //            //MessageBox.Show(this, "مشتریی جدید در سیستم ثبت گردید");
      //            requery = true;
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         /*
      //          * Requery Data From Database
      //          */
      //         Get_Current_Record();
      //         Execute_Query();
      //         Set_Current_Record();
      //         requery = false;
      //      }
      //   }
      //}

      //private void Btn_RqstDelete1_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if (MessageBox.Show(this, "آیا با انصراف و حذف ثبت نام مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //      var Rqst = RqstBs1.Current as Data.Request;

      //      if (Rqst != null && Rqst.RQID > 0)
      //      {
      //         /*
      //          *  Remove Data From Tables
      //          */
      //         iScsc.ADM_TCNL_F(
      //            new XElement("Process",
      //               new XElement("Request",
      //                  new XAttribute("rqid", Rqst.RQID),
      //                  new XElement("Fighter",
      //                     new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
      //                  )
      //               )
      //            )
      //         );
      //         //MessageBox.Show(this, "مشتری حذف گردید!");
      //      }
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Get_Current_Record();
      //         Execute_Query();
      //         Set_Current_Record();
      //         Create_Record();
      //         requery = false;
      //      }
      //   }
      //}

      bool setOnDebt = false;
      //private void Btn_RqstSav1_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var Rqst = RqstBs1.Current as Data.Request;
      //      if (Rqst != null && Rqst.RQST_STAT == "001")
      //      {
      //         iScsc.ADM_TSAV_F(
      //            new XElement("Process",
      //               new XElement("Request",
      //                  new XAttribute("rqid", Rqst.RQID),
      //                  new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
      //                  new XAttribute("regncode", Rqst.REGN_CODE),
      //                  new XElement("Fighter",
      //                     new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
      //                  ),
      //                  new XElement("Payment",
      //                     new XAttribute("setondebt", setOnDebt),
      //                     PydtsBs1.List.OfType<Data.Payment_Detail>().ToList()
      //                     .Select(pd => 
      //                        new XElement("Payment_Detail",
      //                           new XAttribute("code", pd.CODE),
      //                           new XAttribute("rcptmtod", pd.RCPT_MTOD ?? "")
      //                        )
      //                     )
      //                  )
      //               )
      //            )
      //         );
      //         requery = true;
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if(requery)
      //      {
      //         Get_Current_Record();
      //         Execute_Query();
      //         Set_Current_Record();
      //         Create_Record();
      //         requery = false;
      //      }            
      //   }
      //}

      private void Btn_RqstExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "ADM_TOTL_F", 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      //private void RqstBs2_CurrentChanged(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var Rqst = RqstBs2.Current as Data.Request;

      //      if (Rqst.RQID == 0)
      //      {
      //         DefaultTabPage002();
      //      }
      //   }
      //   catch
      //   {            
      //      DefaultTabPage002();
      //   }
      //}


      //private void Btn_RqstDelete2_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if (MessageBox.Show(this, "آیا با انصراف و حذف ثبت مربی مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //      var Rqst = RqstBs2.Current as Data.Request;

      //      if (Rqst != null && Rqst.RQID > 0)
      //      {
      //         /*
      //          *  Remove Data From Tables
      //          */
      //         iScsc.ADM_TCNL_F(
      //            new XElement("Process",
      //               new XElement("Request",
      //                  new XAttribute("rqid", Rqst.RQID),
      //                  new XElement("Fighter",
      //                     new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
      //                  )
      //               )
      //            )
      //         );
      //         //MessageBox.Show(this, "مربی حذف گردید!");
      //      }
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Get_Current_Record();
      //         Execute_Query();
      //         Set_Current_Record();
      //         Create_Record();
      //         requery = false;
      //      }
      //   }
      //}

      //private void Btn_RqstSav2_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var Rqst = RqstBs2.Current as Data.Request;
      //      iScsc.ADM_MSAV_F(
      //         new XElement("Process",
      //            new XElement("Request",
      //               new XAttribute("rqid", 0),
      //               new XAttribute("rqtpcode", "001"),
      //               new XAttribute("rqttcode", RQTT_CODE_LookUpEdit2.EditValue),
      //               new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit2.EditValue),
      //               new XAttribute("regncode", REGN_CODELookUpEdit2.EditValue),
      //               new XElement("Fighter",
      //                  new XAttribute("fileno", 0),
      //                  new XElement("Frst_Name", Frst_Name_TextEdit2.Text),
      //                  new XElement("Last_Name", Last_Name_TextEdit2.Text),
      //                  new XElement("Fath_Name", Fath_Name_TextEdit2.Text),
      //                  new XElement("Coch_Deg", COCH_DEG_LookUpEdit2.EditValue),
      //                  new XElement("Coch_Crtf_Date", COCH_CRTF_DATE_PersianDateEdit2.Value == null ? "" : COCH_CRTF_DATE_PersianDateEdit2.Value.Value.ToString("yyyy-MM-dd")),
      //                  new XElement("Gudg_Deg", GUDG_DEG_LookUpEdit2.EditValue),
      //                  new XElement("glob_Code", GLOB_CODE_TextEdit2.EditValue),
      //                  new XElement("Sex_Type", Sex_Type_LookUpEdit2.EditValue),
      //                  new XElement("Natl_Code", NATL_CODE_TextEdit2.Text),
      //                  new XElement("Brth_Date", Brth_Date_PersianDateEdit2.Value == null ? "" : Brth_Date_PersianDateEdit2.Value.Value.ToString("yyyy-MM-dd")),
      //                  new XElement("Cell_Phon", Cell_Phon_TextEdit2.Text),
      //                  new XElement("Tell_Phon", Tell_Phon_TextEdit2.Text),
      //                  new XElement("Type", RQTT_CODE_LookUpEdit2.EditValue),
      //                  new XElement("Post_Adrs", Post_Adrs_TextEdit2.Text),
      //                  new XElement("Emal_Adrs", Emal_Adrs_TextEdit2.Text),
      //                  new XElement("Insr_Numb", Insr_Numb_TextEdit2.Text),
      //                  new XElement("Insr_Date", Insr_Date_PersianDateEdit2.Value == null ? "" : Insr_Date_PersianDateEdit2.Value.Value.ToString("yyyy-MM-dd")),
      //                  new XElement("Educ_Deg", Educ_Code_LookUpEdit2.EditValue ?? ""),
      //                  new XElement("Mtod_Code", MTOD_CODE_LookUpEdit2.EditValue ?? ""),
      //                  new XElement("Dise_Code", Dise_Code_LookUpEdit2.EditValue ?? ""),
      //                  new XElement("Calc_Expn_Type", CALC_EXPN_TYPE_LookUpEdit2.EditValue ?? ""),
      //                  new XElement("Blod_grop", Blod_Grop_LookupEdit.EditValue ?? ""),
      //                  new XElement("Fngr_Prnt", Fngr_Prnt_TextEdit2.EditValue ?? ""),
      //                  new XElement("Dpst_Acnt_Slry_Bank", DpstAcntSlryBank_Text2.EditValue ?? ""),
      //                  new XElement("Dpst_Acnt_Slry", DpstAcntSlry_Text2.EditValue ?? "")
      //               )
      //            )
      //         )
      //      );
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Get_Current_Record();
      //         Execute_Query();
      //         Set_Current_Record();
      //         Create_Record();
      //         requery = false;
      //      }
      //   }
      //}

      private void LL_MoreInfo2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         //Pn_MoreInfo2.Visible = !Pn_MoreInfo2.Visible;
         //LL_MoreInfo2.Text = Pn_MoreInfo2.Visible ? "- کمتر ( F3 )" : "+ بیشتر ( F3 )";
         //if (Pn_MoreInfo2.Visible && LL_MoreInfo2.Visible)
         //{
         //   Gb_Info2.Height = 330;
         //   //Gb_Expense.Top = 320;
         //}
         //else
         //{
         //   Gb_Info2.Height = 150;
         //   //Gb_Expense.Top = 170;
         //}
      }

      //private void BTN_MBSP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      //{
      //   try
      //   {
      //      var Figh = FighBs1.Current as Data.Fighter;
      //      iScsc.UCC_TRQT_P(
      //         new XElement("Process",
      //            new XElement("Request",
      //               new XAttribute("rqtpcode", "009"),
      //               new XAttribute("rqttcode", Figh.FGPB_TYPE_DNRM),
      //               new XElement("Request_Row",
      //                  new XAttribute("fileno", Figh.FILE_NO),
      //                  new XElement("Member_Ship",
      //                     new XAttribute("strtdate", ""),
      //                     new XAttribute("enddate", ""),
      //                     new XAttribute("prntcont", "1")
      //                  )
      //               )
      //            )
      //         )
      //      );
      //      tb_master.SelectedTab = tp_003;
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Get_Current_Record();
      //         //Execute_Query();
      //         _DefaultGateway.Gateway(
      //            new Job(SendType.External, "localhost", "ADM_TOTL_F", 07 /* Exec LoadData */, SendType.SelfToUserInterface)
      //         );
      //         Set_Current_Record();
      //         requery = false;
      //      }
      //   }
      //}

      private void Btn_Cbmt1_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
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
                              "<Privilege>41</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 11 /* Execute Mstr_Club_F */){ Input = "ADM_TOTL_F" }
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_Dise_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 65 /* Execute CMN_DISE_F */){ Input = "ADM_TOTL_F" }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_RqstDelete3_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انصراف تمدید مشتری مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Rqst = RqstBs3.Current as Data.Request;

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
                           new XAttribute("fileno", Rqst.Fighters.Count > 0 ? Rqst.Fighters.FirstOrDefault().FILE_NO : 0)
                        )
                     )
                  )
               );
               //MessageBox.Show(this, "تمدید مشتری لغو گردید");
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
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs3.List.Count == 0)
                  Btn_RqstExit1_Click(null, null);
               else
                  Create_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstRqt3_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs3.Current as Data.Request;
            rqstindex = RqstBs3.Position;

            StrtDate_DateTime003.CommitChanges();
            EndDate_DateTime003.CommitChanges();

            if (!StrtDate_DateTime003.Value.HasValue) { StrtDate_DateTime003.Value = DateTime.Now; }
            if (!EndDate_DateTime003.Value.HasValue) { EndDate_DateTime003.Value = DateTime.Now.AddDays(29); }

            if (StrtDate_DateTime003.Value.Value.Date > EndDate_DateTime003.Value.Value.Date)
            {
               throw new Exception("تاریخ شروع باید از تاریخ پایان کوچکتر با مساوی باشد");
            }
   
            iScsc.UCC_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "009"),
                     new XAttribute("rqttcode", RQTT_CODE_LookUpEdit3.EditValue),
                     new XElement("Request_Row",
                        new XAttribute("fileno", Figh_Lov.EditValue),
                        new XElement("Fighter",
                           //new XAttribute("mtodcodednrm", MtodCode_LookupEdit003.EditValue ?? ""),
                           new XAttribute("ctgycodednrm", CtgyCode_LookupEdit003.EditValue ?? ""),
                           new XAttribute("cbmtcodednrm", CBMT_CODE_GridLookUpEdit003.EditValue ?? "")
                        ),
                        new XElement("Member_Ship",
                           new XAttribute("strtdate", StrtDate_DateTime003.Value.HasValue ? StrtDate_DateTime003.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("enddate", EndDate_DateTime003.Value.HasValue ? EndDate_DateTime003.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("prntcont", "1"),
                           new XAttribute("numbmontofer", NumbMontOfer_TextEdit003.Text ?? "0"),
                           new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit003.Text ?? "0"),
                           new XAttribute("numbofattnweek", "0"),
                           new XAttribute("attndaytype", "")
                        )
                     )
                  )
               )
            );
            //tabControl1.SelectedTab = tabPage3;
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

      private void RqstBs3_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs3.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101 && (cbmt.Club.REGN_PRVN_CODE + cbmt.Club.REGN_CODE).Contains(Rqst.REGN_PRVN_CODE + Rqst.REGN_CODE))/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
               Gb_Expense3.Visible = true;

               RqstBnDelete3.Enabled = true;
               RqstBnASav3.Enabled = false;

               //Btn_RqstDelete3.Visible = true;
               //Btn_RqstSav3.Visible = false;

               FIGH_FILE_NOLookUpEdit_EditValueChanged(null, null);

               Pn_MbspInfo.Visible = true;

               ReloadSelectedData();

               try
               {
                  UserProFile_Rb.ImageProfile = null;
                  MemoryStream mStream = new MemoryStream();
                  byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO))).ToArray();
                  mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                  Bitmap bm = new Bitmap(mStream, false);
                  mStream.Dispose();

                  //Pb_FighImg.Visible = true;

                  if (InvokeRequired)
                     Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
                  else
                     UserProFile_Rb.ImageProfile = bm;
               }
               catch
               { //Pb_FighImg.Visible = false;
                  UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;               
               }
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101 && (cbmt.Club.REGN_PRVN_CODE + cbmt.Club.REGN_CODE).Contains(Rqst.REGN_PRVN_CODE + Rqst.REGN_CODE))/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
               Gb_Expense3.Visible = false;

               //Btn_RqstDelete3.Visible = Btn_RqstSav3.Visible = true;

               RqstBnDelete3.Enabled = RqstBnASav3.Enabled = true;

               FIGH_FILE_NOLookUpEdit_EditValueChanged(null, null);

               Pn_MbspInfo.Visible = true;

               ReloadSelectedData();

               try
               {
                  UserProFile_Rb.ImageProfile = null;
                  MemoryStream mStream = new MemoryStream();
                  byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO))).ToArray();
                  mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                  Bitmap bm = new Bitmap(mStream, false);
                  mStream.Dispose();

                  //Pb_FighImg.Visible = true;

                  if (InvokeRequired)
                     Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
                  else
                     UserProFile_Rb.ImageProfile = bm;
               }
               catch
               { //Pb_FighImg.Visible = false;
                  UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;               
               }
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense3.Visible = false;

               //Btn_RqstDelete3.Visible = Btn_RqstSav3.Visible = false;

               RqstBnDelete3.Enabled = RqstBnASav3.Enabled = false;

               Pn_MbspInfo.Visible = false;
               DefaultTabPage003();

               UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;               
            }
         }
         catch
         {
            Gb_Expense3.Visible = false;
            //Btn_RqstDelete3.Visible = Btn_RqstSav3.Visible = false;

            RqstBnDelete3.Enabled = RqstBnASav3.Enabled = false;

            Pn_MbspInfo.Visible = false;
            DefaultTabPage003();
         }
      }

      private void Btn_RqstSav3_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs3.Current as Data.Request;
            iScsc.UCC_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID),
                     new XElement("Payment",
                        new XAttribute("setondebt", setOnDebt)
                     )
                  )
               )               
            );
            //tabControl1.SelectedTab = tabPage3;

            // ثبت حضوری به صورت اتوماتیک
            if (SaveAttn_PkBt.PickChecked)
               AutoAttn();

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
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs3.List.Count == 0)
                  Btn_RqstExit1_Click(null, null);
               else
                  Create_Record();
               requery = false;
            }
         }
      }
      LookUpEdit lov_prvn = null;
      //private void REGN_PRVN_CODE_EditValueChanged(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      lov_prvn = sender as LookUpEdit;
      //      //REGN_CODELookUpEdit.EditValue = null;
      //      RegnBs1.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == lov_prvn.EditValue.ToString() && Fga_Urgn_U.Split(',').Contains(r.PRVN_CODE + r.CODE));

      //      REGN_CODELookUpEdit_EditValueChanged(REGN_CODELookUpEdit, e);
      //   }
      //   catch { }
      //}

      LookUpEdit lov_regn;
      private void REGN_CODELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
          try
          {
              lov_regn = sender as LookUpEdit;
              if (lov_regn.EditValue == null || lov_regn.EditValue.ToString().Length != 3) return;
              CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101 && (cbmt.Club.REGN_PRVN_CODE + cbmt.Club.REGN_CODE).Contains(lov_prvn.EditValue.ToString() + lov_regn.EditValue.ToString()))/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
          }
          catch
          {

          }
      }

      private void RqstBnADoc_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   var rqst = RqstBs1.Current as Data.Request;
         //   if (rqst == null) return;

         //   _DefaultGateway.Gateway(
         //      new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
         //   );
         //}
         //if(tb_master.SelectedTab == tp_002)
         //{
         //   var rqst = RqstBs2.Current as Data.Request;
         //   if (rqst == null) return;

         //   _DefaultGateway.Gateway(
         //      new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
         //   );
         //}
         //else if(tb_master.SelectedTab == tp_003)
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
         //if(tb_master.SelectedTab == tp_001)
         //{
         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //            new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //if(tb_master.SelectedTab == tp_002)
         //{
         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //            new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if(tb_master.SelectedTab == tp_003)
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
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstBs1.Current == null) return;
         //   var crnt = RqstBs1.Current as Data.Request;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //if (tb_master.SelectedTab == tp_002)
         //{
         //   if (RqstBs2.Current == null) return;
         //   var crnt = RqstBs2.Current as Data.Request;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedTab == tp_003)
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
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstBs1.Current == null) return;
         //   var crnt = RqstBs1.Current as Data.Request;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //if (tb_master.SelectedTab == tp_002)
         //{
         //   if (RqstBs2.Current == null) return;
         //   var crnt = RqstBs2.Current as Data.Request;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedTab == tp_003)
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
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstBs1.Current == null) return;
         //   var crnt = RqstBs1.Current as Data.Request;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //if (tb_master.SelectedTab == tp_003)
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

      private void bn_PaymentMethods_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   var rqst = RqstBs1.Current as Data.Request;
         //   if (rqst == null) return;
         //   var pymt = PymtsBs1.Current as Data.Payment;

         //   _DefaultGateway.Gateway(
         //      new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) { Input = pymt }
         //   );
         //}
         //if(tb_master.SelectedTab == tp_003)
         {
            var rqst = RqstBs3.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs3.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 86 /* Execute Pay_Mtod_F */){Input = pymt},
                     new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 10 /* Execute Actn_CalF_F*/)
                     {
                        Input = 
                           new XElement("Payment_Method",
                              new XAttribute("callerform", GetType().Name)
                           )
                     }
                  }

               )
            );
            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) { Input = pymt }
            //);
         }
      }

      private void bn_CashPayment_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            //{
            //   if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            //   var rqst = RqstBs1.Current as Data.Request;
            //   if (rqst == null) return;
            //   //var pymt = PymtsBs1.Current as Data.Payment;

            //   /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
            //   {
            //      MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
            //      return;
            //   }*/

            //   foreach (Data.Payment pymt in PymtsBs1)
            //   {
            //      iScsc.PAY_MSAV_P(
            //         new XElement("Payment",
            //            new XAttribute("actntype", "CheckoutWithoutPOS"),
            //            new XElement("Insert",
            //               new XElement("Payment_Method",
            //                  new XAttribute("cashcode", pymt.CASH_CODE),
            //                  new XAttribute("rqstrqid", pymt.RQST_RQID)
            //         //new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
            //               )
            //            )
            //         )
            //      );
            //   }

            //   /* Loop For Print After Pay */
            //   RqstBnPrintAfterPay_Click(null, null);

            //   /* End Request */
            //   Btn_RqstSav1_Click(null, null);
            //}
            //if (tb_master.SelectedTab == tp_003)
            {
               if (MessageBox.Show(this, "عملیات پرداخت به صورت نقدی و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;
               //var pymt = PymtsBs3.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
                  return;
               }*/

               foreach (Data.Payment pymt in PymtsBs3)
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
               Btn_RqstSav3_Click(null, null);
            }
         }catch(SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void Btn_InDebt_Click(object sender, EventArgs e)
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

            //if (tb_master.SelectedTab == tp_001)
            //{
            //   if (MessageBox.Show(this, "عملیات بدهکاری و ذخیره نهایی کردن انجام شود؟", "بدهکاری و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            //   var rqst = RqstBs1.Current as Data.Request;
            //   if (rqst == null) return;
            //   var pymt = PymtsBs1.Current as Data.Payment;

            //   /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
            //   {
            //      MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
            //      return;
            //   }*/

            //   /*iScsc.PAY_MSAV_P(
            //      new XElement("Payment",
            //         new XAttribute("actntype", "CheckoutWithoutPOS"),
            //         new XElement("Insert",
            //            new XElement("Payment_Method",
            //               new XAttribute("cashcode", pymt.CASH_CODE),
            //               new XAttribute("rqstrqid", pymt.RQST_RQID)                  
            //            )
            //         )
            //      )
            //   );*/

            //   /* Loop For Print After Pay */
            //   RqstBnPrintAfterPay_Click(null, null);

            //   /* End Request */
            //   Btn_RqstSav1_Click(null, null);
            //}
            //if (tb_master.SelectedTab == tp_003)
            {
               if (MessageBox.Show(this, "عملیات بدهکاری و ذخیره نهایی کردن انجام شود؟", "بدهکاری و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs3.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
                  return;
               }*/

               /*iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithoutPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID)
                        )
                     )
                  )
               );*/

               /* Loop For Print After Pay */
               //RqstBnPrintAfterPay_Click(null, null);
               
               /* End Request */
               Btn_RqstSav3_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
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

      private void tbn_POSPayment_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_003)
            {
               if (RqstBs3.Current == null) return;
               var rqst = RqstBs3.Current as Data.Request;

               if (MessageBox.Show(this, "عملیات پرداخت توسط کارتخوان و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               if (VPosBs1.List.Count == 0)
                  UsePos_Cb.Checked = false;

               if (UsePos_Cb.Checked)
               {
                  foreach (Data.Payment pymt in PymtsBs3)
                  {
                     var amnt = ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
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
                                             new XAttribute("rqid", pymt.RQST_RQID),
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
                  // 1397/01/07 * ثبت دستی مبلغ به صورت پایانه فروش
                  foreach (Data.Payment pymt in PymtsBs3)
                  {
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "CheckoutWithPOS"),
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
                  Btn_RqstSav3_Click(null, null);
               }
            }

         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         //if(tb_master.SelectedTab == tp_001)
         //{
         //   var rqst = RqstBs1.Current as Data.Request;
         //   if (rqst == null) return;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 94 /* Execute Cmn_Resn_F */){Input = rqst.Request_Rows.FirstOrDefault()},
         //            //new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //if(tb_master.SelectedTab == tp_002)
         //{
         //   var rqst = RqstBs3.Current as Data.Request;
         //   if (rqst == null) return;

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 94 /* Execute Cmn_Resn_F */){Input = rqst.Request_Rows.FirstOrDefault()},
         //            //new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedTab == tp_003)
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

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         //if(tb_master.SelectedTab != tp_003)
         //{
         //   var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").SingleOrDefault();
         //   if (Rg1 == null) return;

         //   _DefaultGateway.Gateway(
         //      new Job(SendType.External, "Localhost",
         //         new List<Job>
         //         {
         //            new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
         //            new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "001")))}
         //         })
         //      );
         //}
         //else if(tb_master.SelectedTab == tp_003)
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
      }

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            //{
            //   FNGR_PRNT_TextEdit.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt64(f.FNGR_PRNT_DNRM)) + 1;
            //}
            //if (tb_master.SelectedTab == tp_002)
            //{
            //   Fngr_Prnt_TextEdit2.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt64(f.FNGR_PRNT_DNRM)) + 1;
            //}
         }
         catch {
            //if (tb_master.SelectedTab == tp_001)
            //{
            //   FNGR_PRNT_TextEdit.EditValue = 1;
            //}
            //if (tb_master.SelectedTab == tp_002)
            //{
            //   Fngr_Prnt_TextEdit2.EditValue = 1;
            //}
         }
      }

      //private void sUNT_BUNT_DEPT_ORGN_CODELookUpEdit_Popup(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString();
      //      DeptBs1.DataSource = iScsc.Departments.Where(d => d.ORGN_CODE == crntorgn);*/
      //      OrgnBs1.Position = SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue);
      //   }
      //   catch
      //   {            
      //   }
      //}

      //private void sUNT_BUNT_DEPT_CODELookUpEdit_Popup(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString(); 
      //      var crntdept = sUNT_BUNT_DEPT_CODELookUpEdit.EditValue.ToString();            
      //      BuntBs1.DataSource = iScsc.Base_Units.Where(b => b.DEPT_CODE == crntdept && b.DEPT_ORGN_CODE == crntorgn);*/
      //      DeptBs1.Position = SUNT_BUNT_DEPT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_CODELookUpEdit.EditValue);
      //   }
      //   catch
      //   {
      //   }
      //}

      //private void sUNT_BUNT_CODELookUpEdit_Popup(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString();
      //      var crntdept = sUNT_BUNT_DEPT_CODELookUpEdit.EditValue.ToString();
      //      var crntbunt = sUNT_BUNT_CODELookUpEdit.EditValue.ToString();
      //      SuntBs1.DataSource = iScsc.Sub_Units.Where(s => s.BUNT_CODE == crntbunt && s.BUNT_DEPT_CODE == crntdept && s.BUNT_DEPT_ORGN_CODE == crntorgn);*/
      //      BuntBs1.Position = SUNT_BUNT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_CODELookUpEdit.EditValue);
      //   }
      //   catch
      //   {
      //   }
      //}

      private void RqstBnADocPicProfile1_Click(object sender, EventArgs e)
      {
         try
         {
            //if(tb_master.SelectedTab == tp_001)
            //{
            //   var rqst = RqstBs1.Current as Data.Request;
            //   if(rqst == null) return;

            //   var result = (
            //            from r in iScsc.Regulations
            //            join rqrq in iScsc.Request_Requesters on r equals rqrq.Regulation
            //            join rqdc in iScsc.Request_Documents on rqrq equals rqdc.Request_Requester                          
            //            join rcdc in iScsc.Receive_Documents on rqdc equals rcdc.Request_Document
            //            where r.TYPE == "001"
            //               && r.REGL_STAT == "002"
            //               && rqrq.RQTP_CODE == rqst.RQTP_CODE
            //               && rqrq.RQTT_CODE == rqst.RQTT_CODE
            //               && rqdc.DCMT_DSID == 13930903120048833 // عکس 4*3
            //               && rcdc.RQRO_RQST_RQID == rqst.RQID
            //               && rcdc.RQRO_RWNO == 1
            //            select rcdc).FirstOrDefault();
            //   if (result == null) return;

            //   _DefaultGateway.Gateway(
            //      new Job(SendType.External, "Localhost",
            //         new List<Job>
            //         {
            //            new Job(SendType.Self,  59 /* Execute Cmn_Dcmt_F */){ Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() },
            //            new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_CalF_F */)
            //            {
            //               Input = 
            //                  new XElement("Action",
            //                     new XAttribute("type", "001"),
            //                     new XAttribute("typedesc", "Force Active Camera Picture Profile"),
            //                     new XElement("Document",
            //                        new XAttribute("rcid", result.RCID)
            //                     )
            //                  )
            //            }
            //         }
            //      ) 
            //   );

            //}
            //if (tb_master.SelectedTab == tp_002)
            //{
            //   var rqst = RqstBs2.Current as Data.Request;
            //   if (rqst == null) return;

            //   var result = (
            //            from r in iScsc.Regulations
            //            join rqrq in iScsc.Request_Requesters on r equals rqrq.Regulation
            //            join rqdc in iScsc.Request_Documents on rqrq equals rqdc.Request_Requester
            //            join rcdc in iScsc.Receive_Documents on rqdc equals rcdc.Request_Document
            //            where r.TYPE == "001"
            //               && r.REGL_STAT == "002"
            //               && rqrq.RQTP_CODE == rqst.RQTP_CODE
            //               && rqrq.RQTT_CODE == rqst.RQTT_CODE
            //               && rqdc.DCMT_DSID == 13930903120048833 // عکس 4*3
            //               && rcdc.RQRO_RQST_RQID == rqst.RQID
            //               && rcdc.RQRO_RWNO == 1
            //            select rcdc).FirstOrDefault();
            //   if (result == null) return;

            //   _DefaultGateway.Gateway(
            //      new Job(SendType.External, "Localhost",
            //         new List<Job>
            //         {
            //            new Job(SendType.Self,  59 /* Execute Cmn_Dcmt_F */){ Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() },
            //            new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_CalF_F */)
            //            {
            //               Input = 
            //                  new XElement("Action",
            //                     new XAttribute("type", "001"),
            //                     new XAttribute("typedesc", "Force Active Camera Picture Profile"),
            //                     new XElement("Document",
            //                        new XAttribute("rcid", result.RCID)
            //                     )
            //                  )
            //            }
            //         }
            //      )
            //   );

            //}
            //else if (tb_master.SelectedTab == tp_003)
            {
               var rqst = RqstBs3.Current as Data.Request;
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

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {            
            //if (tb_master.SelectedTab == tp_001)
            //{
            //   //var rqst = RqstBs1.Current as Data.Request;
            //   //if (rqst == null) return;

            //   long mtodcode = (long)MtodCode_LookupEdit001.EditValue;
            //   long ctgycode = (long)CtgyCode_LookupEdit001.EditValue;
            //   string rqttcode = (string)RQTT_CODE_LookUpEdit1.EditValue;
            //   var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "001" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && exp.MTOD_CODE == mtodcode && exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();
               
            //   StrtDate_DateTime001.Value = DateTime.Now;
            //   //if (MessageBox.Show(this, "تعداد جلسات با احتساب یک روز در میان می باشد؟", "مشخص شدن تاریخ پایان", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //   //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(2*(expn.NUMB_OF_ATTN_MONT - 1)));
            //   //else
            //   //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_OF_ATTN_MONT));
            //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
            //   NumbOfAttnMont_TextEdit001.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;
            //   NumbOfAttnWeek_TextEdit001.EditValue = expn.NUMB_OF_ATTN_WEEK ?? 0;
            //   NumbMontOfer_TextEdit001.EditValue = expn.NUMB_MONT_OFER ?? 0;
            //}
            //if (tb_master.SelectedTab == tp_003)
            {
               //var rqst = RqstBs3.Current as Data.Request;
               //if (rqst == null) return;

               long ctgycode = (long)CtgyCode_LookupEdit003.EditValue;
               string rqttcode = (string)RQTT_CODE_LookUpEdit3.EditValue;
               var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "009" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

               StrtDate_DateTime003.Value = DateTime.Now;
               //if(MessageBox.Show(this, "تعداد جلسات با احتساب یک روز در میان می باشد؟", "مشخص شدن تاریخ پایان", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               //   EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(2 * (expn.NUMB_OF_ATTN_MONT - 1)));
               //else
               //   EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(expn.NUMB_OF_ATTN_MONT));
               EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               NumbOfAttnMont_TextEdit003.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;
               NumbMontOfer_TextEdit003.EditValue = expn.NUMB_MONT_OFER ?? 0;
            }
         }
         catch (Exception )
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }

      }

      private void Pblc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = Figh_Lov.EditValue;
            if (fileno == null) return;

            _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
                  );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CopyDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            EndDate_DateTime003.Value = StrtDate_DateTime003.Value;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void CrntDate_Butn_Click(object sender, EventArgs e)
      {
         var strtdate = StrtDate_DateTime003.Value;
         if (strtdate != null && MessageBox.Show(this, "آیا تاریخ شروع را میخواهید اصلاح کنید", "اصلاح تاریخ شروع", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         StrtDate_DateTime003.Value = DateTime.Now;
      }

      private void IncDecMont_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var strtdate = StrtDate_DateTime003.Value;
         if (strtdate == null) StrtDate_DateTime003.Value = DateTime.Now;

         var enddate = EndDate_DateTime003.Value;
         if (enddate == null) EndDate_DateTime003.Value = StrtDate_DateTime003.Value;

         switch (e.Button.Index)
         {
            case 1:
               EndDate_DateTime003.Value = EndDate_DateTime003.Value.Value.AddDays(30);
               break;
            case 0:
               EndDate_DateTime003.Value = EndDate_DateTime003.Value.Value.AddDays(-30);
               break;
         }
      }

      private void FIGH_FILE_NOLookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if(Figh_Lov.EditValue.ToString() == "")return;

            var figh = FighBs3.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == Convert.ToInt64(Figh_Lov.EditValue));//iScsc.Fighters.First(f => f.FIGH_FILE_NO == Convert.ToInt64(FIGH_FILE_NOLookUpEdit.EditValue));

            CBMT_CODE_GridLookUpEdit003.EditValue = figh.CBMT_CODE_DNRM;
            CtgyCode_LookupEdit003.EditValue = figh.CTGY_CODE_DNRM;
         }
         catch 
         {
         }
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void Cbmt003_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long code = (long)CBMT_CODE_GridLookUpEdit003.EditValue;

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

      private void AutoAttn()
      {
         try
         {
            var figh = (RqroBs3.Current as Data.Request_Row).Fighter;

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
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ReloadSelectedData()
      {
         // 1396/08/18 * برای بروز رسانی اطلاعات جدید مشترک * سبک و رسته و کلاس
         var rqst = RqstBs3.Current as Data.Request;
         if (rqst == null) return;

         var figh = rqst.Request_Rows.FirstOrDefault().Fighter;
         //MtodCode_LookupEdit003.EditValue = figh.MTOD_CODE_DNRM;
         CtgyCode_LookupEdit003.EditValue = figh.CTGY_CODE_DNRM;
         //CtgyBs2.Position = CtgyBs2.List.OfType<Data.Category_Belt>().ToList().FindIndex(c => c.CODE == figh.CTGY_CODE_DNRM);//CtgyCode_LookupEdit003.Properties.GetDataSourceRowIndex(CtgyCode_LookupEdit003.Properties.ValueMember, CtgyCode_LookupEdit003.EditValue);
         CBMT_CODE_GridLookUpEdit003.EditValue = figh.CBMT_CODE_DNRM;
         FNGR_PRNT_TextEdit.EditValue = figh.FNGR_PRNT_DNRM;
      }

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }

      private void PydsType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydsType_Butn.Text = PydsType_Butn.Tag.ToString() == "0" ? "مبلغی" : "درصدی";
            PydsType_Butn.Tag = PydsType_Butn.Tag.ToString() == "0" ? "1" : "0";
            if (PydsType_Lov.EditValue != null || PydsType_Lov.EditValue.ToString() != "") PydsType_Lov.EditValue = "002";

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

      private void RcmtType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RcmtType_Butn.Text = RcmtType_Butn.Tag.ToString() == "0" ? "POS" : "نقدی";
            RcmtType_Butn.Tag = RcmtType_Butn.Tag.ToString() == "0" ? "1" : "0";
            PymtAmnt_Txt.Focus();
            var pymt = PymtsBs3.Current as Data.Payment;
            if (pymt == null) return;
            PymtAmnt_Txt.EditValue = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
         }
         catch { }
      }

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs3.Current as Data.Payment;
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

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text);

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
            var pyds = PydsBs3.Current as Data.Payment_Discount;
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

      private void SavePymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PymtDate_DateTime001.CommitChanges();
            var pymt = PymtsBs3.Current as Data.Payment;
            if (pymt == null) return;

            if (PymtAmnt_Txt.EditValue == null || PymtAmnt_Txt.EditValue.ToString() == "" || Convert.ToInt64(PymtAmnt_Txt.EditValue) == 0) return;

            switch (RcmtType_Butn.Tag.ToString())
            {
               case "0":
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                           )
                        )
                     )
                  );
                  break;
               case "1":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked)
                  {
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
                        PymtAmnt_Txt.EditValue = Convert.ToInt64(PymtAmnt_Txt.EditValue) * 10;

                     // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
                     UsePos_Cb.Checked = false;

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
                                             new XAttribute("rqid", pymt.RQST_RQID),
                                             new XAttribute("rqtpcode", ""),
                                             new XAttribute("router", GetType().Name),
                                             new XAttribute("callback", 20),
                                             new XAttribute("amnt", Convert.ToInt64( PymtAmnt_Txt.EditValue) )
                                          )
                                    }
                                 }
                              )
                           }
                        )
                     );

                     UsePos_Cb.Checked = true;
                  }
                  else
                  {
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "InsertUpdate"),
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", pymt.CASH_CODE),
                                 new XAttribute("rqstrqid", pymt.RQST_RQID),
                                 new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                                 new XAttribute("rcptmtod", "003"),
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                              )
                           )
                        )
                     );
                  }
                  break;
               default:
                  break;
            }

            PymtAmnt_Txt.EditValue = null;
            PymtDate_DateTime001.Value = DateTime.Now;
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

      private void DeltPymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pmmt = PmmtBs3.Current as Data.Payment_Method;
            if (pmmt == null) return;

            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "Delete"),
                  new XAttribute("cashcode", pmmt.PYMT_CASH_CODE),
                  new XAttribute("rqstrqid", pmmt.PYMT_RQST_RQID),
                  new XAttribute("rwno", pmmt.RWNO)
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

      private void CBMT_CODE_GridLookUpEdit003_Popup(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CBMT_CODE_GridLookUpEdit003.EditValue;

            if (cbmt == null || cbmt.ToString() == "") return;

            var crntcbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(c => c.CODE == (long)cbmt);

            CtgyBs2.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == crntcbmt.MTOD_CODE && c.CTGY_STAT == "002");
         }
         catch (Exception)
         {

         }
      }

      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch (Exception exc) { }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }

   }
}
