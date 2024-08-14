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
using System.MaxUi;
using System.Globalization;
using DevExpress.XtraEditors;
using System.Scsc.ExtCode;
using System.Threading;

namespace System.Scsc.Ui.Notifications
{
   public partial class WHO_ARYU_F : UserControl
   {
      public WHO_ARYU_F()
      {
         InitializeComponent();

         //System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         //gp.AddEllipse(0, 0, Pb_FighImg.Width , Pb_FighImg.Height );
         //System.Drawing.Region rg = new System.Drawing.Region(gp);
         //Pb_FighImg.Region = rg;
      }

      private bool requery = false;
      private string hBDay = "", durtAttn = "", debtAlrm = "";

      private void Pb_FighImg_Paint(object sender, PaintEventArgs e)
      {
         //e.Graphics.DrawEllipse(
         //    new Pen(Color.White, 2f),
         //    0, 0, Pb_FighImg.Size.Width, Pb_FighImg.Size.Height);
      }

      private void Execute_Query(bool runAllQuery)
      {
         try
         {
            attnvist = 0;
            iScsc = new Data.iScscDataContext(ConnectionString);

            //DRES_NUMB_Txt.Focus();
            // 1395/12/11 * مشخص کردن نوع مبلغ برای بدهی
            if (Lbl_AmntType.Tag == null)
            {
               Lbl_AmntType.Text = (from r in iScsc.Regulations
                                    join a in iScsc.D_ATYPs on r.AMNT_TYPE ?? "001" equals a.VALU
                                    where r.REGL_STAT == "002"
                                       && r.TYPE == "001"
                                    select a).FirstOrDefault().DOMN_DESC;
               Lbl_AmntType.Tag = "Set Amnt Type From Regulation";
            }

            // 1396/07/16 * برای اینکه به حضوری مورد نظر دسترسی پیدا کنیم
            if (attncode != null)
            {
               AttnBs1.DataSource =
                     iScsc.Attendances.FirstOrDefault(a => a.CODE == attncode);
               return;
            }

            if (fileno != null)
            {
               if (AttnDate_Date.Value.HasValue)
               {
                  var result =
                     iScsc.Attendances
                     .Where(a => a.FIGH_FILE_NO == Convert.ToInt64(fileno)
                        && a.ATTN_DATE.Date == AttnDate_Date.Value.Value.Date
                        && a.MBSP_RWNO_DNRM == (mbsprwno == null ? a.MBSP_RWNO_DNRM : mbsprwno)
                        && a.EXIT_TIME == null
                      )
                     .OrderByDescending(a => a.CRET_DATE).FirstOrDefault();

                  if (result == null)
                     AttnBs1.DataSource =
                        iScsc.Attendances
                        .Where(a => a.FIGH_FILE_NO == Convert.ToInt64(fileno)
                           && a.ATTN_DATE.Date == AttnDate_Date.Value.Value.Date
                           && a.MBSP_RWNO_DNRM == (mbsprwno == null ? a.MBSP_RWNO_DNRM : mbsprwno)
                           && a.EXIT_TIME != null
                         )
                        .OrderByDescending(a => a.EXIT_TIME).FirstOrDefault();
                  else
                     AttnBs1.DataSource = result;
               }
               else
               {
                  AttnBs1.DataSource =
                     iScsc.Attendances
                     .Where(a => a.FIGH_FILE_NO == Convert.ToInt64(fileno)).OrderByDescending(a => a.EXIT_TIME);
               }
            }
            else
            {
               AttnBs1.DataSource =
                     iScsc.Attendances.OrderByDescending(a => a.EXIT_TIME);
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); MessageBox.Show("Execute_Query Error"); }
      }

