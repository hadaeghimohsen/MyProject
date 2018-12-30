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
               iScsc = new Data.iScscDataContext(ConnectionString);

               rqstindex = RqstBs1.Position;

               var Rqids = iScsc.VF_Requests(new XElement("Request"))
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
                                 new XElement("Frst_Name", FRST_NAME_TextEdit.Text),
                                 new XElement("Last_Name", LAST_NAME_TextEdit.Text),
                                 new XElement("Fath_Name", ""),
                                 new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue),
                                 new XElement("Natl_Code", NatlCode_Txt.EditValue ?? ""),
                                 new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                                 new XElement("Cell_Phon", CELL_PHON_TextEdit.Text),
                                 new XElement("Tell_Phon", TELL_PHON_TextEdit.Text),
                                 new XElement("Type", "001"),
                                 new XElement("Post_Adrs", ""),
                                 new XElement("Emal_Adrs", ""),
                                 new XElement("Insr_Numb", ""),
                                 new XElement("Insr_Date", ""),
                                 new XElement("Educ_Deg", ""),
                                 new XElement("Club_Code", Club_CodeLookUpEdit.EditValue ?? ""),
                                 new XElement("Dise_Code", ""),
                                 new XElement("Blod_Grop", ""),
                                 new XElement("Fngr_Prnt", ""),
                                 new XElement("Sunt_Bunt_Dept_Orgn_Code", ""),
                                 new XElement("Sunt_Bunt_Dept_Code", ""),
                                 new XElement("Sunt_Bunt_Code", ""),
                                 new XElement("Sunt_Code", ""),
                                 new XElement("Cord_X", ""),
                                 new XElement("Cord_Y", ""),
                                 new XElement("Mtod_Code", MtodCode_LookupEdit001.EditValue ?? ""),
                                 new XElement("Ctgy_Code", CtgyCode_LookupEdit001.EditValue ?? ""),
                                 new XElement("Most_Debt_Clng", ""),
                                 new XElement("Serv_No", ""),
                                 new XElement("Chat_Id", TelgCode_Txt.EditValue ?? "")
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
            if (MessageBox.Show(this, "آیا با انصراف و حذف ثبت نام مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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
               vf_FighBs.DataSource = iScsc.VF_Last_Info_Fighter(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE);//.Where(f => Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Fga_Uclb_U.Contains(f.CLUB_CODE));
            }
         }
      }

      private void Btn_RqstExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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

      private void MtodCode_LookupEdit001_Popup(object sender, EventArgs e)
      {
         try
         {
            var mtod = MtodCode_LookupEdit001.EditValue;

            if (mtod == null || mtod.ToString() == "") return;            

            CtgyBs1.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long)mtod);
         }
         catch (Exception )
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

      private void Butn_TakePicture_Click(object sender, EventArgs e)
      {
         try
         {
            if (true)
            {
               var fileno = (vf_FighBs.Current as Data.VF_Last_Info_FighterResult).FILE_NO;
               var rqst = (from r in iScsc.Requests
                           join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                           where rr.FIGH_FILE_NO == Convert.ToInt64(fileno)
                              && (r.RQTP_CODE == "001" || r.RQTP_CODE == "025")
                           select r).FirstOrDefault();
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

      private void vF_Last_Info_FighterResultBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         var fileno = (vf_FighBs.Current as Data.VF_Last_Info_FighterResult).FILE_NO;
         try
         {
            UserProFile_Rb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            UserProFile_Rb.Visible = true;

            if (InvokeRequired)
               Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
            else
               UserProFile_Rb.ImageProfile = bm;
         }
         catch { UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482; }
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

   }
}
