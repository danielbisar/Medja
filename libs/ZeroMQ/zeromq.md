# Ubuntu

## example projects
- zeromq-vsc: basic client server with request/reply pattern in c

## required software
please add missing if you find some

apt install build-essential autoconf libtool
apt install cmake	# for cppzmq 
apt install pkgconf

## build packages  

mkdir zeromq
cd zeromq

### Base lib

git clone https://github.com/zeromq/libzmq
cd libzmq
git checkout tags/v4.2.3
./autogen.sh
./configure.sh; make; 
sudo make install
cd ..

### C++ Bindings

git clone https://github.com/zeromq/cppzmq
cd cppzmq
git checkout tags/v4.2.3
mkdir build
cd build
cmake ..
sudo make install     # no make before required
cd ../..

### For QT (TBD if needed)

git clone https://github.com/jonnydee/nzmqt

