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

	protected override void DisplayMenu()
	{
		bool IsMenuRunning = true;
		while (IsMenuRunning)
		{
			Console.Clear();

			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<StacksChoices>()
				.Title("What would you like to do today?")
				.AddChoices(
					StacksChoices.ViewStacks,
					StacksChoices.AddStack,
					StacksChoices.UpdateStack,
					StacksChoices.DeleteStack,
					StacksChoices.ReturnToMainMenu)
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
		var stack = new Stack();

		stack.Id = ChooseStack("Choose stack you want to update: ");

		string tempName = stack.Name;
		Input.ValidateInput(ref tempName, "Insert a new name of your stack: ", ValidationType.AnyNonBlank, menuColors, BackOptions.Exit);
		stack.Name = tempName;

		var dataAccess = new DataAccess();
		dataAccess.UpdateStack(stack);
	}	

	private void DeleteStack()
	{
		var id = ChooseStack("Choose stack to delete:");

		if (!AnsiConsole.Confirm("Are you sure?"))
			return;

		var dataAccess = new DataAccess();
		dataAccess.DeleteStack(id);
	}

	private void AddStack()
	{
		Stack stack = new();

		string dummyName = stack.Name;
		Input.ValidateInput(ref dummyName, "Insert the stack's name: ", ValidationType.Text, menuColors, BackOptions.Exit);
		stack.Name = dummyName;

		var dataAccess = new DataAccess();
		dataAccess.InsertStack(stack);
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
			.AddChoices(stacksArray));

		return stacks.Single(x => x.Name == option)?.Id ?? -1;
	}
}
