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

if [ "$NUGET_RESTORE" = "true" ]; then
	nuget restore    || exit -1
fi

msbuild ./Medja.sln /p:Configuration=$BUILD_CONFIG /t:Build /p:OutDir=$(realpath "$BUILD_TARGET_DIR") /p:AllowedReferenceRelatedFileExtensions=none  || exit -1

popd    # back to build
