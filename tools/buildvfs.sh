#!/usr/bin/env bash

set -x

LOC=$(dirname "$0")

emsdk=$(dirname "$(which emcc)")
file_packager="$emsdk/tools/file_packager"
wwwroot=$1

echo "Packing data..."
"$file_packager" data.data --preload Content/@/Content --js-output="data.js.tmp" --lz4 --no-node --use-preload-cache
echo "Done!"

echo "Moving data to $wwwroot/_framework/data.data..."
mv data.data "$wwwroot/_framework/data.data"
echo "Done!"

echo "Processing data.js.tmp..."
sed -i '2d' data.js.tmp

content=$(<data.js.tmp)
content=${content/\.data\'\);/.data\'); doneCallback();}
content=${content/packageName, t/assetblob, t}

# Ensure content is not empty
if [ -z "$content" ]; then
  echo "Error: content is empty after processing data.js.tmp"
  exit 1
fi

echo "Done!"

echo "Putting everything in place..."
# echo "function loadData(Module, doneCallback) {" > "$wwwroot/data.js"
# echo "$content" >> "$wwwroot/data.js"
# echo "}" >> "$wwwroot/data.js"

echo "function loadData(Module, doneCallback) {" > "$LOC/../wwwroot/data.js"
echo "$content" >> "$LOC/../wwwroot/data.js"
echo "}" >> "$LOC/../wwwroot/data.js"

rm data.js.tmp
echo "All done!"