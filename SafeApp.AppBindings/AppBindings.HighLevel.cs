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
            string preload,
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
            string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        #endregion Keys
    }
}
