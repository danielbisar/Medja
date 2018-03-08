#!

# installes the latest stable version of mono-complete which is much higher than what 
# ubuntu offers directly

# originally take from http://www.mono-project.com/download/stable/#download-lin
# version for ubuntu 16.04, tested with 17.10

set -e # makes the script stop at the first error

if command -v mono >/dev/null 2>&1; then 
	echo mono is already installed
	echo checking version

	# check for version 5 or higher as this is the version which existed
	# at the time writing this script (actually 5.10.0.160)
	MONO_REQUIRED=5
	MONO_VERSION=$(mono --version | awk '/version/ { print $5 }')

	printf "found version %s\n" "$MONO_VERSION"
	
	if [ "$(printf '%s\n' "$MONO_REQUIRED" "$MONO_VERSION" | sort -V | head -n1)" = "$MONO_REQUIRED" ]; then 
		printf "All good!\n"
		exit 0
	 fi
fi

printf "going to install mono...\n"

sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/ubuntu stable-xenial main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt-get update

sudo apt update
sudo apt install mono-complete

