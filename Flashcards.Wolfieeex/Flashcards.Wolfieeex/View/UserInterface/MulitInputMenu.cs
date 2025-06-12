using Spectre.Console;
using System.Collections;
using System.Collections.Immutable;

namespace Flashcards.Wolfieeex.View.UserInterface;

abstract public class MulitInputMenu : Menu
{
	protected Type _selectionType;

	protected Dictionary<Enum, string> inputs = new Dictionary<Enum, string>();

	public MulitInputMenu(Color color) : base(color) {}

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

	abstract public void MenuRunningLoop();

	protected IEnumerable<Enum> GenerateOptions()
	{
		List<Enum> menuSelections = Enum.GetValues(_selectionType).Cast<Enum>().ToList();

		foreach (var enumVal in _selectionType.GetEnumValues())
		{
			// Check keys and values for 2 reasons: 1) Should main option be enabled. 2) To display a current option.
		}

		return menuSelections.ToImmutableList();
	}

	protected string SmartOptionConverter(Enum option)
	{
		if (inputs.ContainsKey(option))
			return GetDisplayName(option) + ": " + inputs[option].ToString();

		else
			return GetDisplayName(option);
	}
}
