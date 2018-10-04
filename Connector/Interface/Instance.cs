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

    sealed class Instance
    {
        readonly Output output;

        public Instance(Output output)
        {
            this.output = output;
        }

        public void SetNumber(string field, double val)
        {
            if (output.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_setNumberIntoSamples(
                output.Connector.Handle,
                output.EntityName,
                field,
                val);
        }

        public void SetBool(string field, bool val)
        {
            if (output.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_setBooleanIntoSamples(
                output.Connector.Handle,
                output.EntityName,
                field,
                val ? 1 : 0);
        }

        public void SetString(string field, string val)
        {
            if (output.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_setStringIntoSamples(
                output.Connector.Handle,
                output.EntityName,
                field,
                val);
        }

        public void SetJson(string json)
        {
            if (output.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            NativeMethods.RTIDDSConnector_setJSONInstance(
                output.Connector.Handle,
                output.EntityName,
                json);
        }

        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setNumberIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                double val);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setBooleanIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                int val);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setStringIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                string val);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setJSONInstance(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string json);
        }
    }
}
