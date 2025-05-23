using Spectre.Console;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

public class MainMenu : Menu
{
	public MainMenu() : base(Color.Purple3) { }

	public override void DisplayMenu()
	{
		var isMenuRunning = true;
		while (isMenuRunning)
		{
			AnsiConsole.Write(
				new FigletText("Flashcards")
				.Centered()
				.Color(Color.Red));

			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<MainMenuChoices>()
				.Title("Welcome to flashcards! Make your selection:")
				.AddChoices(
					MainMenuChoices.ManageStacks,
					MainMenuChoices.ManageFlashcards,
					MainMenuChoices.Quit)
				);

			switch (userChoice)
			{
				case MainMenuChoices.ManageStacks:
					StacksMenu();
					break;
				case MainMenuChoices.ManageFlashcards:
					FlashcardsMenu();
					break;
				case MainMenuChoices.Quit:
					System.Console.Clear();
					AnsiConsole.Write(new Markup("Thank you for using this app. [#00ffff]See you soon! :D[/]", style: new Style(decoration: Decoration.RapidBlink)).Justify(Justify.Center));
					isMenuRunning = false;
					break;
			}
		}
	}
}
