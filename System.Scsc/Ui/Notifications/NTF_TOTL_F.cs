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
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using System.MaxUi;

namespace System.Scsc.Ui.Notifications
{
   public partial class NTF_TOTL_F : UserControl
   {
      public NTF_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requry = false;
      private short mbsprwno;
      private string compname = "", chckattnalrm = "";
      private string attnsystype = "002";
      private string attnignrtype = "001";

      private void Execute_Query()
      {
         if (tb_master.SelectedTab == tp_002)
         {
            Pb_FileNo2.PickChecked = true;
            if (formStatus == "RunInForeground")
            {
               FighBs2.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
               CochBs2.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
            }
         }
      }

      private void Btn_Search_Click(object sender, EventArgs e)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         //#region Tab1
         //if (tb_master.SelectedTab == tp_001)
         //{
         //   if(Pb_ExpDate1.PickChecked)
         //      FighBs1.DataSource = 
         //         iScsc.Fighters
         //         .Where(f => 
         //            f.CONF_STAT == "002" && 
         //            (f.FGPB_TYPE_DNRM != "002" && 
         //             f.FGPB_TYPE_DNRM != "003" &&
         //             f.FGPB_TYPE_DNRM != "007" &&
         //             !f.NAME_DNRM.Contains("مشتری, جلسه ای")) && 
         //            Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && 
         //            Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) && 
         //            (
         //               (Dt_ExpDate1.Value ?? DateTime.Now ).Date > f.Member_Ships.Where(m => m.RWNO == f.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").Select(m => m.END_DATE.Value).SingleOrDefault().Date
         //            )
         //         );
         //   else if(Pb_ExpDay1.PickChecked)
         //      FighBs1.DataSource =
         //      iScsc.Fighters
         //      .Where(f =>
         //         f.CONF_STAT == "002" &&
         //         (f.FGPB_TYPE_DNRM != "002" &&
         //          f.FGPB_TYPE_DNRM != "003" &&
         //          f.FGPB_TYPE_DNRM != "007" &&
         //          !f.NAME_DNRM.Contains("مشتری, جلسه ای")) &&
         //         Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
         //         Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
         //         (
         //            DateTime.Now.AddDays((double)Nud_ExpDay1.Value).Date > f.Member_Ships.Where(m => m.RWNO == f.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").Select(m => m.END_DATE.Value).SingleOrDefault().Date
         //         )
         //      );
         //   else if(Pb_ExpSession1.PickChecked)
         //      FighBs1.DataSource =
         //         iScsc.Fighters
         //         .Where(f =>
         //            f.CONF_STAT == "002" &&
         //            (f.FGPB_TYPE_DNRM != "002" &&
         //             f.FGPB_TYPE_DNRM != "003" &&
         //             f.FGPB_TYPE_DNRM != "007" &&
         //             !f.NAME_DNRM.Contains("مشتری, جلسه ای")) &&
         //            Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
         //            Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
         //            (
         //               Nud_ExpSession1.Value >= f.Member_Ships.Where(m => m.RWNO == f.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").Select(m => (m.NUMB_OF_ATTN_MONT - m.SUM_ATTN_MONT_DNRM)).FirstOrDefault()
         //            )
         //         );
         //}
         //#endregion
         #region Tab2
         if (tb_master.SelectedTab == tp_002)
         {
            if (Nud_FileNo2.Text == "") Nud_FileNo2.Text = "0";
            if (tc_sub.SelectedTab == tp_002001 || tc_sub.SelectedTab == tp_002003)
            {               
               if (Pb_FileNo2.PickChecked)
                  AttnBs2.DataSource =
                     iScsc.Attendances
                     .Where(a =>
                        Fga_Uclb_U.Contains(a.CLUB_CODE) &&
                        a.FIGH_FILE_NO == (Convert.ToInt64(Nud_FileNo2.EditValue) == 0 ? a.FIGH_FILE_NO : Convert.ToInt64(Nud_FileNo2.EditValue)) &&
                        a.ATTN_DATE.Date == (Pb_AttnDateNow2.PickChecked ? DateTime.Now.Date : Pb_AttnDateAll2.PickChecked ? a.ATTN_DATE.Date : Dt_CrntDate2R.Value.Value.Date)
                     );
               else if (Pb_FileName2.PickChecked)
                  if (Lov_FileName2.Text != "" /**/)
                     AttnBs2.DataSource =
                        iScsc.Attendances
                        .Where(a =>
                           Fga_Uclb_U.Contains(a.CLUB_CODE) &&
                           a.FIGH_FILE_NO == (/*(long)*/Lov_FileName2.Text == "" /**/ ? a.FIGH_FILE_NO : (long)Lov_FileName2.EditValue) &&
                           a.ATTN_DATE.Date == (Pb_AttnDateNow2.PickChecked ? DateTime.Now.Date : Pb_AttnDateAll2.PickChecked ? a.ATTN_DATE.Date : Dt_CrntDate2R.Value.Value.Date)
                        );
                  else
                     AttnBs2.DataSource =
                        iScsc.Attendances
                        .Where(a =>
                           Fga_Uclb_U.Contains(a.CLUB_CODE) &&
                           a.ATTN_DATE.Date == (Pb_AttnDateNow2.PickChecked ? DateTime.Now.Date : Pb_AttnDateAll2.PickChecked ? a.ATTN_DATE.Date : Dt_CrntDate2R.Value.Value.Date)
                        );
               
            }
            else if (tc_sub.SelectedTab == tp_002002)
            {
               //if (Pb_FileNo2.PickChecked)
               //   Drat2Bs2.DataSource =
               //      iScsc.Dresser_Attendances
               //      .Where(da =>
               //         Fga_Uclb_U.Contains(da.Dresser.CLUB_CODE) &&
               //         !da.TKBK_TIME.HasValue &&
               //         da.Attendance.FIGH_FILE_NO == (Convert.ToInt64(Nud_FileNo2.EditValue) == 0 ? da.Attendance.FIGH_FILE_NO : Convert.ToInt64(Nud_FileNo2.EditValue)) &&
               //         (!Tpb_HasPently.PickChecked || da.Attendance.ATTN_DATE != DateTime.Now)

               //      );
               //else if (Pb_FileName2.PickChecked)
               //   if (Lov_FileName2.Text != "" /**/)
               //      Drat2Bs2.DataSource =
               //      iScsc.Dresser_Attendances
               //      .Where(da =>
               //         Fga_Uclb_U.Contains(da.Dresser.CLUB_CODE) &&
               //         !da.TKBK_TIME.HasValue &&
               //         da.Attendance.FIGH_FILE_NO == (/*(long)*/Lov_FileName2.Text == "" /**/ ? da.Attendance.FIGH_FILE_NO : (long)Lov_FileName2.EditValue) &&
               //         (!Tpb_HasPently.PickChecked || da.Attendance.ATTN_DATE != DateTime.Now)
               //      );
               //   else
               //      Drat2Bs2.DataSource =
               //      iScsc.Dresser_Attendances
               //      .Where(da =>
               //         Fga_Uclb_U.Contains(da.Dresser.CLUB_CODE) &&
               //         !da.TKBK_TIME.HasValue &&
               //         (!Tpb_HasPently.PickChecked || da.Attendance.ATTN_DATE != DateTime.Now)
               //      );
            }
            if (Nud_FileNo2.Text == "0") Nud_FileNo2.Text = "";
         }
         #endregion
         //#region Tab3
         //else if (tb_master.SelectedPage == tp_003)
         //{
         //   if (Pb_ExpDate3.PickChecked)
         //      FighBs3.DataSource =
         //         iScsc.Fighters
         //         .Where(f =>
         //            f.CONF_STAT == "002" &&
         //            f.FGPB_TYPE_DNRM != "007" &&
         //            !f.NAME_DNRM.Contains("مشتری, جلسه ای") &&
         //            Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
         //            Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
         //            (
         //               (Dt_ExpDate2.Value ?? DateTime.Now).Date >= f.Member_Ships.Where(m => m.RWNO == f.Member_Ships.Where(mm => mm.TYPE == "002" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "002").Select(m => m.END_DATE.Value).SingleOrDefault().Date
         //            )
         //         );
         //   else if (Pb_ExpDay3.PickChecked)
         //      FighBs3.DataSource =
         //      iScsc.Fighters
         //      .Where(f =>
         //         f.CONF_STAT == "002" &&
         //         f.FGPB_TYPE_DNRM != "007" &&
         //         !f.NAME_DNRM.Contains("مشتری, جلسه ای") &&
         //         Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
         //         Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
         //         (
         //            DateTime.Now.AddDays((double)Nud_ExpDay2.Value).Date >= f.Member_Ships.Where(m => m.RWNO == f.Member_Ships.Where(mm => mm.TYPE == "002" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "002").Select(m => m.END_DATE.Value).SingleOrDefault().Date
         //         )
         //      );
         //}
         //#endregion
         #region Tab4
         else if (tb_master.SelectedTab == tp_004)
         {
            if (Pb_ExpDate4.PickChecked)
               FighBs4.DataSource =
                  iScsc.Fighters
                  .Where(f =>
                     f.CONF_STAT == "002" &&
                     f.FGPB_TYPE_DNRM != "007" &&
                     !f.NAME_DNRM.Contains("مشتری, جلسه ای") &&
                     Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
                     Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
                     (
                        (Dt_ExpDate4.Value ?? DateTime.Now).Date >= f.INSR_DATE_DNRM.Value.Date
                     )
                  );
            else if (Pb_ExpDay4.PickChecked)
               FighBs4.DataSource =
               iScsc.Fighters
               .Where(f =>
                  f.CONF_STAT == "002" &&
                  f.FGPB_TYPE_DNRM != "007" &&
                  !f.NAME_DNRM.Contains("مشتری, جلسه ای") &&
                  Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
                  Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
                  (
                     DateTime.Now.AddDays((double)Nud_ExpDay4.Value).Date >= f.INSR_DATE_DNRM.Value.Date
                  )
               );
            else if(Pb_ValidInsrDate4.PickChecked)
               FighBs4.DataSource =
               iScsc.Fighters
               .Where(f =>
                  f.CONF_STAT == "002" &&
                  f.FGPB_TYPE_DNRM != "007" &&
                  !f.NAME_DNRM.Contains("مشتری, جلسه ای") &&
                  Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
                  Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
                  (
                     iScsc.GET_MTOS_U(f.INSR_DATE_DNRM.Value.Date).Substring(0, 4) == iScsc.GET_MTOS_U(DateTime.Now).Substring(0, 4) &&
                     iScsc.GET_MTOS_U(f.INSR_DATE_DNRM.Value.Date).Substring(0, 7).CompareTo(iScsc.GET_MTOS_U(DateTime.Now).Substring(0, 7)) <= 0
                  )
               );
         }
         #endregion
         //#region Tab5
         //else if(tb_master.SelectedPage == tp_005)
         //{
         //   if (Pb_ExpDate5.PickChecked)
         //      FighBs5.DataSource =
         //         iScsc.Fighters
         //         .Where(f =>
         //            f.CONF_STAT == "002" &&
         //            (f.FGPB_TYPE_DNRM == "008" &&
         //             /*f.FGPB_TYPE_DNRM != "003" &&
         //             f.FGPB_TYPE_DNRM != "007" &&*/
         //             !f.NAME_DNRM.Contains("مشتری, جلسه ای")) &&
         //            Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
         //            Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
         //            (
         //               (Dt_ExpDate5.Value ?? DateTime.Now).Date > f.Member_Ships.Where(m => m.RWNO == f.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").Select(m => m.END_DATE.Value).SingleOrDefault().Date
         //            )
         //         );
         //   else if (Pb_ExpDay5.PickChecked)
         //      FighBs5.DataSource =
         //      iScsc.Fighters
         //      .Where(f =>
         //         f.CONF_STAT == "002" &&
         //         (f.FGPB_TYPE_DNRM == "008" &&
         //          /*f.FGPB_TYPE_DNRM != "003" &&
         //          f.FGPB_TYPE_DNRM != "007" &&*/
         //          !f.NAME_DNRM.Contains("مشتری, جلسه ای")) &&
         //         Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) &&
         //         Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) &&
         //         (
         //            DateTime.Now.AddDays((double)Nud_ExpDay5.Value).Date > 
         //            f.Member_Ships
         //            .Where(
         //               m => m.RWNO == 
         //                    f.Member_Ships
         //                    .Where(
         //                       mm => mm.TYPE == "001" 
         //                          && mm.RECT_CODE == "004")
         //                    .Max(mm => mm.RWNO) && 
         //                    m.RECT_CODE == "004" && 
         //                    m.TYPE == "001")
         //                    .Select(m => m.END_DATE.Value)
         //                    .SingleOrDefault().Date
         //         )
         //      );
         //}
         //#endregion
         #region Tab6
         else if(tb_master.SelectedTab == tp_006)
         {
            PmtcBs6.DataSource = 
               iScsc.Payment_Checks
               .Where(pc => 
                  (Allcheck_Cb.Checked ? true : pc.CHEK_TYPE != "002")
               );
         }
         #endregion
         //#region Tab7
         //else if (tb_master.SelectedPage == tp_007)
         //{
         //   MbsnBs7.DataSource =
         //      iScsc.Member_Ships
         //      .Where(m =>
         //         m.RECT_CODE == "004" && 
         //         m.TYPE == "006"
         //      );
         //}
         //#endregion
      }

