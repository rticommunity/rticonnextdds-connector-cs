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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class InstanceTests
    {
        Connector connector;
        Output output;
        Instance instance;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreatePublisherConnector();
            output = connector.GetOutput(TestResources.OutputName);
            instance = output.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            connector.Dispose();
        }

        [Test]
        public void SetNonExistingFieldsDoNotThrowException()
        {
            // You will see warning messages on the console instead
            Assert.DoesNotThrow(() => instance.SetValue("fakeInt", 3));
            Assert.DoesNotThrow(() => instance.SetValue("fakeString", "helloworld"));
            Assert.DoesNotThrow(() => instance.SetValue("fakeBool", false));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetWrongVariableTypeForIntegersDoesNotThrowException()
        {
            Assert.DoesNotThrow(() => instance.SetValue("x", "test"));
            Assert.DoesNotThrow(() => instance.SetValue("shapesize", "3"));
            Assert.DoesNotThrow(() => instance.SetValue("angle", "3.14"));
            Assert.DoesNotThrow(() => instance.SetValue("x", true));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetWrongVariableTypeForStringsDoesNotThrowException()
        {
            // You will see warning messages on the console instead
            Assert.DoesNotThrow(() => instance.SetValue("color", 3));
            Assert.DoesNotThrow(() => instance.SetValue("color", true));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetWrongVariableTypeForBoolDoesNotThrowException()
        {
            Assert.DoesNotThrow(() => instance.SetValue("hidden", "test"));
            Assert.DoesNotThrow(() => instance.SetValue("hidden", "true"));
            Assert.DoesNotThrow(() => instance.SetValue("hidden", 0));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetClassObjectWithNonExistingTypesDoesNotThrowException()
        {
            MyFakeFieldsTypes sample = new MyFakeFieldsTypes {
                Color = "test",
                Fake = 3,
            };
            Assert.DoesNotThrow(() => instance.SetValuesFrom(sample));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetClassObjectWithInvalidTypesDoesNotThrowException()
        {
            MyInvalidClassType sample = new MyInvalidClassType {
                Color = 4,
                X = 3.3,
            };
            Assert.DoesNotThrow(() => instance.SetValuesFrom(sample));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetCorrectTypesDoNotThrowException()
        {
            Assert.DoesNotThrow(() => instance.SetValue("x", 3));
            Assert.DoesNotThrow(() => instance.SetValue("angle", 3.14f));
            Assert.DoesNotThrow(() => instance.SetValue("color", "BLUE"));
            Assert.DoesNotThrow(() => instance.SetValue("hidden", false));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetClassObjectWithValidTypesDoesNotThrowException()
        {
            MyClassType sample = new MyClassType {
                Color = "test",
                X = 3,
                Hidden = true,
                Angle = 3.14f
            };
            Assert.DoesNotThrow(() => instance.SetValuesFrom(sample));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetStructObjectWithValidTypesDoesNotThrowException()
        {
            MyStructType sample = new MyStructType {
                Color = "test",
                X = 3,
                Hidden = true,
                Angle = 3.14f
            };
            Assert.DoesNotThrow(() => instance.SetValuesFrom(sample));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetObjectWithAnonymousTypesDoesNotThrowException()
        {
            var sample = new {
                color = "test",
                x = 3,
                hidden = true,
                angle = 3.14
            };
            Assert.DoesNotThrow(() => instance.SetValuesFrom(sample));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetDictionaryWithValidTypesDoesNotThrowException()
        {
            var sample = new Dictionary<string, object> {
                { "color", "test" },
                { "x", 3 },
                { "hidden", true },
                { "angle", 3.14 }
            };

            Assert.DoesNotThrow(() => instance.SetValuesFrom(sample));
            Assert.DoesNotThrow(output.Write);
        }

        [Test]
        public void SetFieldsAfterDisposingConnectorThrowsException()
        {
            MyClassType sample = new MyClassType { Color = "test" };

            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => instance.SetValue("x", 3));
            Assert.Throws<ObjectDisposedException>(() => instance.SetValue("angle", 3.3));
            Assert.Throws<ObjectDisposedException>(() => instance.SetValue("color", "BLUE"));
            Assert.Throws<ObjectDisposedException>(() => instance.SetValue("hidden", false));
            Assert.Throws<ObjectDisposedException>(() => instance.SetValuesFrom(sample));
        }
    }
}
