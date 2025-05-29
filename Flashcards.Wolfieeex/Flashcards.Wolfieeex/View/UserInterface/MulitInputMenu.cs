using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class MulitInputMenu : Menu
{
	private Type _selectionType;

	MulitInputMenu(Color color, Type EnumSelectionType) : base(color)
	{
		_selectionType = EnumSelectionType;
	}

	public override void DisplayMenu()
	{


		bool menuIsRunning = true;
		while (menuIsRunning)
		{
			Console.Clear();
		}
	}
}
