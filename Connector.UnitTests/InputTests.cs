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
    using NUnit.Framework;

    [TestFixture]
    public class InputTests
    {
        Connector connector;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreateSubscriberConnector();
        }

        [TearDown]
        public void TearDown()
        {
            connector?.Dispose();
        }

        [Test]
        public void SetsProperties()
        {
            Input input = connector.GetInput(TestResources.InputName);
            Assert.AreEqual(TestResources.InputName, input.Name);
            Assert.IsNotNull(input.Samples);
        }

        [Test]
        public void ReadDoesNotThrowException()
        {
            Input input = connector.GetInput(TestResources.InputName);
            Assert.DoesNotThrow(input.Read);
        }

        [Test]
        public void ReadAfterDisposingConnectorThrowsException()
        {
            Input input = connector.GetInput(TestResources.InputName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(input.Read);
        }

        [Test]
        public void TakeDoesNotThrowException()
        {
            Input input = connector.GetInput(TestResources.InputName);
            Assert.DoesNotThrow(input.Take);
        }

        [Test]
        public void TakeAfterDisposingConnectorThrowsException()
        {
            Input input = connector.GetInput(TestResources.InputName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(input.Take);
        }
    }
}
