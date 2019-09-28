using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.AppBindings
{
    // ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        #region Low Level
        Task<string> AppExeFileStemAsync();

        Task AppInitLoggingAsync(string outputFileNameOverride);

        Task<string> AppOutputLogPathAsync(string outputFileName);

        Task AppSetAdditionalSearchPathAsync(string newPath);

        Task<(uint, string)> EncodeAuthReqAsync(AuthReq req);

        Task<(uint, string)> EncodeContainersReqAsync(ContainersReq req);

        Task<(uint, string)> EncodeShareMDataReqAsync(ShareMDataReq req);

        Task<(uint, string)> EncodeUnregisteredReqAsync(byte[] extraData);

        bool IsMockBuild();

        #endregion

        #region High Level
        void Connect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb);
        #endregion

        #region XorEncoder

        Task<string> XorurlEncodeAsync(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding);

        Task<XorUrlEncoder> XorurlEncoderAsync(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion);

        Task<XorUrlEncoder> XorurlEncoderFromUrlAsync(string xorUrl);

        #endregion

        #region Fetch

        Task<ISafeData> FetchAsync(IntPtr app, string uri);

        #endregion

        #region Keys

        Task<BlsKeyPair> GenerateKeyPairAsync(IntPtr app);

        Task<(string, BlsKeyPair)> CreateKeysAsync(IntPtr app, string from, string preloadAmount, string pk);

        Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(IntPtr app, string preloadAmount);

        Task<string> KeysBalanceFromSkAsync(IntPtr app, string sk);

        Task<string> KeysBalanceFromUrlAsync(IntPtr app, string url, string sk);

        Task<string> ValidateSkForUrlAsync(IntPtr app, string sk, string url);

        Task<ulong> KeysTransferAsync(IntPtr app, string amount, string fromSk, string toUrl, ulong txId);

        #endregion Keys

        #region Files

        #endregion Files
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
