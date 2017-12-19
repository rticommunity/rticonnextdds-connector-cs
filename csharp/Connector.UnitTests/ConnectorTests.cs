// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace RTI.Connext.Connector.UnitTests
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using NUnit.Framework;

    [TestFixture, SingleThreaded]
    public class ConnectorTests
    {
        [Test]
        public void ConstructorWithNullOrEmptyConfigNameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Connector(null, TestResources.ConfigPath));
            Assert.Throws<ArgumentNullException>(
                () => new Connector(string.Empty, TestResources.ConfigPath));
        }

        [Test]
        public void ConstructorWithNullOrEmptyConfigFileThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Connector(TestResources.PublisherConfig, null));
            Assert.Throws<ArgumentNullException>(
                () => new Connector(TestResources.PublisherConfig, string.Empty));
        }

        [Test]
        public void ConstructorWithMissingConfigFileThrowsException()
        {
            Assert.Throws<SEHException>(
                () => new Connector(TestResources.PublisherConfig, "FakeConfig.xml"));
        }

        [Test]
        public void ConstructorWithInvalidConfigNameThrowsException()
        {
            Assert.Throws<SEHException>(
                () => new Connector("InvalidLib::PartPub", TestResources.ConfigPath));
            Assert.Throws<SEHException>(
                () => new Connector("PartLib::InvalidPart", TestResources.ConfigPath));
        }

        [Test]
        public void ConstructorWithValidConfigIsSuccessful()
        {
            Connector connector = null;
            Assert.DoesNotThrow(
                () => connector = new Connector(
                    TestResources.PublisherConfig,
                    TestResources.ConfigPath));
            Assert.IsNotNull(connector.InternalConnector);
            connector.Dispose();
        }

        [Test]
        public void CreatingAndDeletingConnectorIsSuccessful()
        {
            int tries = 5;
            for (int i = 0; i < tries; i++) {
                ConstructorWithValidConfigIsSuccessful();
            }
        }

        [Test]
        public void ConstructorSetsProperties()
        {
            using (var connector = TestResources.CreatePublisherConnector()) {
                Assert.AreEqual(TestResources.PublisherConfig, connector.ConfigName);
                Assert.AreEqual(TestResources.ConfigPath, connector.ConfigFile);
            }
        }

        [Test]
        public void DisposeChangesProperty()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            Assert.IsFalse(connector.Disposed);
            connector.Dispose();
            Assert.IsTrue(connector.Disposed);
        }

        [Test]
        public void DisposingTwiceDoesNotThrowException()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            connector.Dispose();
            Assert.DoesNotThrow(connector.Dispose);
            Assert.IsTrue(connector.Disposed);
        }

        [Test]
        public void GetOutputWithNullOrEmptyEntityNameThrowsException()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            Assert.Throws<ArgumentNullException>(
                () => connector.GetOutput(null));
            Assert.Throws<ArgumentNullException>(
                () => connector.GetOutput(string.Empty));
        }

        [Test]
        public void GetOutputAfterDisposeThrowsException()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(
                () => connector.GetOutput(TestResources.OutputName));
        }

        [Test]
        public void GetOutputWithMissingEntityNameThrowsException()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            Assert.Throws<COMException>(
                () => connector.GetOutput("FakePublisher::MySquareWriter"));
            Assert.Throws<COMException>(
                () => connector.GetOutput("MyPublisher::FakeWriter"));
        }

        [Test]
        public void GetOutputWithValidConfigIsSuccessful()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            Output output = null;
            Assert.DoesNotThrow(
                () => output = connector.GetOutput(TestResources.OutputName));
            Assert.IsNotNull(output.InternalOutput);
        }

        [Test]
        public void GetInputWithNullOrEmptyEntityNameThrowsException()
        {
            Connector connector = TestResources.CreateSubscriberConnector();
            Assert.Throws<ArgumentNullException>(
                () => connector.GetInput(null));
            Assert.Throws<ArgumentNullException>(
                () => connector.GetInput(string.Empty));
        }

        [Test]
        public void GetInputWithMissingEntityNameThrowsException()
        {
            Connector connector = TestResources.CreateSubscriberConnector();
            Assert.Throws<SEHException>(
                () => connector.GetInput("FakeSubscriber::MySquareReader"));
            Assert.Throws<SEHException>(
                () => connector.GetInput("MySubscriber::FakeReader"));
        }

        [Test]
        public void GetInputAfterDisposeThrowsException()
        {
            Connector connector = TestResources.CreateSubscriberConnector();
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(
                () => connector.GetInput(TestResources.InputName));
        }

        [Test]
        public void GetInputWithValidConfigIsSuccessful()
        {
            Connector connector = TestResources.CreateSubscriberConnector();
            Input input = null;
            Assert.DoesNotThrow(
                () => input = connector.GetInput(TestResources.InputName));
            Assert.IsNotNull(input.InternalInput);
        }

        [Test]
        public void WaitForSamplesWithNegativeTimeOutThrowsException()
        {
            using (var connector = TestResources.CreateSubscriberConnector()) {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => connector.Wait(-10));
            }
        }

        // These tests may block indefinitely so we need the timeout but it's
        // not available in the NUnit .NET Core version:
        // https://github.com/nunit/nunit/issues/1638
#if NET45
        [Test, Timeout(1000)]
        public void WaitForSamplesInMultipleThreadsThrowsException()
        {
            using (var connector = TestResources.CreateSubscriberConnector()) {
                System.Threading.Tasks.Task.Run(
                    () => Assert.IsFalse(connector.Wait(300)));
                System.Threading.Thread.Sleep(100);
                Assert.Throws<SEHException>(() => connector.Wait(100));
            }
        }

        [Test, Timeout(1000)]
        public void WaitForSamplesWithZeroTimeOutDoesNotBlock()
        {
            using (var connector = TestResources.CreateSubscriberConnector()) {
                Stopwatch watch = Stopwatch.StartNew();
                Assert.IsFalse(connector.Wait(0));
                watch.Stop();
                Assert.Less(watch.ElapsedMilliseconds, 10);
            }
        }

        [Test, Timeout(1000)]
        public void WaitForSamplesCanDoMsTimeOut()
        {
            using (var connector = TestResources.CreateSubscriberConnector()) {
                Stopwatch watch = Stopwatch.StartNew();
                Assert.IsFalse(connector.Wait(100));
                watch.Stop();
                Assert.Less(watch.ElapsedMilliseconds, 110);
                Assert.Greater(watch.ElapsedMilliseconds, 90);
            }
        }

        [Test, Timeout(5000)]
        public void WaitForSamplesCanDoOneSecTimeOut()
        {
            using (var connector = TestResources.CreateSubscriberConnector()) {
                Stopwatch watch = Stopwatch.StartNew();
                Assert.IsFalse(connector.Wait(1000));
                watch.Stop();
                Assert.Less(watch.ElapsedMilliseconds, 1010);
                Assert.Greater(watch.ElapsedMilliseconds, 990);
            }
        }
#endif

        [Test]
        public void WaitForSamplesAfterDisposeThrowsException()
        {
            Connector connector = TestResources.CreateSubscriberConnector();
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => connector.Wait(0));
        }
    }
}
