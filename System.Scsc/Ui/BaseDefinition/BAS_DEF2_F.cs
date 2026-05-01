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
using System.MaxUi;
using System.Scsc.ExtCode;
using DevExpress.XtraTab;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_DEF2_F : UserControl
   {
      public BAS_DEF2_F()
      {
         InitializeComponent();

         Flp_001.EnableDragScroll();
         Flp_002.EnableDragScroll();
         Flp_003.EnableDragScroll();
         Flp_004.EnableDragScroll();
         Flp_005.EnableDragScroll();
      }

      private bool requery = false, isLoading = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private async Task Execute_Query()
      {
         isLoading = true;
         await Task.Yield();
         iScsc = new Data.iScscDataContext(ConnectionString);

         switch (Master_Tc.SelectedTabPage.Name)
         {
            case "xTp_001":
               
               break;
            case "xTp_002":
               
               break;
            case "xTp_003":
               // مقادیر ثابت               
               var __pos = iScsc.V_Pos_Devices.AsQueryable();

               if(PosExst_Fcb.Checked)
               {
                  switch (PosExst_Fcb.SelectedIndex)
                  {
                     case 0:
                        __pos =
                           from p in __pos
                           where (from pt in iScsc.V_Pos_Transaction_Logs
                                  where pt.POSD_PSID == p.PSID
                                  select pt).Any()
                           select p;
                        break;
                     case 1:
                        __pos =
                           from p in __pos
                           where !(from pt in iScsc.V_Pos_Transaction_Logs
                                   where pt.POSD_PSID == p.PSID
                                   select pt).Any()
                           select p;
                        break;
                  }
               }

               vPosBs.DataSource = __pos;
               break;
            case "xTp_004":
               // مقادیر ثابت
               var _trgtEdevStat = EdevStat_Fcb.SelectedIndex == 0 ? "002" : "001";

               // شروع با کل Methods
               var __Edev = iScsc.External_Devices.AsQueryable();

               // اعمال فیلتر MtodStat
               if (EdevStat_Fcb.Checked)
               {
                  __Edev = __Edev.Where(a => a.STAT == _trgtEdevStat);
               }

               ExdvBs.DataSource = __Edev;
               break;
            case "xTp_005":
               ComaBs.DataSource = iScsc.Computer_Actions;               
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

      #region Network Computer tab page
      #region Network Computer rollout
      private void AddComa_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self)
               {
                  AfterChangedOutput =
                     new Action<object>(
                        (output) =>
                        {
                           if (ComaBs.List.OfType<Data.Computer_Action>().Any(ca => ca.CODE == 0)) return;

                           var hostinfo = output as XElement;

                           if (ComaBs.List.OfType<Data.Computer_Action>().Any(ca => ca.COMP_NAME == hostinfo.Attribute("name").Value)) return;

                           ComaBs.AddNew();
                           var coma = ComaBs.Current as Data.Computer_Action;
                           coma.COMP_NAME = hostinfo.Attribute("name").Value;
                           coma.IP_ADRS = hostinfo.Attribute("ip").Value;
                           coma.CHCK_ATTN_ALRM = "001";
                           coma.CHCK_DOBL_ATTN_STAT = "001";

                           SaveComa_Btn_Click(null, null);
                        }
                     )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveComa_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            ComaBs.EndEdit();
            Coma_Gv.PostEditor();

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>234</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    //iScsc.ExecuteCommand(string.Format("DELETE dbo.Computer_Action WHERE Code = {0}", coma.CODE));
                                    iScsc.SubmitChanges();
                                    requery = true;
                                    return;
                                 }
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 234 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),                  
                  })
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         
         if (requery)
         {
            await Execute_Query();
         }
      }

      private async void DelComa_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            ComaBs.EndEdit();
            Coma_Gv.PostEditor();

            var coma = ComaBs.Current as Data.Computer_Action;
            if (coma == null || coma.CODE == 0) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>235</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    iScsc.ExecuteCommand(string.Format("DELETE dbo.Computer_Action WHERE Code = {0}", coma.CODE));
                                    requery = true;
                                    return;
                                 }
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 235 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),                  
                  })   
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         if (requery)
         {
            await Execute_Query();
         }
      }
      #endregion

      #region Online Locker Configuration rollout
      private void LevlApbs_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Dresser_Level"),
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
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion
      #endregion

      #region External Device tab page
      #region External Device rollout
      private void AddEdev_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ExdvBs.List.OfType<Data.External_Device>().Any(a => a.CODE == 0)) return;

            var __exdv = ExdvBs.AddNew() as Data.External_Device;
            __exdv.STAT = "002";

            iScsc.External_Devices.InsertOnSubmit(__exdv);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void SaveEdev_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            ExdvBs.EndEdit();
            Exdv_Gv.PostEditor();

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

      private async void DelEdev_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var __exdv = ExdvBs.Current as Data.External_Device;
            if (__exdv == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.External_Devices.DeleteOnSubmit(__exdv);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private void AddEdml_Btn_Click(object sender, EventArgs e)
      {
         var edev = ExdvBs.Current as Data.External_Device;
         if (edev == null) return;

         if (EdlmBs.List.OfType<Data.External_Device_Link_Method>().Any(x => x.CODE == 0)) return;

         var edlm = EdlmBs.AddNew() as Data.External_Device_Link_Method;
         edlm.External_Device = edev;

         iScsc.External_Device_Link_Methods.InsertOnSubmit(edlm);
      }

      private async void DelEdml_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var edlm = EdlmBs.Current as Data.External_Device_Link_Method;
            if (edlm == null) return;

            if (MessageBox.Show(this, "آیا حذف لینک گروه موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.External_Device_Link_Methods.DeleteOnSubmit(edlm);

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

      private async void SaveEdml_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Edlm_Gv.PostEditor();
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

      private void ShowMtod_Btn_Click(object sender, EventArgs e)
      {

      }

      private void AddElnd_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var edev = ExdvBs.Current as Data.External_Device;
            if (edev == null) return;

            if (CExdvBs.List.OfType<Data.External_Device_Link_External_Device>().Any(ele => ele.CODE == 0)) return;

            var cedev = CExdvBs.AddNew() as Data.External_Device_Link_External_Device;
            iScsc.External_Device_Link_External_Devices.InsertOnSubmit(cedev);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void DelElnd_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var cedev = CExdvBs.Current as Data.External_Device_Link_External_Device;
            if (cedev == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.External_Device_Link_External_Device WHERE Code = {0};", cedev.CODE));

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }

         if (requery)
            await Execute_Query();
      }

      private async void SaveElnd_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            CExdv_Gv.PostEditor();
            CExdvBs.EndEdit();

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

      private void AddEdwk_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edev = ExdvBs.Current as Data.External_Device;
            if (_edev == null) return;

            if (EdvwBs.List.OfType<Data.External_Device_Weekday>().Any(ex => ex.CODE == 0)) return;

            var _edvw = EdvwBs.AddNew() as Data.External_Device_Weekday;
            _edvw.External_Device = _edev;
            _edvw.STAT = "002";

            iScsc.External_Device_Weekdays.InsertOnSubmit(_edvw);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void DelEdwk_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edvw = EdvwBs.Current as Data.External_Device_Weekday;
            if (_edvw == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.External_Device_Weekdays.DeleteOnSubmit(_edvw);
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

      private async void SaveEdwk_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Edvw_Gv.PostEditor();
            Edwt_Gv.PostEditor();

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

      private void AddEdwt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edvw = EdvwBs.Current as Data.External_Device_Weekday;
            if (_edvw == null) return;

            if (EdwtBs.List.OfType<Data.External_Device_Weekday_Timing>().Any(ex => ex.CODE == 0)) return;

            var _edwt = EdwtBs.AddNew() as Data.External_Device_Weekday_Timing;
            _edwt.External_Device_Weekday = _edvw;
            _edwt.STRT_TIME = DateTime.Now;
            _edwt.END_TIME = DateTime.Now.AddHours(2);
            _edwt.STAT = "002";

            iScsc.External_Device_Weekday_Timings.InsertOnSubmit(_edwt);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void DelEdwt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var _edwt = EdwtBs.Current as Data.External_Device_Weekday_Timing;
            if (_edwt == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.External_Device_Weekday_Timings.DeleteOnSubmit(_edwt);
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

      private async void SaveEdwt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            Edvw_Gv.PostEditor();
            Edwt_Gv.PostEditor();

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

      #region Pos Devices tab page
      #region Pos Devices rollout
      private void vPosBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var __pos = vPosBs.Current as Data.V_Pos_Device;
            if (__pos == null) return;

            switch (Master_Tc.SelectedTabPage.Name)
            {
               case "xTp_003":
                  if(PosTranLog_Rlt.RolloutStatus)
                  {
                     // مقادیر ثابت
                     //var __trgtPtrnStat = PtrnStat_Fcb.SelectedIndex == 0 ? "002" : "001";

                     // شروع با کل Methods
                     var __ptrnLog = iScsc.V_Pos_Transaction_Logs.AsQueryable();

                     // اعمال فیلتر MtodStat
                     //if (PtrnStat_Fcb.Checked)
                     //{
                     //   __ptrnLog = __ptrnLog.Where(a => a.PAY_STAT == __trgtPtrnStat);
                     //}

                     if (FromPtrnDate_Dt.Value.HasValue)
                        __ptrnLog = __ptrnLog.Where(a => a.TRAN_DATE >= FromPtrnDate_Dt.Value);
                     if (ToPtrnDate_Dt.Value.HasValue)
                        __ptrnLog = __ptrnLog.Where(a => a.TRAN_DATE <= ToPtrnDate_Dt.Value);
                     if (PtrnCretBy_Lov.EditValue != null && PtrnCretBy_Lov.EditValue.ToString() != "")
                        __ptrnLog = __ptrnLog.Where(a => a.CRET_BY == PtrnCretBy_Lov.EditValue.ToString());
                     if (PayStat_Cbx.Checked)
                        __ptrnLog = __ptrnLog.Where(a => a.PAY_STAT == "002");
                     else
                        __ptrnLog = __ptrnLog.Where(a => a.PAY_STAT == "001");

                     if (Rqtp001_Cbx.Checked)
                        __ptrnLog = __ptrnLog.Where(a => a.RQTP_CODE == "001");
                     if (Rqtp009_Cbx.Checked)
                        __ptrnLog = __ptrnLog.Where(a => a.RQTP_CODE == "009");
                     if (Rqtp016_Cbx.Checked)
                        __ptrnLog = __ptrnLog.Where(a => a.RQTP_CODE == "016");

                     if (RqstCretBy_Lov.EditValue != null && RqstCretBy_Lov.EditValue.ToString() != "")
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.CRET_BY == RqstCretBy_Lov.EditValue.ToString()
                                   select a;

                     if(InvcNumb_Txt.Text != "")
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.INVC_NUMB == InvcNumb_Txt.Text.ToInt64()
                                    select a;

                     if(FromRqstDate_Dt.Value.HasValue)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.RQST_DATE.Value >= FromRqstDate_Dt.Value
                                    select a;

                     if (ToRqstDate_Dt.Value.HasValue)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.RQST_DATE.Value <= ToRqstDate_Dt.Value
                                    select a;

                     if (FromSaveDate_Dt.Value.HasValue)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.SAVE_DATE.Value >= FromSaveDate_Dt.Value
                                    select a;

                     if (ToSaveDate_Dt.Value.HasValue)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.SAVE_DATE.Value <= ToSaveDate_Dt.Value
                                    select a;

                     if (FromInvcDate_Dt.Value.HasValue)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.INVC_DATE.Value >= FromInvcDate_Dt.Value
                                    select a;

                     if (ToInvcDate_Dt.Value.HasValue)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.INVC_DATE.Value <= ToInvcDate_Dt.Value
                                    select a;

                     if(RqstStat001_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.RQST_STAT == "001"
                                    select a;
                     if (RqstStat002_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.RQST_STAT == "002"
                                    select a;
                     if (RqstStat003_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Requests on a.RQID equals b.RQID
                                    where b.RQST_STAT == "003"
                                    select a;

                     if (FngrPrnt_Txt.Text != "")
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.FNGR_PRNT_DNRM == FngrPrnt_Txt.Text
                                    select a;

                     if (Name_Txt.Text != "")
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.NAME_DNRM.Contains(Name_Txt.Text)
                                    select a;

                     if (CellPhon_Txt.Text != "")
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.CELL_PHON_DNRM == CellPhon_Txt.Text
                                    select a;

                     if (NatlCode_Txt.Text != "")
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.NATL_CODE_DNRM == NatlCode_Txt.Text
                                    select a;

                     if(DebtAmnt_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.DEBT_DNRM > 0
                                    select a;

                     if (DpstAmnt_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.DPST_AMNT_DNRM > 0
                                    select a;

                     if(Women_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.SEX_TYPE_DNRM == "002"
                                    select a;

                     if (Men_Cbx.Checked)
                        __ptrnLog = from a in __ptrnLog
                                    join b in iScsc.Fighters on a.FILE_NO equals b.FILE_NO
                                    where b.SEX_TYPE_DNRM == "001"
                                    select a;

                     vPosTBs.DataSource = __ptrnLog;
                  }
                  break;
            }

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion      

      #region Pos Transaction Log rollout
      private void ClerPtrn_Btn_Click(object sender, EventArgs e)
      {
         FromPtrnDate_Dt.Value = ToPtrnDate_Dt.Value = null;
         FngrPrnt_Txt.Text = Name_Txt.Text = CellPhon_Txt.Text = NatlCode_Txt.Text = "";
         DebtAmnt_Cbx.Checked = DpstAmnt_Cbx.Checked = Women_Cbx.Checked = Men_Cbx.Checked = false;
         PtrnCretBy_Lov.EditValue = null;
         PayStat_Cbx.Checked = true;
         Rqtp001_Cbx.Checked = Rqtp009_Cbx.Checked = Rqtp016_Cbx.Checked = false;
         RqstCretBy_Lov.EditValue = null;
         InvcNumb_Txt.Text = "";
         FromRqstDate_Dt.Value = ToRqstDate_Dt.Value = FromSaveDate_Dt.Value = ToSaveDate_Dt.Value = FromInvcDate_Dt.Value = ToInvcDate_Dt.Value = null;
         RqstStat001_Cbx.Checked = RqstStat002_Cbx.Checked = RqstStat003_Cbx.Checked = false;
      }

      private async void FindPtrn_Butn_Click(object sender, EventArgs e)
      {
         await Execute_Query();
      }

      private void FngrPrnt01_Btx_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  if (FngrPrnt01_Btx.EditValue == null || FngrPrnt01_Btx.EditValue.ToString() == "") { FngrPrnt01_Btx.Focus(); return; }
                  FighBs.DataSource = iScsc.Fighters.FirstOrDefault(a => a.FNGR_PRNT_DNRM == FngrPrnt01_Btx.Text);
                  break;
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 #region Access Privilege
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>260</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 260 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */),
                           new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"))}
                        })
                  );
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

      private void ShowProfile01_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt01_Btx.Text == "") return;

            var __figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == FngrPrnt01_Btx.Text);
            if (__figh == null)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "MAIN_PAGE_F", 44 /* PlaySystemSound */, SendType.SelfToUserInterface)
                  {
                     Input = new XElement("Sound", new XAttribute("type", "031"))
                  }
               );

               return;
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", __figh.FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void TranServ2Serv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var __tran = vPosTBs.Current as Data.V_Pos_Transaction_Log;
            if (__tran == null) return;

            if (FngrPrnt01_Btx.EditValue == null || FngrPrnt01_Btx.EditValue.ToString() == "") { FngrPrnt01_Btx.Focus(); return; }

            if (MessageBox.Show(this, "آیا با انتقال اطلاعات از مشتری به مشتری دیگر موافق هستید؟", "انتقال تراکنش", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            await Task.Yield();

            TrsnServ2Serv_Mpb.Visible = true;

            iScsc.DUP_RQST_P(
               new XElement("Duplicate",
                   new XAttribute("type", "copy"),
                   new XAttribute("rqid", __tran.RQID),
                   new XAttribute("fngrprnt", FngrPrnt01_Btx.Text),
                   new XElement("Transaction", __tran.TLID)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            TrsnServ2Serv_Mpb.Visible = false;
         }

         if (requery)
            await Execute_Query();
      }

      private void vPosTBs_CurrentChanged(object sender, EventArgs e)
      {
         var __tran = vPosTBs.Current as Data.V_Pos_Transaction_Log;
         if (__tran == null) return;

         switch(xTp003_Tc.SelectedTabPage.Name)
         {
            case "xTp_00304":
               RqroBs.DataSource = iScsc.Request_Rows.Where(a => a.RQST_RQID == __tran.RQID);
               break;
         }
      }
      #endregion      

      
      #endregion

      #region SMS Config tab page
      #region SMS Config rollout

      #endregion
      #endregion
   }
}
