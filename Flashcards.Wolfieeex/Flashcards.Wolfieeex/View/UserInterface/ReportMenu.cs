using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

enum ReportOptions
{
	
}

internal class ReportMenu : Menu
{
	public ReportMenu(Color color) : base(color) { }

	public override void DisplayMenu()
	{
		Console.Clear();
	}
}
