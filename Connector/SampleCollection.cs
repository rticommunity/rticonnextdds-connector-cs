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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Collection of samples read with a <see cref="Input"/>.
    /// </summary>
    public class SampleCollection : IEnumerable<Sample>
    {
        readonly Input input;

        internal SampleCollection(Input input)
        {
            this.input = input;
        }

        /// <summary>
        /// Gets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        public int Count => input.InternalInput.GetSamplesLength();

        /// <summary>
        /// Gets a <see cref="Sample"/> at the specified index.
        /// </summary>
        /// <param name="index">Sample index.</param>
        /// <returns>
        /// Sample at the specified index.
        /// </returns>
        public Sample this[int index] {
            get {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return new Sample(input, index + 1);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Sample> GetEnumerator()
        {
            return new SampleEnumerator(input, Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Sample collection enumerator for a SampleCollection.
        /// </summary>
        sealed class SampleEnumerator : IEnumerator<Sample>
        {
            readonly Input input;
            readonly int count;
            int index;

            internal SampleEnumerator(Input input, int count)
            {
                this.input = input;
                this.count = count;
            }

            /// <summary>
            /// Gets the current iterating sample.
            /// </summary>
            /// <value>The current sample.</value>
            public Sample Current => new Sample(input, index);

            object IEnumerator.Current => Current;

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// <c>true</c>, if the enumerator was successfully advanced to the
            /// next element; <c>false</c> if the enumerator has passed the end
            /// of the collection.
            /// </returns>
            public bool MoveNext()
            {
                if (index >= count) {
                    return false;
                }

                index++;
                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the
            /// first element in the collection.
            /// </summary>
            public void Reset()
            {
                index = 0;
            }

            /// <summary>
            /// Releases all resource used by the
            /// <see cref="SampleEnumerator"/> object.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}
