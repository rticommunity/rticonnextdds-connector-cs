// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace Simple
{
    using System;
    using System.Threading;
    using RTI.Connext.Connector;

    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] != "pub" && args[0] != "sub")) {
                Console.WriteLine("USAGE: Simple.exe pub|sub [count=0]");
                return;
            }

            string configPath = "ShapeExample.xml";
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
                Console.WriteLine("Writing sample {0}", i);

                // Optionally, clear the instance field from previous iterations
                output.ClearValues();
                instance.SetValue("x", i);
                instance.SetValue("y", i * 2);
                instance.SetValue("y", 3.14);
                instance.SetValue("shapesize", 30);
                instance.SetValue("color", "BLUE");

                output.Write();
                Thread.Sleep(100);
            }
        }

        static void Subscribe(Connector connector, int count)
        {
            string inputName = "MySubscriber::MySquareReader";
            Input input = connector.GetInput(inputName);

            for (int i = 0; i < count || count == 0; i++) {
                // Poll for samples every second
                Console.WriteLine("Waiting 1 second...");
                Thread.Sleep(1000);

                // Take samples. Accesible from Input.Samples
                input.Take();
                Console.WriteLine("Received {0} samples", input.Samples.Count);
                foreach (Sample sample in input.Samples) {
                    if (sample.Info.IsValid) {
                        Console.WriteLine(
                            "Received [x={0}, y={1}, size={2}, color={3}]",
                            sample.Data.GetInt32Value("x"),
                            sample.Data.GetInt32Value("y"),
                            sample.Data.GetInt32Value("shapesize"),
                            sample.Data.GetStringValue("color"));
                    } else {
                        Console.WriteLine("Received metadata");
                    }
                }
            }
        }
    }
}
