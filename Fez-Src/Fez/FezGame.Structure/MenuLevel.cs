using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezGame.Components;
using FezGame.Tools;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Structure
{
	internal class MenuLevel
	{
		private string titleString;

		public readonly List<MenuItem> Items = new List<MenuItem>();

		public MenuLevel Parent;

		public bool IsDynamic;

		public bool Oversized;

		public Action OnScrollUp;

		public Action OnScrollDown;

		public Action OnClose;

		public Action OnReset;

		private bool initialized;

		public Action<SpriteBatch, SpriteFont, GlyphTextRenderer, float> OnPostDraw;

		private string xButtonString;

		private string aButtonString;

		private string bButtonString;

		public Action XButtonAction;

		public Action AButtonAction;

		public bool AButtonStarts;

		public string Title
		{
			get
			{
				return (titleString == null) ? null : StaticText.GetString(titleString);
			}
			set
			{
				titleString = value;
			}
		}

		public MenuItem SelectedItem => (SelectedIndex == -1 || Items.Count <= SelectedIndex) ? null : Items[SelectedIndex];

		public int SelectedIndex { get; set; }

		public bool ForceCancel { get; set; }

		public bool TrapInput { get; set; }

		public string XButtonString
		{
			get
			{
				return xButtonString;
			}
			set
			{
				xButtonString = value;
			}
		}

		public virtual string AButtonString
		{
			get
			{
				return (aButtonString == null) ? null : StaticText.GetString(aButtonString);
			}
			set
			{
				aButtonString = value;
			}
		}

		public string BButtonString
		{
			get
			{
				return (bButtonString == null) ? null : StaticText.GetString(bButtonString);
			}
			set
			{
				bButtonString = value;
			}
		}

		public IContentManagerProvider CMProvider { protected get; set; }

		public virtual void Initialize()
		{
			initialized = true;
		}

		public virtual void Dispose()
		{
		}

		public bool MoveDown()
		{
			MenuItem selectedItem = SelectedItem;
			if (SelectedItem != null)
			{
				SelectedItem.Hovered = false;
			}
			if (Items.Count == 0 || Items.All((MenuItem x) => !x.Selectable || SelectedItem.Hidden))
			{
				SelectedIndex = -1;
			}
			else
			{
				do
				{
					SelectedIndex++;
					if (SelectedIndex != Items.Count)
					{
						continue;
					}
					if (OnScrollDown != null)
					{
						OnScrollDown();
						if (SelectedIndex == Items.Count)
						{
							do
							{
								SelectedIndex--;
							}
							while ((!SelectedItem.Selectable || SelectedItem.Hidden) && !Items.All((MenuItem x) => !x.Selectable || x.Hidden));
							if (SelectedItem == null)
							{
								SelectedIndex = 0;
							}
							break;
						}
						if (SelectedItem == null)
						{
							SelectedIndex = 0;
						}
					}
					else
					{
						SelectedIndex = 0;
					}
				}
				while ((!SelectedItem.Selectable || SelectedItem.Hidden) && !Items.All((MenuItem x) => !x.Selectable || x.Hidden));
				if (selectedItem != SelectedItem)
				{
					SelectedItem.SinceHovered = TimeSpan.Zero;
				}
				SelectedItem.Hovered = true;
			}
			return selectedItem != SelectedItem;
		}

		public bool MoveUp()
		{
			MenuItem selectedItem = SelectedItem;
			if (SelectedItem != null)
			{
				SelectedItem.Hovered = false;
			}
			if (Items.Count == 0 || Items.All((MenuItem x) => !x.Selectable || SelectedItem.Hidden))
			{
				SelectedIndex = -1;
			}
			else
			{
				do
				{
					SelectedIndex--;
					if (SelectedIndex != -1)
					{
						continue;
					}
					if (OnScrollUp != null)
					{
						OnScrollUp();
						if (SelectedIndex == -1)
						{
							SelectedIndex++;
						}
					}
					else
					{
						SelectedIndex = Items.Count - 1;
					}
				}
				while ((!SelectedItem.Selectable || SelectedItem.Hidden) && !Items.All((MenuItem x) => !x.Selectable || x.Hidden));
				if (selectedItem != SelectedItem)
				{
					SelectedItem.SinceHovered = TimeSpan.Zero;
				}
				SelectedItem.Hovered = true;
			}
			return selectedItem != SelectedItem;
		}

		public virtual void Update(TimeSpan elapsed)
		{
			foreach (MenuItem item in Items)
			{
				if (item.Hovered)
				{
					item.SinceHovered += elapsed;
				}
				else
				{
					item.SinceHovered -= elapsed;
				}
				item.ClampTimer();
			}
		}

		public virtual void PostDraw(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
		{
			if (OnPostDraw != null)
			{
				OnPostDraw(batch, font, tr, alpha);
			}
		}

		public void Select()
		{
			if (SelectedItem != null && SelectedItem.Selectable)
			{
				SelectedItem.OnSelected();
			}
		}

		public MenuItem AddItem(string text)
		{
			return AddItem(text, -1);
		}

		public MenuItem AddItem(string text, Action onSelect)
		{
			return AddItem(text, onSelect, defaultItem: false, -1);
		}

		public MenuItem AddItem(string text, Action onSelect, bool defaultItem)
		{
			return AddItem(text, onSelect, defaultItem, Util.NullFunc<float>, Util.NullAction, -1);
		}

		public MenuItem<T> AddItem<T>(string text, Action onSelect, bool defaultItem, Func<T> sliderValueGetter, Action<T, int> sliderValueSetter)
		{
			return AddItem(text, onSelect, defaultItem, sliderValueGetter, sliderValueSetter, -1);
		}

		public MenuItem AddItem(string text, int at)
		{
			return AddItem(text, MenuBase.SliderAction, defaultItem: false, at);
		}

		public MenuItem AddItem(string text, Action onSelect, int at)
		{
			return AddItem(text, onSelect, defaultItem: false, at);
		}

		public MenuItem AddItem(string text, Action onSelect, bool defaultItem, int at)
		{
			return AddItem(text, onSelect, defaultItem, Util.NullFunc<float>, Util.NullAction, at);
		}

		public MenuItem<T> AddItem<T>(string text, Action onSelect, bool defaultItem, Func<T> sliderValueGetter, Action<T, int> sliderValueSetter, int at)
		{
			MenuItem<T> menuItem = new MenuItem<T>();
			menuItem.Parent = this;
			menuItem.Text = text;
			menuItem.Selected = onSelect;
			menuItem.IsSlider = sliderValueGetter != new Func<T>(Util.NullFunc<T>);
			menuItem.SliderValueGetter = sliderValueGetter;
			menuItem.SliderValueSetter = sliderValueSetter;
			MenuItem<T> menuItem2 = menuItem;
			if (!initialized && (Items.Count == 0 || defaultItem))
			{
				foreach (MenuItem item in Items)
				{
					item.Hovered = false;
				}
				menuItem2.Hovered = true;
				SelectedIndex = Items.Count;
			}
			if (onSelect == new Action(Util.NullAction))
			{
				bool hovered = (menuItem2.Selectable = false);
				menuItem2.Hovered = hovered;
			}
			if (at == -1)
			{
				Items.Add(menuItem2);
			}
			else
			{
				Items.Insert(at, menuItem2);
			}
			return menuItem2;
		}

		public virtual void Reset()
		{
			if (OnReset != null)
			{
				OnReset();
			}
		}
	}
}
