using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.AppBindings
{
    internal partial class AppBindings
    {
        #region Connect

        public void SafeConnect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb)
        {
            var userData = BindingUtils.ToHandlePtr(oCb);
            SafeConnectNative(appId, authCredentials, userData, DelegateOnFfiResultSafeCb);
        }

        [DllImport(DllName, EntryPoint = "connect")]
        private static extern void SafeConnectNative(
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

        #endregion Connect

        #region Keys

        public Task<BlsKeyPair> GenerateKeyPairAsync(ref IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<BlsKeyPair>();
            GenerateKeyPairNative(ref app, userData, DelegateOnFfiResultBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "generate_keypair")]
        private static extern void GenerateKeyPairNative(
            ref IntPtr app,
            IntPtr userData,
            FfiResultBlsKeyPairCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

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

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<(string, BlsKeyPair?)> CreateKeysAsync(
            ref IntPtr app,
            string from,
            string preloadAmount,
            string pk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair?)>();
            CreateKeysNative(ref app,  from, preloadAmount, pk, userData, DelegateOnFfiResultStringNullableBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_create")]
        private static extern void CreateKeysNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string from,
            [MarshalAs(UnmanagedType.LPStr)] string preload,
            [MarshalAs(UnmanagedType.LPStr)] string pk,
            IntPtr userData,
            FfiResultStringNullableBlsKeyPairCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        private delegate void FfiResultStringNullableBlsKeyPairCb(
            IntPtr userData,
            IntPtr result,
            string xorUrl,
            IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringNullableBlsKeyPairCb))]
#endif
        private static void OnFfiResultStringNullableBlsKeyPairCb(
            IntPtr userData,
            IntPtr result,
            string xorUrl,
            IntPtr safeKey)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (xorUrl, Marshal.PtrToStructure<BlsKeyPair?>(safeKey)));

        private static readonly FfiResultStringNullableBlsKeyPairCb DelegateOnFfiResultStringNullableBlsKeyPairCb = OnFfiResultStringNullableBlsKeyPairCb;

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(ref IntPtr app, string preloadAmount)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair)>();
            KeysCreatePreloadTestCoinsNative(ref app, preloadAmount, userData, DelegateOnFfiResultStringBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_create_preload_test_coins")]
        private static extern void KeysCreatePreloadTestCoinsNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string preload,
            IntPtr userData,
            FfiResultStringBlsKeyPairCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

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

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<string> KeysBalanceFromSkAsync(ref IntPtr app, string sk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            KeysBalanceFromSkNative(ref app, sk, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_balance_from_sk")]
        private static extern void KeysBalanceFromSkNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<string> KeysBalanceFromUrlAsync(ref IntPtr app, string url, string sk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            KeysBalanceFromUrlNative(ref app, url, sk, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_balance_from_url")]
        private static extern void KeysBalanceFromUrlNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<string> ValidateSkForUrlAsync(ref IntPtr app, string sk, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            ValidateSkForUrlNative(ref app, sk, url, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "validate_sk_for_url")]
        private static extern void ValidateSkForUrlNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultStringCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<ulong> KeysTransferAsync(ref IntPtr app, string amount, string fromSk, string toUrl, ulong txId)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            KeysTransferNative(ref app, amount, fromSk, toUrl, txId, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_transfer")]
        private static extern void KeysTransferNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string amount,
            [MarshalAs(UnmanagedType.LPStr)] string from,
            [MarshalAs(UnmanagedType.LPStr)] string to,
            ulong id,
            IntPtr userData,
            FfiResultULongCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        #endregion Keys

        #region NRS

        public Task<XorUrlEncoder> ParseUrlAsync(ref IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            ParseUrlNative(ref app, url, userData, DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "parse_url")]
        private static extern void ParseUrlNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        private delegate void FfiResultXorUrlEncoderCb(
            IntPtr userData,
            IntPtr result,
            IntPtr xorUrlEncoder);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultXorUrlEncoderCb))]
#endif
        private static void OnFfiResultXorUrlEncoderCb(
            IntPtr userData,
            IntPtr result,
            IntPtr xorUrlEncoder)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<XorUrlEncoder>(xorUrlEncoder));

        private static readonly FfiResultXorUrlEncoderCb DelegateOnFfiResultXorUrlEncoderCb = OnFfiResultXorUrlEncoderCb;

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<(XorUrlEncoder, bool)> ParseAndResolveUrlAsync(ref IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(XorUrlEncoder, bool)>();
            ParseAndResolveUrlNative(ref app, url, userData, DelegateOnFfiResultXorUrlEncoderBoolCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "parse_and_resolve_url")]
        private static extern void ParseAndResolveUrlNative(
            ref IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultXorUrlEncoderBoolCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        private delegate void FfiResultXorUrlEncoderBoolCb(
            IntPtr userData,
            IntPtr result,
            IntPtr xorUrlEncoder,
            bool resolvesAsNrs);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultXorUrlEncoderBoolCb))]
#endif
        private static void OnFfiResultXorUrlEncoderBoolCb(
            IntPtr userData,
            IntPtr result,
            IntPtr xorUrlEncoder,
            bool resolvesAsNrs)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (Marshal.PtrToStructure<XorUrlEncoder>(xorUrlEncoder), resolvesAsNrs));

        private static readonly FfiResultXorUrlEncoderBoolCb DelegateOnFfiResultXorUrlEncoderBoolCb = OnFfiResultXorUrlEncoderBoolCb;

        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        #endregion NRS
    }
}
