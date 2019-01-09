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
    /// Internal wrapper for samples.
    /// </summary>
    sealed class Sample
    {
        readonly Input input;
        readonly int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sample"/> class.
        /// </summary>
        /// <param name="input">The associated input entity.</param>
        /// <param name="index">The sample index.</param>
        public Sample(Input input, int index)
        {
            this.input = input;
            this.index = index;
        }

        /// <summary>
        /// Gets a numeric field from the sample.
        /// </summary>
        /// <param name="field">The field name.</param>
        /// <returns>The field value.</returns>
        public double GetNumberFromSample(string field)
        {
            if (input.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            return NativeMethods.RTIDDSConnector_getNumberFromSamples(
                input.Connector.Handle,
                input.EntityName,
                index,
                field);
        }

        /// <summary>
        /// Gets a boolean field from the sample.
        /// </summary>
        /// <param name="field">The field name.</param>
        /// <returns>The field value.</returns>
        public bool GetBoolFromSample(string field)
        {
            if (input.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            return NativeMethods.RTIDDSConnector_getBooleanFromSamples(
                input.Connector.Handle,
                input.EntityName,
                index,
                field) != 0;
        }

        /// <summary>
        /// Gets a string field from the sample.
        /// </summary>
        /// <param name="field">The field name.</param>
        /// <returns>The field value.</returns>
        public string GetStringFromSample(string field)
        {
            if (input.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            return GetStringAndFreeUnmanaged(
                NativeMethods.RTIDDSConnector_getStringFromSamples(
                    input.Connector.Handle,
                    input.EntityName,
                    index,
                    field));
        }

        /// <summary>
        /// Gets the JSON representation of the sample fields.
        /// </summary>
        /// <returns>The sample in JSON.</returns>
        public string GetJsonFromSample()
        {
            if (input.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            return GetStringAndFreeUnmanaged(
                NativeMethods.RTIDDSConnector_getJSONSample(
                    input.Connector.Handle,
                    input.EntityName,
                    index));
        }

        /// <summary>
        /// Gets a boolean field from the sample information.
        /// </summary>
        /// <param name="field">The field name.</param>
        /// <returns>The field value.</returns>
        public bool GetBoolFromInfo(string field)
        {
            if (input.Connector.Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            return NativeMethods.RTIDDSConnector_getBooleanFromInfos(
                input.Connector.Handle,
                input.EntityName,
                index,
                field) != 0;
        }

        /// <summary>
        /// Converts an unmanaged string into managed and release resources.
        /// </summary>
        /// <param name="strPtr">The unmanaged string pointer.</param>
        /// <returns>The managed string.</returns>
        string GetStringAndFreeUnmanaged(IntPtr strPtr)
        {
            if (strPtr == IntPtr.Zero) {
                throw new SEHException("Error getting the string");
            }

            // Since the C library owns the memory, we convert into string
            // and let the C library free the memory
            string str = Marshal.PtrToStringAnsi(strPtr);
            NativeMethods.RTIDDSConnector_freeString(strPtr);
            return str;
        }

        /// <summary>
        /// Interface with the native library.
        /// </summary>
        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern int RTIDDSConnector_getBooleanFromInfos(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern double RTIDDSConnector_getNumberFromSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern int RTIDDSConnector_getBooleanFromSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_getStringFromSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_getJSONSample(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index);

            [DllImport("rtiddsconnector")]
            public static extern void RTIDDSConnector_freeString(IntPtr strPtr);
        }
    }
}
