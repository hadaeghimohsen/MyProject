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

namespace System.Scsc.Ui.OtherIncome
{
   public partial class ADM_BRSR_F : UserControl
   {
      public ADM_BRSR_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private int rqstindex = default(int);

      private void Execute_Query()
      {
         setOnDebt = false;
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               GC.Collect();
               GC.WaitForPendingFinalizers();
               iScsc = new Data.iScscDataContext(ConnectionString);
               if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.RUN_QURY == "002"))                  
                  vf_FighBs.DataSource = iScsc.VF_Last_Info_Fighter(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE);

               rqstindex = RqstBs1.Position;

               var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
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
            }
         }
         catch { }
      }

      private void Execute_Query_Force()
      {
         setOnDebt = false;
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               GC.Collect();
               GC.WaitForPendingFinalizers();
               iScsc = new Data.iScscDataContext(ConnectionString);
               //if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.RUN_QURY == "002"))
               vf_FighBs.DataSource = iScsc.VF_Last_Info_Fighter(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE);

               rqstindex = RqstBs1.Position;

               var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
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
            }
         }
         catch { }
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
            }
            else if (Rqst.RQID == 0)
            {
               //Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               //DefaultTabPage001();
               RqstBnASav1.Enabled = false;
            }
         }
         catch
         {
            //Gb_Expense.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            //DefaultTabPage001();
            RqstBnASav1.Enabled = false;
         }
      }

      private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      {
         try
         {           
            var Rqst = RqstBs1.Current as Data.Request;
            rqstindex = RqstBs1.Position;

            if (FngrPrnt_Txt.Text == "")
            {
               if (MessageBox.Show(this, "کد شناسایی خالی میباشد آیا مایل به ایجاد کد پیش فرض هستید؟", "هشدار کد شناسایی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                  MaxF_Butn001_Click(null, null);
               else
               {
                  FngrPrnt_Txt.Focus();
                  return;
               }
            }

            if (Rqst == null || Rqst.RQID >= 0)
            {
               FgpbsBs1.EndEdit();

               if (Rqst == null || Rqst.RQST_STAT == null || Rqst.RQST_STAT == "001")
                  iScsc.BYR_TRQT_P(
                        new XElement("Process",
                           new XElement("Request",
                              new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                              new XAttribute("rqtpcode", "025"),
                              new XAttribute("rqttcode", "004"),
                              new XAttribute("prvncode", "017"),
                              new XAttribute("regncode", "001"),
                              new XElement("Fighter",
                                 new XAttribute("fileno", Rqst == null ? 0 : Rqst.Fighters.FirstOrDefault() == null ? 0 : Rqst.Fighters.FirstOrDefault().FILE_NO),
                                 new XElement("Frst_Name", FrstName_Txt.Text.Trim()),
                                 new XElement("Last_Name", LastName_Txt.Text.Trim()),
                                 new XElement("Fath_Name", ""),
                                 new XElement("Sex_Type", SexType_Lov.EditValue),
                                 new XElement("Natl_Code", (NatlCode_Txt.EditValue ?? "").ToString().Trim()),
                                 new XElement("Brth_Date", BrthDate_Dt.Value == null ? "" : BrthDate_Dt.Value.Value.ToString("yyyy-MM-dd")),
                                 new XElement("Cell_Phon", CellPhon_Txt.Text.Trim()),
                                 new XElement("Tell_Phon", TellPhon_Txt.Text.Trim()),
                                 new XElement("Type", "001"),
                                 new XElement("Post_Adrs", (PostAdrs_Txt.EditValue ?? "").ToString().Trim()),
                                 new XElement("Emal_Adrs", ""),
                                 new XElement("Insr_Numb", (InsrNumb_Txt.Text ?? "").ToString().Trim()),
                                 new XElement("Insr_Date", InsrDate_Dt.Value == null ? "" : InsrDate_Dt.Value.Value.ToString("yyyy-MM-dd")),
                                 new XElement("Educ_Deg", ""),
                                 new XElement("Club_Code", ClubCode_Lov.EditValue ?? ""),
                                 new XElement("Dise_Code", ""),
                                 new XElement("Blod_Grop", ""),
                                 new XElement("Fngr_Prnt", (FngrPrnt_Txt.EditValue ?? "").ToString().Trim()),
                                 new XElement("Sunt_Bunt_Dept_Orgn_Code", ""),
                                 new XElement("Sunt_Bunt_Dept_Code", ""),
                                 new XElement("Sunt_Bunt_Code", ""),
                                 new XElement("Sunt_Code", SuntCode_Lov.EditValue ?? ""),
                                 new XElement("Cord_X", ""),
                                 new XElement("Cord_Y", ""),
                                 new XElement("Mtod_Code", ""),
                                 new XElement("Ctgy_Code",  ""),
                                 new XElement("Most_Debt_Clng", ""),
                                 new XElement("Serv_No", (ServNo_Txt.EditValue ?? "").ToString().Trim()),
                                 new XElement("Chat_Id", (TelgCode_Txt.EditValue ?? "").ToString().Trim()),
                                 new XElement("Ref_Code", (RefCode_Lov.EditValue ?? "").ToString().Trim())
                              ),
                              new XElement("Member_Ship",
                                 new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                                 new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
                                 //new XAttribute("numbmontofer", NumbMontOfer_TextEdit001.Text ?? "0"),
                                 //new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit001.Text ?? "0"),
                                 //new XAttribute("numbofattnweek", NumbOfAttnWeek_TextEdit001.Text ?? "0"),
                                 //new XAttribute("attndaytype", "7")
                              )
                           )
                        )
                     );
               else if (Rqst.RQST_STAT == "002") return;
               //MessageBox.Show(this, "مشتریی جدید در سیستم ثبت گردید");
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
               /*
                * Requery Data From Database
                */
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
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
               requery = false;
            }
         }
      }

      bool setOnDebt = false;
      private void Btn_RqstSav1_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            if (Rqst != null && Rqst.RQST_STAT == "001")
            {
               iScsc.BYR_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                        new XAttribute("regncode", Rqst.REGN_CODE),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );
               requery = true;
               //tc_pblc.SelectedTab = tp_pblcinfo;
               
               if(!LockAftrSave_Cbx.Checked)
                  PBLC.FindFilterText = "";

               // 1399/12/09
               if (GoProfile_Pbt.PickChecked)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)) }
                  );
               }

               // 1397/05/26 * اگر درخواست گزینه های جانبی داشته باشد باید شماره پرونده ها رو به فرم های مربوطه ارسال کنیم
               string followups = "";
               if (OthrExpnInfo_Ckbx.Checked)
                  followups += "OIC_TOTL_F;";
               if (ChargeCredit_Ckbx.Checked)
                  followups += "GLR_INDC_F;";
               
               #region 3th
               if (OthrExpnInfo_Ckbx.Checked)
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                           new List<Job>
                           {                  
                              new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                              new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Request", 
                                       new XAttribute("type", "01"), 
                                       new XElement("Request_Row", 
                                          new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)),
                                       new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                       new XAttribute("rqstrqid", Rqst.RQID),
                                       new XAttribute("formcaller", GetType().Name)
                                    )
                              }
                           })
                  );
               else if (ChargeCredit_Ckbx.Checked)
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                           new List<Job>
                           {                  
                              new Job(SendType.Self, 153 /* Execute Glr_Indc_F */),
                              new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 10 /* Execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Request", 
                                       new XAttribute("type", "newrequest"), 
                                       new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO),
                                       new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                       new XAttribute("rqstrqid", Rqst.RQID),
                                       new XAttribute("formcaller", GetType().Name)
                                    )
                              }
                           })
                  );
               #endregion

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
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
               //Create_Record();
               requery = false;
               // 1400/12/14
               // vf_FighBs.DataSource = iScsc.VF_Last_Info_Fighter(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE);//.Where(f => Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Fga_Uclb_U.Contains(f.CLUB_CODE));
            }
         }
      }

      private void Btn_RqstExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            
         }
         catch (Exception )
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }
      }

      private void Btn_Dise_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 65 /* Execute CMN_DISE_F */){ Input = GetType().Name }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      
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

      private void sUNT_BUNT_DEPT_ORGN_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString();
            DeptBs1.DataSource = iScsc.Departments.Where(d => d.ORGN_CODE == crntorgn);*/
            //OrgnBs1.Position = SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void sUNT_BUNT_DEPT_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString(); 
            var crntdept = sUNT_BUNT_DEPT_CODELookUpEdit.EditValue.ToString();            
            BuntBs1.DataSource = iScsc.Base_Units.Where(b => b.DEPT_CODE == crntdept && b.DEPT_ORGN_CODE == crntorgn);*/
            //DeptBs1.Position = SUNT_BUNT_DEPT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void sUNT_BUNT_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString();
            var crntdept = sUNT_BUNT_DEPT_CODELookUpEdit.EditValue.ToString();
            var crntbunt = sUNT_BUNT_CODELookUpEdit.EditValue.ToString();
            SuntBs1.DataSource = iScsc.Sub_Units.Where(s => s.BUNT_CODE == crntbunt && s.BUNT_DEPT_CODE == crntdept && s.BUNT_DEPT_ORGN_CODE == crntorgn);*/
            //BuntBs1.Position = SUNT_BUNT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void BTN_MBSP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         
      }

      private void CopyDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //EndDate_DateTime001.Value = StrtDate_DateTime001.Value;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void CrntDate_Butn_Click(object sender, EventArgs e)
      {
         
      }

      private void IncDecMont_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {

      }

      private void vF_Last_Info_FighterResultBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         var fileno = (vf_FighBs.Current as Data.VF_Last_Info_FighterResult).FILE_NO;
         //try
         //{
         //   UserProFile_Rb.ImageProfile = null;
         //   MemoryStream mStream = new MemoryStream();
         //   byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
         //   mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
         //   Bitmap bm = new Bitmap(mStream, false);
         //   mStream.Dispose();

         //   UserProFile_Rb.Visible = true;

         //   if (InvokeRequired)
         //      Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
         //   else
         //      UserProFile_Rb.ImageProfile = bm;
         //}
         //catch { UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482; }
      }

      private void UserProFile_Rb_Click(object sender, EventArgs e)
      {
         try
         {
            var CrntFigh = vf_FighBs.Current as Data.VF_Last_Info_FighterResult;
            if (CrntFigh == null) return;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", CrntFigh.FILE_NO)) }
            );
         }
         catch { }
      }

      private void OthrInCm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var CrntFigh = vf_FighBs.Current as Data.VF_Last_Info_FighterResult;
            if (CrntFigh == null) return;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                  
                        new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", CrntFigh.FILE_NO)))}
                     })
            );
         }
         catch { }
      }

      private void colActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var figh = vf_FighBs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                          new List<Job>
                           {                  
                              new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                              new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                           })
                  );
                  break;
               case 1:                  
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 153 /* Execute Glr_Indc_F */),
                           new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("Request", 
                                    new XAttribute("type", "newrequest"), 
                                    new XAttribute("fileno", figh.FILE_NO),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        })
                  );
                  break;
               case 2:
                  if (MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "عملیات حذف موقت مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
                           new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 07 /* Execute Load_Data */){Input = new XElement("LoadData", new XAttribute("requery", "1"))},
                        })
                  );
                  break;
               case 3:         
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
                        })
                  );
                  break;
               case 4:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
                  );
                  break;               
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveOthr_Butn_Click(object sender, EventArgs e)
      {
         if(FngrPrnt_Txt.Text != "")
         {
            FrstName_Txt.Text = LastName_Txt.Text = FngrPrnt_Txt.Text;

            Btn_RqstRqt1_Click(null, null);
         }
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query_Force();
      }

      private void AdvnAdmnServ_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", FngrPrnt_Txt.Text))}
               })
         );
      }

      private void BaleCodeGnrt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (TelgCode_Txt.Text == "") return;

            if(FngrPrnt_Txt.Text == "")
            {
               TelgCode_Txt.Text = "B" + TelgCode_Txt.Text;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            FngrPrnt_Txt.EditValue =
                iScsc.Fighters
                .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                .Select(f => f.FNGR_PRNT_DNRM)
                .ToList()
                .Where(f => f.All(char.IsDigit))
                .Max(f => Convert.ToInt64(f)) + 1;
         }
         catch
         {
            FngrPrnt_Txt.EditValue = 1;
         }
      }

      private void CELL_PHON_TextEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if(SrchType_Cbx.Checked)
               PBLC.FindFilterText = CellPhon_Txt.Text;
            // اگر تعداد کاراکتر از 11 تا کمتر باشد هیچ گونه مقایسه ای انجام نشود
            #region Comment
            //if (e.NewValue.ToString().Length < 11) return;

            //var _qury = vf_FighBs.List.OfType<Data.VF_Last_Info_FighterResult>().Where(f => f.CELL_PHON_DNRM != null && f.CELL_PHON_DNRM.Contains(e.NewValue.ToString()));
            
            //// 1400/05/03 * اگر خروجی بدست آماده فقط یک کاربر داشته باشد 
            //if(_qury != null && _qury.Count() == 1)
            //{
            //   var _rslt = 
            //      string.Format(
            //         "براساس ورودی شماره موبایل توسط شما یک مشخصه کاربری پیدا شده که به شرح زیر میباشد" + "\n" + 
            //         "{0} با شماره موبایل {1} و کد شناسایی {2} در حال حاضر قابل دسترس میباشد، آیا مایل به جای گذاری کد شناسایی جدید میباشید؟",
            //         _qury.FirstOrDefault().NAME_DNRM, _qury.FirstOrDefault().CELL_PHON_DNRM, _qury.FirstOrDefault().FNGR_PRNT_DNRM
            //      );
            //   if (MessageBox.Show(this, _rslt, "جایگزینی کد شناسایی برای مشتری", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //   //CELL_PHON_TextEdit.EditValue = _qury.FirstOrDefault().CELL_PHON_DNRM;
            //   //Btn_RqstRqt1_Click(null, null);

            //   #region Store Fngr Prnt
            //   iScsc.BYR_TRQT_P(
            //      new XElement("Process",
            //         new XElement("Request",
            //            new XAttribute("rqid", 0),
            //            new XAttribute("rqtpcode", "025"),
            //            new XAttribute("rqttcode", "004"),
            //            new XAttribute("prvncode", "017"),
            //            new XAttribute("regncode", "001"),
            //            new XElement("Fighter",
            //               new XAttribute("fileno", _qury.FirstOrDefault().FILE_NO),
            //               new XElement("Cell_Phon", _qury.FirstOrDefault().CELL_PHON_DNRM),
            //               new XElement("Fngr_Prnt", FngrPrnt_Txt.EditValue ?? "")
            //            ),
            //            new XElement("Member_Ship",
            //               new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
            //               new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
            //            )
            //         )
            //      )
            //   );
            //   #endregion

            //   // 1399/12/09
            //   if (GoProfile_Pbt.PickChecked)
            //   {
            //      _DefaultGateway.Gateway(
            //         new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _qury.FirstOrDefault().FILE_NO)) }
            //      );
            //   }

            //   // 1397/05/26 * اگر درخواست گزینه های جانبی داشته باشد باید شماره پرونده ها رو به فرم های مربوطه ارسال کنیم
            //   string followups = "";
            //   if (OthrExpnInfo_Ckbx.Checked)
            //      followups += "OIC_TOTL_F;";
            //   if (ChargeCredit_Ckbx.Checked)
            //      followups += "GLR_INDC_F;";


            //   #region 3th
            //   if (OthrExpnInfo_Ckbx.Checked)
            //      _DefaultGateway.Gateway(
            //         new Job(SendType.External, "Localhost",
            //               new List<Job>
            //               {                  
            //                  new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
            //                  new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */)
            //                  {
            //                     Input = 
            //                        new XElement("Request", 
            //                           new XAttribute("type", "01"), 
            //                           new XElement("Request_Row", 
            //                              new XAttribute("fileno", _qury.FirstOrDefault().FILE_NO)),
            //                           new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
            //                           //new XAttribute("rqstrqid", ""),
            //                           new XAttribute("formcaller", GetType().Name)
            //                        )
            //                  }
            //               })
            //      );
            //   else if (ChargeCredit_Ckbx.Checked)
            //      _DefaultGateway.Gateway(
            //         new Job(SendType.External, "Localhost",
            //               new List<Job>
            //               {                  
            //                  new Job(SendType.Self, 153 /* Execute Glr_Indc_F */),
            //                  new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 10 /* Execute Actn_CalF_F */)
            //                  {
            //                     Input = 
            //                        new XElement("Request", 
            //                           new XAttribute("type", "newrequest"), 
            //                           new XAttribute("fileno", _qury.FirstOrDefault().FILE_NO),
            //                           new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
            //                           //new XAttribute("rqstrqid", ""),
            //                           new XAttribute("formcaller", GetType().Name)
            //                        )
            //                  }
            //               })
            //      );
            //#endregion
            //}
            #endregion

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CELL_PHON_TextEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (CellPhon_Txt.EditValue == null || CellPhon_Txt.Text == "") { CellPhon_Txt.Focus(); return; }

            var _qury =
               iScsc.Fighters
                  .Where(
                     f => f.CELL_PHON_DNRM.Contains(CellPhon_Txt.Text) &&
                          f.CONF_STAT == "002" &&
                          (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) ||
                              (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&
                          Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101
                  );

            if (_qury.Count() > 0)
            {
               PBLC.FindFilterText = CellPhon_Txt.Text;
               //throw new Exception("این شماره همراه قبلا درون سیستم ثبت شده است، لطفا لیست خود را چک کنید");
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NatlCode_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (NatlCode_Txt.EditValue == null || NatlCode_Txt.Text == "") { NatlCode_Txt.Focus(); return; }

            var _qury =
               iScsc.Fighters
                  .Where(
                     f => f.NATL_CODE_DNRM.Contains(NatlCode_Txt.Text) &&
                          f.CONF_STAT == "002" &&
                          (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) ||
                              (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&
                          Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101
                  );

            if (_qury.Count() > 0)
            {
               PBLC.FindFilterText = NatlCode_Txt.Text;
               //throw new Exception("این شماره کد ملی قبلا درون سیستم ثبت شده است، لطفا لیست خود را چک کنید");
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NatlCode_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if(SrchType_Cbx.Checked)
               PBLC.FindFilterText = NatlCode_Txt.Text;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SUNT_CODELookUpEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {

      }

      private void SuntCode_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:                  
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
                                       "<Privilege>171</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       #region Show Error
                                       MessageBox.Show("خطا: عدم دسترسی به کد 171");
                                       #endregion                           
                                    })
                                 },
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>175</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       #region Show Error
                                       MessageBox.Show("خطا: عدم دسترسی به کد 175");
                                       #endregion                           
                                    })
                                 }
                                 #endregion
                              }),
                           #region DoWork
                           new Job(SendType.Self, 108 /* Execute Orgn_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 10 /* Actn_CalF_P */)
                           #endregion
                           })
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CellPhonRefCode_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            //var _rqst = RqstBs1.Current as Data.Request;
            //if (_rqst == null) return;

            RServBs.DataSource = iScsc.Fighters.Where(s => s.CELL_PHON_DNRM.Contains(CellPhonRefCode_Txt.Text));
            if (RServBs.List.Count == 1)
            {
               RefCode_Lov.EditValue = RServBs.List.OfType<Data.Fighter>().FirstOrDefault().FILE_NO;
               CellPhonRefCode_Txt.EditValue = null;
            }
            else if (RServBs.List.Count > 1)
            {
               RefCode_Lov.Focus();
               RefCode_Lov.ShowPopup();
            }
            else
            {
               RefCode_Lov.EditValue = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RefCode_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            var _fgpb = FgpbsBs1.Current as Data.Fighter_Public;
            if (_fgpb == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  _fgpb.REF_CODE = null;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

   }
}
