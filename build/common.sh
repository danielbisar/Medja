#! /bin/bash

COMMON_SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
COMMON_BIN_DIR="$COMMON_SCRIPT_DIR/bin"

# USER VARIABLES

NET_TARGET="net472"
BUILD_CONFIG="Debug" # OR "Release"

XUNIT_VERSION="2.4.1"
XUNIT_RUNNER_DIR="$COMMON_BIN_DIR/xunit.runner.console.$XUNIT_VERSION"
TEST_RUN_DIR="$COMMON_SCRIPT_DIR/test_runs"

BUILD_TARGET_DIR="$COMMON_SCRIPT_DIR/out"

# install required nuget packages
mkdir -p $COMMON_BIN_DIR
pushd $COMMON_BIN_DIR

if [ "$BUILD_CONFIG" == "Debug" ]; then
    function mono_cmd()
    {
        mono --debug "$@"
    }
else
    function mono_cmd()
    {
        mono "$@"
    }
fi

function nuget()
{
    mono_cmd "$COMMON_BIN_DIR/nuget.exe" "$@"
}

function xunit()
{
    mono_cmd "$XUNIT_RUNNER_DIR/tools/$NET_TARGET/xunit.console.exe" "$@"
}

popd
