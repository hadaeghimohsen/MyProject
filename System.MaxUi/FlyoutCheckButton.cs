using System;
using System.Collections.Generic;
using System.Text;

namespace System.MaxUi
{
    /// <summary>
    /// Similar to the <see cref="FlyoutButton"/>, but also has a checked state. Think of the 3dsmax 'snap' main toolbar flyout button.
    /// </summary>
    public class FlyoutCheckButton : FlyoutButton
    {
        /// <summary>
        /// Raised when the checked state of the control has changed.
        /// </summary>
        public event EventHandler CheckedChanged;

        private bool isChecked;
        /// <summary>
        /// Get/Set the checked state of the control.
        /// </summary>
        public bool Checked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnCheckedChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        protected override bool ShowChecked
        {
            get { return (IsMouseDown && !IsFlyoutOpen) || isChecked; }
        }

        /// <summary>
        /// Called when the checked state of the control has changed. Inheritors can use this method to override default behaviour.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (CheckedChanged != null) CheckedChanged(this, e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs mevent)
        {
            Checked = !Checked;
            base.OnMouseUp(mevent);
        }

        protected override void OnFlyoutClosed(int index)
        {
            base.OnFlyoutClosed(index);
            if (index >= 0) Checked = true;
        }
    }
}
