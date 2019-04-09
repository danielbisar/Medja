#! /bin/zsh

cd ../
nuget restore
msbuild /p:Configuration=Release ./Medja.sln
