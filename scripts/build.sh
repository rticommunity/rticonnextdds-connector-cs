#!/bin/bash
REPO_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/..

function compile {
    pushd "$1"
    command -v dotnet >/dev/null 2>&1 && dotnet restore "$1"/$2.sln || nuget restore "$1"/$2.sln
    popd

    msbuild /v:minimal "$1"/$2.sln
}

compile "${REPO_DIR}/src" Connector
compile "${REPO_DIR}/examples/Simple/" Simple
compile "${REPO_DIR}/examples/Mixed/" Mixed
compile "${REPO_DIR}/examples/Objects/" Objects
