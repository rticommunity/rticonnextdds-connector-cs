// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace RTI.Connext.Connector
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Sample data.
    /// </summary>
    public class SampleData
    {
        readonly Interface.Sample internalSample;

        internal SampleData(Interface.Sample internalSample)
        {
            this.internalSample = internalSample;
        }

        /// <summary>
        /// Gets the value from an double field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public double GetDoubleValue(string field)
        {
            return internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an float field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public float GetFloatValue(string field)
        {
            return (float)internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an byte field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public byte GetByteValue(string field)
        {
            return (byte)internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an short field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public short GetInt16Value(string field)
        {
            return (short)internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an ushort field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        [CLSCompliant(false)]
        public ushort GetUInt16Value(string field)
        {
            return (ushort)internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an int field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public int GetInt32Value(string field)
        {
            return (int)internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an uint field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        [CLSCompliant(false)]
        public uint GetUInt32Value(string field)
        {
            return (uint)internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an boolean field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public bool GetBoolValue(string field)
        {
            return internalSample.GetBoolFromSample(field);
        }

        /// <summary>
        /// Gets the value from an string field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public string GetStringValue(string field)
        {
            return internalSample.GetStringFromSample(field);
        }

        /// <summary>
        /// Get the sample as a deserialized-JSON object.
        /// </summary>
        /// <returns>The sample.</returns>
        public object GetSampleAsObject()
        {
            return JsonConvert.DeserializeObject(internalSample.GetJsonFromSample());
        }

        /// <summary>
        /// Deserialize the sample from the JSON internal representation.
        /// </summary>
        /// <returns>The sample.</returns>
        /// <typeparam name="T">Type of the sample.</typeparam>
        public T GetSampleAs<T>()
        {
            return JsonConvert.DeserializeObject<T>(internalSample.GetJsonFromSample());
        }
    }
}
