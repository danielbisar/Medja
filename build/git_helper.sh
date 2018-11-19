#!

isRunning=true

clear
git status


while $isRunning = "true"; do
	echo "press [c]ommit [f]etch [m]erge [p]ush [q]uit [s]tatus [u]ntracked"
	read -rsn1 key

	if [ "$key" = "u" ]; then
		untracked=($(git ls-files --others --exclude-standard))

		for i in "${!untracked[@]}"; do 
		    printf "%s\t%s\n" "$i" "${untracked[$i]}"
		done

		read -p "Enter the number of the file you want to use or [c]ancel.: " index

		if [ "$index" != "c" ]; then
		    file=${untracked[$index]}

		    if [ "$file" != "" ]; then
			echo "You selected: $file"
			echo "press [a]dd [d]elete [c]ancel"
			read -rsn1 key2

			if [ "$key2" = "a" ]; then
				git add $file
			elif [ "$key2" = "d" ]; then
				rm $file
			fi
		    fi
		fi
	elif [ "$key" = "c" ]; then
		git commit -a
	elif [ "$key" = "f" ]; then
		git fetch
	elif [ "$key" = "m" ]; then
		echo "not implemented yet" 
	elif [ "$key" = "p" ]; then
		git push
	elif [ "$key" = "q" ]; then
		isRunning=false
	elif [ "$key" = "s" ]; then
		git status
	fi
done

