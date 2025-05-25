namespace Flashcards.Wolfieeex.View.UserInterface;

using Spectre.Console;

public abstract class Menu
{
	Color _menuColor;
	public struct MenuColors
	{
		public Color TitleColor;
		public Color SelectorsColor;
		public Color Important1Color;
		public Color Important2Color;
		public Color Important3Color;
		public Color UserInputColor;
		public Color PositiveColor;
		public Color NegativeColor;
	}
	protected MenuColors menuColors;
	protected Color MenuColor { get => _menuColor ; set
		{
			_menuColor = value;
			menuColors.TitleColor = value;
			menuColors.SelectorsColor = value.Blend(Color.White, 0.8f);
			menuColors.Important1Color = value.Blend(Color.Orange1, 0.8f);
			menuColors.Important2Color = value.Blend(Color.Magenta1, 0.8f);
			menuColors.Important3Color = value.Blend(Color.Pink1, 0.8f);
			menuColors.UserInputColor = value.Blend(Color.Yellow, 0.8f);
			menuColors.PositiveColor = value.Blend(Color.Green, 0.8f);
			menuColors.NegativeColor = value.Blend(Color.Red, 0.8f);
		}
	}
	public Menu(Color color)
	{
		MenuColor = color;
	}
	public abstract void DisplayMenu();
}
