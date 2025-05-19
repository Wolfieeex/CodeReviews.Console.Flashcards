using Spectre.Console;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class MainMenu : Menu
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

			var usersChoice = AnsiConsole.Prompt(
				new SelectionPrompt<MainMenuChoices>()
				.Title("Welcome to flashcards! Make your selection:")
				.AddChoices(
					MainMenuChoices.ManageStacks,
					MainMenuChoices.ManageFlashcards,
					MainMenuChoices.Quit)
				);

		}
	}
}
