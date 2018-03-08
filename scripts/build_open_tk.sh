#!

set -e # makes the script stop at the first error

mkdir source
cd source

git clone https://github.com/opentk/opentk.git
cd opentk

./build.sh

