using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Model;
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
			var stack = new Stack();

			var id = ChooseStack("Choose stack you want to update: ");
			if (id == -1)
				return;

			stack.Id = id;

			string tempName = stack.Name;
			Input.ValidateInput(ref tempName, "Insert a new name of your stack: ", ValidationType.AnyNonBlank, menuColors, BackOptions.Exit);
			stack.Name = tempName;

			if (string.IsNullOrEmpty(tempName))
			{
				Console.Clear();
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
			var id = ChooseStack("Choose stack to delete:");
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
		stack.Name = dummyName;

		if (string.IsNullOrEmpty(dummyName))
			return;

		var dataAccess = new DataAccess();
		dataAccess.InsertStack(stack);

		Console.Clear();
		AnsiConsole.Markup($"New stack [#{menuColors.Important1Color.ToHex()}]\"{dummyName}\"[/] has been added to database. Press any button to continue: ");
		Console.ReadKey();
		Console.Clear();
	}

	private void ViewStacks()
	{
		throw new NotImplementedException();
	}

	/// <returns>Returns -1 if user returns to previous menu without selection.</returns>
	private static int ChooseStack(string message)
	{
		var dataAccess = new DataAccess();
		var stacks = dataAccess.GetAllStacks();

		var stacksArray = stacks.Select(x => x.Name).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(message)
			.AddChoices("[grey]Return to previous menu[/]")
			.AddChoices(stacksArray)
			);

		return stacks.SingleOrDefault(x => x.Name == option)?.Id ?? -1;
	}
}
