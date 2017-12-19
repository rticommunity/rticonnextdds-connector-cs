// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace RTI.Connext.Connector.Interface
{
    using System;
    using System.Runtime.InteropServices;

    sealed class Connector : IDisposable
    {
        public Connector(string configName, string configFile)
        {
            Handle = new ConnectorPtr(configName, configFile);
            if (Handle.IsInvalid) {
                throw new SEHException("Error creating connector");
            }
        }

        ~Connector()
        {
            Dispose(false);
        }

        public ConnectorPtr Handle {
            get;
            private set;
        }

        public bool Disposed {
            get;
            private set;
        }

        public bool Wait(int timeoutMillis)
        {
            ReturnCode retcode = (ReturnCode)NativeMethods.RTIDDSConnector_wait(
                Handle,
                timeoutMillis);
            if (retcode != ReturnCode.Ok && retcode != ReturnCode.Timeout) {
                throw new SEHException("Wait faiulre. Retcode: " + retcode.ToString());
            }

            return retcode != ReturnCode.Timeout;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool freeManagedResources)
        {
            Disposed = true;
            if (freeManagedResources && !Handle.IsInvalid) {
                Handle.Dispose();
            }
        }

        internal sealed class ConnectorPtr : SafeHandle
        {
            public ConnectorPtr(string configName, string configFile)
                : base(IntPtr.Zero, true)
            {
                handle = NativeMethods.RTIDDSConnector_new(
                    configName,
                    configFile,
                    IntPtr.Zero);
            }

            public override bool IsInvalid => handle == IntPtr.Zero;

            protected override bool ReleaseHandle()
            {
                NativeMethods.RTIDDSConnector_delete(handle);
                handle = IntPtr.Zero;
                return true;
            }
        }

        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_new(
                string configName,
                string configFile,
                IntPtr config);

            [DllImport("rtiddsconnector")]
            public static extern void RTIDDSConnector_delete(IntPtr handle);

            [DllImport("rtiddsconnector")]
            public static extern int RTIDDSConnector_wait(
                ConnectorPtr connectorHandle,
                int timeout);
        }
    }
}
