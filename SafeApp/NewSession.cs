using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.API;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp
{
    public class NewSession
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;

        private SafeAppPtr _appPtr;

        public bool IsDisconnected { get; private set; }

        public Keys Keys { get; private set; }

        public Nrs Nrs { get; private set; }

        private NewSession()
        {
            IsDisconnected = true;
            _appPtr = new SafeAppPtr();
        }

        public static Task<NewSession> AppConnectAsync(string appId, string authCredentials)
        {
            return Task.Run(
                () =>
                {
                    var tcs = new TaskCompletionSource<NewSession>(TaskCreationOptions.RunContinuationsAsynchronously);
                    var session = new NewSession();
                    Action<FfiResult, IntPtr, GCHandle> acctConnectedCb = (result, ptr, disconnectedHandle) =>
                    {
                        if (result.ErrorCode != 0)
                        {
                            tcs.SetException(result.ToException());
                            return;
                        }

                        session.Init(ptr, disconnectedHandle);
                        tcs.SetResult(session);
                    };

                    AppBindings.SafeConnect(appId, authCredentials, acctConnectedCb);
                    return tcs.Task;
                });
        }

        private void Init(IntPtr appPtr, GCHandle disconnectedHandle)
        {
            IsDisconnected = false;
            _appPtr = new SafeAppPtr(appPtr);
            Keys = new Keys(_appPtr);
            Nrs = new Nrs(_appPtr);
        }
    }
}
