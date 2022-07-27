using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Xml.Linq;

namespace MyProject.Programs.Ui
{
   partial class Wall : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private int dY = 0, dX = 0;

      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               PastManualOnWall(job);
               break;
            case 01:
               PastOnWall(job);
               break;
            case 02:
               RemoveFromWall(job);
               break;
            case 03:
               TruncateWall(job);
               break;
            case 04:
               SetBackColor(job);
               break;
            case 05:
               SetListControls(job);
               break;
            case 06:
               CloseWall(job);
               break;
            case 07:
               SetLockScreenToWallUp(job);
               break;
            case 08:
               GetWallSize(job);
               break;
            case 09:
               JustAddControlOnWall(job);
               break;
            case 10:
               JustAddListControlsOnWall(job);
               break;
            case 11:
               JustRemoveListControlOnWall(job);
               break;
            case 12:
               SetTop(job);
               break;
            case 15:
               Push(job);
               break;
            case 16:
               Pop(job);
               break;
            case 17:
               ResetUi(job);
               break;
            case 18:
               SetTextOnTitleForm(job);
               break;
            case 19:
               SetClearForm(job);
               break;
            case 20:
               FullScreenOrNormal(job);
               break;
            case 21:
               GetToggleStat(job);
               break;
            case 22:
               SetSystemNotification(job);
               break;
            default: break;
         }
      }

      /// <summary>
      /// Code 000
      /// </summary>
      /// <param name="job"></param>
      private void PastManualOnWall(Job job)
      {
         List<object> input = job.Input as List<object>;
         UserControl uc = input[0] as UserControl;
         string settings = input[1] as string;

         dY = toggleMode ? 40 : 0;

         uc.Anchor = AnchorStyles.None;

         switch (settings.Split(':')[0])
         {
            case "top":
               if (InvokeRequired) Invoke(new Action(() => SetTopPosition(uc, settings.Split(':'))));
               else
                  SetTopPosition(uc, settings.Split(':'));
               break;
            case "left":
               if (InvokeRequired) Invoke(new Action(() => SetLeftPosition(uc, settings.Split(':'))));
               else
                  SetLeftPosition(uc, settings.Split(':'));
               break;
            case "bottom":
               if (InvokeRequired) Invoke(new Action(() => SetBottomPosition(uc, settings.Split(':'))));
               else
                  SetBottomPosition(uc, settings.Split(':'));
               break;
            case "right":
               if (InvokeRequired) Invoke(new Action(() => SetRightPosition(uc, settings.Split(':'))));
               else
                  SetRightPosition(uc, settings.Split(':'));
               break;
            case "cntrhrz":
               if (InvokeRequired) Invoke(new Action(() => SetCenterHorizontal(uc, settings.Split(':'))));
               else 
                  SetCenterHorizontal(uc, settings.Split(':'));
               break;
            case "cntrvrt":
               if (InvokeRequired) Invoke(new Action(() => SetCenterVertical(uc, settings.Split(':'))));
               else 
                  SetCenterVertical(uc, settings.Split(':'));
               break;

         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void PastOnWall(Job job)
      {
         UserControl obj = (UserControl)job.Input;

         if(InvokeRequired)
            Invoke(new Action<UserControl>(c =>
            {
               c.Dock = DockStyle.Fill;
               c.Visible = true;
               Controls.Add(c);
               Controls.SetChildIndex(c, 0);
            }), obj);
         else
         {
            obj.Dock = DockStyle.Fill;
            obj.Visible = true;
            Controls.Add(obj);
            Controls.SetChildIndex(obj, 0);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void RemoveFromWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            //obj.Dock = DockStyle.Fill;
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  Controls.Remove(c);
                  if (_ActiveUI.Count() != 1)
                  {
                     ActiveUi activeUi = _ActiveUI.Peek();
                     // 1397/05/20 * User Control not change enabled
                     //activeUi.Ui.Enabled = false;
                  }
               }), obj);
            else
            {
               Controls.Remove(obj);
               if (_ActiveUI.Count() != 1)
               {
                  ActiveUi activeUi = _ActiveUI.Peek();
                  // 1397/05/20 * User Control not change enabled
                  //activeUi.Ui.Enabled = false;
               }
            }

         }
         catch
         {
            UserControl obj = (UserControl)job.Input;
            //obj.Dock = DockStyle.Fill;
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  Controls.Remove(c);
                  if (_ActiveUI.Count() != 1)
                  {
                     ActiveUi activeUi = _ActiveUI.Peek();
                     // 1397/05/20 * User Control not change enabled
                     //activeUi.Ui.Enabled = false;
                  }
               }), obj);
            else
            {
               Controls.Remove(obj);
               if (_ActiveUI.Count() != 1)
               {
                  ActiveUi activeUi = _ActiveUI.Peek();
                  // 1397/05/20 * User Control not change enabled
                  //activeUi.Ui.Enabled = false;
               }
            }
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void TruncateWall(Job job)
      {
         Invoke(new Action(() => Controls.Clear()));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void SetBackColor(Job job)
      {
         List<object> input = job.Input as List<object>;
         Color backcolor = (Color)input[0];
         string applyType = input[1] as string;


         switch (applyType)
         {
            case "default":
            case "now":
               BackColor = backcolor;
               return;
            case "fade":
            default:
               break;
         }

         Color NowColor = BackColor;
         Color DesColor = backcolor;

         RD = DesColor.R;
         BD = DesColor.B;
         GD = DesColor.G;

         if (RD > NowColor.R)
            RID = IncOrDec.Inc;
         else if (RD < NowColor.R)
            RID = IncOrDec.Dec;
         else
            RID = IncOrDec.None;

         if (BD > NowColor.B)
            BID = IncOrDec.Inc;
         else if (BD < NowColor.B)
            BID = IncOrDec.Dec;
         else
            BID = IncOrDec.None;

         if (GD > NowColor.G)
            GID = IncOrDec.Inc;
         else if (GD < NowColor.G)
            GID = IncOrDec.Dec;
         else
            GID = IncOrDec.None;

         Trd_BackColorChanger = new Thread(FuncTrdBackColorChanger);
         Trd_BackColorChanger.Start();

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void SetListControls(Job job)
      {
         var list = job.Input as List<UserControl>;

         foreach (UserControl uc in list)
         {
            Invoke(new Action<UserControl>((control) =>
            {
               control.Dock = DockStyle.Fill;
               control.Visible = true;
               Controls.Add(control);
            }), uc);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void CloseWall(Job job)
      {
         job.Status = StatusType.Successful;
         Close();
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void SetLockScreenToWallUp(Job job)
      {
         UserControl obj = (UserControl)job.Input;

         Invoke(new Action<UserControl>(c =>
         {
            c.Dock = DockStyle.None;
            c.Top = 38 - Height;
            c.Height = Top - 38;
            c.Width = Width - 16;
            //p.Visible = true;
            Controls.Add(c);
            Controls.SetChildIndex(c, 0);
         }), obj);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void GetWallSize(Job job)
      {
         job.Output = this.Size;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void JustAddControlOnWall(Job job)
      {
         UserControl obj = (UserControl)job.Input;

         Invoke(new Action<UserControl>(c =>
         {
            obj.Visible = true;
            Controls.Add(c);
            Controls.SetChildIndex(c, 0);
            c.Refresh();
         }), obj);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void JustAddListControlsOnWall(Job job)
      {
         var list = job.Input as List<UserControl>;

         foreach (UserControl uc in list)
         {
            Invoke(new Action<UserControl>((control) =>
            {
               control.Visible = true;
               Controls.Add(control);
               Controls.SetChildIndex(control, 0);
            }), uc);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void JustRemoveListControlOnWall(Job job)
      {
         var list = job.Input as List<UserControl>;

         foreach (UserControl uc in list)
         {
            Invoke(new Action<UserControl>((control) =>
            {
               Controls.Remove(control);
            }), uc);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void SetTop(Job job)
      {
         var uc = job.Input as UserControl;

         Invoke(new Action<UserControl>((control) =>
         {
            control.Visible = true;
            Controls.Add(control);
            Controls.SetChildIndex(control, 0);
         }), uc);
         job.Status = StatusType.Successful;
      }

      ///// <summary>
      ///// Code 13
      ///// </summary>
      ///// <param name="job"></param>
      //private void PastOnMainPanel(Job job)
      //{
      //    UserControl obj = (UserControl)job.Input;

      //    Invoke(new Action<UserControl>(p =>
      //    {
      //        p.Dock = DockStyle.Fill;
      //        p.Visible = true;
      //        Pn_MainPanelPlace.Controls.Add(p);
      //        Pn_MainPanelPlace.Controls.SetChildIndex(p, 0);
      //    }), obj);

      //    job.Status = StatusType.Successful;
      //}

      ///// <summary>
      ///// Code 14
      ///// </summary>
      ///// <param name="job"></param>
      //private void ChangeVisibleMainPanel(Job job
      //{
      //    Msb_ShowMainDrawer.Visible = Pn_MainPanelPlace.Visible = (bool)job.Input;

      //    job.Status = StatusType.Successful;
      //}

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void Push(Job job)
      {
         List<object> input = job.Input as List<object>;

         if(_ActiveUI.Count == 0)
         {
            job.Status = StatusType.Successful;
            return;
         }
         
         var crntobj = _ActiveUI.FirstOrDefault().Ui;
         if (crntobj == input[1] as UserControl)
         {
            job.Status = StatusType.Successful;
            return;
         }

         _ActiveUI.Push(new ActiveUi { Name = input[0].ToString(), Ui = input[1] as Control });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void Pop(Job job)
      {
         if(_ActiveUI.Count >= 1)
            job.Output = _ActiveUI.Pop();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void ResetUi(Job job)
      {
         if (_ActiveUI.Count > 1)
         {
            if (!InvokeRequired)
            {
               ActiveUi activeUi = _ActiveUI.Peek();
               // 1397/05/20 * User Control not change enabled
               //activeUi.Ui.Enabled = !activeUi.Ui.Enabled;
               if (activeUi.Ui.Enabled) activeUi.Ui.Focus();
            }
            else
            {
               Invoke(new Action(() =>
               {
                  ActiveUi activeUi = _ActiveUI.Peek();
                  // 1397/05/20 * User Control not change enabled
                  //activeUi.Ui.Enabled = !activeUi.Ui.Enabled;
                  if (activeUi.Ui.Enabled) activeUi.Ui.Focus();
               }));
            }
            
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 18
      /// </summary>
      /// <param name="job"></param>
      private void SetTextOnTitleForm(Job job)
      {
         string input = job.Input as string;

         Text = string.Format("کاربر جاری {0} می باشد ", 
            //XElement.Parse(input).Descendants("UserEnName").First().Value,
            XElement.Parse(input).Descendants("UserFaName").First().Value);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 19
      /// </summary>
      /// <param name="job"></param>
      private void SetClearForm(Job job)
      {
         Text = "";
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void FullScreenOrNormal(Job job)
      {
         GoFullscreen();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void GetToggleStat(Job job)
      {
         job.Output = toggleMode;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 22
      /// </summary>
      /// <param name="job"></param>
      private void SetSystemNotification(Job job)
      {
         var ntfydata = job.Input as List<object>;

         SysNtfy_Ni.BalloonTipIcon = (ToolTipIcon)ntfydata[0];
         SysNtfy_Ni.BalloonTipTitle = ntfydata[1].ToString();
         SysNtfy_Ni.BalloonTipText = ntfydata[2].ToString();
         SysNtfy_Ni.Visible = true;
         SysNtfy_Ni.ShowBalloonTip((int)ntfydata[3]);

         //Thread.Sleep((int)ntfydata[3]);

         job.Status = StatusType.Successful;
      }

      #region Private Member
      private void FuncTrdBackColorChanger()
      {
      Loop:
         if (RID == IncOrDec.None && BID == IncOrDec.None && GID == IncOrDec.None)
            return;
         FuncTrdBCC();
         Thread.Sleep(10);
         //Trd_BCC = new Thread(FuncTrdBCC);
         //Trd_BCC.Start();
         //Trd_BCC.Join();
         goto Loop;
      }

      private void FuncTrdBCC()
      {
         Color NowColor = BackColor;
         int Red = NowColor.R, Blue = NowColor.B, Green = NowColor.G;
         switch (RID)
         {
            case IncOrDec.Inc:
               Red++;
               break;
            case IncOrDec.Dec:
               Red--;
               break;
            case IncOrDec.None:
               break;
            default:
               break;
         }
         if (Red == RD)
            RID = IncOrDec.None;

         switch (BID)
         {
            case IncOrDec.Inc:
               Blue++;
               break;
            case IncOrDec.Dec:
               Blue--;
               break;
            case IncOrDec.None:
               break;
            default:
               break;
         }
         if (Blue == BD)
            BID = IncOrDec.None;

         switch (GID)
         {
            case IncOrDec.Inc:
               Green++;
               break;
            case IncOrDec.Dec:
               Green--;
               break;
            case IncOrDec.None:
               break;
            default:
               break;
         }
         if (Green == GD)
            GID = IncOrDec.None;

         BackColor = Color.FromArgb(Red, Green, Blue);

      }
      int RD, GD, BD;
      enum IncOrDec { Inc, Dec, None };
      IncOrDec RID, BID, GID;
      Thread Trd_BackColorChanger;
      #endregion

      #region Private Method
      /// <summary>
      /// set user control location on wall
      /// </summary>
      /// <param name="uc">user control</param>
      /// <param name="settings">
      /// Bnf : Location:State:Size:Alignment
      ///  Location : { top, left, bottom, right, cntrhrz, cntrvrt, (x,y) }
      ///     State : { default, dock, off-screen, in-screen } default is off-screen
      ///      Size : { default, normal, stretch } default is stretch
      /// Alignment : { default, near, center, far } default is near
      /// </param>
      private void SetTopPosition(UserControl uc, string[] settings)
      {
         string state = "";
         string size = "";
         string alignment = "";

         try
         {
            state = settings[1];
            size = settings[2];
            alignment = settings[3];
         }
         catch { }
         finally
         {
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Visible = true;
                  Controls.Add(c);
                  Controls.SetChildIndex(c, 0);
               }), uc);
            else
            {
               uc.Visible = true;
               Controls.Add(uc);
               Controls.SetChildIndex(uc, 0);
            }
         }

         switch (state)
         {
            case "dock":
               uc.Dock = DockStyle.Top;
               return;
            case "default":
            case "off-screen":
               uc.Dock = DockStyle.None;
               uc.Top = -uc.Height;
               uc.Anchor = AnchorStyles.Top;
               break;
            case "in-screen":
               uc.Dock = DockStyle.None;
               uc.Top = 0;
               uc.Anchor = AnchorStyles.Top;
               break;
         }

         switch (size)
         {
            case "normal":
               break;
            case "default":
            case "stretch":
               uc.Left = 0;
               uc.Width = Width;
               uc.Anchor = uc.Anchor | AnchorStyles.Left | AnchorStyles.Right;
               return;
         }

         switch (alignment)
         {
            case "default":
            case "near":
               uc.Left = 0;
               uc.Anchor = uc.Anchor | AnchorStyles.Left;
               break;
            case "center":
               uc.Left = Width / 2 - uc.Width / 2;
               break;
            case "far":
               uc.Left = Width - uc.Width;
               uc.Anchor = uc.Anchor | AnchorStyles.Right;
               break;
         }

      }

      /// <summary>
      /// set user control location on wall
      /// </summary>
      /// <param name="uc">user control</param>
      /// <param name="settings">
      /// Bnf : Location:State:Size:Alignment
      ///  Location : { top, left, bottom, right, cntrhrz, cntrvrt, (x,y) }
      ///     State : { default, dock, off-screen, in-screen } default is off-screen
      ///      Size : { default, normal, stretch } default is stretch
      /// Alignment : { default, near, center, far } default is near
      /// </param>
      private void SetLeftPosition(UserControl uc, string[] settings)
      {
         string state = "";
         string size = "";
         string alignment = "";

         try
         {
            state = settings[1];
            size = settings[2];
            alignment = settings[3];

         }
         catch { }
         finally
         {
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Visible = true;
                  Controls.Add(c);
                  Controls.SetChildIndex(c, 0);
               }), uc);
            else
            {
               uc.Visible = true;
               Controls.Add(uc);
               Controls.SetChildIndex(uc, 0);
            }
         }

         switch (state)
         {
            case "dock":
               uc.Dock = DockStyle.Left;
               return;
            case "default":
            case "off-screen":
               uc.Dock = DockStyle.None;
               uc.Left = -uc.Width;
               uc.Anchor = AnchorStyles.Left;
               break;
            case "in-screen":
               uc.Dock = DockStyle.None;
               uc.Left = 0;
               uc.Anchor = AnchorStyles.Left;
               break;
         }

         switch (size)
         {
            case "normal":
               break;
            case "default":
            case "stretch":
               uc.Top = 0;
               uc.Height = Height;
               uc.Anchor = uc.Anchor | AnchorStyles.Top | AnchorStyles.Bottom;
               return;
         }

         switch (alignment)
         {
            case "default":
            case "near":
               uc.Top = 0;
               uc.Anchor = uc.Anchor | AnchorStyles.Top;
               break;
            case "center":
               uc.Top = Height / 2 - uc.Height / 2;
               break;
            case "far":
               uc.Top = Height - uc.Height;
               uc.Anchor = uc.Anchor | AnchorStyles.Bottom;
               break;
         }
      }

      /// <summary>
      /// set user control location on wall
      /// </summary>
      /// <param name="uc">user control</param>
      /// <param name="settings">
      /// Bnf : Location:State:Size:Alignment
      ///  Location : { top, left, bottom, right, cntrhrz, cntrvrt, (x,y) }
      ///     State : { default, dock, off-screen, in-screen } default is off-screen
      ///      Size : { default, normal, stretch } default is stretch
      /// Alignment : { default, near, center, far } default is near
      /// </param>
      private void SetBottomPosition(UserControl uc, string[] settings)
      {
         string state = "";
         string size = "";
         string alignment = "";

         try
         {
            state = settings[1];
            size = settings[2];
            alignment = settings[3];

         }
         catch { }
         finally
         {
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Visible = true;
                  Controls.Add(c);
                  Controls.SetChildIndex(c, 0);
               }), uc);
            else
            {
               uc.Visible = true;
               Controls.Add(uc);
               Controls.SetChildIndex(uc, 0);
            }
         }

         switch (state)
         {
            case "dock":
               uc.Dock = DockStyle.Bottom;
               return;
            case "default":
            case "off-screen":
               uc.Dock = DockStyle.None;
               uc.Top = Height - 38;
               uc.Anchor = AnchorStyles.Bottom;
               break;
            case "in-screen":
               uc.Dock = DockStyle.None;
               uc.Top = Height - uc.Height - 38;
               uc.Anchor = AnchorStyles.Bottom;
               break;
         }

         switch (size)
         {
            case "normal":
               break;
            case "default":
            case "stretch":
               uc.Left = 0;
               uc.Width = Width;
               uc.Anchor = uc.Anchor | AnchorStyles.Left | AnchorStyles.Right;
               return;
         }

         switch (alignment)
         {
            case "default":
            case "near":
               uc.Left = 0;
               uc.Anchor = uc.Anchor | AnchorStyles.Left;
               break;
            case "center":
               uc.Left = Width / 2 - uc.Width / 2;
               break;
            case "far":
               uc.Left = Width - uc.Width;
               uc.Anchor = uc.Anchor | AnchorStyles.Right;
               break;
         }
      }

      /// <summary>
      /// set user control location on wall
      /// </summary>
      /// <param name="uc">user control</param>
      /// <param name="settings">
      /// Bnf : Location:State:Size:Alignment
      ///  Location : { top, left, bottom, right, cntrhrz, cntrvrt, (x,y) }
      ///     State : { default, dock, off-screen, in-screen } default is off-screen
      ///      Size : { default, normal, stretch } default is stretch
      /// Alignment : { default, near, center, far } default is near
      /// </param>
      private void SetRightPosition(UserControl uc, string[] settings)
      {
         string state = "";
         string size = "";
         string alignment = "";

         try
         {
            state = settings[1];
            size = settings[2];
            alignment = settings[3];

         }
         catch { }
         finally
         {
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Visible = true;
                  Controls.Add(c);
                  Controls.SetChildIndex(c, 0);
               }), uc);
            else
            {
               uc.Visible = true;
               Controls.Add(uc);
               Controls.SetChildIndex(uc, 0);
            }
         }

         switch (state)
         {
            case "dock":
               uc.Dock = DockStyle.Right;
               return;
            case "default":
            case "off-screen":
               uc.Dock = DockStyle.None;
               uc.Left = Width;
               uc.Anchor = AnchorStyles.Right;
               break;
            case "in-screen":
               uc.Dock = DockStyle.None;
               uc.Left = Width - uc.Width;
               uc.Anchor = AnchorStyles.Right;
               break;
         }
         
         switch (size)
         {
            case "normal":
               break;
            case "default":
            case "stretch":
               uc.Top = 0;
               uc.Height = Height - dY;
               uc.Anchor = uc.Anchor | AnchorStyles.Top | AnchorStyles.Bottom;
               return;
         }

         switch (alignment)
         {
            case "default":
            case "near":
               uc.Top = 0;
               uc.Anchor = uc.Anchor | AnchorStyles.Top;
               break;
            case "center":
               uc.Top = Height / 2 - uc.Height / 2;
               break;
            case "far":
               uc.Top = Height - uc.Height;
               uc.Anchor = uc.Anchor | AnchorStyles.Bottom;
               break;
         }
      }

      /// <summary>
      /// set user control location on wall
      /// </summary>
      /// <param name="uc">user control</param>
      /// <param name="settings">
      /// Bnf : Location:State:Size:Alignment
      ///  Location : { top, left, bottom, right, cntrhrz, cntrvrt, (x,y) }        
      ///      Size : { default, normal, stretch } default is stretch
      /// </param>
      private void SetCenterHorizontal(UserControl uc, string[] settings)
      {
         string size = "";

         try
         {
            size = settings[1];
         }
         catch { }
         finally
         {
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Visible = true;
                  if (c.Parent == null)
                     Controls.Add(c);
                  Controls.SetChildIndex(c, 0);
               }), uc);
            else
            {
               uc.Visible = true;
               if (uc.Parent == null)
                  Controls.Add(uc);
               Controls.SetChildIndex(uc, 0);
            }

         }

         switch (size)
         {
            case "normal":
               uc.Top = Height / 2 - uc.Height / 2;
               uc.Left = Width / 2 - uc.Width / 2;
               uc.Anchor = AnchorStyles.None;
               break;
            case "default":
            case "stretch":
               uc.Top = Height / 2 - uc.Height / 2;
               uc.Left = 0;
               uc.Width = Width;
               uc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
               return;
         }
      }

      /// <summary>
      /// set user control location on wall
      /// </summary>
      /// <param name="uc">user control</param>
      /// <param name="settings">
      /// Bnf : Location:State:Size:Alignment
      ///  Location : { top, left, bottom, right, cntrhrz, cntrvrt, (x,y) }        
      ///      Size : { default, normal, stretch } default is stretch
      /// </param>
      private void SetCenterVertical(UserControl uc, string[] settings)
      {
         string size = "";

         try
         {
            size = settings[1];
         }
         catch { }
         finally
         {
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Visible = true;
                  Controls.Add(c);
                  Controls.SetChildIndex(c, 0);
               }), uc);
            else
            {
               uc.Visible = true;
               Controls.Add(uc);
               Controls.SetChildIndex(uc, 0);
            }
         }

         switch (size)
         {
            case "normal":
               uc.Top = Height / 2 - uc.Height / 2;
               uc.Left = Width / 2 - uc.Width / 2;
               uc.Anchor = AnchorStyles.None;
               break;
            case "default":
            case "stretch":
               uc.Top = 0;
               uc.Left = Width / 2 - uc.Width / 2;
               uc.Height = Height;
               uc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
               return;
         }
      }
      #endregion
   }
}
