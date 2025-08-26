using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.MultiInputMenuEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class UpdateFlashcardMenu : MulitInputMenu
{
	public Flashcard Flashcard { get; private set; } = new();
	public Flashcard oldFlashcardData { get; private set; }
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

				oldFlashcardData = dataAccess.GetFlashcard(stackId, flashcardId);
				Flashcard = new();
				inputs.Clear();

				bool isFlashcardSelected = true;
				while (isFlashcardSelected)
				{
					Console.Clear();

					string stackOldName = dataAccess.GetStackName(stackId);
					string flashcardOldName = dataAccess.GetFlashcardName(stackId, flashcardId);
					string title = $"Choose new parameters for your [#{menuColors.Important1Color.ToHex()}]{stackOldName}[/] flashcard " +
						$"from [#{menuColors.Important2Color.ToHex()}]{flashcardOldName}[/] stack:";

					GenerateInputs();
					title = RepetitionChecker(title);

					var userInput = AnsiConsole.Prompt(new SelectionPrompt<Enum>()
				.Title(title)
				.AddChoices(GenerateOptions(checkForFlashcardRepetitions: true))
				.UseConverter(s => SmartOptionConverter(s, oldFlashcardData))
				.HighlightStyle(style)
				.WrapAround()
				);

					switch (userInput)
					{
						case UpdateFlashcardSelection.Confirm:
							var properties = Flashcard.GetType().GetProperties();
							Dictionary<string, object> dic = new Dictionary<string, object>();

							foreach (var prop in properties)
							{
								if (prop.GetValue(Flashcard, null) != null && prop.Name != "Id")
								{
									dic.Add(prop.Name, prop.GetValue(Flashcard, null));
								}
							}
							dataAccess.UpdateFlashcard(oldFlashcardData.Id, dic);

							Console.Clear();
							AnsiConsole.Markup($"Flashcard has been updated! Press any button to return to previous menu: ");
							Console.ReadKey();
							Console.Clear();
							isFlashcardSelected = false;
							break;

						case UpdateFlashcardSelection.ReturnToPreviousMenu:
							isFlashcardSelected = false;
							continue;

						case UpdateFlashcardSelection.UpdateAnswer:
							UpdateAnswer();
							break;

						case UpdateFlashcardSelection.UpdateQuestion:
							UpdateQuestion();
							break;

						case UpdateFlashcardSelection.UpdateStack:
							UpdateStack();
							break;

						default:
							throw new UnauthorizedAccessException("Insert Flashcard Selection option not recognised.");
					}
				}
			}
		}
	}

	private void GenerateInputs()
	{
		if (inputs.ContainsKey(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateQuestion))
		{
			if (inputs[MultiInputMenuEnums.UpdateFlashcardSelection.UpdateQuestion].ToLower() == oldFlashcardData.Question.ToLower())
			{
				inputs.Remove(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateQuestion);
			}
		}
		if (inputs.ContainsKey(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateAnswer))
		{
			if (inputs[MultiInputMenuEnums.UpdateFlashcardSelection.UpdateAnswer].ToLower() == oldFlashcardData.Answer.ToLower())
			{
				inputs.Remove(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateAnswer);
			}
		}
		if (inputs.ContainsKey(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateStack))
		{
			if (inputs[MultiInputMenuEnums.UpdateFlashcardSelection.UpdateStack].ToLower() == oldFlashcardData.StackId.ToString())
			{
				inputs.Remove(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateStack);
			}
		}
	}

	private string RepetitionChecker(string title)
	{
		if (inputs.Count > 0)
		{
			string currentQuestion = "";
			string currentAnwer = "";
			int currentStack = -1;

			if (inputs.ContainsKey(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateQuestion))
			{
				currentQuestion = inputs[MultiInputMenuEnums.UpdateFlashcardSelection.UpdateQuestion];
			}
			else
			{
				currentQuestion = oldFlashcardData.Question;
			}
			if (inputs.ContainsKey(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateAnswer))
			{
				currentAnwer = inputs[MultiInputMenuEnums.UpdateFlashcardSelection.UpdateAnswer];
			}
			else
			{
				currentAnwer = oldFlashcardData.Answer;
			}
			if (inputs.ContainsKey(MultiInputMenuEnums.UpdateFlashcardSelection.UpdateStack))
			{
				currentStack = int.Parse(inputs[MultiInputMenuEnums.UpdateFlashcardSelection.UpdateStack]);
			}
			else
			{
				currentStack = oldFlashcardData.StackId;
			}


			if (!Input.FlashcardDataBaseRepetitionCheck(currentStack.ToString(), currentQuestion))
			{
				repetitionCheckFail = true;
				title = $"The Flashcard with question [#{menuColors.Important3Color.ToHex()}]\"" +
				$"{currentQuestion}\"[/] in stack " +
				$"[#{menuColors.Important1Color.ToHex()}]\"" +
				$"{dataAccess.GetStackName(currentStack)}\"[/] already exists." +
				$" Choose a different question to update: ";
			}
			else
			{
				repetitionCheckFail = false;
			}
		}

		return title;
	}

	private void UpdateAnswer()
	{
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
	}

	private void UpdateQuestion()
	{
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
	}

	private void UpdateStack()
	{
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
	}
}
