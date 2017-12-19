// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace Mixed
{
    using System;
    using System.Threading;
    using RTI.Connext.Connector;

    static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] != "pub" && args[0] != "sub")) {
                Console.WriteLine("USAGE: Mixed.exe pub|sub [count=0]");
                return;
            }

            string configPath = "Mixed.xml";
            string configName = "MyParticipantLibrary::Zero";
            bool publishMode = args[0] == "pub";
            int count = args.Length > 1 ? Int32.Parse(args[1]) : 0;

            Console.WriteLine("Initializating RTI Connector");
            using (var connector = new Connector(configName, configPath)) {
                if (publishMode)
                    Publish(connector, count);
                else
                    Subscribe(connector, count);

                Console.WriteLine("Finalizing RTI Connector");
            }
        }

        static void Publish(Connector connector, int count)
        {
            string outputName = "MyPublisher::MySquareWriter";
            Output output = connector.GetOutput(outputName);

            Instance instance = output.Instance;
            for (int i = 0; i < count || count == 0; i++) {
                // Clear all the instance members
                output.ClearValues();

                // Sets a number
                instance.SetValue("x", i);

                // Sets a string
                instance.SetValue("color", "BLUE");

                // Sets elements of a sequence.
                // The sequence size is automatically updated (2 or 3).
                instance.SetValue("aOctetSeq[1]", 42);
                instance.SetValue("aOctetSeq[2]", 43);
                if (i % 2 == 0)
                    instance.SetValue("aOctetSeq[3]", 44);

                // Sets elements of complex types
                instance.SetValue("innerStruct[1].x", i);
                instance.SetValue("innerStruct[2].x", i + 1);

                // Finally write the sample and wait some time
                Console.WriteLine("Writing sample {0}", i);
                output.Write();
                Thread.Sleep(2000);
            }
        }

        static void Subscribe(Connector connector, int count)
        {
            string inputName = "MySubscriber::MySquareReader";
            Input inpout = connector.GetInput(inputName);

            for (int i = 0; i < count || count == 0; i++) {
                // Poll for samples every 2 seconds
                Console.WriteLine("Waiting 2 seconds...");
                Thread.Sleep(2000);

                inpout.Take();
                Console.WriteLine("Received {0} samples", inpout.Samples.Count);
                foreach (Sample sample in inpout.Samples) {
                    if (sample.Info.IsValid) {
                        // Gets an integer using generic types
                        int x = sample.Data.GetInt32Value("x");

                        // Gets a string
                        string color = sample.Data.GetStringValue("color");

                        // Gets the size of a sequence
                        int seqLength = sample.Data.GetInt32Value("aOctetSeq#");
                        Console.WriteLine("I received a sequence with {0} elements",
                                          seqLength);
                    } else {
                        Console.WriteLine("Received metadata");
                    }
                }
            }
        }
    }
}
