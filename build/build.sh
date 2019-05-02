#! /bin/bash
# please run with build as current folder

NUGET_RESTORE=true

while [[ $# -gt 0 ]]
do
	key="$1"
	
  case $key in
    -n|--no-restore)
    	echo "disable nuget restore"
    	NUGET_RESTORE=false
    	shift
     ;;
    --help) 
      echo "Usage: [options]"
      echo "-h\tprint this help"
      echo "-n\tno nuget restore"
      exit 2
      ;;
  esac
done

. common.sh

./install_build_tools.sh

mkdir -p "$BUILD_TARGET_DIR"

pushd ../src || exit -1
pushd native || exit -1

./build.sh || exit -1

if [[ "$OSTYPE" == darwin* ]]; then
	echo "Copy libs for mac"
	cp out/libglfw.3.3.dylib "$BUILD_TARGET_DIR"  || exit -1
	cp out/libmedja.dylib "$BUILD_TARGET_DIR"  || exit -1
else # unix and currently all others
	cp -u out/libmedja.so "$BUILD_TARGET_DIR"  || exit -1
	cp -u out/libglfw.so.3.3 "$BUILD_TARGET_DIR"  || exit -1
fi

popd    # back to src

if [ "$NUGET_RESTORE" = "true" ]; then
	nuget restore    || exit -1
fi

msbuild ./Medja.sln /p:Configuration=$BUILD_CONFIG /t:Build /p:OutDir=$(realpath "$BUILD_TARGET_DIR") /p:AllowedReferenceRelatedFileExtensions=none  || exit -1

popd    # back to build
