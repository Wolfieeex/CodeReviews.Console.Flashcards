using Spectre.Console;
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

	private static char[] specialSymbols = { '\"', '/', '\\', '<', '>', '{', '}' };

	public static string ValidateInput(string text, ValidationType type, MenuColors menuColors, BackOptions backOptions = BackOptions.Blank)
	{
		validationText = text;
		validationType = type;
		Input.menuColors = menuColors;
		Input.backOptions = backOptions;

		try
		{
			switch (type)
			{
				case ValidationType.Text:
					ValidateText();
					break;
				case ValidationType.Integer:
					ValidateInteger();
					break;
				case ValidationType.DateTime:
					ValidateDateTime();
					break;
				case ValidationType.Any:
					ValidateAny();
					break;
				case ValidationType.TimeSpan:
					ValidateTimeSpan();
					break;
			}
		}
		catch (Exception e)
		{
			Console.Write($"Some functions haven't been enabled yet in the validation class: {e.Message}\n");
		}

		return null;
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
		DisplayBackOptions();
		bool validationPending = true;
		while (validationPending)
		{
			string input = AnsiConsole.Ask<string>(validationText);
			if (!string.IsNullOrEmpty(input))
			{
				foreach (char c in specialSymbols)
				{
					if (input.Contains(c))
					{
						AnsiConsole.Write(new Markup($"You cannot use some of the special characters in your input ({specialSymbols}). Try again.\n", style: new Style(foreground: menuColors.NegativeColor)));
						continue;
					}
				}
			}
			else
			{
				AnsiConsole.Write(new Markup("Your input cannot be empty/blank. Try again.\n", style: new Style(foreground: menuColors.NegativeColor)));
			}
		}
	}

	private static void DisplayBackOptions()
	{
		if (backOptions == BackOptions.Exit)
		{
			AnsiConsole.Write(new Markup("Insert \"e\" to return to previous menu", style: new Style(decoration: Decoration.RapidBlink)).Justify(Justify.Left));
			Console.SetCursorPosition(0, 0);
		}
		else if (backOptions == BackOptions.ExitBlank)
		{
			throw new NotImplementedException("ExitBlank option for data validation hasn't been enabled yet.");
		}
		else if (backOptions == BackOptions.Blank)
		{
			throw new NotImplementedException("Blank option for data validation hasn't been enabled yet.");
		}
	}
}
