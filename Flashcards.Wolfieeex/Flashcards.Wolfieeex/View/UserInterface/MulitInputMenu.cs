using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class MulitInputMenu : Menu
{
	private readonly Type _selectionType;
	private readonly string _title;

	private Dictionary<Enum, string> inputs;

	MulitInputMenu(Color color, Type EnumSelectionType, string title) : base(color)
	{
		_selectionType = EnumSelectionType;
		_title = title;
	}

	protected override void DisplayMenu()
	{
		try
		{
			if (!_selectionType.IsEnum)
				throw new ArgumentException("EnumSelectionType parameter must be of type Enum.");

			List<string> menuOptions = Enum.GetNames(_selectionType).ToList();

			bool menuIsRunning = true;
			while (menuIsRunning)
			{
				Console.Clear();

				string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
					.Title(_title)
					.AddChoices(menuOptions)
					.HighlightStyle(style)
					.WrapAround()
					);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"There was an error while running Multi Input Menu with {_selectionType} type: {ex.Message}");
		}
	}
}
