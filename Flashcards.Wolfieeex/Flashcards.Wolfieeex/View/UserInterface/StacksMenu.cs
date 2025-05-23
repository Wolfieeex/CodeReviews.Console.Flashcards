using Spectre.Console;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class StacksMenu : Menu
{
	StacksMenu() : base(Color.Green3_1) { }

	public override void DisplayMenu()
	{
		bool IsMenuRunning = true;

		while (IsMenuRunning)
		{
			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<StacksChoices>()
				.Title("What would you like to do today?")
				.AddChoices(
					StacksChoices.ViewStacks,
					StacksChoices.AddStack,
					StacksChoices.UpdateStack,
					StacksChoices.DeleteStack,
					StacksChoices.ReturnToMainMenu)
				);

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
	}

	private void UpdateStack()
	{
		throw new NotImplementedException();
	}

	private void DeleteStack()
	{
		throw new NotImplementedException();
	}

	private void AddStack()
	{
		throw new NotImplementedException();
	}

	private void ViewStacks()
	{
		throw new NotImplementedException();
	}
}
