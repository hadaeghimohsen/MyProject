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

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private async Task Execute_Query()
      {
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
               MtodBs.DataSource = iScsc.Methods;
               break;
            case "xTp_004":
               CochBs.DataSource = iScsc.Fighters.Where(a => a.FGPB_TYPE_DNRM == "003");
               MtodBs.DataSource = iScsc.Methods.Where(a => a.MTOD_STAT == "002");
               break;
            case "xTp_005":
               CbmtBs.DataSource = iScsc.Club_Methods;
               CntyBs.DataSource = iScsc.Countries;
               MtodBs.DataSource = iScsc.Methods.Where(a => a.MTOD_STAT == "002");
               CochBs.DataSource = iScsc.Fighters.Where(a => a.FGPB_TYPE_DNRM == "003" && a.ACTV_TAG_DNRM == "101");
               break;
            case "xTp_006":
               break;
         }
         requery = false;
         BuildRolloutMenuForCurrentTab();
      }      

      private async void Master_Tc_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
      {
         await Execute_Query();
      }

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
      #endregion

      #region Expense Item tab page
      #region Expense Item Rollout
      private async void Ittp001_Rb_CheckedChanged(object sender, EventArgs e)
      {
         await Execute_Query();
      }

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

            CtgyBs.DataSource = _mtod.Category_Belts;
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
            await Execute_Query();
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
      #endregion

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
            await Execute_Query();
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
      #endregion
   }
}
