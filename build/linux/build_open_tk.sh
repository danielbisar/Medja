#!

set -e # makes the script stop at the first error

mkdir source
cd source

git clone https://github.com/opentk/opentk.git

# TODO use a specific (working) version
# currently we just get the latest on develop branch

cd opentk

./build.sh


