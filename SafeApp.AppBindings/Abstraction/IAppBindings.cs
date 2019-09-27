﻿using System;
using System.Collections.Generic;
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

        Task<(uint, string)> EncodeAuthReqAsync(ref AuthReq req);

        Task<(uint, string)> EncodeContainersReqAsync(ref ContainersReq req);

        Task<(uint, string)> EncodeShareMDataReqAsync(ref ShareMDataReq req);

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
            ref byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding);

        Task<XorUrlEncoder> XorurlEncoderAsync(
            ref byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion);

        Task<XorUrlEncoder> XorurlEncoderFromUrlAsync(string xorUrl);

        #endregion
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
