using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.AppBindings
{
    internal partial class AppBindings : IAppBindings
    {
        #region Connect
        public void Connect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb)
        {
            var userData = BindingUtils.ToHandlePtr(oCb);
            ConnectNative(appId, authCredentials, userData, DelegateOnFfiResultSafeCb);
        }

        [DllImport(DllName, EntryPoint = "connect")]
        private static extern void ConnectNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            [MarshalAs(UnmanagedType.LPStr)] string authCredentials,
            IntPtr userData,
            FfiResultSafeCb oCb);

        private delegate void FfiResultSafeCb(IntPtr userData, IntPtr result, IntPtr app);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultSafeCb))]
#endif
        private static void OnFfiResultSafeCb(IntPtr userData, IntPtr result, IntPtr app)
        {
            var action = BindingUtils.FromHandlePtr<Action<FfiResult, IntPtr, GCHandle>>(userData, false);
            action(Marshal.PtrToStructure<FfiResult>(result), app, GCHandle.FromIntPtr(userData));
        }

        private static readonly FfiResultSafeCb DelegateOnFfiResultSafeCb = OnFfiResultSafeCb;

        #endregion

        #region XorUrl
        public Task<string> XorurlEncodeAsync(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            XorurlEncodeNative(
                name,
                typeTag,
                dataType,
                contentType,
                path,
                subNames,
                contentVersion,
                baseEncoding,
                userData,
                DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encode")]
        private static extern void XorurlEncodeNative(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            [MarshalAs(UnmanagedType.LPStr)] string baseEncoding,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<XorUrlEncoder> XorurlEncoderAsync(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            XorurlEncoderNative(
                name,
                typeTag,
                dataType,
                contentType,
                path,
                subNames,
                contentVersion,
                userData,
                DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encoder")]
        private static extern void XorurlEncoderNative(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        public Task<XorUrlEncoder> XorurlEncoderFromUrlAsync(string xorUrl)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            XorurlEncoderFromUrlNative(xorUrl, userData, DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encoder_from_url")]
        private static extern void XorurlEncoderFromUrlNative(
            [MarshalAs(UnmanagedType.LPStr)] string xorUrl,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        private delegate void FfiResultXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorurlEncoder);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultXorUrlEncoderCb))]
#endif
        private static void OnFfiResultXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorurlEncoder)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<XorUrlEncoder>(xorurlEncoder));
        }

        private static readonly FfiResultXorUrlEncoderCb DelegateOnFfiResultXorUrlEncoderCb = OnFfiResultXorUrlEncoderCb;

        #endregion

        #region Fetch

        public Task<ISafeData> FetchAsync(IntPtr app, string url)
        {
            var (task, userData) = BindingUtils.PrepareTask<ISafeData>();
            FetchNative(
              app,
              url,
              userData,
              DelegateOnFfiResultPublishedImmutableDataCb,
              DelegateOnFfiResultWalletCb,
              DelegateOnFfiResultSafeKeyCb,
              DelegateOnFfiResultFilesContainerCb,
              DelegateOnFfiFetchFailedCb);
            return task;
        }

        [DllImport(DllName, EntryPoint = "fetch")]
        private static extern void FetchNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultPublishedImmutableDataCb oPublished,
            FfiResultWalletCb oWallet,
            FfiResultSafeKeyCb oKeys,
            FfiResultFilesContainerCb oContainer,
            FfiFetchFailedCb oErr);

        private delegate void FfiResultPublishedImmutableDataCb(IntPtr userData, IntPtr publishedImmutableData);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultPublishedImmutableDataCb))]
#endif
        private static void OnFfiResultPublishedImmutableDataCb(IntPtr userData, IntPtr publishedImmutableData)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new PublishedImmutableData(Marshal.PtrToStructure<PublishedImmutableDataNative>(publishedImmutableData)));
        }

        private static readonly FfiResultPublishedImmutableDataCb DelegateOnFfiResultPublishedImmutableDataCb = OnFfiResultPublishedImmutableDataCb;

        private delegate void FfiResultWalletCb(IntPtr userData, IntPtr wallet);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultWalletCb))]
#endif
        private static void OnFfiResultWalletCb(IntPtr userData, IntPtr wallet)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(Marshal.PtrToStructure<Wallet>(wallet));
        }

        private static readonly FfiResultWalletCb DelegateOnFfiResultWalletCb = OnFfiResultWalletCb;

        private delegate void FfiResultSafeKeyCb(IntPtr userData, IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultSafeKeyCb))]
#endif
        private static void OnFfiResultSafeKeyCb(IntPtr userData, IntPtr safeKey)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(Marshal.PtrToStructure<SafeKey>(safeKey));
        }

        private static readonly FfiResultSafeKeyCb DelegateOnFfiResultSafeKeyCb = OnFfiResultSafeKeyCb;

        private delegate void FfiResultFilesContainerCb(IntPtr userData, IntPtr filesContainer);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultFilesContainerCb))]
