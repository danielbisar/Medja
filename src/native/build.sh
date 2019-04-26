#! /bin/bash

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

pushd $SCRIPT_DIR || exit 1
mkdir -p out || exit 1
cd out || exit 1
cmake .. || exit 1
make || exit 1

cp -u lib64/libglfw.so.3.3 .

