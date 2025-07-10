using Flashcards.Wolfieeex.Controller;
using Spectre.Console;
using static Flashcards.Wolfieeex.Model.SelectionEnums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class FlashcardMenu : Menu
{
	public FlashcardMenu() : base(Color.Orange4) { }

	public override void DisplayMenu()
	{
		Console.Clear();

		var menuSelections = Enum.GetValues(typeof(FlashcardChoices)).Cast<FlashcardChoices>();
		
		bool IsMenuRunning = true;
		while (IsMenuRunning)
		{
			DataAccess dataAccess = new DataAccess();
			var stacks = dataAccess.GetAllStacks();
			if (stacks.Count() == 0)
			{
				AnsiConsole.Prompt(
				new SelectionPrompt<string>()
				.Title("You do not have any stacks added to your library, hence you cannot edit your flashcards yet.\n" +
				"Go to the stacks menu, from the main menu and add your first stack:")
				.AddChoices("[grey]Return to previous menu[/]")
				.HighlightStyle(style)
				);
				return;
			}

			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<FlashcardChoices>()
				.Title("What would you like to do today?")
				.AddChoices(menuSelections)
				.UseConverter(s => GetDisplayName(s))
				.HighlightStyle(style)
				.WrapAround()
				);

			try
			{
				switch (userChoice)
				{
					case FlashcardChoices.ViewFlashcards:
						ViewFlashcards();
						break;
					case FlashcardChoices.AddFlashcard:
						AddFlashcard();
						break;
					case FlashcardChoices.DeleteFlashcard:
						DeleteFlashcard();
						break;
					case FlashcardChoices.UpdateFlashcard:
						UpdateFlashcard();
						break;
					case FlashcardChoices.ReturnToMainMenu:
						IsMenuRunning = false;
						break;
				}
			}
			catch (Exception ex)
			{
				Console.Write($"Some functions in Flashcard Menu haven't been enabled yet: {ex.Message}\n");
			}
		}
	}

	

	private void UpdateFlashcard()
	{
		var stackId = ChooseStack("Choose the stack where the flashcard is: ");

		AddFlashcardMenu addFlashcardMenu = new(menuColors.UserInputColor);
		addFlashcardMenu.DisplayMenu();

		/*var flashcardId = ChooseFlashcard("Choose flashcard to update", stackId);

		var propertiesToUpdate = new Dictionary<string, object>();

		if (Ansi)*/
	}

	private void DeleteFlashcard()
	{
		bool mainDeleteMenu = true;
		while (mainDeleteMenu)
		{
			var stackId = ChooseStack("Where is the flashcard you want to delete?:", "There are no stacks to delete your flashcards from: ");
			if (stackId == -1)
			{
				Console.Clear();
				return;
			}

			bool stackSelectedMenu = true;
			while (stackSelectedMenu)
			{
				var dataAccess = new DataAccess();

				var flashcard = ChooseFlashcard("Choose flashcard you want to delete: ", stackId, 
					emptyListMessage: $"There are no flashcards to delete from [#{menuColors.NegativeColor.ToHex()}]\"" +
					$"{dataAccess.GetStackName(stackId)}\"[/] stack:");
				if (flashcard == -1)
				{
					stackSelectedMenu = false;
					Console.Clear();
					continue;
				}

				bool flashcardSelectedMenu = true;
				while (flashcardSelectedMenu)
				{
					if (!AnsiConsole.Confirm("Are you sure?"))
					{
						flashcardSelectedMenu = false;
						Console.Clear();
						continue;
					}

					

					string flashcardName = dataAccess.GetFlashcardName(stackId, flashcard);

					dataAccess.DeleteFlashcard(flashcard);

					flashcardSelectedMenu = false;
					Console.Clear();

					AnsiConsole.Markup($"Flashcard [#{menuColors.NegativeColor.ToHex()}]\"{flashcardName}\"[/] was deleted successfully! Press any button to continue: ");
					Console.ReadKey();
					Console.Clear();
				}
			}
		}
	}

	private void AddFlashcard()
	{
		string dummyInput = "";

		AddFlashcardMenu addFlashcardMenu = new(menuColors.UserInputColor);
		addFlashcardMenu.DisplayMenu();

		if (addFlashcardMenu.Flashcard == null)
			return;

		var dataAccess = new DataAccess();
		dataAccess.InsertFlashcard(addFlashcardMenu.Flashcard);

		Console.Clear();
		AnsiConsole.Markup($"Your new Flashcard [#{menuColors.Important3Color.ToHex()}]\"{addFlashcardMenu.Flashcard.Question}\"[/] " +
			$"in Stack [#{menuColors.Important2Color.ToHex()}]\"{dataAccess.GetStackName(addFlashcardMenu.Flashcard.StackId)}\"[/]" +
			$" has been added! Press any button to continue: ");
		Console.ReadKey();
		Console.Clear();
	}

	public static int ChooseStack(string message, string emptyListMessage = null)
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
			.AddChoices(stacksArray));

		return stacks.SingleOrDefault(x => x.Name == option)?.Id ?? -1;
	}

	/// <returns>Returns -1 if user returns to previous menu without selection.</returns>
	public static int ChooseFlashcard(string message, int stackId, string emptyListMessage = null)
	{
		var dataAccess = new DataAccess();
		var flashcards = dataAccess.GetAllFlashcards(stackId);

		var flashcardsArray = flashcards.Select(x => x.Question).ToArray();

		string title = message;
		if (flashcardsArray.Length == 0 && !string.IsNullOrEmpty(emptyListMessage))
		{
			title = emptyListMessage;
		}

		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(title)
			.AddChoices("[grey]Return to previous menu[/]")
			.AddChoices(flashcardsArray)
			);
			
		return flashcards.SingleOrDefault(x => x.Question == option)?.Id ?? -1;
	}

	private void ViewFlashcards()
	{
		DataAccess dataAccess = new DataAccess();

		bool viewFlashcardsMenuRunning = true;
		while (viewFlashcardsMenuRunning)
		{
			var userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
				.Title("View flashcard menu. Select your option: ")
				.AddChoices(new string[] {"[grey]Return to previous menu[/]",
				"View all flashcards",
				"View flashcards by stack"})
				.HighlightStyle(style)
				);

			switch (userInput)
			{
				case "[grey]Return to previous menu[/]":
					return;
				case "View all flashcards":
					// Get all stacks, loop from each of them to get flashcards from, build data table. If list empty, display message instead.
					break;
				case "View flashcards by stack":
					var stack = ChooseStack("Select a stack from which you want to view your flashcards: ");
					// Retreive flashcards from stack and build a data table. Check for empty case.
					break;

			}
		}
	}
}
