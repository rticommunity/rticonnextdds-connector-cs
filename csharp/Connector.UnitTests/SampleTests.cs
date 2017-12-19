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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using NUnit.Framework;

    // Since we are sending and waiting for samples in the same domain and topic
    // we need to run these test sequentially.
    [TestFixture, SingleThreaded]
    public class SampleTests
    {
        Connector connector;
        Output output;
        Input input;
        SampleCollection samples;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreatePubSubConnector();
            output = connector.GetOutput(TestResources.OutputName);
            input = connector.GetInput(TestResources.InputName);
            samples = input.Samples;

            // Wait for discovery
            Thread.Sleep(100);
        }

        [TearDown]
        public void TearDown()
        {
            connector.Dispose();
        }

        [Test]
        public void GetNumberOfSamplesReturnsValidValue()
        {
            SendAndTakeOrReadStandardSample(true);
            Assert.AreEqual(1, samples.Count);
        }

        [Test]
        public void SampleIteratorContainsOneSample()
        {
            SendAndTakeOrReadStandardSample(true);
            int count = 0;
            foreach (var sample in samples) {
                count++;
            }

            Assert.AreEqual(1, count);
        }

        [Test]
        public void ListContainsJustOneSampleWithNormalEnumerator()
        {
            SendAndTakeOrReadStandardSample(true);
            var list = NonGenericSampleEnumerable(samples);
            Assert.AreEqual(1, list.Cast<Sample>().Count());
        }

#if NET45
        [Test, Timeout(1000)]
        public void SampleIteratorContainTwoSample()
        {
            output.Instance.SetValue("x", 0);
            output.Write();
            output.Instance.SetValue("x", 1);
            output.Write();

            do {
                Thread.Sleep(50);
                input.Read();
            } while (input.Samples.Count < 2);

            int count = 0;
            foreach (var sample in samples) {
                Assert.AreEqual(count, sample.Data.GetInt32Value("x"));
                count++;
            }

            Assert.AreEqual(2, count);
        }

        [Test, Timeout(1000)]
        public void SampleIndexersReturnSamples()
        {
            output.Instance.SetValue("x", 0);
            output.Write();
            output.Instance.SetValue("x", 1);
            output.Write();

            do {
                Thread.Sleep(50);
                input.Read();
            } while (input.Samples.Count < 2);

            Assert.AreEqual(2, input.Samples.Count);
            Assert.AreEqual(0, input.Samples[0].Data.GetInt32Value("x"));
            Assert.AreEqual(1, input.Samples[1].Data.GetInt32Value("x"));
        }
