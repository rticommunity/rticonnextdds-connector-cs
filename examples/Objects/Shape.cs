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
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Shape
    {
        public static int MaxSequenceLength { get; } = 30;

        public static int MaxInnerStructLength { get; } = 3;

        public string Color {
            get;
            set;
        }

        public int X {
            get;
            set;
        }

        public float Angle {
            get;
            set;
        }

        [JsonProperty("aLongSeq")]
        public IList<byte> Sequence {
            get;
            set;
        }

        public ShapeFillKind FillKind {
            get;
            set;
        }

        public InnerStruct[] InnerStruct {
            get;
            set;
        }
    }
}
