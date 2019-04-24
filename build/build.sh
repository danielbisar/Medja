#! /bin/bash
# please run with build as current folder

. common.sh

./install_build_tools.sh

mkdir -p "$BUILD_TARGET_DIR"

pushd ../src || exit -1
pushd native || exit -1

mkdir -p out
cd out       || exit -1
cmake ..     || exit -1
make         || exit -1

cp libmedja.so "$BUILD_TARGET_DIR"  || exit -1
cp -P libs/lib64/libglfw* "$BUILD_TARGET_DIR"  || exit -1

popd    # back to src

nuget restore    || exit -1
msbuild ./Medja.sln /p:Configuration=$BUILD_CONFIG /t:Build /p:OutDir=$(realpath "$BUILD_TARGET_DIR") /p:AllowedReferenceRelatedFileExtensions=none  || exit -1

popd    # back to build
