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
using DevExpress.XtraGrid.Controls;
using System.IO;
using System.Scsc.ExtCode;
using System.Threading;
using System.Globalization;

namespace System.Scsc.Ui.OtherIncome
{
   public partial class OIC_TOTL_F : UserControl
   {
      public OIC_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);
      private string hBDay = "";

      private void Execute_Query()
      {
         setOnDebt = false;
         //if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            int pydt = PydtsBs1.Position;
            int pcdt = PcdtBs1.Position;

            var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
               .Where(rqst =>
                     rqst.RQTP_CODE == "016" &&
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

            var _expnIndx = ExpnBs1.Position;

            //if (!LinkCochPydt_Cbx.Checked)
            if (!LinkMtod_Cbx.Checked)
               ExpnBs1.DataSource =
                  iScsc.Expenses.Where(ex =>
                     ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                     ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                     ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
                     ex.EXPN_STAT == "002" /* هزینه های فعال */
                  );

            Expn_Gv.TopRowIndex = _expnIndx;

            DocsBs1.DataSource = iScsc.Modual_Reports.Where(m => m.MDUL_NAME == GetType().Name && m.STAT == "002");

            // 1397/05/15 * بدست آوردن شماره پرونده های درگیر در تمدید
            FighsBs1.DataSource = 
               iScsc.Fighters
               .Where(f => 
                  f.CONF_STAT == "002" &&
                  Rqids.Contains((long)f.RQST_RQID) &&
                  /*&& (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "004" || 
                   *    f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")*/ 
                  /*(Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || 
                      (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&*/
                  Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101)
               .OrderBy(f => f.FGPB_TYPE_DNRM);
            //FighBs1.DataSource = iScsc.Fighters.Where(f => );

            Grop_FLP.Controls.Clear();
            var allItems = new Button();

            allItems.Text = "همه موارد";
            allItems.Tag = 0;

            allItems.Click += GropButn_Click;
            Grop_FLP.Controls.Add(allItems);

            ExpnBs1.List.OfType<Data.Expense>().OrderBy(e => e.GROP_CODE).GroupBy(e => e.Group_Expense1).ToList().ForEach(
               g =>
               {
                  var b = new Button() { AutoSize = true };
                  if (g.Key != null)
                  {
                     b.Text = g.Key.GROP_DESC;
                     b.Tag = g.Key.CODE;
                  }
                  else
                     b.Text = "سایر موارد";                  
                  b.Click += GropButn_Click;
                  Grop_FLP.Controls.Add(b);
               }
            );
            

            PydtsBs1.Position = pydt;
            PcdtBs1.Position = pcdt;

            GustBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM == "005" && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);

            // 1401/01/03 * کنار مصطفی تو استخر هوابرد
            if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.RUN_QURY == "002"))
               SearchCustTell_Butn_Click(null, null);
         }
         requery = false;
      }

      void GropButn_Click(object sender, EventArgs e)
      {
         Button b = (Button) sender;
         if(b.Tag != null)
            if(Convert.ToInt64(b.Tag) != 0)
               Expn_Gv.ActiveFilterString = string.Format("GROP_CODE = {0}", b.Tag);
            else
               Expn_Gv.ActiveFilterString = "";
         else
            Expn_Gv.ActiveFilterString = "GROP_CODE IS NULL";
      }

      int RqstIndex;
      private void Get_Current_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Count >= 1)
               RqstIndex = RqstBs1.Position;
         }
      }

      private void Set_Current_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstIndex >= 0)
               RqstBs1.Position = RqstIndex;
         }
      }

      private void Create_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            RqstBs1.AddNew();
            FILE_NO_LookUpEdit.Focus();
         }
      }

      private void RqroBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;

            if (_rqst.SSTT_MSTT_CODE == 2 && (_rqst.SSTT_CODE == 1 || _rqst.SSTT_CODE == 2 || _rqst.SSTT_CODE == 3))
            {
               Gb_Expense.Visible = true;
               //*Gb_ExpenseItem.Visible = true;
               //Btn_RqstDelete1.Visible = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnDelete1.Enabled = true;
            }
            else if (!(_rqst.SSTT_MSTT_CODE == 2 && (_rqst.SSTT_CODE == 1 || _rqst.SSTT_CODE == 2 || _rqst.SSTT_CODE == 3)) && _rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               //*Gb_ExpenseItem.Visible = true;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnDelete1.Enabled = true;
            }
            else if (_rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               //*Gb_ExpenseItem.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               RqstBnDelete1.Enabled = false;
            }

            if(_rqst.RQID > 0)
            {
               var _rqro = RqroBs1.Current as Data.Request_Row;
               if (_rqro == null) return;

               PersianCalendar pc = new PersianCalendar();
               string _crntDate = string.Format("{0}/{1}", pc.GetMonth(DateTime.Now), pc.GetDayOfMonth(DateTime.Now));
               string _brthDate = string.Format("{0}/{1}", pc.GetMonth(_rqro.Fighter.BRTH_DATE_DNRM.Value.Date), pc.GetDayOfMonth(_rqro.Fighter.BRTH_DATE_DNRM.Value.Date));

               if (_crntDate == _brthDate && _rqro.Fighter.BRTH_DATE_DNRM.Value.Date.Year != DateTime.Now.Date.Year)
               {
                  PlayHappyBirthDate_Butn.Visible = true;
                  if (!iScsc.Log_Operations.Any(l => l.FIGH_FILE_NO == _rqro.FIGH_FILE_NO && l.LOG_TYPE == "010" && l.CRET_DATE.Value.Date == DateTime.Now.Date))
                  {
                     iScsc.INS_LGOP_P(
                        new XElement("Log",
                            new XAttribute("fileno", _rqro.FIGH_FILE_NO),
                            new XAttribute("type", "010"),
                            new XAttribute("text", string.Format("پخش آهنگ تبریک تولد برای " + "{0}", _rqro.Fighter.NAME_DNRM))
                        )
                     );
                     PlayHappyBirthDate_Butn.Tag = "stop";
                     PlayHappyBirthDate_Butn_Click(null, null);
                  }
               }
               else
                  PlayHappyBirthDate_Butn.Visible = false;

               // 1402/06/08
               AdatnBs.DataSource = iScsc.Dresser_Attendances.Where(a => a.RQST_RQID == _rqst.RQID && a.FIGH_FILE_NO == _rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && a.TKBK_TIME == null);
               HdatnBs.DataSource = iScsc.Dresser_Attendances.Where(a => a.FIGH_FILE_NO == _rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && a.CONF_STAT == "002" && a.TKBK_TIME != null);
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            //*Gb_ExpenseItem.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            RqstBnDelete1.Enabled = false;
         }
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
            Scsc.Data.Fighter Figh = FighBs1.Current as Scsc.Data.Fighter;

            iScsc.OIC_ERQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "016"),
                     new XAttribute("rqttcode", "001"),
                     new XAttribute("rqstrqid", rqstRqid),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XAttribute("rqstdesc", RqstDesc_Txt.EditValue ?? ""),
                     new XAttribute("lettno", Rqst == null ? "" : Rqst.LETT_NO ?? ""),
                     new XAttribute("lettdate", Rqst == null ? "" : (Rqst.LETT_NO == null ? "" : ( Rqst.LETT_DATE == null ? "" : Rqst.LETT_DATE.Value.ToString("yyyy-MM-dd")))),
                     new XAttribute("invcdate", InvcDate_Dt.Value.HasValue ? InvcDate_Dt.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                     new XElement("Request_Row",
                        new XAttribute("fileno", Figh == null ? FILE_NO_LookUpEdit.EditValue ?? "" : Figh.FILE_NO),
                        new XElement("Fighter_Public", 
                           new XAttribute("frstname", FrstName_Txt.Text),
                           new XAttribute("lastname", LastName_Txt.Text),
                           new XAttribute("natlcode", NatlCode_Txt.Text),
                           new XAttribute("cellphon", CellPhon_Txt.Text),
                           new XAttribute("suntcode", SuntCode_Lov.EditValue ?? ""),
                           new XAttribute("servno", SERV_NO_TextEdit.EditValue ?? ""),
                           new XAttribute("cochfileno", Coch_Lov.EditValue ?? "")
                        )
                     )
                  )
               )
            );
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
            Data.Request Rqst = RqstBs1.Current as Data.Request;
            if (Rqst == null) return;

            if (MessageBox.Show(this, "آیا با انصراف دادن درخواست موافق هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            
            // 1402/07/07 * Delete Locker
            if(AdatnBs.List.Count > 0)
               iScsc.ExecuteCommand(
                  string.Format("DELETE dbo.Dresser_Attendance WHERE Code IN ({0});", string.Join(",", AdatnBs.List.OfType<Data.Dresser_Attendance>().Select(da => da.CODE)))
               );

            iScsc.CNCL_RQST_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );
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
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs1.List.Count == 0)
                  RqstBnExit1_Click(null, null);
               else
                  Create_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
            Scsc.Data.Fighter Figh = FighBs1.Current as Scsc.Data.Fighter;
            if (Rqst == null) return;

            // 1402/08/29 * اگر دستبندی که به مشتری داده میشود تایید نشده باید آنها را تایید کنیم
            if (AdatnBs.List.OfType<Data.Dresser_Attendance>().Any(da => da.CONF_STAT == "001"))
            {
               ConfDasr_Butn_Click(null, null);
            }

            iScsc.OIC_ESAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID),
                     Rqst.Request_Rows
                     .Select(r =>
                        new XElement("Request_Row",
                           new XAttribute("rwno", r.RWNO),
                           new XAttribute("fileno", r.FIGH_FILE_NO)
                        )
                     ),
                     new XElement("Payment",
                        new XAttribute("setondebt", setOnDebt)
                     )
                  )
               )
            );
            //OldRecdBs1.List.Clear();
            // 1398/04/03 * به درخواست باشگاه بهاران خانم نقیبی قرار شد برای مشتریان مهمان گزینه ای اضافه کنیم که مدام بعد از اتمام درخواست از فرم خارج نشوند
            //  برای اینکار ما بایستی گزینه ای طراحی کنیم که اگر فعال باشد بتوانیم مشخص کنیم که اگر هنرجو مهمان هست دوباره درخواست دیگیری برای ان ثبت شود
            if(GustSaveRqst_PickButn.PickChecked)
            {
               if(FighsBs1.List.OfType<Data.Fighter>().Any(f => f.FILE_NO ==  Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && f.FGPB_TYPE_DNRM == "005"))
               {
                  if (RqstBs1.Count > 0)
                     RqstBs1.AddNew();

                  FILE_NO_LookUpEdit.EditValue = Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO;

                  Btn_RqstBnARqt1_Click(null, null);
               }
            }
            else if (GotoProfile_Cbx.Checked)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)) }
               );
            }

            // 1399/12/10 * اضافه کردن فرم مربوط به ثبت اطلاعات درخواست مشتریان
            if(followups != "")
            {
               switch (followups.Split(';').First())
               {
                  case "GLR_INDC_F":
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
                                             new XAttribute("formcaller", formCaller)
                                          )
                                    }
                                 })
                     );
                     formCaller = "";
                     break;
                  default:
                     break;
               }
               followups = "";
               rqstRqid = 0;
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
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs1.List.Count == 0)
                  RqstBnExit1_Click(null, null);
               else
               {
                  if(!GustSaveRqst_PickButn.PickChecked)
                     Create_Record();
               }
               requery = false;
            }
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2 || Rqst.SSTT_CODE == 3))
            {
               Gb_Expense.Visible = true;
               RqstBnDelete1.Enabled = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;

               UserProFile_Rb.ImageVisiable = true;
               var rqro = Rqst.Request_Rows.FirstOrDefault() as Data.Request_Row;
               try
               {
                  //var rqro = RqroBs1.Current as Data.Request_Row;

                  UserProFile_Rb.ImageProfile = null;
                  MemoryStream mStream = new MemoryStream();
                  byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", rqro.FIGH_FILE_NO))).ToArray();
                  mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                  Bitmap bm = new Bitmap(mStream, false);
                  mStream.Dispose();

                  if (InvokeRequired)
                     Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
                  else
                     UserProFile_Rb.ImageProfile = bm;
               }
               catch { UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482; }

               MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == rqro.FIGH_FILE_NO && mb.RECT_CODE == "004" && mb.VALD_TYPE == "002" && (mb.TYPE == "001" || mb.TYPE == "005") && (AllMbsp_Cbx.Checked || (mb.END_DATE.Value.Date >= DateTime.Now.Date)));
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2 || Rqst.SSTT_CODE == 3)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               RqstBnDelete1.Enabled /*= Btn_RqstSav1.Visible */= true;
               RqstBnASav1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               RqstBnDelete1.Enabled = /*Btn_RqstSav1.Visible = */false;
               RqstBnASav1.Enabled = false;
            }

            FgpbBs1_CurrentChanged(null, null);
         }
         catch
         {
            Gb_Expense.Visible = false;
            RqstBnDelete1.Enabled /*= Btn_RqstSav1.Visible*/ = false;
            RqstBnASav1.Enabled = false;
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
         //if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
            );
         }
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
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

      private void RqstBnPrint_Click(object sender, EventArgs e)
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

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
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

      private void RqstBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         if (RqstBs1.Current == null) return;
         var crnt = RqstBs1.Current as Data.Request;

         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bn_PaymentMethods_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs1.Current as Data.Payment;

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
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
            if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
            {
               var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
               if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
               {
                  FrstName_Txt.Focus();
                  return;
               }

               if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
               {
                  LastName_Txt.Focus();
                  return;
               }

               if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
               {
                  CellPhon_Txt.Focus();
                  return;
               }

               iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
            }

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
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
            //var pymt = PymtsBs1.Current as Data.Payment;

            /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
            {
               MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
               return;
            }*/

            // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
            if(PydtsBs1.List.Count == 0)
            {
               MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
               return;
            }

            foreach (Data.Payment pymt in PymtsBs1)
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithoutPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID),
                           new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                        )
                     )
                  )
               );
            }

            // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
            PymtVldtType_Cbx.Checked = true;

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstBnASav1_Click(null, null);
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
            Pydt_Gv.PostEditor();
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
                  rqst = RqstBs1.Current as Data.Request;

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
                                       new XAttribute("pydtdesc", pd.PYDT_DESC ?? ""),
                                       new XAttribute("qnty", pd.QNTY ?? 1),
                                       new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                                       new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
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
                                    new XAttribute("expncode", pd.EXPN_CODE),
                                    new XAttribute("expnpric", pd.EXPN_PRIC),
                                    new XAttribute("pydtdesc", pd.PYDT_DESC),
                                    new XAttribute("qnty", pd.QNTY ?? 1),
                                    new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                                    new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0),
                                    new XAttribute("mbsprwno", pd.MBSP_RWNO ?? 0),
                                    new XAttribute("extscode", pd.EXTS_CODE ?? 0),
                                    new XAttribute("extsrsrvdate", pd.EXTS_RSRV_DATE == null ? "" : pd.EXTS_RSRV_DATE.Value.ToString("yyyy-MM-dd"))
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

      private void RQTT_CODE_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         //try
         //{
         //   if (RQTT_CODE_LookUpEdit.ItemIndex == -1) return;

         //   if (PydtsBs1.List.Count > 0)
         //   {
         //      if (MessageBox.Show(this, "برای نوع متقاضی جدید باید هزینه های قبلی پاک شده و دوباره اعلام هزینه کنید. آیا موافقید؟", "حذف تمامی هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         //      PydtsBs1.List.OfType<Data.Payment_Detail>().ToList()
         //         .ForEach(pd =>
         //            {
         //               var rqst = RqstBs1.Current as Data.Request;
         //               iScsc.DEL_SEXP_P(
         //                  new XElement("Request",
         //                     new XAttribute("rqid", rqst.RQID),
         //                     new XElement("Payment",
         //                        new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
         //                        new XElement("Payment_Detail",
         //                           new XAttribute("code", pd.CODE)
         //                        )
         //                     )
         //                  )
         //               );
         //            }
         //         );
         //   }

         //   var rqro = RqroBs1.Current as Data.Request_Row;

         //   ExpnBs1.DataSource =
         //      iScsc.Expenses.Where(ex =>
         //         ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
         //         ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
         //         ex.Expense_Type.Request_Requester.RQTT_CODE == RQTT_CODE_LookUpEdit.EditValue.ToString() &&
         //         ex.MTOD_CODE == rqro.Fighter.MTOD_CODE_DNRM &&
         //         ex.CTGY_CODE == rqro.Fighter.CTGY_CODE_DNRM &&
         //         ex.EXPN_STAT == "002" /* هزینه های فعال */
         //      );
         //}catch(Exception)
         //{
            
         //}

      }

      private void ntb_POSPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
            if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
            {
               var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
               if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
               {
                  FrstName_Txt.Focus();
                  return;
               }

               if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
               {
                  LastName_Txt.Focus();
                  return;
               }

               if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
               {
                  CellPhon_Txt.Focus();
                  return;
               }

               iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
            }

            if (Accept_Cb.Checked /* 1402/11/01 if payment no debt */ && PymtsBs1.List.OfType<Data.Payment>().Any(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM) > 0))
            {
               var pymt = PymtsBs1.Current as Data.Payment;
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

            // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
            if (PydtsBs1.List.Count == 0)
            {
               MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
               return;
            }

            if (UsePos_Cb.Checked)
            {
               foreach (Data.Payment pymt in PymtsBs1)
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
               foreach (Data.Payment pymt in PymtsBs1)
               {
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithPOS"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                           )
                        )
                     )
                  );
               }

               // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
               PymtVldtType_Cbx.Checked = true;

               
               /* Loop For Print After Pay */
               if(DoPrint_Pkb.PickChecked) RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
         ////if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstBs1.Current == null) return;
         //   var rqst = RqstBs1.Current as Data.Request;
         //   var pymt = PymtsBs1.Current as Data.Payment;

         //   var xSendPos =
         //      new XElement("Form",
         //         new XAttribute("name", GetType().Name),
         //         new XAttribute("tabpage", "tp_001"),
         //         new XElement("Request",
         //            new XAttribute("rqid", rqst.RQID),
         //            new XAttribute("rqtpcode", rqst.RQTP_CODE),
         //            new XAttribute("fileno", rqst.Fighters.FirstOrDefault().FILE_NO),
         //            new XElement("Payment",
         //               new XAttribute("cashcode", pymt.CASH_CODE),
         //               new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
         //            )
         //         )
         //      );

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 93 /* Execute Pos_Totl_F */),
         //            new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "016")))}
                  })
               );
         }         
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

            //if (tb_master.SelectedTab == tp_001)
            {

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
               if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
               {
                  var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
                  if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
                  {
                     FrstName_Txt.Focus();
                     return;
                  }

                  if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
                  {
                     LastName_Txt.Focus();
                     return;
                  }

                  if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
                  {
                     CellPhon_Txt.Focus();
                     return;
                  }

                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
               }

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs1.Current as Data.Payment;
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

               // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
               if (PydtsBs1.List.Count == 0)
               {
                  MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
                  return;
               }

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

      private void AddItem_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            // اگر در جدول هزینه قبلا رکوردی درج شده باشد
            if (rqst == null) return;

            var expn = ExpnBs1.Current as Data.Expense;

            // چک میکنیم که قبلا از این آیتم هزینه در جدول ریز هزینه وجود نداشته باشد
            if (!PydtsBs1.List.OfType<Data.Payment_Detail>().Any(p => p.EXPN_CODE == expn.CODE))
            {
               PydtsBs1.AddNew();
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; pydt.PAY_STAT = "001"; pydt.RQRO_RWNO = 1; pydt.MTOD_CODE_DNRM = expn.MTOD_CODE; pydt.CTGY_CODE_DNRM = expn.CTGY_CODE; pydt.EXPR_DATE = DateTime.Now.AddDays((int)(expn.NUMB_CYCL_DAY ?? 0)).AddHours(expn.MIN_TIME.Value.Hour).AddMinutes(expn.MIN_TIME.Value.Minute).AddSeconds(expn.MIN_TIME.Value.Second); });
            }
            else
            {
               var pydt = PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.EXPN_CODE == expn.CODE).First();
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.QNTY += 1; });
            }

            // 1401/11/11 * اگر هزینه ای که انتخاب میکنیم صاحب هزینه تک داشته باشد سریعا صاحب هزینه را انتخاب میکنیم
            //if(AutoSlctCbmt_Cbx.Checked)
            if(LinkCochPydt_Cbx.Checked)
            {
               var _coch = CochBs1.Current as Data.Fighter;
               if (_coch != null)
               {
                  var _pydt = PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.EXPN_CODE == expn.CODE).First();
                  var _cbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.MTOD_CODE == expn.MTOD_CODE && cm.MTOD_STAT == "002" && cm.COCH_FILE_NO == _coch.FILE_NO);
                  if (_pydt.CODE == 0)
                  {
                     _pydt.CBMT_CODE_DNRM = _cbmt.CODE;
                     _pydt.FIGH_FILE_NO = _coch.FILE_NO;//CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.MTOD_CODE == expn.MTOD_CODE).COCH_FILE_NO;
                        
                     PydtsBs1.EndEdit();
                     iScsc.SubmitChanges();
                  }
                  else
                  {
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET Cbmt_Code_Dnrm = {0}, Figh_File_No = {1}, Qnty = {2} WHERE Code = {3};", _cbmt.CODE, _coch.FILE_NO, _pydt.QNTY, _pydt.CODE));
                  }
               }
            }
            else
            {
               PydtsBs1.EndEdit();
               iScsc.SubmitChanges();
            }

            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally 
         { 
            if(requery)            
            {
               requery = false;
               Execute_Query();
               requery = false;
            }
         }
      }

      private void LOV_EXPN_EditValueChanged(object sender, EventArgs e)
      {
         var pydt = PydtsBs1.Current as Data.Payment_Detail;

         if (((LookUpEdit)sender).EditValue != null)
         {
            if ((pydt.EXPN_PRIC ?? 0) == 0)
            {
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == (long)((LookUpEdit)sender).EditValue).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; });
            }
         }
      }

      private void advBandedGridView1_RowCountChanged(object sender, EventArgs e)
      {
         if(Expn_Gv.RowCount == 1)
         {
            //AddItem_ButtonClick(null, null);
            //advBandedGridView1.ApplyFindFilter("");
            //FindControl find = advBandedGridView1.GridControl.Controls.Find("FindControl", true)[0] as FindControl;
            //find.FindEdit.Focus();
         }
      }

      private void Butn_TakePicture_Click(object sender, EventArgs e)
      {
         try
         {
            if (true)
            {
               var fileno = (RqroBs1.Current as Data.Request_Row).FIGH_FILE_NO;
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

      private void Btn_IncressAccount_Click(object sender, EventArgs e)
      {
         try
         {
            //var glrl = GlrlBs3.Current as Data.Gain_Loss_Rial;
            var rqro = RqroBs1.Current as Data.Request_Row;

            if (rqro == null) return;
            //if (Txt_Amnt.EditValue == null || Txt_Amnt.EditValue.ToString() == "") return;

            if (MessageBox.Show(this, "آیا ثبت اعتبار برای عضو مورد نظر ثبت گردد؟", "افزایش اعتبار اعضا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            // این قسمت برنامه باید به واحد خودش در تغییرات ریالی انتقال پیدا کند تا از پرداکندگی کدها جلوگیری شود
            #region Gain_Loss_Rials
            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqstrqid", rqro.RQST_RQID),
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_003_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", rqro.FIGH_FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("chngtype", "002"),
                        new XAttribute("debttype", "005"),
                        new XAttribute("amnt", 0 /*Txt_Amnt.EditValue*/),
                        new XAttribute("paiddate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("chngresn", "006"),
                        new XAttribute("resndesc", "ثبت مبلغ افزایش سپرده * ثبت سیستمی")
                     )
                  )
               )
            );

            var rqst = iScsc.Requests.FirstOrDefault(r => r.RQTP_CODE == "020" && r.RQTT_CODE == "004" && r.MDUL_NAME == GetType().Name && r.SECT_NAME == GetType().Name.Substring(0, 3) + "_003_F" && r.Request_Rows.Any(rr => rr.FIGH_FILE_NO == rqro.FIGH_FILE_NO) && r.RQST_STAT == "001");
            
            if (rqst == null) return;

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID)
                  )
               )
            );
            #endregion

            //Txt_Amnt.EditValue = 0;

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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Pblc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = FILE_NO_LookUpEdit.EditValue;
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

      private void ntb_POSPayment1_Click_1(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               if (VPosBs1.List.Count == 0)
                  UsePos_Cb.Checked = false;

               if (UsePos_Cb.Checked)
               {
                  foreach (Data.Payment pymt in PymtsBs1)
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
                  foreach (Data.Payment pymt in PymtsBs1)
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
                  Btn_RqstBnASav1_Click(null, null);
               }
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void FgpbBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            if (FgpbBs1.Current != null)
               //if((FgpbBs1.Current as Data.Fighter_Public).TYPE == "005")
                  FreeAdm_Ro.RolloutStatus = true;
               //else
                  //FreeAdm_Ro.RolloutStatus = false;
            else
               FreeAdm_Ro.RolloutStatus = false;
         }
         catch (Exception )
         {

         }
      }

      private void RemoveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            ///* Do Delete Payment_Detail */
            //var Crnt = PydtsBs1.Current as Data.Payment_Detail;
            //var rqst = RqstBs1.Current as Data.Request;
            //iScsc.DEL_SEXP_P(
            //   new XElement("Request",
            //      new XAttribute("rqid", rqst.RQID),
            //      new XElement("Payment",
            //         new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
            //         new XElement("Payment_Detail",
            //            new XAttribute("code", Crnt.CODE)
            //         )
            //      )
            //   )
            //);


            //if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            /* Do Delete Payment_Detail */
            var Crnt = PydtsBs1.Current as Data.Payment_Detail;
            if (Crnt == null) return;
            if (Crnt.QNTY > 1)
            {
               Crnt.QNTY--;
               PydtsBs1.EndEdit();
               iScsc.SubmitChanges();
            }
            else
            {
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
            }
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

      private void SaveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Pydt_Gv.PostEditor();
            Pydt2_Gv.PostEditor();
            Pydt3_Gv.PostEditor();
            /* Do Something for insert or update Payment Detail Price */
            var rqst = RqstBs1.Current as Data.Request;

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
                              new XAttribute("pydtdesc", pd.PYDT_DESC ?? ""),
                              new XAttribute("qnty", pd.QNTY ?? 1),
                              new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0),
                              new XAttribute("mbsprwno", pd.MBSP_RWNO ?? 0),
                              new XAttribute("exprdate", pd.EXPR_DATE == null ? "" : pd.EXPR_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                              new XAttribute("totlwegh", pd.TOTL_WEGH ?? 0),
                              new XAttribute("unitnumb", pd.UNIT_NUMB ?? 0),
                              new XAttribute("unitapbscode", pd.UNIT_APBS_CODE ?? 0),
                              new XAttribute("cmnt", pd.CMNT ?? "")
                           )
                        )
                     )
                  );
               }
            );

            PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.CODE != 0).ToList()
               .ForEach(pd =>
               {
                  // 1401/07/27 * بررسی اینکه آیا جایگاه رزرو میباشد یا خیر 
                  // کیرم_تو_بیت_رهبری
                  // مهسا_امینی
                  if (pd.EXTS_CODE != null)
                  {
                     DateTime? _extsrsrvdate = pd.EXTS_RSRV_DATE == null ? DateTime.Now : pd.EXTS_RSRV_DATE;

                     if (iScsc.Expense_Type_Steps.FirstOrDefault(ets => ets.CODE == (long)pd.EXTS_CODE).QNTY <=
                            iScsc.Payment_Details
                            .Where(pdt => pdt.Request_Row.Request.RQST_STAT == "002" &&
                                         pdt.EXTS_CODE == (long)pd.EXTS_CODE &&
                                         pdt.EXTS_RSRV_DATE.Value.Date == _extsrsrvdate.Value.Date).Count())
                     {
                        if (MessageBox.Show(this, "جایگاه رزرو پر شده است. آیا باز میخواهید جایگاه را پر کنید؟", "جایگاه رزرو شده", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                     }
                  }

                  rqst = RqstBs1.Current as Data.Request;
                  iScsc.UPD_SEXP_P(
                     new XElement("Request",
                        new XAttribute("rqid", rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("code", pd.CODE),
                              new XAttribute("expncode", pd.EXPN_CODE),
                              new XAttribute("expnpric", pd.EXPN_PRIC),
                              new XAttribute("pydtdesc", pd.PYDT_DESC),
                              new XAttribute("qnty", pd.QNTY ?? 1),
                              new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0),
                              new XAttribute("mbsprwno", pd.MBSP_RWNO ?? 0),
                              new XAttribute("exprdate", pd.EXPR_DATE == null ? "" : pd.EXPR_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                              new XAttribute("fromnumb", pd.FROM_NUMB ?? 0),
                              new XAttribute("tonumb", pd.TO_NUMB ?? 0),
                              new XAttribute("extscode", pd.EXTS_CODE ?? 0),
                              new XAttribute("extsrsrvdate", pd.EXTS_RSRV_DATE == null ? "" : pd.EXTS_RSRV_DATE.Value.ToString("yyyy-MM-dd")),
                              new XAttribute("totlwegh", pd.TOTL_WEGH ?? 0),
                              new XAttribute("unitnumb", pd.UNIT_NUMB ?? 0),
                              new XAttribute("unitapbscode", pd.UNIT_APBS_CODE ?? 0),
                              new XAttribute("cmnt", pd.CMNT ?? "")
                           )
                        )
                     )
                  );
               }
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

      private void PydtsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            if (requery) return;

            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            if(_pydt.Expense != null)
               ExtsBs1.DataSource =
                  iScsc.Expense_Type_Steps
                  .Where(ets => ets.EXTP_CODE == _pydt.Expense.EXTP_CODE && ets.STAT == "002" &&
                                ((RsrvTimeList_Cbx.Checked && (TimeSpan.Compare(DateTime.Now.TimeOfDay, ets.FROM_TIME.Value.TimeOfDay) <= 0) && TimeSpan.Compare(ets.TO_TIME.Value.TimeOfDay, DateTime.Now.TimeOfDay) >= 0) ||                                  
                                  !RsrvTimeList_Cbx.Checked)
                  );

            requery = true;
            CbmtCode_GLov.EditValue = _pydt.CBMT_CODE_DNRM;
            MbspRwnoPydt_Lov.EditValue = _pydt.MBSP_RWNO;
            ExtsCode_Lov.EditValue = _pydt.EXTS_CODE;            

            // 1401/12/09 ** نمایش عکس پروفایل پرسنل که به مشتری خدمات داده شده است
            if (_pydt.FIGH_FILE_NO != null)
            {
               CexcBs.DataSource = iScsc.Calculate_Expense_Coaches.Where(c => c.RQTP_CODE == "016" && c.RQTT_CODE == "001" && c.COCH_FILE_NO == _pydt.FIGH_FILE_NO && c.EXPN_CODE == _pydt.EXPN_CODE && c.STAT == "002");

               if (CochServProFile_Rb.Tag == null || CochServProFile_Rb.Tag.ToString().ToInt64() != _pydt.FIGH_FILE_NO)
               {
                  if (_pydt.Fighter.IMAG_RCDC_RCID_DNRM != null)
                  {
                     try
                     {
                        CochServProFile_Rb.ImageProfile = null;
                        MemoryStream mStream = new MemoryStream();
                        byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _pydt.FIGH_FILE_NO))).ToArray();
                        mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                        Bitmap bm = new Bitmap(mStream, false);
                        mStream.Dispose();

                        if (InvokeRequired)
                           Invoke(new Action(() => CochServProFile_Rb.ImageProfile = bm));
                        else
                           CochServProFile_Rb.ImageProfile = bm;

                        CochProFile_Rb.Tag = _pydt.FIGH_FILE_NO;
                     }
                     catch { }
                  }
                  else
                  {
                     CochServProFile_Rb.ImageProfile = null;
                     CochServProFile_Rb.Tag = null;
                  }
               }

               if (CochServProFile_Rb.ImageProfile == null && _pydt.Fighter.SEX_TYPE_DNRM == "002")
                  CochServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1507;
               else if (CochServProFile_Rb.ImageProfile == null)
                  CochServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1076;
            }

            CochServProFile_Rb.Visible = _pydt.FIGH_FILE_NO != null;               

            requery = false;
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
            if (requery) { requery = false; return; }
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;
            
            //pydt.Club_Method = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == (long?)e.NewValue);
            iScsc.UPD_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                  new XElement("Payment",
                     new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                     new XElement("Payment_Detail",
                        new XAttribute("code", pydt.CODE),
                        new XAttribute("expncode", pydt.EXPN_CODE),
                        new XAttribute("expnpric", pydt.EXPN_PRIC),
                        new XAttribute("pydtdesc", pydt.PYDT_DESC),
                        new XAttribute("qnty", pydt.QNTY ?? 1),
                        new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                        new XAttribute("cbmtcodednrm", e.NewValue ?? 0),
                        new XAttribute("mbsprwno", pydt.MBSP_RWNO ?? 0),
                        new XAttribute("exprdate", pydt.EXPR_DATE == null ? "" : pydt.EXPR_DATE.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("extscode", pydt.EXTS_CODE ?? 0),
                        new XAttribute("extsrsrvdate", pydt.EXTS_RSRV_DATE == null ? "" : pydt.EXTS_RSRV_DATE.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("totlwegh", pydt.TOTL_WEGH ?? 0),
                        new XAttribute("unitnumb", pydt.UNIT_NUMB ?? 0),
                        new XAttribute("unitapbscode", pydt.UNIT_APBS_CODE ?? 0)
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
               Execute_Query();
         }
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
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;
            PymtAmnt_Txt.EditValue = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
         }
         catch { }
      }

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

            long? amnt = null;
            switch (PydsType_Butn.Tag.ToString())
            {
               case "0":
                  if (!(Convert.ToInt32(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt32(PydsAmnt_Txt.EditValue) <= 100))
                  {
                     PydsAmnt_Txt.EditValue = null;
                     PydsAmnt_Txt.Focus();
                  }

                  if(HowPyds_Cbx.Checked)
                     amnt = ((long?)(pydt.EXPN_PRIC * pydt.QNTY) * Convert.ToInt64(PydsAmnt_Txt.EditValue)) / 100;
                  else
                     amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt64(PydsAmnt_Txt.EditValue)) / 100;
                  break;
               case "1":
                  amnt = Convert.ToInt32(PydsAmnt_Txt.EditValue);
                  if (amnt == 0) return;
                  break;
            }

            // 1401/09/19 * #MahsaAmini
            // اگر تخفیف برای پرسنل بخواهیم ثبت کنیم باید چک کنیم که آیا تخفیف وارد شده بیشتر سهم پرسنل نباشد
            if (PydsType_Lov.EditValue.ToString() == "005")
            {
               var _pydt = PydtsBs1.Current as Data.Payment_Detail;

               var _calcexpn =
                  iScsc.CALC_EXPN_U(
                     new XElement("Request",
                         new XAttribute("rqid", pymt.RQST_RQID),
                         new XAttribute("expncode", _pydt.EXPN_CODE)
                     )
                  );

               // اگر مبلغ تخفیف بیشتر از سهم پرسنل باشد باید جلو آن گرفته شود
               if (_calcexpn < amnt)
               {
                  MessageBox.Show(this, "مبلغ تخفیف وارد شده از سهم پرسنل بیشتر حق پرداختی ایشان میباشد، لطفا درصد تخفیف یا مبلغ تخفیف را اصلاح کنید", "تخفیف غیرمجاز پرسنل");
                  return;
               }
            }

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, HowPyds_Cbx.Checked ? pydt.EXPN_CODE : null, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text, null, null);

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
            var pyds = PydsBs1.Current as Data.Payment_Discount;
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
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            if (PymtAmnt_Txt.EditValue == null || PymtAmnt_Txt.EditValue.ToString() == "" || Convert.ToInt64(PymtAmnt_Txt.EditValue) == 0) return;

            //1403/08/26 * اگر تاریخ پرداخت بیشتر از تاریخ جاری باشد
            if (PymtDate_DateTime001.Value.HasValue && PymtDate_DateTime001.Value.Value.Date > DateTime.Now.Date)
            {
               MessageBox.Show(this, "پرداختی در گذشته داریم ولی پرداختی در آینده نداریم، اینجاست که باید بگم داش داری اشتباه میزنی");
               PymtDate_DateTime001.Focus();
               PymtDate_DateTime001.Value = DateTime.Now;
               return;
            }

            switch ((RcmtType_Lov.EditValue ?? "001").ToString())
            {
               case "003":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked && (!PymtDate_DateTime001.Value.HasValue || PymtDate_DateTime001.Value.Value.Date == DateTime.Now.Date))
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
                                             new XAttribute("amnt", Convert.ToInt64(PymtAmnt_Txt.EditValue)),
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
                                 new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? ""),
                                 new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                              )
                           )
                        )
                     );

                     // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
                     PymtVldtType_Cbx.Checked = true;
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
                              new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? ""),
                              new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                           )
                        )
                     )
                  );

                  // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
                  PymtVldtType_Cbx.Checked = true;
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
            var pmmt = PmmtBs1.Current as Data.Payment_Method;
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

      private void Expn_Gv_DoubleClick(object sender, EventArgs e)
      {
         AddItem_ButtonClick(null, null);
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();

         switch (ShowRqst_PickButn.PickChecked)
         {
            case true:
               ShowRqst_Lb.Text =
                  string.Format(
                     "کاربر جاری " + CurrentUser + " . تعداد درخواست ثبت شده " + "{0} عدد میباشد", RqstBs1.Count
                  );
               break;
            case false:
               ShowRqst_Lb.Text =
                  string.Format(
                     "درخواست همه کاربران که تعداد ثبت شده " + "{0} عدد میباشد", RqstBs1.Count
                  );
               break;
            default:
               break;
         }
      }

      private void LOV_EXPRDATE_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               Pydt_Gv.PostEditor();
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               if(pydt.EXPR_DATE == null && MessageBox.Show(this, "تاریخ خالی می باشد آیا میخواهید تاریخ اعتبار همگی خالی شود؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(p => p.EXPR_DATE = null);
               }
               else
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(p => p.EXPR_DATE = pydt.EXPR_DATE);
               }

               SaveExpn_Butn_Click(null, null);
            }
            else if(e.Button.Index == 2)
            {
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               if (pydt == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 161 /* Execute Bas_Prod_F */),
                        new Job(SendType.SelfToUserInterface,"BAS_PROD_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Product", new XAttribute("epitcode", pydt.Expense.Expense_Type.Expense_Item.CODE), new XAttribute("pydtcode", pydt.CODE), new XAttribute("formstat", "sale"))}
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SubmitExpiDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null || pymt.Payment_Details.Count == 0) return;

            if (SlctAllPydt_Cb.Checked)
            {
               if (NumbDay_Txt.Text == "0" && NumbMonth_Txt.Text == "0")
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(pd => pd.EXPR_DATE = null);
               }
               else
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(pd => pd.EXPR_DATE = DateTime.Now.AddDays(Convert.ToDouble( NumbDay_Txt.Text)).AddMonths(Convert.ToInt32(NumbMonth_Txt.Text)));
               }
            }
            else
            {
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               if (pydt == null) return;

               // 1403/08/13
               if(ModifierKeys == Keys.Control)            
               {
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET EXPR_DATE = '{0}' WHERE CODE = {1};", pydt.EXPR_DATE, pydt.CODE));
                  return;
               }

               if (NumbDay_Txt.Text == "0" && NumbMonth_Txt.Text == "0")
               {
                  pydt.EXPR_DATE = null;
               }
               else
               {
                  pydt.EXPR_DATE = DateTime.Now.AddDays(Convert.ToDouble(NumbDay_Txt.Text)).AddMonths(Convert.ToInt32(NumbMonth_Txt.Text));
               }
            }

            SaveExpn_Butn_Click(null, null);
         }
         catch ( Exception exp)
         {
            MessageBox.Show(exp.Message);
         }
      }

      private void ExpnBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if(expn == null) return;

            ExpnItem_Tsmi.Text = ExpnDesc_Tsmi.Text = expn.EXPN_DESC;
            ExpnPric_Tsmi.Text = expn.PRIC.ToString();
            ExpnOrdrItem_Tsmi.Text = expn.ORDR_ITEM;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExpnItem_Tsmi_Click(object sender, EventArgs e)
      {
         AddItem_ButtonClick(null, null);
         Expn_Gc.Focus();
      }

      private void ExpnEdit_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_DESC = ExpnDesc_Tsmi.Text;
            expn.PRIC = Convert.ToInt64(ExpnPric_Tsmi.Text);
            if (ExpnOrdrItem_Tsmi.Text == "")
               expn.ORDR_ITEM = null;
            else
               expn.ORDR_ITEM = ExpnOrdrItem_Tsmi.Text;

            iScsc.SubmitChanges();
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

      private void DupExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", expn.CODE),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", DupExpnText_Tsmi.Text),
                  new XAttribute("pric", DupExpnPric_Tsmi.Text),
                  new XAttribute("ordritem", DupExpnOrdrItem_Tsmi.Text)
               )
            );

            DupExpnPric_Tsmi.Text = DupExpnText_Tsmi.Text = DupExpnOrdrItem_Tsmi.Text = "";

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

      private void OffExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_STAT = "001";
            iScsc.SubmitChanges();
            requery = true;               
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
               Execute_Query();
         }
      }

      private void NewExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", ""),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", NewExpnText_Tsmi.Text),
                  new XAttribute("pric", NewExpnPric_Tsmi.Text),
                  new XAttribute("ordritem", NewExpnOrdrItem_Tsmi.Text)
               )
            );

            NewExpnPric_Tsmi.Text = NewExpnText_Tsmi.Text = "";

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

      private void ServFind_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            ServFind_Tsmi.Text = "جستجوی مشتری";

            var fighs = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text));
            if(fighs.Count() == 0)
               return;

            if(fighs.Count() == 1)
            {
               ServFind_Tsmi.Text = string.Format("{0} : {1}", "جستجوی مشتری", fighs.FirstOrDefault().NAME_DNRM);

               if(SaveAutoRqst_Tsmi.CheckState == CheckState.Checked)
               {
                  // Create Request for Service
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", fighs.FirstOrDefault().FILE_NO)))}
                        })
                  );

                  // اولین گام این هست که ببینیم آیا ما توانسته ایم برای مشترک درخواست درآمد متفرقه ثبت کنیم یا خیر
                  fighs = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text));
                  var figh = fighs.FirstOrDefault();
                  if(!(figh.FIGH_STAT == "001" && figh.RQST_RQID != null && figh.Request.RQTP_CODE == "016" && figh.Request.RQTT_CODE == "001"))
                  {
                     MessageBox.Show("ثبت درخواست برای مشتری با مشکلی مواجه شده است، لطفا بررسی کنید");
                     return;
                  }

                  // Find Request for Service
                  RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.Request_Rows.Any(rr => rr.Fighter.FILE_NO == fighs.FirstOrDefault().FILE_NO)));
               }

               // مبلغ بدهی مشتری
               PayDebtAmnt2_Tsmi.Text = fighs.FirstOrDefault().DEBT_DNRM.ToString();

               DebtMenu_Tsmi.Enabled = ServPymt_Tsmi.Enabled = true;
            }
            else
            {
               ServFind_Tsmi.Text = string.Format("{0} : {1} {2}", "جستجوی مشتری", fighs.Count(), "مشتری پیدا شد");
               // مبلغ بدهی مشتری
               PayDebtAmnt2_Tsmi.Text = "0";

               DebtMenu_Tsmi.Enabled = ServPymt_Tsmi.Enabled = false;
            }

            Serv_Cms.Show();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveAutoRqst_Tsmi_Click(object sender, EventArgs e)
      {
         switch (SaveAutoRqst_Tsmi.CheckState)
         {
            case CheckState.Checked:
               SaveAutoRqst_Tsmi.CheckState = CheckState.Unchecked;
               break;
            case CheckState.Unchecked:
               SaveAutoRqst_Tsmi.CheckState = CheckState.Checked;
               break;
         }

         Serv_Cms.Show();
      }

      private void FindResult_Tsmi_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */),
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"), new XAttribute("filtering", "cellphon"), new XAttribute("filter_value", CellPhonFind_Tsmi.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #region پرداخت بدهی قبلی
      private void PayCashDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "001"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayPosDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "003"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayDepositeDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "005"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
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
      #endregion

      private void SaveServ2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.BYR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "025"),
                     new XAttribute("rqttcode", "004"),
                     new XAttribute("prvncode", "017"),
                     new XAttribute("regncode", "001"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName2_Tsmi.Text),
                        new XElement("Last_Name", LastName2_Tsmi.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Sex_Type", "001"),
                        new XElement("Cell_Phon", CellPhon2_Tsmi.Text),
                        new XElement("Type", "001"),
                        new XElement("Fngr_Prnt",
                           iScsc.Fighters
                            .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                            .Select(f => f.FNGR_PRNT_DNRM)
                            .ToList()
                            .Where(f => f.All(char.IsDigit))
                            .Max(f => Convert.ToInt64(f)) + 1)
                     ),
                     new XElement("Member_Ship",
                        new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               ).FirstOrDefault();

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

            FrstName2_Tsmi.Text = LastName2_Tsmi.Text = CellPhon2_Tsmi.Text = "";

            var figh = Rqst.Fighters.FirstOrDefault();

            if (SaveAutoRqst_Tsmi.CheckState == CheckState.Checked)
            {
               // Create Request for Service
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
               );

               // اولین گام این هست که ببینیم آیا ما توانسته ایم برای مشترک درخواست درآمد متفرقه ثبت کنیم یا خیر
               figh = iScsc.Fighters.Where(f => f.FILE_NO == figh.FILE_NO).FirstOrDefault();
               if (!(figh.FIGH_STAT == "001" && figh.RQST_RQID != null && figh.Request.RQTP_CODE == "016" && figh.Request.RQTT_CODE == "001"))
               {
                  MessageBox.Show("ثبت درخواست برای مشتری با مشکلی مواجه شده است، لطفا بررسی کنید");
                  return;
               }

               // Find Request for Service
               RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.Request_Rows.Any(rr => rr.Fighter.FILE_NO == figh.FILE_NO)));
            }

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

      private void ServPymt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                     },
                     new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", figh.FILE_NO),
                           new XAttribute("type", "refresh"),
                           new XAttribute("tabfocued", "tp_003")
                        )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ProdList_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 161 /* Execute Bas_Prod_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_PROD_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Product", new XAttribute("epitcode", expn.Expense_Type.Expense_Item.CODE), new XAttribute("formstat", "show"))}
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MbspRwnoPydt_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (requery) { requery = false; return; }
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

            if (e.NewValue == null) return;

            //pydt.Club_Method = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == (long?)e.NewValue);
            iScsc.UPD_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                  new XElement("Payment",
                     new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                     new XElement("Payment_Detail",
                        new XAttribute("code", pydt.CODE),
                        new XAttribute("expncode", pydt.EXPN_CODE),
                        new XAttribute("expnpric", pydt.EXPN_PRIC),
                        new XAttribute("pydtdesc", pydt.PYDT_DESC),
                        new XAttribute("qnty", pydt.QNTY ?? 1),
                        new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                        new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                        new XAttribute("exprdate", pydt.EXPR_DATE == null ? "" : pydt.EXPR_DATE.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("mbsprwno", e.NewValue ?? 0),
                        new XAttribute("extscode", pydt.EXTS_CODE ?? 0),
                        new XAttribute("extsrsrvdate", pydt.EXTS_RSRV_DATE == null ? "" : pydt.EXTS_RSRV_DATE.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("totlwegh", pydt.TOTL_WEGH ?? 0),
                        new XAttribute("unitnumb", pydt.UNIT_NUMB ?? 0),
                        new XAttribute("unitapbscode", pydt.UNIT_APBS_CODE ?? 0),
                        new XAttribute("cmnt", pydt.CMNT ?? "")
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
               Execute_Query();
         }
      }

      private void AutoRecalc_Tmr_Tick(object sender, EventArgs e)
      {
         try
         {
            List<string> evntLogs = new List<string>();

            var _pydts = 
               iScsc.Payment_Details
               .Where(
                  pd => 
                     pd.Payment.Request.RQTP_CODE == "016" &&
                     pd.Payment.Request.RQST_STAT == "002" &&
                     pd.EXPR_DATE.Value <= DateTime.Now.AddMinutes(AlrtPreExit_Cbx.Checked ? Convert.ToDouble(IntervalAlrtPreExit_Txt.EditValue) : 0) &&
                     pd.TRAN_DATE == null
               );

            if (_pydts.Count() == 0) goto L_Attendance;

            if(_pydts.Count() > 0)
            {
               if (PlaySondAlrm_Cbx.Checked)
               {
                  if (evntLogs.Count == 0)
                     evntLogs.Add(DateTime.Now.ToString("HH:mm:ss => --------- بلیط فروشی آزاد ---------"));

                  if (evntLogs.Count == 1)
                  {
                     //_wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\timeout.wav";
                     new Thread(AlarmShow).Start();
                  }

                  _pydts.ToList().ForEach(
                     pd =>
                     {
                        evntLogs.Add(string.Format(">> \"{0}\" - [ {1} ] <<", iScsc.GET_STRD_U(new XElement("Request", new XAttribute("type", "001"), new XAttribute("rqid", pd.PYMT_RQST_RQID))), pd.QNTY));
                     }
                  );                  
               }
            }

            iScsc.ExecuteCommand(string.Format("UPDATE Payment_Detail SET TRAN_DATE = GETDATE() WHERE Code IN ({0}) AND TRAN_DATE IS NULL;", string.Join(",", _pydts.Select(pd => pd.CODE))));

            if (evntLogs.Count() > 1)
            {
               foreach (var item in evntLogs)
               {
                  Evnt_Lbx.Items.Insert(0, item);
               }

               Gb_ExpenseItem.SelectedTab = tabPage3;
            }

            evntLogs.Clear();

            ///////////////////////////////////////////////////////////////// Attendance

            L_Attendance:
            var _attns =
               iScsc.Attendances
               .Where(
                  a => 
                     a.ATTN_DATE.Date == DateTime.Now.Date &&
                     a.CBMT_CODE_DNRM != null &&
                     //a.ENTR_TIME.Value.Add(new TimeSpan(0, (int)(a.Club_Method.CLAS_TIME ?? 90), 0))  <= DateTime.Now.TimeOfDay &&
                     ((DateTime)(a.ATTN_DATE + a.ENTR_TIME)).AddMinutes((double)(a.Club_Method.CLAS_TIME ?? 90)) <= DateTime.Now.AddMinutes(AlrtPreExit_Cbx.Checked ? Convert.ToDouble(IntervalAlrtPreExit_Txt.EditValue) : 0) &&
                     a.MUST_EXIT_TIME_DNRM == null &&
                     a.Fighter1.FGPB_TYPE_DNRM == "001"
               );

            if (_attns.Count() == 0) return;

            if(_attns.Count() > 0)
            {
               if(PlaySondAlrm_Cbx.Checked)
               {
                  if(evntLogs.Count == 0)
                     evntLogs.Add(DateTime.Now.ToString("HH:mm:ss => --------- مشتریان اشتراکی ---------"));

                  if(evntLogs.Count == 1)
                  {
                     _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\timeout.wav";
                     new Thread(AlarmShow).Start();
                  }

                  _attns.ToList().ForEach(
                     a => 
                        evntLogs.Add(string.Format(">> \"{0}\" - [ {1} ] <<", a.NAME_DNRM, a.CELL_PHON_DNRM))
                  );
               }
            }

            iScsc.ExecuteCommand(string.Format("UPDATE a SET a.MUST_EXIT_TIME_DNRM = DateAdd(Minute, ISNULL(cm.Clas_Time, 90), a.Entr_Time) FROM dbo.Attendance a, dbo.Club_Method cm WHERE a.Cbmt_Code_Dnrm = cm.Code AND a.Code in ({0}) AND a.Must_Exit_Time_Dnrm IS NULL;", string.Join(",", _attns.Select(a => a.CODE))));

            ////////////////// End of Calc
 
            if (evntLogs.Count() > 1)
            {
               foreach (var item in evntLogs)
               {
                  Evnt_Lbx.Items.Insert(0, item);
               }

               Gb_ExpenseItem.SelectedTab = tabPage3;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AutoRecalc_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         AutoRecalc_Tmr.Enabled = AutoRecalc_Cbx.Checked;
         AutoRecalc_Tmr.Interval = IntervalRecalc_Txt.Text.ToInt32() * 60000;
      }

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      string _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
      Color _evencolor = Color.YellowGreen, _oddcolor = Color.LimeGreen;
      private void AlarmShow()
      {
         if (InvokeRequired)
         {
            try
            {
               wplayer.URL = _wplayer_url;
               wplayer.controls.play();
            }
            catch { }

            //var tempcolor = BackGrnd_Butn.NormalColorA;
            //for (int i = 0; i < 5; i++)
            //{
            //   if (i % 2 == 0)
            //      BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.YellowGreen;
            //   else
            //      BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.LimeGreen;

            //   Thread.Sleep(100);
            //}
            //BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = tempcolor;
            _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
            _evencolor = Color.YellowGreen; _oddcolor = Color.LimeGreen;
         }
      }

      private void DelEvntLogs_Butn_Click(object sender, EventArgs e)
      {
         Evnt_Lbx.Items.Clear();
      }

      private void bn_DpstPayment3_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            bool finalAction = false;
            long? debtamnt = 0, dpstamnt = 0;

            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
            dpstamnt = pymt.Request.Request_Rows.FirstOrDefault().Fighter.DPST_AMNT_DNRM;

            if (dpstamnt == 0) { MessageBox.Show(this, "مبلغ سپرده مشتری صفر میباشد", "عدم موجودی سپرده مشتری"); return; }
            // 1401/02/04 * بروزرسانی مبلغ سپرده مشتری
            if(dpstamnt - pymt.Payment_Methods.Where(pm => pm.RCPT_MTOD == "005").Sum(pm => pm.AMNT) <= 0)
            {
               MessageBox.Show("مبلغ اعتبار سپرده برای مشتری وجود ندارد");
               return;
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

            foreach (Data.Payment pymt1 in PymtsBs1)
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithDeposit"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt1.CASH_CODE),
                           new XAttribute("rqstrqid", pymt1.RQST_RQID),
                           new XAttribute("amnt", debtamnt),
                           new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                        )
                     )
                  )
               );
            }

            // 1401/02/04 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
            PymtVldtType_Cbx.Checked = true;

            if (finalAction)
            {
               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
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
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void bn_Card2CardPayment3_Click(object sender, EventArgs e)
      {
         try
         {            
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
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

            foreach (Data.Payment pymt in PymtsBs1)
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithCard2Card"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID),
                           new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                        )
                     )
                  )
               );
            }

            // 1401/02/04 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
            PymtVldtType_Cbx.Checked = true;

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstBnASav1_Click(null, null);            
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void SUNT_CODELookUpEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Index)
         {
            case 1:
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
                        });
               _DefaultGateway.Gateway(_InteractWithScsc);
               break;
         }
      }

      private void RcmtType_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         RcmtType_Butn_Click(null, null);
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

      private void Advc_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null || _rqst.RQID == 0) return;

            var _fgdc = FgdcBs1.Current as Data.Fighter_Discount_Card;
            if (_fgdc == null) return;

            if (_fgdc.RQST_RQID != null) { MessageBox.Show(this, "این رکورد کد تخفیف قبلا درون درخواست ثبت شده است", "عدم ثبت کد تخفیف"); return; }
            if (_fgdc.CTGY_CODE != null && _rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault().CTGY_CODE != _fgdc.CTGY_CODE)
            {
               MessageBox.Show(this, "کاربر گرامی این کد تخفیف برای نرخ مورد نظر شما قابل استفاده نمی باشد", "عدم استفاده از کد تخفیف");
               return;
            }
            if (_fgdc.EXPR_DATE.Value.Date < DateTime.Now.Date) { MessageBox.Show(this, "تاریخ انقضای کد تخفیف شما تمام شده است", "عدم اعتبار تاریخ انقضا"); return; }

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

      private void bn_DiscountPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
            if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
            {
               var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
               if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
               {
                  FrstName_Txt.Focus();
                  return;
               }

               if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
               {
                  LastName_Txt.Focus();
                  return;
               }

               if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
               {
                  CellPhon_Txt.Focus();
                  return;
               }

               iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
            }

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
               if (pymt == null) return;

               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

               string mesg = "";
               if (debtamnt > 0)
               {
                  mesg =
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> تخفیف << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                        string.Format("{0:n0}", debtamnt),
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                        "امروز",
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }
            //var pymt = PymtsBs1.Current as Data.Payment;

            /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
            {
               MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
               return;
            }*/

            // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
            if (PydtsBs1.List.Count == 0)
            {
               MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
               return;
            }

            foreach (Data.Payment pymt in PymtsBs1)
            {
               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
               iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, debtamnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text, PydsDesc_Txt.Tag == null ? null : (long?)PydsDesc_Txt.Tag, null);
            }

            // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
            PymtVldtType_Cbx.Checked = true;

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstBnASav1_Click(null, null);
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void ExtsCode_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (requery) { requery = false; return; }
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;
            if (e.NewValue == null) return;

            // 1401/07/27 * بررسی اینکه آیا جایگاه رزرو میباشد یا خیر 
            // کیرم_تو_بیت_رهبری
            // مهسا_امینی
            DateTime? _extsrsrvdate = pydt.EXTS_RSRV_DATE == null ? DateTime.Now : pydt.EXTS_RSRV_DATE;

            if (iScsc.Expense_Type_Steps.FirstOrDefault(ets => ets.CODE == (long)e.NewValue).QNTY <=
                   iScsc.Payment_Details
                   .Where(pd => pd.Request_Row.Request.RQST_STAT == "002" &&
                                pd.EXTS_CODE == (long)e.NewValue &&
                                pd.EXTS_RSRV_DATE.Value.Date == _extsrsrvdate.Value.Date).Count())
            {
               if (MessageBox.Show(this, "جایگاه رزرو پر شده است. آیا باز میخواهید جایگاه را پر کنید؟", "جایگاه رزرو شده", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            }

            //pydt.Club_Method = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == (long?)e.NewValue);
            iScsc.UPD_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                  new XElement("Payment",
                     new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                     new XElement("Payment_Detail",
                        new XAttribute("code", pydt.CODE),
                        new XAttribute("expncode", pydt.EXPN_CODE),
                        new XAttribute("expnpric", pydt.EXPN_PRIC),
                        new XAttribute("pydtdesc", pydt.PYDT_DESC),
                        new XAttribute("qnty", pydt.QNTY ?? 1),
                        new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                        new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),                        
                        new XAttribute("mbsprwno", pydt.MBSP_RWNO ?? 0),
                        new XAttribute("exprdate", pydt.EXPR_DATE == null ? "" : pydt.EXPR_DATE.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("extscode", e.NewValue ?? 0),
                        new XAttribute("extsrsrvdate", pydt.EXTS_RSRV_DATE == null ? "" : pydt.EXTS_RSRV_DATE.Value.ToString("yyyy-MM-dd"))
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
               Execute_Query();
         }
      }

      private void RsrvTimeList_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         PydtsBs1_CurrentChanged(null, null);
      }

      private void ExtsCodes_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            ExtsRsrvDate_Dt.CommitChanges();
            if (!ExtsRsrvDate_Dt.Value.HasValue) { ExtsRsrvDate_Dt.Value = DateTime.Now; }

            switch (e.Button.Index)
            {
               case 1:
                  ExtsCodes_Lov.EditValue = null;
                  break;
               case 2:
                  ExtsSaveBs.DataSource =
                     (from r in iScsc.Requests
                     join pd in iScsc.Payment_Details on r.RQID equals pd.PYMT_RQST_RQID
                     join ets in iScsc.Expense_Type_Steps on pd.EXTS_CODE equals ets.CODE
                     where r.RQST_STAT == "002" &&
                           r.RQTP_CODE == "016" &&
                           pd.EXTS_CODE != null && pd.EXTS_RSRV_DATE.Value.Date == ExtsRsrvDate_Dt.Value.Value.Date
                     select ets).Distinct();

                  if (ExtsCodes_Lov.EditValue == null || ExtsCodes_Lov.EditValue.ToString() == "") { ExtsCodes_Lov.Focus(); return; }

                  RqroExtsBs.DataSource =
                     from r in iScsc.Requests
                     join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                     join pd in iScsc.Payment_Details on r.RQID equals pd.PYMT_RQST_RQID
                     where r.RQST_STAT == "002" &&
                           r.RQTP_CODE == "016" &&
                           pd.EXTS_CODE == (long?)ExtsCodes_Lov.EditValue &&
                           pd.EXTS_RSRV_DATE.Value.Date == ExtsRsrvDate_Dt.Value.Value.Date
                    select rr;
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

      private void SaveNewRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqro = RqroExtsBs.Current as Data.Request_Row;
            if (_rqro == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                  
                        //new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Request", 
                                 new XAttribute("type", "01"), 
                                 new XElement("Request_Row", 
                                    new XAttribute("fileno", _rqro.FIGH_FILE_NO)),                                 
                                 new XAttribute("rqstrqid", _rqro.RQST_RQID)
                              )
                        }
                     })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CBMT_CODE_GridLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  iScsc.ExecuteCommand(
                     string.Format("UPDATE dbo.Payment_Detail SET Figh_File_No = NULL, Cbmt_Code_Dnrm = NULL WHERE Code = {0};", _pydt.CODE)
                  );
                  break;
            }

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

      private void MbspRwnoPydt_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  iScsc.ExecuteCommand(
                     string.Format("UPDATE dbo.Payment_Detail SET Mbsp_Figh_File_No = NULL, Mbsp_Rect_Code = NULL, Mbsp_Rwno = NULL WHERE Code = {0};", _pydt.CODE)
                  );
                  break;
            }
            

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

      private void TreeSavePymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = (RqroExtsBs.Current as Data.Request_Row).Fighter;

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;
            if (TreePymtAmnt_Txt.EditValue == null || TreePymtAmnt_Txt.EditValue.ToString() == "" || Convert.ToInt64(TreePymtAmnt_Txt.EditValue) == 0) return;
            var paydebt = Convert.ToInt64(TreePymtAmnt_Txt.EditValue);

            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", TreeRcmtType_Lov.EditValue),
                           new XAttribute("actndate", TreePymtDate_DateTime001.Value.HasValue ? TreePymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

            TreeRcmtType_Lov.EditValue = TreePymtAmnt_Txt.EditValue = TreePymtDate_DateTime001.Value = null;
            requery = true;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               RqroExtsBs_CurrentChanged(null, null);
            }
         }
      }

      private void RqroExtsBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _rqroExts = RqroExtsBs.Current as Data.Request_Row;
            if (_rqroExts == null) return;

            // 1st Step Find Root Request
            var _rqst = _rqroExts.Request;
            do
            {               
               if (_rqst.RQST_RQID == null)
                  break;
               _rqst = _rqst.Request1;
            } while (true);

            long? _sumTotlPric = 0,
                  _sumDsctPric = 0,
                  _sumPymtPric = 0,
                  _SumDebtPric = 0;
            
            ///

            var _rqstLevl =
               iScsc.ExecuteQuery<Data.Request>(
                  string.Format(
                     @"WITH RequestLeveling (
                        Rqid,
                        Rqst_Rqid,
                        LEVL
                     ) 
                     AS 
                     (
                        SELECT R.RQID,
                               R.RQST_RQID,
                               1 AS LEVL
                          FROM dbo.Request r
                         WHERE r.RQID = {0}
                         UNION ALL 
                        SELECT c.RQID,
                               c.RQST_RQID,
                               rl.LEVL + 1 AS LEVL
                          FROM dbo.Request c INNER JOIN RequestLeveling rl ON c.RQST_RQID = rl.Rqid
                     )
                     SELECT Rqid,
                            Rqst_Rqid                     
                       FROM RequestLeveling rl
                      ORDER BY rl.Rqid;", _rqst.RQID

                  )
               );

            foreach (var _rqid in _rqstLevl)
            {
               var _pymt = iScsc.Payments.FirstOrDefault(p => p.RQST_RQID == _rqid.RQID);
               _sumTotlPric += _pymt.SUM_EXPN_PRIC;
               _sumDsctPric += _pymt.SUM_PYMT_DSCN_DNRM;
               _sumPymtPric += _pymt.SUM_RCPT_EXPN_PRIC;
               _SumDebtPric += (_pymt.SUM_EXPN_PRIC) - (_pymt.SUM_RCPT_EXPN_PRIC + _pymt.SUM_PYMT_DSCN_DNRM);
            }           

            SumTreeTotlPric_Txt.EditValue = _sumTotlPric;
            SumTreeDsctPric_Txt.EditValue = _sumDsctPric;
            SumTreePymtPric_Txt.EditValue = _sumPymtPric;
            SumTreeDebtPric_Txt.EditValue = _SumDebtPric;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void TreeRcmtType_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if(e.NewValue != null)
            TreePymtAmnt_Txt.EditValue = SumTreeDebtPric_Txt.EditValue;
      }

      private void Coch_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  Coch_Lov.EditValue = null;
                  break;
               case 2:
                  Btn_RqstBnARqt1_Click(null, null);
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

      private void AllMbsp_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         RqstBs1_CurrentChanged(null, null);
      }

      //WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      private void PlayHappyBirthDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqro = RqroBs1.Current as Data.Request_Row;
            if (_rqro == null) return;

            if (hBDay == "")
            {
               hBDay = iScsc.Settings.FirstOrDefault(s => s.CLUB_CODE == _rqro.Fighter.CLUB_CODE_DNRM).SND7_PATH;
            }

            wplayer.URL = hBDay;
            switch (PlayHappyBirthDate_Butn.Tag.ToString())
            {
               case "stop":
                  PlayHappyBirthDate_Butn.Tag = "play";
                  new Threading.Thread(PlaySound).Start();
                  break;
               case "play":
                  PlayHappyBirthDate_Butn.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      void PlaySound()
      {
         if (InvokeRequired)
            Invoke(new Action(() => wplayer.controls.play()));
         else
            wplayer.controls.play();
      }

      void StopSound()
      {
         if (InvokeRequired)
            Invoke(new Action(() => wplayer.controls.stop()));
         else
            wplayer.controls.stop();
      }

      private void CochBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _coch = CochBs1.Current as Data.Fighter;
            if (_coch == null) return;

            // 1401/12/16 
            var _mtod = MtodBs1.Current as Data.Method;
            if (_mtod == null) return;

            //if (!LinkCochPydt_Pbt.PickChecked) return;

            if (CochProFile_Rb.Tag == null || CochProFile_Rb.Tag.ToString().ToInt64() != _coch.FILE_NO)
            {
               if (_coch.IMAG_RCDC_RCID_DNRM != null)
               {
                  try
                  {
                     CochProFile_Rb.ImageProfile = null;
                     MemoryStream mStream = new MemoryStream();
                     byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _coch.FILE_NO))).ToArray();
                     mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                     Bitmap bm = new Bitmap(mStream, false);
                     mStream.Dispose();

                     if (InvokeRequired)
                        Invoke(new Action(() => CochProFile_Rb.ImageProfile = bm));
                     else
                        CochProFile_Rb.ImageProfile = bm;

                     CochProFile_Rb.Tag = _coch.FILE_NO;
                  }
                  catch { }
               }
               else
               {
                  CochProFile_Rb.ImageProfile = null;
                  CochProFile_Rb.Tag = null;
               }
            }

            if (CochProFile_Rb.ImageProfile == null && _coch.SEX_TYPE_DNRM == "002")
               CochProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1507;
            else if (CochProFile_Rb.ImageProfile == null)
               CochProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1076;

            if (!LinkMtod_Cbx.Checked) return;
            if (!LinkCochPydt_Cbx.Checked) return;

            ExpnBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */ &&
                  (!LinkCochPydt_Cbx.Checked || iScsc.Club_Methods.Any(cm => cm.MTOD_STAT == "002" && cm.MTOD_CODE == ex.MTOD_CODE && cm.COCH_FILE_NO == _coch.FILE_NO)) &&
                  (!LinkMtod_Cbx.Checked || ex.MTOD_CODE == _mtod.CODE)
               );

            Grop_FLP.Controls.Clear();
            var allItems = new Button();

            allItems.Text = "همه موارد";
            allItems.Tag = 0;

            allItems.Click += GropButn_Click;
            Grop_FLP.Controls.Add(allItems);

            ExpnBs1.List.OfType<Data.Expense>().OrderBy(ex => ex.GROP_CODE).GroupBy(ex => ex.Group_Expense1).ToList().ForEach(
               g =>
               {
                  var b = new Button() { AutoSize = true };
                  if (g.Key != null)
                  {
                     b.Text = g.Key.GROP_DESC;
                     b.Tag = g.Key.CODE;
                  }
                  else
                     b.Text = "سایر موارد";
                  b.Click += GropButn_Click;
                  Grop_FLP.Controls.Add(b);
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void LinkCochPydt_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         CochBs1_CurrentChanged(null, null);
      }

      private void FindRqst_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            FromRqstDate_Dt.CommitChanges();
            ToRqstDate_Dt.CommitChanges();

            DateTime? _fromrqstdate = null, _torqstdate = null;
            if (FromRqstDate_Cbx.Checked && !FromRqstDate_Dt.Value.HasValue) { FromRqstDate_Dt.Focus(); return; }
            if (FromRqstDate_Cbx.Checked && FromRqstDate_Dt.Value.HasValue) { _fromrqstdate = FromRqstDate_Dt.Value.Value.Date; }
            if (ToRqstDate_Cbx.Checked && !ToRqstDate_Dt.Value.HasValue) { ToRqstDate_Dt.Focus(); return; }
            if (ToRqstDate_Cbx.Checked && ToRqstDate_Dt.Value.HasValue) { _torqstdate = ToRqstDate_Dt.Value.Value.Date; }

            // 1398/05/20 * بررسی اینکه کاربر اجازه اجرا کردن گزارش در هر تاریخی را دارد یا خیر
            if (!checkValidateDate(_fromrqstdate.HasValue ? _fromrqstdate.Value : DateTime.Now.AddDays(-1))) return;

            long? _fighfileno = null, _cochfileno = null, _expncode = null;
            string _cellphon = null, _fngrprnt = null;
            if (RqstBs1.Current != null)
            {
               _fighfileno = (RqroBs1.Current as Data.Request_Row).FIGH_FILE_NO;
            }
            if (CochBs1.Current != null)
            {
               _cochfileno = (CochBs1.Current as Data.Fighter).FILE_NO;
            }
            if (ExpnBs1.Current != null)
            {
               _expncode = (ExpnBs1.Current as Data.Expense).CODE;
            }
            if(WhoCellPhon_Cbx.Checked)
            {
               if (OldCellPhon_Txt.EditValue == null || OldCellPhon_Txt.EditValue.ToString() == "") { OldCellPhon_Txt.Focus(); return; }
               else {_cellphon = OldCellPhon_Txt.Text;}
            }
            if (WhoFngrPrnt_Cbx.Checked)
            {
               if (OldFngrPrnt_Txt.EditValue == null || OldFngrPrnt_Txt.EditValue.ToString() == "") { OldFngrPrnt_Txt.Focus(); return; }
               else { _fngrprnt = OldFngrPrnt_Txt.Text; }
            }

            string _rcptmtod = null, _pydstype = null;
            if (HasPmmt_Cbx.Checked && (OldPmmt_Lov.EditValue == null || OldPmmt_Lov.EditValue.ToString() == "")) { OldPmmt_Lov.Focus(); return; }
            if(HasPmmt_Cbx.Checked) { _rcptmtod = OldPmmt_Lov.EditValue.ToString(); }
            if (HasPyds_Cbx.Checked && (OldPyds_Lov.EditValue == null || OldPyds_Lov.EditValue.ToString() == "")) { OldPyds_Lov.Focus(); return; }
            if(HasPyds_Cbx.Checked) { _pydstype = OldPyds_Lov.EditValue.ToString(); }

            if (RqstHist_Rlt.RolloutStatus)
            {
               OldRqstBs1.DataSource =
                  iScsc.Requests
                  .Where(r =>
                     r.RQTP_CODE == "016" &&
                     r.RQST_STAT == "002" &&
                     (!CrntUser_Cbx.Checked || r.CRET_BY == CurrentUser) &&
                     (!FromRqstDate_Cbx.Checked || _fromrqstdate == null || r.RQST_DATE.Value.Date >= (_fromrqstdate ?? r.RQST_DATE.Value.Date)) &&
                     (!ToRqstDate_Cbx.Checked || _torqstdate == null || r.RQST_DATE.Value.Date <= (_torqstdate ?? r.RQST_DATE.Value.Date)) &&
                     (!CrntServ_Cbx.Checked || _fighfileno == null || r.Request_Rows.Any(rr => rr.FIGH_FILE_NO == (_fighfileno ?? rr.FIGH_FILE_NO))) &&
                     (!CrntCoch_Cbx.Checked || _cochfileno == null || r.Payments.Any(p => p.Payment_Details.Any(pd => pd.FIGH_FILE_NO == (_cochfileno ?? pd.FIGH_FILE_NO)))) &&
                     (!CrntExpn_Cbx.Checked || _expncode == null || r.Payments.Any(p => p.Payment_Details.Any(pd => pd.EXPN_CODE == (_expncode ?? pd.EXPN_CODE)))) &&
                     (!HasDebt_Cbx.Checked || r.Payments.Any(p => p.SUM_EXPN_PRIC - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM) > 0)) &&
                     (!HasPmmt_Cbx.Checked || r.Payments.Any(p => p.Payment_Methods.Any(pm => pm.RCPT_MTOD == _rcptmtod))) &&
                     (!HasPyds_Cbx.Checked || r.Payments.Any(p => p.Payment_Discounts.Any(ps => ps.AMNT_TYPE == _pydstype))) &&
                     (!WhoCellPhon_Cbx.Checked || _cellphon == null || r.Request_Rows.Any(rr => rr.Fighter.CELL_PHON_DNRM.Contains(_cellphon))) &&
                     (!WhoFngrPrnt_Cbx.Checked || _fngrprnt == null || r.Request_Rows.Any(rr => rr.Fighter.FNGR_PRNT_DNRM.Contains(_fngrprnt)))
                  )
                  .OrderByDescending(r => r.SAVE_DATE);
            }

            if(RqstExpir_Rlt.RolloutStatus)
            {
               OldPydtsBs2.DataSource =
                  iScsc.Payment_Details
                  .Where(pd => 
                     pd.Request_Row.Request.RQTP_CODE == "016" &&
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (!CrntUser_Cbx.Checked || pd.CRET_BY == CurrentUser) &&
                     //(!FromRqstDate_Cbx.Checked || _fromrqstdate == null || pd.Request_Row.Request.RQST_DATE.Value.Date >= (_fromrqstdate ?? pd.Request_Row.Request.RQST_DATE.Value.Date)) &&
                     //(!ToRqstDate_Cbx.Checked || _torqstdate == null || pd.Request_Row.Request.RQST_DATE.Value.Date <= (_torqstdate ?? pd.Request_Row.Request.RQST_DATE.Value.Date)) &&
                     (!FromRqstDate_Cbx.Checked || _fromrqstdate == null || pd.EXPR_DATE.Value.Date >= (_fromrqstdate ?? pd.EXPR_DATE.Value.Date)) &&
                     (!ToRqstDate_Cbx.Checked || _torqstdate == null || pd.EXPR_DATE.Value.Date <= (_torqstdate ?? pd.EXPR_DATE.Value.Date)) &&
                     (!CrntServ_Cbx.Checked || _fighfileno == null || pd.Request_Row.FIGH_FILE_NO == (_fighfileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
                     (!CrntCoch_Cbx.Checked || _cochfileno == null || pd.FIGH_FILE_NO == (_cochfileno ?? pd.FIGH_FILE_NO)) &&
                     (!CrntExpn_Cbx.Checked || _expncode == null || pd.EXPN_CODE == (_expncode ?? pd.EXPN_CODE)) &&
                     (!HasDebt_Cbx.Checked || pd.Payment.SUM_EXPN_PRIC - (pd.Payment.SUM_RCPT_EXPN_PRIC + pd.Payment.SUM_PYMT_DSCN_DNRM) > 0) &&
                     (!HasPmmt_Cbx.Checked || pd.Payment.Payment_Methods.Any(pm => pm.RCPT_MTOD == _rcptmtod)) &&
                     (!HasPyds_Cbx.Checked || pd.Payment.Payment_Discounts.Any(ps => ps.AMNT_TYPE == _pydstype)) &&
                     (!WhoCellPhon_Cbx.Checked || _cellphon == null || pd.Request_Row.Fighter.CELL_PHON_DNRM.Contains(_cellphon)) &&
                     (!WhoFngrPrnt_Cbx.Checked || _fngrprnt == null || pd.Request_Row.Fighter.FNGR_PRNT_DNRM.Contains(_fngrprnt)) &&
                     (!ShowRmndNext_Cbx.Checked || pd.EXPR_DATE.Value.Date >= DateTime.Now.Date)
                  );

               OldPydts2_Gv.ExpandAllGroups();
            }

            if(HistPydt_Rlt.RolloutStatus)
            {
               HistPydtBs.DataSource =
                  iScsc.Payment_Details
                  .Where(pd =>
                     pd.Request_Row.Request.RQTP_CODE == "016" &&
                     pd.Request_Row.Request.RQST_STAT == "002" &&
                     (!CrntUser_Cbx.Checked || pd.CRET_BY == CurrentUser) &&
                     (!FromRqstDate_Cbx.Checked || _fromrqstdate == null || pd.CRET_DATE.Value.Date >= (_fromrqstdate ?? pd.CRET_DATE.Value.Date)) &&
                     (!ToRqstDate_Cbx.Checked || _torqstdate == null || pd.CRET_DATE.Value.Date <= (_torqstdate ?? pd.CRET_DATE.Value.Date)) &&
                     (!CrntServ_Cbx.Checked || _fighfileno == null || pd.Request_Row.FIGH_FILE_NO == (_fighfileno ?? pd.Request_Row.FIGH_FILE_NO)) &&
                     (!CrntCoch_Cbx.Checked || _cochfileno == null || pd.FIGH_FILE_NO == (_cochfileno ?? pd.FIGH_FILE_NO)) &&
                     (!CrntExpn_Cbx.Checked || _expncode == null || pd.EXPN_CODE == (_expncode ?? pd.EXPN_CODE)) &&
                     (!HasDebt_Cbx.Checked || pd.Payment.SUM_EXPN_PRIC - (pd.Payment.SUM_RCPT_EXPN_PRIC + pd.Payment.SUM_PYMT_DSCN_DNRM) > 0) &&
                     (!HasPmmt_Cbx.Checked || pd.Payment.Payment_Methods.Any(pm => pm.RCPT_MTOD == _rcptmtod)) &&
                     (!HasPyds_Cbx.Checked || pd.Payment.Payment_Discounts.Any(ps => ps.AMNT_TYPE == _pydstype)) &&
                     (!WhoCellPhon_Cbx.Checked || _cellphon == null || pd.Request_Row.Fighter.CELL_PHON_DNRM.Contains(_cellphon)) &&
                     (!WhoFngrPrnt_Cbx.Checked || _fngrprnt == null || pd.Request_Row.Fighter.FNGR_PRNT_DNRM.Contains(_fngrprnt)) &&
                     pd.FIGH_FILE_NO != null
                  );
            }

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private bool checkValidateDate(DateTime dateTime)
      {
         bool result = false;
         if (dateTime.Date == DateTime.Now.Date) return true;

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
                              "<Privilege>240</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output) {
                                 result = true;
                                 return;
                              }
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 240 سطوح امینتی", "عدم دسترسی");
                              result = false;
                           })
                        },
                        #endregion
                     })
               });
         _DefaultGateway.Gateway(_InteractWithScsc);

         return result;
      }

      private void AllRqst4CrntServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Param1_Rlt.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false);
            CrntServ_Cbx.Checked = true;

            RqstHist_Rlt.RolloutStatus = true;
            FindRqst_Btn_Click(null, null);
            Rqst_Tc.SelectedTab = tp_002;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowRmndNext_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            FindRqst_Btn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CochProFile_Rb_Click(object sender, EventArgs e)
      {
         try
         {
            var _coch = CochBs1.Current as Data.Fighter;
            if (_coch == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _coch.FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CochServProFile_Rb_Click(object sender, EventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null || _pydt.FIGH_FILE_NO == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _pydt.FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MtodBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs1.Current as Data.Method;
            if (_mtod == null) return;

            if (!LinkMtod_Cbx.Checked) return;

            CochBs1.DataSource =
               iScsc.Fighters.
               Where(c =>
                  c.CONF_STAT == "002" &&
                  c.ACTV_TAG_DNRM == "101" &&
                  c.FGPB_TYPE_DNRM == "003" &&
                  (!LinkMtod_Cbx.Checked || c.Club_Methods.Any(cm => cm.MTOD_CODE == _mtod.CODE && cm.MTOD_STAT == "002"))
               );

            ExpnBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */ &&
                  ex.MTOD_CODE == _mtod.CODE
               );

            Grop_FLP.Controls.Clear();
            var allItems = new Button();

            allItems.Text = "همه موارد";
            allItems.Tag = 0;

            allItems.Click += GropButn_Click;
            Grop_FLP.Controls.Add(allItems);

            ExpnBs1.List.OfType<Data.Expense>().OrderBy(ex => ex.GROP_CODE).GroupBy(ex => ex.Group_Expense1).ToList().ForEach(
               g =>
               {
                  var b = new Button() { AutoSize = true };
                  if (g.Key != null)
                  {
                     b.Text = g.Key.GROP_DESC;
                     b.Tag = g.Key.CODE;
                  }
                  else
                     b.Text = "سایر موارد";
                  b.Click += GropButn_Click;
                  Grop_FLP.Controls.Add(b);
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void LinkMtod_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs1.Current as Data.Method;
            if (_mtod == null) return;

            if (!LinkMtod_Cbx.Checked)
            {
               CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003" && c.ACTV_TAG_DNRM == "101" && c.CONF_STAT == "002" && c.Club_Methods.Any(cm => cm.MTOD_STAT == "002"));

               ExpnBs1.DataSource =
                  iScsc.Expenses.Where(ex =>
                     ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                     ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                     ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
                     ex.EXPN_STAT == "002" /* هزینه های فعال */
                  );               
            }
            else
            {
               ExpnBs1.DataSource =
                  iScsc.Expenses.Where(ex =>
                     ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                     ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                     ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
                     ex.EXPN_STAT == "002" /* هزینه های فعال */ &&
                     ex.MTOD_CODE == _mtod.CODE
                  );
            }

            Grop_FLP.Controls.Clear();
            var allItems = new Button();

            allItems.Text = "همه موارد";
            allItems.Tag = 0;

            allItems.Click += GropButn_Click;
            Grop_FLP.Controls.Add(allItems);

            ExpnBs1.List.OfType<Data.Expense>().OrderBy(ex => ex.GROP_CODE).GroupBy(ex => ex.Group_Expense1).ToList().ForEach(
               g =>
               {
                  var b = new Button() { AutoSize = true };
                  if (g.Key != null)
                  {
                     b.Text = g.Key.GROP_DESC;
                     b.Tag = g.Key.CODE;
                  }
                  else
                     b.Text = "سایر موارد";
                  b.Click += GropButn_Click;
                  Grop_FLP.Controls.Add(b);
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveExpnInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ExpnBs1.EndEdit();
            Expn_Gv.PostEditor();

            //var _expn = ExpnBs1.Current as Data.Expense;
            //if (_expn == null) return;

            //iScsc.UPD_EXPN_P(_expn.CODE, _expn.PRIC, _expn.EXPN_STAT, _expn.ADD_QUTS, _expn.COVR_DSCT, _expn.EXPN_TYPE, _expn.BUY_PRIC, _expn.BUY_EXTR_PRCT, _expn.NUMB_OF_STOK, _expn.NUMB_OF_SALE, _expn.COVR_TAX, _expn.NUMB_OF_ATTN_MONT, _expn.NUMB_OF_ATTN_WEEK, _expn.MODL_NUMB_BAR_CODE, _expn.PRVT_COCH_EXPN, _expn.NUMB_CYCL_DAY, _expn.NUMB_MONT_OFER, _expn.MIN_NUMB, _expn.GROP_CODE, _expn.EXPN_DESC, _expn.MIN_TIME, _expn.RELY_CMND, _expn.ORDR_ITEM, _expn.BRND_CODE, _expn.MIN_PRIC, _expn.MAX_PRIC, _expn.UNIT_APBS_CODE);
            iScsc.SubmitChanges();
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

      private void SaveCustTell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long? fileno = null, rqid = null;
            string cntycode, prvncode, regncode;
            var _conts = FgpbCellBs.List.OfType<Data.Fighter_Public>().FirstOrDefault();
            if (_conts == null)
            {
               var _gust = GustBs.OfType<Data.Fighter>().FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005");
               if (_gust == null)
                  throw new Exception("لطفا مشتری آزادی را درون سیستم تعریف کنید");
               else
               {
                  fileno = _gust.FILE_NO;
                  cntycode = _gust.REGN_PRVN_CNTY_CODE;
                  prvncode = _gust.REGN_PRVN_CODE;
                  regncode = _gust.REGN_CODE;
                  rqid = iScsc.VF_Request_Changing(fileno).Where(r => r.RQTP_CODE == "001" || r.RQTP_CODE == "025").FirstOrDefault().RQID;
               }
            }
            else
            {
               fileno = _conts.FIGH_FILE_NO;
               cntycode = _conts.REGN_PRVN_CNTY_CODE;
               prvncode = _conts.REGN_PRVN_CODE;
               regncode = _conts.REGN_CODE;
               rqid = _conts.RQRO_RQST_RQID;

            }

            // 1401/01/03 * Check NOT EXISTS Member in Contacts List
            if (CellPhon1_Txt.Text == null || CellPhon1_Txt.Text == "" || !CellPhon1_Txt.Text.Length.IsBetween(10, 11)) { CellPhon1_Txt.Focus(); return; }
            var _cont = FgpbCellBs.List.OfType<Data.Fighter_Public>().Any(fp => fp.CELL_PHON == CellPhon1_Txt.Text);
            if (!_cont)
            {
               // Save Contact in Database
               iScsc.ExecuteCommand(string.Format("INSERT INTO dbo.Fighter_Public(Regn_Prvn_Cnty_Code, Regn_Prvn_Code, Regn_Code, Rqro_Rqst_Rqid, Rqro_Rwno, Figh_File_No, Rect_Code, Frst_Name, Last_Name, Cell_Phon) VALUES('{0}', '{1}', '{2}', {3}, 1, {4}, '002', N'{5}', N'{6}', '{7}');", cntycode, prvncode, regncode, rqid, fileno, FrstName1_Txt.Text, LastName1_Txt.Text, CellPhon1_Txt.Text));
               FrstName1_Txt.Text = LastName1_Txt.Text = CellPhon1_Txt.Text = "";
               FgpbCustTell_Gv.FindFilterText = "";
               CellPhon1_Txt.Focus();
               requery = true;
            }
            else
            {
               // Focus in list
               FgpbCellBs.Position = FgpbCellBs.IndexOf(FgpbCellBs.List.OfType<Data.Fighter_Public>().FirstOrDefault(fp => fp.CELL_PHON == CellPhon1_Txt.Text));
               FrstName1_Txt.Text = LastName1_Txt.Text = CellPhon1_Txt.Text = "";
               //MessageBox.Show(this, "اطلاعات درون سیستم وجود دارد لطفا بررسی بفرمایید");
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

      private void CnclCustTell_Butn_Click(object sender, EventArgs e)
      {
         FrstName1_Txt.Text = LastName1_Txt.Text = CellPhon1_Txt.Text = "";
         FgpbCustTell_Gv.FindFilterText = "";
         FrstName1_Txt.Focus();
      }

      private void SearchCustTell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (CrntDateCustTell_Rb.Checked)
               FgpbCellBs.DataSource = iScsc.Fighter_Publics.Where(fb => fb.RECT_CODE == "002" && fb.CRET_DATE.Value.Date == DateTime.Now.Date);
            else if (AllDateCustTell_Rb.Checked)
               FgpbCellBs.DataSource = iScsc.Fighter_Publics.Where(fb => fb.RECT_CODE == "002");
            else if (SetDateCustTell_Rb.Checked)
            {
               if (SetDateCustTell_Dt.Value.HasValue)
                  FgpbCellBs.DataSource = iScsc.Fighter_Publics.Where(fb => fb.RECT_CODE == "002" && fb.CRET_DATE.Value.Date == SetDateCustTell_Dt.Value.Value.Date);
               else
               {
                  SetDateCustTell_Dt.Focus();
                  return;
               }
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UpdtCustTell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FgpbCustTell_Gv.PostEditor();

            iScsc.SubmitChanges();
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

      private void GotoCustTell_Butn_Click(object sender, EventArgs e)
      {
         Rqst_Tc.SelectedTab = tp_003;
         CellPhon1_Txt.Focus();
         CellPhon1_Txt.SelectAll();
      }

      private void CellPhon1_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         FgpbCustTell_Gv.FindFilterText = CellPhon1_Txt.Text;
      }

      private void FRqpm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Request_Parameter"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DRqpm_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            var _drqpm = DRqpmBs.Current as Data.App_Base_Define;
            if (_drqpm == null) return;

            PymtOprt_Tc.SelectedTab = More_Tp;
            More_Tc.SelectedTab = tp_rqpm;

            if (RqpmBs.List.OfType<Data.Request_Parameter>().Any(rp => rp.APBS_CODE == _drqpm.CODE)) return;

            var _rqpm = RqpmBs.AddNew() as Data.Request_Parameter;
            _rqpm.APBS_CODE = _drqpm.CODE;
            _rqpm.RQST_RQID = _rqst.RQID;

            iScsc.Request_Parameters
               .InsertOnSubmit(_rqpm);

            iScsc.SubmitChanges();
            //iScsc.ExecuteCommand(string.Format("INSERT INTO dbo.Request_Parameter(Rqst_Rqid, Apbs_Code, Code) VALUES ({0}, {1}, 0);", _rqst.RQID, _drqpm.CODE));

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

      private void Rqpm_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {            
            var _rqpm = RqpmBs.Current as Data.Request_Parameter;
            if (_rqpm == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  iScsc.Request_Parameters.DeleteOnSubmit(_rqpm);
                  break;
               case 1:
                  Rqpm_Gv.PostEditor();
                  break;
               default:
                  break;
            }            

            iScsc.SubmitChanges();
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

      private void Cexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            if (_pydt.FIGH_FILE_NO == null) { CbmtCode_GLov.Focus(); CbmtCode_GLov.ShowPopup(); return; }

            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            Totl1PydtAmnt_Txt.EditValue = _pydt.EXPN_PRIC * _pydt.QNTY;
            Totl1PydsAmnt_Txt.EditValue = PydsBs1.List.OfType<Data.Payment_Discount>().Where(pd => pd.AMNT_TYPE != "005" && pd.EXPN_CODE == _pydt.EXPN_CODE).Sum(pd => pd.AMNT);
            Totl2PydsAmnt_Txt.EditValue = PydsBs1.List.OfType<Data.Payment_Discount>().Where(pd => pd.AMNT_TYPE == "005" && pd.EXPN_CODE == _pydt.EXPN_CODE).Sum(pd => pd.AMNT);

            // Calculate amount
            if(_cexc.CALC_TYPE == "001") // %
            {
               Totl2RcptAmnt_Txt.EditValue = (_pydt.PROF_AMNT_DNRM * _cexc.PRCT_VALU / 100) - PydsBs1.List.OfType<Data.Payment_Discount>().Where(pd => pd.AMNT_TYPE == "005" && pd.EXPN_CODE == _pydt.EXPN_CODE).Sum(pd => pd.AMNT);
               Totl1RcptAmnt_Txt.EditValue = (_pydt.PROF_AMNT_DNRM + _pydt.DEDU_AMNT_DNRM) - (_pydt.PROF_AMNT_DNRM * _cexc.PRCT_VALU / 100) - PydsBs1.List.OfType<Data.Payment_Discount>().Where(pd => pd.AMNT_TYPE != "005" && pd.EXPN_CODE == _pydt.EXPN_CODE).Sum(pd => pd.AMNT);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ChckPreCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            if (_pydt.FIGH_FILE_NO == null) { CbmtCode_GLov.Focus(); CbmtCode_GLov.ShowPopup(); return; }

            if(!RtoaBs.List.OfType<Data.App_Base_Define>().Any(p => p.ENTY_NAME == "Payment_To_Another_Account" && p.TITL_DESC.StartsWith(_pydt.FIGH_FILE_NO.ToString())))
            {
               iScsc.ExecuteCommand(
                  string.Format("INSERT INTO dbo.App_Base_Define (Code, Rwno, Titl_Desc, Enty_Name) VALUES(0, {0}, N'{1} - {2} - {3} - {4}', 'Payment_To_Another_Account');",
                  (RtoaBs.List.OfType<Data.App_Base_Define>().Max(p => p.RWNO) ?? 0) + 1,
                  _pydt.Fighter.FILE_NO,
                  _pydt.Fighter.NAME_DNRM,
                  _pydt.Fighter.DPST_ACNT_SLRY_BANK_DNRM ?? "بانک نامشخص",
                  _pydt.Fighter.DPST_ACNT_SLRY_DNRM ?? "حساب بانک نامشخص"
                  )
               );

               RtoaBs.DataSource = iScsc.App_Base_Defines.Where(p => p.ENTY_NAME == "Payment_To_Another_Account").ToList();
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SetInit1Rcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RcmtType_Lov.EditValue = Totl1RcptAmnt_Lov.EditValue;
            PymtAmnt_Txt.EditValue = Totl1RcptAmnt_Txt.EditValue;
            Rtoa_Lov.EditValue = null;
            SavePymt_Butn.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SetInit2Rcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ChckPreCexc_Butn_Click(null, null);

            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            RcmtType_Lov.EditValue = Totl2RcptAmnt_Lov.EditValue;
            PymtAmnt_Txt.EditValue = Totl2RcptAmnt_Txt.EditValue;
            Rtoa_Lov.EditValue = RtoaBs.List.OfType<Data.App_Base_Define>().FirstOrDefault(p => p.TITL_DESC.StartsWith(_pydt.FIGH_FILE_NO.ToString())).CODE;
            SavePymt_Butn.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      class SumPymtAcnt
      {
         public SumPymtAcnt(long code, long amnt)
         {
            Code = code;
            Amnt = amnt;
         }

         public long Code { get; set; }
         public long Amnt { get; set; }
      }

      List<SumPymtAcnt> _sumPymtAcnt = new List<SumPymtAcnt>();

      private void CexcListSumPymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _sumPymtAcnt.Clear();
            Pymt_Lbx.Items.Clear();
            foreach (var _pydt in PydtsBs1.List.OfType<Data.Payment_Detail>())
            {
               PydtsBs1.Position = PydtsBs1.IndexOf(_pydt);
               Cexc_Butn_Click(null, null);

               SetInit1Rcpt_Butn_Click(null, null);
               _sumPymtAcnt.Add(
                  new SumPymtAcnt(Convert.ToInt64((Rtoa_Lov.EditValue ?? 0)), Convert.ToInt64(PymtAmnt_Txt.EditValue))
               );

               SetInit2Rcpt_Butn_Click(null, null);
               _sumPymtAcnt.Add(
                  new SumPymtAcnt(Convert.ToInt64((Rtoa_Lov.EditValue ?? 0)), Convert.ToInt64(PymtAmnt_Txt.EditValue))
               );
            }

            var _query = _sumPymtAcnt.GroupBy(g => g.Code);

            foreach (var _g in _query)
            {
               Pymt_Lbx.Items.Add(string.Format("{0} - {1:n0}", (_g.Key == 0 ? "حساب کارفرما" : RtoaBs.List.OfType<Data.App_Base_Define>().FirstOrDefault(g => g.CODE == _g.Key).TITL_DESC), _g.Sum(gs => gs.Amnt)));
            }
            Pymt_Lbx.Items.Add("-----------------------------");
            Pymt_Lbx.Items.Add(string.Format("{0} - {1:n0}", "سرجمع کل دریافتی", _query.Sum(g => g.Sum(gs => gs.Amnt))));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PycoActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            var _dpyco = DAPycoBs.Current as Data.App_Base_Define;

            if (PycoBs.List.OfType<Data.Payment_Cost>().Any(pc => pc.CODE == 0)) return;

            var _pyco = PycoBs.AddNew() as Data.Payment_Cost;
            if (_pyco == null) return;

            _pyco.PYCO_APBS_CODE = _dpyco.CODE;
            _pyco.Payment = _pydt.Payment;
            _pyco.COST_TYPE = "004";
            _pyco.EFCT_TYPE = "002";
            _pyco.AMNT = 0;

            iScsc.Payment_Costs.InsertOnSubmit(_pyco);
            iScsc.SubmitChanges();
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

      private void DelPyco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _pyco = PycoBs.Current as Data.Payment_Cost;
            if (_pyco == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Payment_Costs.DeleteOnSubmit(_pyco);
            iScsc.SubmitChanges();
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

      private void Pyco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "PaymentCost_INFO"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SavePyco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Pyco_Gv.PostEditor();

            iScsc.SubmitChanges();
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

      private void PydtActn1_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;

            switch (e.Button.Index)
            {
               case 0:
                  // Decriment or delete record
                  if (ModifierKeys.HasFlag(Keys.Control))
                  {                     
                     if (_pydt == null) return;
                     _pydt.QNTY = 1;
                  }
                  
                  RemoveExpn_Butn_Click(null, null);
                  break;
               case 1:
                  // Submit Change and save data
                  SaveExpn_Butn_Click(null, null);
                  break;
               case 2:
                  // Incriment record
                  //1403/06/23 * error operation in call action
                  //AddItem_ButtonClick(null, null);
                  
                  if (_pydt == null) return;
                  _pydt.QNTY += 1;

                  SaveExpn_Butn_Click(null, null);
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

      private void FromRqstDate_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         if (!FromRqstDate_Dt.Value.HasValue) FromRqstDate_Dt.Value = DateTime.Now;
      }

      private void ToRqstDate_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         if (!ToRqstDate_Dt.Value.HasValue) ToRqstDate_Dt.Value = DateTime.Now;
      }

      private void CalcHistPydt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            decimal _calcAmnt = 0.0m, _prctValu = PrctHist_Se.Value;

            foreach (var _pydt in HistPydtBs.List.OfType<Data.Payment_Detail>())
            {
               _calcAmnt += (decimal)((_pydt.PROF_AMNT_DNRM - _pydt.DEDU_AMNT_DNRM) * _prctValu) / 100;
            }

            HistYouCalcValu_Txt.EditValue = _calcAmnt;
            HistTotlAmnt_Txt.EditValue = HistPydtBs.List.OfType<Data.Payment_Detail>().Sum(p => p.PROF_AMNT_DNRM - p.DEDU_AMNT_DNRM);
            HistCalcValu_Txt.EditValue = HistPydtBs.List.OfType<Data.Payment_Detail>().Sum(p => p.PROF_AMNT_DNRM - p.DEDU_AMNT_DNRM) - _calcAmnt;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void InsCalcHistPydt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromRqstDate_Dt.Value.HasValue) { FromRqstDate_Dt.Value = DateTime.Now; }
            if (!ToRqstDate_Dt.Value.HasValue) { ToRqstDate_Dt.Value = DateTime.Now; }
            iScsc.CALC_EXPN_P(
               new XElement("Payment",
                   new XAttribute("fromdate", FromRqstDate_Dt.Value.Value.Date.ToString("yyyy-MM-dd")),
                     new XAttribute("todate", ToRqstDate_Dt.Value.Value.Date.ToString("yyyy-MM-dd")),
                     new XAttribute("cochfileno", CrntCoch_Cbx.Checked ? (CochBs1.Current as Data.Fighter).FILE_NO.ToString() : ""),
                     new XAttribute("dyncprctvalu", 100 - PrctHist_Se.Value)
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

      private void FinlCalcHistPydt_Butn_Click(object sender, EventArgs e)
      {
         try
         {

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

      private void JoinDasr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            if (AdatnBs.List.OfType<Data.Dresser_Attendance>().Any(a => a.DERS_NUMB == DresNumb_Txt.Text.ToInt32() && a.FIGH_FILE_NO == _rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && a.LEND_TIME != null && a.TKBK_TIME == null)) return;

            int _dres;
            if (!int.TryParse(DresNumb_Txt.Text, out _dres)) { DresNumb_Txt.Text = ""; return; }

            if (!iScsc.Dressers.Any(d => d.DRES_NUMB == DresNumb_Txt.Text.ToInt32())) return;

            var _adatn = AdatnBs.AddNew() as Data.Dresser_Attendance;
            _adatn.FIGH_FILE_NO = _rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO;
            _adatn.DERS_NUMB = DresNumb_Txt.Text.ToInt32();
            _adatn.RQST_RQID = _rqst.RQID;

            iScsc.Dresser_Attendances.InsertOnSubmit(_adatn);
            iScsc.SubmitChanges();

            DresNumb_Txt.Text = "";
            DresNumb_Txt.Focus();

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

      private void ConfDasr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE dbo.Dresser_Attendance SET Conf_Stat = '002' WHERE FIGH_FILE_NO = {0} AND RQST_RQID = {1} AND TKBK_TIME IS NULL;" + Environment.NewLine +
                  "UPDATE dbo.Dresser_Attendance SET TKBK_TIME = GETDATE() WHERE RQST_RQID != {1} AND TKBK_TIME IS NULL AND Ders_Numb IN (SELECT da.Ders_Numb FROM dbo.Dresser_Attendance da WHERE da.Rqst_Rqid = {1});",
                   _rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO,
                   _rqst.RQID
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

      private void DelDasr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "DELETE dbo.Dresser_Attendance WHERE FIGH_FILE_NO = {0} AND RQST_RQID = {1} AND TKBK_TIME IS NULL AND CONF_STAT = '001';",
                  _rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO,
                  _rqst.RQID
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

      private void DresNumb_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            JoinDasr_Butn_Click(null, null);
            e.Handled = false;
         }
      }

      private void DresConf_Butn_Click(object sender, EventArgs e)
      {
         PymtOprt_Tc.SelectedTab = Dres_Tp;
         DresNumb_Txt.Focus();
      }

      private void DartActv_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _dart = AdatnBs.Current as Data.Dresser_Attendance;
            if (_dart == null) return;

            iScsc.Dresser_Attendances.DeleteOnSubmit(_dart);
            iScsc.SubmitChanges();

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

      private void AddNewMbsp_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ContRecd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "PaymentContractItem_INFO"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SetPymtContItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _pmct = PmctBs1.Current as Data.Payment_Contract;
            if(_pmct == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "MERGE dbo.Payment_Contract_Detail T" + Environment.NewLine +
                  "USING (SELECT {0} AS PMCT_CODE, a.CODE AS ITEM_CODE, a.REF_CODE AS GROP_CODE FROM dbo.App_Base_Define a WHERE a.ENTY_NAME = 'PaymentContractItem_INFO' AND a.REF_CODE IS NOT NULL) S" + Environment.NewLine +
                  "ON (T.Pmct_Code = S.Pmct_Code AND T.Grop_Item_Apbs_Code = S.Grop_Code AND T.Sub_Item_Apbs_Code = S.Item_Code)" + Environment.NewLine +
                  "WHEN NOT MATCHED THEN INSERT (Pmct_Code, Grop_Item_Apbs_Code, Sub_Item_Apbs_Code, Code) VALUES (S.Pmct_Code, S.Grop_Code, S.Item_Code, dbo.GNRT_NVID_U());",
                  _pmct.CODE
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

      private void PmctItemActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _item = DPmctBs.Current as Data.App_Base_Define;
            if (_item == null) return;

            var _personel = CochBs1.Current as Data.Fighter;
            if (_personel == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  if (FlpcBs.List.OfType<Data.Fighter_Link_Payment_Contarct_Item>().Any(a => a.FIGH_FILE_NO == _personel.FILE_NO && a.PMCT_ITEM_APBS_CODE == _item.CODE)) return;

                  var _flpc = FlpcBs.AddNew() as Data.Fighter_Link_Payment_Contarct_Item;
                  _flpc.FIGH_FILE_NO = _personel.FILE_NO;
                  _flpc.PMCT_ITEM_APBS_CODE = _item.CODE;

                  iScsc.Fighter_Link_Payment_Contarct_Items.InsertOnSubmit(_flpc);
                  iScsc.SubmitChanges();
                  break;
               default:
                  break;
            }

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

      private void FlcpActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _flpc = FlpcBs.Current as Data.Fighter_Link_Payment_Contarct_Item;
            if (_flpc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //iScsc.Fighter_Link_Payment_Contarct_Items.DeleteOnSubmit(_flpc);
            //iScsc.SubmitChanges();
            iScsc.ExecuteCommand(string.Format("DELETE dbo.Fighter_Link_Payment_Contarct_Item WHERE Code = {0};", _flpc.CODE));

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

      private void FindObject_BTxt_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _btxt = sender as ButtonEdit;

            switch (e.Button.Index)
            {
               case 0:
                  switch (_btxt.Tag.ToString())
	               {
                     case "cell":
                        FindFighBs.DataSource =
                           iScsc.Fighters
                           .Where(f => 
                              Equal_Rb.Checked ? f.CELL_PHON_DNRM == _btxt.Text :
                              Contain_Rb.Checked ? f.CELL_PHON_DNRM.Contains(_btxt.Text) :
                              Start_Rb.Checked ? f.CELL_PHON_DNRM.StartsWith(_btxt.Text) :
                              f.CELL_PHON_DNRM.EndsWith(_btxt.Text)
                           );
                        break;
                     case "natl":
                        FindFighBs.DataSource =
                           iScsc.Fighters
                           .Where(f =>
                              Equal_Rb.Checked ? f.NATL_CODE_DNRM == _btxt.Text :
                              Contain_Rb.Checked ? f.NATL_CODE_DNRM.Contains(_btxt.Text) :
                              Start_Rb.Checked ? f.NATL_CODE_DNRM.StartsWith(_btxt.Text) :
                              f.NATL_CODE_DNRM.EndsWith(_btxt.Text)
                           );
                        break;
                     case "fngr":
                        FindFighBs.DataSource =
                           iScsc.Fighters
                           .Where(f =>
                              Equal_Rb.Checked ? f.FNGR_PRNT_DNRM == _btxt.Text :
                              Contain_Rb.Checked ? f.FNGR_PRNT_DNRM.Contains(_btxt.Text) :
                              Start_Rb.Checked ? f.FNGR_PRNT_DNRM.StartsWith(_btxt.Text) :
                              f.FNGR_PRNT_DNRM.EndsWith(_btxt.Text)
                           );
                        break;
                     case "serv":
                        FindFighBs.DataSource =
                           iScsc.Fighters
                           .Where(f =>
                              Equal_Rb.Checked ? f.SERV_NO_DNRM == _btxt.Text :
                              Contain_Rb.Checked ? f.SERV_NO_DNRM.Contains(_btxt.Text) :
                              Start_Rb.Checked ? f.SERV_NO_DNRM.StartsWith(_btxt.Text) :
                              f.SERV_NO_DNRM.EndsWith(_btxt.Text)
                           );
                        break;
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
      }

      private void CretNewRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _figh = FindFighBs.Current as Data.Fighter;
            if (_figh == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", _figh.FILE_NO)))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowProfile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _figh = FindFighBs.Current as Data.Fighter;
            if (_figh == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _figh.FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ClearObject_Butn_Click(object sender, EventArgs e)
      {
         FindCellPhon_BTxt.Text = FindNatlCode_BTxt.Text = FindNatlCode_BTxt.Text = FindServCode_BTxt.Text = "";
      }

      private void SavePmct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Pcdt_Gv.PostEditor();

            var _pymt = PymtsBs1.Current as Data.Payment;
            if(_pymt == null)return;

            //if (PmctBs1.List.Count != 1) return;

            if (PmctBs1.Current == null)
            {
               var _pmct = PmctBs1.AddNew() as Data.Payment_Contract;
               _pmct.Payment = _pymt;

               iScsc.Payment_Contracts.InsertOnSubmit(_pmct);
            }

            iScsc.SubmitChanges();

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

      private void PcdtiActn_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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
                                    new XAttribute("tablename", "PaymentContractItemColor_INFO"),
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

      private void PctdBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _pcdt = PcdtBs1.Current as Data.Payment_Contract_Detail;
            if (_pcdt == null) return;

            Flpc_Gv.ActiveFilterString = string.Format("PMCT_ITEM_APBS_CODE = {0}", _pcdt.SUB_ITEM_APBS_CODE);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FlpcActn1_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _flpc = FlpcBs.Current as Data.Fighter_Link_Payment_Contarct_Item;
            if (_flpc == null) return;

            var _pcdt = PcdtBs1.Current as Data.Payment_Contract_Detail;
            if (_pcdt == null) return;

            if (_pcdt.SUB_ITEM_APBS_CODE != _flpc.PMCT_ITEM_APBS_CODE) return;

            iScsc.ExecuteCommand(
               string.Format("UPDATE dbo.Payment_Contract_Detail SET FLPC_CODE = {0} WHERE CODE = {1};", _flpc.CODE, _pcdt.CODE)
            );
            iScsc.SubmitChanges();

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

      private void SaveRfndDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            var _pymt = PymtsBs1.Current as Data.Payment;
            if (_pymt == null) return;

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment SET RFND_DATE = '{0}' WHERE RQST_RQID = {1};", _pymt.RFND_DATE, _pymt.RQST_RQID));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OrdrItem1_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (OrdrItem1_Txt.Text != "")
               Expn_Gv.ActiveFilterString = string.Format("Ordr_Item = '{0}'", OrdrItem1_Txt.Text);
            else
               Expn_Gv.ActiveFilterString = "";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CochCode1_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (CochCode1_Txt.Text != "")
               Coch_Gv.ActiveFilterString = string.Format("Fngr_Prnt_Dnrm = '{0}'", CochCode1_Txt.Text);
            else
               Coch_Gv.ActiveFilterString = "";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GoUpExpn_Butn_Click(object sender, EventArgs e)
      {
         Expn_Gv.MovePrev();
      }

      private void GoDnExpn_Butn_Click(object sender, EventArgs e)
      {
         Expn_Gv.MoveNext();
      }

      private void AddToCart1_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _pymt = PymtsBs1.Current as Data.Payment;
            if (_pymt == null) return;

            var _expn = ExpnBs1.Current as Data.Expense;
            if (_expn == null) return;

            if (OrdrItem1_Txt.Text == "") return;

            if (CochCode1_Txt.Text != "")
            {
               var _coch = CochBs1.List.OfType<Data.Fighter>().FirstOrDefault(c => c.FNGR_PRNT_DNRM == CochCode1_Txt.Text);
               if (_coch != null)
               {
                  LinkCochPydt_Cbx.Checked = true;
                  CochBs1.Position = CochBs1.IndexOf(_coch);
               }
            }

            if (!PydtsBs1.List.OfType<Data.Payment_Detail>().Any(pd => pd.EXPN_CODE == _expn.CODE))
            {
               AddItem_ButtonClick(null, null);
            }

            PydtsBs1.List.OfType<Data.Payment_Detail>().FirstOrDefault(pd => pd.EXPN_CODE == _expn.CODE).QNTY = Convert.ToSingle(Qnty1_Txt.Text);
            
            if(per1000_Cbx.Checked)
               PydtsBs1.List.OfType<Data.Payment_Detail>().FirstOrDefault(pd => pd.EXPN_CODE == _expn.CODE).EXPN_PRIC = (long)(Convert.ToSingle(Pric1_Txt.Text) * 1000);
            else
               PydtsBs1.List.OfType<Data.Payment_Detail>().FirstOrDefault(pd => pd.EXPN_CODE == _expn.CODE).EXPN_PRIC = (long)Convert.ToSingle(Pric1_Txt.Text);

            PydtActn1_Butn_ButtonClick(sender, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(PydtActn1_Butn.Buttons[1]));

            OrdrItem1_Txt.Text = CochCode1_Txt.Text = "";
            Pric1_Txt.EditValue = Qnty1_Txt.EditValue = null;            
            OrdrItem1_Txt.Focus();
            Expn_Gv.ActiveFilterString = Coch_Gv.ActiveFilterString = "";            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MtodList_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            /// Must Be Change
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 144 /* Execute Bas_Dfin_F */),
                   new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("showtabpage", "tp_003"))}
                 });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CochList_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            /// Must Be Change
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 144 /* Execute Bas_Dfin_F */),
                   new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("showtabpage", "tp_005"))}
                 });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveCutomer_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ActnEGrp_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _expn = ExpnBs1.Current as Data.Expense;
            if (_expn == null) return;

            var _grop = GropBs.Current as Data.Group_Expense;
            if (_grop == null) return;

            if (ModifierKeys.HasFlag(Keys.Control))
            {
               _expn.Group_Expense1 = null;
               iScsc.ExecuteCommand(string.Format("UPDATE dbo.Expense SET Grop_Code = NULL WHERE Code = {0};", _expn.CODE));
            }
            else
            {
               _expn.Group_Expense1 = _grop;
               iScsc.ExecuteCommand(string.Format("UPDATE dbo.Expense SET Grop_Code = {0} WHERE Code = {1};", _grop.CODE, _expn.CODE));
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OldRqstShowProfile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _OldRqst = OldRqstBs1.Current as Data.Request;
            if (_OldRqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _OldRqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExprData_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SelectExportContactFile_Butn.Tag == null)
            {
               SelectExportContactFile_Butn_Click(null, null);
               if (ExportFile_Sfd.FileName == null) return;
            }

            File.AppendAllText(ExportLabel_Txt.Text,
               string.Join(Environment.NewLine, OldRqstBs1.List.OfType<Data.Request>().Where(r => r.Request_Rows.Any(rr => rr.Fighter.CELL_PHON_DNRM.Length >= 8)).Select(rr => rr.Request_Rows.FirstOrDefault().Fighter.CELL_PHON_DNRM ))
            );

            MessageBox.Show(this, "شماره تلفن مشتریان در فایل مربوطه ذخیره شد", "ذخیره سازی اطلاعات", MessageBoxButtons.OK);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SelectExportContactFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ExportFile_Sfd.ShowDialog() != DialogResult.OK) return;
            ExportLabel_Txt.EditValue = SelectExportContactFile_Butn.Tag = ExportFile_Sfd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }      
   }
}
