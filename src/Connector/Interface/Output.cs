﻿// (c) 2017 Copyright, Real-Time Innovations, All rights reserved.
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
    /// Internal wrapper for outputs.
    /// </summary>
    sealed class Output
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Output"/> class.
        /// </summary>
        /// <param name="connector">The associated connector instance.</param>
        /// <param name="entityName">The entity name.</param>
        public Output(Connector connector, string entityName)
        {
            Connector = connector;
            EntityName = entityName;

            // We don't use this internal pointer but if it's null the entity
            // doesn't exist
            IntPtr handle = NativeMethods.RTIDDSConnector_getWriter(
                    connector.Handle,
                    entityName);
            if (handle == IntPtr.Zero) {
                throw new COMException("Error getting output");
            }
        }

        /// <summary>
        /// Gets the associated connector instance.
        /// </summary>
        /// <value>The connector instance.</value>
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
        /// Clear all the fields from the instance.
        /// </summary>
        public void Clear()
        {
            if (Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_clear(Connector.Handle, EntityName);
        }

        /// <summary>
        /// Writes the instance.
        /// </summary>
        public void Write()
        {
            if (Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_write(Connector.Handle, EntityName, null);
        }

        /// <summary>
        /// Interface with the native library.
        /// </summary>
        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_getWriter(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_clear(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_write(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string paramsJson);
        }
    }
}
