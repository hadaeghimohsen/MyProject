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
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using System.Diagnostics;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class TREE_BASE_F : UserControl
   {
      public TREE_BASE_F()
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

         int trbs = TrbsBs.Position;
         int unit = UnitBs.Position;
         int rbsc = RbscBs.Position;
         int rbss = RbssBs.Position;
         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int srbt = SrbtBs.Position;
         int sral = SralBs.Position;

         TrbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "TREEBASEACCOUNT_INFO");
         UnitBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "PRODUCTUNIT_INFO");
         RbscBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "SECTION_INFO");
         RbssBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "STORESPEC_INFO");
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));
         ExtrServBs.DataSource = iRoboTech.V_External_Services;

         TrbsBs.Position = trbs;
         UnitBs.Position = unit;
         RbscBs.Position = rbsc;
         RbssBs.Position = rbss;
         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         SrbtBs.Position = srbt;
         SralBs.Position = sral;
         requery = false;
      }

      private void AddTrbs_Butn_Click(object sender, EventArgs e)
      {
         if (TrbsBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var trbs = TrbsBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(trbs);
         trbs.ENTY_NAME = "TREEBASEACCOUNT_INFO";
         trbs.LEVL_LENT = 1;
         trbs.RWNO = (TrbsBs.List.OfType<Data.App_Base_Define>().Where(t => t.REF_CODE == trbs.REF_CODE).Max(t => t.RWNO) ?? 0) + 1;
      }

      private void DelTrbs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var trbs = TrbsBs.Current as Data.App_Base_Define;
            if (trbs == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            iRoboTech.DEL_APBS_P(trbs.CODE);

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

      private void SaveTrbs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            TrbsBs.EndEdit();
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
               Execute_Query();
         }
      }

      private void AddUnit_Butn_Click(object sender, EventArgs e)
      {
         if (UnitBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var unit = UnitBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(unit);
         unit.ENTY_NAME = "PRODUCTUNIT_INFO";
      }

      private void DelUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var unit = UnitBs.Current as Data.App_Base_Define;
            if (unit == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;
            iRoboTech.DEL_APBS_P(unit.CODE);

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

      private void SaveUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            UnitBs.EndEdit();
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
               Execute_Query();
         }
      }

      private void RootTrbs_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var trbs = TrbsBs.Current as Data.App_Base_Define;
            if(trbs == null)return;

            if(e.Button.Index == 1)
            {
               RootTrbs_Lov.EditValue = trbs.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RootUnit_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var unit = UnitBs.Current as Data.App_Base_Define;
            if (unit == null) return;

            if (e.Button.Index == 1)
            {
               RootUnit_Lov.EditValue = unit.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddRbsc_Butn_Click(object sender, EventArgs e)
      {
         if (RbscBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var rbsc = RbscBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(rbsc);
         rbsc.ENTY_NAME = "SECTION_INFO";
      }

      private void DelRbsc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbsc = RbscBs.Current as Data.App_Base_Define;
            if (rbsc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;
            iRoboTech.DEL_APBS_P(rbsc.CODE);

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

      private void SaveRbsc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RbscBs.EndEdit();
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
               Execute_Query();
         }
      }

      private void RootRbsc_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rbsc = RbscBs.Current as Data.App_Base_Define;
            if (rbsc == null) return;

            if (e.Button.Index == 1)
            {
               RootRbsc_Lov.EditValue = rbsc.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RootRbss_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rbss = RbssBs.Current as Data.App_Base_Define;
            if (rbss == null) return;

            if (e.Button.Index == 1)
            {
               RootRbss_Lov.EditValue = rbss.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddRbss_Butn_Click(object sender, EventArgs e)
      {
         if (RbssBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var rbss = RbssBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(rbss);
         rbss.ENTY_NAME = "STORESPEC_INFO";
      }

      private void DelRbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbss = RbssBs.Current as Data.App_Base_Define;
            if (rbss == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;
            iRoboTech.DEL_APBS_P(rbss.CODE);

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

      private void SaveRbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RbssBs.EndEdit();
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
               Execute_Query();
         }
      }

      private void RootSrbt_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  //RootSrbt_Lov.EditValue = srbt.REF_CHAT_ID = null;
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

      private void AddSrbt_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         if (SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.SERV_FILE_NO == 0)) return;

         var srbt = SrbtBs.AddNew() as Data.Service_Robot;
         srbt.Robot = robo;
         srbt.REGS_TYPE = "002"; // ثبت دستی
         srbt.STAT = "002"; // فعال

         if (SrbtBs.List.OfType<Data.Service_Robot>().Count(sr => sr.CHAT_ID > 0) > 0)
            srbt.CHAT_ID = SrbtBs.List.OfType<Data.Service_Robot>().Where(sr => sr.CHAT_ID > 0).Max(sr => sr.CHAT_ID) + 1;
         else
            srbt.CHAT_ID = 1;

         iRoboTech.Service_Robots.InsertOnSubmit(srbt);
      }

      private void DelSrbt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaveSrbt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SrbtBs.EndEdit();

            if(SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.SERV_FILE_NO == 0 && (iRoboTech.CHK_MOBL_U(sr.CELL_PHON) == 0)))
            {
               if (MessageBox.Show(this, "اطلاعات شماره موبایل درست وارد نشده، آیا با ادامه ثبت اطلاعات موافق هستید؟", "اطلاعات تکراری", MessageBoxButtons.YesNo) != DialogResult.Yes) { ImpAll_Tm.Enabled = false; return; }
            }

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
               Execute_Query();
         }
      }

      private void RefChatId_Txt_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            srbt.REF_CHAT_ID = srbt.REF_SERV_FILE_NO = srbt.REF_ROBO_RBID = null;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddSral_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            if (SralBs.List.OfType<Data.Service_Robot_Account_Link>().Any(a => a.CODE == 0)) return;

            var sral = SralBs.AddNew() as Data.Service_Robot_Account_Link;
            sral.Service_Robot = srbt;

            iRoboTech.Service_Robot_Account_Links.InsertOnSubmit(sral);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelSral_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sral = SralBs.Current as Data.Service_Robot_Account_Link;
            if (sral == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if(iRoboTech.Orders.Any(o => o.APBS_CODE == sral.CODE))

            iRoboTech.Service_Robot_Account_Links.DeleteOnSubmit(sral);

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
               Execute_Query();
         }
      }

      private void SaveSral_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Sral_gv.PostEditor();

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
               Execute_Query();
         }
      }

      private void Input_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var _qury = 
               ExtrServBs.List.OfType<Data.V_External_Service>()
               .Where(es => 
                  (NatlCode_Rb.Checked && es.NATL_CODE_DNRM != null && es.NATL_CODE_DNRM.Contains(e.NewValue.ToString())) || 
                  (CellPhon_Rb.Checked && es.CELL_PHON_DNRM != null && es.CELL_PHON_DNRM.Contains(e.NewValue.ToString()))
                ).ToList();

            Rslt_Butn.Tag = null;

            if (_qury == null || _qury.Count() == 0) return;

            Rslt_Butn.Text = _qury.Count() > 1 ? _qury.Count().ToString() : _qury.Count() == 1 ? _qury.FirstOrDefault().NAME_DNRM : "0";
            Rslt_Butn.Tag = _qury;
            
            Rslt_Butn.Enabled = 
               _qury.Count() == 1 && 
               !SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.CELL_PHON == _qury.FirstOrDefault().CELL_PHON_DNRM)
               ? true : false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Rslt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Rslt_Butn.Tag == null) return;
            var _es = (Rslt_Butn.Tag as List<Data.V_External_Service>).FirstOrDefault();
            AddSrbt_Butn_Click(null, null);
            var _newserv = SrbtBs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => sr.SERV_FILE_NO == 0);
            _newserv.REAL_FRST_NAME = _es.FRST_NAME_DNRM;
            _newserv.REAL_LAST_NAME = _es.LAST_NAME_DNRM;
            _newserv.NAME = _es.FRST_NAME_DNRM + " " + _es.LAST_NAME_DNRM;
            _newserv.CELL_PHON = _es.CELL_PHON_DNRM;
            _newserv.NATL_CODE = _es.NATL_CODE_DNRM;

            var _histServ = iRoboTech.Service_Robots.Where(sr => (sr.ROBO_RBID == 391 || sr.ROBO_RBID == 401) && _newserv.ROBO_RBID != sr.ROBO_RBID && _newserv.CELL_PHON == sr.CELL_PHON).FirstOrDefault();
            if (_histServ != null)
               _newserv.CHAT_ID = _histServ.CHAT_ID;

            SaveSrbt_Butn_Click(null, null);

            Input_Txt.Text = "";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SyncChatId_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iRoboTech.Sync_S2db_P(
               new XElement("Service",
                   new XAttribute("newchatid", newChatId_Txt.Text),
                   new XAttribute("oldchatid", oldChatId_Txt.Text)
               )
            );

            newChatId_Txt.Text = oldChatId_Txt.Text = "";
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

      private void ImpAll_Tm_Tick(object sender, EventArgs e)
      {
         try
         {
            ImpAll_Tm.Enabled = false;
            var _qury = ExtrServBs.List.OfType<Data.V_External_Service>().Where(es => es.CELL_PHON_DNRM != null && iRoboTech.CHK_MOBL_U(es.CELL_PHON_DNRM) == 1 && !SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.CELL_PHON == es.CELL_PHON_DNRM));
            int i = 0;
            ImpAll_Pb.Visible = true;
            foreach (var serv in _qury)
            {
               ++i;
               CellPhon_Rb.Checked = true;
               Input_Txt.Text = serv.CELL_PHON_DNRM;
               Rslt_Butn_Click(null, null);
               ImpAll_Butn.Text = string.Format("{0} / {1}", i, _qury.Count());
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            ImpAll_Butn.Text = "انجام عملیات";
            ImpAll_Pb.Visible = false;            
         }
      }

      private void ImpAll_Butn_Click(object sender, EventArgs e)
      {
         var _qury = ExtrServBs.List.OfType<Data.V_External_Service>().Where(es => es.CELL_PHON_DNRM != null && iRoboTech.CHK_MOBL_U(es.CELL_PHON_DNRM) == 1 && !SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.CELL_PHON == es.CELL_PHON_DNRM));
         if (MessageBox.Show(this, "آیا با ثبت اطلاعات کلیه مشتریان آرتا موافق هستید؟" + "\n" + "تعداد کل مشتریان: " + _qury.Count().ToString(), "ورود دسته ای اطلاعات", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

         ImpAll_Tm.Enabled = true;
      }

      private void NewCellPhon_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var _qury =
               ExtrServBs.List.OfType<Data.V_External_Service>()
               .Where(es =>
                  (NatlCode_Rb.Checked && es.NATL_CODE_DNRM != null && es.NATL_CODE_DNRM.Contains(e.NewValue.ToString())) ||
                  (CellPhon_Rb.Checked && es.CELL_PHON_DNRM != null && es.CELL_PHON_DNRM.Contains(e.NewValue.ToString()))
                ).ToList();

            Rslt_Butn.Tag = null;

            if (_qury == null || _qury.Count() == 0) return;

            Rslt_Butn.Text = _qury.Count() > 1 ? _qury.Count().ToString() : _qury.Count() == 1 ? _qury.FirstOrDefault().NAME_DNRM : "0";
            Rslt_Butn.Tag = _qury;

            Rslt_Butn.Enabled =
               _qury.Count() == 1 &&
               !SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.CELL_PHON == _qury.FirstOrDefault().CELL_PHON_DNRM)
               ? true : false;
            
            if(Rslt_Butn.Enabled)
            {
               MessageBox.Show(this, "این مشتری درون سیستم ارتا تعریف شده و نیازی به تعریف مجدد نمیباشد" ,"اطلاعات تکراری");

               Input_Txt.EditValue = e.NewValue;
               Rslt_Butn_Click(null, null);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
