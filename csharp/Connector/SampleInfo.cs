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
    /// <summary>
    /// Information associated to the sample.
    /// </summary>
    public class SampleInfo
    {
        readonly Interface.Sample internalSample;

        internal SampleInfo(Interface.Sample internalSample)
        {
            this.internalSample = internalSample;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Sample"/> contains
        /// data or just metadata information.
        /// </summary>
        /// <value><c>true</c> if contains data; otherwise, <c>false</c>.</value>
        public bool IsValid => internalSample.GetBoolFromInfo("valid_data");
    }
}
