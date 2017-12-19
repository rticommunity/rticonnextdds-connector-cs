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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    struct MyStructType
    {
        public string Color { get; set; }

        public int X { get; set; }

        public bool Hidden { get; set; }

        public float Angle { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class MyClassType
    {
        public string Color { get; set; }

        public int X { get; set; }

        public bool Hidden { get; set; }

        public float Angle { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class MyInvalidClassType
    {
        public int Color { get; set; }

        public double X { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class MyFakeFieldsTypes
    {
        public string Color { get; set; }

        public int X { get; set; }

        public bool Hidden { get; set; }

        public int Fake { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class ComplexType : MyClassType
    {
        public int[] List { get; set; }

        public InnerType Inner { get; set; }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class InnerType
        {
            public int Z { get; set; }
        }
    }
}
