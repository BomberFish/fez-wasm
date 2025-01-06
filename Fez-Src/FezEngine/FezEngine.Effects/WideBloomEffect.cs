using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
	public class WideBloomEffect : BaseEffect
	{
		private readonly SemanticMappedVector2 texelSize;

		private readonly SemanticMappedTexture texture;

		private readonly SemanticMappedSingle blurWidth;

		private readonly SemanticMappedSingle intensity;

		public float BlurWidth
		{
			set
			{
				blurWidth.Set(value);
			}
		}

		public float Intensity
		{
			set
			{
				intensity.Set(value);
			}
		}

		public WideBloomEffect()
			: base("WideBloomEffect")
		{
			texelSize = new SemanticMappedVector2(effect.Parameters, "TexelSize");
			texture = new SemanticMappedTexture(effect.Parameters, "Texture");
			blurWidth = new SemanticMappedSingle(effect.Parameters, "BlurWidth");
			intensity = new SemanticMappedSingle(effect.Parameters, "Intensity");
		}

		public override void Prepare(Mesh mesh)
		{
			base.Prepare(mesh);
			texture.Set(mesh.Texture);
			texelSize.Set(new Vector2(1f / (float)mesh.TextureMap.Width, 1f / (float)mesh.TextureMap.Height));
		}
	}
}
