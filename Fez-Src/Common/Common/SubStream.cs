using System;
using System.IO;

namespace Common
{
	public class SubStream : Stream
	{
		private readonly Stream baseStream;

		private readonly long length;

		private readonly long baseOffset;

		private long position;

		public override long Length => length;

		public override long Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
				Seek(position, SeekOrigin.Begin);
			}
		}

		public Stream BaseStream => baseStream;

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => false;

		public SubStream(Stream baseStream, long offset, long length)
		{
			if (!baseStream.CanRead || !baseStream.CanSeek)
			{
				throw new InvalidOperationException("Underlying stream must be seekable and readable");
			}
			this.baseStream = baseStream;
			this.length = length;
			baseOffset = offset;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				baseStream.Seek(baseOffset + offset, SeekOrigin.Begin);
				position = offset;
				break;
			case SeekOrigin.Current:
				offset = Math.Max(-position, Math.Min(offset, (int)(length - position)));
				baseStream.Seek(offset, SeekOrigin.Current);
				position += offset;
				break;
			case SeekOrigin.End:
				offset = Math.Max(-length, Math.Min(offset, 0L));
				baseStream.Seek(offset, SeekOrigin.End);
				position = length + offset;
				break;
			}
			return position;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			count = Math.Min(count, (int)(length - position));
			int num = baseStream.Read(buffer, offset, count);
			position += num;
			return num;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Substreams are readonly");
		}

		public override void Flush()
		{
			throw new NotSupportedException("Substreams are readonly");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException("Substreams are readonly");
		}
	}
}
