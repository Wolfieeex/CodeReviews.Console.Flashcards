﻿using Spectre.Console;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

public class MainMenu : Menu
{
	public MainMenu() : base(Color.Purple3) { }

	public override void DisplayMenu()
	{
		Console.Clear();

		var isMenuRunning = true;
		while (isMenuRunning)
		{
			AnsiConsole.Write(
				new FigletText("Flashcards")
				.Centered()
				.Color(Color.CadetBlue));

			var menuSelections = Enum.GetValues(typeof(MainMenuChoices)).Cast<MainMenuChoices>();

			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<MainMenuChoices>()
				.Title("Welcome to flashcards! Make your selection:")
				.AddChoices(menuSelections)
				.UseConverter(s => GetDisplayName(s))
				.HighlightStyle(style)
				.WrapAround()
				);

			switch (userChoice)
			{
				case MainMenuChoices.ManageStacks:
					StacksMenu stacksMenu = new StacksMenu();
					stacksMenu.DisplayMenu();
					break;
				case MainMenuChoices.ManageFlashcards:
					FlashcardMenu flashcardMenu = new FlashcardMenu();
					flashcardMenu.DisplayMenu();
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