#endif
        private static void OnFfiResultFilesContainerCb(IntPtr userData, IntPtr filesContainer)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(Marshal.PtrToStructure<FilesContainer>(filesContainer));
        }

        private static readonly FfiResultFilesContainerCb DelegateOnFfiResultFilesContainerCb = OnFfiResultFilesContainerCb;

        private delegate void FfiFetchFailedCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiFetchFailedCb))]
#endif
        private static void OnFfiFetchFailedCb(IntPtr userData, IntPtr result)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            var ffiResult = Marshal.PtrToStructure<FfiResult>(result);
            tcs.SetResult(new SafeDataFetchFailed(ffiResult.ErrorCode, ffiResult.Description));
        }

        private static readonly FfiFetchFailedCb DelegateOnFfiFetchFailedCb = OnFfiFetchFailedCb;

        #endregion Connect

        #region Keys

        public Task<BlsKeyPair> GenerateKeyPairAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<BlsKeyPair>();
            GenerateKeyPairNative(app, userData, DelegateOnFfiResultBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "generate_keypair")]
        private static extern void GenerateKeyPairNative(
            IntPtr app,
            IntPtr userData,
            FfiResultBlsKeyPairCb oCb);

        private delegate void FfiResultBlsKeyPairCb(IntPtr userData, IntPtr result, IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultBlsKeyPairCb))]
#endif
        private static void OnFfiResultBlsKeyPairCb(IntPtr userData, IntPtr result, IntPtr safeKey)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<BlsKeyPair>(safeKey));

        private static readonly FfiResultBlsKeyPairCb DelegateOnFfiResultBlsKeyPairCb = OnFfiResultBlsKeyPairCb;

        public Task<(string, BlsKeyPair)> CreateKeysAsync(
            IntPtr app,
            string from,
            string preloadAmount,
            string pk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair)>();
            CreateKeysNative(app,  from, preloadAmount, pk, userData, DelegateOnFfiResultStringBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_create")]
        private static extern void CreateKeysNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string from,
            [MarshalAs(UnmanagedType.LPStr)] string preload,
            [MarshalAs(UnmanagedType.LPStr)] string pk,
            IntPtr userData,
            FfiResultStringBlsKeyPairCb oCb);

        public Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(IntPtr app, string preloadAmount)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair)>();
            KeysCreatePreloadTestCoinsNative(app, preloadAmount, userData, DelegateOnFfiResultStringBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_create_preload_test_coins")]
        private static extern void KeysCreatePreloadTestCoinsNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string preload,
            IntPtr userData,
            FfiResultStringBlsKeyPairCb oCb);

        private delegate void FfiResultStringBlsKeyPairCb(
            IntPtr userData,
            IntPtr result,
            string xorUrl,
            IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringBlsKeyPairCb))]
#endif
        private static void OnFfiResultStringBlsKeyPairCb(
            IntPtr userData,
            IntPtr result,
            string xorUrl,
            IntPtr safeKey)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (xorUrl, Marshal.PtrToStructure<BlsKeyPair>(safeKey)));

        private static readonly FfiResultStringBlsKeyPairCb DelegateOnFfiResultStringBlsKeyPairCb = OnFfiResultStringBlsKeyPairCb;

        public Task<string> KeysBalanceFromSkAsync(IntPtr app, string sk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            KeysBalanceFromSkNative(app, sk, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_balance_from_sk")]
        private static extern void KeysBalanceFromSkNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> KeysBalanceFromUrlAsync(IntPtr app, string url, string sk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            KeysBalanceFromUrlNative(app, url, sk, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_balance_from_url")]
        private static extern void KeysBalanceFromUrlNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> ValidateSkForUrlAsync(IntPtr app, string sk, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            ValidateSkForUrlNative(app, sk, url, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "validate_sk_for_url")]
        private static extern void ValidateSkForUrlNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<ulong> KeysTransferAsync(IntPtr app, string amount, string fromSk, string toUrl, ulong txId)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            KeysTransferNative(app, amount, fromSk, toUrl, txId, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_transfer")]
        private static extern void KeysTransferNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string amount,
            [MarshalAs(UnmanagedType.LPStr)] string from,
            [MarshalAs(UnmanagedType.LPStr)] string to,
            ulong id,
            IntPtr userData,
            FfiResultULongCb oCb);

        private delegate void FfiResultULongCb(IntPtr userData, IntPtr result, ulong handle);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongCb))]
#endif
        private static void OnFfiResultULongCb(IntPtr userData, IntPtr result, ulong handle)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => handle);
        }

        private static readonly FfiResultULongCb DelegateOnFfiResultULongCb = OnFfiResultULongCb;

        #endregion Keys

        #region Files

        #endregion Files
    }
}
