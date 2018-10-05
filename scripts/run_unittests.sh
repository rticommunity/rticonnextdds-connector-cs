#!/bin/bash
ROOT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/..
export LD_LIBRARY_PATH=${ROOT_DIR}/rticonnextdds-connector/lib/x64Linux2.6gcc4.4.5:$LD_LIBRARY_PATH

# First for mono / .net framework
nuget install NUnit.Runners -OutputDirectory "${ROOT_DIR}"/packages
mono "${ROOT_DIR}"/packages/NUnit.ConsoleRunner.*/tools/nunit3-console.exe \
    "${ROOT_DIR}"/src/Connector.UnitTests/bin/Debug/net45/librtiddsconnector_dotnet.UnitTests.dll \
    --process:Single
if [ $? -ne 0 ] ; then exit 1; fi

# Then with .NET Core
dotnet test \
    --no-build \
    -f netcoreapp2.1 \
    "${ROOT_DIR}"/src/Connector.UnitTests/Connector.UnitTests.csproj
if [ $? -ne 0 ] ; then exit 2; fi
