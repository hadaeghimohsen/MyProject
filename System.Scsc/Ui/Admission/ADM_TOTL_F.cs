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
using System.Scsc.ExtCode;

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
      private long? cbmtcode = null, ctgycode = null;

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
         RqttCode_Lov.EditValue = "001";
         MemberShip_Gbx.Visible = MbspInfo_Gbx.Visible = false;
      }

      private void DefaultTabPage002()
      {
         /* تنظیم کردن ناحیه و استان قابل دسترس */
         
      }

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
         if(!GustSaveRqst_PickButn.PickChecked)
            RqstBs3.AddNew();            

         RqttCode_Lov.Focus();

      }

      bool setOnDebt = false;

      private void Btn_RqstExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "ADM_TOTL_F", 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

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

            if (Rqst != null && Rqst.RQID != 0 && NewFngrPrnt_Cb.Checked && NewFngrPrnt_Txt.Text == "")
            {
               throw new Exception("کد شناسایی جدید وارد نشده");
            }

            // 1400/04/25 * بررسی اینکه جنسیت مشتری در کلاس ثبت نامی درست میباشد یا خیر
            if (Rqst != null && Rqst.RQID != 0)
            {
               if (CbmtCode_Lov.EditValue == null || !CbmtBs1.List.OfType<Data.Club_Method>().Any(c => c.CODE == (long)CbmtCode_Lov.EditValue)) return;
               if (FighBs3.List.OfType<Data.Fighter>().Any(f => f.FILE_NO == (long)(Figh_Lov.EditValue)))
               {
                  if (CbmtBs1.List.OfType<Data.Club_Method>().Any(c => c.CODE == (long)CbmtCode_Lov.EditValue && c.SEX_TYPE != "003" && c.SEX_TYPE != FighBs3.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)(Figh_Lov.EditValue)).SEX_TYPE_DNRM))
                  {
                     if (MessageBox.Show(this, "جنسیت مشتری در گروه ثبت نامی قابل قبول نمیباشد، آیا با ثبت مشتری موافق هستید؟ در غیر اینصورت اطلاعات را اصلاح فرمایید", "عدم تطابق جنسیت در گروه ثبت نامی", MessageBoxButtons.YesNo) != DialogResult.Yes) { CbmtCode_Lov.Focus(); return; }
                  }
               }
               else
               {
                  if (CbmtBs1.List.OfType<Data.Club_Method>().Any(c => c.CODE == (long)CbmtCode_Lov.EditValue && c.SEX_TYPE != "003" && c.SEX_TYPE != iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == (long)(Figh_Lov.EditValue)).SEX_TYPE_DNRM))
                  {
                     if (MessageBox.Show(this, "جنسیت مشتری در گروه ثبت نامی قابل قبول نمیباشد، آیا با ثبت مشتری موافق هستید؟ در غیر اینصورت اطلاعات را اصلاح فرمایید", "عدم تطابق جنسیت در گروه ثبت نامی", MessageBoxButtons.YesNo) != DialogResult.Yes) { CbmtCode_Lov.Focus(); return; }
                  }
               }

               // 1400/06/08 * بررسی اینکه تعداد جلسات به نرخ تعرفه درست انتخاب شده یا خیر
               if (!CtgyBs2.List.OfType<Data.Category_Belt>().Any(c => c.CODE == Convert.ToInt64(CtgyCode_Lov.EditValue) && c.NUMB_OF_ATTN_MONT == Convert.ToInt32(NumbOfAttnMont_TextEdit003.Text)) &&
                  MessageBox.Show(this, "اطلاعات ورودی با اطلاعات آیین نامه مغایرت دارد، آیا نیاز به اصلاح کردن اطلاعات را دارید؟", "مغایرت اطلاعات آیین نامه با اطلاعات ورودی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
               {
                  NumbOfAttnMont_TextEdit003.Focus();
                  return;
               }

               // 1401/05/20 * بررسی اینکه آیا منشی رشته تکراری دارد ذخیره میکند
               if (MbspBs.List.OfType<Data.Member_Ship>()
                  .Any(m => m.VALD_TYPE == "002" &&                     
                     DateTime.Now.Date.IsBetween(m.STRT_DATE.Value.Date, m.END_DATE.Value.Date) &&
                     (m.NUMB_OF_ATTN_MONT == 0 || m.SUM_ATTN_MONT_DNRM < m.NUMB_OF_ATTN_MONT) &&
                     m.Fighter_Public.CTGY_CODE == Convert.ToInt64(CtgyCode_Lov.EditValue)
                  )
                  &&
                  MessageBox.Show(this, "اطلاعات دوره جدید تکراری میباشد" + Environment.NewLine + "در ردیف دوره های فعال مشتری با مشخصات ثبت شده توسط شما [دوره تکراری وجود دارد]، آیا مایل به اصلاح اطلاعات هستید؟", "وجود اطلاعات تکراری", MessageBoxButtons.YesNo) == DialogResult.Yes)
               {
                  CtgyCode_Lov.Focus();
                  return;
               }
            }

            iScsc.UCC_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "009"),
                     new XAttribute("rqttcode", RqttCode_Lov.EditValue),
                     new XElement("Request_Row",
                        new XAttribute("fileno", Figh_Lov.EditValue),
                        new XElement("Fighter",
                           //new XAttribute("mtodcodednrm", MtodCode_LookupEdit003.EditValue ?? ""),
                           new XAttribute("ctgycodednrm", CtgyCode_Lov.EditValue ?? ""),
                           new XAttribute("cbmtcodednrm", CbmtCode_Lov.EditValue ?? "")
                        ),
                        new XElement("Member_Ship",
                           new XAttribute("strtdate", StrtDate_DateTime003.Value.HasValue ? StrtDate_DateTime003.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("enddate", EndDate_DateTime003.Value.HasValue ? EndDate_DateTime003.Value.Value.ToString("yyyy-MM-dd") : ""),
                           new XAttribute("prntcont", "1"),
                           new XAttribute("numbmontofer", NumbMontOfer_TextEdit003.Text ?? "0"),
                           new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit003.Text ?? "0"),
                           new XAttribute("numbofattnweek", "0"),
                           new XAttribute("attndaytype", ""),
                           new XAttribute("newfngrprnt", NewFngrPrnt_Cb.Checked ? NewFngrPrnt_Txt.Text : "")
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

            // 1401/05/21 * فعال سازی فیلتر برای عدم نمایش اطلاعات کد تخفیف سوخته مشتریان
            Fgdc_Gv.ActiveFilterString = "STAT = '002'";

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101 && (cbmt.Club.REGN_PRVN_CODE + cbmt.Club.REGN_CODE).Contains(Rqst.REGN_PRVN_CODE + Rqst.REGN_CODE))/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
               Gb_Expense3.Visible = true;
               MemberShip_Gbx.Visible = true;
               MbspInfo_Gbx.Visible = true;

               RqstBnDelete3.Enabled = true;
               RqstBnASav3.Enabled = false;

               //Btn_RqstDelete3.Visible = true;
               //Btn_RqstSav3.Visible = false;

               FIGH_FILE_NOLookUpEdit_EditValueChanged(null, null);

               MbspInfo_Gbx.Visible = true;

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

               // 1397/12/18 * نمایش اطلاعات دوره های قبلی مشتری
               MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101 && (cbmt.Club.REGN_PRVN_CODE + cbmt.Club.REGN_CODE).Contains(Rqst.REGN_PRVN_CODE + Rqst.REGN_CODE))/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
               Gb_Expense3.Visible = false;
               MemberShip_Gbx.Visible = true;
               MbspInfo_Gbx.Visible = true;

               //Btn_RqstDelete3.Visible = Btn_RqstSav3.Visible = true;

               RqstBnDelete3.Enabled = RqstBnASav3.Enabled = true;

               FIGH_FILE_NOLookUpEdit_EditValueChanged(null, null);

               MbspInfo_Gbx.Visible = true;

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

               // 1397/12/18 * نمایش اطلاعات دوره های قبلی مشتری
               MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense3.Visible = false;
               MemberShip_Gbx.Visible = false;
               MbspInfo_Gbx.Visible = false;

               //Btn_RqstDelete3.Visible = Btn_RqstSav3.Visible = false;

               RqstBnDelete3.Enabled = RqstBnASav3.Enabled = false;

               MbspInfo_Gbx.Visible = false;
               DefaultTabPage003();

               UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
               FNGR_PRNT_TextEdit.Text = "";
               //DPST_AMNT_Txt.EditValue = "";
               //DEBT_AMNT_Txt.EditValue = "";
               SexFltr_Pkb.Visible = false;
            }
         }
         catch
         {
            Gb_Expense3.Visible = false;
            MemberShip_Gbx.Visible = false;
            //Btn_RqstDelete3.Visible = Btn_RqstSav3.Visible = false;
            MbspInfo_Gbx.Visible = false;

            RqstBnDelete3.Enabled = RqstBnASav3.Enabled = false;

            MbspInfo_Gbx.Visible = false;
            DefaultTabPage003();

            UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
            FNGR_PRNT_TextEdit.Text = "";
            //DPST_AMNT_Txt.EditValue = "";
            //DEBT_AMNT_Txt.EditValue = "";
            SexFltr_Pkb.Visible = false;
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

            NewFngrPrnt_Cb.Checked = false;

            // 1398/05/10 * ثبت پکیج کلاس ها به صورت گروهی
            //if (GustSaveRqst_PickButn.PickChecked)
            if(GustSaveRqst_Cbx.Checked)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                        {
                           // new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", FNGR_PRNT_TextEdit.Text), new XAttribute("formcaller", GetType().Name))}
                        })
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
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
         }
      }

      private void bn_CashPayment_Click(object sender, EventArgs e)
      {
         try
         {            
            {
               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs3.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> نقدی << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }
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

      private void bn_DpstPayment_Click(object sender, EventArgs e)
      {
         try
         {            
            var rqst = RqstBs3.Current as Data.Request;
            if (rqst == null) return;

            bool finalAction = false;
            long? debtamnt = 0, dpstamnt = 0;

            var pymt = PymtsBs3.Current as Data.Payment;
            if (pymt == null) return;

            debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
            dpstamnt = pymt.Request.Request_Rows.FirstOrDefault().Fighter.DPST_AMNT_DNRM;

            if (dpstamnt == 0) { MessageBox.Show(this, "مبلغ سپرده مشتری صفر میباشد", "عدم موجودی سپرده مشتری"); return; }
            // 1401/02/04 * بروزرسانی مبلغ سپرده مشتری
            if (dpstamnt - pymt.Payment_Methods.Where(pm => pm.RCPT_MTOD == "005").Sum(pm => pm.AMNT) <= 0)
            {
               MessageBox.Show(this, "مبلغ اعتبار سپرده برای مشتری وجود ندارد", "عدم موجودی سپرده مشتری"); return;
            }
            else
            {
               dpstamnt -= pymt.Payment_Methods.Where(pm => pm.RCPT_MTOD == "005").Sum(pm => pm.AMNT);
            }

            finalAction = debtamnt <= dpstamnt;
            debtamnt = dpstamnt < debtamnt ? dpstamnt : debtamnt;

            if (Accept_Cb.Checked)
            {
               string mesg = "";
               if (debtamnt > 0)
               {
                  mesg =
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> کسر از سپرده << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                        string.Format("{0:n0}", debtamnt),
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                        "امروز",
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }

            foreach (Data.Payment pymt1 in PymtsBs3)
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithDeposit"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt1.CASH_CODE),
                           new XAttribute("rqstrqid", pymt1.RQST_RQID),
                           new XAttribute("amnt", debtamnt)
                        )
                     )
                  )
               );
            }

            if (finalAction)
            {
               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstSav3_Click(null, null);
            }
            else
               requery = true;            
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
         finally
         {
            if(requery)
            {
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               //if (RqstBs3.List.Count == 0)
               //   Btn_RqstExit1_Click(null, null);
               //else
               //   Create_Record();
               requery = false;
            }
         }
      }

      private void bn_Card2CardPayment_Click(object sender, EventArgs e)
      {
         try
         {
            {
               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs3.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> کارت به کارت << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }

               foreach (Data.Payment pymt in PymtsBs3)
               {
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithCard2Card"),
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
         catch (SqlException se)
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
            
            {
               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs3.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> بدهکار << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  else
                     setOnDebt = false;

                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }

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
               var rqst = RqstBs3.Current as Data.Request;
               if (rqst == null) return;

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs3.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> کارتخوان << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }

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

               long ctgycode = (long)CtgyCode_Lov.EditValue;
               string rqttcode = (string)RqttCode_Lov.EditValue;
               var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "009" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

               //StrtDate_DateTime003.Value = DateTime.Now;
               //if(MessageBox.Show(this, "تعداد جلسات با احتساب یک روز در میان می باشد؟", "مشخص شدن تاریخ پایان", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               //   EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(2 * (expn.NUMB_OF_ATTN_MONT - 1)));
               //else
               //   EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(expn.NUMB_OF_ATTN_MONT));
               if (ModifierKeys == Keys.Control)
               {
                  // تاریخ پایان بر اساس تاریخ شروعی که وارد شده محاسبه گردد
                  StrtDate_DateTime003.CommitChanges();
                  var strtdate = StrtDate_DateTime003.Value;
                  if (strtdate.HasValue)
                     EndDate_DateTime003.Value = strtdate.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
                  else
                  {
                     StrtDate_DateTime003.Value = DateTime.Now;
                     EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
                  }
               }
               else if (ModifierKeys == Keys.Shift)
               {
                  // تاریخ شروع به اولین روز همان ماه برگردد
                  StrtDate_DateTime003.CommitChanges();
                  var strtdate = StrtDate_DateTime003.Value;
                  if (strtdate.HasValue)
                  {
                     var day = StrtDate_DateTime003.GetText("dd").ToInt32();
                     if (day != 1)
                        StrtDate_DateTime003.Value = StrtDate_DateTime003.Value.Value.AddDays((day - 1) * -1);
                     EndDate_DateTime003.Value = StrtDate_DateTime003.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
                  }
                  else
                  {
                     StrtDate_DateTime003.Value = DateTime.Now;
                     var day = StrtDate_DateTime003.GetText("dd").ToInt32();
                     if (day != 1)
                        StrtDate_DateTime003.Value = StrtDate_DateTime003.Value.Value.AddDays((day - 1) * -1);
                     EndDate_DateTime003.Value = StrtDate_DateTime003.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
                  }
               }
               else
               {
                  StrtDate_DateTime003.Value = DateTime.Now;
                  EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
               //EndDate_DateTime003.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               NumbOfAttnMont_TextEdit003.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;
               NumbMontOfer_TextEdit003.EditValue = expn.NUMB_MONT_OFER ?? 0;

               // Set Price on Label
               DfltPric_Lb.Text = string.Format("{0:n0} {1} *** {2}", expn.PRIC, DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(a => a.VALU == expn.Expense_Type.Request_Requester.Regulation.AMNT_TYPE).DOMN_DESC, expn.Method.MTOD_DESC);
               DfltPric_Lb.Tag = expn.PRIC;
               TotlPic_Lb.Tag = DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(a => a.VALU == expn.Expense_Type.Request_Requester.Regulation.AMNT_TYPE).DOMN_DESC;
               IncAttnPric_Nud.Value = expn.NUMB_OF_ATTN_MONT ?? 0;

               // 1401/05/22 * اگر ظرفیت کلاسی پر شده باشد به منشی اعلام میکنیم
               if (CapacityCycle_Lb.Tag != null && Convert.ToInt64(CapacityCycle_Lb.Tag) <= 0 && MessageBox.Show(this, "ظرفیت ثبت نام گروه انتخابی پر شده، آیا مایل به این هستید که گروه دیگری را انتخاب کنید؟", "محدودیت ظرفیت ثبت نام", MessageBoxButtons.YesNo) == DialogResult.Yes) { CbmtCode_Lov.Focus(); return; }

               Btn_RqstRqt3_Click(null, null);
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
            if (fileno == null || fileno.ToString() == "") return;

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

            CbmtCode_Lov.EditValue = figh.CBMT_CODE_DNRM;
            CtgyCode_Lov.EditValue = figh.CTGY_CODE_DNRM;
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
            long code = (long)CbmtCode_Lov.EditValue;

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
         CtgyCode_Lov.EditValue = figh.CTGY_CODE_DNRM;
         //CtgyBs2.Position = CtgyBs2.List.OfType<Data.Category_Belt>().ToList().FindIndex(c => c.CODE == figh.CTGY_CODE_DNRM);//CtgyCode_LookupEdit003.Properties.GetDataSourceRowIndex(CtgyCode_LookupEdit003.Properties.ValueMember, CtgyCode_LookupEdit003.EditValue);
         CbmtCode_Lov.EditValue = figh.CBMT_CODE_DNRM;
         FNGR_PRNT_TextEdit.EditValue = figh.FNGR_PRNT_DNRM;
         //DPST_AMNT_Txt.EditValue = figh.DPST_AMNT_DNRM;
         //DEBT_AMNT_Txt.EditValue = figh.DEBT_DNRM;
         SexFltr_Pkb.Visible = true;
         SexFltr_Pkb.ImageIndexPickDown = SexFltr_Pkb.ImageIndexPickUp = figh.SEX_TYPE_DNRM == "001" ? 4 : 5;
         SexFltr_Pkb_PickCheckedChange(null);

         NoteBs.DataSource =
            iScsc.Notes.Where(n => n.FIGH_FILE_NO == figh.FILE_NO);
         FGrpBs.DataSource =
            iScsc.Fighter_Groupings.Where(g => g.GROP_STAT == "002" && g.FIGH_FILE_NO == figh.FILE_NO);
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

            long? amnt = null;
            switch (PydsType_Butn.Tag.ToString())
            {
               case "0":
                  if (!(Convert.ToInt32(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt32(PydsAmnt_Txt.EditValue) <= 100))
                  {
                     PydsAmnt_Txt.EditValue = null;
                     PydsAmnt_Txt.Focus();
                  }

                  amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt64(PydsAmnt_Txt.EditValue)) / 100;
                  break;
               case "1":
                  amnt = Convert.ToInt32(PydsAmnt_Txt.EditValue);
                  if (amnt == 0) return;
                  break;
            }

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text, null, PydsDesc_Txt.Tag == null ? null : (long?)PydsDesc_Txt.Tag);

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

            #region Old Payment Operation
            //switch (RcmtType_Butn.Tag.ToString())
            //{
            //   case "0":
            //      iScsc.PAY_MSAV_P(
            //         new XElement("Payment",
            //            new XAttribute("actntype", "InsertUpdate"),
            //            new XElement("Insert",
            //               new XElement("Payment_Method",
            //                  new XAttribute("cashcode", pymt.CASH_CODE),
            //                  new XAttribute("rqstrqid", pymt.RQST_RQID),
            //                  new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
            //                  new XAttribute("rcptmtod", "001"),
            //                  new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
            //               )
            //            )
            //         )
            //      );
            //      break;
            //   case "1":
            //      if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

            //      if (UsePos_Cb.Checked)
            //      {
            //         var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            //         long psid;
            //         if (Pos_Lov.EditValue == null)
            //         {
            //            var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
            //            if (posdflts.Count() == 1)
            //               Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
            //            else
            //            {
            //               Pos_Lov.Focus();
            //               return;
            //            }
            //         }
            //         else
            //         {
            //            psid = (long)Pos_Lov.EditValue;
            //         }

            //         if (regl.AMNT_TYPE == "002")
            //            PymtAmnt_Txt.EditValue = Convert.ToInt64(PymtAmnt_Txt.EditValue) * 10;

            //         // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
            //         UsePos_Cb.Checked = false;

            //         _DefaultGateway.Gateway(
            //            new Job(SendType.External, "localhost",
            //               new List<Job>
            //               {
            //                  new Job(SendType.External, "Commons",
            //                     new List<Job>
            //                     {
            //                        new Job(SendType.Self, 34 /* Execute PosPayment */)
            //                        {
            //                           Input = 
            //                              new XElement("PosRequest",
            //                                 new XAttribute("psid", psid),
            //                                 new XAttribute("subsys", 5),
            //                                 new XAttribute("rqid", pymt.RQST_RQID),
            //                                 new XAttribute("rqtpcode", ""),
            //                                 new XAttribute("router", GetType().Name),
            //                                 new XAttribute("callback", 20),
            //                                 new XAttribute("amnt", Convert.ToInt64( PymtAmnt_Txt.EditValue) )
            //                              )
            //                        }
            //                     }
            //                  )
            //               }
            //            )
            //         );

            //         UsePos_Cb.Checked = true;
            //      }
            //      else
            //      {
            //         iScsc.PAY_MSAV_P(
            //            new XElement("Payment",
            //               new XAttribute("actntype", "InsertUpdate"),
            //               new XElement("Insert",
            //                  new XElement("Payment_Method",
            //                     new XAttribute("cashcode", pymt.CASH_CODE),
            //                     new XAttribute("rqstrqid", pymt.RQST_RQID),
            //                     new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
            //                     new XAttribute("rcptmtod", "003"),
            //                     new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
            //                  )
            //               )
            //            )
            //         );
            //      }
            //      break;
            //   default:
            //      break;
            //}
            #endregion

            switch ((RcmtType_Lov.EditValue ?? "001").ToString())
            {               
               case "003":
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
                                             new XAttribute("amnt", Convert.ToInt64( PymtAmnt_Txt.EditValue)),
                                             new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                                             new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                                             new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? "")
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
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                                 new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                                 new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                                 new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? "")
                              )
                           )
                        )
                     );
                  }
                  break;
               default:
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", RcmtType_Lov.EditValue ?? "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                              new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                              new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                              new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? "")
                           )
                        )
                     )
                  );
                  break;
            }

            PymtAmnt_Txt.EditValue = null;
            PymtDate_DateTime001.Value = DateTime.Now;
            Rtoa_Lov.EditValue = null;
            FlowNo_Txt.EditValue = null;
            RcptFilePath_Txt.EditValue = null;
            RcmtType_Lov.Focus();
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
            var cbmt = CbmtCode_Lov.EditValue;

            if (cbmt == null || cbmt.ToString() == "") return;

            var crntcbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(c => c.CODE == (long)cbmt);

            CtgyBs2.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == crntcbmt.MTOD_CODE && c.CTGY_STAT == "002");

            var cbmtt = iScsc.Club_Methods.First(cm => cm.CODE == (long)CbmtCode_Lov.EditValue);
            if (cbmtt == null) return;

            var cmwk = cbmtt.Club_Method_Weekdays.ToList();

            if (cmwk.Count == 0)
            {
               ClubWkdy_Pn.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }

            foreach (var wkdy in cmwk)
            {
               var rslt = ClubWkdy_Pn.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
               rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
            }

            // 1401/05/22 * امروز تولد ملودی هست که من بهش تبریک گفتم
            // ملودی عزیزم همیشه شاد خوشحال باشی، چون قشنگی دنیای من بسته به لبخندهای شیرین تو هست عزیز دلم
            // بررسی ظرفیت کلاسی
            if (cbmtt.CPCT_STAT == "002")
            {
               var listMbsp =
                  iScsc.Member_Ships
                  .Where(ms =>
                     ms.RECT_CODE == "004" &&
                     ms.VALD_TYPE == "002" &&
                     ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                     ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                     (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                     ms.Fighter_Public.CBMT_CODE == cbmtt.CODE
                  );
               CapacityCycle_Lb.Text = string.Format("ک.ظ :" + " {0} " + "ب.ظ :" + "{1}", cbmtt.CPCT_NUMB, (cbmtt.CPCT_NUMB - listMbsp.Count()));
               CapacityCycle_Lb.Tag = cbmtt.CPCT_NUMB - listMbsp.Count();

               if (cbmtt.CPCT_NUMB > listMbsp.Count())
                  CapacityCycle_Lb.ForeColor = Color.Black;
               else if (cbmtt.CPCT_NUMB < listMbsp.Count())
                  CapacityCycle_Lb.ForeColor = Color.Red;
               else
                  CapacityCycle_Lb.ForeColor = Color.Green;
            }
            else
            {
               CapacityCycle_Lb.Text = "نامحدود";
               CapacityCycle_Lb.Tag = null;
               CapacityCycle_Lb.ForeColor = Color.Green;
            }
         }
         catch { }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                     {                  
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
                     });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
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
      #endregion

      private void CbmtCode_Lov_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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
                        new Job(SendType.SelfToUserInterface,"BAS_CBMT_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("formcaller", GetType().Name))}
                     }
                  )
               );
            }
         }
         catch { }
      }

      private void MbspBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;            

            long? rqid = 0;
            if (mbsp.RWNO == 1)
               rqid = mbsp.Request_Row.Request.Request1.RQID;
            else
               rqid = mbsp.RQRO_RQST_RQID;

            MbspValdType_Butn.Text = mbsp.VALD_TYPE == "001" ? "فعال کردن" : "غیرفعال کردن";

            ExpnAmnt_Txt.EditValue = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => (pd.EXPN_PRIC + pd.EXPN_EXTR_PRCT) * pd.QNTY);
            DscnAmnt_Txt.EditValue = iScsc.Payment_Discounts.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => pd.AMNT);
            PymtAmnt1_Txt.EditValue = iScsc.Payment_Methods.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => pd.AMNT);
         }
         catch
         {
         }
      }

      private void NewFngrPrnt_Cb_CheckedChanged(object sender, EventArgs e)
      {
         NewFngrPrnt_Txt.Visible = NewFngrPrnt_Cb.Checked;
      }

      private void SexFltr_Pkb_PickCheckedChange(object sender)
      {
         try
         {
            var rqst = RqstBs3.Current as Data.Request;
            if(rqst == null || rqst.RQID == 0)return;

            var figh = rqst.Request_Rows.FirstOrDefault().Fighter;

            Cbmt_Gv.ActiveFilterString = SexFltr_Pkb.PickChecked ? string.Format("[Sex_Type] = '{0}' OR [Sex_Type] = '003'", figh.SEX_TYPE_DNRM) : "";
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void IncDec_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstBs3.Current as Data.Request;
            if (rqst == null) return;

            var pydt = PydtsBs3.Current as Data.Payment_Detail;

            switch (e.Button.Index)
            {
               case 0:
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET QNTY += 1 WHERE PYMT_RQST_RQID = {0};", rqst.RQID));
                  requery = true;
                  break;
               case 1:
                  if (pydt.QNTY > 1)
                  {
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET QNTY -= 1 WHERE PYMT_RQST_RQID = {0};", rqst.RQID));
                     requery = true;
                  }
                  break;
            }
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

      private void IncAttnPric_Nud_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstBs3.Current as Data.Request;
            if (rqst == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  if (IncAttnPric_Nud.Value > 0)
                  {
                     NumbOfAttnMont_TextEdit003.EditValue = IncAttnPric_Nud.EditValue;
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET QNTY = {1} WHERE PYMT_RQST_RQID = {0}; UPDATE dbo.Member_Ship SET NUMB_OF_ATTN_MONT = {1} WHERE Rqro_Rqst_Rqid = {0};", rqst.RQID, IncAttnPric_Nud.Value));
                     TotlPic_Lb.Text = string.Format("{0:n0} {1}", Convert.ToInt64(DfltPric_Lb.Tag) * IncAttnPric_Nud.Value, TotlPic_Lb.Tag);

                     requery = true;
                  }
                  break;
               default:
                  break;
            }
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

      private void IncAttnPric_Nud_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var rqst = RqstBs3.Current as Data.Request;
            if (rqst == null) return;

            if (Convert.ToInt64(e.NewValue) > 0)
            {
               TotlPic_Lb.Visible = true;
               NumbOfAttnMont_TextEdit003.EditValue = e.NewValue;
               TotlPic_Lb.Text = string.Format("{0:n0} {1}", Convert.ToInt64(DfltPric_Lb.Tag) * Convert.ToInt64(e.NewValue), TotlPic_Lb.Tag);
            }            
            else
            {
               TotlPic_Lb.Visible = false;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RcmtType_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         RcmtType_Butn_Click(null, null);
      }

      private void NumAttn_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         NumAttn2QntyExpn_Pn.Visible = NumAttn_Cbx.Checked;
      }

      private void Advc_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs3.Current as Data.Request;
            if (_rqst == null || _rqst.RQID == 0) return;

            var _fgdc = FgdcBs3.Current as Data.Fighter_Discount_Card;
            if (_fgdc == null) return;

            if (_fgdc.RQST_RQID != null) { MessageBox.Show(this, "این رکورد کد تخفیف قبلا درون درخواست ثبت شده است", "عدم ثبت کد تخفیف"); return; }
            if (_fgdc.CTGY_CODE != null && _rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault().CTGY_CODE != _fgdc.CTGY_CODE)
            {
               MessageBox.Show(this, "کاربر گرامی این کد تخفیف برای نرخ مورد نظر شما قابل استفاده نمی باشد", "عدم استفاده از کد تخفیف");
               return;
            }

            switch (_fgdc.DSCT_TYPE)
            {
               case "001":
                  // %
                  // اگر دکمه عملیات تخفیف گذاری غیر محتوای درصدی باشد
                  if (PydsType_Butn.Tag.ToString() != "0") { PydsType_Butn_Click(null, null); }
                  break;
               case "002":
                  // $
                  // اگر دکمه عملیات تخفیف گذاری غیر محتوای مبلغی باشد
                  if (PydsType_Butn.Tag.ToString() != "1") { PydsType_Butn_Click(null, null); }
                  break;
               default:
                  break;
            }

            PydsAmnt_Txt.EditValue = _fgdc.DSCT_AMNT;
            PydsDesc_Txt.Text = string.Format("کد تخفیف " + "( {0} )" + " بابت : " + "( {1} )" + " توسط کاربر : " + "( {2} )" + " ذخیره شد.", _fgdc.DISC_CODE, _fgdc.DSCT_DESC, CurrentUser);
            PydsDesc_Txt.Tag = _fgdc.CODE;
            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Discount_Card SET RQST_RQID = {0} WHERE CODE = {1};", _rqst.RQID, _fgdc.CODE));
            SavePyds_Butn_Click(null, null);
            PydsDesc_Txt.Tag = null;
            PymtOprt_Tc.SelectedTab = PymtDsct_Tp;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {

      }

      private void Adm_Tc_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs3.Current as Data.Request;
            if (_rqst == null) return;

            switch (Adm_Tc.SelectedTab.Name)
            {
               case "tp_005":
                  var _mbsp = MbspBs.Current as Data.Member_Ship;
                  if (_mbsp == null) return;

                  AttnBs2.DataSource = _mbsp.Attendances.Where(a => a.ATTN_STAT == "002");
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

      private void CapacityCycle_Lb_Click(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = iScsc.Club_Methods.First(cm => cm.CODE == (long)CbmtCode_Lov.EditValue);
            if (_cbmt == null) return;

            ListMbspBs.DataSource =
               iScsc.Member_Ships
                  .Where(ms =>
                     ms.RECT_CODE == "004" &&
                     ms.VALD_TYPE == "002" &&
                     ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                     ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                     (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                     ms.Fighter_Public.CBMT_CODE == _cbmt.CODE
                  );

            if(ListMbspBs.List.Count > 0)
            {
               Adm_Tc.SelectedTab = tp_006;
               More_Tc.SelectedTab = tp_007;
               CochName_Txt.Text = _cbmt.Fighter.NAME_DNRM;
               MtodName_Txt.Text = _cbmt.Method.MTOD_DESC;
               QStrtTime_Tim.EditValue = _cbmt.STRT_TIME;
               QEndTime_Tim.EditValue = _cbmt.END_TIME;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Rtoa_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Payment_To_Another_Account"),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
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

      private void ListMbspBs_CurrentChanged(object sender, EventArgs e)
      {
         var _mbsp = ListMbspBs.Current as Data.Member_Ship;
         if (_mbsp == null) return;

         long? _rqid = 0;
         if (_mbsp.RWNO == 1)
            _rqid = _mbsp.Request_Row.Request.Request1.RQID;
         else
            _rqid = _mbsp.RQRO_RQST_RQID;

         ExpnAmnt1_Txt.EditValue = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == _rqid).Sum(pd => (pd.EXPN_PRIC + pd.EXPN_EXTR_PRCT) * pd.QNTY);
         DscnAmnt1_Txt.EditValue = iScsc.Payment_Discounts.Where(pd => pd.PYMT_RQST_RQID == _rqid).Sum(pd => pd.AMNT);
         PymtAmnt2_Txt.EditValue = iScsc.Payment_Methods.Where(pd => pd.PYMT_RQST_RQID == _rqid).Sum(pd => pd.AMNT);
         DebtPymtAmnt1_Txt.EditValue = Convert.ToInt64(ExpnAmnt_Txt.EditValue) - (Convert.ToInt64(PymtAmnt1_Txt.EditValue) + Convert.ToInt64(DscnAmnt_Txt.EditValue));
      }
   }
}
