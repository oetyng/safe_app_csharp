using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

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

        /// <summary>
        /// Parses a string xor url into a XorUrlEncoder.
        /// </summary>
        /// <returns>XorUrlEncoder.</returns>
        public Task<XorUrlEncoder> ParseUrlAsync(string url)
            => AppBindings.ParseUrlAsync(ref _appPtr, url);

        /// <summary>
        /// Parses a safe:// URL and returns all the info in a XorUrlEncoder instance.
        /// It also returns a flag indicating if the URL has to be resolved as NRS-URL
        /// </summary>
        /// <returns>XorUrlEncoder and boolean indicating if the URL has to be resolved as NRS-URL.</returns>
        public Task<(XorUrlEncoder, bool)> ParseAndResolveUrlAsync(string url)
            => AppBindings.ParseAndResolveUrlAsync(ref _appPtr, url);

        /// <summary>
        /// Create a NrsMapContainer.
        /// </summary>
        public Task<(NrsMap, string)> CreateNrsMapContainerAsync(string name, string link, bool directLink, bool dryRun, bool setDefault)
            => AppBindings.CreateNrsMapContainerAsync(ref _appPtr, name, link, directLink, dryRun, setDefault);

        /// <summary>
        /// Add to a NrsMapContainer.
        /// </summary>
        public Task<(NrsMap, string, ulong)> AddToNrsMapContainerAsync(string name, string link, bool setDefault, bool directLink, bool dryRun)
            => AppBindings.AddToNrsMapContainerAsync(ref _appPtr, name, link, setDefault, directLink, dryRun);
    }
}
