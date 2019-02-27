# RTI Connector C# / .NET for Connext DDS

[![Build Status](https://www.travis-ci.org/rticommunity/rticonnextdds-connector-cs.svg?branch=master)](https://www.travis-ci.org/rticommunity/rticonnextdds-connector-cs)
[![codecov](https://codecov.io/gh/rticommunity/rticonnextdds-connector-cs/branch/master/graph/badge.svg)](https://codecov.io/gh/rticommunity/rticonnextdds-connector-cs)
[![Nuget](https://img.shields.io/nuget/v/RTI.Connext.Connector.svg)](https://www.nuget.org/packages/RTI.Connext.Connector)

*RTI Connector* for Connext DDS is a quick and easy way to access the power and
functionality of [RTI Connext DDS](http://www.rti.com/products/index.html).
It is based on [XML-Based Application Creation](https://community.rti.com/static/documentation/connext-dds/6.0.0/doc/manuals/connext_dds/xml_application_creation/RTI_ConnextDDS_CoreLibraries_XML_AppCreation_GettingStarted.pdf)
and Dynamic Data.

*Connector* was created to quickly and easily develop
demos and proof of concept. We think that it can be useful for anybody who needs
a quick way to script tests and interact with Connext DDS using different scripting languages.

*Connector* can be used to quickly create tests for your distributed system and, thanks to the binding with scripting languages and the use of XML, to easily integrate with many other available technologies.

The *Connector* library is provided in binary form for selected architectures.
Scripting language bindings and examples are provided in source format.

## Compatibility

We use P/Invoke technology to call the native functions. The library targets
.NET Standard 1.1 and.NET Framework 3.5, so it's compatible with most .NET
implementations like .NET Core, .NET Framework, and Mono (including Xamarin
and Unity). It has been tested on Windows, Linux, Mac OS and Android.

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

The online documentation is available
[here](https://rticommunity.github.io/rticonnextdds-connector-cs).

The .NET API is self-documented. You can generate the documentation with
[docfx](https://dotnet.github.io/docfx) running:

```bash
# Linux / Mac OS
./build.sh --target="Generate-DocWeb"

# Windows
build.ps1 --target="Generate-DocWeb"
```

The documentation includes an [overview of the API](xref:api_overview) and
the description of each class.

You can also check the _examples_. They cover the full API usage.

## Getting started with .NET

Make sure you have installed [Mono](http://www.mono-project.com/download/),
[.NET Core](https://www.microsoft.com/net/core) or .NET Framework.
Then clone the repository and compile the API:

```bash
git clone https://github.com/rticommunity/rticonnextdds-connector-cs --recursive

# Go to the Connector directory
cd rticonnextdds-connector-cs

# Build and run tests
# Linux and Mac OS
./build.sh

# Windows
build.ps1
```

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
