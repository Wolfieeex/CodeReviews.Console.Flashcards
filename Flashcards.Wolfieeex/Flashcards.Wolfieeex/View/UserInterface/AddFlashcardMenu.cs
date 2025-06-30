using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.MultiInputMenuEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class AddFlashcardMenu : MulitInputMenu
{
	public Flashcard Flashcard { get; private set; } = new();

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

				case InsertFlashcardSelection.ReturnToPreviousMenu:
					Flashcard = null;
					return;

				case InsertFlashcardSelection.ChooseAnswer:
					string tempAnswer = null;
					Input.ValidateInput(ref tempAnswer, GetDescription(InsertFlashcardSelection.ChooseAnswer), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
					if (tempAnswer == "")
					{
						inputs.Remove(InsertFlashcardSelection.ChooseAnswer);
						Flashcard.Answer = null;
					}
					else if (tempAnswer != null)
					{
						if (inputs.ContainsKey(InsertFlashcardSelection.ChooseAnswer))
						{
							inputs[InsertFlashcardSelection.ChooseAnswer] = tempAnswer;
							Flashcard.Answer = tempAnswer;
						}
						else
						{
							inputs.Add(InsertFlashcardSelection.ChooseAnswer, tempAnswer);
							Flashcard.Answer = tempAnswer;
						}
					}
					break;

				case InsertFlashcardSelection.ChooseQuestion:
					string tempQuestion = null;
					Input.ValidateInput(ref tempQuestion, GetDescription(InsertFlashcardSelection.ChooseQuestion), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
					if (tempQuestion == "")
					{
						inputs.Remove(InsertFlashcardSelection.ChooseQuestion);
						Flashcard.Question = null;
					}
					else if (tempQuestion != null)
					{
						if (inputs.ContainsKey(InsertFlashcardSelection.ChooseQuestion))
						{
							inputs[InsertFlashcardSelection.ChooseQuestion] = tempQuestion;
							Flashcard.Question = tempQuestion;
						}
						else
						{
							inputs.Add(InsertFlashcardSelection.ChooseQuestion, tempQuestion);
							Flashcard.Question = tempQuestion;
						}
					}
					break;

				case InsertFlashcardSelection.ChooseStack:
					int tempId = FlashcardMenu.ChooseStack(GetDescription(InsertFlashcardSelection.ChooseStack));
					if (tempId != -1)
					{
						if (inputs.ContainsKey(InsertFlashcardSelection.ChooseStack))
						{
							inputs[InsertFlashcardSelection.ChooseStack] = tempId.ToString();
							Flashcard.StackId = tempId;
						}
						else
						{
							inputs.Add(InsertFlashcardSelection.ChooseStack, tempId.ToString());
							Flashcard.StackId = tempId;
						}
					}
					break;

				default:
					throw new UnauthorizedAccessException("Insert Flashcard Selection option not recognised.");
			}
		}
	}

	
}
