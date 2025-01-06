using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
	public struct DirectionalLight
	{
		public Vector3 Direction { get; set; }

		public Vector3 Diffuse { get; set; }

		public Vector3 Specular { get; set; }

		public void Initialize()
		{
			Vector3 vector2 = (Diffuse = (Specular = Vector3.One));
		}
	}
}
