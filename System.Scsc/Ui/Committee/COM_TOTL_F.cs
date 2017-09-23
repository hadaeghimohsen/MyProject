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
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Committee
{
   public partial class COM_TOTL_F : UserControl
   {
      public COM_TOTL_F()
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
                     rqst.RQTP_CODE == "015" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               );
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
            RQIDTextEdit.Focus();
         }
      }

      LookUpEdit lov_prvn;
      private void REGN_PRVN_CODELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         lov_prvn = sender as LookUpEdit;
         REGN_CODELookUpEdit.EditValue = null;
         RegnBs1.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == lov_prvn.EditValue.ToString() && Fga_Urgn_U.Split(',').Contains(r.PRVN_CODE + r.CODE));

         REGN_CODELookUpEdit_EditValueChanged(REGN_CODELookUpEdit, e);
      }

      LookUpEdit lov_regn;
      private void REGN_CODELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            lov_regn = sender as LookUpEdit;
            if (lov_regn.EditValue == null || lov_regn.EditValue.ToString().Length != 3) return;
            FighsBs1.DataSource = iScsc.Fighters.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE_DNRM) && cbmt.CONF_STAT == "002" && (cbmt.REGN_PRVN_CODE + cbmt.REGN_CODE).Contains(lov_prvn.EditValue.ToString() + lov_regn.EditValue.ToString()))/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
         }
         catch{}
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            RqstBs1.EndEdit();
            Validate();

            var rqst = RqstBs1.Current as Data.Request;
            if(rqst == null) return;
            
            iScsc.COM_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue ?? ""),
                     new XAttribute("regncode", REGN_CODELookUpEdit.EditValue ?? ""),
                     new XAttribute("lettno", rqst == null ? "" : rqst.LETT_NO),
                     new XAttribute("lettdate", rqst == null ? "" : rqst.LETT_DATE.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("lettownr", rqst == null ? "" : rqst.LETT_OWNR),
                     RqroBs1.List.OfType<Data.Request_Row>().ToList()
                     .Select(rr => 
                        new XElement("Request_Row", 
                           new XAttribute("fileno", rr.FIGH_FILE_NO)
                        )
                     ),
                     CommBs1.List.OfType<Data.Committee>().ToList()
                     .Select(c => 
                        new XElement("Committee",
                           new XAttribute("commtype", c.COMM_TYPE ?? ""),
                           new XAttribute("cmid", c.CMID),
                           c.Meetings.Select(m =>
                              new XElement("Meeting",
                                 new XAttribute("mtid", m.MTID),                                 
                                 new XAttribute("actndate", m.ACTN_DATE.Value.ToString("yyyy-MM-dd")),
                                 new XAttribute("meetstat", m.MEET_STAT ?? ""),
                                 new XAttribute("strttime", m.STRT_TIME.Value.ToString()),
                                 new XAttribute("endtime", m.END_TIME.Value.ToString()),
                                 new XAttribute("meetplac", m.MEET_PLAC ?? ""),
                                 new XAttribute("meetsubj", m.MEET_SUBJ ?? ""),
                                 m.Meeting_Comments.Select(mc =>
                                    new XElement("Meeting_Comment",
                                       new XAttribute("mcid", mc.MCID),
                                       new XAttribute("rspnimpl", mc.RSPN_IMPL ?? ""),
                                       new XAttribute("expdate", mc.EXP_DATE.Value.ToString("yyyy-MM-dd")),
                                       mc.CMNT
                                    )
                                 ),
                                 m.Presents.Select(p =>
                                    new XElement("Present", 
                                       new XAttribute("prid", p.PRID),
                                       new XAttribute("prsntype", p.PRSN_TYPE ?? ""),
                                       new XAttribute("fgpbfighfileno", p.FGPB_FIGH_FILE_NO ?? 0),
                                       new XAttribute("invtby", p.INVT_BY ?? 0),
                                       new XAttribute("frstname", p.FRST_NAME ?? ""),
                                       new XAttribute("lastname", p.LAST_NAME ?? ""),
                                       new XAttribute("fathname", p.FATH_NAME ?? ""),
                                       new XAttribute("natlcode", p.NATL_CODE ?? ""),
                                       new XAttribute("sextype", p.SEX_TYPE ?? ""),
                                       new XAttribute("cellphon", p.CELL_PHON ?? ""),
                                       new XElement("Person_Desc", p.PRSN_DESC ?? ""),
                                       p.Present_Comments.Select(pc =>
                                          new XElement("Present_Comment",
                                             new XAttribute("pcid", pc.PCID),
                                             pc.CMNT ?? ""
                                          )
                                       )
                                    )
                                 )
                              )
                           )
                        )
                     )

                  )
               )
            );

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
            iScsc.CNCL_RQST_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );
            
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

      private void Btn_RqstBnASav1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnNew1_Click(object sender, EventArgs e)
      {
         Data.Request rqst = new Data.Request();
         rqst.Committees = new System.Data.Linq.EntitySet<Data.Committee>()
         {
            new Data.Committee(){ COMM_TYPE = "001" }
         };
         RqstBs1.List.Add(rqst);
         RqstBs1.MoveLast();
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
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
      #region Print Configuration
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
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
         }         
      }
      #endregion

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqroBs1.Current == null) return;
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

   }
}
