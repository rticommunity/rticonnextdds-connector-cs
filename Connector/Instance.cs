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
    using Newtonsoft.Json;

    /// <summary>
    /// Output instance.
    /// </summary>
    public class Instance
    {
        readonly Output output;
        readonly Interface.Instance instance;

        internal Instance(Output output)
        {
            this.output = output;
            instance = new Interface.Instance(output.InternalOutput);
        }

        /// <summary>
        /// Set the specified number value to a field.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value for the field.</param>
        public void SetValue(string field, double value)
        {
            instance.SetNumber(field, value);
        }

        /// <summary>
        /// Set the specified boolean value to a field.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value for the field.</param>
        public void SetValue(string field, bool value)
        {
            instance.SetBool(field, value);
        }

        /// <summary>
        /// Set the specified string value to a field.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value for the field.</param>
        public void SetValue(string field, string value)
        {
            instance.SetString(field, value);
        }

        /// <summary>
        /// Set instance fields from the object.
        /// </summary>
        /// <param name="obj">Object to serialize as json for the instance.</param>
        public void SetValuesFrom(object obj)
        {
            instance.SetJson(JsonConvert.SerializeObject(obj));
        }
    }
}
