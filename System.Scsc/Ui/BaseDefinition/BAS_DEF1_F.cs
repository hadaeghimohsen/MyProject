using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using DevExpress.XtraTab;
using System.MaxUi;
using System.Scsc.ExtCode;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_DEF1_F : UserControl
   {
      public BAS_DEF1_F()
      {
         InitializeComponent();

         Flp_001.EnableDragScroll();
         Flp_002.EnableDragScroll();
         Flp_003.EnableDragScroll();
         Flp_004.EnableDragScroll();
         Flp_005.EnableDragScroll();
         Flp_006.EnableDragScroll();
      }

      private bool requery = false, isLoading = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      #region Execute Query and Reload data and triggers
      private async Task Execute_Query()
      {
         isLoading = true;
         await Task.Yield();
         iScsc = new Data.iScscDataContext(ConnectionString);

         switch (Master_Tc.SelectedTabPage.Name)
         {
            case "xTp_001":
               EpitBs.DataSource = iScsc.Expense_Items.Where(a => a.TYPE == (Ittp001_Rb.Checked ? "001" : "002") );
               break;
            case "xTp_002":
               CntyBs.DataSource = iScsc.Countries;
               break;
            case "xTp_003":
               if ((ExpnEpit_Lov.EditValue == null || ExpnEpit_Lov.EditValue.ToString() == "") && EpitBs.List.OfType<Data.Expense_Item>().Any(ei => ei.TYPE == "001" && ei.EPIT_DESC.Contains("شهریه")))
                  ExpnEpit_Lov.EditValue = EpitBs.List.OfType<Data.Expense_Item>().FirstOrDefault(ei => ei.TYPE == "001" && ei.EPIT_DESC.Contains("شهریه")).CODE;

               // مقادیر ثابت
               var _trgtMtodStat = MtodStat_Fcb.SelectedIndex == 0 ? "002" : "001";

               // شروع با کل Methods
               var _methods = iScsc.Methods.AsQueryable();

               // اعمال فیلتر MtodStat
               if (MtodStat_Fcb.Checked)
               {
                  _methods = _methods.Where(a => a.MTOD_STAT == _trgtMtodStat);
               }

               // اعمال فیلتر MtodExst با Join
               if (MtodExst_Fcb.Checked)
               {
                  switch(MtodExst_Fcb.SelectedIndex)
                  { 
                     case 0:
                        _methods = 
                           from m in _methods
                           where (from cm in iScsc.Club_Methods
                                  where cm.MTOD_CODE == m.CODE
                                  select cm).Any()
                           select m;
                     break;
                     case 1:
                        _methods =
                           from m in _methods
                           where !(from cm in iScsc.Club_Methods
                                   where cm.MTOD_CODE == m.CODE
                                   select cm).Any()
                           select m;
                     break;
                  }
               }

               int _m = MtodBs.Position;
               MtodBs.DataSource = _methods.ToList();
               MtodBs.Position = _m;

               CntyBs.DataSource = iScsc.Countries;
               break;
            case "xTp_004":
               // مقادیر ثابت
               var _trgtCochStat = CochStat_Fcb.SelectedIndex == 0 ? "101" : "001";

               var _coachs = iScsc.Fighters.Where(a => a.FGPB_TYPE_DNRM == "003");

               if (CochStat_Fcb.Checked)
                  _coachs = _coachs.Where(a => a.ACTV_TAG_DNRM == _trgtCochStat);

               if(CochExst_Fcb.Checked)
               {
                  switch (CochExst_Fcb.SelectedIndex)
                  {
                     case 0:
                        _coachs = 
                           from c in _coachs
                           where (from cm in iScsc.Club_Methods
                                  where cm.COCH_FILE_NO == c.FILE_NO
                                  select cm).Any()
                           select c;
                        break;
                     case 1:
                        _coachs = 
                           from c in _coachs
                           where !(from cm in iScsc.Club_Methods
                                  where cm.COCH_FILE_NO == c.FILE_NO
                                  select cm).Any()
                           select c;
                        break;
                  }
               }

               CochBs.DataSource = _coachs;//iScsc.Fighters.Where(a => a.FGPB_TYPE_DNRM == "003");
               //MtodBs.DataSource = iScsc.Methods.Where(a => a.MTOD_STAT == "002");
               break;
            case "xTp_005":
               // مقادیر ثابت
               var _trgtCbmtStat = CbmtStat_Fcb.SelectedIndex == 0 ? "002" : "001";

               var _clubMethods = iScsc.Club_Methods.AsQueryable();

               if(CbmtStat_Fcb.Checked)
               {
                  _clubMethods = _clubMethods.Where(a => a.MTOD_STAT == _trgtCbmtStat);
               }

               if(CbmtExst_Fcb.Checked)
               {
                  switch (CbmtExst_Fcb.SelectedIndex)
                  {
                     case 0:
                        _clubMethods = 
                           from cm in _clubMethods
                           where (from p in iScsc.Fighter_Publics
                                  where p.RECT_CODE == "004"
                                     && p.CBMT_CODE == cm.CODE
                                  select p).Any()
                           select cm;
                        break;
                     case 1:
                        _clubMethods = 
                           from cm in _clubMethods
                           where !(from p in iScsc.Fighter_Publics
                                  where p.RECT_CODE == "004"
                                     && p.CBMT_CODE == cm.CODE
                                  select p).Any()
                           select cm;
                        break;
                  }
               }

               CbmtBs.DataSource = _clubMethods;
               CntyBs.DataSource = iScsc.Countries;
               MtodBs.DataSource = iScsc.Methods;//.Where(a => a.MTOD_STAT == "002");
               CochBs.DataSource = iScsc.Fighters.Where(a => a.FGPB_TYPE_DNRM == "003" /*&& a.ACTV_TAG_DNRM == "101"*/);
               break;
            case "xTp_006":
               VAudsBs.DataSource = iScsc.V_Acess_User_Datasources.OrderBy(a => a.DATA_BASE);
               break;
         }
         requery = false;
         BuildRolloutMenuForCurrentTab();
         isLoading = false;
      }      

      private async void Master_Tc_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
      {
         await Execute_Query();
      }

      private async void BoolOption_CheckedChanged(object sender, EventArgs e)
      {
         await Execute_Query();
      }
      #endregion

      #region Rollout management
      private Rollout _currentRolloutUnderMouse = null;

      private void panel_MouseUp(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right)
         {
            FlowLayoutPanel panel = sender as FlowLayoutPanel;
            if (panel == null) return;

            // پیدا کردن rollout زیر موس
            _currentRolloutUnderMouse = panel.GetChildAtPoint(e.Location) as Rollout;

            if (_currentRolloutUnderMouse != null)
            {
               // فقط روی rollout منو باز شود
               Rollout_Cms.Show(panel, e.Location);
            }
         }
      }

      private void BuildDefultMenuItemsRolloutCms()
      {
         Rollout_Cms.Items.Clear();

         // Close Rollout
         ToolStripMenuItem closeItem = new ToolStripMenuItem("Close Rollout");
         closeItem.Click += CloseRollout_Click;
         Rollout_Cms.Items.Add(closeItem);

         // Close All
         ToolStripMenuItem closeAllItem = new ToolStripMenuItem("Close All");
         closeAllItem.Click += CloseAll_Click;
         Rollout_Cms.Items.Add(closeAllItem);

         // Open All
         ToolStripMenuItem openAllItem = new ToolStripMenuItem("Open All");
         openAllItem.Click += OpenAll_Click;
         Rollout_Cms.Items.Add(openAllItem);

         // ---
         ToolStripSeparator sepratorItem = new ToolStripSeparator();
         //openAllItem.Click += OpenAll_Click;
         Rollout_Cms.Items.Add(sepratorItem);
      }

      private void CloseRollout_Click(object sender, EventArgs e)
      {
         if (_currentRolloutUnderMouse != null)
         {
            _currentRolloutUnderMouse.RolloutStatus = false;
            // کد Collapse UI
            //_currentRolloutUnderMouse.Height = 30; // مثال برای بستن rollout
         }
      }

      private void CloseAll_Click(object sender, EventArgs e)
      {
         if (_currentRolloutUnderMouse == null) return;

         FlowLayoutPanel panel = _currentRolloutUnderMouse.Parent as FlowLayoutPanel;
         if (panel == null) return;

         foreach (Rollout r in panel.Controls.OfType<Rollout>())
         {
            r.RolloutStatus = false;
            //r.Height = 30; // یا هر کد Collapse خودت
         }
      }

      private void OpenAll_Click(object sender, EventArgs e)
      {
         if (_currentRolloutUnderMouse == null) return;

         FlowLayoutPanel panel = _currentRolloutUnderMouse.Parent as FlowLayoutPanel;
         if (panel == null) return;

         foreach (Rollout r in panel.Controls.OfType<Rollout>())
         {
            r.RolloutStatus = true;
            //r.Height = r.PreferredSize.Height; // مثال Expand
         }
      }

      private void BuildRolloutMenuForCurrentTab()
      {
         XtraTabPage tab = Master_Tc.SelectedTabPage;
         if (tab == null) return;

         FlowLayoutPanel panel = GetFlowPanel(tab);
         if (panel == null) return;

         // پیدا کردن rollout ها
         var rollouts = panel.Controls
                             .OfType<Rollout>()
                             .ToList();

         // ساخت ContextMenu
         //ContextMenuStrip menu = new ContextMenuStrip();
         Rollout_Cms.Items.Clear();

         BuildDefultMenuItemsRolloutCms();

         foreach (var rollout in rollouts)
         {
            ToolStripMenuItem item = new ToolStripMenuItem(rollout.RolloutTitle);
            item.Tag = rollout; // ذخیره rollout در آیتم
            item.Click += (s, e) =>
            {
               OpenAndFocusRollout(panel, rollout);
            };

            Rollout_Cms.Items.Add(item);
         }

         // attach منو به همه rollout ها در panel
         AttachRightClickRecursively(panel);
         // attach منو به TabPage یا FlowPanel
         //tab.ContextMenuStrip = Rollout_Cms;         
      }

      private void AttachRightClickRecursively(Control parent)
      {
         // خود کنترل
         parent.MouseUp -= rollout_MouseUp;
         parent.MouseUp += rollout_MouseUp;

         // همه فرزندان
         foreach (Control c in parent.Controls)
         {
            AttachRightClickRecursively(c); // بازگشتی
         }
      }

      private void rollout_MouseUp(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;

         Control ctrl = sender as Control;

         // بالا رفتن تا پیدا کردن rollout والد (Recursive)
         _currentRolloutUnderMouse = FindParentRollout(ctrl);

         if (_currentRolloutUnderMouse != null)
         {
            // موقعیت راست کلیک نسبت به rollout اصلی
            Point pt = _currentRolloutUnderMouse.PointToClient(Cursor.Position);
            Rollout_Cms.Show(_currentRolloutUnderMouse, pt);
         }
      }

      // تابع کمکی برای پیدا کردن Rollout والد واقعی
      private Rollout FindParentRollout(Control ctrl)
      {
         while (ctrl != null)
         {
            if (ctrl is Rollout)
               return ctrl as Rollout;

            ctrl = ctrl.Parent;
         }
         return null;
      }


      private FlowLayoutPanel GetFlowPanel(XtraTabPage tab)
      {
         return tab.Controls
                   .OfType<FlowLayoutPanel>()
                   .FirstOrDefault();
      }

      private void OpenAndFocusRollout(FlowLayoutPanel panel, Rollout rollout)
      {
         // rollout باز شود
         rollout.RolloutStatus = true;

         // rollout فوکوس شود
         rollout.Focus();

         // اسکرول بیاد روی rollout
         panel.ScrollControlIntoView(rollout);
      }

      private async void RolloutStatusChanged(object sender, EventArgs e)
      {
         try
         {
            var _rlt = (Rollout)sender;
            if (_rlt == null) return;

            switch (_rlt.RolloutStatus)
            {
               case true:
                  await Execute_Query();
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion

      #region Expense Item tab page
      #region Expense Item Rollout
      private void AddEpit_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (EpitBs.OfType<Data.Expense_Item>().Any(a => a.CODE == 0)) return;

            var _epit = EpitBs.AddNew() as Data.Expense_Item;
            // Income
            if(Ittp001_Rb.Checked)
            {               
               _epit.TYPE = "001";
            }
            else
            {
               _epit.TYPE = "002";
            }

            iScsc.Expense_Items.InsertOnSubmit(_epit);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveEpit_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Epit_Gv.PostEditor();

            iScsc.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         if (requery)
            await Execute_Query();
      }

      private async void DelEpit_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _epit = EpitBs.Current as Data.Expense_Item;
            if (_epit == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Expense_Items.DeleteOnSubmit(_epit);
            iScsc.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         if (requery)
            await Execute_Query();
      }
      #endregion
      #endregion

      #region Region and Club tab page
      #region Region Rollout
      private void AddCnty_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);            
         }
      }

      private async void SaveCnty_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Cnty_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DelCnty_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void AddPrvn_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SavePrvn_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Prvn_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DelPrvn_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void AddRegn_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveRegn_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Regn_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DelRegn_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }
      #endregion

      #region Club Rollout
      private void ClubBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _club = ClubBs.Current as Data.Club;
            if (_club == null) return;

            VUcfgBs.DataSource = iScsc.V_UCFGAs.Where(a => a.CLUB_CODE == _club.CODE);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddClub_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClubBs.List.OfType<Data.Club>().Any(a => a.CODE == 0)) return;

            var _regn = RegnBs.Current as Data.Region;
            if (_regn == null) return;

            var _club = ClubBs.AddNew() as Data.Club;

            iScsc.Clubs.InsertOnSubmit(_club);            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveClub_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Club_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DelClub_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _club = ClubBs.Current as Data.Club;
            if (_club == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Clubs.DeleteOnSubmit(_club);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }
      #endregion

      #region User Club Rollout
      private async void UserActn_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _club = ClubBs.Current as Data.Club;
            if (_club == null) return;

            var _user = vUserBs.Current as Data.V_User;
            if (_user == null) return;

            if (VUcfgBs.List.OfType<Data.V_UCFGA>().Any(a => a.SYS_USER == _user.USER_DB && a.CLUB_CODE == _club.CODE)) return;

            switch (e.Button.Index)
            {
               case 0:
                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "003"),
                        new XElement("FgaUClub",
                           new XAttribute("sysuser", _user.USER_DB),                           
                           new XAttribute("clubcode", _club.CODE)
                        )
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

         if (requery)
            await Execute_Query();
      }

      private async void UcfgActn_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _ucfg = VUcfgBs.Current as Data.V_UCFGA;
            if (_ucfg == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "004"),
                        new XElement("FgaUClub",
                           new XAttribute("sysuser", _ucfg.SYS_USER),
                           new XAttribute("clubcode", _ucfg.CLUB_CODE)
                        )
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

         if (requery)
            await Execute_Query();
      }
      #endregion
      #endregion

      #region Method and Category tab page
      #region Method and Category Rollout
      #region Method
      private void MtodBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;            

            switch (Master_Tc.SelectedTabPage.Name)
            {
               case "xTp_003":
                  CtgyBs.DataSource = _mtod.Category_Belts;

                  if (MtodCochCbmt_Rlt.RolloutStatus)
                  {
                     // 1405/12/10 * دقیقا دو روز پیش خامنه ای ضحاک خوارش گوییده شد و ایران داره پوست اندازی میکنه 
                     // این این دستوری که اینجا نوشته شده باعث میشه که رکوردها پاک بشن و این اشتباه هست 
                     // ببینم با نال کردن مقدار این متغییر کار درست میشه یا نه 
                     // کیر تو کص ننه مادر جنده ای حروم زاده ای که اینترنت رو قطع کرده و ما نمی تونیم از سرچ کنیم ببینیم مشکل رو چطوری حل کنیم
                     // CbmtBs.Clear();
                     CbmtBs.SuspendBinding();
                     CbmtBs.DataSource = null;
                     CbmtBs.ResumeBinding();

                     // مقادیر ثابت
                     var _trgtMtodCochStat = MtodCochStat_Fcb.SelectedIndex == 0 ? "101" : "001";

                     var _coachs =
                        iScsc.Fighters
                        .Where(a =>
                           a.FGPB_TYPE_DNRM == "003").AsQueryable();

                     if (MtodCochStat_Fcb.Checked)
                     {
                        _coachs = _coachs.Where(a => a.ACTV_TAG_DNRM == _trgtMtodCochStat);
                     }

                     if (MtodCochExst_Fcb.Checked)
                     {
                        switch (MtodCbmtExst_Fcb.SelectedIndex)
                        {
                           case 0:
                              _coachs =
                                 from c in _coachs
                                 where (from cm in iScsc.Club_Methods
                                        where cm.COCH_FILE_NO == c.FILE_NO
                                           && cm.MTOD_CODE == _mtod.CODE
                                        select cm).Any()
                                 select c;
                              break;
                           case 1:
                              _coachs =
                                 from c in _coachs
                                 where !(from cm in iScsc.Club_Methods
                                         where cm.COCH_FILE_NO == c.FILE_NO
                                            && cm.MTOD_CODE == _mtod.CODE
                                         select cm).Any()
                                 select c;
                              break;
                        }
                     }

                     int _c = CochBs.Position;
                     CochBs.DataSource = _coachs;
                     CochBs.Position = _c;
                  }
                  break;
               case "xTp_004":
                  if (CochMtodCbmt_Rlt.RolloutStatus)
                  {
                     var _coch = CochBs.Current as Data.Fighter;
                     if (_coch == null) return;

                     // مقادیر ثابت
                     var _trgtMtodCbmtStat = CochCbmtStat_Fcb.SelectedIndex == 0 ? "002" : "001";

                     var _clubMethods = iScsc.Club_Methods.Where(a => a.MTOD_CODE == _mtod.CODE && a.COCH_FILE_NO == _coch.FILE_NO);

                     if (CochCbmtStat_Fcb.Checked)
                     {
                        _clubMethods = _clubMethods.Where(a => a.MTOD_STAT == _trgtMtodCbmtStat);
                     }

                     if (CochCbmtExst_Fcb.Checked)
                     {
                        switch (CochCbmtExst_Fcb.SelectedIndex)
                        {
                           case 0:
                              _clubMethods =
                                 from cm in _clubMethods
                                 where (from p in iScsc.Fighter_Publics
                                        where p.RECT_CODE == "004"
                                           && p.CBMT_CODE == cm.CODE
                                        select p).Any()
                                 select cm;
                              break;
                           case 1:
                              _clubMethods =
                                 from cm in _clubMethods
                                 where !(from p in iScsc.Fighter_Publics
                                         where p.RECT_CODE == "004"
                                            && p.CBMT_CODE == cm.CODE
                                         select p).Any()
                                 select cm;
                              break;
                        }
                     }

                     int _cm = CbmtBs.Position;
                     CbmtBs.DataSource = _clubMethods;
                     CbmtBs.Position = _cm;
                  }
                  break;
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddMtod_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MtodBs.List.OfType<Data.Method>().Any(a => a.CODE == 0)) return;

            var _mtod = MtodBs.AddNew() as Data.Method;
            iScsc.Methods.InsertOnSubmit(_mtod);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void DelMtod_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Methods.DeleteOnSubmit(_mtod);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void SaveMtod_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Mtod_Tl.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await UpdateExpense();
      }

      private TreeListNode draggedNode = null;
      private void Mtod_Tl_DragOver(object sender, DragEventArgs e)
      {
         Point pt = Mtod_Tl.PointToClient(new Point(e.X, e.Y));

         // پیدا کردن نود زیر ماوس
         TreeListHitInfo hit = Mtod_Tl.CalcHitInfo(pt);

         if (hit.HitInfoType == HitInfoType.Cell || hit.HitInfoType == HitInfoType.RowIndicator)
         {
            e.Effect = DragDropEffects.Move;
         }
         else
         {
            e.Effect = DragDropEffects.None;
         }
      }

      private async void Mtod_Tl_DragDrop(object sender, DragEventArgs e)
      {
         try
         {
            Point pt = Mtod_Tl.PointToClient(new Point(e.X, e.Y));
            TreeListHitInfo hit = Mtod_Tl.CalcHitInfo(pt);

            if (hit.Node == null || draggedNode == null) return;
            if (hit.Node == draggedNode) return;

            // جلوگیری از قرار دادن Parent در Child خودش
            TreeListNode parent = hit.Node.ParentNode;
            while (parent != null)
            {
               if (parent == draggedNode)
               {
                  e.Effect = DragDropEffects.None;
                  return;
               }
               parent = parent.ParentNode;
            }

            var draggedNodeData = Mtod_Tl.GetDataRecordByNode(draggedNode) as Data.Method;
            var hitNodeData = Mtod_Tl.GetDataRecordByNode(hit.Node) as Data.Method;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE dbo.Method SET MTOD_CODE = {0} WHERE CODE = {1};",
                  hitNodeData.CODE,
                  draggedNodeData.CODE
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }      

      private void Mtod_Tl_BeforeDragNode(object sender, BeforeDragNodeEventArgs e)
      {
         draggedNode = e.Node;

         // می‌توانیم شرطی برای درگ گذاری بگذاریم
         // if (e.Node.Level == 0) e.Cancel = true; // ریشه‌ها درگ نشوند
      }

      private async void UpMtod_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE dbo.Method SET MTOD_CODE = NULL WHERE CODE = {0};",
                  _mtod.CODE
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void UpdtExpn_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            await UpdateExpense();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async Task UpdateExpense()
      {
         try
         {
            var _epit = ExpnEpit_Lov.EditValue;
            if (_epit == null || _epit.ToString() == "") return;

            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;

            var _ctgy = CtgyBs.Current as Data.Category_Belt;
            if (_ctgy == null) { if (_mtod.CODE == 0) requery = true; throw new Exception("Reload data"); }

            //if (MessageBox.Show(this, "آیا با بروزرسانی اطلاعات آیتم های درآمد موافق هستید؟", "بروزرسانی رکوردهای درامدی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Expenses
            .Where(a =>
               a.Expense_Type.EPIT_CODE == (long)_epit &&
               (!Mtod_Cbx.Checked || a.MTOD_CODE == _mtod.CODE) &&
               (!Ctgy_Cbx.Checked || a.CTGY_CODE == _ctgy.CODE) &&
               (!CyclRqst_Cbx.Checked || (a.Expense_Type.Request_Requester.RQTP_CODE == "001" || a.Expense_Type.Request_Requester.RQTP_CODE == "009")) &&
               (!IncmRqst_Cbx.Checked || (a.Expense_Type.Request_Requester.RQTP_CODE == "016")) &&
               (a.Expense_Type.Request_Requester.RQTT_CODE == "001")
            )
            .ToList()
            .ForEach(a =>
            {
               var _getCtgy = iScsc.Category_Belts.FirstOrDefault(b => b.CODE == a.CTGY_CODE);
               if (ExpnPric_Cbx.Checked)
                  a.PRIC = _getCtgy.PRIC ?? 0;
               if (ExpnStat_Cbx.Checked)
                  a.EXPN_STAT = _getCtgy.CTGY_STAT;
               if (ExpnTNum_Cbx.Checked)
                  a.NUMB_OF_ATTN_MONT = _getCtgy.NUMB_OF_ATTN_MONT;
               if (ExpnCD_Cbx.Checked)
                  a.NUMB_CYCL_DAY = _getCtgy.NUMB_CYCL_DAY;
            });

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }
      #endregion

      #region Category Belt
      private void AddCtgy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;

            if (CtgyBs.List.OfType<Data.Category_Belt>().Any(a => a.CODE == 0)) return;

            var _ctgy = CtgyBs.AddNew() as Data.Category_Belt;
            _ctgy.MTOD_CODE = _mtod.CODE;
            iScsc.Category_Belts.InsertOnSubmit(_ctgy);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void DelCtgy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _ctgy = CtgyBs.Current as Data.Category_Belt;
            if (_ctgy == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Category_Belts.DeleteOnSubmit(_ctgy);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void SaveCtgy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Ctgy_Tl.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await UpdateExpense();
      }

      private void Ctgy_Tl_DragOver(object sender, DragEventArgs e)
      {
         Point pt = Ctgy_Tl.PointToClient(new Point(e.X, e.Y));

         // پیدا کردن نود زیر ماوس
         TreeListHitInfo hit = Ctgy_Tl.CalcHitInfo(pt);

         if (hit.HitInfoType == HitInfoType.Cell || hit.HitInfoType == HitInfoType.RowIndicator)
         {
            e.Effect = DragDropEffects.Move;
         }
         else
         {
            e.Effect = DragDropEffects.None;
         }
      }

      private async void Ctgy_Tl_DragDrop(object sender, DragEventArgs e)
      {
         try
         {
            Point pt = Ctgy_Tl.PointToClient(new Point(e.X, e.Y));
            TreeListHitInfo hit = Ctgy_Tl.CalcHitInfo(pt);

            if (hit.Node == null || draggedNode == null) return;
            if (hit.Node == draggedNode) return;

            // جلوگیری از قرار دادن Parent در Child خودش
            TreeListNode parent = hit.Node.ParentNode;
            while (parent != null)
            {
               if (parent == draggedNode)
               {
                  e.Effect = DragDropEffects.None;
                  return;
               }
               parent = parent.ParentNode;
            }

            var draggedNodeData = Ctgy_Tl.GetDataRecordByNode(draggedNode) as Data.Category_Belt;
            var hitNodeData = Ctgy_Tl.GetDataRecordByNode(hit.Node) as Data.Category_Belt;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE dbo.Category_Belt SET CTGY_CODE = {0} WHERE CODE = {1};",
                  hitNodeData.CODE,
                  draggedNodeData.CODE
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void Ctgy_Tl_BeforeDragNode(object sender, BeforeDragNodeEventArgs e)
      {
         draggedNode = e.Node;

         // می‌توانیم شرطی برای درگ گذاری بگذاریم
         // if (e.Node.Level == 0) e.Cancel = true; // ریشه‌ها درگ نشوند
      }

      private async void UpCtgy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _ctgy = CtgyBs.Current as Data.Category_Belt;
            if (_ctgy == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE dbo.Category_Belt SET CTGY_CODE = NULL WHERE CODE = {0};",
                  _ctgy.CODE
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }
      #endregion      
      #endregion

      #region Method, User and Computer Link Rollout
      private async void UserActn1_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _user = vUserBs.Current as Data.V_User;
            if (_user == null) return;

            var _comp = vCompBs.Current as Data.Computer_Action;
            if (_comp == null) return;

            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;

            var _ulkm = UlkmBs.AddNew() as Data.User_Link_Method;
            _ulkm.COMA_CODE = _comp.CODE;
            _ulkm.MTOD_CODE = _mtod.CODE;
            _ulkm.USER_ID = _user.ID;
            _ulkm.STAT = "002";

            iScsc.User_Link_Methods.InsertOnSubmit(_ulkm);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void CompActn1_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         UserActn1_Btn_ButtonClick(null, null);
      }

      private void MtodActn1_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         UserActn1_Btn_ButtonClick(null, null);
      }

      private async void UlkmActn1_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _ulkm = UlkmBs.Current as Data.User_Link_Method;
            if (_ulkm == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  // Delete
                  iScsc.User_Link_Methods.DeleteOnSubmit(_ulkm);
                  break;
               case 1:
                  // Active & Deactive
                  _ulkm.STAT = (_ulkm.STAT == "002" ? "001" : "002");
                  break;
            }

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }
      #endregion

      #region Coach and Club Method
      private void AddMtodCoch_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Master_Tc.SelectedTabPage = xTp_004;
            BeginInvoke(new Action(() => AddCoch_Btn_Click(null, null)));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddMtodCbmt_Btn_Click(object sender, EventArgs e)
      {
         Master_Tc.SelectedTabPage = xTp_005;
         BeginInvoke(new Action(() => AddCbmt_Btn_Click(null, null)));
      }
      private void SaveMtodCbmt_Btn_Click(object sender, EventArgs e)
      {
         SaveCbmt_Btn_Click(null, null);
      }
      #endregion
      #endregion

      #region Coach tab page
      #region Coach Rollout
      private void Coch_Tl_DragOver(object sender, DragEventArgs e)
      {
         Point pt = Coch_Tl.PointToClient(new Point(e.X, e.Y));

         // پیدا کردن نود زیر ماوس
         TreeListHitInfo hit = Coch_Tl.CalcHitInfo(pt);

         if (hit.HitInfoType == HitInfoType.Cell || hit.HitInfoType == HitInfoType.RowIndicator)
         {
            e.Effect = DragDropEffects.Move;
         }
         else
         {
            e.Effect = DragDropEffects.None;
         }
      }

      private async void Coch_Tl_DragDrop(object sender, DragEventArgs e)
      {
         try
         {
            Point pt = Coch_Tl.PointToClient(new Point(e.X, e.Y));
            TreeListHitInfo hit = Coch_Tl.CalcHitInfo(pt);

            if (hit.Node == null || draggedNode == null) return;
            if (hit.Node == draggedNode) return;

            // جلوگیری از قرار دادن Parent در Child خودش
            TreeListNode parent = hit.Node.ParentNode;
            while (parent != null)
            {
               if (parent == draggedNode)
               {
                  e.Effect = DragDropEffects.None;
                  return;
               }
               parent = parent.ParentNode;
            }

            var draggedNodeData = Coch_Tl.GetDataRecordByNode(draggedNode) as Data.Fighter;
            var hitNodeData = Coch_Tl.GetDataRecordByNode(hit.Node) as Data.Fighter;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE fp SET fp.REF_CODE = {0} FROM dbo.Fighter_Public fp INNER JOIN dbo.Fighter f ON (f.FILE_NO = fp.FIGH_FILE_NO) WHERE fp.RECT_CODE = '004' AND f.FGPB_RWNO_DNRM = fp.RWNO AND f.FILE_NO = {1};",
                  hitNodeData.FILE_NO,
                  draggedNodeData.FILE_NO
               )
            );
            
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void Coch_Tl_BeforeDragNode(object sender, BeforeDragNodeEventArgs e)
      {
         draggedNode = e.Node;

         // می‌توانیم شرطی برای درگ گذاری بگذاریم
         // if (e.Node.Level == 0) e.Cancel = true; // ریشه‌ها درگ نشوند
      }

      private async void CochTopLevl_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _coch = CochBs.Current as Data.Fighter;
            if (_coch == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE fp SET fp.REF_CODE = NULL FROM dbo.Fighter_Public fp INNER JOIN dbo.Fighter f ON (f.FILE_NO = fp.FIGH_FILE_NO) WHERE fp.RECT_CODE = '004' AND f.FGPB_RWNO_DNRM = fp.RWNO AND f.FILE_NO = {0};",
                  _coch.FILE_NO
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void AddCoch_Btn_Click(object sender, EventArgs e)
      {
         AddCoch_Rlt.RolloutStatus = true;
         ClerCoch_Btn_Click(null, null);
      }

      private async void ActvCoch_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _coch = CochBs.Current as Data.Fighter;
            if (_coch == null) return;

            if (_coch.ACTV_TAG_DNRM == "101") return;

            if (MessageBox.Show(this, "آیا با بازیابی پرسنل موافق هستید؟", "عملیات بازیابی پرسنل", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            await Task.Run(() =>
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_dsen_f"},
                           new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 02 /* Execute Set */),
                           new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 07 /* Execute Load_Data */),                        
                           new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", _coch.FILE_NO), new XAttribute("auto", "true"), new XAttribute("fngrprnt", ""))},
                        })
                     );
               }
            );
            

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DeacCoch_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _coch = CochBs.Current as Data.Fighter;
            if (_coch == null) return;

            if (_coch.ACTV_TAG_DNRM == "001") return;

            if (MessageBox.Show(this, "آیا با حذف پرسنل موافق هستید؟", "عملیات حذف موقت پرسنل", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            await Task.Run(() => 
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                           new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", _coch.FILE_NO), new XAttribute("auto", "true"))},
                        })
                  );
               }
            );            

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void CochBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _coch = CochBs.Current as Data.Fighter;
            if (_coch == null) return;

            switch (Master_Tc.SelectedTabPage.Name)
            {
               case "xTp_003":
                  if (MtodCochCbmt_Rlt.RolloutStatus)
                  {
                     var _mtod = MtodBs.Current as Data.Method;
                     if (_mtod == null) return;

                     // مقادیر ثابت
                     var _trgtMtodCbmtStat = MtodCbmtStat_Fcb.SelectedIndex == 0 ? "002" : "001";

                     var _clubMethods = iScsc.Club_Methods.Where(a => a.MTOD_CODE == _mtod.CODE && a.COCH_FILE_NO == _coch.FILE_NO);

                     if (MtodCbmtStat_Fcb.Checked)
                     {
                        _clubMethods = _clubMethods.Where(a => a.MTOD_STAT == _trgtMtodCbmtStat);
                     }

                     if (MtodCbmtExst_Fcb.Checked)
                     {
                        switch (MtodCbmtExst_Fcb.SelectedIndex)
                        {
                           case 0:
                              _clubMethods =
                                 from cm in _clubMethods
                                 where (from p in iScsc.Fighter_Publics
                                        where p.RECT_CODE == "004"
                                           && p.CBMT_CODE == cm.CODE
                                        select p).Any()
                                 select cm;
                              break;
                           case 1:
                              _clubMethods =
                                 from cm in _clubMethods
                                 where !(from p in iScsc.Fighter_Publics
                                         where p.RECT_CODE == "004"
                                            && p.CBMT_CODE == cm.CODE
                                         select p).Any()
                                 select cm;
                              break;
                        }
                     }

                     int _cm = CbmtBs.Position;
                     CbmtBs.DataSource = _clubMethods;
                     CbmtBs.Position = _cm;
                  }
                  break;
               case "xTp_004":                  
                  // Load coach profile image
                  await LoadImageAsync(_coch.FILE_NO, CochProFile1_Rb);
                  if(CochMtodCbmt_Rlt.RolloutStatus)
                  {
                     CbmtBs.Clear();

                     // مقادیر ثابت
                     var _trgtMtodStat = CochMtodStat_Fcb.SelectedIndex == 0 ? "002" : "001";

                     // شروع با کل Methods
                     var _methods = iScsc.Methods.AsQueryable();

                     // اعمال فیلتر MtodStat
                     if (CochMtodStat_Fcb.Checked)
                     {
                        _methods = _methods.Where(a => a.MTOD_STAT == _trgtMtodStat);
                     }

                     // اعمال فیلتر MtodExst با Join
                     if (CochMtodExst_Fcb.Checked)
                     {
                        switch (CochMtodExst_Fcb.SelectedIndex)
                        {
                           case 0:
                              _methods =
                                 from m in _methods
                                 where (from cm in iScsc.Club_Methods
                                        where cm.MTOD_CODE == m.CODE
                                        select cm).Any()
                                 select m;
                              break;
                           case 1:
                              _methods =
                                 from m in _methods
                                 where !(from cm in iScsc.Club_Methods
                                         where cm.MTOD_CODE == m.CODE
                                         select cm).Any()
                                 select m;
                              break;
                        }
                     }

                     int _m = MtodBs.Position;
                     MtodBs.DataSource = _methods.ToList();
                     MtodBs.Position = _m;
                  }
                  
                  if(Cexc_Rlt.RolloutStatus)
                  {
                     int _c = CexcBs.Position;
                     CexcBs.DataSource =
                        iScsc.Calculate_Expense_Coaches.Where(a => a.COCH_FILE_NO == _coch.FILE_NO);
                     CexcBs.Position = _c;
                  }
                  break;
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async Task LoadImageAsync(long fileno, RoundedButton profile)
      {
         try
         {
            await Task.Yield();
            profile.ImageVisiable = true;
            profile.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            if (InvokeRequired)
               Invoke(new Action(() => profile.ImageProfile = bm));
            else
               profile.ImageProfile = bm;
         }
         catch
         { 
            profile.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
         }
      }
      #endregion

      #region New Coach Rollout
      private async void SaveCoch_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (CochFrstName_Txt.Text == "") { CochFrstName_Txt.Focus(); return; }
            if (CochLastName_Txt.Text == "") { CochLastName_Txt.Focus(); return; }
            if (CochNatlCode_Txt.Text == "" && !ModifierKeys.HasFlag(Keys.Control)) { CochNatlCode_Txt.Focus(); return; }
            if (CochCellPhon_Txt.Text == "" && !ModifierKeys.HasFlag(Keys.Control)) { CochCellPhon_Txt.Focus(); return; }
            long mtod = 0;
            if (CochMtod_Lov.EditValue == null || !long.TryParse(CochMtod_Lov.EditValue.ToString(), out mtod)) { CochMtod_Lov.Focus(); return; }
            int sex = 0;
            if (CochSex_Lov.EditValue == null || !int.TryParse(CochSex_Lov.EditValue.ToString(), out sex)) { CochSex_Lov.Focus(); return; }

            // 1404/08/10 * اگر مربی تکراری ثبت داره میکنه
            var _existsCoch = CochBs.List.OfType<Data.Fighter>().Any(a => a.FRST_NAME_DNRM.Contains(CochFrstName_Txt.Text) && a.LAST_NAME_DNRM.Contains(CochLastName_Txt.Text));
            if (_existsCoch && MessageBox.Show(this, "با این مشخصات قبلا این پرسنل ثبت شده، آیا نیاز به بررسی میبینید؟", "خطای ثبت تکراری", MessageBoxButtons.YesNo) == DialogResult.Yes) return;

            await Task.Run(() => 
               {
                  iScsc.ADM_MSAV_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", 0),
                           new XAttribute("rqtpcode", "001"),
                           new XAttribute("rqttcode", "003"),
                           new XElement("Fighter",
                              new XAttribute("fileno", 0),
                              new XElement("Frst_Name", CochFrstName_Txt.Text),
                              new XElement("Last_Name", CochLastName_Txt.Text),
                              new XElement("Coch_Crtf_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                              new XElement("Sex_Type", CochSex_Lov.EditValue),
                              new XElement("Natl_Code", CochNatlCode_Txt.Text),
                              new XElement("No_Chek_Natl_Code", (ModifierKeys.HasFlag(Keys.Control) ? "002" : "001")),
                              new XElement("Brth_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                              new XElement("Cell_Phon", CochCellPhon_Txt.Text),
                              new XElement("No_Chek_Cell_Phon", (ModifierKeys.HasFlag(Keys.Control) ? "002" : "001")),
                              new XElement("Type", "003"),
                              new XElement("Insr_Date", DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")),
                              new XElement("Mtod_Code", CochMtod_Lov.EditValue),
                              new XElement("Fngr_Prnt", CochFngrPrnt_Txt.Text)
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

         if (requery)
         {
            ClerCoch_Btn_Click(null, null);
            await Execute_Query();
         }
      }

      private void ClerCoch_Btn_Click(object sender, EventArgs e)
      {
         CochFrstName_Txt.EditValue = CochLastName_Txt.EditValue = CochNatlCode_Txt.EditValue = CochCellPhon_Txt.EditValue = CochSex_Lov.EditValue = CochMtod_Lov.EditValue = CochFngrPrnt_Txt.EditValue = null;
         CochFrstName_Txt.Focus();
      }

      private void SaveNewCoch_Btn_Click(object sender, EventArgs e)
      {
         SaveCoch_Btn_Click(null, null);
         ClerCoch_Btn_Click(null, null);
      }

      private void CochFngrPrnt_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _fngrPrnt = CochFngrPrnt_Txt.Text;

            switch (e.Button.Index)
            {
               case 0:
                  try
                  {
                     CochFngrPrnt_Txt.EditValue =
                         iScsc.Fighters
                         .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                         .Select(f => f.FNGR_PRNT_DNRM)
                         .ToList()
                         .Where(f => f.All(char.IsDigit))
                         .Max(f => Convert.ToInt64(f)) + 1;
                  }
                  catch
                  {
                     CochFngrPrnt_Txt.Text = "1";
                  }
                  break;
               case 1:
                  if (string.IsNullOrEmpty(_fngrPrnt.Trim())) { CochFngrPrnt_Txt.Focus(); return; }
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", _fngrPrnt))},
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", _fngrPrnt))}
                        })
                  );
                  break;
               case 2:
                  if (string.IsNullOrEmpty(_fngrPrnt.Trim())) { CochFngrPrnt_Txt.Focus(); return; }
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", _fngrPrnt))}
                        })
                  );
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion

      #region Method and Club Method Rollout
      private void AddCochMtod_Btn_Click(object sender, EventArgs e)
      {
         Master_Tc.SelectedTabPage = xTp_003;
         BeginInvoke(new Action(() => AddMtod_Btn_Click(null, null)));
      }
      #endregion

      #region Parameter Coach Calculate Expense
      private async void SaveCochCexp_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Cexc_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void ActvCochCexp_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cexp = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexp == null) return;

            if (_cexp.STAT == "002") return;

            _cexp.STAT = "002";

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DactCochCexp_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cexp = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexp == null) return;

            if (_cexp.STAT == "001") return;

            _cexp.STAT = "001";

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void CopyCochCexp_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            CexcBs.EndEdit();
            Cexc_Gv.PostEditor();

            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            if (ModifierKeys == Keys.Control)
            {
               // برای سرگروه تمام پرسنل
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{3}', Calc_Type = '{4}', Prct_Valu = {5}, Stat = '{6}', Pymt_Stat = '{7}', Rduc_Amnt = {8}, Efct_Date_Type = '{9}', Fore_Givn_Attn_Numb = {10} " +
                     "WHERE Mtod_Code = {0} AND Rqtp_Code = {1} AND Extp_Code = {2};",
                     _cexc.MTOD_CODE,
                     _cexc.RQTP_CODE,
                     _cexc.EXTP_CODE,
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            else if (ModifierKeys == Keys.Shift)
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{1}', Calc_Type = '{2}', Prct_Valu = {3}, Stat = '{4}', Pymt_Stat = '{5}', Rduc_Amnt = {6}, Efct_Date_Type = '{7}', Fore_Givn_Attn_Numb = {8} " +
                     "WHERE Coch_File_No = {0};",
                     _cexc.COCH_FILE_NO,
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            else if (ModifierKeys == (Keys.Control | Keys.Shift))
            {
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{0}', Calc_Type = '{1}', Prct_Valu = {2}, Stat = '{3}', Pymt_Stat = '{4}', Rduc_Amnt = {5}, Efct_Date_Type = '{6}', Fore_Givn_Attn_Numb = {7};",
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            else
            {
               // برای تمام آیتم های سرگروه خوده پرسنل
               iScsc.ExecuteCommand(
                  string.Format(
                     "UPDATE dbo.Calculate_Expense_Coach " +
                     "SET Calc_Expn_Type = '{4}', Calc_Type = '{5}', Prct_Valu = {6}, Stat = '{7}', Pymt_Stat = '{8}', Rduc_Amnt = {9}, Efct_Date_Type = '{10}', Fore_Givn_Attn_Numb = {11} " +
                     "WHERE Coch_File_No = {0} AND Mtod_Code = {1} AND Rqtp_Code = {2} AND Extp_Code = {3};",
                     _cexc.COCH_FILE_NO,
                     _cexc.MTOD_CODE,
                     _cexc.RQTP_CODE,
                     _cexc.EXTP_CODE,
                     _cexc.CALC_EXPN_TYPE,
                     _cexc.CALC_TYPE,
                     _cexc.PRCT_VALU,
                     _cexc.STAT,
                     _cexc.PYMT_STAT,
                     _cexc.RDUC_AMNT ?? 0,
                     _cexc.EFCT_DATE_TYPE,
                     _cexc.FORE_GIVN_ATTN_NUMB ?? 0
                  )
               );
            }
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void SyncCochCexp_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            iScsc.DUP_CEXC_P(
               new XElement("OpIran",
                   new XAttribute("cexccode", _cexc.CODE)
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }
      #endregion
      #endregion

      #region Club Method tab page
      #region Club Method rollout
      private async void CbmtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = CbmtBs.Current as Data.Club_Method;
            if (_cbmt == null || _cbmt.CODE == 0) return;

            CbwkBs.DataSource = _cbmt.Club_Method_Weekdays;

            if(Holiday_Rlt.RolloutStatus)
            {
               HldyBs.DataSource = iScsc.Holidays.Where(a => a.HLDY_DATE.Value.Date >= DateTime.Now.Date);
            }

            switch (Master_Tc.SelectedTabPage.Name)
            {
               case "xTp_005":
                  // Load coach profile image
                  await LoadImageAsync((long)_cbmt.COCH_FILE_NO, CochProFile2_Rb);
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

      private void AddCbmt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (CbmtBs.List.OfType<Data.Club_Method>().Any(a => a.CODE == 0)) return;

            var _club = ClubBs.Current as Data.Club;
            if (_club == null) return;

            var _mtod = MtodBs.Current as Data.Method;
            if (_mtod == null) return;

            var _coch = CochBs.Current as Data.Fighter;
            if (_coch == null) return;

            var _cbmt = CbmtBs.AddNew() as Data.Club_Method;
            _cbmt.CLUB_CODE = _club.CODE;
            _cbmt.MTOD_CODE = _mtod.CODE;
            _cbmt.COCH_FILE_NO = _coch.FILE_NO;
            _cbmt.MTOD_STAT = "002";
            _cbmt.CPCT_STAT = _cbmt.CBMT_TIME_STAT = "001";
            _cbmt.CLAS_TIME = 90;
            _cbmt.CPCT_NUMB = 0;

            iScsc.Club_Methods.InsertOnSubmit(_cbmt);

            SexCbmt_Lov.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveCbmt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Cbmt003_Gv.PostEditor();
            Cbmt004_Gv.PostEditor();
            Cbmt005_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DelCbmt_Btn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void CbwkBs_DataSourceChanged(object sender, EventArgs e)
      {
         try
         {
            switch (Master_Tc.SelectedTabPage.Name)
            {
               case "xTp_005":
                  CbwkBs.List.OfType<Data.Club_Method_Weekday>().ToList()
                     .ForEach(a =>
                        {
                           xTp_005001.Controls.OfType<System.MaxUi.CheckButton>()
                              .Where(b => b.Tag.ToString() == a.WEEK_DAY).ToList()
                              .ForEach(b => b.Checked = a.STAT == "002" ? true : false );
                        }
                     );
                  break;
               case "xTp_003":
                  CbwkBs.List.OfType<Data.Club_Method_Weekday>().ToList()
                     .ForEach(a =>
                        {
                           xTp_003002.Controls.OfType<System.MaxUi.CheckButton>()
                              .Where(b => b.Tag.ToString() == a.WEEK_DAY).ToList()
                              .ForEach(b => b.Checked = a.STAT == "002" ? true : false);
                        }
                     );
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void WeekDay_Cbt_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            if (isLoading) return;

            var _weekday = (System.MaxUi.CheckButton)sender;
            if (_weekday == null) return;

            var _cbwk = CbwkBs.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(a => a.WEEK_DAY == _weekday.Tag.ToString());

            _cbwk.STAT = _weekday.Checked ? "002" : "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion

      #region Holdiday Rollout
      private void AddHldy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (HldyBs.List.OfType<Data.Holiday>().Any(a => a.CODE == 0)) return;

            var _hldy = HldyBs.AddNew() as Data.Holiday;
            _hldy.HLDY_DATE = DateTime.Now;

            iScsc.Holidays.InsertOnSubmit(_hldy);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveHldy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Hldy_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void DelHldy_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _hldy = HldyBs.Current as Data.Holiday;
            if (_hldy == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید?", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Holidays.DeleteOnSubmit(_hldy);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if(requery)
            await Execute_Query();
      }
      #endregion
      #endregion

      #region Settings tab page
      #region Settings rollout
      private async void RunArch_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!ArchDatabase_Cbx.Checked) return;

            if (ArchDbName_Txt.Text.Trim() == "") { ArchDbName_Txt.Focus(); return; }
            if (VAudsBs.List.OfType<Data.V_Acess_User_Datasource>().Any(a => a.DATA_BASE == ArchDbName_Txt.Text)) { ArchDbName_Txt.Focus(); ArchDbName_Txt.SelectAll(); return; }

            if (ArchUser_Txt.Text.Trim() == "") { ArchUser_Txt.Focus(); return; }
            if (ArchUserDesc_Txt.Text.Trim() == "") { ArchUserDesc_Txt.Focus(); return; }
            if (ArchUserPass_Txt.Text.Trim() == "") { ArchUserPass_Txt.Focus(); return; }
            if (ArchUserPass_Txt.Text != ArchUserConfPass_Txt.Text) { ArchUserConfPass_Txt.Focus(); ArchUserConfPass_Txt.SelectAll(); return; }

            iScsc.CLON_DATA_P(
               new XElement("Clonedb",
                   new XAttribute("sorcdb", iScsc.Connection.Database),
                   new XAttribute("trgtdb", ArchDbName_Txt.Text),
                   new XAttribute("username", ArchUser_Txt.Text),
                   new XAttribute("userdesc", ArchUserDesc_Txt.Text),
                   new XAttribute("pswd", ArchUserPass_Txt.Text)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void ArchDbYearName_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            if (!ArchDbYearName_Cbx.Checked) return;

            var year = DateTime.Now.Year.ToString();
            if (ArchDbName_Txt.Text.Trim() == "") { ArchDbName_Txt.Focus(); return; }
            if (!ArchDbName_Txt.Text.EndsWith(year)) { ArchDbName_Txt.Text += year; }

            if (ArchUser_Txt.Text.Trim() == "") { ArchUser_Txt.Text = ArchDbName_Txt.Text; }
            if (!ArchUser_Txt.Text.EndsWith(year)) { ArchUser_Txt.Text += year; }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ArchUser_Txt_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (ArchUserDesc_Txt.Text.Trim() != "") return;

            ArchUserDesc_Txt.Text = ArchUser_Txt.Text;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion
      #endregion
   }
}
