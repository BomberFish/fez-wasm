#include <emscripten/console.h>
// #include <emscripten/wasmfs.h>
#include <emscripten/proxying.h>
// #include <emscripten/threading.h>
#include <assert.h>
#include <memory.h>
#include <stdlib.h>
#include <stdio.h>

// int mount_opfs() {
// 	emscripten_console_log("mount_opfs: starting");
// 	backend_t opfs = wasmfs_create_opfs_backend();
// 	emscripten_console_log("mount_opfs: created opfs backend");
// 	int ret = wasmfs_create_directory("/libsdl", 0777, opfs);
// 	emscripten_console_log("mount_opfs: mounted opfs");
// 	return ret;
// }

// malloc wrapper for emscripten
void *wrap_malloc(size_t size)
{
	char log_msg[50];
	snprintf(log_msg, sizeof(log_msg), "wrap_malloc: %zu", size);
	emscripten_console_log(log_msg);
	return malloc(size);
}
void wrap_free(void *ptr)
{
	char log_msg[50];
	snprintf(log_msg, sizeof(log_msg), "wrap_free: %p", ptr);
	emscripten_console_log(log_msg);
	free(ptr);
}

// memcpy wrapper for emscripten
void *wrap_memcpy(void *dest, const void *src, size_t n)
{
	char log_msg[50];
	snprintf(log_msg, sizeof(log_msg), "wrap_memcpy: %p, %p, %zu", dest, src, n);
	emscripten_console_log(log_msg);
	return memcpy(dest, src, n);
}