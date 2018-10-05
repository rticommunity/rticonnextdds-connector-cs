# RTI Connector C# / .NET for Connext DDS

*RTI Connector* for Connext DDS is a quick and easy way to access the power and
functionality of [RTI Connext DDS](http://www.rti.com/products/index.html).
It is based on [XML App Creation](https://community.rti.com/static/documentation/connext-dds/5.3.1/doc/manuals/connext_dds/xml_application_creation/RTI_ConnextDDS_CoreLibraries_XML_AppCreation_GettingStarted.pdf)
and Dynamic Data.

*Connector* was created by the RTI Research Group to quickly and easily develop
demos and proof of concept. We think that it can be useful for anybody who needs
a quick way to script tests and interact with DDS using different scripting
languages.

It can be used to quickly create tests for your distributed system and, thanks
to the binding with scripting languages and the use of XML, to easily integrate
with tons of other available technologies.

The *Connector* library is provided in binary form for selected architectures.
Scripting language bindings and examples are provided in source format.

## Compatibility

We use P/Invoke technology to call the native functions. This API works with
.NET Standard 1.1, .NET Framework 3.5, .NET Core .1.0, .NET Core 2.1 and Mono.
It's compatible with Windows, Linux, Mac OS X and Android.

### Native libraries

The *Connector* C# API use native libraries from the main
[Connector](https://github.com/rticommunity/rticonnextdds-connector) repository.
It can be found in the _rticonnextdds-connector_ directory.
Make sure to clone this repository with the `--recursive` argument or run:

```bash
git submodule update --init --recurse
```

The native libraries have been built for few architectures only.
If you need another architecture, please contact your RTI account manager or
sales@rti.com.

If you want to check the version of the libraries, run the following command:

``` bash
strings librtiddsconnector.dll | grep BUILD
```

### Threading model

The *Connector* Native and C# APIs do not yet implement any mechanism
for thread safety.

## Documentation

The .NET API is self-documented. You can generate the documentation with
[docfx](https://dotnet.github.io/docfx) running:

```bash
docfx docs/docfx.json --serve
```

The documentation includes an [overview of the API](xref:api_overview) and
the description of each class.

You can also check the _examples_. They cover the full API usage.

## Getting started with .NET

Make sure you have installed [Mono](http://www.mono-project.com/download/),
[.NET Core](https://www.microsoft.com/net/core) or .NET Framework.
Then clone the repository and compile the API:

* From terminal:

```bash
git clone https://github.com/rticommunity/rticonnextdds-connector-cs --recursive

# Go to the Connector directory
cd rticonnextdds-connector-cs/src

# To build with .NET Core you need this step too
cd Connector && dotnet restore && cd ..

# Build by default targets:
# .NET Framework 3.5, .NET Standard 1.1, .NET Core 1.0 and .NET Core 2.1
msbuild
```

* From Visual Studio or Monodevelop open *src/Connector-VS.sln*.

## Support

This is an experimental RTI product. As such we do offer support through the
[RTI Community Forum](https://community.rti.com/forums/technical-questions).
We also welcome issues and pull request in this GitHub repository.
We'd love your feedback.

## License

With the sole exception of the contents of the "examples" subdirectory,
all use of this product is subject to the RTI Software License Agreement
included at the top level of this repository. Files within the "examples"
subdirectory are licensed as marked within the file.

This software is an experimental ("pre-production") product. The Software is
provided "as is," with no warranty of any type, including any warranty for
fitness for any purpose. RTI is under no obligation to maintain or support the
software. RTI shall not be liable for any incidental or consequential damages
arising out of the use or inability to use the software.

The following non-RTI software is included in this distribution under the
corresponding license:

    Newtonsoft.Json https://www.newtonsoft.com/json

    Copyright (c) 2007 James Newton-King

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to
    deal in the Software without restriction, including without limitation the
    rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
    sell copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
    IN THE SOFTWARE.
