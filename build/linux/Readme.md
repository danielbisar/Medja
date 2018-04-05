Use setup_system.sh to setup your system for building.

* install_mono.sh installes mono (latest version)
* build_opentk.sh clones opentk and builds it

# Build
from root folder of the repo

CURRENTLY WORKING PARTLY even though you get an error message

nuget restore
msbuild /t:Rebuild /p:Configuration=Release Medja.sln


# Install mono

https://baruchcodes.visualstudio.com/Medja/_wiki/wikis/Medja.wiki?wikiVersion=GBwikiMaster&pagePath=%2FLinux%20development%20environment

# Native libs for linux

Downloaded from https://github.com/mono/SkiaSharp/releases
Must match the used NuGet Package Version.

Must currently be placed at the top level of the project, else they will be in a subfolder in the build directory.




# currently not needed anymore
-------------------------------

# Build opentk

If the scripts fails during execution you can continue by 
cd source
cd opentk
./build.sh


