using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  public partial class MockAuthBindings : IMockAuthBindings {
    #if __IOS__
    internal const string DllName = "__Internal";
    #else
    internal const string DllName = "safe_app";
    #endif

    [DllImport(DllName, EntryPoint = "test_create_app")]
    internal static extern int TestCreateAppNative(out IntPtr oApp);

    [DllImport(DllName, EntryPoint = "test_create_app_with_access")]
    internal static extern int TestCreateAppWithAccessNative([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ContainerPermissions[] accessInfo, IntPtr accessInfoLen, out IntPtr oApp);

  }
}
