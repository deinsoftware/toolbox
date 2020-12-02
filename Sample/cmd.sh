#!/bin/bash
cmd="$1";
dir="$2";

if [ -n "$dir" ]; then

osascript <<EOF
    tell application "Terminal" to do script "cd \"$dir\"; $cmd"
EOF

else

osascript <<EOF
    tell application "Terminal" to do script "$cmd"
EOF

fi
clear

exit 0