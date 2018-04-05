Use setup_system.sh to setup your system for building.

* install_mono.sh installes mono (latest version)
* build_opentk.sh clones opentk and builds it

# Build
from root folder of the repo

CURRENTLY WORKING PARTLY even though you get an error message

nuget restore
msbuild /t:Rebuild /p:Configuration=Release Medja.sln


# Install mono

Install the latest stable version of mono-complete which is much higher than what ubuntu offers directly. Official for the used package 16.04, tested with 17.10.

http://www.mono-project.com/download/stable/#download-lin
http://fsharp.org/use/linux/

FSharp is required by opentk.

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


