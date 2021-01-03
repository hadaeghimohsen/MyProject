using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace System.MaxUi
{
   /// <summary>
   /// A singleton manager class for getting UI information from 3dsmax.
   /// </summary>
   public class MaxConnection
   {
      private static MaxConnection instance;
      /// <summary>
      /// Returns the singleton instance.
      /// </summary>
      public static MaxConnection Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new MaxConnection();
            }
            return instance;
         }
      }

      private bool nonMaxMode;
      private bool max2017OrHigher;
      private Assembly mgdServices;
      private object cuiUpdater;
      private object maxColorManager;
      private Type appSdk;

      internal Color ForeColor;
      internal Color BackColor;
      internal Color CheckedColor;
      internal Color FrameLowColor;
      internal Color FrameHighColor;
      internal Color WindowColor;
      internal Color WindowTextColor;

      internal SolidBrush ForeBrush;
      internal SolidBrush BackBrush;
      internal SolidBrush CheckedBrush;
      internal SolidBrush WindowBrush;
      internal SolidBrush WindowTextBrush;
      internal SolidBrush FrameLowBrush;

      internal Pen ForePen;
      internal Pen BackPen;
      internal Pen FrameLowPen;
      internal Pen FrameHighPen;
      internal Pen FocusPen;

      private List<IMaxControl> Controls;

      private void InitGdiObjects()
      {
         if (ForeBrush != null) ForeBrush.Dispose();
         ForeBrush = new SolidBrush(ForeColor);

         if (BackBrush != null) BackBrush.Dispose();
         BackBrush = new SolidBrush(BackColor);

         if (CheckedBrush != null) CheckedBrush.Dispose();
         CheckedBrush = new SolidBrush(CheckedColor);

         if (WindowBrush != null) WindowBrush.Dispose();
         WindowBrush = new SolidBrush(WindowColor);

         if (WindowTextBrush != null) WindowTextBrush.Dispose();
         WindowTextBrush = new SolidBrush(WindowTextColor);

         if (FrameLowBrush != null) FrameLowBrush.Dispose();
         FrameLowBrush = new SolidBrush(FrameLowColor);

         if (ForePen != null) ForePen.Dispose();
         ForePen = new Pen(ForeColor);

         if (BackPen != null) BackPen.Dispose();
         BackPen = new Pen(BackColor);

         if (FrameLowPen != null) FrameLowPen.Dispose();
         FrameLowPen = new Pen(FrameLowColor);

         if (FrameHighPen != null) FrameHighPen.Dispose();
         FrameHighPen = new Pen(FrameHighColor);

         if (FocusPen != null) FocusPen.Dispose();
         FocusPen = new Pen(Color.Black);
         FocusPen.DashStyle = DashStyle.Dot;
      }

      private MaxConnection()
      {
         Controls = new List<IMaxControl>();
         try
         {
            LoadManagedObjects();
            RegisterCui();
         }
         catch
         {
            nonMaxMode = true;
         }
         UpdateColors();
      }

      private void LoadManagedObjects()
      {
         using (var p = Process.GetCurrentProcess())
         {
            string location = Path.GetDirectoryName(p.MainModule.FileName).TrimEnd('/', '\\');
            string mgdServicesPath = Path.Combine(location, "ManagedServices.dll");
            if (File.Exists(mgdServicesPath))
            {
               //resorting to this because the assembly isn't versioned.
               string release = location.Substring(location.Length - 2);
               int releaseNum;
               if (Int32.TryParse(release, out releaseNum))
               {
                  max2017OrHigher = releaseNum >= 17;
               }

               mgdServices = Assembly.LoadFile(mgdServicesPath);

               Type cuiUpdaterClass = mgdServices.GetType("ManagedServices.CuiUpdater", true, true);
               cuiUpdater = cuiUpdaterClass.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

               appSdk = mgdServices.GetType("ManagedServices.AppSDK", true, true);

               if (max2017OrHigher)
               {
                  Type clrMgrClass = mgdServices.GetType("ManagedServices.MaxColorManager", true, true);
                  maxColorManager = clrMgrClass.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
               }
            }
         }
      }

      private void RegisterCui()
      {
         cuiUpdater.GetType().GetEvent("CUIUpdated").AddEventHandler(cuiUpdater, new EventHandler<EventArgs>(MaxConnection_CUIUpdated));
      }

      /// <summary>
      /// Registers an <see cref="IMaxControl"/> for updates on 3dsmax UI color changes.
      /// </summary>
      /// <param name="ctrl">The <see cref="IMaxControl"/> to register.</param>
      internal void RegisterControl(IMaxControl ctrl)
      {
         if (!Controls.Contains(ctrl))
         {
            Controls.Add(ctrl);
            ctrl.UpdateColors();
         }
      }

      /// <summary>
      /// Unregisters an <see cref="IMaxControl"/> for updates on 3dsmax UI color changes.
      /// </summary>
      /// <param name="ctrl">The <see cref="IMaxControl"/> to unregister.</param>
      internal void UnregisterControl(IMaxControl ctrl)
      {
         if (Controls.Contains(ctrl))
         {
            Controls.Remove(ctrl);
         }
      }

      private void MaxConnection_CUIUpdated(object sender, EventArgs e)
      {
         UpdateColors();
         foreach (IMaxControl ctrl in Controls)
         {
            ctrl.UpdateColors();
         }
      }

      private void UpdateColors()
      {
         if (nonMaxMode)
         {
            ForeColor = SystemColors.ControlText;
            BackColor = Color.FromArgb(197, 197, 197);
            CheckedColor = Color.FromArgb(238, 204, 85);
            FrameLowColor = SystemColors.ControlDark;
            FrameHighColor = SystemColors.ControlLight;
            WindowColor = SystemColors.Window;
            WindowTextColor = SystemColors.WindowText;
         }
         else
         {
            if (max2017OrHigher)
            {
               GetCuiColors2017();
            }
            else
            {
               GetCuiColors();
            }
         }

         InitGdiObjects();
      }

      private Color GetColorMgrColor(int colorId)
      {
         uint colRef = (uint)(maxColorManager.GetType().GetMethod("GetUIColor").Invoke(maxColorManager, new object[] { colorId }));

         byte r = (byte)(colRef & 0xFF);
         byte g = (byte)((colRef >> 8) & 0xFF);
         byte b = (byte)((colRef >> 16) & 0xFF);

         return Color.FromArgb(r, g, b);

      }

      private void GetCuiColors2017()
      {
         const int kText = 1;
         const int kPressedButton = 25;
         const int kBackgroundOdd = 111;
         const int kBackgroundEven = 112;

         ForeColor = GetColorMgrColor(kText);
         BackColor = GetColorMgrColor(kBackgroundEven);
         CheckedColor = GetColorMgrColor(kPressedButton);
         FrameLowColor = GetColorMgrColor(kBackgroundOdd);
         FrameHighColor = GetColorMgrColor(kBackgroundOdd);
         WindowColor = GetColorMgrColor(kBackgroundEven);
         WindowTextColor = GetColorMgrColor(kText);
      }

      private Color GetCuiColor(string methodName)
      {
         return (Color)(cuiUpdater.GetType().GetMethod(methodName).Invoke(cuiUpdater, null));
      }

      private Color GetCuiColor(string methodName, uint idParam)
      {
         return (Color)(cuiUpdater.GetType().GetMethod(methodName).Invoke(cuiUpdater, new object[] { idParam }));
      }

      private void GetCuiColors()
      {
         const uint kText = 1;
         const uint kWindowText = 12;

         ForeColor = GetCuiColor("GetMaxColor", kText);
         BackColor = GetCuiColor("GetControlColor");
         CheckedColor = GetCuiColor("GetButtonPressedColor");
         FrameLowColor = GetCuiColor("GetButtonDarkShadow");
         FrameHighColor = GetCuiColor("GetButtonLightShadow");
         WindowColor = GetCuiColor("GetEditControlColor");
         WindowTextColor = GetCuiColor("GetMaxColor", kWindowText);
      }

      /// <summary>
      /// Draws a 3dsmax compliant frame within a Rectangle.
      /// </summary>
      /// <param name="g">The Graphics object to use for drawing the frame</param>
      /// <param name="bounds">The bounds of the frame. The frame will be drawn <i>inside</i> the rectangle.</param>
      /// <param name="inverted">If true, the frame will appear sunken in, e.g. when a button is pressed.</param>
      public void DrawFrame(Graphics g, Rectangle bounds, bool inverted)
      {
         SmoothingMode s = g.SmoothingMode;
         g.SmoothingMode = SmoothingMode.None;

         g.DrawLines(inverted ? FrameHighPen : FrameLowPen, new Point[] {
                new Point(bounds.Left, bounds.Bottom - 1),
                new Point(bounds.Right - 1, bounds.Bottom - 1),
                new Point(bounds.Right - 1, bounds.Top)});

         g.DrawLines(inverted ? FrameLowPen : FrameHighPen, new Point[] {
                new Point(bounds.Left, bounds.Bottom - 1),
                new Point(bounds.Left, bounds.Top),
                new Point(bounds.Right - 2, bounds.Top)});

         g.SmoothingMode = s;
      }

      /// <summary>
      /// Disable 3dsmax keyboard accelerators.
      /// </summary>
      internal void DisableAccelerators()
      {
         if (!nonMaxMode)
         {
            appSdk.GetMethod("DisableAccelerators", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
         }
      }

      /// <summary>
      /// Enable 3dsmax keyboard accelerators.
      /// </summary>
      internal void EnableAccelerators()
      {
         if (!nonMaxMode)
         {
            appSdk.GetMethod("EnableAccelerators", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
         }
      }
   }
}