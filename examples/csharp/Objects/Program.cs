// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using RTI.Connext.Connector;

    static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] != "pub" && args[0] != "sub")) {
                Console.WriteLine("USAGE: Objects.exe pub|sub [count=0]");
                return;
            }

            string configPath = "Configuration.xml";
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

            for (int i = 0; i < count || count == 0; i++) {
                // Create the class to send
                Shape shape = new Shape {
                    X = i,
                    Color = "BLUE",
                    Angle = 3.14f,
                    FillKind = ShapeFillKind.HorizontalHatch
                };

                // Sets elements of a sequence.
                // The sequence size is automatically updated (2 or 3).
                shape.Sequence = new List<byte>();;
                shape.Sequence.Add(42);
                shape.Sequence.Add(43);
                if (i % 2 == 0)
                    shape.Sequence.Add(44);

                // Sets elements of complex types
                shape.InnerStruct = new InnerStruct[3];
                shape.InnerStruct[1].X = i;
                shape.InnerStruct[2].X = i + 1;

                // Finally write the sample and wait some time
                Console.WriteLine("Writing sample {0}", i);

                output.ClearValues();
                output.Instance.SetValuesFrom(shape);
                output.Write();

                Thread.Sleep(2000);
            }
        }

        static void Subscribe(Connector connector, int count)
        {
            string inputName = "MySubscriber::MySquareReader";
            Input input = connector.GetInput(inputName);

            for (int i = 0; i < count || count == 0; i++) {
                // Wait for upto 2 seconds for samples.
                if (!connector.Wait(3000))
                    continue;

                input.Take();
                var sampleList = input.Samples
                    .Where(s => s.Info.IsValid)
                    .Select(s => s.Data.GetSampleAs<Shape>());
                foreach (Shape sample in sampleList) {
                    Console.Write($"[x:{sample.X}");
                    Console.Write($",color:{sample.Color}");
                    Console.Write($",angle:{sample.Angle}");
                    Console.Write($",fillKind:{sample.FillKind}");
                    for (int j = 0; j < sample.Sequence.Count; j++)
                        Console.Write($",seq[{j}]:{sample.Sequence[j]}");
                    for (int j = 0; j < sample.InnerStruct.Length; j++)
                        Console.Write($",inner[{j}]:{sample.InnerStruct[j].X}");
                    Console.WriteLine("]");
                }
            }
        }
    }
}