#endif

        [Test]
        public void SampleIndexerThrowExceptionForInvalidIndex()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample;
            Assert.DoesNotThrow(() => sample = input.Samples[0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => sample = input.Samples[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => sample = input.Samples[1]);
        }

        [Test]
        public void RecievedSampleIsValid()
        {
            SendAndTakeOrReadStandardSample(true);
            Assert.IsTrue(samples.Single().Info.IsValid);
        }

        [Test]
        public void ReceivedSampleHasValidNumberFields()
        {
            output.ClearValues();
            output.Instance.SetValue("x", -1453873);
            output.Instance.SetValue("byte", 0x80);
            output.Instance.SetValue("ushort", 0xCAFE);
            output.Instance.SetValue("short", -20812);
            output.Instance.SetValue("uint", 0xCAFEBABE);
            output.Instance.SetValue("angle", -3.14);
            output.Instance.SetValue("double", -3.14);
            output.Write();

            Assert.IsTrue(connector.Wait(1000));
            input.Take();
            Sample sample = samples.Single();

            Assert.AreEqual(-1453873, sample.Data.GetInt32Value("x"));
            Assert.AreEqual(0x80, sample.Data.GetByteValue("byte"));
            Assert.AreEqual(0xCAFE, sample.Data.GetUInt16Value("ushort"));
            Assert.AreEqual(-20812, sample.Data.GetInt16Value("short"));
            Assert.AreEqual(0xCAFEBABE, sample.Data.GetUInt32Value("uint"));
            Assert.That(sample.Data.GetFloatValue("angle"), Is.InRange(-3.140005, -3.140000));
            Assert.That(sample.Data.GetDoubleValue("double"), Is.InRange(-3.140005, -3.140000));
        }

        [Test]
        public void ReceivedSampleHasValidStringField()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.AreEqual("BLUE", sample.Data.GetStringValue("color"));
        }

        [Test]
        public void ReceivedSampleHasValidBoolField()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.AreEqual(true, sample.Data.GetBoolValue("hidden"));
        }

        [Test]
        public void GetNonExistingFieldsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.Data.GetInt32Value("fakeInt"));
            Assert.DoesNotThrow(() => sample.Data.GetStringValue("fakeString"));
            Assert.DoesNotThrow(() => sample.Data.GetBoolValue("fakeBool"));
        }

        [Test]
        public void GetWrongVariableTypeForIntsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.Data.GetInt32Value("color"));
            Assert.DoesNotThrow(() => sample.Data.GetInt32Value("hidden"));
        }

        [Test]
        public void GetWrongVariableTypeForStringsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.Data.GetStringValue("x"));
            Assert.DoesNotThrow(() => sample.Data.GetStringValue("hidden"));
            Assert.DoesNotThrow(() => sample.Data.GetStringValue("angle"));
            Assert.DoesNotThrow(() => sample.Data.GetStringValue("fillKind"));
        }

        [Test]
        public void GetWrongVariableTypeForBoolsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.Data.GetBoolValue("x"));
            Assert.DoesNotThrow(() => sample.Data.GetBoolValue("color"));
            Assert.DoesNotThrow(() => sample.Data.GetBoolValue("angle"));
            Assert.DoesNotThrow(() => sample.Data.GetBoolValue("fillKind"));
        }

        [Test]
        public void TakeRemovesSamples()
        {
            SendAndTakeOrReadStandardSample(true);
            Assert.AreEqual(1, samples.Count);
            input.Take();
            Assert.AreEqual(0, samples.Count);
        }

        [Test]
        public void ReadDoesNotRemoveSamples()
        {
            SendAndTakeOrReadStandardSample(false);
            Assert.AreEqual(1, samples.Count);
            input.Read();
            Assert.AreEqual(1, samples.Count);
            Assert.AreEqual(4, samples.First().Data.GetInt32Value("y"));
        }

        [Test]
        public void TakeAfterReadRemovesSample()
        {
            SendAndTakeOrReadStandardSample(false);
            Assert.AreEqual(1, samples.Count);
            input.Take();
            Assert.AreEqual(1, samples.Count);
            input.Take();
            Assert.AreEqual(0, samples.Count);
        }

        [Test]
        public void GetValidObjectSample()
        {
            MyClassType obj = new MyClassType {
                Color = "test",
                X = 3,
                Hidden = true
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            MyClassType received = sample.Data.GetSampleAs<MyClassType>();

            Assert.AreEqual("test", received.Color);
            Assert.AreEqual(3, received.X);
            Assert.AreEqual(true, received.Hidden);
        }

        [Test]
        public void GetValidStructSample()
        {
            MyStructType obj = new MyStructType {
                Color = "test",
                X = 3,
                Hidden = true
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            MyStructType received = sample.Data.GetSampleAs<MyStructType>();

            Assert.AreEqual("test", received.Color);
            Assert.AreEqual(3, received.X);
            Assert.AreEqual(true, received.Hidden);
        }

        [Test]
        public void SendStructAndReceiveClassSample()
        {
            MyStructType obj = new MyStructType {
                Color = "test",
                X = 3,
                Hidden = true
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            MyClassType received = sample.Data.GetSampleAs<MyClassType>();

            Assert.AreEqual("test", received.Color);
            Assert.AreEqual(3, received.X);
            Assert.AreEqual(true, received.Hidden);
        }

        [Test]
        public void SendAnonymousAndReceiveObjectSample()
        {
            var obj = new {
                color = "test",
                x = 3,
                hidden = true
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            dynamic received = sample.Data.GetSampleAsObject();

            Assert.AreEqual("test", received.color.Value);
            Assert.AreEqual(3, received.x.Value);
            Assert.AreEqual(true, received.hidden.Value);
        }

        [Test]
        public void SendDictionaryAndReceiveSample()
        {
            var obj = new Dictionary<string, object> {
                { "color", "test" },
                { "x", 3 },
                { "hidden", true }
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            var received = sample.Data.GetSampleAs<Dictionary<string, object>>();

            Assert.AreEqual("test", received["color"]);
            Assert.AreEqual(3, received["x"]);
            Assert.AreEqual(true, received["hidden"]);
        }

        [Test]
        public void GetCompleteClassSample()
        {
            ComplexType obj = new ComplexType {
                Color = "test",
                X = 3,
                Angle = 3.14f,
                Hidden = true,
                List = new[] { 0, 1, 2, 3, 4 },
                Inner = new ComplexType.InnerType { Z = -1 }
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            ComplexType received = sample.Data.GetSampleAs<ComplexType>();

            Assert.AreEqual("test", received.Color);
            Assert.AreEqual(3, received.X);
            Assert.AreEqual(true, received.Hidden);
            Assert.AreEqual(3.14f, received.Angle);
            Assert.AreEqual(5, received.List.Length);
            Assert.AreEqual(3, received.List[3]);
            Assert.AreEqual(-1, received.Inner.Z);
        }

        [Test]
        public void GetClassWithInvalidFieldsThrowsException()
        {
            MyInvalidClassType obj = new MyInvalidClassType {
                Color = 3,
                X = 3.3,
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            Assert.Throws<Newtonsoft.Json.JsonSerializationException>(
                () => sample.Data.GetSampleAs<MyInvalidClassType>());
        }

        [Test]
        public void GetClassWithMissingFieldsIsEmpty()
        {
            MyFakeFieldsTypes obj = new MyFakeFieldsTypes {
                Color = "blue",
                X = 3,
                Fake = 4,
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            MyFakeFieldsTypes received = sample.Data.GetSampleAs<MyFakeFieldsTypes>();

            Assert.AreEqual("blue", received.Color);
            Assert.AreEqual(3, received.X);
            Assert.AreEqual(0, received.Fake);
        }

        [Test]
        public void GetNumberSamplesAfterDisposingConnectorThrowsException()
        {
            SendAndTakeOrReadStandardSample(true);
            connector.Dispose();
            int count = 0;
            Assert.Throws<ObjectDisposedException>(() => count = samples.Count);
            Assert.Throws<ObjectDisposedException>(() => count = samples.Count());
            Assert.Throws<ObjectDisposedException>(() => samples.Single());
        }

        [Test]
        public void GetFieldsAfterDisposingConnectorThrowsException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            bool validSample = false;
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => sample.Data.GetInt32Value("x"));
            Assert.Throws<ObjectDisposedException>(() => sample.Data.GetStringValue("color"));
            Assert.Throws<ObjectDisposedException>(() => sample.Data.GetBoolValue("hidden"));
            Assert.Throws<ObjectDisposedException>(() => validSample = sample.Info.IsValid);
        }

        [Test]
        public void GetJsonSampleAfterDisposingConnectorThrowsException()
        {
            MyClassType obj = new MyClassType {
                Color = "test",
                X = 3,
                Hidden = true
            };

            SendAndTakeObjectSample(obj);
            Sample sample = samples.Single();
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => sample.Data.GetSampleAs<MyStructType>());
        }

        [Test]
        public void SetInstanceFieldDoesNotCleanJsonObj()
        {
            output.Instance.SetValue("shapesize", 10);
            MyClassType obj = new MyClassType {
                Color = "test",
                X = 3,
                Hidden = true
            };

            output.Instance.SetValuesFrom(obj);
            output.Instance.SetValue("x", 5);
            output.Instance.SetValue("y", 4);
            output.Write();

            Assert.IsTrue(connector.Wait(1000));
            input.Take();
            Sample sample = samples.Single();

            Assert.AreEqual("test", sample.Data.GetStringValue("color"));
            Assert.AreEqual(5, sample.Data.GetInt32Value("x"));
            Assert.AreEqual(true, sample.Data.GetBoolValue("hidden"));
            Assert.AreEqual(4, sample.Data.GetInt32Value("y"));
            Assert.AreEqual(10, sample.Data.GetInt32Value("shapesize"));
        }

        [Test]
        public void SetJsonObjDoesNotResetInstance()
        {
            output.Instance.SetValue("shapesize", 10);
            output.Instance.SetValue("x", 5);
            output.Instance.SetValue("y", 4);
            MyClassType obj = new MyClassType {
                Color = "test",
                X = 3,
                Hidden = true
            };

            output.Instance.SetValuesFrom(obj);
            output.Write();

            Assert.IsTrue(connector.Wait(1000));
            input.Take();
            Sample sample = samples.Single();

            Assert.AreEqual("test", sample.Data.GetStringValue("color"));
            Assert.AreEqual(3, sample.Data.GetInt32Value("x"));
            Assert.AreEqual(true, sample.Data.GetBoolValue("hidden"));
            Assert.AreEqual(4, sample.Data.GetInt32Value("y"));
            Assert.AreEqual(10, sample.Data.GetInt32Value("shapesize"));
        }

        // This is just for coverage, all IEnumerable<T> implementations
        // have also the non-generic version, we are testing it here:
        // Helper extension method
        static IEnumerable NonGenericSampleEnumerable(IEnumerable samples)
        {
            foreach (object sample in samples) {
                yield return sample;
            }
        }

        void SendAndTakeOrReadStandardSample(bool take)
        {
            output.Instance.SetValue("x", 3);
            output.Instance.SetValue("y", 4);
            output.Instance.SetValue("color", "BLUE");
            output.Instance.SetValue("hidden", true);
            output.Write();

            Assert.IsTrue(connector.Wait(1000));
            if (take) {
                input.Take();
            } else {
                input.Read();
            }
        }

        void SendAndTakeObjectSample(object obj)
        {
            output.Instance.SetValuesFrom(obj);
            output.Write();

            Assert.IsTrue(connector.Wait(1000));
            input.Take();
        }
    }
}
