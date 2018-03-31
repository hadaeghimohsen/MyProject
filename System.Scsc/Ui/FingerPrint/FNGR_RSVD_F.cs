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
using DevExpress.XtraGrid.Views.Grid;
using System.Xml.Linq;

namespace System.Scsc.Ui.FingerPrint
{
   public partial class FNGR_RSVD_F : UserControl
   {
      public FNGR_RSVD_F()
      {
         InitializeComponent();
      }

      private void Execute_Query(bool runAll)
      {
         SRCH_BUTN01_Click(null, null);
      }

      private void SRCH_BUTN01_Click(object sender, EventArgs e)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            if (Txt_Mnth01.Text.Trim() == "") return;

            FighBs1.DataSource =
               iScsc.Fighters.Where(f => 
                  Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) && 
                  (f.FGPB_TYPE_DNRM != "002" && f.FGPB_TYPE_DNRM != "003") &&
                  (
                     f.Member_Ships.FirstOrDefault(m => 
                        m.RWNO == f.MBSP_RWNO_DNRM && 
                        m.RECT_CODE == "004").END_DATE <= DateTime.Now.AddMonths(-1 * Convert.ToInt32(Txt_Mnth01.Text)) ||
                     Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") <= 100 // مشتری غیرفعال باشد
                  )
               );
         }
      }

      private void FighBs1_CurrentChanged(object sender, EventArgs e)
      {
         var crntFigh = FighBs1.Current as Data.Fighter;
         if (crntFigh == null) return;

         var mbsp = crntFigh.Member_Ships.FirstOrDefault(m => m.RWNO == crntFigh.MBSP_RWNO_DNRM && m.RECT_CODE == "004");
         Txt_MnthRemnd01.Text = (DateTime.Now.Subtract((DateTime)mbsp.END_DATE).Days / 30).ToString();
         Dt_EndDate01.Value = mbsp.END_DATE;
      }

      private void EXIT_BUTN01_Click(object sender, EventArgs e)
      {
         if(tb_master.SelectedTab == tp_001)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
            );
         }
      }

      private void DACT_BUTN001_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با غیرفعال کردن مشترییان موافقید", "غیرفعال کردن مشترییان", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            int[] selRows = ((GridView)fighterGridControl.MainView).GetSelectedRows();
            foreach (int i in selRows)
            {
               var figh = ((GridView)fighterGridControl.MainView).GetRow(i) as Data.Fighter;
               if (Convert.ToInt32(figh.ACTV_TAG_DNRM) <= 100) continue;

               iScsc.ADT_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", 0),
                           new XAttribute("rqtpcode", "013"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO),
                              new XElement("Fighter_Public",
                                 new XElement("Actv_Tag", "002")
                              )
                           )
                        )
                     )
                  );

               //iScsc = new Data.iScscDataContext(ConnectionString);
               var Rqids = iScsc.VF_Requests(new XElement("Request_Row", figh.FILE_NO))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "013" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.SUB_SYS == 1).FirstOrDefault();

               iScsc.ADT_SAVE_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqids.RQID),
                           new XAttribute("prvncode", Rqids.REGN_PRVN_CODE),
                           new XAttribute("regncode", Rqids.REGN_CODE),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );
            }
            if (selRows.Count() > 0)
            {
               SRCH_BUTN01_Click(null, null);
            }
         }catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void RMVF_BUTN001_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف کردن کد اثر انگشتی مشترییان موافقید", "حذف کردن کد اثر انگشتی مشترییان", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            int[] selRows = ((GridView)fighterGridControl.MainView).GetSelectedRows();
            foreach (int i in selRows)
            {
               var figh = ((GridView)fighterGridControl.MainView).GetRow(i) as Data.Fighter;
               var lastfgpb = figh.Fighter_Publics.FirstOrDefault(fb => fb.RECT_CODE == "004" && fb.RWNO == figh.FGPB_RWNO_DNRM);

               if (Convert.ToInt32(figh.ACTV_TAG_DNRM) > 100) continue;
               if (figh.FNGR_PRNT_DNRM == null || figh.FNGR_PRNT_DNRM == "") continue;

               iScsc.PBL_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", 0),
                           new XAttribute("rqtpcode", "002"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );

               var Rqids = iScsc.VF_Requests(new XElement("Request_Row", figh.FILE_NO))
                           .Where(rqst =>
                                 rqst.RQTP_CODE == "002" &&
                                 rqst.RQTT_CODE == "004" &&
                                 rqst.RQST_STAT == "001" &&
                                 rqst.SUB_SYS == 1).FirstOrDefault();

               iScsc.PBL_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqids.RQID),
                           new XAttribute("rqtpcode", "002"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO),
                              new XElement("Fighter_Public",
                                 new XElement("Type", figh.FGPB_TYPE_DNRM),
                                 new XElement("Frst_Name", lastfgpb.FRST_NAME),
                                 new XElement("Last_Name", lastfgpb.LAST_NAME),
                                 new XElement("Fath_Name", lastfgpb.FATH_NAME ?? ""),
                                 new XElement("Sex_Type", lastfgpb.SEX_TYPE),
                                 new XElement("Natl_Code", lastfgpb.NATL_CODE ?? ""),
                                 new XElement("Brth_Date", lastfgpb.BRTH_DATE),
                                 new XElement("Cell_Phon", lastfgpb.CELL_PHON ?? ""),
                                 new XElement("Tell_Phon", lastfgpb.TELL_PHON ?? ""),
                                 new XElement("Post_Adrs", lastfgpb.POST_ADRS ?? ""),
                                 new XElement("Emal_Adrs", lastfgpb.EMAL_ADRS ?? ""),
                                 new XElement("Insr_Numb", lastfgpb.INSR_NUMB ?? ""),
                                 new XElement("Insr_Date", lastfgpb.INSR_DATE ?? DateTime.Now.Date),
                                 new XElement("Educ_Deg", lastfgpb.EDUC_DEG ?? ""),
                                 new XElement("Cbmt_Code", lastfgpb.CBMT_CODE),
                                 new XElement("Dise_Code", lastfgpb.DISE_CODE ?? 13930723152335833),
                                 new XElement("Calc_Expn_Type", lastfgpb.CALC_EXPN_TYPE ?? ""),
                                 new XElement("Coch_Deg", lastfgpb.COCH_DEG ?? ""),
                                 new XElement("Coch_Crtf_Date", lastfgpb.COCH_CRTF_DATE ?? DateTime.Now.Date),
                                 new XElement("Gudg_Deg", lastfgpb.GUDG_DEG ?? ""),
                                 new XElement("Glob_Code", lastfgpb.GLOB_CODE ?? ""),
                                 new XElement("Blod_Grop", lastfgpb.BLOD_GROP ?? ""),
                                 new XElement("Fngr_Prnt", "")
                              )
                           )
                        )
                     )
                  );

               iScsc.PBL_SAVE_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqids.RQID),
                           new XAttribute("prvncode", Rqids.REGN_PRVN_CODE),
                           new XAttribute("regncode", Rqids.REGN_CODE),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );
            }
            if (selRows.Count() > 0)
            {
               SRCH_BUTN01_Click(null, null);
            }
         }catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void ALLC_BUTN001_Click(object sender, EventArgs e)
      {
         try
         {
            DACT_BUTN001_Click(null, null);
            RMVF_BUTN001_Click(null, null);
         }
         catch
         {
            MessageBox.Show("در انجام عملیات خطایی رخ داده لطفا بررسی و اصلاح کنید");
         }
      }

      private void ACTV_BUTN001_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با فعال کردن مشترییان موافقید", "فعال کردن مشترییان", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            int[] selRows = ((GridView)fighterGridControl.MainView).GetSelectedRows();
            foreach (int i in selRows)
            {
               var figh = ((GridView)fighterGridControl.MainView).GetRow(i) as Data.Fighter;
               if (Convert.ToInt32(figh.ACTV_TAG_DNRM) > 100) continue;

               iScsc.ADT_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", 0),
                           new XAttribute("rqtpcode", "014"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO),
                              new XElement("Fighter_Public",
                                 new XElement("Actv_Tag", "102")
                              )
                           )
                        )
                     )
                  );

               //iScsc = new Data.iScscDataContext(ConnectionString);
               var Rqids = iScsc.VF_Requests(new XElement("Request_Row", figh.FILE_NO))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "014" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.SUB_SYS == 1).FirstOrDefault();

               iScsc.ADT_SAVE_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqids.RQID),
                           new XAttribute("prvncode", Rqids.REGN_PRVN_CODE),
                           new XAttribute("regncode", Rqids.REGN_CODE),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );
            }
            if (selRows.Count() > 0)
            {
               SRCH_BUTN01_Click(null, null);
            }
         }catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void RCVF_BUTN001_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با بازگردانی کد اثر انگشتی مشترییان موافقید", "بازگردانی کد اثر انگشتی مشترییان", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            int[] selRows = ((GridView)fighterGridControl.MainView).GetSelectedRows();
            foreach (int i in selRows)
            {
               var figh = ((GridView)fighterGridControl.MainView).GetRow(i) as Data.Fighter;
               var lastfgpbwithfngrprnt = figh.Fighter_Publics.FirstOrDefault(fb => fb.RECT_CODE == "004" && fb.RWNO == figh.Fighter_Publics.Where(fbt => fbt.RECT_CODE == "004" && fbt.FNGR_PRNT != "").Max(fbt => fbt.RWNO));

               if (Convert.ToInt32(figh.ACTV_TAG_DNRM) <= 100) continue;
               if (figh.FNGR_PRNT_DNRM != "") continue;
               if (lastfgpbwithfngrprnt == null) continue;
               if (iScsc.Fighters.Any(f => f.FNGR_PRNT_DNRM == lastfgpbwithfngrprnt.FNGR_PRNT)) continue;

               iScsc.PBL_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", 0),
                           new XAttribute("rqtpcode", "002"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );

               var Rqids = iScsc.VF_Requests(new XElement("Request_Row", figh.FILE_NO))
                           .Where(rqst =>
                                 rqst.RQTP_CODE == "002" &&
                                 rqst.RQTT_CODE == "004" &&
                                 rqst.RQST_STAT == "001" &&
                                 rqst.SUB_SYS == 1).FirstOrDefault();

               iScsc.PBL_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqids.RQID),
                           new XAttribute("rqtpcode", "002"),
                           new XAttribute("rqttcode", "004"),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO),
                              new XElement("Fighter_Public",
                                 new XElement("Type", figh.FGPB_TYPE_DNRM),
                                 new XElement("Frst_Name", lastfgpbwithfngrprnt.FRST_NAME),
                                 new XElement("Last_Name", lastfgpbwithfngrprnt.LAST_NAME),
                                 new XElement("Fath_Name", lastfgpbwithfngrprnt.FATH_NAME ?? ""),
                                 new XElement("Sex_Type", lastfgpbwithfngrprnt.SEX_TYPE),
                                 new XElement("Natl_Code", lastfgpbwithfngrprnt.NATL_CODE ?? ""),
                                 new XElement("Brth_Date", lastfgpbwithfngrprnt.BRTH_DATE),
                                 new XElement("Cell_Phon", lastfgpbwithfngrprnt.CELL_PHON ?? ""),
                                 new XElement("Tell_Phon", lastfgpbwithfngrprnt.TELL_PHON ?? ""),
                                 new XElement("Post_Adrs", lastfgpbwithfngrprnt.POST_ADRS ?? ""),
                                 new XElement("Emal_Adrs", lastfgpbwithfngrprnt.EMAL_ADRS ?? ""),
                                 new XElement("Insr_Numb", lastfgpbwithfngrprnt.INSR_NUMB ?? ""),
                                 new XElement("Insr_Date", lastfgpbwithfngrprnt.INSR_DATE ?? DateTime.Now.Date),
                                 new XElement("Educ_Deg", lastfgpbwithfngrprnt.EDUC_DEG ?? ""),
                                 new XElement("Cbmt_Code", lastfgpbwithfngrprnt.CBMT_CODE),
                                 new XElement("Dise_Code", lastfgpbwithfngrprnt.DISE_CODE ?? 13930723152335833),
                                 new XElement("Calc_Expn_Type", lastfgpbwithfngrprnt.CALC_EXPN_TYPE ?? ""),
                                 new XElement("Coch_Deg", lastfgpbwithfngrprnt.COCH_DEG ?? ""),
                                 new XElement("Coch_Crtf_Date", lastfgpbwithfngrprnt.COCH_CRTF_DATE ?? DateTime.Now.Date),
                                 new XElement("Gudg_Deg", lastfgpbwithfngrprnt.GUDG_DEG ?? ""),
                                 new XElement("Glob_Code", lastfgpbwithfngrprnt.GLOB_CODE ?? ""),
                                 new XElement("Blod_Grop", lastfgpbwithfngrprnt.BLOD_GROP ?? ""),
                                 new XElement("Fngr_Prnt", lastfgpbwithfngrprnt.FNGR_PRNT)
                              )
                           )
                        )
                     )
                  );

               iScsc.PBL_SAVE_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqids.RQID),
                           new XAttribute("prvncode", Rqids.REGN_PRVN_CODE),
                           new XAttribute("regncode", Rqids.REGN_CODE),
                           new XElement("Request_Row",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                        )
                     )
                  );
            }
            if (selRows.Count() > 0)
            {
               SRCH_BUTN01_Click(null, null);
            }
         }catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void ALRC_BUTN001_Click(object sender, EventArgs e)
      {
         try
         {
            ACTV_BUTN001_Click(null, null);
            RCVF_BUTN001_Click(null, null);
         }
         catch 
         {
            MessageBox.Show("در انجام عملیات خطایی رخ داده لطفا بررسی و اصلاح کنید");
         }
      }
   }
}
