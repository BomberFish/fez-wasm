using System;
using System.Runtime.InteropServices;

public static class Vorbisfile
{
	public enum SeekWhence
	{
		SEEK_SET,
		SEEK_CUR,
		SEEK_END
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr read_func(IntPtr ptr, IntPtr size, IntPtr nmemb, IntPtr datasource);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int seek_func(IntPtr datasource, long offset, SeekWhence whence);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int close_func(IntPtr datasource);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr tell_func(IntPtr datasource);

	public struct vorbis_info
	{
		public int version;

		public int channels;

		public IntPtr rate;

		public IntPtr bitrate_upper;

		public IntPtr bitrate_nominal;

		public IntPtr bitrate_lower;

		public IntPtr bitrate_window;

		public IntPtr codec_setup;
	}

	public struct vorbis_comment
	{
		public IntPtr user_comments;

		public IntPtr comment_lengths;

		public int comments;

		public IntPtr vendor;
	}

	public struct ov_callbacks
	{
		public read_func read_func;

		public seek_func seek_func;

		public close_func close_func;

		public tell_func tell_func;
	}

	private const string nativeLibName = "libvorbisfile";

	// [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
	// private static extern IntPtr malloc(IntPtr size);

	// [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
	// private static extern void free(IntPtr memblock);

	[DllImport("Emscripten")]
	private static extern IntPtr wrap_malloc(IntPtr size);

	[DllImport("Emscripten")]
	private static extern void wrap_free(IntPtr memblock);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ov_fopen")]
	private static extern int INTERNAL_ov_fopen([In][MarshalAs(UnmanagedType.LPStr)] string path, IntPtr vf);

	public static int ov_fopen(string path, out IntPtr vf)
	{
		vf = AllocVorbisFile();
		return INTERNAL_ov_fopen(path, vf);
	}

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ov_open_callbacks")]
	private static extern int INTERNAL_ov_open_callbacks(IntPtr datasource, IntPtr vf, IntPtr initial, IntPtr ibytes, ov_callbacks callbacks);

	public static int ov_open_callbacks(IntPtr datasource, out IntPtr vf, IntPtr initial, IntPtr ibytes, ov_callbacks callbacks)
	{
		vf = AllocVorbisFile();
		return INTERNAL_ov_open_callbacks(datasource, vf, initial, ibytes, callbacks);
	}

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ov_info")]
	private static extern IntPtr INTERNAL_ov_info(IntPtr vf, int link);

	public static vorbis_info ov_info(IntPtr vf, int link)
	{
		IntPtr intPtr = INTERNAL_ov_info(vf, link);
		if (intPtr == IntPtr.Zero)
		{
			throw new InvalidOperationException("Specified bitstream does not exist or the file has been initialized improperly.");
		}
		return (vorbis_info)Marshal.PtrToStructure(intPtr, typeof(vorbis_info));
	}

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ov_comment")]
	private static extern IntPtr INTERNAL_ov_comment(IntPtr vf, int link);

	public static vorbis_comment ov_comment(IntPtr vf, int link)
	{
		IntPtr intPtr = INTERNAL_ov_comment(vf, link);
		if (intPtr == IntPtr.Zero)
		{
			throw new InvalidOperationException("Specified bitstream does not exist or the file has been initialized improperly.");
		}
		return (vorbis_comment)Marshal.PtrToStructure(intPtr, typeof(vorbis_comment));
	}

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern double ov_time_total(IntPtr vf, int i);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern long ov_pcm_total(IntPtr vf, int i);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ov_read(IntPtr vf, byte[] buffer, int length, int bigendianp, int word, int sgned, out int current_section);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ov_read(IntPtr vf, IntPtr buffer, int length, int bigendianp, int word, int sgned, out int current_section);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ov_time_seek(IntPtr vf, double s);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ov_pcm_seek(IntPtr vf, long s);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern double ov_time_tell(IntPtr vf);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern long ov_pcm_tell(IntPtr vf);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ov_clear")]
	private static extern int INTERNAL_ov_clear(IntPtr vf);

	public static int ov_clear(ref IntPtr vf)
	{
		int result = INTERNAL_ov_clear(vf);
		wrap_free(vf);
		vf = IntPtr.Zero;
		return result;
	}

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ov_streams(IntPtr vf);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ov_seekable(IntPtr vf);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ov_raw_seek(IntPtr vf, IntPtr pos);

	[DllImport("libvorbisfile", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ov_raw_tell(IntPtr vf);

	private static IntPtr AllocVorbisFile()
	{
		PlatformID platform = Environment.OSVersion.Platform;
		if (IntPtr.Size == 4)
		{
			return wrap_malloc((IntPtr)720);
		}
		if (IntPtr.Size == 8)
		{
			return platform switch
			{
				PlatformID.Unix => wrap_malloc((IntPtr)944), 
				PlatformID.Win32NT => wrap_malloc((IntPtr)840), 
				_ => throw new NotSupportedException("Unhandled platform!"), 
			};
		}
		throw new NotSupportedException("Unhandled architecture!");
	}
}
