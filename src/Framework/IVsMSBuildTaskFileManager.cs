// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Security;


namespace Microsoft.Build.Framework
{
    // TODO wul no checkin mark internal at least
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("33372170-A08F-47F9-B1AE-CD9F2C3BB7C9")]
    public interface IVsMSBuildTaskFileManager
    {
        string GetFileContents([In, MarshalAs(UnmanagedType.LPWStr)] string wszFilename);

        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetFileDocData([In, MarshalAs(UnmanagedType.LPWStr)] string wszFilename);

        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        long GetFileLastChangeTime([In, MarshalAs(UnmanagedType.LPWStr)] string wszFilename);
        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        void PutGeneratedFileContents([In, MarshalAs(UnmanagedType.LPWStr)] string wszFilename, [In, MarshalAs(UnmanagedType.LPWStr)] string strFileContents);

        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool IsRealBuildOperation();

        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        void Delete([In, MarshalAs(UnmanagedType.LPWStr)] string wszFilename);

        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool Exists([In, MarshalAs(UnmanagedType.LPWStr)] string wszFilename, [In, MarshalAs(UnmanagedType.Bool)] bool fOnlyCheckOnDisk);
    }
}
