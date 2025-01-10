# fez-wasm

A very work-in-progress port of the 2012 game Fez to WebAssembly. 

Please don't kill me, Polytron.

## What works

- The game starts up, displays developer credits, and gets to the title screen.

## What doesn't work

- The game crashes when attempting to go past the title screen.

## Building

This doesn't work, so don't bother trying. 

If you really, really want to do it and you encounter issues, feel free to open an issue and I'll try to help.

- Run `make fna` to clone and patch FNA.
- Download the latest beta version of the game from Steam. (Yes, you must own the game)
- Copy the `Content` folder from the game's files into the repository's root.
- Run `make serve` to build the game and assets, and start a local server.
- Open `http://localhost:5002` in your browser.

> [!TIP]
> To avoid recreating the VFS on each subsequent rebuild, you can use `make dev` instead of `make serve`.