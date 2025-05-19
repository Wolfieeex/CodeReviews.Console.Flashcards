namespace Flashcards.Wolfieeex.View.UserInterface;

using Spectre.Console;

abstract internal class Menu
{
	private Color titleColor;
	private Color selectorsColor;
	private Color important1Color;
	private Color important2Color;
	private Color important3Color;
	private Color userInputColor;
	private Color _color;
	public abstract Color Color { get; set; }

}
