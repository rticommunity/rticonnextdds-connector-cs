// (c) 2017 Copyright, Real-Time Innovations, All rights reserved.
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
    /// Sample read with a <see cref="Input"/>.
    /// </summary>
    public class Sample
    {
        readonly Interface.Sample internalSample;

        internal Sample(Input input, int index)
        {
            internalSample = new Interface.Sample(input.InternalInput, index);
            Info = new SampleInfo(internalSample);
            Data = new SampleData(internalSample);
        }

        /// <summary>
        /// Gets the sample information.
        /// </summary>
        /// <value>The info.</value>
        public SampleInfo Info {
            get;
            private set;
         }

        /// <summary>
        /// Gets the sample data.
        /// </summary>
        /// <value>The data.</value>
        public SampleData Data {
            get;
            private set;
        }
    }
}
