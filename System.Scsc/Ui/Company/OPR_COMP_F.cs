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
using System.Scsc.ExtCode;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.Company
{
   public partial class OPR_COMP_F : UserControl
   {
      public OPR_COMP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            int _cnty = CntyBs.Position;
            int _prvn = PrvnBs.Position;
            int _regn = RegnBs.Position;

            CntyBs.DataSource = iScsc.Countries;

            CntyBs.Position = _cnty;
            PrvnBs.Position = _prvn;
            RegnBs.Position = _regn;

            requery = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void AddComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _regn = RegnBs.Current as Data.Region;
            if (_regn == null) return;

            if (CompBs.List.OfType<Data.Company>().Any(c => c.CODE == 0)) return;

            var _comp = CompBs.AddNew() as Data.Company;
            iScsc.Companies.InsertOnSubmit(_comp);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _comp = CompBs.Current as Data.Company;
            if (_comp == null) return;

            if (_comp.Company_Fighters.Any() && MessageBox.Show(this, "ارتباط شرکت و مشتری در این رکورد وجود دارد آیا مایل به ادامه دادن هستید؟", "رکورد های مرتبط به شرکت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            iScsc.Companies.DeleteOnSubmit(_comp);
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

      private void SaveComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CompBs.EndEdit();
            Comp_Gv.PostEditor();

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

      private void CELL_PHON_TextEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         Comp_Gv.FindFilterText = CELL_PHON_TextEdit.Text;
      }

      private void FindFigh_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FighBs.DataSource =
               iScsc.Fighters.
               Where(f => 
                  (f.FRST_NAME_DNRM == null || f.FRST_NAME_DNRM.Contains(FrstName_Txt.Text)) &&
                  (f.LAST_NAME_DNRM == null || f.LAST_NAME_DNRM.Contains(LastName_Txt.Text)) &&
                  (f.CELL_PHON_DNRM == null || f.CELL_PHON_DNRM.Contains(CellPhon_Txt.Text)) &&
                  (f.NATL_CODE_DNRM == null || f.NATL_CODE_DNRM.Contains(NatlCode_Txt.Text)) &&
                  f.CONF_STAT == "002"
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FighActn_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _comp = CompBs.Current as Data.Company;
            if (_comp == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _figh = FighBs.Current as Data.Fighter;
                  if (_figh == null) return;

                  iScsc.ExecuteCommand(
                     string.Format("INSERT INTO dbo.Company_Fighter (Comp_Code, Figh_File_No, Code) VALUES ({0}, {1}, 0);", _comp.CODE, _figh.FILE_NO)
                  );
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

      private void CmfgActn_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _cmfg = CmfgBs.Current as Data.Company_Fighter;
            if (_cmfg == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  // Delete Record
                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(string.Format("DELETE dbo.Company_Fighter WHERE Code = {0};", _cmfg.CODE));
                  break;
               case 1:
                  // Submit Change
                  Cmfg_Gv.PostEditor();

                  iScsc.SubmitChanges();
                  break;
               case 2:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Company_Fighter_Relationship_INFO"),
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

      private void Rshp_Butn_Click(object sender, EventArgs e)
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
                           new XAttribute("tablename", "Company_Fighter_Relationship_INFO"),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void Tag_Butn_Click(object sender, EventArgs e)
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
                           new XAttribute("tablename", "Company_Tag_INFO"),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void ActnTag_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _comp = CompBs.Current as Data.Company;
            if (_comp == null) return;

            var _tag = TagBs.Current as Data.App_Base_Define;
            if (_tag == null) return;
            
            switch (e.Button.Index)
            {
               case 0:
                  iScsc.ExecuteCommand(
                     string.Format("INSERT INTO dbo.Company_Tag(Comp_Code, Tag_Apbs_Code, Code) VALUES({0}, {1}, 0);", _comp.CODE, _tag.CODE)
                  );
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

      private void ActnCmtg_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _cmtg = CmtgBs.Current as Data.Company_Tag;
            if (_cmtg == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  // Delete Record
                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(string.Format("DELETE dbo.Company_Tag WHERE Code = {0};", _cmtg.CODE));
                  break;
               case 1:
                  // Submit Change
                  Cmtg_Gv.PostEditor();

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

      private void FindComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            List<long?> _CmfgLst = new List<long?>();
            List<long?> _CmtgLst = new List<long?>();

            if(CompTagFltr_Cbx.Checked)
               foreach (var i in CompTag_Gv.GetSelectedRows())
               {
                  var _item = CompTag_Gv.GetRow(i) as Data.App_Base_Define;
                  _CmtgLst.Add(
                     _item.CODE
                  );
               }

            if(RshpCmfgFltr_Cbx.Checked)
               foreach (var i in RshpCmfg_Gv.GetSelectedRows())
               {
                  var _item = RshpCmfg_Gv.GetRow(i) as Data.App_Base_Define;
                  _CmfgLst.Add(
                     _item.CODE
                  );
               }

            FindCompBs.DataSource =
               iScsc.Companies.
               Where(c => 
                  (!CompName_Cbx.Checked && !CompPostAdrs_Cbx.Checked && !CompCellPhon_Cbx.Checked && 
                  !CompTellPhon_Cbx.Checked && !CompSite_Cbx.Checked && !CompFax_Cbx.Checked &&
                  !CompDesc_Cbx.Checked && !CompFromAmntDebt_Cbx.Checked && !CompToAmntDebt_Cbx.Checked &&
                  !CompFromAmntDpst_Cbx.Checked && !CompToAmntDpst_Cbx.Checked && !CompTagFltr_Cbx.Checked &&
                  !FighFrstName_Cbx.Checked && !FighLastName_Cbx.Checked && !FighCellPhon_Cbx.Checked &&
                  !FighNatlCode_Cbx.Checked && !RshpCmfgFltr_Cbx.Checked) ||
                  (CompName_Cbx.Checked && (c.NAME == null || c.NAME.Contains(CompName_Txt.Text))) ||
                  (CompPostAdrs_Cbx.Checked && (c.POST_ADRS == null || c.POST_ADRS.Contains(CompPostAdrs_Txt.Text))) ||
                  (CompCellPhon_Cbx.Checked && (c.CELL_PHON == null || c.CELL_PHON.Contains(CompCellPhon_Txt.Text))) ||
                  (CompTellPhon_Cbx.Checked && (c.TELL_PHON == null || c.TELL_PHON.Contains(CompTellPhon_Txt.Text))) ||
                  (CompSite_Cbx.Checked && (c.WEB_SITE == null || c.WEB_SITE.Contains(CompSite_Txt.Text))) ||
                  (CompFax_Cbx.Checked && (c.FAX_NUMB == null || c.FAX_NUMB.Contains(CompFax_Txt.Text))) ||
                  (CompDesc_Cbx.Checked && (c.COMP_DESC == null || c.COMP_DESC.Contains(CompDesc_Txt.Text))) ||
                  (CompFromAmntDebt_Cbx.Checked && (c.DEBT_DNRM == null || c.DEBT_DNRM >= CompFromAmntDebt_Txt.Text.ToInt64())) ||
                  (CompToAmntDebt_Cbx.Checked && (c.DEBT_DNRM == null || c.DEBT_DNRM <= CompToAmntDebt_Txt.Text.ToInt64())) ||
                  (CompFromAmntDpst_Cbx.Checked && (c.CRDT_AMNT == null || c.CRDT_AMNT >= CompFromAmntDpst_Txt.Text.ToInt64())) ||
                  (CompToAmntDpst_Cbx.Checked && (c.CRDT_AMNT == null || c.CRDT_AMNT <= CompToAmntDpst_Txt.Text.ToInt64())) ||
                  (CompTagFltr_Cbx.Checked && c.Company_Tags.Any(ct => _CmtgLst.Contains(ct.TAG_APBS_CODE))) ||
                  (
                     (  FighFrstName_Cbx.Checked ||
                        FighLastName_Cbx.Checked ||
                        FighCellPhon_Cbx.Checked ||
                        FighNatlCode_Cbx.Checked || 
                        RshpCmfgFltr_Cbx.Checked)
                     && 
                     (
                        c.Company_Fighters.Any(cf => 
                           (FighFrstName_Cbx.Checked && cf.Fighter.FRST_NAME_DNRM.Contains(FighFrstName_Txt.Text)) ||
                           (FighLastName_Cbx.Checked && cf.Fighter.LAST_NAME_DNRM.Contains(FighLastName_Txt.Text)) ||
                           (FighCellPhon_Cbx.Checked && cf.Fighter.CELL_PHON_DNRM.Contains(FighCellPhon_Txt.Text)) ||
                           (FighNatlCode_Cbx.Checked && cf.Fighter.NATL_CODE_DNRM.Contains(FighNatlCode_Txt.Text)) ||
                           (RshpCmfgFltr_Cbx.Checked && _CmfgLst.Contains(cf.ROLE_APBS_CODE))
                        )
                     )
                  )
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FindCompActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _findcomp = FindCompBs.Current as Data.Company;
            if (_findcomp == null) return;

            CntyBs.Position = CntyBs.IndexOf(_findcomp.Region.Province.Country);
            PrvnBs.Position = PrvnBs.IndexOf(_findcomp.Region.Province);
            RegnBs.Position = RegnBs.IndexOf(_findcomp.Region);

            CompBs.Position = CompBs.IndexOf(_findcomp);

            tb_master.SelectedTab = tp_001;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
