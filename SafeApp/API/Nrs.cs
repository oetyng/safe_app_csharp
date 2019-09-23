using System;
using SafeApp.AppBindings;

namespace SafeApp.API
{
    /// <summary>
    /// Nrs API.
    /// </summary>
    public class Nrs
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _appPtr;

        /// <summary>
        /// Initializes an NRS object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal Nrs(SafeAppPtr appPtr)
            => _appPtr = appPtr;
    }
}
