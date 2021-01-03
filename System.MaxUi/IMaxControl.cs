using System;
using System.Collections.Generic;
using System.Text;

namespace System.MaxUi
{
    /// <summary>
    /// Implemented by all controls that are linked to 3dsmax via the <see cref="MaxConnection"/> static class.
    /// </summary>
    interface IMaxControl
    {
        /// <summary>
        /// Is called at the time of registering the control with the <see cref="MaxConnection"/> static class, or when the UI colors of 3dsmax have changed.
        /// </summary>
        void UpdateColors();
    }
}