      int attnvist = 0;
      private void AttnBs1_CurrentChanged(object sender, EventArgs e)
      {
         var _attn = AttnBs1.Current as Data.Attendance;
         if (_attn == null) return;

         if (attnvist > 0) return;
         attnvist++;

         if(_stng == null)
            _stng = iScsc.Settings.FirstOrDefault(s => Fga_Uclb_U.Contains(s.CLUB_CODE));

         // 1403/02/21 * اگر قفل انلاین میباشد شماره کمد دیگر قابل ویرایش نباشد
         DRES_NUMB_Txt.Properties.ReadOnly = _stng.DRES_AUTO == "002";
         // 1403/02/21 * نمایس شماره ردیف ورود و خروج
         AttnRwno_Txt.Text = _attn.RWNO.ToString();

         if (_attn.EXIT_TIME != null)
         {
            //Lbl_AccessControl.Text = "خروج از باشگاه";
            //Lbl_AccessControl.ForeColor = Color.White;
            //DRES_NUMB_Txt.Properties.ReadOnly = true;
            DresNumb_Butn.Enabled = false;
         }
         else
         {
            //Lbl_AccessControl.Text = "ورود مجاز است";
            //Lbl_AccessControl.ForeColor = Color.GreenYellow;
            //DRES_NUMB_Txt.Properties.ReadOnly = false;
            DresNumb_Butn.Enabled = true;

            // 1396/10/18 * آیا گزینه نمایش چاپ حضوری انجام شود یا خیر
            if (_attn.Fighter1.FGPB_TYPE_DNRM == "001" && _stng.ATTN_PRNT_STAT == "002")
            {
               // 1397/01/28 * برای آن دسته از ورود هایی که هنوز چاپ نشده اند
               if(_attn.PRNT_STAT != "002")
                  PrintDefault_Butn_Click(null, null);
            }
         }

         // 1402/10/21 * اگر سیستم قفل انلاین باشد
         SetDVip_Butn.Visible = OpenDVip_Butn.Visible = _stng.DRES_AUTO == "002";

         if(gateControl)
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("type", "gatecontrol"),
                              new XAttribute("gateactn", _attn.EXIT_TIME == null ? "open" : "close"),
                              new XAttribute("mtodcode", _attn.MTOD_CODE_DNRM ?? 0), // 1400/01/12 * برای اینکه بخواهیم به گیت مورد نظر همان ورزش درخواست دهیم
                              new XAttribute("enddate", _attn.MBSP_END_DATE_DNRM),
                              new XAttribute("numbattnmont", _attn.NUMB_OF_ATTN_MONT),
                              new XAttribute("sumattnmont",_attn.SUM_ATTN_MONT_DNRM ?? 0),
                              new XAttribute("debt", _attn.DEBT_DNRM),
                              new XAttribute("fngrprnt", _attn.FNGR_PRNT_DNRM)
                           )
                     }
                  }
               )
            );

         // 1395/12/11 * چک کردن وضعیت بدهی مشتری
         //if(attn.Fighter1.DEBT_DNRM > 0)
         if (_attn.Fighter1.DEBT_DNRM > 0)
         {
            DebtAmnt_Pn.Visible = true;
            DebtAmnt_Lb.Text = _attn.Fighter1.DEBT_DNRM.Value.ToString("n0");

            if(_attn.EXIT_TIME == null)
            {
               PlayDebtAlrm_Lb_Click(null, null);
               PlayDebtAlrm_Lb.Tag = "stop";
            }
         }
         //else if (attn.Fighter1.DEBT_DNRM == 0)
         else
         {
            DebtAmnt_Pn.Visible = false;
         }

         if (_attn.Fighter1.DPST_AMNT_DNRM > 0)
         {
            DpstAmnt_Pn.Visible = true;
            DpstAmnt_Lb.Text = _attn.Fighter1.DPST_AMNT_DNRM.Value.ToString("n0");
         }
         //else if (attn.Fighter1.DEBT_DNRM == 0)
         else
         {
            DpstAmnt_Pn.Visible = false;
         }

         AttnDate_Date.Value = _attn.ATTN_DATE;
         AttnDate_Lb.Text = AttnDate_Date.GetText("yyyy/MM/dd");
         if (_attn.FGPB_TYPE_DNRM == "001")
         {
            Mtod_Lb.Text = string.Format("{0} - {1}", _attn.Method.MTOD_DESC, _attn.Category_Belt.CTGY_DESC);
            CochProFile_Rb.Visible = true;
         }
         else
         {
            Mtod_Lb.Text = "پرسنل";
            CochName_Lb.Text = "";
            CochProFile_Rb.Visible = false;
         }
         if (_attn.NUMB_OF_ATTN_MONT == 0) NumbAttnMont_Lb.Visible = false;
         else NumbAttnMont_Lb.Visible = true;

         if (ServProFile_Rb.Tag == null || ServProFile_Rb.Tag.ToString().ToInt64() != _attn.FIGH_FILE_NO)
         {
            if (_attn.IMAG_RCDC_RCID_DNRM != null)
            {
               try
               {
                  ServProFile_Rb.ImageProfile = null;
                  MemoryStream mStream = new MemoryStream();
                  byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _attn.FIGH_FILE_NO))).ToArray();
                  mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                  Bitmap bm = new Bitmap(mStream, false);
                  mStream.Dispose();

                  if (InvokeRequired)
                     Invoke(new Action(() => ServProFile_Rb.ImageProfile = bm));
                  else
                     ServProFile_Rb.ImageProfile = bm;

                  ServProFile_Rb.Tag = _attn.FIGH_FILE_NO;
               }
               catch { }
            }
            else
            {
               ServProFile_Rb.ImageProfile = null;
               ServProFile_Rb.Tag = null;
            }
         }

         /////////////// Show Coach Profile
         if (CochProFile_Rb.Tag == null || CochProFile_Rb.Tag.ToString().ToInt64() != _attn.COCH_FILE_NO)
         {
            if (_attn.Fighter != null && _attn.Fighter.IMAG_RCDC_RCID_DNRM != null)
            {
               try
               {
                  CochProFile_Rb.ImageProfile = null;
                  MemoryStream mStream = new MemoryStream();
                  byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _attn.COCH_FILE_NO))).ToArray();
                  mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                  Bitmap bm = new Bitmap(mStream, false);
                  mStream.Dispose();

                  if (InvokeRequired)
                     Invoke(new Action(() => CochProFile_Rb.ImageProfile = bm));
                  else
                     CochProFile_Rb.ImageProfile = bm;

                  CochProFile_Rb.Tag = _attn.COCH_FILE_NO;
               }
               catch { }
            }
            else
            {
               CochProFile_Rb.ImageProfile = null;
               CochProFile_Rb.Tag = null;
            }
         }

         PersianCalendar pc = new PersianCalendar();
         StrtDate_Lb.Text = string.Format("{0}/{1}/{2}", pc.GetYear((DateTime)_attn.MBSP_STRT_DATE_DNRM), pc.GetMonth((DateTime)_attn.MBSP_STRT_DATE_DNRM), pc.GetDayOfMonth((DateTime)_attn.MBSP_STRT_DATE_DNRM));
         EndDate_Lb.Text = string.Format("{0}/{1}/{2}", pc.GetYear((DateTime)_attn.MBSP_END_DATE_DNRM), pc.GetMonth((DateTime)_attn.MBSP_END_DATE_DNRM), pc.GetDayOfMonth((DateTime)_attn.MBSP_END_DATE_DNRM));
         BrthDate_Lb.Text = string.Format("{0}/{1}/{2}", pc.GetYear((DateTime)_attn.BRTH_DATE_DNRM), pc.GetMonth((DateTime)_attn.BRTH_DATE_DNRM), pc.GetDayOfMonth((DateTime)_attn.BRTH_DATE_DNRM));

         if (_attn.Fighter1.INSR_DATE_DNRM != null)
         {
            InsrDate_Lb.Text = string.Format("{0}/{1}/{2}", pc.GetYear((DateTime)_attn.Fighter1.INSR_DATE_DNRM), pc.GetMonth((DateTime)_attn.Fighter1.INSR_DATE_DNRM), pc.GetDayOfMonth((DateTime)_attn.Fighter1.INSR_DATE_DNRM));
            InsrRmnd_Lb.Text = (_attn.Fighter1.INSR_DATE_DNRM.Value.Date - DateTime.Now.Date).Days.ToString();
         }
         else
         {
            InsrDate_Lb.Text = "بیمه ندارد";
            InsrRmnd_Lb.Text = "";
         }


         if (iScsc.GET_MTOS_U((DateTime?)_attn.ATTN_DATE.Date).Substring(5) == BrthDate_Lb.Text.Substring(5) && BrthDate_Lb.Text.Substring(0, 4) != pc.GetYear(DateTime.Now).ToString())
         {
            PlayHappyBirthDate_Butn.Visible = true;
            if (_attn.EXIT_TIME == null && !iScsc.Log_Operations.Any(l => l.FIGH_FILE_NO == _attn.FIGH_FILE_NO && l.LOG_TYPE == "010" && l.CRET_DATE.Value.Date == DateTime.Now.Date))
            {
               iScsc.INS_LGOP_P(
                  new XElement("Log",
                      new XAttribute("fileno", _attn.FIGH_FILE_NO),
                      new XAttribute("type", "010"), 
                      new XAttribute("text", string.Format("پخش آهنگ تبریک تولد برای " + "{0}", _attn.NAME_DNRM))
                  )
               );
               PlayHappyBirthDate_Butn.Tag = "stop";
               PlayHappyBirthDate_Butn_Click(null, null);
            }
         }
         else
            PlayHappyBirthDate_Butn.Visible = false;

         // 1401/11/25 * برای اجرای مدت زمان کلاس
         if(_attn.Club_Method != null && _attn.Club_Method.DURT_ATTN_SOND_PATH != "")
         {
            durtAttn = _attn.Club_Method.DURT_ATTN_SOND_PATH;
            if(_attn.EXIT_TIME == null)
            {
               PlayDurtAttn_Butn.Tag = "stop";
               PlayDurtAttn_Butn_Click(null, null);
            }
         }

         //if(attn.EXIT_TIME == null)
         //   AttnType_Lab.ImageKey = "IMAGE_1106.png";
         //else
         //   AttnType_Lab.ImageKey = "IMAGE_1058.png";

         if (ServProFile_Rb.ImageProfile == null && _attn.Fighter1.SEX_TYPE_DNRM == "002")
            ServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1148;
         else if(ServProFile_Rb.ImageProfile == null)
            ServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1149;

         if (_attn.Fighter != null)
         {
            CochProFile_Rb.Visible = true;
            if (CochProFile_Rb.ImageProfile == null && _attn.Fighter.SEX_TYPE_DNRM == "002")
               CochProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1507;
            else if (CochProFile_Rb.ImageProfile == null)
               CochProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1076;
         }
         else
            CochProFile_Rb.Visible = false;

         //if (attn.Fighter1.FGPB_TYPE_DNRM == "002" || attn.Fighter1.FGPB_TYPE_DNRM == "003")
         if (_attn.FGPB_TYPE_DNRM == "002" || _attn.FGPB_TYPE_DNRM == "003")
         {
            //***FighterType_Lab.ImageKey = "IMAGE_1126.png";
            //Figh00156.Visible = false;
            //Sesn_Pn.Visible = false;
            //PrivSesn_Pn.Visible = false;
            //PrivateSession_Lab.Visible = false;
            //PrivateSession_Lab.Image = null;
            if (_attn.FGPB_TYPE_DNRM == "002")
            {
               //panel1.Visible = false;
               return;
            }
            //Ctgy_Lb.Visible = false;
         }
         else
         {
            //Ctgy_Lb.Visible = true;
         }
         //***else if (attn.Fighter1.FGPB_TYPE_DNRM == "008")
         //***   FighterType_Lab.ImageKey = "IMAGE_1087.png";
         //***else
         //***   FighterType_Lab.ImageKey = "IMAGE_1115.png";
         //panel1.Visible = true;
         //Figh00156.Visible = true;
         //Sesn_Pn.Visible = false;
         
         //if(attn.Fighter1.MBCO_RWNO_DNRM != null && (attn.Fighter1.FGPB_TYPE_DNRM != "002"))
         

         /*MbspBs1.DataSource = 
            iScsc.Member_Ships
            .Where(m => m.FIGH_FILE_NO == attn.FIGH_FILE_NO
                && m.RWNO == attn.Fighter1.MBSP_RWNO_DNRM
                && m.RECT_CODE == "004");*/

         /*var dsat = attn.Dresser_Attendances.FirstOrDefault();
         if (dsat != null)
         {
            Lbl_Dresser.Visible = true;
            Lbl_Dresser.Text = dsat.Dresser.DRES_NUMB.ToString();
            Lbl_Dresser.Image = System.Scsc.Properties.Resources.IMAGE_1230;
         }
         else
         {
            Lbl_Dresser.Visible = false;
            Lbl_Dresser.Text = "";
         }*/

         MbspStat_Rb.NormalColorA = Color.GreenYellow;
         MbspStat_Rb.NormalColorB = Color.GreenYellow;
         if (_attn.MBSP_END_DATE_DNRM.Value.AddDays(-1).Date >= DateTime.Now.Date &&
            _attn.MBSP_END_DATE_DNRM.Value.AddDays(-3).Date <= DateTime.Now.Date)
         {
            MbspStat_Rb.NormalColorA = Color.Yellow;
            MbspStat_Rb.NormalColorB = Color.Yellow;
         }
         else if (_attn.MBSP_END_DATE_DNRM.Value.Date == DateTime.Now.Date)
         {
            MbspStat_Rb.NormalColorA = Color.Red;
            MbspStat_Rb.NormalColorB = Color.Red;
         }
         ///***
         //if(attn.Member_Ship.Sessions.Any())
         //   SesnBs1.DataSource =
         //      iScsc.Sessions
         //      .Where(s => s.Member_Ship == attn.Member_Ship);

         //if(attn.EXIT_TIME != null)
         //{
         //   var diff = attn.EXIT_TIME.Value.Subtract(attn.ENTR_TIME.Value);
         //   TotlAttnTime_Lbl.Text = string.Format("{0}:{1}", diff.Hours, diff.Minutes); ;
         //}
         //else
         //{
         //   var diff = DateTime.Now.Subtract(attn.ENTR_TIME.Value);
         //   TotlAttnTime_Lbl.Text = string.Format("{0}:{1}",  diff.Hour, diff.Minute);
         //}
         if (_attn.COCH_FILE_NO != null)
            CochName_Lb.Text = _attn.Fighter.NAME_DNRM;
         else
            CochName_Lb.Text = "";
         
         //if(attn.CBMT_CODE_DNRM != null)
         //{
         //   var cmbt = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == attn.CBMT_CODE_DNRM);
         //   StrtTime_Lb.Text = cmbt.STRT_TIME.ToString().Substring(0, 5);
         //   EndTime_Lb.Text = cmbt.END_TIME.ToString().Substring(0, 5);            
         //}
         //else
         //{
         //   StrtTime_Lb.Text = "";
         //   EndTime_Lb.Text = "";
         //}

         StrtTime_Lb.Text = _attn.ENTR_TIME.ToString().Substring(0, 5);
         if (_attn.EXIT_TIME != null)
         {
            EndTime_Lb.Text = _attn.EXIT_TIME.ToString().Substring(0, 5);
            EndTime_Lb.Visible = true;
         }
         else
            EndTime_Lb.Visible = false;

         // 1400/01/08 * نمایش دکمه یاد آوری
         var notes = _attn.Fighter1.Notes.Where(n => n.VIST_STAT == "001");
         if(notes != null && notes.Count() > 0)
         {
            Note_Lb.Visible = true;
            NotfNote_Butn.Visible = true;
            NotfNote_Butn.Caption = notes.Count().ToString();
            this.toolTip1.SetToolTip(this.Note_Lb, string.Join(Environment.NewLine, notes.OrderBy(n => n.RWNO).Select(n => n.RWNO.ToString() + ") " + n.NOTE_SUBJ + Environment.NewLine + n.NOTE_CMNT)));
         }
         else
         {
            Note_Lb.Visible = false;
            NotfNote_Butn.Visible = false;
            NotfNote_Butn.Caption = "0";
         }

         DRES_NUMB_Txt.Focus();

         RemindAttn_Lb.Text = Math.Abs( (int)(_attn.NUMB_OF_ATTN_MONT - _attn.SUM_ATTN_MONT_DNRM ?? 0) ).ToString();
         TotlNumbAttn_Lb.Text = _attn.SUM_ATTN_MONT_DNRM.ToString();
         DayRmnd_Lb.Text = (_attn.MBSP_END_DATE_DNRM.Value.Date - DateTime.Now.Date).Days.ToString();

         // 1400/09/17 * مشخص شدن آیتم های درآمدی اعتباری
         var pydts = iScsc.Payment_Details.Where(pd => pd.EXPR_DATE != null && pd.Request_Row.FIGH_FILE_NO == _attn.FIGH_FILE_NO && pd.Request_Row.Request.RQST_STAT == "002");
         PivBs.DataSource = pydts.Where(pd => pd.EXPR_DATE.Value.Date >= DateTime.Now && pd.EXPR_DATE.Value.Date != pd.CRET_DATE.Value.Date);
         PinvBs.DataSource = pydts.Where(pd => pd.EXPR_DATE.Value.Date < DateTime.Now && pd.EXPR_DATE.Value.Date != pd.CRET_DATE.Value.Date);

         // 1401/07/23 * روز سرگونی حکومت کثیف آخوندی
         PdtMBs.DataSource = iScsc.Payment_Details.Where(pd => pd.MBSP_FIGH_FILE_NO == _attn.FIGH_FILE_NO && pd.MBSP_RECT_CODE == "004" && pd.MBSP_RWNO == _attn.MBSP_RWNO_DNRM);

         // 1403/01/18 * اگر مشتری دستبند از باشگاه گرفته باشد باید با یه پیام هشدار به منشی ها اعلام کنیم که دستبند ها رو دریافت کنند
         if(_attn.EXIT_TIME != null && _attn.Attendance_Wrists.Any(aw => aw.STAT == "001"))
         {
            Info_Tb.SelectedTabPage = Tp_DefWrst;
            wplayer.URL = @".\Media\SubSys\Kernel\Desktop\Sounds\Illuminate.mp3";
            PlaySound();
         }

         DoBkg_Tr.Enabled = true;
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void MoreInfo_Butn_Click(object sender, EventArgs e)
      {
         //string fc = formcaller;
         formcaller = "";

         RqstBnExit1_Click(null, null);
         var attn = AttnBs1.Current as Data.Attendance;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", attn.FIGH_FILE_NO)) }
         );
      }

      private void Butn_TakePicture_Click(object sender, EventArgs e)
      {
          try
          {
              if (true)
              {
                  var rqst = (from r in iScsc.Requests
                             join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                             where rr.FIGH_FILE_NO == Convert.ToInt64(fileno)
                                && r.RQTP_CODE == "001"
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

      //bool zoomed = false;
      private void Butn_Zoom_Click(object sender, EventArgs e)
      {

      }

      private void DresNumb_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            AttnBs1.EndEdit();

            var attn = AttnBs1.Current as Data.Attendance;

            if (attn == null) return;
            if (attn.DERS_NUMB == null) return;

            iScsc.SubmitChanges();

            //Lbl_Dresser.BackColor = Color.YellowGreen;
            requery = true;
         }
         catch (Exception exc)
         {
            //Lbl_Dresser.BackColor = Color.Tomato;
            //MsgBox.Show(exc.Message, "خطای ثبت کمد شماره کمد", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, exc.Message, "خطای ثبت کمد شماره کمد", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if(requery)
            {
               RqstBnExit1_Click(null, null);
               //Execute_Query(true);
               requery = false;
            }
         }
      }

      private void DRES_NUMB_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         if(e.KeyCode == Keys.Enter)
         {
            DresNumb_Butn_Click(null, null);
         }
      }

      private void AttnPartner_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            AttnBs1.EndEdit();

            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;

            // ذخیره کردن شماره کمد و توضیحات ورودی
            iScsc.SubmitChanges();

            var chosbutn = MessageBox.Show(this, "حضوری همراه به صورت هزینه دار با کسر جلسه برای عضو ثبت شود؟", "ثبت حضوری همراه", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
            switch (chosbutn)
	         {
		         case DialogResult.Cancel:
                  return;
               case DialogResult.No:
                  iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, attn.ATTN_DATE, attn.COCH_FILE_NO, "008", attn.MBSP_RWNO_DNRM, "001", "002");
                  break;
               case DialogResult.Yes:
                  iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, attn.ATTN_DATE, attn.COCH_FILE_NO, "007", attn.MBSP_RWNO_DNRM, "001", "002");
                  break;               
	         }
            requery = true;
            AttnDate_Date.Value = attn.ATTN_DATE;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               attncode = null;
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void SaveAttnDesc_Txt_Click(object sender, EventArgs e)
      {
         try
         {
            AttnBs1.EndEdit();

            var attn = AttnBs1.Current as Data.Attendance;

            if (attn == null) return;

            iScsc.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            //MsgBox.Show(exc.Message, "برای ثبت اطلاعات توضیحات با خطا مواجه شد", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, exc.Message, "برای ثبت اطلاعات توضیحات با خطا مواجه شد", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void PrintDefault_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;
            
            string fc = formcaller;
            formcaller = "";

            iScsc.UPD_ATNP_P(
               new XElement("Attendance",
                  new XAttribute("code", attn.CODE)
               )
            );

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProccessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("a.Code = {0}", attn.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            formcaller = fc;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); MessageBox.Show("Print Default Error"); }
      }

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;

            string fc = formcaller;
            formcaller = "";

            iScsc.UPD_ATNP_P(
               new XElement("Attendance",
                  new XAttribute("code", attn.CODE)
               )
            );

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProccessCmdKey */){Input = Keys.Escape},
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("a.Code = {0}", attn.CODE))}
                    });
            _DefaultGateway.Gateway(_InteractWithScsc);
            
            formcaller = fc;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); MessageBox.Show("Print Error"); }
      }

      private void PrintSetting_Butn_Click(object sender, EventArgs e)
      {
         string fc = formcaller;
         formcaller = "";
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {
                    new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProccessCmdKey */){Input = Keys.Escape},
                    new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                    new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                 });
         _DefaultGateway.Gateway(_InteractWithScsc);

         formcaller = fc;
      }

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {

      }

      private void DebtDnrm_Lb_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", fileno)                               
                           )
                     },
                     new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", fileno),
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
            MessageBox.Show("DebtDnrm Error");
         }
      }

      private void MoveLastItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            if(attn == null)return;

            var _qury = iScsc.Attendances.Where(a => a.ATTN_DATE == attn.ATTN_DATE && Fga_Uclb_U.Contains(a.CLUB_CODE) && a.ATTN_STAT == "002").OrderBy(a => a.ENTR_TIME).FirstOrDefault();            

            if (_qury == null) return;

            attncode = _qury.CODE;
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            MessageBox.Show("Move Last Item Error");
         }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      private void MoveNextItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;

            var _qury = iScsc.Attendances.Where(a => a.ATTN_DATE == attn.ATTN_DATE && Fga_Uclb_U.Contains(a.CLUB_CODE) && a.ATTN_STAT == "002" && a.CODE != attn.CODE && a.ENTR_TIME <= attn.ENTR_TIME).OrderByDescending(a => a.ENTR_TIME).FirstOrDefault();

            if (_qury == null) return;

            attncode = _qury.CODE;
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            MessageBox.Show("Move Next Item Error");
         }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      private void MovePreviousItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;

            var _qury = iScsc.Attendances.Where(a => a.ATTN_DATE == attn.ATTN_DATE && Fga_Uclb_U.Contains(a.CLUB_CODE) && a.ATTN_STAT == "002" && a.CODE != attn.CODE && a.ENTR_TIME >= attn.ENTR_TIME).OrderBy(a => a.ENTR_TIME).FirstOrDefault();

            if (_qury == null) return;

            attncode = _qury.CODE;
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            MessageBox.Show("Move Perv Item Error");
         }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      private void MoveFirstItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;

            var _qury = iScsc.Attendances.Where(a => a.ATTN_DATE == attn.ATTN_DATE && Fga_Uclb_U.Contains(a.CLUB_CODE) && a.ATTN_STAT == "002").OrderByDescending(a => a.ENTR_TIME).FirstOrDefault();

            if (_qury == null) return;

            attncode = _qury.CODE;
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      private void OwnrCbmtCode_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Attendance SET Ownr_Cbmt_Code_Dnrm = {0} WHERE Code = {1};", e.NewValue, _attn.CODE));
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            MessageBox.Show("Ownr Cbmt Code Error");
         }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      private void PlayHappyBirthDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            if(hBDay == "")
            {
               if (_stng == null)
                  _stng = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
               hBDay = _stng.SND7_PATH;               
            }

            if (hBDay == null || hBDay.Trim() == "") return;

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
            MessageBox.Show("Play Happy Day Error");
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

      private void PlayDurtAttn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (durtAttn == null || durtAttn.Trim() == "") return;

            wplayer.URL = durtAttn;
            switch (PlayDurtAttn_Butn.Tag.ToString())
            {
               case "stop":
                  PlayDurtAttn_Butn.Tag = "play";
                  new Threading.Thread(PlaySound).Start();
                  break;
               case "play":
                  PlayDurtAttn_Butn.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            MessageBox.Show("Play Durt Attn Error");
         }
      }

      private void PlayDebtAlrm_Lb_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            if (debtAlrm == "")
            {
               if (_stng == null)
                  _stng = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
               debtAlrm = _stng.SND9_PATH;
            }

            if (debtAlrm == null || debtAlrm.Trim() == "") return;

            wplayer.URL = debtAlrm;
            switch (PlayDebtAlrm_Lb.Tag.ToString())
            {
               case "stop":
                  PlayDebtAlrm_Lb.Tag = "play";
                  new Threading.Thread(PlaySound).Start();
                  break;
               case "play":
                  PlayDebtAlrm_Lb.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            MessageBox.Show("Play Debt Alrm Error");
         }
      }

      private void DoBkg_Tr_Tick(object sender, EventArgs e)
      {
         try
         {
            // First Calculate Current Attendance 
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            int h = (int)(DateTime.Now.TimeOfDay.TotalHours - _attn.ENTR_TIME.Value.TotalHours);
            int m = Math.Abs((int)(DateTime.Now.TimeOfDay.TotalHours - _attn.ENTR_TIME.Value.TotalHours) * 60 - (int)(DateTime.Now.TimeOfDay.TotalMinutes - _attn.ENTR_TIME.Value.TotalMinutes));

            if(_attn.EXIT_TIME != null)
            {
               h = (int)(_attn.EXIT_TIME.Value.TotalHours - _attn.ENTR_TIME.Value.TotalHours);
               m = Math.Abs((int)(_attn.EXIT_TIME.Value.TotalHours - _attn.ENTR_TIME.Value.TotalHours) * 60 - (int)(_attn.EXIT_TIME.Value.TotalMinutes - _attn.ENTR_TIME.Value.TotalMinutes));
            }

            if (m >= 60)
            {
               h += m / 60;
               m = m % 60;
            }

            TotlCrntAttn_Lb.Text = string.Format("{0} : {1}", h, m);

            // Load All Attendance Cycle
            AllCyclAttnBs1.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == _attn.FIGH_FILE_NO && a.MBSP_RWNO_DNRM == _attn.MBSP_RWNO_DNRM && a.ATTN_STAT == "002" && a.EXIT_TIME != null /*&& a.CODE != _attn.CODE*/);
            var _attns = AllCyclAttnBs1.List.OfType<Data.Attendance>();

            TotlCyclAttn_Lb.Visible = false;

            if(_attns.Count() >= 1)
            {
               h = _attns.Sum(a => (int)(a.EXIT_TIME.Value.TotalHours - a.ENTR_TIME.Value.TotalHours));
               m = Math.Abs(_attns.Sum(a => (int)(a.EXIT_TIME.Value.TotalHours - a.ENTR_TIME.Value.TotalHours) * 60 - (int)(a.EXIT_TIME.Value.TotalMinutes - a.ENTR_TIME.Value.TotalMinutes)));

               if(m >= 60)
               {
                  h += m / 60;
                  m = m % 60;
               }
            
               TotlCyclAttn_Lb.Text = string.Format("{0} : {1}\n\r{2} : {3}\n\r{4} : {5}", "جلسات", _attns.Count(), "ساعت", h, "دقیقه", m);
               TotlCyclAttn_Lb.Visible = true;
               Infos_Ro.RolloutStatus = true;
            }

            // Check IF Service Have Shop Order
            Shop_Rbtn.Visible = 
               iScsc
               .Request_Rows
               .Any(rr => 
                  rr.FIGH_FILE_NO == _attn.FIGH_FILE_NO && 
                  rr.RQTP_CODE == "016" && 
                  rr.Request.RQST_STAT == "002" && 
                  rr.Request.SAVE_DATE.Value.Date == _attn.ATTN_DATE.Date
               );

            // Check IF Service Have SMS
            Sms_Rbtn.Visible = 
               iScsc
               .V_Sms_Message_Boxes
               .Any(s =>
                  s.PHON_NUMB == _attn.CELL_PHON_DNRM
               );

            // 1402/10/21 * بار گذاری کمدهایی که میتوان به مشتریان داد
            DresBs.DataSource = iScsc.Dressers.Where(d => d.VIP_STAT == "001" && d.REC_STAT == "002" && !d.Dresser_Attendances.Any(da => da.Attendance.EXIT_TIME == null) && !d.Dresser_Vip_Fighters.Any(dv => dv.STAT == "002"));
            DratGv.ActiveFilterString = "Tkbk_Time IS NULL";

            // 1403/01/18 * اگر مشتری دستبند از باشگاه گرفته باشد باید با یه پیام هشدار به منشی ها اعلام کنیم که دستبند ها رو دریافت کنند
            if (_attns.Any(a => a.Attendance_Wrists.Any(aw => aw.STAT == "001")))
            {
               Tb_WristInfo.SelectedTabPage = Tp_OldGetWrist;
               OldAttnGetWrstBs.DataSource = AllCyclAttnBs1.List.OfType<Data.Attendance>().Any(a => a.CODE != _attn.CODE && a.Attendance_Wrists.Any(aw => aw.STAT == "001"));
               wplayer.URL = @".\Media\SubSys\Kernel\Desktop\Sounds\Illuminate.mp3";
               PlaySound();
            }
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
            //MessageBox.Show("Do Bkg Error");
         }
         finally
         {
            DoBkg_Tr.Enabled = false;
         }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            if (ModifierKeys.HasFlag(Keys.Control))
            {
               
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                  
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Process", new XAttribute("type", "fngrdevlst"), new XAttribute("fngrprnt", _attn.FNGR_PRNT_DNRM))}
                     })
               );
               return;
            }

            RqstBnDeleteFngrPrnt1_Click(null, null);            

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", _attn.FNGR_PRNT_DNRM))}
                  })
            );
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", _attn.FNGR_PRNT_DNRM))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", _attn.FNGR_PRNT_DNRM))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", _attn.FNGR_PRNT_DNRM)
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
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", _attn.FNGR_PRNT_DNRM)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }
      #endregion

      private void SetDVip_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if(_attn == null && _attn.Dresser_Attendances == null)return;
            
            var _dvipcode = _attn.Dresser_Attendances.FirstOrDefault(d => d.DRAT_CODE == null).CODE;

            bool _freelockvip = false;
            var _lockbydvip = iScsc.Dresser_Vip_Fighters.FirstOrDefault(d => d.DRES_CODE == _dvipcode && d.STAT == "002" && d.MBSP_FIGH_FILE_NO != _attn.FIGH_FILE_NO);
            if(_lockbydvip != null)
            {
               if (MessageBox.Show(this, string.Format("در حال حاضر کمد در اختیار {0} مباشد آیا با آزاد کردن کمد موافق هستید?", _lockbydvip.Fighter.NAME_DNRM), "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
               _freelockvip = true;
            }

            // اگر مشتری دارای کمد اختصاصی یا اجاره ای باشه نباید کمد دیگری به آن داده شود
            if(iScsc.Dresser_Vip_Fighters.Any(d => d.MBSP_FIGH_FILE_NO == _attn.FIGH_FILE_NO && d.STAT == "002"))
            {
               MessageBox.Show(this, "مشتری دارای کمد اختصاصی یا اجاره ای میباشد، شما قادر به اختصاص کمد جدید به این مشتری نیستید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }

            // 1402/10/10 * بررسی اینکه آیا این کمد برای این دوره درست انتخاب شده یا خیر
            var _edlm = iScsc.External_Device_Link_Methods;
            if(_edlm.Any())
            {
               if(!_edlm.Any(i => i.MTOD_CODE == _attn.Member_Ship.FGPB_MTOD_CODE_DNRM && iScsc.Dressers.Any(d => d.CODE == _dvipcode && d.IP_ADRS == i.External_Device.IP_ADRS)))
               {
                  MessageBox.Show(this, "کمد انتخاب شده برای این گروه خدمات تعریف نشده، لطفا اصلاح کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }
            }

            if(_freelockvip)
               iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser_Vip_Fighter SET Stat = '001' WHERE Code = {0};", _lockbydvip.CODE));
            iScsc.ExecuteCommand("INSERT INTO dbo.Dresser_Vip_Fighter (Dres_Code, Mbsp_Figh_File_No, Mbsp_Rwno, Mbsp_Rect_Code, Code, Stat) VALUES ({0}, {1}, {2}, '004', 0, '002');", _dvipcode, _attn.FIGH_FILE_NO, _attn.Member_Ship.RWNO);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
               {
                  Input =
                     new List<object>
                     {
                        ToolTipIcon.Info,
                        "کمد به مشتری اختصاص داده شد",
                        "اختصاص کمد به مشتری",
                        2000
                     }
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OpenDVip_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            var dres = _attn.Dresser_Attendances.FirstOrDefault().Dresser as Data.Dresser;
            if (dres == null) return;

            if (MessageBox.Show(this, "آیا با باز کردن کمد موافق هستید؟", "تایید فعالیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            // 1402/08/30 * User access to open online locker
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
                                 "<Privilege>272</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 272 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region Dowork
                     new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("OprtDres",
                                 new XAttribute("type", "sendoprtdres"),
                                 new XAttribute("cmndname", dres.DRES_NUMB),
                                 new XAttribute("devip", dres.IP_ADRS),
                                 new XAttribute("cmndsend", dres.CMND_SEND ?? "")
                                 )
                     }
                     #endregion
                  })
            );

            // 1402/10/21 * باز کردن کمدهای همراهان
            if (iScsc.Dresser_Attendances.Any(da => da.ATTN_CODE == _attn.CODE && da.DRAT_CODE != null))
            {
               new Thread(new ThreadStart(() => OpenDresPart_Tmr_Tick(_attn.CODE))).Start();
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OpenDresPart_Tmr_Tick(long attncode)
      {
         try
         {
            foreach (var _dres in iScsc.Dresser_Attendances.Where(da => da.ATTN_CODE == attncode && da.DRAT_CODE != null).OrderBy(d => d.DERS_NUMB))
            {
               Thread.Sleep(4000);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                  {
                     Input =
                        new XElement("OprtDres",
                           new XAttribute("type", "sendoprtdres"),
                           new XAttribute("cmndname", _dres.Dresser.DRES_NUMB),
                           new XAttribute("devip", _dres.Dresser.IP_ADRS),
                           new XAttribute("cmndsend", _dres.Dresser.CMND_SEND ?? "")
                        )
                  }
               );
            }
         }
         catch { }
      } 

      private void DresActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _dres = DresBs.Current as Data.Dresser;
            if (_dres == null) return;

            var _attn = AttnBs1.Current as Data.Attendance;

            attncode = _attn.CODE;

            switch (e.Button.Index)
            {               
               case 0:
                  // 1402/08/30 * User access to open online locker
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
                                       "<Privilege>272</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 272 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region Dowork
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input =
                                 new XElement("OprtDres",
                                       new XAttribute("type", "sendoprtdres"),
                                       new XAttribute("cmndname", _dres.DRES_NUMB),
                                       new XAttribute("devip", _dres.IP_ADRS),
                                       new XAttribute("cmndsend", _dres.CMND_SEND ?? "")
                                       )
                           }
                           #endregion
                        })
                  );
                  break;
               case 1:
                  //var _dvipcode = _attn.Dresser_Attendances.FirstOrDefault(d => d.DRAT_CODE == null).DRES_CODE;
                  var _mbsp = _attn.Member_Ship;            

                  bool _freelockvip = false;
                  var _lockbydvip = iScsc.Dresser_Vip_Fighters.FirstOrDefault(d => d.DRES_CODE == _dres.CODE && d.STAT == "002" && d.MBSP_FIGH_FILE_NO != _attn.FIGH_FILE_NO);
                  if(_lockbydvip != null)
                  {
                     if (MessageBox.Show(this, string.Format("در حال حاضر کمد در اختیار {0} مباشد آیا با آزاد کردن کمد موافق هستید?", _lockbydvip.Fighter.NAME_DNRM), "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
                     _freelockvip = true;
                  }

                  //// اگر مشتری دارای کمد اختصاصی یا اجاره ای باشه نباید کمد دیگری به آن داده شود
                  //if(iScsc.Dresser_Vip_Fighters.Any(d => d.MBSP_FIGH_FILE_NO == _attn.FIGH_FILE_NO && d.STAT == "002"))
                  //{
                  //   MessageBox.Show(this, "مشتری دارای کمد اختصاصی یا اجاره ای میباشد، شما قادر به اختصاص کمد جدید به این مشتری نیستید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  //   return;
                  //}

                  // 1402/10/10 * بررسی اینکه آیا این کمد برای این دوره درست انتخاب شده یا خیر
                  var _edlm = iScsc.External_Device_Link_Methods;
                  if(_edlm.Any())
                  {
                     if(!_edlm.Any(i => i.MTOD_CODE == _mbsp.FGPB_MTOD_CODE_DNRM && iScsc.Dressers.Any(d => d.CODE == _dres.CODE && d.IP_ADRS == i.External_Device.IP_ADRS)))
                     {
                        MessageBox.Show(this, "کمد انتخاب شده برای این گروه خدمات تعریف نشده، لطفا اصلاح کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                     }
                  }

                  if(_freelockvip)
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser_Vip_Fighter SET Stat = '001' WHERE Code = {0};", _lockbydvip.CODE));
                  iScsc.ExecuteCommand("INSERT INTO dbo.Dresser_Attendance (Dres_Code, Attn_Code, Figh_File_No, Code, Drat_Code, Ders_Numb, Lend_Time) SELECT {0}, {1}, {2}, 0, {4}, {3}, CAST(GETDATE() AS TIME(0)) WHERE NOT EXISTS (SELECT 0 FROM dbo.Dresser_Attendance WHERE Dres_Code = {0} AND Attn_Code = {1});", _dres.CODE, _attn.CODE, fileno, _dres.DRES_NUMB, _attn.Dresser_Attendances.FirstOrDefault(d => d.DRAT_CODE == null).CODE);
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
               Execute_Query(true);
         }
      }

      private void ActnDrat_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _drat = DratBs.Current as Data.Dresser_Attendance;
            if (_drat == null || _drat.DRAT_CODE == null) return;

            var _dres = _drat.Dresser;

            attncode = _drat.ATTN_CODE;

            switch (e.Button.Index)
            {
               case 0:
                  // 1402/08/30 * User access to open online locker
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
                                       "<Privilege>272</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 272 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region Dowork
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input =
                                 new XElement("OprtDres",
                                       new XAttribute("type", "sendoprtdres"),
                                       new XAttribute("cmndname", _dres.DRES_NUMB),
                                       new XAttribute("devip", _dres.IP_ADRS),
                                       new XAttribute("cmndsend", _dres.CMND_SEND ?? "")
                                       )
                           }
                           #endregion
                        })
                  );
                  break;
               case 1:
                  iScsc.ExecuteCommand("DELETE dbo.Dresser_Attendance WHERE Attn_Code = {0} AND Ders_Numb = '{0}';", _drat.ATTN_CODE, _drat.DERS_NUMB);
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
               Execute_Query(true);
         }
      }

      private void WristBand_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            if (WristBand_Txt.EditValue == null || WristBand_Txt.EditValue.ToString() == "") return;

            string _tmp = "";
            attncode = _attn.CODE;

            switch (e.Button.Index)
            {
               case 0:
                  if (!AtnwBs.List.OfType<Data.Attendance_Wrist>().Any(aw => aw.Fighter1.FNGR_PRNT_DNRM == WristBand_Txt.Text))
                  {
                     var _wrst = iScsc.Fighters.FirstOrDefault(w => w.FNGR_PRNT_DNRM == WristBand_Txt.Text);
                     if (_wrst == null) return;

                     if (!AutoAddToList_Cbx.Checked && MessageBox.Show(this, "آیا با اضافه کردن دستبند به لیست موافق هستید؟", "عملیات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
                     
                     iScsc.ExecuteCommand(string.Format("INSERT INTO dbo.Attendance_Wrist(Attn_Code, Atnw_Figh_File_No, Code, Atnw_Fngr_Prnt_Dnrm) VALUES ({0}, {1}, 0, '{2}');", _attn.CODE, _wrst.FILE_NO, _wrst.FNGR_PRNT_DNRM));

                     // اگر تعداد دستبند هایی که داده میوشد بیشتر از یک عدد باشد باید تعداد جلسات بیشتری از مشتری کم شود
                     if (AtnwBs.Count >= 1)
                     {
                        iScsc.ExecuteCommand(
                           string.Format(
                              "UPDATE dbo.Attendance SET SUM_ATTN_MONT_DNRM += 1 WHERE CODE = {0};" + Environment.NewLine +
                              "UPDATE ms SET ms.SUM_ATTN_MONT_DNRM += 1 FROM dbo.Member_Ship ms, dbo.Attendance a WHERE a.Code = {0} AND a.MBSP_RWNO_DNRM = ms.RWNO AND a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND ms.RECT_CODE = '004';", 
                           _attn.CODE)
                        );

                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                               new XAttribute("fileno", _attn.FIGH_FILE_NO),
                               new XAttribute("type", "011"),
                               new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _attn.NAME_DNRM + " یک جلسه به صورت دستی بابت دستبند اضافه کم کرد")
                           )
                        );
                     }
                  }
                  else
                  {
                     if (!ConfBackWrst_Cbx.Checked && MessageBox.Show(this, "آیا با تحویل و برگشت دستبند موافق هستید؟", "عملیات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                     // اگر قبلا این دستبند برگشت داده شده باشد
                     if (AtnwBs.List.OfType<Data.Attendance_Wrist>().Any(aw => aw.ATTN_CODE == _attn.CODE && aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text && aw.STAT == "002")) return;

                     // وضعیت برگشت دستبند را فعال میکنیم
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Attendance_Wrist SET STAT = '002' WHERE Attn_Code = {0} AND Atnw_Fngr_Prnt_Dnrm = '{1}';", _attn.CODE, WristBand_Txt.Text));

                     _tmp = AtnwBs.List.OfType<Data.Attendance_Wrist>().FirstOrDefault(aw => aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text).Dresser == null ? AtnwBs.List.OfType<Data.Attendance_Wrist>().FirstOrDefault(aw => aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text).ATNW_FNGR_PRNT_DNRM : AtnwBs.List.OfType<Data.Attendance_Wrist>().FirstOrDefault(aw => aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text).Dresser.DRES_NUMB.ToString();

                     iScsc.INS_LGOP_P(
                        new XElement("Log",
                              new XAttribute("fileno", _attn.FIGH_FILE_NO),
                              new XAttribute("type", "015"),
                              new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _attn.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " را تحویل گرفت")
                        )
                     );
                  }
                  break;
               default:
                  break;
            }
            requery = true;
            WristBand_Txt.Text = "";
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      private void Atnw_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _atnw = AtnwBs.Current as Data.Attendance_Wrist;
            if (_atnw == null) return;

            string _tmp = "";
            attncode = _atnw.ATTN_CODE;

            switch (e.Button.Index)
            {
               case 0:
                  if (_atnw.Dresser == null) return;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("OprtDres",
                                 new XAttribute("type", "sendoprtdres"),
                                 new XAttribute("cmndname", _atnw.Dresser.DRES_NUMB),
                                 new XAttribute("devip", _atnw.Dresser.IP_ADRS),
                                 new XAttribute("cmndsend", _atnw.Dresser.CMND_SEND ?? "")
                               )
                     }
                  );

                  _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                  iScsc.INS_LGOP_P(
                     new XElement("Log",
                           new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                           new XAttribute("type", "016"),
                           new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " باز کرد")
                     )
                  );
                  break;
               case 1:
                  if (!ConfBackWrst_Cbx.Checked && MessageBox.Show(this, "آیا با تحویل و برگشت دستبند موافق هستید؟", "عملیات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                  // اگر قبلا این دستبند برگشت داده شده باشد
                  if (AtnwBs.List.OfType<Data.Attendance_Wrist>().Any(aw => /*aw.ATTN_CODE == _atnw.ATTN_CODE && aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text*/aw.CODE == _atnw.CODE && aw.STAT == "002")) return;

                  // وضعیت برگشت دستبند را فعال میکنیم
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Attendance_Wrist SET STAT = '002' WHERE CODE = {0};", /*_atnw.ATTN_CODE, WristBand_Txt.Text*/ _atnw.CODE));

                  _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                  iScsc.INS_LGOP_P(
                     new XElement("Log",
                           new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                           new XAttribute("type", "015"),
                           new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " را تحویل گرفت")
                     )
                  );
                  break;
               case 2:
                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف دستبند", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(string.Format("DELETE dbo.Attendance_Wrist WHERE Code = {0};", _atnw.CODE));

                  _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                  iScsc.INS_LGOP_P(
                     new XElement("Log",
                           new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                           new XAttribute("type", "014"),
                           new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " را حذف کرد")
                     )
                  );

                  if(AtnwBs.List.Count > 1)
                  {
                     iScsc.ExecuteCommand(
                        string.Format(
                           "UPDATE dbo.Attendance SET SUM_ATTN_MONT_DNRM -= 1 WHERE CODE = {0};" + Environment.NewLine +
                           "UPDATE ms SET ms.SUM_ATTN_MONT_DNRM -= 1 FROM dbo.Member_Ship ms, dbo.Attendance a WHERE a.Code = {0} AND a.MBSP_RWNO_DNRM = ms.RWNO AND a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND ms.RECT_CODE = '004';",
                        _atnw.ATTN_CODE)
                     );

                     _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                     iScsc.INS_LGOP_P(
                        new XElement("Log",
                              new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                              new XAttribute("type", "012"),
                              new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " برگشت یک جلسه به دوره مشتری به صورت دستی بابت حذف دستبند شماره " + _tmp + " انجام شد")
                        )
                     );
                  }
                  break;
               case 3:
                  formcaller = "";
                  RqstBnExit1_Click(null, null);
                  var attn = AttnBs1.Current as Data.Attendance;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _atnw.ATNW_FIGH_FILE_NO)) }
                  );
                  break;
               default:
                  break;
            }
            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }

      private void OthrIncome_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            formcaller = "";
            //RqstBnExit1_Click(null, null);
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */) { Input = Keys.Escape },
                     new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", fileno)))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
