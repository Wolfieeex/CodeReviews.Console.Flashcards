using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using System.Text.RegularExpressions;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class FlashcardMenu : Menu
{
	public FlashcardMenu() : base(Color.Orange4) { }

	public override void DisplayMenu()
	{
		Console.Clear();

		bool IsMenuRunning = true;
		while (IsMenuRunning)
		{
			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<FlashcardChoices>()
				.Title("What would you like to do today?")
				.AddChoices(
					FlashcardChoices.ViewFlashcards,
					FlashcardChoices.AddFlashcard,
					FlashcardChoices.UpdateFlashcard,
					FlashcardChoices.DeleteFlashcard,
					FlashcardChoices.ReturnToMainMenu)
				.UseConverter(x => Regex.Replace(x.ToString(), @"(.)([A-Z]{1})", @"$1 $2"))
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
		throw new NotImplementedException();
	}

	private void DeleteFlashcard()
	{
		var stackId = ChooseStack("Where is the flashcard you want to delete?:");
		var flashcard = ChooseFlashcard("Choose flashcard you want to delete", stackId);

		if (!AnsiConsole.Confirm("Are you sure?"))
			return;

		var dataAccess = new DataAccess();
		dataAccess.DeleteFlashcard(flashcard);
	}

	private void AddFlashcard()
	{
		Flashcard flashcard = new();

		string dummyInput = "";
		flashcard.StackId = ChooseStack("Choose one of your previous stacks:");

		Input.ValidateInput(ref dummyInput, "Insert question: ", ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
		flashcard.Question = dummyInput;
		Input.ValidateInput(ref dummyInput, "Insert answer: ", ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
		flashcard.Answer = dummyInput;

		var dataAccess = new DataAccess();
		dataAccess.InsertFlashcard(flashcard);
	}

	private static int ChooseStack(string message)
	{
		var dataAccess = new DataAccess();
		var stacks = dataAccess.GetAllStacks();

		var stacksArray = stacks.Select(x => x.Name).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(message)
			.AddChoices(stacksArray));

		return stacks.Single(x => x.Name == option).Id;
	}

	private static int ChooseFlashcard(string message, int stackId)
	{
		var dataAccess = new DataAccess();
		var flashcards = dataAccess.GetAllFlashcards(stackId);

		var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(message)
			.AddChoices(flashcardsArray));

		var flashcardId = flashcards.Single(x => x.Question == option).Id;

		return flashcardId;
	}

	private void ViewFlashcards()
	{
		throw new NotImplementedException();
	}
}
