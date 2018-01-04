#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SafeApp.Utilities {
    public class FfiException : Exception {
        public readonly int ErrorCode;

        public FfiException(int code, String description)
            : base($"Error Code: {code}. Description: {description}")
        {
            ErrorCode = code;
        }
    }

    public struct FfiResult {
        public int ErrorCode;
        [MarshalAs(UnmanagedType.LPStr)]
        public String Description;

        public FfiException ToException() {
            return new FfiException(ErrorCode, Description);
        }
    }

    internal class BindingUtils {
        public static IntPtr ToHandlePtr<T>(T obj) {
            return GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }

        public static T FromHandlePtr<T>(IntPtr ptr, bool free = true) {
            var handle = GCHandle.FromIntPtr(ptr);
            var result = (T) handle.Target;

            if (free) handle.Free();

            return result;
        }

        public static (Task<T>, IntPtr) PrepareTask<T>() {
            var tcs = new TaskCompletionSource<T>();
            var userData = ToHandlePtr(tcs);

            return (tcs.Task, userData);
        }

        public static (Task, IntPtr) PrepareTask() {
            return PrepareTask<bool>();
        }

        public static void CompleteTask<T>(TaskCompletionSource<T> tcs, ref FfiResult result, T arg) {
            if (result.ErrorCode != 0) {
                tcs.SetException(result.ToException());
            } else {
                tcs.SetResult(arg);
            }
        }

        public static void CompleteTask<T>(IntPtr userData, ref FfiResult result, T arg) {
            var tcs = FromHandlePtr<TaskCompletionSource<T>>(userData);
            CompleteTask(tcs, ref result, arg);
        }

        public static void CompleteTask(IntPtr userData, ref FfiResult result) {
            CompleteTask(userData, ref result, true);
        }

        public static List<byte> CopyToByteList(IntPtr ptr, IntPtr len) {
            var array = new byte[(int) len];
            Marshal.Copy(ptr, array, 0, (int) len);

            return new List<byte>(array);
        }

        public static List<T> CopyToObjectList<T>(IntPtr ptr, IntPtr len) {
            var array = new T[(int) len];
            for (int i = 0; i < (int) len; ++i) {
                array[i] = Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Marshal.SizeOf<T>() * i));
            }

            return new List<T>(array);
        }

        public static IntPtr CopyFromByteList(List<byte> list) {
            var array = list.ToArray();
            var size = Marshal.SizeOf(array[0]) * array.Length;
            var ptr  = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, array.Length);

            return ptr;
        }

        public static IntPtr CopyFromObjectList<T>(List<T> list) {
            var array = list.ToArray();
            var size = Marshal.SizeOf(array[0]) * array.Length;
            var ptr  = Marshal.AllocHGlobal(size);
            for (int i = 0; i < array.Length; ++i) {
                Marshal.StructureToPtr(array[i], IntPtr.Add(ptr, Marshal.SizeOf<T>() * i), false);
            }

            return ptr;
        }

        public static void FreeList(ref IntPtr ptr, ref IntPtr len) {
            if (ptr != IntPtr.Zero) {
                Marshal.FreeHGlobal(ptr);
            }

            ptr = IntPtr.Zero;
            len = IntPtr.Zero;
        }

    }
}
