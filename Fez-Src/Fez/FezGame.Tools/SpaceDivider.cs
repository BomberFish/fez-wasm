using System;
using System.Collections.Generic;
using FezEngine.Tools;
using Microsoft.Xna.Framework;

namespace FezGame.Tools
{
	internal class SpaceDivider
	{
		public struct DividedCell
		{
			public int Left;

			public int Bottom;

			public int Width;

			public int Height;

			public int Top => Bottom + Height;

			public int Right => Left + Width;

			public Vector2 Center => new Vector2((float)Left + (float)Width / 2f, (float)Bottom + (float)Height / 2f);

			public DividedCell(int left, int bottom, int width, int height)
			{
				Left = left;
				Bottom = bottom;
				Width = width;
				Height = height;
			}
		}

		public static List<DividedCell> Split(int count)
		{
			List<DividedCell> list = new List<DividedCell>
			{
				new DividedCell(0, 0, 16, 16)
			};
			for (int i = 0; i < count - 1; i++)
			{
				int num = 4;
				int num2 = 0;
				DividedCell item;
				do
				{
					item = RandomHelper.InList(list);
					if (num2++ > 100)
					{
						break;
					}
					float num3 = (float)item.Bottom + (float)item.Height / 2f;
					num = ((num3 < 6f) ? 2 : 4);
				}
				while (item.Width <= num && item.Height <= num);
				if (num2 > 100)
				{
					break;
				}
				list.Remove(item);
				if ((RandomHelper.Probability(0.5) && item.Height != num) || item.Width == num)
				{
					int num4 = (int)Math.Round((float)item.Height / 2f);
					list.Add(new DividedCell(item.Left, item.Bottom + num4, item.Width, item.Height - num4));
					list.Add(new DividedCell(item.Left, item.Bottom, item.Width, num4));
				}
				else
				{
					int num5 = (int)Math.Round((float)item.Width / 2f);
					list.Add(new DividedCell(item.Left, item.Bottom, num5, item.Height));
					list.Add(new DividedCell(item.Left + num5, item.Bottom, item.Width - num5, item.Height));
				}
			}
			return list;
		}
	}
}
