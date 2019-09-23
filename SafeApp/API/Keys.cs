using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Keys API.
    /// </summary>
    public class Keys
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _appPtr;

        /// <summary>
        /// Initializes an Keys object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal Keys(SafeAppPtr appPtr)
            => _appPtr = appPtr;

        /// <summary>
        /// Generate a key pair without creating and/or storing a SafeKey on the network.
        /// </summary>
        /// <returns>Key pair.</returns>
        public Task<BlsKeyPair> GenerateKeyPairAsync()
            => AppBindings.GenerateKeyPairAsync(ref _appPtr);

        /// <summary>
        /// Create a SafeKey on the network and returns its XOR-URL.
        /// Optionally pre-loads it with an amount.
        /// Also returns new key pair if a recipient public key was not provided.
        /// </summary>
        /// <param name="from">Secret key of source funds.</param>
        /// <param name="preloadAmount">Optional amount of funds to send.</param>
        /// <param name="pk">Public key of recipient if provided.</param>
        /// <returns>Xor url of the balance, and a new key pair if no public key was provided.</returns>
        public Task<(string, BlsKeyPair?)> CreateKeysAsync(
            string from,
            string preloadAmount,
            string pk)
            => AppBindings.CreateKeysAsync(ref _appPtr, from, preloadAmount, pk);

        public Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(string preloadAmount)
            => AppBindings.KeysCreatePreloadTestCoinsAsync(ref _appPtr, preloadAmount);
    }
}
