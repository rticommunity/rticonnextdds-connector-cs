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

    sealed class Input
    {
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

        public Connector Connector {
            get;
            private set;
        }

        public string EntityName {
            get;
            private set;
        }

        public int GetSamplesLength()
        {
            return (int)NativeMethods.RTIDDSConnector_getSamplesLength(
                Connector.Handle,
                EntityName);
        }

        public void Read()
        {
            if (Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_read(Connector.Handle, EntityName);
        }

        public void Take()
        {
            if (Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_take(Connector.Handle, EntityName);
        }

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
