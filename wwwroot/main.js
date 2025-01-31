let setModuleImports, getAssemblyExports, getConfig;
let initted = false;

const wasm = await eval(`import("/_framework/dotnet.js")`);
const dotnet = wasm.dotnet;

console.debug("initializing dotnet");
const runtime = await dotnet.withConfig({
	jsThreadBlockingMode: "DangerousAllowBlockingWait",
}).create();

const config = runtime.getConfig();
const exports = await runtime.getAssemblyExports(config.mainAssemblyName);
const canvas = document.getElementById("canvas");
dotnet.instance.Module.canvas = canvas;

self.wasm = {
	Module: dotnet.instance.Module,
	dotnet,
	runtime,
	config,
	exports,
	canvas,
};

export async function init() {
	if (initted) return;
	({ setModuleImports, getAssemblyExports, getConfig } = await dotnet
		.withModuleConfig({
			onConfigLoaded: (config) => {
				config.disableIntegrityCheck = true;
			},
		})
		.withDiagnosticTracing(false)
		.withApplicationArgumentsFromQuery()
		.create());

	if (!dotnet.instance.Module.FS.analyzePath("/libsdl").path) {
		dotnet.instance.Module.FS.mkdir("/libsdl", 0o755);
		dotnet.instance.Module.FS.mount(
			dotnet.instance.Module.FS.filesystems.IDBFS,
			{},
			"/libsdl",
		);
	}
	await new Promise((r) => dotnet.instance.Module.FS.syncfs(true, r));
	console.debug("synced; exposing dotnet FS");

	window.FS = dotnet.instance.Module.FS;
	setModuleImports("main.js", {
		setMainLoop: MainLoop,
		stopMainLoop: () => dotnet.instance.Module.pauseMainLoop(),
		syncFs: (cb) => dotnet.instance.Module.FS.syncfs(false, cb),
	});
	initted = true;
}

let ts = performance.now();
export let fps;
const MainLoop = (cb) => {
	dotnet.instance.Module.setMainLoop(() => {
		let now = performance.now();
		let dt = now - ts;
		ts = now;
		fps = 1000 / dt;
		cb();
		document.getElementById("fps").innerText = fps.toFixed(2);
	});
};

export async function startGame() {
	document.getElementById("start").disabled = true;
	if (!dotnet.instance.Module.FS.analyzePath("/Content").path) {
		await new Promise(r => loadData(dotnet.instance.Module, r));
		console.info("Loaded assets into VFS");
	}

	console.debug("PreInit...");
	await runtime.runMain();
	await exports.Program.PreInit();
	console.debug("dotnet initialized");

	console.debug("Init...");
	exports.Program.Init();

	console.debug("MainLoop...");
	const main = () => {
		const ret = exports.Program.MainLoop();

		if (!ret) {
			console.debug("Cleanup...");
			exports.Program.Cleanup();
			console.debug("Done");
			document.getElementById("start").disabled = false;
			return;
		}

		requestAnimationFrame(main);
	}
	requestAnimationFrame(main);
}

// document.getElementById("start").addEventListener("click", startGame);