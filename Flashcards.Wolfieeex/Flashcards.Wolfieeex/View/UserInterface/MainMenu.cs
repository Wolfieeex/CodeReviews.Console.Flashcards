using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class MainMenu
{
	internal static void DisplayMainMenu()
	{
		var isMenuRunning = true;
		while (isMenuRunning)
		{
			AnsiConsole.Write(
				new FigletText("Flashcards")
				.Centered()
				.Color(Color.Red));

			//var usersChoice = AnsiConsole.Prompt(
		}
	}
}
