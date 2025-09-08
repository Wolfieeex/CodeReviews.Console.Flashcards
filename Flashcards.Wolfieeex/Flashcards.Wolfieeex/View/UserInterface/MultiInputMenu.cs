using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spectre.Console;
using System.Collections.Immutable;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.MultiInputMenuEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

abstract public class MultiInputMenu : Menu
{
	protected Flashcard Flashcard { get; set; } = new();
	internal DataAccess dataAccess { get; private set; } = new();

	protected Type _selectionType;
	protected bool repetitionCheckFail = false;
	protected Dictionary<Enum, string> inputs = new Dictionary<Enum, string>();

	public MultiInputMenu(Color color) : base(color) {}

	public sealed override void DisplayMenu()
	{
		try
		{
			if (!_selectionType.IsEnum)
				throw new ArgumentException("\"Enum Selection Type\" parameter must be of type Enum. (Also, make sure you assign" +
					"a value to it while instantiating a new menu).");

			MenuRunningLoop();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"There was an error while running Multi Input Menu with {_selectionType} type: {ex.Message}");
		}
	}

	protected virtual IEnumerable<Enum> GenerateOptions(bool checkForFlashcardRepetitions = false)
	{
		List<Enum> menuSelections = Enum.GetValues(_selectionType).Cast<Enum>().ToList();
		List<Enum> generatedOptions = new();

		bool displayConfirmation = true;
		bool oneOfInList = false;
		bool oneOfConfirmed = false;

		foreach (Enum enumVal in _selectionType.GetEnumValues())
		{
			if (GetSpecialLabel(enumVal) == SpecialLabels.NonOptional)
				if (!inputs.ContainsKey(enumVal))
					displayConfirmation = false;

			if (GetSpecialLabel(enumVal) == SpecialLabels.OneOf)
			{
				oneOfInList = true;

				if (inputs.ContainsKey(enumVal))
				{
					oneOfConfirmed = true;
				}
			}
		}

		if (oneOfInList == true && oneOfConfirmed == false)
			displayConfirmation = false;

		foreach (Enum enumVal in _selectionType.GetEnumValues())
		{
			if (GetSpecialLabel(enumVal) == SpecialLabels.Confirm)
			{
				if (displayConfirmation && !repetitionCheckFail)
				{
					generatedOptions.Add(enumVal);
				}
			}
			else if (GetSpecialLabel(enumVal) != SpecialLabels.Confirm)
			{
				generatedOptions.Add(enumVal);
			}
		}

		return generatedOptions.ToImmutableList();
	}

	abstract protected void MenuRunningLoop();

	protected string SmartOptionConverter(Enum option, Flashcard flashcard = null)
	{
		if (inputs.ContainsKey(option))
			if (!int.TryParse(inputs[option], out _))
			{
				return GetDisplayName(option) + ": [#" + menuColors.Important2Color.ToHex() + "]" + inputs[option].ToString() + "[/]";
			}
			else
			{
				if (Enum.TryParse<UpdateFlashcardSelection>(option.ToString(), out _))
				{
					return (GetDisplayName(option) + ": [#" + menuColors.Important2Color.ToHex() + "]" + dataAccess.GetStackName(int.Parse(inputs[option])) + "[/]").TrimEnd(':', ' ');
				}
				else
				{
					return GetDisplayName(option);
				}
			}
		else if (flashcard != null)
		{
			switch (option)
			{
				case UpdateFlashcardSelection.UpdateStack:
					return (GetDisplayName(option) + ": [#" + menuColors.UserInputColor.ToHex() + "]" + 
						dataAccess.GetStackName(flashcard.StackId) + "[/]").TrimEnd(':', ' ');
				case UpdateFlashcardSelection.UpdateAnswer:
					return (GetDisplayName(option) + ": [#" + menuColors.UserInputColor.ToHex() + "]" +
						flashcard.Answer + "[/]").TrimEnd(':', ' ');
				case UpdateFlashcardSelection.UpdateQuestion:
					return (GetDisplayName(option) + ": [#" + menuColors.UserInputColor.ToHex() + "]" +
						flashcard.Question + "[/]").TrimEnd(':', ' ');
				default:
					return GetDisplayName(option);
			}
		}
		else
			return GetDisplayName(option);
	}

	protected virtual string AddToInputs(Enum key, string inputPointer)
	{
		if (key.ToString().ToLower().Contains("stack"))
		{
			int tempId = FlashcardMenu.ChooseStack(GetDescription(key), menuColors.Important2Color);
			if (tempId > 0)
			{
				if (inputs.ContainsKey(key))
				{
					inputs[key] = tempId.ToString();
					inputPointer = tempId.ToString();
				}
				else
				{
					inputs.Add(key, tempId.ToString());
					inputPointer = tempId.ToString();
				}
			}
			return tempId.ToString();
		}
		else
		{
			string tempQuestion = null;
			Input.ValidateInput(ref tempQuestion, GetDescription(key), ValidationType.AnyNonBlank, menuColors, BackOptions.ExitBlank);
			if (tempQuestion == "")
			{
				inputs.Remove(key);
				inputPointer = null;
			}
			else if (tempQuestion != null)
			{
				if (inputs.ContainsKey(key))
				{
					inputs[key] = tempQuestion;
					inputPointer = tempQuestion;
				}
				else
				{
					inputs.Add(key, tempQuestion);
					inputPointer = tempQuestion;
				}
			}
			return inputPointer;
		}
	}
}
