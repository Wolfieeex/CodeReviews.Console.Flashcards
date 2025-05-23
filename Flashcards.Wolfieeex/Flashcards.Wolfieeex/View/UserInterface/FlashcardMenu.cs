using Spectre.Console;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class FlashcardMenu : Menu
{
	FlashcardMenu() : base(Color.Orange4) { }

	public override void DisplayMenu()
	{
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
				);

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
	}

	private void UpdateFlashcard()
	{
		throw new NotImplementedException();
	}

	private void DeleteFlashcard()
	{
		throw new NotImplementedException();
	}

	private void AddFlashcard()
	{
		throw new NotImplementedException();
	}

	private void ViewFlashcards()
	{
		throw new NotImplementedException();
	}
}
