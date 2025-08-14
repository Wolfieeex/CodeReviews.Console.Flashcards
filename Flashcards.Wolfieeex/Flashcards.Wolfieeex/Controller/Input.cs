using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using System.Text.RegularExpressions;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.View.UserInterface.Menu;

namespace Flashcards.Wolfieeex.Controller;

internal class Input
{
	private static char universalExitChar = 'e';
	private static char univarsalRemoveInputChar = ' ';

	private static string validationText;
	private static ValidationType validationType;
	private static MenuColors menuColors;
	private static BackOptions backOptions;

	private static string specialSymbols = @""", ', /, \\, <, >, {, }, [, ]";
	private static string specialSymbolsRegex = @"[""'\/\\<>\{\}\[\]]";

	private static string input;

	private static bool noClear;

	public static void ValidateInput(ref string previousInput, string text, ValidationType type, MenuColors menuColors, BackOptions backOptions = BackOptions.Blank, bool dontClear = false)
	{
		validationText = text;
		validationType = type;
		Input.menuColors = menuColors;
		Input.backOptions = backOptions;
		input = previousInput;
		noClear = dontClear;

		try
		{
			if (!dontClear)
			{
				Console.Clear();
			}

			switch (type)
			{
				case ValidationType.Any:
					ValidateAny();
					break;
				case ValidationType.AnyNonBlank:
					ValidateAnyNonBlank();
					break;
				case ValidationType.Text:
					ValidateText();
					break;
				case ValidationType.Integer:
					ValidateInteger();
					break;
				case ValidationType.DateTime:
					ValidateDateTime();
					break;
				case ValidationType.TimeSpan:
					ValidateTimeSpan();
					break;
			}

			previousInput = input;
		}
		catch (Exception e)
		{
			Console.Write($"Some functions haven't been enabled yet in the validation class: {e.Message}\n");
		}
	}

	private static void ValidateAnyNonBlank()
	{
		if (!noClear)
		{
			DisplayBackOptions();
		}

		bool validationPending = true;
		while (validationPending)
		{
			string input = AnsiConsole.Prompt(new TextPrompt<string>(validationText).AllowEmpty());

			if (ShouldQuitInputLoop(input))
				break;

			if (!string.IsNullOrEmpty(input) && input != "")
			{
				if (Regex.Match(input, specialSymbolsRegex).Success)
				{
					AnsiConsole.Write(new Markup($"You cannot use some of the special characters in your input. Try again.\n\n", style: new Style(foreground: menuColors.NegativeColor)));
					continue;
				}
			}
			else
			{
				AnsiConsole.Write(new Markup("Your input cannot be empty or blank. Try again.\n\n", style: new Style(foreground: menuColors.NegativeColor)));
				continue;
			}

			Input.input = input;
			break;
		} 
	}

	private static void ValidateTimeSpan()
	{
		throw new NotImplementedException();
	}

	private static void ValidateAny()
	{
		throw new NotImplementedException();
	}

	private static void ValidateDateTime()
	{
		throw new NotImplementedException();
	}

	private static void ValidateInteger()
	{
		throw new NotImplementedException();
	}

	private static void ValidateText()
	{
		if (!noClear)
		{
			DisplayBackOptions();
		}

		bool validationPending = true;
		while (validationPending)
		{
			string input = AnsiConsole.Prompt(new TextPrompt<string>(validationText).AllowEmpty());

			if (ShouldQuitInputLoop(input))
				break;

			if (!string.IsNullOrEmpty(input) && input != "")
			{
				if (Regex.Match(input, specialSymbolsRegex).Success)
				{
					AnsiConsole.Write(new Markup($"You cannot use some of the special characters in your input. Try again.\n\n", style: new Style(foreground: menuColors.NegativeColor)));
					continue;
				}
				if (Regex.Match(input, @"[0-9]").Success)
				{
					AnsiConsole.Write(new Markup($"You cannot use numbers in your input- only plain text with spaces is allowed. Try again.\n\n", style: new Style(foreground: menuColors.NegativeColor)));
					continue;
				}
			}
			else
			{
				AnsiConsole.Write(new Markup("Your input cannot be empty/blank. Try again.\n\n", style: new Style(foreground: menuColors.NegativeColor)));
				continue;
			}

			Input.input = input;
			break;
		}
	}

	private static void DisplayBackOptions()
	{
		if (backOptions == BackOptions.Exit)
		{
			AnsiConsole.Write(new Markup($"Insert [#{menuColors.Important1Color.ToHex()}]\"e\"[/] to return to previous menu. ", style: new Style(decoration: Decoration.RapidBlink)).Justify(Justify.Right));
			Console.SetCursorPosition(0, 1);
		}
		else if (backOptions == BackOptions.ExitBlank)
		{
			AnsiConsole.Write(new Markup($"Insert [#{menuColors.Important1Color.ToHex()}]\"e\"[/] to return to previous menu. \nOr, press [#{menuColors.Important2Color.ToHex()}]ENTER[/] with blank space  \nto clear out your previous input. ", style: new Style(decoration: Decoration.RapidBlink)).Justify(Justify.Right));
			Console.SetCursorPosition(0, 3);
		}
		else if (backOptions == BackOptions.Blank)
		{
			AnsiConsole.Write(new Markup($"Press [#{menuColors.Important2Color.ToHex()}]ENTER[/] with blank space to clear out your previous input. ", style: new Style(decoration: Decoration.RapidBlink)).Justify(Justify.Right));
			Console.SetCursorPosition(0, 1);
		}
	}

	/// <returns>
	/// Returns if user input loop should be exited.
	/// </returns>
	private static bool ShouldQuitInputLoop(string input)
	{
		switch (backOptions)
		{
			case BackOptions.None:
				return false;
			case BackOptions.Exit:
				if (string.IsNullOrEmpty(input))
					return false;
				else if (input.ToLower() == "e")
					return true;
				else 
					return false;
			case BackOptions.Blank:
				if (string.IsNullOrEmpty(input))
				{
					Input.input = "";
					return true;
				}
				else
					return false;
			case BackOptions.ExitBlank:
				if (string.IsNullOrEmpty(input))
				{
					Input.input = "";
					return true;
				}
				else if (input.ToLower() == "e")
					return true;
				else
					return false;
			default:
				throw new ArgumentException("There is an error in Compare method in Input class. No default results are expected in the switch statement.");
		}
	}

	internal static bool StackDatabaseRepetitionCheck(string input)
	{
		DataAccess dataAccess = new DataAccess();
		var stacks = dataAccess.GetAllStacks();

		var arrayOfStacks = stacks.Select(x => x.Name).ToArray();

		foreach (var stack in arrayOfStacks)
		{
			if (stack.ToLower() == input.ToLower())
				return false;
		}
		return true;
	}

	internal static bool FlashcardDataBaseRepetitionCheck(string id, string input)
	{
		DataAccess data = new DataAccess();

		var flashcards = data.GetAllFlashcards(int.Parse(id));

		var arrayOfFlashcards = flashcards.Select(x => x.Question).ToArray();

		foreach (var flashcard in arrayOfFlashcards)
		{
			if (flashcard.ToLower() == input.ToLower())
				return false;
		}
		return true;
	}
}
