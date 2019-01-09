// (c) 2017 Copyright, Real-Time Innovations, All rights reserved.
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

    /// <summary>
    /// Internal wrapper for Connector.
    /// </summary>
    sealed class Connector : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Connector"/> class.
        /// </summary>
        /// <param name="configName">XML configuration name.</param>
        /// <param name="configFile">XML configuration file path.</param>
        public Connector(string configName, string configFile)
        {
            Handle = new ConnectorPtr(configName, configFile);
            if (Handle.IsInvalid) {
                throw new SEHException("Error creating connector");
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Connector" /> class.
        /// </summary>
        ~Connector()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the internal Connector pointer handle.
        /// </summary>
        /// <value>The internal Connector pointer handle.</value>
        public ConnectorPtr Handle {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Connector"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed {
            get;
            private set;
        }

        /// <summary>
        /// Waits until any <see cref="Input"/> receives at least one sample
        /// or the specific time pass.
        /// </summary>
        /// <param name="timeoutMillis">
        /// Timeout in milliseconds.
        /// Use -1 to wait indefinitely for samples.
        /// </param>
        /// <returns>
        /// <c>true</c> if a sample was received;
        /// otherwise if timeout <c>false</c>.
        /// </returns>
        public bool Wait(int timeoutMillis)
        {
            ReturnCode retcode = (ReturnCode)NativeMethods.RTIDDSConnector_wait(
                Handle,
                timeoutMillis);
            if (retcode != ReturnCode.Ok && retcode != ReturnCode.Timeout) {
                throw new SEHException($"Wait failure. Retcode: {retcode}");
            }

            return retcode != ReturnCode.Timeout;
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Connector"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Connector"/> object.
        /// </summary>
        /// <param name="freeManagedResources">
        /// True to dispose managed resources.
        /// </param>
        void Dispose(bool freeManagedResources)
        {
            Disposed = true;
            if (freeManagedResources && !Handle.IsInvalid) {
                Handle.Dispose();
            }
        }

        /// <summary>
        /// Connector pointer handle.
        /// </summary>
        internal sealed class ConnectorPtr : SafeHandle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConnectorPtr"/> class.
            /// </summary>
            /// <param name="configName">XML configuration name.</param>
            /// <param name="configFile">XML configuration file path.</param>
            public ConnectorPtr(string configName, string configFile)
                : base(IntPtr.Zero, true)
            {
                handle = NativeMethods.RTIDDSConnector_new(
                    configName,
                    configFile,
                    IntPtr.Zero);
            }

            /// <summary>
            /// Gets a value indicating whether the handle value is invalid.
            /// </summary>
            public override bool IsInvalid => handle == IntPtr.Zero;

            /// <summary>
            /// Free the handle.
            /// </summary>
            /// <returns>Always true.</returns>
            protected override bool ReleaseHandle()
            {
                NativeMethods.RTIDDSConnector_delete(handle);
                handle = IntPtr.Zero;
                return true;
            }
        }

        /// <summary>
        /// Interface with the native library.
        /// </summary>
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
