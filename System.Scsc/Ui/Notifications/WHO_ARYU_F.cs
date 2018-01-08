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

namespace System.Scsc.Ui.Notifications
{
   public partial class WHO_ARYU_F : UserControl
   {
      public WHO_ARYU_F()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, Pb_FighImg.Width , Pb_FighImg.Height );
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         Pb_FighImg.Region = rg;
      }

      private bool requery = false;

      private void Pb_FighImg_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawEllipse(
             new Pen(Color.White, 2f),
             0, 0, Pb_FighImg.Size.Width, Pb_FighImg.Size.Height);
      }

      private void Execute_Query(bool runAllQuery)
      {
         try
         {
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
                        && a.MBSP_RWNO_DNRM == mbsprwno
                        && a.EXIT_TIME == null
                      )
                     .OrderByDescending(a => a.CRET_DATE).FirstOrDefault();

                  if (result == null)
                     AttnBs1.DataSource =
                        iScsc.Attendances
                        .Where(a => a.FIGH_FILE_NO == Convert.ToInt64(fileno)
                           && a.ATTN_DATE.Date == AttnDate_Date.Value.Value.Date
                           && a.MBSP_RWNO_DNRM == mbsprwno
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
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void AttnBs1_CurrentChanged(object sender, EventArgs e)
      {
         var attn = AttnBs1.Current as Data.Attendance;
         if (attn == null) return;

         if (attn.EXIT_TIME != null)
         {
            Lbl_AccessControl.Text = "خروج از باشگاه";
            Lbl_AccessControl.ForeColor = Color.White;
            DRES_NUMB_Txt.Properties.ReadOnly = true;
            DresNumb_Butn.Enabled = false;
         }
         else
         {
            Lbl_AccessControl.Text = "ورود مجاز است";
            Lbl_AccessControl.ForeColor = Color.GreenYellow;
            DRES_NUMB_Txt.Properties.ReadOnly = false;
            DresNumb_Butn.Enabled = true;

            // 1396/10/18 * آیا گزینه نمایش چاپ حضوری انجام شود یا خیر
            if (iScsc.Settings.FirstOrDefault(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).ATTN_PRNT_STAT == "002")
               PrintDefault_Butn_Click(null, null);
         }

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
                              new XAttribute("gateactn", attn.EXIT_TIME == null ? "open" : "close")
                           )
                     }
                  }
               )
            );

         // 1395/12/11 * چک کردن وضعیت بدهی هنرجو
         //if(attn.Fighter1.DEBT_DNRM > 0)
         if (attn.DEBT_DNRM > 0)
         {
            Lbl_DebtStatDesc.Text = "بدهکار";
            Lbl_DebtStatDesc.ForeColor = Color.Salmon;
         }
         //else if (attn.Fighter1.DEBT_DNRM == 0)
         else if (attn.DEBT_DNRM == 0)
         {
            Lbl_DebtStatDesc.Text = "تسویه";
            Lbl_DebtStatDesc.ForeColor = Color.Black;
         }
         else
         {
            Lbl_DebtStatDesc.Text = "بستانکار";
            Lbl_DebtStatDesc.ForeColor = Color.GreenYellow;
         }

         if (attn.IMAG_RCDC_RCID_DNRM != null)
         {
            try
            {
               Pb_FighImg.Image = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", attn.FIGH_FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               if (InvokeRequired)
                  Invoke(new Action(() => Pb_FighImg.Image = bm));
               else
                  Pb_FighImg.Image = bm;
            }
            catch { }
         }
         else
         {
            Pb_FighImg.Image = null;
         }

         if (iScsc.GET_MTOS_U((DateTime?)attn.ATTN_DATE.Date).Substring(5) == Brth_Date_PersianDateEdit2.GetText("MM/dd"))
            HappyBirthDate_Lab.Visible = true;
         else
            HappyBirthDate_Lab.Visible = false;

         if(attn.EXIT_TIME == null)
            AttnType_Lab.ImageKey = "IMAGE_1106.png";
         else
            AttnType_Lab.ImageKey = "IMAGE_1058.png";

         if (Pb_FighImg.Image == null && attn.Fighter1.SEX_TYPE_DNRM == "002")
            Pb_FighImg.Image = System.Scsc.Properties.Resources.IMAGE_1148;
         else if(Pb_FighImg.Image == null)
            Pb_FighImg.Image = System.Scsc.Properties.Resources.IMAGE_1149;

         //if (attn.Fighter1.FGPB_TYPE_DNRM == "002" || attn.Fighter1.FGPB_TYPE_DNRM == "003")
         if (attn.FGPB_TYPE_DNRM == "002" || attn.FGPB_TYPE_DNRM == "003")
         {
            //***FighterType_Lab.ImageKey = "IMAGE_1126.png";
            //Figh00156.Visible = false;
            //Sesn_Pn.Visible = false;
            PrivSesn_Pn.Visible = false;
            //PrivateSession_Lab.Visible = false;
            //PrivateSession_Lab.Image = null;
            if (attn.FGPB_TYPE_DNRM == "002")
            {
               panel1.Visible = false;
               return;
            }
         }
         //***else if (attn.Fighter1.FGPB_TYPE_DNRM == "008")
         //***   FighterType_Lab.ImageKey = "IMAGE_1087.png";
         //***else
         //***   FighterType_Lab.ImageKey = "IMAGE_1115.png";
         panel1.Visible = true;
         //Figh00156.Visible = true;
         //Sesn_Pn.Visible = false;
         
         //if(attn.Fighter1.MBCO_RWNO_DNRM != null && (attn.Fighter1.FGPB_TYPE_DNRM != "002"))
         if (attn.MBCO_RWNO_DNRM != null && (attn.FGPB_TYPE_DNRM != "003"))
         {
            var mbco = iScsc.Member_Ships.First(m => m.FIGH_FILE_NO == attn.FIGH_FILE_NO && m.RWNO == attn.Fighter1.MBCO_RWNO_DNRM && m.RECT_CODE == "004");
            if (mbco.END_DATE.Value.Date >= DateTime.Now.Date)
            {
               //PrivateSession_Lab.Visible = true;
               //PrivateSession_Lab.Image = System.Scsc.Properties.Resources.IMAGE_1007;
               PrivSesn_Pn.Visible = true;
               StrtPrivSesn_Date.Value = mbco.STRT_DATE;
               EndPrivSesn_Date.Value = mbco.END_DATE;
            }
            else
            {
               PrivSesn_Pn.Visible = false;
               //PrivateSession_Lab.Visible = false;
               //PrivateSession_Lab.Image = null;
            }
         }
         else
         {
            PrivSesn_Pn.Visible = false;
            //PrivateSession_Lab.Visible = false;
            //PrivateSession_Lab.Image = null;
         }

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

         dateTimeSelector3.BackColor = Color.DimGray;
         if (attn.MBSP_END_DATE_DNRM.Value.AddDays(1).Date == DateTime.Now.Date)
            dateTimeSelector3.BackColor = Color.Yellow;
         else if (attn.MBSP_END_DATE_DNRM.Value.Date == DateTime.Now.Date)
            dateTimeSelector3.BackColor = Color.Red;
         ///***
         //if(attn.Member_Ship.Sessions.Any())
         //   SesnBs1.DataSource =
         //      iScsc.Sessions
         //      .Where(s => s.Member_Ship == attn.Member_Ship);

         if(attn.EXIT_TIME != null)
         {
            var diff = attn.EXIT_TIME.Value.Subtract(attn.ENTR_TIME.Value);
            TotlAttnTime_Lbl.Text = string.Format("{0}:{1}", diff.Hours, diff.Minutes); ;
         }
         else
         {
            var diff = DateTime.Now.Subtract(attn.ENTR_TIME.Value);
            TotlAttnTime_Lbl.Text = string.Format("{0}:{1}",  diff.Hour, diff.Minute);
         }

         DRES_NUMB_Txt.Focus();

      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void MoreInfo_Butn_Click(object sender, EventArgs e)
      {
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

            Lbl_Dresser.BackColor = Color.YellowGreen;
            requery = true;
         }
         catch (Exception exc)
         {
            Lbl_Dresser.BackColor = Color.Tomato;
            MsgBox.Show(exc.Message, "خطای ثبت کمد شماره کمد", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
         }
         finally
         {
            if(requery)
            {
               RqstBnExit1_Click(null, null);
               Execute_Query(true);
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

            var chosbutn = MessageBox.Show(this, "حضوری همراه به صورت هزینه دار با کسر جلسه برای عضو ثبت شود؟", "ثبت حضوری همراه", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading);
            switch (chosbutn)
	         {
		         case DialogResult.Cancel:
                  return;
               case DialogResult.No:
                  iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, attn.ATTN_DATE, attn.COCH_FILE_NO, "008", attn.MBSP_RWNO_DNRM);
                  break;
               case DialogResult.Yes:
                  iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, attn.ATTN_DATE, attn.COCH_FILE_NO, "007", attn.MBSP_RWNO_DNRM);
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
            MsgBox.Show(exc.Message, "برای ثبت اطلاعات توضیحات با خطا مواجه شد", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
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

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("a.Code = {0}", attn.CODE))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            if (attn == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("a.Code = {0}", attn.CODE))}
                    });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintSetting_Butn_Click(object sender, EventArgs e)
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
}
