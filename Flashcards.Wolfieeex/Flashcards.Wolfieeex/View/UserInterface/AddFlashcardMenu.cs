using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using System.Collections.Immutable;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class AddFlashcardMenu : MulitInputMenu
{
	public AddFlashcardMenu(Color color) : base(color)
	{
		_selectionType = typeof(MultiInputMenuEnums.InsertFlashcardSelection);
	}

	public override void MenuRunningLoop()
	{
		bool menuIsRunning = true;
		while (menuIsRunning)
		{
			Console.Clear();

			var userInput = AnsiConsole.Prompt(new SelectionPrompt<Enum>()
				.Title("Choose options to insert your new Flashcard:")
				.AddChoices(GenerateOptions())
				.UseConverter(s => SmartOptionConverter(s))
				.HighlightStyle(style)
				.WrapAround()
				);
		}
	}
}
