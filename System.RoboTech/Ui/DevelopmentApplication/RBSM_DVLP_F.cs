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
using System.RoboTech.ExceptionHandlings;
using System.Xml.Linq;
using System.IO;
using System.Drawing.Imaging;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class RBSM_DVLP_F : UserControl
   {
      public RBSM_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int rspg = RspgBs.Position;
         int rsgm = RsgmBs.Position;
         int rsgd = RsgdBs.Position;
        
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         RspgBs.Position = rspg;
         RsgmBs.Position = rsgm;
         RsgdBs.Position = rsgd;

         Search_Butn_Click(null, null);

         requery = false;
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Invalidate();

            Rsgm_Gv.PostEditor();
            Rsgd_Gv.PostEditor();
            RsgmBs.EndEdit();
            RsgdBs.EndEdit();

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
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

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if(robo == null)return;

            var grop = RspgBs.Current as Data.Robot_Spy_Group;

            long? chatid = null;
            if (Serv_Lov.EditValue != null && Serv_Lov.EditValue.ToString() != "")
               chatid = (long)Serv_Lov.EditValue;

            string emlttype = null;
            if (ElmtType_Lov.EditValue != null && ElmtType_Lov.EditValue.ToString() != "")
               emlttype = ElmtType_Lov.EditValue.ToString();

            DateTime? mesgdate = null;
            if (MesgDate_Dat.Value.HasValue)
               mesgdate = MesgDate_Dat.Value.Value.Date;

            string mesgcmnt = null;
            if (MesgCmnt_Txt.EditValue != null && MesgCmnt_Txt.EditValue.ToString() != "")
               mesgcmnt = MesgCmnt_Txt.EditValue.ToString();

            RsgmBs.DataSource =
               iRoboTech.ExecuteQuery<Data.Robot_Spy_Group_Message>(
                  string.Format(
                     @"SELECT * 
                         FROM dbo.[Robot_Spy_Group_Message] o
                        WHERE o.RSPG_ROBO_RBID = {0}
                          AND o.Rspg_Grop_Code = {1}
                          AND o.Chat_Id = {2}
                          AND o.Mesg_Type = {3}
                          AND CAST(o.Mesg_Date AS DATE) = {4}
                          AND {5}
                          AND ( {6} )",
                     robo.RBID,
                     grop.GROP_CODE,
                     (chatid == null ? "Chat_ID" : chatid.ToString()),                     
                     (emlttype == null ? "Mesg_Type" : "\'" + emlttype + "\'"),
                     (mesgdate == null ? "CAST(Mesg_DATE AS DATE)" : "'" + mesgdate.Value.ToString("yyyy-MM-dd") + "'") ,
                     (mesgcmnt == null ? "1=1" : string.Format("Mesg_Text LIKE N'%{0}%'", mesgcmnt.Replace(' ', '%'))),
                     (ContTypeStat_Ckb.Checked ? "0 = (SELECT count(D.Code) FROM Robot_Spy_Group_Message_Detail d WHERE o.Code = d.Rsgm_Code AND d.Stat = '002')" : "o.Cont_Type IS NOT NULL AND EXISTS(SELECT * FROM Robot_Spy_Group_Message_Detail d WHERE o.Code = d.Rsgm_Code AND d.Stat = '002')")
                  )
               ).ToList();
         }
         catch (Exception exc)
         {
            iRoboTech.SaveException(exc);
         }
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) { RsgmBs.List.Clear(); return; }

            Search_Butn_Click(null, null);

         }
         catch (Exception exc)
         {
            iRoboTech.SaveException(exc);
         }
      }

      private void ClearParm_Butn_Click(object sender, EventArgs e)
      {
         Serv_Lov.EditValue = null;
         ElmtType_Lov.EditValue = null;
         MesgDate_Dat.Value = null;
         Search_Butn_Click(null, null);
      }

      private void ExcelResult_Butn_Click(object sender, EventArgs e)
      {
         if (RsgmBs.Current == null) return;
         var crnt = RsgmBs.Current as Data.Robot_Spy_Group_Message;

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.CODE))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SettingReport_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 14 /* Execute Stng_Rprt_F */),
                  new Job(SendType.SelfToUserInterface, "STNG_RPRT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SelectOtherReport_Butn_Click(object sender, EventArgs e)
      {
         if (RsgmBs.Current == null) return;
         var crnt = RsgmBs.Current as Data.Order;

         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         var grop = RspgBs.Current as Data.Robot_Spy_Group;

         long? chatid = null;
         if (Serv_Lov.EditValue != null && Serv_Lov.EditValue.ToString() != "")
            chatid = (long)Serv_Lov.EditValue;

         string emlttype = null;
         if (ElmtType_Lov.EditValue != null && ElmtType_Lov.EditValue.ToString() != "")
            emlttype = ElmtType_Lov.EditValue.ToString();

         DateTime? mesgdate = null;
         if (MesgDate_Dat.Value.HasValue)
            mesgdate = MesgDate_Dat.Value.Value.Date;

         string mesgcmnt = null;
         if (MesgCmnt_Txt.EditValue != null && MesgCmnt_Txt.EditValue.ToString() != "")
            mesgcmnt = MesgCmnt_Txt.EditValue.ToString();

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 15 /* Execute Rpt_Mngr_F */){
                        Input = 
                           new XElement("Print", new XAttribute("type", "Selection"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                              string.Format(
                                 @"AND a.RSPG_ROBO_RBID = {0}
                                   AND a.Rspg_Grop_Code = {1}
                                   AND a.Chat_Id = {2}
                                   AND a.Mesg_Type = {3}
                                   AND CAST(a.Mesg_Date AS DATE) = {4}
                                   AND {5}
                                   AND ( {6} )",
                                 robo.RBID,
                                 grop.GROP_CODE,
                                 (chatid == null ? "a.Chat_ID" : chatid.ToString()),                     
                                 (emlttype == null ? "a.Mesg_Type" : "\'" + emlttype + "\'"),
                                 (mesgdate == null ? "CAST(a.Mesg_DATE AS DATE)" : "'" + mesgdate.Value.ToString("yyyy-MM-dd") + "'") ,
                                 (mesgcmnt == null ? "1=1" : string.Format("a.Mesg_Text LIKE N'%{0}%'", mesgcmnt.Replace(' ', '%'))),
                                 (ContTypeStat_Ckb.Checked ? "0 = (SELECT count(D.Code) FROM Robot_Spy_Group_Message_Detail d WHERE a.Code = d.Rsgm_Code AND d.Stat = '002')" : "a.Cont_Type IS NOT NULL AND EXISTS(SELECT * FROM Robot_Spy_Group_Message_Detail d WHERE a.Code = d.Rsgm_Code AND d.Stat = '002')")
                              )
                           )
                     }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SelectDefaultReport_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         var grop = RspgBs.Current as Data.Robot_Spy_Group;

         long? chatid = null;
         if (Serv_Lov.EditValue != null && Serv_Lov.EditValue.ToString() != "")
            chatid = (long)Serv_Lov.EditValue;

         string emlttype = null;
         if (ElmtType_Lov.EditValue != null && ElmtType_Lov.EditValue.ToString() != "")
            emlttype = ElmtType_Lov.EditValue.ToString();

         DateTime? mesgdate = null;
         if (MesgDate_Dat.Value.HasValue)
            mesgdate = MesgDate_Dat.Value.Value.Date;

         string mesgcmnt = null;
         if (MesgCmnt_Txt.EditValue != null && MesgCmnt_Txt.EditValue.ToString() != "")
            mesgcmnt = MesgCmnt_Txt.EditValue.ToString();

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 15 /* Execute Rpt_Mngr_F */){
                        Input = 
                           new XElement("Print", new XAttribute("type", "Default"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                              string.Format(
                                 @"AND a.RSPG_ROBO_RBID = {0}
                                   AND a.Rspg_Grop_Code = {1}
                                   AND a.Chat_Id = {2}
                                   AND a.Mesg_Type = {3}
                                   AND CAST(a.Mesg_Date AS DATE) = {4}
                                   AND {5}
                                   AND ( {6} )",
                                 robo.RBID,
                                 grop.GROP_CODE,
                                 (chatid == null ? "a.Chat_ID" : chatid.ToString()),                     
                                 (emlttype == null ? "a.Mesg_Type" : "\'" + emlttype + "\'"),
                                 (mesgdate == null ? "CAST(a.Mesg_DATE AS DATE)" : "'" + mesgdate.Value.ToString("yyyy-MM-dd") + "'") ,
                                 (mesgcmnt == null ? "1=1" : string.Format("a.Mesg_Text LIKE N'%{0}%'", mesgcmnt.Replace(' ', '%'))),
                                 (ContTypeStat_Ckb.Checked ? "0 = (SELECT count(D.Code) FROM Robot_Spy_Group_Message_Detail d WHERE a.Code = d.Rsgm_Code AND d.Stat = '002')" : "a.Cont_Type IS NOT NULL AND EXISTS(SELECT * FROM Robot_Spy_Group_Message_Detail d WHERE a.Code = d.Rsgm_Code AND d.Stat = '002')")
                              )
                           )
                     }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }      
      
      private void Gmtp_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var rsgd = RsgdBs.Current as Data.Robot_Spy_Group_Message_Detail;
         if(rsgd == null)return;

         switch (e.Button.Index)
         {
            case 0:
               rsgd.STAT = "002";
               break;
            case 1:
               rsgd.STAT = "001";
               break;
            default:
               break;
         }
      }      
   }
}
