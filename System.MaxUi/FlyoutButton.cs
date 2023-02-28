using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace System.MaxUi
{
   /// <summary>
   /// A flyout button such as those found in 3dsmax toolbar menus. This flyout button does not have a checked state (think of the 3dsmax 'align' main toolbar flyout button).
   /// </summary>
   public class FlyoutButton : UserControl, IMaxControl
   {
      /// <summary>
      /// Raised when an item has been selected.
      /// </summary>
      public event EventHandler<FlyoutEventArgs> ItemSelected;
      /// <summary>
      /// Raised after the flyout menu has opened.
      /// </summary>
      public event EventHandler FlyoutOpened;
      /// <summary>
      /// Raised after the flyout menu has closed.
      /// </summary>
      public event EventHandler FlyoutClosed;

      private bool frameOnMouseOverOnly;
      /// <summary>
      /// When true, A frame is only shown around the button when the mouse is over it. Default value: false.
      /// </summary>
      public bool FrameOnMouseOverOnly
      {
         get { return frameOnMouseOverOnly; }
         set
         {
            frameOnMouseOverOnly = value;
            Invalidate();
         }
      }

      private string[] toolTips;
      /// <summary>
      /// Get/set an array of strings which are shown as tooltips when the flyout is open.
      /// The length of the array should match the length of the <see cref="Images"/> array.
      /// </summary>
      public string[] ToolTips
      {
         get { return toolTips; }
         set { toolTips = value; }
      }

      private ImageList images;
      /// <summary>
      /// Get/set the array of images used for all the options of the button.
      /// </summary>
      public ImageList Images
      {
         get { return images; }
         set
         {
            images = value;
            Invalidate();
         }
      }

      private int selectedIndex;
      /// <summary>
      /// Get/set the currently selected index.
      /// Setting this property does not raise the <see cref="ItemSelected"/> event.
      /// </summary>
      public int SelectedIndex
      {
         get { return selectedIndex; }
         set
         {
            selectedIndex = Math.Min(Math.Max(0, value), (images == null ? 0 : images.Images.Count - 1));
            Invalidate();
         }
      }

      private bool antiAliasing;
      /// <summary>
      /// Get/set whether or not the images are drawn using antialiasing.
      /// </summary>
      public bool AntiAliasing
      {
         get { return antiAliasing; }
         set
         {
            antiAliasing = value;
            Invalidate();
         }
      }

      private FlyoffSelectionMode selectionMode;
      /// <summary>
      /// Determines what happens after an item has been selected.
      /// </summary>
      public FlyoffSelectionMode SelectionMode
      {
         get { return selectionMode; }
         set { selectionMode = value; }
      }

      private int flyoutTime;
      /// <summary>
      /// Determines the time in miliseconds required for the mouse to be held down before the flyout is opened.
      /// The default value is 300ms (same as the default 3dsmax flyout time value.)
      /// </summary>
      public int FlyoutTime
      {
         get { return flyoutTime; }
         set
         {
            flyoutTime = value;
            flyoutTimer.Interval = Math.Max(1, value);
         }
      }

      /// <summary>
      /// Gets a value indicating whether or not the flyout is currently open.
      /// </summary>
      public bool IsFlyoutOpen
      {
         get { return flyoutIsOpen; }
      }

      /// <summary>
      /// Updates colors of the control based on colors from <see cref="MaxConnection"/>. Do not call it, it is called internally by the system.
      /// </summary>
      public void UpdateColors()
      {
         BackColor = MaxConnection.Instance.BackColor;
         ForeColor = MaxConnection.Instance.ForeColor;
      }

      protected virtual bool ShowChecked
      {
         get { return mouseDown && !flyoutIsOpen; }
      }

      protected override Size DefaultSize
      {
         get
         {
            return new Size(32, 32);
         }
      }

      private bool mouseOver;
      private bool mouseDown;
      private bool mouseWasDown;
      private bool flyoutIsOpen;
      private Timer flyoutTimer;
      private FlyoutForm flyoutForm;
      private ToolTip toolTip;

      public FlyoutButton()
      {
         flyoutTime = 300;
         flyoutTimer = new Timer();
         flyoutTimer.Interval = flyoutTime;
         flyoutTimer.Tick += new EventHandler(flyoutTimer_Tick);
         toolTip = new ToolTip();
         this.SuspendLayout();
         this.toolTip.AutoPopDelay = 5000;
         this.toolTip.InitialDelay = 100;
         this.toolTip.ReshowDelay = 100;

         SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
         if (DesignMode) return;
         MaxConnection.Instance.RegisterControl(this);
      }

      private void flyoutTimer_Tick(object sender, EventArgs e)
      {
         flyoutTimer.Stop();
         ShowFlyout();
      }

      internal string GetTooltip(int index)
      {
         if (index < 0 ||
             toolTips == null ||
             index >= toolTips.Length)
         {
            return String.Empty;
         }
         return toolTips[index];
      }

      /// <summary>
      /// Open the flyout part programmatically. e.g. - when you want another control to open the flyout.
      /// </summary>
      public void ShowFlyout()
      {
         if (images == null || images.Images.Count == 0) return;

         flyoutIsOpen = true;
         flyoutForm = new FlyoutForm(this);
         flyoutForm.FormClosed += new FormClosedEventHandler(flyoutForm_FormClosed);
         
         // 1401/10/03
         var _scrn = Screen.FromControl(this).Bounds;
         var _crntPontScrn = PointToScreen(new Point(0, Height + 1));

         if(_crntPontScrn.Y + (Height * images.Images.Count) <= _scrn.Height)
            flyoutForm.ShowFlyout(PointToScreen(new Point(0, Height + 1)));
         else
            flyoutForm.ShowFlyout(PointToScreen(new Point(0, -(images.Images.Count * Height + 1) )));

         OnFlyoutOpened(EventArgs.Empty);
         Invalidate();
      }

      /// <summary>
      /// Called when the flyout has opened. Inheritors can override this method to implement functionality.
      /// When overriding, always call the base method.
      /// </summary>
      /// <param name="e"></param>
      protected virtual void OnFlyoutOpened(EventArgs e)
      {
         if (FlyoutOpened != null) FlyoutOpened(this, e);
      }

      /// <summary>
      /// Called when the flyout has closed. Inheritors can override this method to implement functionality.
      /// When overriding, always call the base method.
      /// </summary>
      /// <param name="e"></param>
      protected virtual void OnFlyoutClosed(int index)
      {
         if (FlyoutClosed != null) FlyoutClosed(this, EventArgs.Empty);
         if (index >= 0)
         {
            SelectedIndex = index;
            OnItemSelected(new FlyoutEventArgs() { Index = SelectedIndex });
            if (selectionMode == FlyoffSelectionMode.ResetAfterSelection)
            {
               SelectedIndex = 0;
            }
         }
      }

      /// <summary>
      /// Called when an item has been selected. Inheritors can override this method to implement functionality.
      /// When overriding, always call the base method.
      /// </summary>
      /// <param name="e"></param>
      protected virtual void OnItemSelected(FlyoutEventArgs e)
      {
         if (ItemSelected != null) { ItemSelected(this, e); }
      }

      private void flyoutForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         int index = flyoutForm.SelectedIndex;
         flyoutForm.FormClosed -= flyoutForm_FormClosed;
         flyoutForm = null;
         flyoutIsOpen = false;
         OnFlyoutClosed(index);

         mouseDown = false;
         Invalidate();
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         e.Graphics.InterpolationMode = antiAliasing ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;

         bool showChecked = ShowChecked;
         //BG colored 1 pixel inner frame
         if (showChecked)
         {
            Rectangle r = new Rectangle(2, 2, Width - 4, Height - 4);
            e.Graphics.FillRectangle(MaxConnection.Instance.CheckedBrush, r);
         }

         //frame
         if (!FrameOnMouseOverOnly || mouseOver || showChecked)
         {
            MaxConnection.Instance.DrawFrame(e.Graphics, ClientRectangle, ShowChecked);
         }

         if (images == null) return;

         // 1401/10/15
         Rectangle _imageRectangle =
             new Rectangle(
                 (Width - (images != null ? images.ImageSize.Width : 0)) / 2,
                 (Height - (images != null ? images.ImageSize.Height : 0)) / 2,
                 images.ImageSize.Width,
                 images.ImageSize.Height
             );

         if (images != null && selectedIndex >= 0 && selectedIndex < images.Images.Count)
         {
            if (showChecked)
            {
               using (Matrix m = e.Graphics.Transform)
               {
                  m.Translate(1, 1);
                  e.Graphics.Transform = m;
                  e.Graphics.DrawImage(images.Images[selectedIndex], /*ClientRectangle*/ _imageRectangle);
                  e.Graphics.ResetTransform();
               }
            }
            else
            {
               e.Graphics.DrawImage(images.Images[selectedIndex], /*ClientRectangle*/ _imageRectangle);
            }
         }



         if (images != null && images.Images.Count > 1)
         {
            //flyout hint triangle
            Point[] bigTriangle = new Point[] {
                    new Point(Width - 7, Height - 2),
                    new Point(Width - 2, Height - 7),
                    new Point(Width - 2, Height - 2)};
            Point[] smallTriangle = new Point[] {
                        new Point(Width - 5, Height - 2),
                        new Point(Width - 2, Height - 5),
                        new Point(Width - 2, Height - 2)};
            if (showChecked)
            {
               e.Graphics.FillPolygon(Brushes.Black, bigTriangle);
               e.Graphics.FillPolygon(MaxConnection.Instance.FrameLowBrush, smallTriangle);
            }
            else
            {
               e.Graphics.FillPolygon(Brushes.White, bigTriangle);
               e.Graphics.FillPolygon(Brushes.Black, smallTriangle);
            }
         }
      }

      protected bool IsMouseDown
      {
         get { return mouseDown; }
         set
         {
            if (value != mouseDown)
            {
               mouseDown = value;
               flyoutTimer.Enabled = value;
            }
         }
      }

      /// <summary>
      /// Creates single images from a long (usually rectangular) bitmap and assign them to the Images property.
      /// </summary>
      /// <remarks>
      /// It is safe to call Dispose on the source image after calling this method.
      /// </remarks>
      /// <param name="Strip">The strip of bitmaps.</param>
      /// <param name="NumImages">The number of images in the strip.</param>
      public void SetImageStrip(Bitmap Strip, int NumImages)
      {
         Bitmap[] bmps = new Bitmap[NumImages];
         int height = Strip.Height;
         int width = Strip.Width / NumImages;
         for (int i = 0; i < NumImages; i++)
         {
            int x = Math.Min(i * width, Strip.Width - 1);
            int y = 0;
            int w = Math.Min(width, Strip.Width - x);
            int h = height;

            bmps[i] = Strip.Clone(new Rectangle(x, y, w, h), Strip.PixelFormat);
         }
         //Images = bmps;
      }

      protected override void OnMouseEnter(EventArgs e)
      {
         base.OnMouseEnter(e);
         mouseOver = true;

         if (images != null || images.Images.Count != 0)
         {            
            string tip = GetTooltip(selectedIndex);
            if (String.IsNullOrEmpty(tip))
            {
               toolTip.Hide(this);
            }
            else
            {
               toolTip.Show(tip, this, Width / 2, -Height / 2);
            }
            Invalidate();
         }

         Invalidate();
      }

      protected override void OnMouseDown(MouseEventArgs mevent)
      {
         base.OnMouseDown(mevent);
         IsMouseDown = true;
         Invalidate();
      }

      protected override void OnMouseUp(MouseEventArgs mevent)
      {
         base.OnMouseUp(mevent);
         if (IsMouseDown)
         {
            OnItemSelected(new FlyoutEventArgs() { Index = SelectedIndex });
         }
         IsMouseDown = false;
         mouseWasDown = false;
         Invalidate();
      }

      protected override void OnMouseMove(MouseEventArgs mevent)
      {
         base.OnMouseMove(mevent);
         if (Bounds.Contains(mevent.Location))
         {
            if (mouseWasDown)
            {
               IsMouseDown = true;
               mouseWasDown = false;
               Invalidate();
            }
            if (!mouseOver)
            {
               mouseOver = true;
               Invalidate();
            }
         }
         else
         {
            if (mouseDown)
            {
               IsMouseDown = false;
               mouseWasDown = true;
               Invalidate();
            }
            if (mouseOver)
            {
               mouseOver = false;
               Invalidate();
            }
         }
      }

      protected override void OnMouseLeave(EventArgs e)
      {
         base.OnMouseLeave(e);
         mouseOver = false;
         toolTip.Hide(this);
         Invalidate();
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (disposing)
         {
            flyoutTimer.Dispose();
            if (flyoutForm != null)
            {
               flyoutForm.Dispose();
               toolTip.Dispose();
            }
         }
      }
   }

   /// <summary>
   /// Determines what happens after an item has been selected from the flyout.
   /// </summary>
   public enum FlyoffSelectionMode
   {
      /// <summary>
      /// When an item is selected from the flyout, its index becomes the selected index. This is the default behaviour.
      /// </summary>
      Modal,
      /// <summary>
      /// When an item is selected form the flyout, the selected index returns to 0 after the <see cref="ItemSelected"/> event has been raised.
      /// </summary>
      ResetAfterSelection
   }
}
