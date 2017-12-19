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
    /// RTI Connext DDS Connector.
    /// </summary>
    public class Connector : IDisposable
    {
        readonly Interface.Connector internalConnector;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connector"/> class.
        /// </summary>
        /// <param name="configName">XML configuration name.</param>
        /// <param name="configFile">XML configuration file path.</param>
        public Connector(string configName, string configFile)
        {
            if (string.IsNullOrEmpty(configName)) {
                throw new ArgumentNullException(nameof(configName));
            }

            if (string.IsNullOrEmpty(configFile)) {
                throw new ArgumentNullException(nameof(configFile));
            }

            ConfigName = configName;
            ConfigFile = configFile;

            internalConnector = new Interface.Connector(configName, configFile);
        }

        ~Connector()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigName {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the configuration file.
        /// </summary>
        /// <value>The configuration file path.</value>
        public string ConfigFile {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Connector"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed {
            get;
            private set;
        }

        internal Interface.Connector InternalConnector {
            get { return internalConnector; }
        }

        /// <summary>
        /// Gets a Connector <see cref="Output"/>.
        /// </summary>
        /// <returns>The output.</returns>
        /// <param name="name">Name of the output.</param>
        public Output GetOutput(string name)
        {
            if (Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            return new Output(this, name);
        }

        /// <summary>
        /// Gets a Connector <see cref="Input"/>.
        /// </summary>
        /// <returns>The input.</returns>
        /// <param name="name">Name of the input.</param>
        public Input GetInput(string name)
        {
            if (Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            return new Input(this, name);
        }

        /// <summary>
        /// Waits until any <see cref="Input"/> receives at least one sample
        /// or the specific time pass.
        /// </summary>
        /// <param name="timeoutMillis">
        /// Timeout in milliseconds.
        /// Use -1 to wait indefinitely for samples.
        /// </param>
        /// <returns>
        /// <c>true</c> if a sample was received;
        /// otherwise if timeout <c>false</c>.
        /// </returns>
        public bool Wait(int timeoutMillis)
        {
            if (Disposed) {
                throw new ObjectDisposedException(nameof(Connector));
            }

            if (timeoutMillis < -1) {
                throw new ArgumentOutOfRangeException(nameof(timeoutMillis));
            }

            return InternalConnector.Wait(timeoutMillis);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Connector"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool freeManagedResources)
        {
            if (Disposed) {
                return;
            }

            Disposed = true;
            if (freeManagedResources) {
                internalConnector.Dispose();
            }
        }
    }
}
