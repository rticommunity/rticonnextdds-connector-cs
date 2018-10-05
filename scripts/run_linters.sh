#!/bin/bash
ROOT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/..

StyleCop.Baboon \
    "${ROOT_DIR}"/scripts/Settings.StyleCop \
    "${ROOT_DIR}"/src/Connector \
    "${ROOT_DIR}"/src/Connector/bin
if [ $? -ne 0 ] ; then exit 1; fi

StyleCop.Baboon \
    "${ROOT_DIR}"/scripts/Settings.StyleCop \
    "${ROOT_DIR}"/src/Connector.UnitTests \
    "${ROOT_DIR}"/src/Connector.UnitTets/bin \
    "${ROOT_DIR}"/src/Connector.UnitTests/TestTypes.cs
if [ $? -ne 0 ] ; then exit 1; fi

# Broken on modern Linux because of a missing dependency webkit-sharp
# See: https://github.com/mono/mono-tools/pull/50
# gendarme \
#     --ignore "${ROOT_DIR}"/scripts/gendarme.ignore \
#     --html "${ROOT_DIR}"/gendarme_report.html \
#     "${ROOT_DIR}"/src/Connector/bin/Debug/net35/librtiddsconnector_dotnet.dll
# if [ $? -ne 0 ] ; then exit 2; fi
