using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spectre.Console;
using System.Text.RegularExpressions;
using static Flashcards.Wolfieeex.Model.InputValidationEnums;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class StacksMenu : Menu
{
	public StacksMenu() : base(Color.Green3_1) { }

	public override void DisplayMenu()
	{
		bool IsMenuRunning = true;
		while (IsMenuRunning)
		{
			Console.Clear();

			var menuSelections = Enum.GetValues(typeof(StacksChoices)).Cast<StacksChoices>();

			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<StacksChoices>()
				.Title("What would you like to do today?")
				.AddChoices(menuSelections)
				.HighlightStyle(style)
				.UseConverter(s => GetDisplayName(s))
				.WrapAround()
				);

			try
			{
				switch (userChoice)
				{
					case StacksChoices.ViewStacks:
						ViewStacks();
						break;
					case StacksChoices.AddStack:
						AddStack();
						break;
					case StacksChoices.DeleteStack:
						DeleteStack();
						break;
					case StacksChoices.UpdateStack:
						UpdateStack();
						break;
					case StacksChoices.ReturnToMainMenu:
						IsMenuRunning = false;
						break;
				}
			}
			catch (Exception ex)
			{
				Console.Write($"Some functions in Stacks Menu haven't been enabled yet: {ex.Message}\n");
			}
		}
	}

	private void UpdateStack()
	{
		bool updateStackLoop = true;
		while (updateStackLoop)
		{
			Console.Clear();

			var stack = new Stack();

			var id = ChooseStack("Choose stack you want to update: ", menuColors.UserInputColor, "At this time there are no stacks to update:");
			if (id == -1)
				return;
			stack.Id = id;

			string tempName = null;
			Input.ValidateInput(ref tempName, "Insert a new name of your stack: ", ValidationType.AnyNonBlank, menuColors, BackOptions.Exit);

			if (string.IsNullOrEmpty(tempName))
			{
				continue;
			}
			stack.Name = tempName;

			bool repetitionCheck = Input.StackDatabaseRepetitionCheck(tempName);
			while (!repetitionCheck)
			{
				AnsiConsole.Markup($"[#{menuColors.NegativeColor.ToHex()}]{tempName}[/] stack name already exists in the database." +
					$" Please select [#{menuColors.Important3Color.ToHex()}]another name[/] for your stack.\n\n");

				tempName = null;
				Input.ValidateInput(ref tempName, "Insert a new name of your stack: ", ValidationType.Text, menuColors, BackOptions.Exit, dontClear: true);

				if (string.IsNullOrEmpty(tempName))
				{
					break;
				}
				stack.Name = tempName;

				repetitionCheck = Input.StackDatabaseRepetitionCheck(tempName);
			}
			if (!repetitionCheck)
			{
				continue;
			}

			var dataAccess = new DataAccess();

			string oldName = dataAccess.GetStackName(id);

			dataAccess.UpdateStack(stack);

			Console.Clear();
			AnsiConsole.Markup($"Stack with name [#{menuColors.Important1Color.ToHex()}]\"{oldName}\"[/] has been renamed to [#{menuColors.Important2Color.ToHex()}]\"{tempName}\"[/]. Press any button to continue: ");
			Console.ReadKey();
			Console.Clear();
		}
	}	

	private void DeleteStack()
	{
		bool deleteStackLoop = true;
		while (deleteStackLoop)
		{
			var id = ChooseStack("Choose stack to delete:", menuColors.UserInputColor, "There are no stacks to delete at this time:");
			if (id == -1)
				return;

			if (!AnsiConsole.Confirm("Are you sure?"))
			{
				Console.Clear();
				continue;
			}
			var dataAccess = new DataAccess();

			string stackName = dataAccess.GetStackName(id);

			dataAccess.DeleteStack(id);

			Console.Clear();
			AnsiConsole.Markup($"Stack [#{menuColors.NegativeColor.ToHex()}]\"{stackName}\"[/] has been deleted. Press any button to continue: ");
			Console.ReadKey();
			Console.Clear();
		}
	}

	private void AddStack()
	{
		Stack stack = new();

		string dummyName = stack.Name;
		Input.ValidateInput(ref dummyName, "Insert the stack's name: ", ValidationType.Text, menuColors, BackOptions.Exit);

		if (string.IsNullOrEmpty(dummyName))
			return;

		bool repetitionCheck = Input.StackDatabaseRepetitionCheck(dummyName);
		while (!repetitionCheck)
		{
			AnsiConsole.Markup($"[#{menuColors.NegativeColor.ToHex()}]{dummyName}[/] stack name already exists in the database." +
				$" Please select [#{menuColors.Important3Color.ToHex()}]another name[/] for your stack.\n\n");

			dummyName = null;
			Input.ValidateInput(ref dummyName, "Insert the stack's name: ", ValidationType.Text, menuColors, BackOptions.Exit, dontClear: true);

			if (string.IsNullOrEmpty(dummyName))
				return;

			repetitionCheck = Input.StackDatabaseRepetitionCheck(dummyName);
		}

		stack.Name = dummyName;

		var dataAccess = new DataAccess();
		dataAccess.InsertStack(stack);

		Console.Clear();
		AnsiConsole.Markup($"New stack [#{menuColors.Important1Color.ToHex()}]\"{dummyName}\"[/] has been added to database. Press any button to continue: ");
		Console.ReadKey();
		Console.Clear();
	}

	private void ViewStacks()
	{
		Console.Clear(); 

		var dataAccess = new DataAccess();
		var stacks = dataAccess.GetAllStacks();

		if (stacks.Count() == 0)
		{
			Console.Clear();
			var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title("At this time you have no stacks to view:")
			.AddChoices("[grey]Return to previous menu[/]")
			);
			return;
		}

		var stacksArray = stacks.Select(x => x.Name.ToString());

		Spectre.Console.Table table = new Spectre.Console.Table();

		table.AddColumn("Index");
		table.AddColumn("Stack Name");

		int longestInsert = 3;
		int index = 1;
		foreach (string stack in stacksArray)
		{
			if (stack.Length > longestInsert)
			{
				longestInsert = stack.Length;
			}
			table.AddRow(new string[] { index.ToString(), stack });

			index++;
		}

		table.ShowRowSeparators();
		table.BorderStyle = new Style(foreground: menuColors.PositiveColor);
		table.Centered();

		AnsiConsole.Write(table);
		Console.WriteLine();

		string markupText = "Your stacks are displayed in the table above. Press any button to return to previous menu: ";
		int windowWidth = Console.WindowWidth;
		int textStart = (windowWidth - markupText.Length) / 2;

		Console.SetCursorPosition(textStart, Console.CursorTop);
		AnsiConsole.Write(markupText);
		Console.ReadKey();
		Console.Clear();
	}

	/// <returns>Returns -1 if user returns to previous menu without selection.</returns>
	private static int ChooseStack(string message, Color color, string emptyListMessage = null)
	{
		var dataAccess = new DataAccess();
		var stacks = dataAccess.GetAllStacks();

		var stacksArray = stacks.Select(x => x.Name).ToArray();

		string title = message;
		if (stacksArray.Length == 0 && !string.IsNullOrEmpty(emptyListMessage))
		{
			title = emptyListMessage;
		}

		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(title)
			.AddChoices("[grey]Return to previous menu[/]")
			.HighlightStyle(color)
			.AddChoices(stacksArray)
			);

		return stacks.SingleOrDefault(x => x.Name == option)?.Id ?? -1;
	}
}
