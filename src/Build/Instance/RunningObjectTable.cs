// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Build.Execution
{
#if FEATURE_COM_INTEROP
    /// <summary>
    /// Wrapper for the COM Running Object Table.
    /// </summary>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/windows/desktop/api/objidl/nn-objidl-irunningobjecttable.
    /// </remarks>
    internal class RunningObjectTable : IRunningObjectTableWrapper
    {
        private Task<IRunningObjectTable> _runningObjectTableTask;

        public RunningObjectTable()
        {
            var tcs = new TaskCompletionSource<IRunningObjectTable>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    Ole32.GetRunningObjectTable(0, out var rot);
                    tcs.SetResult(rot);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();

            _runningObjectTableTask = tcs.Task;

        }

        /// <summary>
        /// Attempts to retrieve an item from the ROT.
        /// </summary>
        public async Task<object> GetObject(string itemName)
        {
            IMoniker mk = await CreateMoniker(itemName);
            var rot = await _runningObjectTableTask;
            int hr = rot.GetObject(mk, out object obj);
            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return obj;
        }

        private Task<IMoniker> CreateMoniker(string itemName)
        {
            var tcs = new TaskCompletionSource<IMoniker>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    Ole32.CreateItemMoniker("!", itemName, out IMoniker mk);
                    tcs.SetResult(mk);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();

            return tcs.Task;
        }

        private static class Ole32
        {
            [DllImport(nameof(Ole32))]
            public static extern void CreateItemMoniker(
                [MarshalAs(UnmanagedType.LPWStr)] string lpszDelim,
                [MarshalAs(UnmanagedType.LPWStr)] string lpszItem,
                out IMoniker ppmk);

            [DllImport(nameof(Ole32))]
            public static extern void GetRunningObjectTable(
                int reserved,
                out IRunningObjectTable pprot);
        }
    }
#endif
}
