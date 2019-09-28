using SafeApp.AppBindings;

namespace SafeApp.API
{
    /// <summary>
    /// Files API.
    /// </summary>
    public class Files
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an Files object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal Files(SafeAppPtr appPtr)
            => _appPtr = appPtr;
    }
}
