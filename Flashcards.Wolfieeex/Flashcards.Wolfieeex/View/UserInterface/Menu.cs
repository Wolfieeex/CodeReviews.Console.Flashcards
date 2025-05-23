namespace Flashcards.Wolfieeex.View.UserInterface;

using Spectre.Console;

public abstract class Menu
{
	protected Color TitleColor;
	protected Color SelectorsColor;
	protected Color Important1Color;
	protected Color Important2Color;
	protected Color Important3Color;
	protected Color UserInputColor;
	protected Color PositiveColor;
	protected Color NegativeColor;
	protected Color _menuColor;
	protected Color MenuColor { get => _menuColor ; set
		{
			_menuColor = value;
			TitleColor = value;
			SelectorsColor = value.Blend(Color.White, 0.8f);
			Important1Color = value.Blend(Color.Orange1, 0.8f);
			Important2Color = value.Blend(Color.Magenta1, 0.8f);
			Important3Color = value.Blend(Color.Pink1, 0.8f);
			UserInputColor = value.Blend(Color.Yellow, 0.8f);
			PositiveColor = value.Blend(Color.Green, 0.8f);
			NegativeColor = value.Blend(Color.Red, 0.8f);
		}
	}
	public Menu(Color color)
	{
		MenuColor = color;
	}
	public abstract void DisplayMenu();
}
