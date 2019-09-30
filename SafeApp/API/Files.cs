using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

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

        /// <summary>
        /// Create a FilesContainer.
        /// </summary>
        public Task<(string, ProcessedFiles, FilesMap)> CreateFilesContainerAsync(
            string location,
            string destination,
            bool recursive,
            bool dryRun) =>
            AppBindings.CreateFilesContainerAsync(_appPtr, location, destination, recursive, dryRun);
    }
}
