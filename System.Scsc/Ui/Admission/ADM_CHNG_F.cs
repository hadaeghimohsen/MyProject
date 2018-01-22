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
using DevExpress.XtraGrid.Views.Grid;

namespace System.Scsc.Ui.Admission
{
   public partial class ADM_CHNG_F : UserControl
   {
      public ADM_CHNG_F()
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
                     rqst.RQTP_CODE == "002" &&
                     rqst.RQTT_CODE == "004" &&
                     rqst.RQST_STAT == "001" &&
                     (ShowRqst_PickButn.PickChecked ? rqst.CRET_BY == CurrentUser : true) &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               )
               .OrderByDescending(
                     rqst =>
                        rqst.RQST_DATE
                  );

            // 1396/11/02 * بدست آوردن شماره پرونده های درگیر در تمدید
            FighsBs1.DataSource = iScsc.Fighters.Where(f => Rqids.Contains((long)f.RQST_RQID));
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
      }

      private void Set_Current_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstIndex >= 0)
               RqstBs1.Position = RqstIndex;
         }         
      }

      private void Create_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            RqstBs1.AddNew();
            FILE_NO_LookUpEdit.Focus();
         }         
      }

      private void LL_MoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         Pn_MoreInfo.Visible = !Pn_MoreInfo.Visible;
         LL_MoreInfo.Text = Pn_MoreInfo.Visible ? "- کمتر ( F3 )" : "+ بیشتر ( F3 )";
         if (Pn_MoreInfo.Visible)
         {
             Gb_Info.Height = 577;
            //Gb_Expense.Top = 320;
         }
         else
         {
            Gb_Info.Height = 228;
            //Gb_Expense.Top = 170;
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               //Gb_Expense.Visible = true;
               Btn_RqstDelete1.Visible = true;
               Btn_RqstSav1.Visible = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               //Gb_Expense.Visible = false;
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
            }
            else if (Rqst.RQID == 0)
            {
               //Gb_Expense.Visible = false;
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            }
         }
         catch
         {
            //Gb_Expense.Visible = false;
            Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
         }
      }

      private void Btn_RqstBnRqt_Click(object sender, EventArgs e)
      {
         try
         {
            if (FILE_NO_LookUpEdit.EditValue == null || Convert.ToInt64(FILE_NO_LookUpEdit.EditValue) == 0) return;
            
            var Rqst = RqstBs1.Current as Data.Request;
            var Figh = FighBs1.Current as Scsc.Data.Fighter;

            if (Rqst == null || Rqst.RQID >= 0)
            {
               iScsc.PBL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                        new XAttribute("rqtpcode", "002"),
                        new XAttribute("rqttcode", "004"),
                        new XElement("Request_Row",
                           new XAttribute("fileno", FILE_NO_LookUpEdit.EditValue),
                           new XElement("Fighter_Public",
                              new XElement("Type", TYPE_LookUpEdit.EditValue ?? ""),
                              new XElement("Frst_Name", FRST_NAME_TextEdit.Text ?? ""),
                              new XElement("Last_Name", LAST_NAME_TextEdit.Text ?? ""),
                              new XElement("Fath_Name", FATH_NAME_TextEdit.Text ?? ""),
                              new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue ?? ""),
                              new XElement("Natl_Code", NATL_CODE_TextEdit.Text ?? ""),
                              new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                              new XElement("Cell_Phon", CELL_PHON_TextEdit.Text ?? ""),
                              new XElement("Tell_Phon", TELL_PHON_TextEdit.Text ?? ""),                           
                              new XElement("Post_Adrs", POST_ADRS_TextEdit.Text ?? ""),
                              new XElement("Emal_Adrs", EMAL_ADRS_TextEdit.Text ?? ""),
                              new XElement("Insr_Numb", iNSR_NUMBTextEdit.Text ?? ""),
                              new XElement("Insr_Date", iNSR_DATEPersianDateEdit.Value == null ? "" : iNSR_DATEPersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                              new XElement("Educ_Deg", EDUC_DEG_LookUpEdit.EditValue ?? ""),
                              new XElement("Cbmt_Code", CBMT_CODE_GridLookUpEdit.EditValue ?? ""),
                              new XElement("Dise_Code", DISE_CODE_LookUpEdit.EditValue ?? ""),
                              new XElement("Calc_Expn_Type", CALC_EXPN_TYPE_LookUpEdit.EditValue ?? ""),
                              //new XElement("Mtod_Code", MTOD_CODE_LookUpEdit.EditValue),
                              //new XElement("Ctgy_Code", CTGY_CODE_LookUpEdit.EditValue),
                              new XElement("Coch_Deg", COCH_DEG_LookUpEdit.EditValue ?? ""),
                              new XElement("Coch_Crtf_Date", COCH_CRTF_DATE_PersianDateEdit.Value == null ? "" : COCH_CRTF_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                              new XElement("Gudg_Deg", GUDG_DEG_LookUpEdit.EditValue ?? ""),
                              new XElement("Glob_Code", GLOB_CODE_TextEdit.EditValue ?? ""),
                              new XElement("Blod_Grop", BLOD_GROPLookUpEdit.EditValue ?? ""),
                              new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.EditValue ?? ""),
                              new XElement("Sunt_Bunt_Dept_Orgn_Code", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
                              new XElement("Sunt_Bunt_Dept_Code", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
                              new XElement("Sunt_Bunt_Code", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
                              new XElement("Sunt_Code", SUNT_CODELookUpEdit.EditValue ?? ""),
                              new XElement("Cord_X", CORD_XTextEdit.EditValue ?? ""),
                              new XElement("Cord_Y", CORD_YTextEdit.EditValue ?? ""),
                              new XElement("Most_Debt_Clng", SE_MostDebtClngAmnt.Value),
                              new XElement("Serv_No", ServNo_Text.EditValue ?? ""),
                              new XElement("Brth_Plac", BrthPlac_TextEdit.EditValue ?? ""),
                              new XElement("Issu_Plac", IssuPlac_TextEdit.EditValue ?? ""),
                              new XElement("Fath_Work", FathWork_TextEdit.EditValue ?? ""),
                              new XElement("Hist_Desc", HistDesc_TextEdit.EditValue ?? ""),
                              new XElement("Intr_File_No", INTR_FILE_NOLookUpEdit.EditValue ?? ""),
                              new XElement("Dpst_Acnt_Slry_Bank", DpstAcntSlryBank_Text2.EditValue ?? ""),
                              new XElement("Dpst_Acnt_Slry", DpstAcntSlry_Text2.EditValue ?? "")
                           )
                        )
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
               /*
                * Requery Data From Database
                */
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstDelete_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انصراف و حذف ثبت نام مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst != null && Rqst.RQID > 0)
            {
               /*
                *  Remove Data From Tables
                */
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
               FighBs1.List.Clear();
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
               Create_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstSav_Click(object sender, EventArgs e)
      {         
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            if (Rqst != null && Rqst.RQST_STAT == "001")
            {
               iScsc.PBL_SAVE_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                        new XAttribute("regncode", Rqst.REGN_CODE),
                        new XElement("Request_Row",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );
               requery = true;

               // Save Card In Device
               if (CardNumb_Text.Text != "")
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
                     {
                        Input =
                        new XElement("User",
                           new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text),
                           new XAttribute("cardnumb", CardNumb_Text.Text),
                           new XAttribute("namednrm", FRST_NAME_TextEdit.Text + ", " + LAST_NAME_TextEdit.Text)
                        )
                     }
                  );

               CardNumb_Text.Text = "";
            }
         }
         catch (Exception ex)
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
               Create_Record();
               requery = false;
               FighBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002").OrderBy(f => f.FGPB_TYPE_DNRM);
            }            
         }
      }

      private void Btn_RqstExit_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name , 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

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
                  new Job(SendType.Self, 11 /* Execute Mstr_Club_F */){ Input = this.GetType().Name }
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
                  new Job(SendType.Self, 65 /* Execute CMN_DISE_F */){ Input = this.GetType().Name }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
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
      }

      private void CBMT_CODEGridLookUpEditView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "TIME_DESC" && e.IsGetData)
         {
            var h = ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "END_TIME")).Hours;
            e.Value = h >= 0 && h < 12 ? "صبح" : h >= 12 && h < 15 ? "ظهر" : h >= 15 && h < 18 ? "بعد ظهر" : h >= 18 ? "عصر" : "نام مشخص";            
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "002")))}
                  })
               );
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

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               var rqst = RqstBs1.Current as Data.Request;

               if (rqst == null) return;

               if (rqst.Request_Rows.First().Fighter_Publics.FirstOrDefault(fp => fp.RECT_CODE == "001").FNGR_PRNT != ""
                && MessageBox.Show(this, "آیا می خواهید کد انگشتی هنرجو تغییر دهید؟", "تغییر کد انگشتی هنرجو", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes
               ) return;
               FNGR_PRNT_TextEdit.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt64(f.FNGR_PRNT_DNRM)) + 1;
            }
         }
         catch
         {
            if (tb_master.SelectedTab == tp_001)
            {
               FNGR_PRNT_TextEdit.EditValue = 1;
            }
         }
      }

      private void INTR_FILE_NOLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 2) // بارگذاری لیست جدید
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
               return;
            }
            else if (e.Button.Index == 3) // تعریف کاربر جدید
            {
               Job _InteractWithScsc =
                   new Job(SendType.External, "Localhost",
                      new List<Job>
                         {
                            new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                            new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                         });
               _DefaultGateway.Gateway(_InteractWithScsc);
               return;
            }

            if (INTR_FILE_NOLookUpEdit.EditValue.ToString() == "") return;

            var fileno = Convert.ToInt64(INTR_FILE_NOLookUpEdit.EditValue);

            switch (e.Button.Index)
            {
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void FNGR_PRNT_TextEdit_TextChanged(object sender, EventArgs e)
      {
         if (AutoTrans_Cb.Checked)
            CardNumb_Text.Text = FNGR_PRNT_TextEdit.Text;
      }
   }
}
