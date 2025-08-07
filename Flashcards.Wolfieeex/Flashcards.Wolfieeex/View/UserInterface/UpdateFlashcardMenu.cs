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
		DataAccess dataAccess = new DataAccess();

		bool updateFlashcardLoop = true;
		while (updateFlashcardLoop)
		{
			int stackId = FlashcardMenu.ChooseStack("Select from which stack you would like to update: ", menuColors.UserInputColor,
				"At this time, there is nothig to update. Check back later.");
			if (stackId == -1)
				return;

			bool isStackSelected = true;
			while (isStackSelected)
			{
				int flashcardId = FlashcardMenu.ChooseFlashcard($"Select which flashcard you would like to update from " +
					$"[#{menuColors.PositiveColor.ToHex()}]\"{dataAccess.GetStackName(stackId)}\"[/] stack:", stackId,
					menuColors.UserInputColor, "There are no flashcards at " +
					$"[#{menuColors.PositiveColor.ToHex()}]\"{dataAccess.GetStackName(stackId)}\"[/] stack. ");
				if (flashcardId == -1)
				{
					isStackSelected = false;
					continue;
				}

				// No table to be displayed while no flashcards ther are for display
				// After adding flashcard don't go completely back to flashcard menu- let the user add another one.

				// Menu pops up
				// Update the details
				// Magic happens!

				isStackSelected = false;
			}
		}
	}
}
