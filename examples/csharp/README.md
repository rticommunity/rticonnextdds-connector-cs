# RTI Connext DDS Connector: C#/.NET

## Installation and Platform support
Check [here](https://github.com/rticommunity/rticonnextdds-connector#getting-started-with-net) and [here](https://github.com/rticommunity/rticonnextdds-connector#platform-support).
If you still have trouble write on the [RTI Community Forum](https://community.rti.com/forums/technical-questions)

### Available examples
In this directory you can find 1 set of examples

 * **Simple**: shows how to write samples and how to read/take.
 * **Mixed**: shows how to write and read samples from complex types like sequences and inner structures.
 * **Objects**: shows how to write and read samples mapped from C# objects like *classes* and *structs*.

### API Overview:
#### Import the RTI Connector library
Import the `librtiddsconnector_dotnet.dll` library to start using *Connector*. The API is under the namespace `RTI.Connext.Connector`.

#### Instantiate a new Connector
To create a new *Connector* you have to pass the path to an XML file and a configuration name. For more information on
the XML format check the [XML App Creation guide](https://community.rti.com/static/documentation/connext-dds/5.2.3/doc/manuals/connext_dds/xml_application_creation/RTI_ConnextDDS_CoreLibraries_XML_AppCreation_GettingStarted.pdf) or
have a look to the [ShapeExample.xml](ShapeExample.xml) file included in this directory.

```csharp
Connector = connector = new Connector("MyParticipantLibrary::Zero", "ShapeExample.xml");
```

#### Delete a Connector
To destroy all the DDS entities that belong to a *Connector* previously created, you can call the `Dispose` method. Another option is to use the `using` statement. This is a safer approach since in case of exception it guarantees that the object will be disposed.

```csharp
using (Connector connector = new Connector(configName, configPath)) {
    // Do stuff with Connector

    // Before going out of the scope, connector.Dispose() is automatically called
}
```

#### Write a sample
To write a sample first we need to create a *Output* by getting it from the *Connector* object. The output name is defined in the XML configuration.

```csharp
Output output = connector.GetOutput("MyPublisher::MySquareWriter");
```

Then we can start using the *Instance* associated to this *Output* and set its fields.

```csharp
Instance instance = output.Instance;
// There are three overloads for 'Set' for int, string and bool types
instance.SetValue("x", 1);
instance.SetValue("color", "BLUE");
instance.SetValue("flag", true);
```

and finally, we can write the instance:

```csharp
output.Write();
```

Alternative, we can fill the *Instance* fields from a C# object like a *class* or *struct*. To do so, we can use the `SetValuesFrom` method from the `Instance` class:

```csharp
// Set some values to the instance
instance.SetValue("x", 2);
instance.SetValue("y", 3);

// Overwrite and set values from this object
MyClass obj = new MyClass {
    x = 1,
    color = "BLUE"
};
instance.SetValuesFrom(obj);

// write
output.Write();
```

*Note that the associated DDS DataWriter was created at the time the Connector class is instanciated*.

#### Reading samples
To read samples we need to get the *Input* defined in the configuration.

```csharp
Input reader = connector.GetInput("MySubscriber::MySquareReader");
```

Then we can retrieve the samples by calling the `Read` or `Take` methods. The former will keep the samples in the internal queue and the latter will remove them.

To access to the samples we can use the `Samples` property from the *Input*. This is an `IEnumerable<Sample>` type so we can iterate over them.

```csharp
// Read samples
input.Take();
Console.WriteLine("Received {0} samples", input.Samples.Count);

// Iterate over the read samples
foreach (Sample sample in input.Samples) {
    if (sample.Info.IsValid) {
        // This sample contains user data.
        // You can get the fields with the GetInt, GetString and GetBool methods.
        int x = sample.Data.GetValueInt32("x");
        string color = sample.Data.GetValueString("color");

        // Or by using the Get<T> method
        int y = sample.DataGetInt32("y");
    } else {
        // This is a metadata sample.
        Console.WriteLine("Received metadata");
    }
}
```

We can also convert the sample into a C# object by using the `GetSampleAs<T>` and `GetSampleAsObject` methods of the sample:
```csharp
MyClass sample = sample.Data.GetSampleAs<MyClass>();
int x = sample.X;
string color = sample.Color;
```
