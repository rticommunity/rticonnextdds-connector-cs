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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Connector input.
    /// </summary>
    public class Input
    {
        readonly List<Sample> samples;
        readonly Interface.Input input;

        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        /// <param name="connector">Parent connector.</param>
        /// <param name="entityName">Entity name.</param>
        internal Input(Connector connector, string entityName)
        {
            Name = entityName;
            input = new Interface.Input(
                connector.InternalConnector,
                entityName);

            samples = new List<Sample>();
            Samples = new ReadOnlyCollection<Sample>(samples);
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get;
            private set;
        }

        /// <summary>
        /// Gets the samples read or taken from this input.
        /// </summary>
        /// <value>The samples read or taken.</value>
        public ReadOnlyCollection<Sample> Samples {
            get;
            private set;
        }

        /// <summary>
        /// Gets the internal wrapper instance.
        /// </summary>
        /// <value>The internal wrapper instance.</value>
        internal Interface.Input InternalInput {
            get { return input; }
        }

        /// <summary>
        /// Reads samples with this input and do not remove them from the
        /// internal queue.
        /// </summary>
        /// <remarks>
        /// The samples are accessible from the <see cref="Samples"/> property.
        /// </remarks>
        public void Read()
        {
            input.Read();
            UpdateSampleList();
        }

        /// <summary>
        /// Reads samples with this input and remove them from the
        /// internal queue.
        /// </summary>
        /// <remarks>
        /// The samples are accesible from the <see cref="Samples"/> property.
        /// </remarks>
        public void Take()
        {
            input.Take();
            UpdateSampleList();
        }

        /// <summary>
        /// Clear and update the list of samples.
        /// </summary>
        void UpdateSampleList()
        {
            samples.Clear();

            int length = InternalInput.GetSamplesLength();
            for (int i = 0; i < length; i++) {
                samples.Add(new Sample(this, i + 1));
            }
        }
    }
}
