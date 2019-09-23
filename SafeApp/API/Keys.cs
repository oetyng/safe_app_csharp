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
        /// <returns>XOR url of the balance, and a new key pair if no public key was provided.</returns>
        public Task<(string, BlsKeyPair?)> CreateKeysAsync(
            string from,
            string preloadAmount,
            string pk)
            => AppBindings.CreateKeysAsync(ref _appPtr, from, preloadAmount, pk);

        /// <summary>
        /// Create a SafeKey on the network, allocates testcoins onto it, and return the SafeKey's XOR-URL.
        /// </summary>
        /// <param name="preloadAmount">Amount of funds to send.</param>
        /// <returns>XOR url of the balance, and a new key pair.</returns>
        public Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(string preloadAmount)
            => AppBindings.KeysCreatePreloadTestCoinsAsync(ref _appPtr, preloadAmount);

        /// <summary>
        /// Check SafeKey's balance from the network from a given SecretKey string.
        /// </summary>
        /// <param name="sk">The secret key.</param>
        /// <returns>The balance.</returns>
        public Task<string> KeysBalanceFromSkAsync(string sk)
            => AppBindings.KeysBalanceFromSkAsync(ref _appPtr, sk);

        /// <summary>
        /// Check SafeKey's balance from the network from a given XOR/NRS-URL and secret key string.
        /// The difference between this and 'keys_balance_from_sk' function is that this will additionally
        /// check that the XOR/NRS-URL corresponds to the public key derived from the provided secret key.
        /// </summary>
        /// <param name="url">The XOR url.</param>
        /// <param name="sk">The secret key.</param>
        /// <returns>The balance.</returns>
        public Task<string> KeysBalanceFromUrlAsync(string url, string sk)
            => AppBindings.KeysBalanceFromUrlAsync(ref _appPtr, url, sk);

        /// <summary>
        /// Check that the XOR/NRS-URL corresponds to the public key derived from the provided secret key.
        /// </summary>
        /// <param name="url">The XOR url.</param>
        /// <param name="sk">The secret key.</param>
        /// <returns>The public key derived from the secret key.</returns>
        public Task<string> ValidateSkForUrlAsync(string sk, string url)
            => AppBindings.ValidateSkForUrlAsync(ref _appPtr, sk, url);

        /// <summary>
        /// Transfers safecoins from one SafeKey to another, or to a Wallet.
        /// </summary>
        /// <param name="amount">The amount to transfer.</param>
        /// <param name="fromSk">The sender secret key.</param>
        /// <param name="toUrl">The reipient XOR url.</param>
        /// <param name="txId">An optional transaction id. A random will be returned if none specified. </param>
        /// <returns></returns>
        public Task<ulong> KeysTransferAsync(string amount, string fromSk, string toUrl, ulong txId)
            => AppBindings.KeysTransferAsync(ref _appPtr, amount, fromSk, toUrl, txId);
    }
}
