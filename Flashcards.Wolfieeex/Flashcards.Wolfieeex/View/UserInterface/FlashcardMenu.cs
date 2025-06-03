using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class FlashcardMenu : Menu
{
	public FlashcardMenu() : base(Color.Orange4) { }

	protected override void DisplayMenu()
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

	private string GetQuestion()
	{
		string tempQuestion = null;
		Input.ValidateInput(ref tempQuestion, "Insert flashcard's question: ", ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);

		return tempQuestion;
	}

	private string GetAnswer()
	{
		string tempAnswer = null;
		Input.ValidateInput(ref tempAnswer, "Insert flashcard's answer: ", ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);

		return tempAnswer;
	}

	private void UpdateFlashcard()
	{
		var stackId = ChooseStack("Choose the stack where the flashcard is: ");
		var flashcardId = ChooseFlashcard("Choose flashcard to update", stackId);

		var propertiesToUpdate = new Dictionary<string, object>();

		//if (Ansi)
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

		flashcard.Question = GetQuestion();
		flashcard.Answer = GetAnswer();

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

	/// <returns>Returns -1 if user returns to previous menu without selection.</returns>
	private static int ChooseFlashcard(string message, int stackId)
	{
		var dataAccess = new DataAccess();
		var flashcards = dataAccess.GetAllFlashcards(stackId);

		var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(message)
			.AddChoices(flashcardsArray));

		return flashcards.Single(x => x.Question == option)?.Id ?? -1;
	}

	private void ViewFlashcards()
	{
		throw new NotImplementedException();
	}
}
