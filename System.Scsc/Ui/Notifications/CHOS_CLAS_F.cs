﻿using System;
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

namespace System.Scsc.Ui.Notifications
{
   public partial class CHOS_CLAS_F : UserControl
   {
      public CHOS_CLAS_F()
      {
         InitializeComponent();
      }

      bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query(bool runAllQuery)
      {
         SnmtBs1.List.Clear();
         iScsc = new Data.iScscDataContext(ConnectionString);
         FighBs1.DataSource = iScsc.Fighters.First(f => f.FILE_NO == Convert.ToInt64(FIGH_FILE_NOTextEdit.EditValue));
      }

      private void FighBs1_CurrentChanged(object sender, EventArgs e)
      {
         var figh = FighBs1.Current as Data.Fighter;

         if (figh == null)
            return;

         MbspBs1.DataSource = 
            iScsc.Member_Ships
               .FirstOrDefault( m => 
                  m.FIGH_FILE_NO == figh.FILE_NO && 
                  m.RWNO == figh.MBSP_RWNO_DNRM && 
                  m.RECT_CODE == "004"
               );
      }

      private void MbspBs1_CurrentChanged(object sender, EventArgs e)
      {
         var mbsp = MbspBs1.Current as Data.Member_Ship;

         if (mbsp == null)
            return;

         RemnDay_TextEdit.EditValue = (mbsp.END_DATE.Value - mbsp.STRT_DATE.Value).Days;

         SesnBs1.DataSource = 
            iScsc.Sessions
            .Where( s => 
               s.MBSP_FIGH_FILE_NO == mbsp.FIGH_FILE_NO &&
               s.MBSP_RWNO == mbsp.RWNO &&
               s.MBSP_RECT_CODE == mbsp.RECT_CODE &&
               (
                  s.TOTL_SESN - (s.SUM_MEET_HELD_DNRM ?? 0) > 0 ||
                  s.Session_Meetings.Any(sm => sm.END_TIME == null)
               )

            );
      }

      private void SesnBs1_CurrentChanged(object sender, EventArgs e)
      {
         var sesn = SesnBs1.Current as Data.Session;

         if (sesn == null)
            return;

         CBMT_CODELookUpEdit.EditValue = sesn.CBMT_CODE;
         SnmtBs1.DataSource = iScsc.Session_Meetings.Where(sm => sm.Session == sesn);
      }

      private void EntrAttn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sesn = SesnBs1.Current as Data.Session;
            var cbmt = Convert.ToInt64(CBMT_CODELookUpEdit.EditValue);
            // اگر حضوری قبلا ثبت شده است باید دکمه خروج را فشار دهد
            if(iScsc.Session_Meetings
               .Any(sm => 
                  sm.Session == sesn && 
                  sm.ACTN_DATE.Value.Date == DateTime.Now.Date &&
                  sm.END_TIME == null   
               ))
            {
               if (MessageBox.Show(this, "برای این فرد قبلا ورود برای امروز ثبت شده! آیا می خواهید خروج برایش ثبت کنم؟", "خطای ورود", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               {
                  ExitAttn_Butn_Click(null, null);
               }
               else
                  return;
            }
            else
            {
               var snmt = new Data.Session_Meeting() 
               { 
                  SESN_SNID = sesn.SNID,
                  RWNO = 0,
                  MBSP_FIGH_FILE_NO = sesn.MBSP_FIGH_FILE_NO, 
                  MBSP_RECT_CODE = sesn.MBSP_RECT_CODE, 
                  MBSP_RWNO = sesn.MBSP_RWNO, 
                  EXPN_CODE = (long)sesn.EXPN_CODE,
                  VALD_TYPE = "002", 
                  CBMT_CODE = cbmt ,
                  NUMB_OF_GAYS = 1
               };
               iScsc.Session_Meetings.InsertOnSubmit(snmt);

               iScsc.SubmitChanges();
               requery = true;
            }
         }
         catch { requery = false; }
         finally
         {
            if(requery)
            {
               Attn_Oprt_P();
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void ExitAttn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var snmt = 
               iScsc.Session_Meetings
               .FirstOrDefault(sm => 
                  sm.Session.Member_Ship.Fighter.FILE_NO == Convert.ToInt64(FIGH_FILE_NOTextEdit.EditValue) &&
                  sm.ACTN_DATE.Value.Date == DateTime.Now.Date &&
                  sm.END_TIME == null 
               );
            // اگر حضوری قبلا ثبت شده است باید دکمه خروج را فشار دهد
            if (snmt == null)
            {
               if (MessageBox.Show(this, "برای این فرد قبلا ورود برای امروز ثبت نشده! آیا می خواهید ورود برایش ثبت کنم؟", "خطای خروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               {
                  EntrAttn_Butn_Click(null, null);
               }
               else
                  return;
            }
            else
            {
               snmt.END_TIME = DateTime.Now.TimeOfDay;
               iScsc.SubmitChanges();
               requery = true;
            }
         }
         catch { requery = false; }
         finally
         {
            if (requery)
            {
               Attn_Oprt_P();
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void Attn_Oprt_P()
      {
         try
         {
            iScsc.INS_ATTN_P(null, Convert.ToInt64(FIGH_FILE_NOTextEdit.EditValue), null, null, "001");
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            if (fNGR_PRNT_DNRMTextEdit.EditValue != null)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                        new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("enrollnumber", fNGR_PRNT_DNRMTextEdit.EditValue),
                              new XAttribute("isnewenroll", false)
                           )
                        }
                     })
               );
            requery = false;
         }
         finally
         {
            if(requery)
            {
               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting.ATTN_NOTF_STAT == "002")
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", FIGH_FILE_NOTextEdit.EditValue),
                                 new XAttribute("attndate", AttnDate_Date.Value),
                                 new XAttribute("gatecontrol", "true")
                              )
                           }
                        }
                     )
                  );
               }
            }
         }
      }
         
   }
}
