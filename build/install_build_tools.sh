#! /bin/bash

. common.sh

[ -f "$COMMON_BIN_DIR/installed" ] && exit 0

touch "$COMMON_BIN_DIR/installed"
pushd "$COMMON_BIN_DIR"

[ ! -f "$COMMON_BIN_DIR/nuget.exe" ] && wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

[ ! -f "$XUNIT_RUNNER_DIR" ] && nuget install -NonInteractive -Source https://api.nuget.org/v3/index.json xunit.runner.console -Version $XUNIT_VERSION

popd
