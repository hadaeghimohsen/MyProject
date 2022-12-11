using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.MaxUi
{
   internal class FlyoutForm : Form
   {
      private FlyoutButton owner;
      private int buttonWidth;
      private int buttonHeight;
      private ToolTip toolTip;

      private int selectedIndex;

      internal int SelectedIndex
      {
         get { return selectedIndex; }
         set
         {
            if (selectedIndex != value)
            {
               selectedIndex = value;
               string tip = owner.GetTooltip(value);
               if (String.IsNullOrEmpty(tip))
               {
                  toolTip.Hide(this);
               }
               else
               {
                  toolTip.Show(tip, this, buttonWidth + 2, buttonHeight * value + buttonHeight / 2 - 8);
               }
               Invalidate();
            }
         }
      }

      public FlyoutForm(FlyoutButton owner)
      {
         this.owner = owner;
         bool hasImages = owner.Images != null && owner.Images.Images.Count > 0;
         // 1401/10/15
         buttonWidth = /*hasImages ? owner.Images.Images[0].Width :*/ owner.Width;
         buttonHeight = /*hasImages ? owner.Images.Images[0].Height :*/ owner.Height;
         DoubleBuffered = true;
         SetStyle(ControlStyles.Selectable, false);
         InitializeComponent();
      }

      internal void ShowFlyout(Point location)
      {
         Bounds = new Rectangle(location, new System.Drawing.Size(buttonWidth, buttonHeight * owner.Images.Images.Count));
         Show();
         Capture = true;
      }

      protected override void OnMouseUp(MouseEventArgs e)
      {
         base.OnMouseUp(e);
         Close();
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         base.OnMouseMove(e);
         if (ClientRectangle.Contains(e.Location))
         {
            SelectedIndex = e.Y / buttonHeight;
         }
         else
         {
            SelectedIndex = -1;
         }
      }

      protected override void OnPaintBackground(PaintEventArgs e)
      {
         //base.OnPaintBackground(e);
      }

      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams c = base.CreateParams;
            const int WS_POPUP = unchecked((int)0x80000000);
            const int WS_EX_NOACTIVATE = 0x08000000;
            c.Style |= WS_POPUP;
            c.ExStyle |= WS_EX_NOACTIVATE;
            return c;
         }
      }

      protected override bool ShowWithoutActivation
      {
         get { return true; }
      }

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

      public new void Show()
      {
         base.Show();
         IntPtr HWND_TOP = new IntPtr(0);
         const int SWP_NOSIZE = 0x0001;
         const int SWP_NOMOVE = 0x0002;
         const int SWP_NOACTIVATE = 0x0010;
         SetWindowPos(Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOMOVE);
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);
         e.Graphics.Clear(MaxConnection.Instance.BackColor);
         e.Graphics.InterpolationMode = owner.AntiAliasing ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
         for (int i = 0; i < owner.Images.Images.Count; i++)
         {
            //Rectangle r = new Rectangle(0, i * buttonHeight, buttonWidth, buttonHeight);
            // 1401/10/15
            Rectangle _imageRectangle = new Rectangle(
               (owner.Width - owner.Images.ImageSize.Width) / 2,
               i == 0 ? (owner.Height - owner.Images.ImageSize.Height) / 2 :
               i * owner.Height + (owner.Height - owner.Images.ImageSize.Height) / 2,
               owner.Images.ImageSize.Width,
               owner.Images.ImageSize.Height
            );

            // 1401/10/15
            Rectangle _frameRectangle = new Rectangle(
               0,
               i * (owner.Height),
               owner.Width ,
               owner.Height
            );

            if (SelectedIndex == i)
            {
               e.Graphics.FillRectangle(MaxConnection.Instance.CheckedBrush, _frameRectangle);
               Rectangle rChecked = new Rectangle(_imageRectangle.X + 1, _imageRectangle.Y + 1, _imageRectangle.Width, _imageRectangle.Height);
               e.Graphics.DrawImage(owner.Images.Images[i], rChecked);
            }
            else
            {
               e.Graphics.DrawImage(owner.Images.Images[i], _imageRectangle);
            }
            
            MaxConnection.Instance.DrawFrame(e.Graphics, _frameRectangle, SelectedIndex == i);
         }
      }

      protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
      {
         if (owner != null)
         {
            base.SetBoundsCore(x, y, buttonWidth, buttonHeight * owner.Images.Images.Count, specified);
         }
         else
         {
            base.SetBoundsCore(x, y, width, height, specified);
         }
      }

      private void InitializeComponent()
      {
         this.toolTip = new System.Windows.Forms.ToolTip();
         this.SuspendLayout();
         // 
         // toolTip
         // 
         this.toolTip.AutoPopDelay = 5000;
         this.toolTip.InitialDelay = 100;
         this.toolTip.ReshowDelay = 100;
         // 
         // FlyoutForm
         // 
         this.ClientSize = new System.Drawing.Size(32, 32);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "FlyoutForm";
         this.ShowInTaskbar = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.ResumeLayout(false);
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (disposing)
         {
            toolTip.Dispose();
         }
      }
   }
}
