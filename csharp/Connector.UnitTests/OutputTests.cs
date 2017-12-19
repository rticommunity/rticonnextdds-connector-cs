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
    using System.Runtime.InteropServices;
    using NUnit.Framework;

    [TestFixture]
    public class OutputTests
    {
        Connector connector;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreatePublisherConnector();
        }

        [TearDown]
        public void TearDown()
        {
            connector?.Dispose();
        }

        [Test]
        public void SetsProperties()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            Assert.AreEqual(TestResources.OutputName, output.Name);
            Assert.IsNotNull(output.Instance);
            Assert.IsNotNull(output.InternalOutput);
        }

        [Test]
        public void WriteWithDisposedConnectorThrowsException()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(output.Write);
        }

        [Test]
        public void WriteDoesNotThrowException()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void ClearDoesNotThrowExceptionWithoutSettingFields()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            Assert.DoesNotThrow(output.ClearValues);
        }

        [Test]
        public void ClearDoesNotThrowExceptionAfterSettingFields()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            output.Instance.SetValue("x", 3);
            Assert.DoesNotThrow(output.ClearValues);
        }

        [Test]
        public void ClearDoesNotThrowExceptionAfterInvalidAssignment()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            output.Instance.SetValue("fakeInt", 4);
            Assert.DoesNotThrow(output.ClearValues);

            Assert.DoesNotThrow(() => output.Instance.SetValue("x", "test"));
            Assert.DoesNotThrow(output.ClearValues);

            Assert.DoesNotThrow(() => output.Instance.SetValue("color", 3));
            Assert.DoesNotThrow(output.ClearValues);

            Assert.DoesNotThrow(() => output.Instance.SetValue("hidden", "test"));
            Assert.DoesNotThrow(output.ClearValues);
        }

        [Test]
        public void ClearFieldsAfterDisposingConnectorThrowsException()
        {
            Output output = connector.GetOutput(TestResources.OutputName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(output.ClearValues);
        }
    }
}
