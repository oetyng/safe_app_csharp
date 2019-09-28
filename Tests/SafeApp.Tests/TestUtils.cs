using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests
{
    public static class TestUtils
    {
        public static readonly Random Random = new Random();

        private static async Task<string> AuthenticateAuthRequest(Authenticator authenticator, string ipcMsg, bool allow)
        {
            var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
            Assert.That(ipcReq, Is.TypeOf<AuthIpcReq>());
            var response = await authenticator.EncodeAuthRespAsync(ipcReq as AuthIpcReq, allow);
            authenticator.Dispose();
            return response;
        }

        public static async Task<string> AuthenticateAuthRequest(string ipcMsg, bool allow)
        {
            var authenticator = await Authenticator.CreateAccountAsync(GetRandomString(10), GetRandomString(10));
            return await AuthenticateAuthRequest(authenticator, ipcMsg, allow);
        }

        public static async Task<string> AuthenticateAuthRequest(string locator, string secret, string ipcMsg, bool allow)
        {
            var authenticator = await Authenticator.LoginAsync(locator, secret);
            return await AuthenticateAuthRequest(authenticator, ipcMsg, allow);
        }

        public static async Task<string> AuthenticateUnregisteredRequest(string ipcMsg)
        {
            var ipcReq = await Authenticator.UnRegisteredDecodeIpcMsgAsync(ipcMsg);
            Assert.That(ipcReq, Is.TypeOf<UnregisteredIpcReq>());
            var response = await Authenticator.EncodeUnregisteredRespAsync(((UnregisteredIpcReq)ipcReq).ReqId, true);
            return response;
        }

        public static Task<Session> CreateTestApp()
        {
            var locator = GetRandomString(10);
            var secret = GetRandomString(10);
            var authReq = new AuthReq
            {
                App = new AppExchangeInfo
                {
                    Id = GetRandomString(10),
                    Name = GetRandomString(5),
                    Scope = null,
                    Vendor = GetRandomString(5)
                },
                AppContainer = true,
                Containers = new List<ContainerPermissions>(),
                AppPermissionTransferCoins = true
            };
            return CreateTestApp(locator, secret, authReq);
        }

        public static Task<Session> CreateTestApp(AuthReq authReq)
        {
            var locator = GetRandomString(10);
            var secret = GetRandomString(10);
            return CreateTestApp(locator, secret, authReq);
        }

        public static async Task<Session> CreateTestApp(string locator, string secret, AuthReq authReq)
        {
            var authenticator = await Authenticator.CreateAccountAsync(locator, secret);
            var (_, reqMsg) = await Session.EncodeAuthReqAsync(authReq);
            var ipcReq = await authenticator.DecodeIpcMessageAsync(reqMsg);
            Assert.That(ipcReq, Is.TypeOf<AuthIpcReq>());
            var authIpcReq = ipcReq as AuthIpcReq;
            var resMsg = await authenticator.EncodeAuthRespAsync(authIpcReq, true);
            return await Session.AppConnectAsync(authReq.App.Id, resMsg);
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
