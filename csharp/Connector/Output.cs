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

    /// <summary>
    /// Connector output.
    /// </summary>
    public class Output
    {
        readonly Interface.Output output;

        /// <summary>
        /// Initializes a new instance of the <see cref="Output"/> class.
        /// </summary>
        /// <param name="connector">Parent connector.</param>
        /// <param name="entityName">Entity name.</param>
        internal Output(Connector connector, string entityName)
        {
            Name = entityName;
            output = new Interface.Output(connector.InternalConnector, entityName);
            Instance = new Instance(this);
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <value>The entity name.</value>
        public string Name {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unique instance associated with this output.
        /// </summary>
        /// <value>The output instance.</value>
        public Instance Instance {
            get;
            private set;
        }

        internal Interface.Output InternalOutput {
            get { return output; }
        }

        /// <summary>
        /// Clear all the members of the associated instance.
        /// </summary>
        public void ClearValues()
        {
            output.Clear();
        }

        /// <summary>
        /// Write the output instance.
        /// </summary>
        public void Write()
        {
            output.Write();
        }
    }
}
