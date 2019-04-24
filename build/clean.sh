#! /bin/bash

. common.sh

echo "Remove test runs..."
rm -rf "$TEST_RUN_DIR"

echo "Remove out and bin folder..."
rm -rf "$BUILD_TARGET_DIR"

echo "Remove build tools..."
rm -rf "$COMMON_BIN_DIR"
