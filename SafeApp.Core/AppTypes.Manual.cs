using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SafeApp.Core
{
#pragma warning disable SA1401 // Fields should be private

    /// <summary>
    /// NrsMap
    /// </summary>
    [PublicAPI]
    public struct NrsMap
    {
        public SubNamesMap SubNamesMap;

        public string Default;

        internal NrsMap(NrsMapNative nrsMapNative)
        {
            SubNamesMap = new SubNamesMap(nrsMapNative.SubNamesMap);
            Default = nrsMapNative.Default;
        }

        /// <summary>
        /// Returns native NrsMap.
        /// </summary>
        /// <returns></returns>
        internal NrsMapNative ToNative()
            => new NrsMapNative
            {
                SubNamesMap = SubNamesMap.ToNative(),
                Default = Default
            };
    }

    /// <summary>
    /// Native NrsMap
    /// </summary>
    internal struct NrsMapNative
    {
        /// <summary>
        /// Native sub name map.
        /// </summary>
        public SubNamesMapNative SubNamesMap;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Default;
    }

    /// <summary>
    /// Some
    /// </summary>
    [PublicAPI]
    public struct SubNamesMap
    {
        /// <summary>
        /// Sub name map entries.
        /// </summary>
        public List<SubNamesMapEntry> SubNames;

        /// <summary>
        /// Initialise a new sub names map object from native sub names map.
        /// </summary>
        /// <param name="native"></param>
        internal SubNamesMap(SubNamesMapNative native)
            => SubNames = BindingUtils.CopyToObjectList<SubNamesMapEntry>(native.SubNamesPtr, (int)native.SubNameLen);

        /// <summary>
        /// Returns native sub names map.
        /// </summary>
        /// <returns></returns>
        internal SubNamesMapNative ToNative()
            => new SubNamesMapNative
            {
                SubNamesPtr = BindingUtils.CopyFromObjectList(SubNames),
                SubNameLen = (UIntPtr)(SubNames?.Count ?? 0),
                SubNameCap = UIntPtr.Zero
            };
    }

    internal struct SubNamesMapNative
    {
        /// <summary>
        /// Sub name entry.
        /// </summary>
        public IntPtr SubNamesPtr;

        /// <summary>
        /// Size of sub name.
        /// </summary>
        public UIntPtr SubNameLen;

        /// <summary>
        /// Capacity of sub name.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr SubNameCap;
    }

    /// <summary>
    /// Some
    /// </summary>
    public struct SubNamesMapEntry
    {
        /// <summary>
        /// The name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string SubName;

        /// <summary>
        /// The rdf name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string SubNameRdf;
    }

    /// <summary>
    /// Some
    /// </summary>
    [PublicAPI]
    public struct XorUrlEncoder
    {
        /// <summary>
        /// Some
        /// </summary>
        public ulong EncodingVersion { get; set; } // currently only v1 supported

        /// <summary>
        /// Some
        /// </summary>
        public byte[] XorName { get; set; }

        /// <summary>
        /// Some
        /// </summary>
        public ulong TypeTag { get; set; }

        /// <summary>
        /// Some
        /// </summary>
        public SafeDataType DataType { get; set; }

        /// <summary>
        /// Some
        /// </summary>
        public SafeContentType ContentType { get; set; }

        /// <summary>
        /// Some
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Some
        /// </summary>
        public string SubNames { get; set; }

        /// <summary>
        /// Some
        /// </summary>
        public ulong ContentVersion { get; set; }
    }

    /// <summary>
    /// Some
    /// </summary>
    [PublicAPI]
    public struct XorNameArray
    {
    }

    /// <summary>
    /// Some
    /// </summary>
    public enum SafeContentType
    {
        /// <summary>
        /// Some
        /// </summary>
        Raw = 0x0000,

        /// <summary>
        /// Some
        /// </summary>
        Wallet = 0x0001,

        /// <summary>
        /// Some
        /// </summary>
        FilesContainer = 0x0002,

        /// <summary>
        /// Some
        /// </summary>
        NrsMapContainer = 0x0003,
    }

    /// <summary>
    /// Some
    /// </summary>
    public enum SafeDataType
    {
        /// <summary>
        /// Some
        /// </summary>
        SafeKey = 0x00,

        /// <summary>
        /// Some
        /// </summary>
        PublishedImmutableData = 0x01,

        /// <summary>
        /// Some
        /// </summary>
        UnpublishedImmutableData = 0x02,

        /// <summary>
        /// Some
        /// </summary>
        SeqMutableData = 0x03,

        /// <summary>
        /// Some
        /// </summary>
        UnseqMutableData = 0x04,

        /// <summary>
        /// Some
        /// </summary>
        PublishedSeqAppendOnlyData = 0x05,

        /// <summary>
        /// Some
        /// </summary>
        PublishedUnseqAppendOnlyData = 0x06,

        /// <summary>
        /// Some
        /// </summary>
        UnpublishedSeqAppendOnlyData = 0x07,

        /// <summary>
        /// Some
        /// </summary>
        UnpublishedUnseqAppendOnlyData = 0x08,
    }

    // -------------------------------------------------------------------------------------------

    /// <summary>
    /// Public and secret BLS key.
    /// </summary>
    public struct BlsKeyPair
    {
        /// <summary>
        /// Public key.
        /// </summary>
        public string PK { get; set; }

        /// <summary>
        /// Secret key.
        /// </summary>
        public string SK { get; set; }
    }

    /// <summary>
    /// Base IPC response message.
    /// </summary>
    [PublicAPI]
    public abstract class IpcMsg
    {
    }

    /// <summary>
    /// Authentication IPC response message.
    /// </summary>
    [PublicAPI]
    public class AuthIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Authentication response.
        /// </summary>
        public AuthGranted AuthGranted;

        /// <summary>
        /// Initialise AuthIPC message.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <param name="authGranted">Authentication response.</param>
        public AuthIpcMsg(uint reqId, AuthGranted authGranted)
        {
            ReqId = reqId;
            AuthGranted = authGranted;
        }
    }

    /// <summary>
    /// Unregistered access IPC response message.
    /// </summary>
    [PublicAPI]
    public class UnregisteredIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Serialised configuration.
        /// </summary>
        public List<byte> SerialisedCfg;

        /// <summary>
        /// Initialise IPC response message.
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="serialisedCfgPtr"></param>
        /// <param name="serialisedCfgLen"></param>
        public UnregisteredIpcMsg(uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen)
        {
            ReqId = reqId;
            SerialisedCfg = BindingUtils.CopyToByteList(serialisedCfgPtr, (int)serialisedCfgLen);
        }
    }

    /// <summary>
    /// Containers permission IPC response message.
    /// </summary>
    [PublicAPI]
    public class ContainersIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialise Containers permission IPC response message.
        /// </summary>
        /// <param name="reqId"></param>
        public ContainersIpcMsg(uint reqId)
        {
            ReqId = reqId;
        }
    }

    /// <summary>
    /// MData share IPC response message.
    /// </summary>
    [PublicAPI]
    public class ShareMDataIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialise ShareMData IPC response message.
        /// </summary>
        /// <param name="reqId"></param>
        public ShareMDataIpcMsg(uint reqId)
        {
            ReqId = reqId;
        }
    }

    /// <summary>
    /// Revoke IPC response message.
    /// </summary>
    [PublicAPI]
    public class RevokedIpcMsg : IpcMsg
    {
    }

    /// <summary>
    /// IPC response message exception
    /// </summary>
    [PublicAPI]
    public class IpcMsgException : FfiException
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public readonly uint ReqId;

        /// <summary>
        /// Initialise IPCMsg exception.
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="code"></param>
        /// <param name="description"></param>
        public IpcMsgException(uint reqId, int code, string description)
            : base(code, description)
        {
            ReqId = reqId;
        }
    }
#pragma warning restore SA1401 // Fields should be private
}
