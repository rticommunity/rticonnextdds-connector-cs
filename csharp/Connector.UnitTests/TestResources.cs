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
    using System.IO;
    using System.Reflection;

    public static class TestResources
    {
        public static string ConfigPath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "TestConfig.xml");

        public static string PublisherConfig => "PartLib::PartPub";

        public static string SubscriberConfig => "PartLib::PartSub";

        public static string PubSubConfig => "PartLib::PartPubSub";

        public static string OutputName => "MyPublisher::MySquareWriter";

        public static string InputName => "MySubscriber::MySquareReader";

        public static Connector CreatePublisherConnector()
        {
            return new Connector(PublisherConfig, ConfigPath);
        }

        public static Connector CreateSubscriberConnector()
        {
            return new Connector(SubscriberConfig, ConfigPath);
        }

        public static Connector CreatePubSubConnector()
        {
            return new Connector(PubSubConfig, ConfigPath);
        }
    }
}
