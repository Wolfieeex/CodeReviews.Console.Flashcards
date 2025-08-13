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

			string title = "Choose options to insert your new Flashcard:";

			if (inputs.ContainsKey(MultiInputMenuEnums.InsertFlashcardSelection.ChooseQuestion)
				&& inputs.ContainsKey(MultiInputMenuEnums.InsertFlashcardSelection.ChooseStack))
			{
				if (!Input.FlashcardDataBaseRepetitionCheck(inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseStack],
						inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseQuestion]))
				{
					title = $"The Flashcard with question [#{menuColors.Important3Color.ToHex()}]\"" +
					$"{inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseQuestion]}\"[/] in stack " +
					$"[#{menuColors.Important1Color.ToHex()}]\"" +
					$"{inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseStack]}\"[/] already exists." +
					$" Change one of those values to add a new Flashcard:";
				}
			}

			var userInput = AnsiConsole.Prompt(new SelectionPrompt<Enum>()
				.Title(title)
				.AddChoices(GenerateOptions(checkForFlashcardRepetitions: true))
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
					int tempId = FlashcardMenu.ChooseStack(GetDescription(InsertFlashcardSelection.ChooseStack), menuColors.Important2Color);
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

	internal void ClearQuestionAnswer()
	{
		inputs.Remove(InsertFlashcardSelection.ChooseQuestion);
		inputs.Remove(InsertFlashcardSelection.ChooseAnswer);
		Flashcard.Question = null;
		Flashcard.Answer = null;
	}
}
