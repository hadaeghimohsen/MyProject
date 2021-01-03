using System;
using System.Collections.Generic;
using System.Text;

namespace System.MaxUi
{
    /// <summary>
    /// Eventargs raised from a FlyoutButton or FlyoutCheckButton control.
    /// </summary>
    public class FlyoutEventArgs : EventArgs
    {
        /// <summary>
        /// The index of the relevant item in the flyout control.
        /// </summary>
        public int Index;
    }
}
