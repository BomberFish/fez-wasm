CWD=$(shell pwd)

statics: fna-clone
	mkdir -p statics
	@test -f statics/FAudio.a || wget -q --show-progress https://github.com/MercuryWorkshop/celeste-wasm/raw/e353189f02c7eb5e32ab5c9a80eb89371c95da1e/FAudio.a -O statics/FAudio.a
	@test -f statics/FNA3D.a || wget -q --show-progress https://github.com/MercuryWorkshop/celeste-wasm/raw/e353189f02c7eb5e32ab5c9a80eb89371c95da1e/FNA3D.a -O statics/FNA3D.a
	@test -f statics/libmojoshader.a || wget -q --show-progress https://github.com/MercuryWorkshop/celeste-wasm/raw/e353189f02c7eb5e32ab5c9a80eb89371c95da1e/libmojoshader.a -O statics/libmojoshader.a
	@test -f statics/SDL2.a || wget -q --show-progress https://github.com/MercuryWorkshop/celeste-wasm/raw/e353189f02c7eb5e32ab5c9a80eb89371c95da1e/SDL2.a -O statics/SDL2.a

clean:
	rm -rv statics obj bin || true
	rm -rv Fez-Src/*/obj Fez-Src/*/bin || true

build: statics
	dotnet publish -c Release -v d
	@# microsoft messed up
	sed -i 's/FS_createPath("\/","usr\/share",!0,!0)/FS_createPath("\/usr","share",!0,!0)/' bin/Release/net9.0/publish/wwwroot/_framework/dotnet.runtime.*.js

vfs:
	@./tools/buildvfs.sh ./bin/Release/net9.0/publish/wwwroot

serve: build vfs serveonly

dev: build serveonly

serveonly:
	python3 tools/serve.py

fna-clone: 
	@test -d ../FNA || git clone --recurse-submodules https://github.com/FNA-XNA/FNA ../FNA

fna-patch: fna-clone
	cd ../FNA && git switch --detach 24.01 && git apply $(CWD)/FNA.patch

gen-patch:
	cd ../FNA && git diff > $(CWD)/FNA.patch

fna: fna-patch