      private void FighBs1_CurrentChanged(object sender, EventArgs e)
      {
         if (FighBs1.Current == null) return;

         var figh = FighBs1.Current as Data.Fighter;
         //figh.Member_Ships.Where(m => m.RWNO == figh.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").ToList().ForEach(m => { Dt_ExpDate1R.Value = m.END_DATE; Nud_ExpDay1R.Value = (decimal)(m.END_DATE.Value - DateTime.Now).Days + 1; Nud_ExpSession1R.Value = (decimal)(m.NUMB_OF_ATTN_MONT - m.SUM_ATTN_MONT_DNRM); });
      }

      private void AttnBs2_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            if (AttnBs2.Current == null) return;

            var figh = AttnBs2.Current as Data.Attendance;
            //figh.Fighter.Member_Ships.Where(m => m.RWNO == figh.Fighter.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").ToList().ForEach(m => { Dt_ExpDate2R.Value = m.END_DATE; Nud_ExpDay2R.Value = (decimal)(m.END_DATE.Value - DateTime.Now).Days + 1; });
         }
         catch { }
      }

      private void FighBs3_CurrentChanged(object sender, EventArgs e)
      {
         if (FighBs3.Current == null) return;

         var figh = FighBs3.Current as Data.Fighter;
         //figh.Member_Ships.Where(m => m.RWNO == figh.Member_Ships.Where(mm => mm.TYPE == "002" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "002").ToList().ForEach(m => { Dt_ExpDate3R.Value = m.END_DATE; Nud_ExpDay3R.Value = (decimal)(m.END_DATE.Value - DateTime.Now).Days + 1; });
      }

      private void FighBs4_CurrentChanged(object sender, EventArgs e)
      {
         if (FighBs4.Current == null) return;

         var figh = FighBs4.Current as Data.Fighter;
         Dt_ExpDate4R.Value = figh.INSR_DATE_DNRM; Nud_ExpDay4R.Value = (decimal)(figh.INSR_DATE_DNRM.Value - DateTime.Now).Days + 1;
      }

      private void FighBs5_CurrentChanged(object sender, EventArgs e)
      {
         if (FighBs5.Current == null) return;

         var figh = FighBs5.Current as Data.Fighter;
         //figh.Member_Ships.Where(m => m.RWNO == figh.Member_Ships.Where(mm => mm.TYPE == "001" && mm.RECT_CODE == "004").Max(mm => mm.RWNO) && m.RECT_CODE == "004" && m.TYPE == "001").ToList().ForEach(m => { Dt_ExpDate5R.Value = m.END_DATE; Nud_ExpDay5R.Value = (decimal)(m.Sessions.FirstOrDefault().TOTL_SESN - (m.Sessions.FirstOrDefault().SUM_MEET_HELD_DNRM.HasValue ? m.Sessions.FirstOrDefault().SUM_MEET_HELD_DNRM : 0)); });
      }


      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_PrintDefault_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedPage == tp_001)
         //{
         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Member_Ship.End_Date <= {0}", Pb_ExpDate1.PickChecked ? (Dt_ExpDate1.Value ?? DateTime.Now ) : DateTime.Now.AddDays((double)Nud_ExpDay1.Value)))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedPage == tp_002)
         //{ }
         //else if (tb_master.SelectedPage == tp_003)
         //{
         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Member_Ship.End_Date <= {0}", Pb_ExpDate3.PickChecked ? (Dt_ExpDate2.Value ?? DateTime.Now ) : DateTime.Now.AddDays((double)Nud_ExpDay2.Value)))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         if(tb_master.SelectedTab == tp_004)         
         {
            Job _InteractWithScsc = null;
            if (Pb_ValidInsrDate4.PickChecked)
            {
               if (FighBs4.Current == null) return;
               string filenos = "";

               foreach (int i in INSR.GetSelectedRows())
               {
                  FighBs4.Position = i;
                  var figh = FighBs4.Current as Data.Fighter;
                  filenos += (filenos.Length == 0 ? "" : ",") + figh.FILE_NO;
               }

               if (filenos.Length == 0) return;

               _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Fighter.File_No IN ( {0} )", filenos))}
                    });
            }
            else
            {
               //_InteractWithScsc =
               //  new Job(SendType.External, "Localhost",
               //     new List<Job>
               //   {
               //      new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Fighter.Insr_Date_Dnrm <= {0}", Pb_ExpDate4.PickChecked ? (Dt_ExpDate4.Value ?? DateTime.Now ) : DateTime.Now.AddDays((double)Nud_ExpDay4.Value)))}
               //   });
            }
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void Btn_Print_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedPage == tp_001)
         //{
         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Member_Ship.End_Date <= {0}", Pb_ExpDate1.PickChecked ? (Dt_ExpDate1.Value ?? DateTime.Now ) : DateTime.Now.AddDays((double)Nud_ExpDay1.Value)))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedPage == tp_002)
         //{}
         //else if (tb_master.SelectedPage == tp_003)
         //{
         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Member_Ship.End_Date <= {0}", Pb_ExpDate3.PickChecked ? (Dt_ExpDate2.Value ?? DateTime.Now ) : DateTime.Now.AddDays((double)Nud_ExpDay2.Value)))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         if (tb_master.SelectedTab == tp_004)
         {
            Job _InteractWithScsc = null;
            if (Pb_ValidInsrDate4.PickChecked)
            {
               if (FighBs4.Current == null) return;
               string filenos = "";

               foreach (int i in INSR.GetSelectedRows())
               {
                  FighBs4.Position = i;
                  var figh = FighBs4.Current as Data.Fighter;
                  filenos += (filenos.Length == 0 ? "" : ",") + figh.FILE_NO;
               }

               if (filenos.Length == 0) return;

               _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Fighter.File_No IN ( {0} )", filenos))}
                    });
            }
            else
            {
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"), string.Format("Fighter.Insr_Date_Dnrm <= {0}", Pb_ExpDate4.PickChecked ? (Dt_ExpDate4.Value ?? DateTime.Now ) : DateTime.Now.AddDays((double)Nud_ExpDay4.Value)))}
                  });
            }
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void Btn_PrintSetting_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedPage == tp_001)
         //{
         //   Job _InteractWithScsc =
         //        new Job(SendType.External, "Localhost",
         //           new List<Job>
         //         {
         //            new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //            new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedPage == tp_002)
         //{
         //   Job _InteractWithScsc =
         //           new Job(SendType.External, "Localhost",
         //              new List<Job>
         //         {
         //            new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //            new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         //else if (tb_master.SelectedPage == tp_003)
         //{
         //   Job _InteractWithScsc =
         //           new Job(SendType.External, "Localhost",
         //              new List<Job>
         //         {
         //            new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //            new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"))}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
         if (tb_master.SelectedTab == tp_004)
         {
            Job _InteractWithScsc =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_004_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void Btn_Attn_Click(object sender, EventArgs e)
      {
         object fileno = "";
         if (Nud_FileNo2.Text == "") Nud_FileNo2.Text = "0";
         try
         {
            if (Pb_FileNo2.PickChecked && Convert.ToInt64(Nud_FileNo2.EditValue) > 0)
            {
               //if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Nud_FileNo2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               //{
                  
               //   /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "localhost",
               //         new List<Job>
               //      {
               //         new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
               //         new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
               //         {
               //            Input = 
               //            new XElement("Fighter",
               //               new XAttribute("fileno", Nud_FileNo2.EditValue)
               //            )
               //         }
               //      })
               //   );
               //}
               //else
               {
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Nud_FileNo2.EditValue), Dt_CrntDate2R.Value.Value, null, "001", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Convert.ToInt64(Nud_FileNo2.EditValue);
               }
            }
            else if (Pb_FileName2.PickChecked && (Lov_FileName2.Text != "" /**/ || Lov_FileName2.EditValue != null))
            {
               //if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Lov_FileName2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               //{
               //   /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "localhost",
               //         new List<Job>
               //      {
               //         new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
               //         new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
               //         {
               //            Input = 
               //            new XElement("Fighter",
               //               new XAttribute("fileno", Lov_FileName2.EditValue)
               //            )
               //         }
               //      })
               //   );
               //}
               //else
               {
                  if(mbsprwno == 0)
                  {
                     throw new Exception("اعضا گرامی دوره ای برای شما وجود ندارد یا دوره شما به پایان رسیده");
                  }
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Lov_FileName2.EditValue), Dt_CrntDate2R.Value.Value, null, "001", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Lov_FileName2.EditValue;
               }
            }

            requry = true;
         }
         catch (Exception ex)
         {
            // 1396/08/01 * 16:02
            // اگر سیستم حضور غیاب دستگاه های کارتی یا انگشتی باشد که مانیتور داشته باشید می توانیم یک پیام برای دستگاه ارسال کنیم که نمایش دهد
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("type", "gatecontrol"),
                              new XAttribute("gateactn", "error")
                           )
                     }
                  }
               )
            );

            //var result = MsgBox.Show(ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MsgBox.Buttons.YesNo, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            //var mtod = iScsc.Member_Ships.FirstOrDefault(mb => mb.FIGH_FILE_NO == (long)Lov_FileName2.EditValue && mb.RWNO == mbsprwno && mb.RECT_CODE == "004").Fighter_Public.Method;
            DialogResult result = DialogResult.None;
            // منشی پشت سیستم حضور دارد
            if( /*mtod.CHCK_ATTN_ALRM == null || mtod.CHCK_ATTN_ALRM*/ chckattnalrm  == "001")
                result = MessageBox.Show(this, ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);


            if(Lov_FileName2.EditValue != null)
            {
               if(FighBs2.List.Count == 0)
                  Execute_Query();
               //Lov_FileName2.Tag = FighBs2.List.OfType<Data.Fighter>().First(f => f.FILE_NO == Convert.ToInt64(Lov_FileName2.EditValue)).FNGR_PRNT_DNRM;
            }

            if(Lov_FileName2.Tag != null && result == DialogResult.Yes)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                        new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("enrollnumber", Lov_FileName2.Tag),
                              new XAttribute("isnewenroll", false)
                           )
                        }
                     })
               );
         }
         finally
         {
            if(requry)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               if (formStatus == "RunInForeground")
               {
                  if (InvokeRequired)
                     Invoke(new Action(() => Btn_Search_Click(null, null)));
                  else
                     Btn_Search_Click(null, null);
               }
               requry = false;

               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting != null && attnNotfSetting.ATTN_NOTF_STAT == "002" && fileno.ToString() != "")
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
                                 new XAttribute("fileno", fileno),
                                 new XAttribute("attndate", /*(AttnBs2.Current as Data.Attendance).ATTN_DATE*/ DateTime.Now.Date),
                                 new XAttribute("gatecontrol", "true"),
                                 new XAttribute("mbsprwno", mbsprwno)
                              )
                           },
                           new Job(SendType.Self, 1 /* Execute GetUi */){Input = "attn_dayn_f"},
                           new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )                        
                        }
                     )
                  );
               }
            }
         }
      }

      private void Btn_Absent_Click(object sender, EventArgs e)
      {
         object fileno = "";
         try
         {
            if (Pb_FileNo2.PickChecked && Convert.ToInt64(Nud_FileNo2.EditValue) > 0)
            {
               if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Nud_FileNo2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               {
                  /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
                        new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", Nud_FileNo2.EditValue)
                           )
                        }
                     })
                  );
               }
               else
               {
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Nud_FileNo2.EditValue), Dt_CrntDate2R.Value, null, "002", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Convert.ToInt64(Nud_FileNo2.EditValue);
               }
            }
            else if (Pb_FileName2.PickChecked && (Lov_FileName2.Text != "" /**/ || Lov_FileName2.EditValue != null))
            {
               if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Lov_FileName2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               {
                  /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
                        new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", Lov_FileName2.EditValue)
                           )
                        }
                     })
                  );
               }
               else
               {
                  if (mbsprwno == 0)
                  {
                     throw new Exception("اعضا گرامی دوره ای برای شما وجود ندارد یا دوره شما به پایان رسیده");
                  }
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Lov_FileName2.EditValue), Dt_CrntDate2R.Value, null, "002", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Lov_FileName2.EditValue;
               }
            }

            requry = true;
         }
         catch (Exception ex)
         {
            // 1396/08/01 * 16:02
            // اگر سیستم حضور غیاب دستگاه های کارتی یا انگشتی باشد که مانیتور داشته باشید می توانیم یک پیام برای دستگاه ارسال کنیم که نمایش دهد
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("type", "gatecontrol"),
                              new XAttribute("gateactn", "error")
                           )
                     }
                  }
               )
            );

            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MsgBox.Buttons.YesNo, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            //var mtod = iScsc.Member_Ships.FirstOrDefault(mb => mb.FIGH_FILE_NO == (long)Lov_FileName2.EditValue && mb.RWNO == mbsprwno && mb.RECT_CODE == "004").Fighter_Public.Method;
            DialogResult result = DialogResult.None;
            if (/*mtod.CHCK_ATTN_ALRM == null || mtod.CHCK_ATTN_ALRM*/ chckattnalrm == "001")
               result = MessageBox.Show(this, ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            

            if (Lov_FileName2.Tag != null && result == DialogResult.Yes)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                        new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("enrollnumber", Lov_FileName2.Tag),
                              new XAttribute("isnewenroll", false)
                           )
                        }
                     })
               );
         }
         finally
         {
            if (requry)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Btn_Search_Click(null, null);
               requry = false;

               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting != null && attnNotfSetting.ATTN_NOTF_STAT == "002" && fileno.ToString() != "")
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
                              new XAttribute("fileno", fileno),
                              new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE),
                              new XAttribute("mbsprwno", mbsprwno)
                           )
                        },
                        new Job(SendType.Self, 1 /* Execute GetUi */){Input = "attn_dayn_f"},
                        new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )                        
                     })
                  );
               }
            }
         }
      }

      private void Btn_AttnWithOutCondition_Click(object sender, EventArgs e)
      {
         object fileno = "";
         try
         {
            if (Pb_FileNo2.PickChecked && Convert.ToInt64(Nud_FileNo2.EditValue) > 0)
            {
               if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Nud_FileNo2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               {
                  /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
                        new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", Nud_FileNo2.EditValue)
                           )
                        }
                     })
                  );
               }
               else
               {
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Nud_FileNo2.EditValue), Dt_CrntDate2R.Value.Value, null, "004", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Convert.ToInt64(Nud_FileNo2.EditValue);
               }
            }
            else if (Pb_FileName2.PickChecked && (Lov_FileName2.Text != "" /**/ || Lov_FileName2.EditValue != null))
            {
               if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Lov_FileName2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               {
                  /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
                        new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", Lov_FileName2.EditValue)
                           )
                        }
                     })
                  );
               }
               else
               {
                  if (mbsprwno == 0)
                  {
                     throw new Exception("اعضا گرامی دوره ای برای شما وجود ندارد یا دوره شما به پایان رسیده");
                  }
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Lov_FileName2.EditValue), Dt_CrntDate2R.Value.Value, null, "004", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Lov_FileName2.EditValue;
               }
            }

            requry = true;
         }
         catch (Exception ex)
         {
            // 1396/08/01 * 16:02
            // اگر سیستم حضور غیاب دستگاه های کارتی یا انگشتی باشد که مانیتور داشته باشید می توانیم یک پیام برای دستگاه ارسال کنیم که نمایش دهد
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("type", "gatecontrol"),
                              new XAttribute("gateactn", "error")
                           )
                     }
                  }
               )
            );

            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MsgBox.Buttons.YesNo, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            //var mtod = iScsc.Member_Ships.FirstOrDefault(mb => mb.FIGH_FILE_NO == (long)Lov_FileName2.EditValue && mb.RWNO == mbsprwno && mb.RECT_CODE == "004").Fighter_Public.Method;
            DialogResult result = DialogResult.None;
            if (/*mtod.CHCK_ATTN_ALRM == null || mtod.CHCK_ATTN_ALRM*/ chckattnalrm == "001")
               result = MessageBox.Show(this, ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

            if (Lov_FileName2.Tag != null && result == DialogResult.Yes)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                        new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("enrollnumber", Lov_FileName2.Tag),
                              new XAttribute("isnewenroll", false)
                           )
                        }
                     })
               );
         }
         finally
         {
            if (requry)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Btn_Search_Click(null, null);
               requry = false;

               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting != null && attnNotfSetting.ATTN_NOTF_STAT == "002" && fileno.ToString() != "")
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
                              new XAttribute("fileno", fileno),
                              new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE),
                              new XAttribute("mbsprwno", mbsprwno)
                           )
                        },
                        new Job(SendType.Self, 1 /* Execute GetUi */){Input = "attn_dayn_f"},
                        new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )                        
                     })
                  );
               }
            }
         }
      }

      private void Btn_AttnWithAnotherCoch_Click(object sender, EventArgs e)
      {
         object fileno = "";
         try
         {
            if (Pb_FileNo2.PickChecked && Convert.ToInt64(Nud_FileNo2.EditValue) > 0)
            {
               if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Nud_FileNo2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               {
                  /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
                        new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", Nud_FileNo2.EditValue)
                           )
                        }
                     })
                  );
               }
               else
               {
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Nud_FileNo2.EditValue), Dt_CrntDate2R.Value.Value, (long)Lov_CochName2.EditValue, "005", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Convert.ToInt64(Nud_FileNo2.EditValue);
               }
            }
            else if (Pb_FileName2.PickChecked && (Lov_FileName2.Text != "" /**/ || Lov_FileName2.EditValue != null))
            {
               if (iScsc.Fighters.Any(f => f.FILE_NO == Convert.ToInt64(Lov_FileName2.EditValue) && f.FGPB_TYPE_DNRM == "009"))
               {
                  /* نمایش فرم مربوط به مشترییات چند جلسه ای ترکیبی */
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 115 /* Execute CHOS_CLAS_F */),
                        new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", Lov_FileName2.EditValue)
                           )
                        }
                     })
                  );
               }
               else
               {
                  if (mbsprwno == 0)
                  {
                     throw new Exception("اعضا گرامی دوره ای برای شما وجود ندارد یا دوره شما به پایان رسیده");
                  }
                  iScsc.INS_ATTN_P(null, Convert.ToInt64(Lov_FileName2.EditValue), Dt_CrntDate2R.Value.Value, (long)Lov_CochName2.EditValue, "005", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                  fileno = Lov_FileName2.EditValue;
               }
            }

            requry = true;
         }
         catch (Exception ex)
         {
            // 1396/08/01 * 16:02
            // اگر سیستم حضور غیاب دستگاه های کارتی یا انگشتی باشد که مانیتور داشته باشید می توانیم یک پیام برای دستگاه ارسال کنیم که نمایش دهد
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("type", "gatecontrol"),
                              new XAttribute("gateactn", "error")
                           )
                     }
                  }
               )
            );

            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MsgBox.Buttons.YesNo, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            //var mtod = iScsc.Member_Ships.FirstOrDefault(mb => mb.FIGH_FILE_NO == (long)Lov_FileName2.EditValue && mb.RWNO == mbsprwno && mb.RECT_CODE == "004").Fighter_Public.Method;
            DialogResult result = DialogResult.None;
            if (/*mtod.CHCK_ATTN_ALRM == null || mtod.CHCK_ATTN_ALRM*/ chckattnalrm == "001")
               result = MessageBox.Show(this, ex.Message + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

            if (Lov_FileName2.Tag != null && result == DialogResult.Yes)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                        new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                        {
                           Input = 
                           new XElement("Fighter",
                              new XAttribute("enrollnumber", Lov_FileName2.Tag),
                              new XAttribute("isnewenroll", false)
                           )
                        }
                     })
               );
         }
         finally
         {
            if (requry)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Btn_Search_Click(null, null);
               requry = false;

               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting != null && attnNotfSetting.ATTN_NOTF_STAT == "002" && fileno.ToString() != "")
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
                                 new XAttribute("fileno", fileno),
                                 new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE),
                                 new XAttribute("mbsprwno", mbsprwno)
                              )
                           },
                           new Job(SendType.Self, 1 /* Execute GetUi */){Input = "attn_dayn_f"},
                           new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )                        
                        })
                  );
               }
            }
         }
      }

      private void Nud_FileNo2_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyData == Keys.Enter)
         {
            Btn_Attn_Click(null, null);
            Nud_FileNo2.Focus();
            Nud_FileNo2.SelectAll();
         }
      }

      private void Btn_Ders_Click(object sender, EventArgs e)
      {
         try
         {
            if (AttnBs2.Current == null) return;

            iScsc.INS_DART_P((AttnBs2.Current as Data.Attendance).CODE);

            requry = true;
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if (requry)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Btn_Search_Click(null, null);
               requry = false;
            }
         }
         
      }

      private void Attn_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if(e.Column.FieldName == "TOTL_IN_TIME" && e.IsGetData)
         {
            if ((view.GetListSourceRowCellValue(e.ListSourceRowIndex, "EXIT_TIME") as TimeSpan?) != null)
               e.Value = (((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "EXIT_TIME")) - ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "ENTR_TIME"))).ToString();
            else
               e.Value = "حضور در باشگاه";
         }
      }

      private void Btn_ShowHowAreYou_Click(object sender, EventArgs e)
      {
         object fileno = "";
         try
         {
            if (Pb_FileNo2.PickChecked && Convert.ToInt64(Nud_FileNo2.EditValue) > 0)
            {
               fileno = Convert.ToInt64(Nud_FileNo2.EditValue);
            }
            else if (Pb_FileName2.PickChecked && (Lov_FileName2.Text != "" /**/ || Lov_FileName2.EditValue != null))
            {
               fileno = Lov_FileName2.EditValue;
            }

            if (fileno.ToString() == "") return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                     new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", fileno),
                           new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE)
                        )
                     }
                  })
            );
         }
         catch
         {

         }
      }

      private void Btn_AutoExitAttn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "با خروج دستی همه مشترییان موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.AUTO_AEXT_P(new XElement("Process"));
            requry = true;
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if(requry)
            {
               requry = false;
               Btn_Search_Click(null, null);
            }
         }
      }

      private void PblcInfo1_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var figh = FighBs1.Current as Data.Fighter;
            Job _InteractWithScsc = null;
            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
                  );
                  break;
               case 1:
                  if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == figh.FILE_NO && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

                  _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
                     });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  break;
               case 2:
                  _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 82 /* Execute Adm_Ends_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO))}
                     });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  break;
               default:
                  break;
            }
         }catch(Exception ex)
         {
            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void PblcInfo2_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var attn = AttnBs2.Current as Data.Attendance;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", attn.FIGH_FILE_NO)) }
                  );
                  break;
               case 1:
                  if (attn.EXIT_TIME == null)
                  {
                     if (MessageBox.Show(this, "با خروج دستی مشتری موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, null, null, "003", mbsprwno == 0 ? null : (short?)mbsprwno, attnsystype, attnignrtype);
                     Btn_Search_Click(null, null);
                  }
                  break;
               case 2:
                  if(attn.ATTN_STAT == "002")
                  {
                     if (MessageBox.Show(this, "با ابطال رکورد مشتری مشتری موافق هستید؟", "ابطال رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.UPD_ATTN_P(
                        new XElement("Process",
                           new XElement("Attendance",
                              new XAttribute("code", attn.CODE),
                              new XAttribute("type", "001") // ابطال رکورد مشتری
                           )
                        )
                     );
                     Btn_Search_Click(null, null);
                  }
                  break;
               case 3:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", (AttnBs2.Current as Data.Attendance).FIGH_FILE_NO),
                                 new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE)
                              )
                           }
                        })
                  );
                  break;
               default:
                  break;
            }
         }catch(Exception ex)
         {
            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void Nud_FileNo2_Enter(object sender, EventArgs e)
      {
         Nud_FileNo2.Focus();
         Nud_FileNo2.SelectAll();
      }
   }
}
