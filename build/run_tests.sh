#! /bin/bash

. common.sh

TEST_DIR=$COMMON_SCRIPT_DIR/test_runs/$(date +"%Y_%m_%d_%H_%M_%S")
mkdir -p $TEST_DIR
cp -r "$COMMON_SCRIPT_DIR/out/." "$TEST_DIR"
pushd $TEST_DIR

LD_LIBRARY_PATH=. xunit *.Test.dll -diagnostics

popd