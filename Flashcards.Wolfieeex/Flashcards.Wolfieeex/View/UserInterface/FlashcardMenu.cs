using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.MultiInputMenuEnums;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class FlashcardMenu : Menu
{
	public FlashcardMenu() : base(Color.Orange4) { }

	public override void DisplayMenu()
	{
		Console.Clear();

		var menuSelections = Enum.GetValues(typeof(FlashcardChoices)).Cast<FlashcardChoices>();
		
		string[] choices = { "1", "2", "3" };

		bool IsMenuRunning = true;
		while (IsMenuRunning)
		{
			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<FlashcardChoices>()
				.Title("What would you like to do today?")
				.AddChoices(menuSelections)
				.UseConverter(s => GetDisplayName(s))
				.HighlightStyle(style)
				.WrapAround()
				);

			try
			{
				switch (userChoice)
				{
					case FlashcardChoices.ViewFlashcards:
						ViewFlashcards();
						break;
					case FlashcardChoices.AddFlashcard:
						AddFlashcard();
						break;
					case FlashcardChoices.DeleteFlashcard:
						DeleteFlashcard();
						break;
					case FlashcardChoices.UpdateFlashcard:
						UpdateFlashcard();
						break;
					case FlashcardChoices.ReturnToMainMenu:
						IsMenuRunning = false;
						break;
				}
			}
			catch (Exception ex)
			{
				Console.Write($"Some functions in Flashcard Menu haven't been enabled yet: {ex.Message}\n");
			}
		}
	}

	

	private void UpdateFlashcard()
	{
		var stackId = ChooseStack("Choose the stack where the flashcard is: ");

		AddFlashcardMenu addFlashcardMenu = new(menuColors.UserInputColor);
		addFlashcardMenu.DisplayMenu();

		/*var flashcardId = ChooseFlashcard("Choose flashcard to update", stackId);

		var propertiesToUpdate = new Dictionary<string, object>();

		if (Ansi)*/
	}

	private void DeleteFlashcard()
	{
		bool menuWhile = true;
		while (menuWhile)
		{
			var stackId = ChooseStack("Where is the flashcard you want to delete?:");
			if (stackId == -1)
				return;

			var flashcard = ChooseFlashcard("Choose flashcard you want to delete", stackId);
			if (flashcard == -1)
				continue;

			if (!AnsiConsole.Confirm("Are you sure?"))
				continue;

			var dataAccess = new DataAccess();
			dataAccess.DeleteFlashcard(flashcard);
			menuWhile = false;
		}
	}

	private void AddFlashcard()
	{
		string dummyInput = "";

		AddFlashcardMenu addFlashcardMenu = new(menuColors.UserInputColor);
		addFlashcardMenu.DisplayMenu();

		if (addFlashcardMenu.Flashcard == null)
			return;

		var dataAccess = new DataAccess();
		dataAccess.InsertFlashcard(addFlashcardMenu.Flashcard);
	}

	public static int ChooseStack(string message)
	{
		var dataAccess = new DataAccess();
		var stacks = dataAccess.GetAllStacks();

		var stacksArray = stacks.Select(x => x.Name).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(message)
			.AddChoices("[grey]Return to previous menu[/]")
			.AddChoices(stacksArray));

		return stacks.SingleOrDefault(x => x.Name == option)?.Id ?? -1;
	}

	/// <returns>Returns -1 if user returns to previous menu without selection.</returns>
	public static int ChooseFlashcard(string message, int stackId)
	{
		var dataAccess = new DataAccess();
		var flashcards = dataAccess.GetAllFlashcards(stackId);

		var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(message)
			.AddChoices("[grey]Return to previous menu[/]")
			.AddChoices(flashcardsArray)
			);
			
		return flashcards.SingleOrDefault(x => x.Question == option)?.Id ?? -1;
	}

	private void ViewFlashcards()
	{
		throw new NotImplementedException();
	}
}
