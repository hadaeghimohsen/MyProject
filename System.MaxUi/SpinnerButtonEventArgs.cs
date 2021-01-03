using System;
using System.Collections.Generic;
using System.Text;

namespace System.MaxUi
{
    /// <summary>
    /// Eventargs raised from a Spinner's ButtonUp event.
    /// </summary>
    public class SpinnerButtonEventArgs : EventArgs
    {
        /// <summary>
        /// Returns true if the spin operation was cancelled using the right mouse button.
        /// </summary>
        public bool SpinCancelled;
    }
}
