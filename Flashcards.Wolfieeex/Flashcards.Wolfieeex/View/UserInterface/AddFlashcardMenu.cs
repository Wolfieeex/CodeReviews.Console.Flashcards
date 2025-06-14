using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.MultiInputMenuEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class AddFlashcardMenu : MulitInputMenu
{
	public Flashcard Flashcard { get; private set; }

	public AddFlashcardMenu(Color color) : base(color)
	{
		_selectionType = typeof(MultiInputMenuEnums.InsertFlashcardSelection);
	}

	protected override void MenuRunningLoop()
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

			switch (userInput)
			{
				case InsertFlashcardSelection.Confirm:
					return;
					break;
				case InsertFlashcardSelection.ReturnToPreviousMenu:
					Flashcard = null;
					return;
					break;
				case InsertFlashcardSelection.ChooseAnswer:
					string tempAnswer = null;
					Input.ValidateInput(ref tempAnswer, GetDescription(InsertFlashcardSelection.ChooseAnswer), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
					Flashcard.Answer = tempAnswer;
					break;
				case InsertFlashcardSelection.ChooseQuestion:
					string tempQuestion = null;
					Input.ValidateInput(ref tempQuestion, GetDescription(InsertFlashcardSelection.ChooseQuestion), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
					Flashcard.Question = tempQuestion;
					break;
				case InsertFlashcardSelection.ChooseStack:
					Flashcard.StackId = FlashcardMenu.ChooseStack(GetDescription(InsertFlashcardSelection.ChooseStack));
					break;
				default:
					throw new UnauthorizedAccessException("Insert Flashcard Selection option not recognised.");
			}
		}
	}
}
