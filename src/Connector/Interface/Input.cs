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
    /// Internal wrapper for inputs.
    /// </summary>
    sealed class Input
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        /// <param name="connector">The internal connector instance.</param>
        /// <param name="entityName">The input name.</param>
        public Input(Connector connector, string entityName)
        {
            Connector = connector;
            EntityName = entityName;

            // We don't use this internal pointer but if it's null the entity
            // doesn't exist
            IntPtr handle = NativeMethods.RTIDDSConnector_getReader(
                connector.Handle,
                entityName);
            if (handle == IntPtr.Zero) {
                throw new SEHException("Error getting input");
            }
        }

        /// <summary>
        /// Gets the Connector associated to the instance.
        /// </summary>
        /// <value>The Connector instance.</value>
        public Connector Connector {
            get;
            private set;
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <value>The entity name.</value>
        public string EntityName {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of samples received.
        /// </summary>
        /// <returns>The number of samples received.</returns>
        public int GetSamplesLength()
        {
            return (int)NativeMethods.RTIDDSConnector_getSamplesLength(
                Connector.Handle,
                EntityName);
        }

        /// <summary>
        /// Reads samples with this input and do not remove them from the
        /// internal queue.
        /// </summary>
        public void Read()
        {
            if (Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_read(Connector.Handle, EntityName);
        }

        /// <summary>
        /// Reads samples with this input and remove them from the
        /// internal queue.
        /// </summary>
        public void Take()
        {
            if (Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_take(Connector.Handle, EntityName);
        }

        /// <summary>
        /// Interface with the native library.
        /// </summary>
        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_getReader(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_read(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_take(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern double RTIDDSConnector_getSamplesLength(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
        }
    }
}
