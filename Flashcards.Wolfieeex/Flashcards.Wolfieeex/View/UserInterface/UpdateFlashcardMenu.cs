using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.MultiInputMenuEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class UpdateFlashcardMenu : MulitInputMenu
{
	public Flashcard Flashcard { get; private set; } = new();
	public DataAccess dataAccess { get; private set; } = new();

	public UpdateFlashcardMenu(Color color) : base(color)
	{
		_selectionType = typeof(MultiInputMenuEnums.UpdateFlashcardSelection);
	}

	protected override void MenuRunningLoop()
	{
		DataAccess dataAccess = new DataAccess();

		bool updateFlashcardLoop = true;
		while (updateFlashcardLoop)
		{
			int stackId = FlashcardMenu.ChooseStack("Select from which stack you would like to update: ", menuColors.UserInputColor,
				"At this time, there is nothig to update. Check back later.");
			if (stackId == -1)
				return;

			bool isStackSelected = true;
			while (isStackSelected)
			{
				int flashcardId = FlashcardMenu.ChooseFlashcard($"Select which flashcard you would like to update from " +
					$"[#{menuColors.PositiveColor.ToHex()}]\"{dataAccess.GetStackName(stackId)}\"[/] stack:", stackId,
					menuColors.UserInputColor, "There are no flashcards to update at " +
					$"[#{menuColors.PositiveColor.ToHex()}]\"{dataAccess.GetStackName(stackId)}\"[/] stack. Press any button to continue: ");
				if (flashcardId == -1)
				{
					isStackSelected = false;
					continue;
				}

				Flashcard = dataAccess.GetFlashcard(stackId, flashcardId);
				inputs.Add(UpdateFlashcardSelection.UpdateAnswer);
				inputs.Add(UpdateFlashcardSelection.UpdateQuestion);
				inputs.Add(UpdateFlashcardSelection.UpdateStack);

				bool isFlashcardSelected = true;
				while (isFlashcardSelected)
				{
					Console.Clear();

					string stackOldName = dataAccess.GetStackName(stackId);
					string flashcardOldName = dataAccess.GetFlashcardName(stackId, flashcardId);
					string title = $"Choose new parameters for your [#{menuColors.Important1Color.ToHex()}]{stackOldName}[/] flashcard " +
						$"from [#{menuColors.Important2Color.ToHex()}]{flashcardOldName}[/] stack:";

					if (inputs.ContainsKey(MultiInputMenuEnums.InsertFlashcardSelection.ChooseQuestion)
						&& inputs.ContainsKey(MultiInputMenuEnums.InsertFlashcardSelection.ChooseStack))
					{
						if (!Input.FlashcardDataBaseRepetitionCheck(inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseStack],
								inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseQuestion]))
						{
							repetitionCheckFail = true;
							title = $"The Flashcard with question [#{menuColors.Important3Color.ToHex()}]\"" +
							$"{inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseQuestion]}\"[/] in stack " +
							$"[#{menuColors.Important1Color.ToHex()}]\"" +
							$"{dataAccess.GetStackName(int.Parse(inputs[MultiInputMenuEnums.InsertFlashcardSelection.ChooseStack]))}\"[/] already exists." +
							$" Choose a different question to update: ";
						}
						else
						{
							repetitionCheckFail = false;
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
						case UpdateFlashcardSelection.Confirm:
							return;

						case UpdateFlashcardSelection.ReturnToPreviousMenu:
							Flashcard = null;
							isFlashcardSelected = false;
							continue;

						case UpdateFlashcardSelection.UpdateAnswer:
							string tempAnswer = null;
							Input.ValidateInput(ref tempAnswer, GetDescription(UpdateFlashcardSelection.UpdateAnswer), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
							if (tempAnswer == "")
							{
								inputs.Remove(UpdateFlashcardSelection.UpdateAnswer);
								Flashcard.Answer = null;
							}
							else if (tempAnswer != null)
							{
								if (inputs.ContainsKey(UpdateFlashcardSelection.UpdateAnswer))
								{
									inputs[UpdateFlashcardSelection.UpdateAnswer] = tempAnswer;
									Flashcard.Answer = tempAnswer;
								}
								else
								{
									inputs.Add(UpdateFlashcardSelection.UpdateAnswer, tempAnswer);
									Flashcard.Answer = tempAnswer;
								}
							}
							break;

						case UpdateFlashcardSelection.UpdateQuestion:
							string tempQuestion = null;
							Input.ValidateInput(ref tempQuestion, GetDescription(UpdateFlashcardSelection.UpdateQuestion), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
							if (tempQuestion == "")
							{
								inputs.Remove(UpdateFlashcardSelection.UpdateQuestion);
								Flashcard.Question = null;
							}
							else if (tempQuestion != null)
							{
								if (inputs.ContainsKey(UpdateFlashcardSelection.UpdateQuestion))
								{
									inputs[UpdateFlashcardSelection.UpdateQuestion] = tempQuestion;
									Flashcard.Question = tempQuestion;
								}
								else
								{
									inputs.Add(UpdateFlashcardSelection.UpdateQuestion, tempQuestion);
									Flashcard.Question = tempQuestion;
								}
							}
							break;

						case UpdateFlashcardSelection.UpdateStack:
							int tempId = FlashcardMenu.ChooseStack(GetDescription(UpdateFlashcardSelection.UpdateStack), menuColors.Important2Color);
							if (tempId != -1)
							{
								if (inputs.ContainsKey(UpdateFlashcardSelection.UpdateStack))
								{
									inputs[UpdateFlashcardSelection.UpdateStack] = tempId.ToString();
									Flashcard.StackId = tempId;
								}
								else
								{
									inputs.Add(UpdateFlashcardSelection.UpdateStack, tempId.ToString());
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
	}
}
