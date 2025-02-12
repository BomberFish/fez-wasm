using System.IO;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Readers
{
	public class FutureTexture2DReader : ContentTypeReader<FutureTexture2D>
	{
		public static readonly FutureTexture2DReader Instance = new FutureTexture2DReader();

		protected override FutureTexture2D Read(ContentReader input, FutureTexture2D existingInstance)
		{
			byte b;
			do
			{
				b = input.ReadByte();
			}
			while (b >> 7 == 1);
			FutureTexture2D futureTexture2D = new FutureTexture2D();
			futureTexture2D.BackingStream = (input.BaseStream as MemoryStream).GetBuffer();
			futureTexture2D.Format = (SurfaceFormat)input.ReadInt32();
			futureTexture2D.Width = input.ReadInt32();
			futureTexture2D.Height = input.ReadInt32();
			futureTexture2D.MipLevels = new FutureTexture2D.MipLevel[input.ReadInt32()];
			FutureTexture2D futureTexture2D2 = futureTexture2D;
			for (int i = 0; i < futureTexture2D2.MipLevels.Length; i++)
			{
				futureTexture2D2.MipLevels[i].SizeInBytes = input.ReadInt32();
				futureTexture2D2.MipLevels[i].StreamOffset = input.BaseStream.Position;
				input.BaseStream.Seek(futureTexture2D2.MipLevels[i].SizeInBytes, SeekOrigin.Current);
			}
			return futureTexture2D2;
		}
	}
}
