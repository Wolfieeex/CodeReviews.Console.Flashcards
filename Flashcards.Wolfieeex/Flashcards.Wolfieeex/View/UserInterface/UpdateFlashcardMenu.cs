using Flashcards.Wolfieeex.Model;
using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class UpdateFlashcardMenu : MulitInputMenu
{
	public UpdateFlashcardMenu(Color color) : base(color)
	{
		_selectionType = typeof(MultiInputMenuEnums.InsertFlashcardSelection);
	}
	protected override void MenuRunningLoop()
	{
		// Choose stack
		// Chose flashcard
		// Load the flashcard
		// Menu pops up
		// Update the details
	}
}
