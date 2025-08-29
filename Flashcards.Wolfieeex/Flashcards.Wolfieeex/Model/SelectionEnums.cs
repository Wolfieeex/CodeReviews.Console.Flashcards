using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Wolfieeex.Model;

internal class SelectionEnums
{
	internal enum MainMenuChoices
	{
		[Display(Name ="Manage Stacks")]
		ManageStacks,

		[Display(Name ="Manage Flashcards")]
		ManageFlashcards,

		[Display(Name ="Study Session")]
		StudySession,

		[Display(Name = "Study History")]
		StudyArea,

		[Display(Name ="Quit")]
		Quit
	}

	internal enum StacksChoices
	{
		[Display(Name = "View Stacks")]
		ViewStacks,

		[Display(Name = "Add Stack")]
		AddStack,

		[Display(Name = "Delete Stack")]
		DeleteStack,

		[Display(Name = "Update Stack")]
		UpdateStack,

		[Display(Name = "Return To Main Menu")]
		ReturnToMainMenu
	}

	internal enum FlashcardChoices
	{
		[Display(Name = "View Flashcards")]
		ViewFlashcards,

		[Display(Name = "Add Flashcard")]
		AddFlashcard,

		[Display(Name = "Delete Flashcard")]
		DeleteFlashcard,

		[Display(Name = "Update Flashcard")]
		UpdateFlashcard,

		[Display(Name = "Return To Main Menu")]
		ReturnToMainMenu
	}

	internal enum FlashcardViewOptions
	{
		[Display(Name = "Return To Main Menu")]
		ReturnToPreviousMenu,

		[Display(Name = "View all Flashcards")]
		ViewAllFlashcards,

		[Display(Name = "View Flashcards by Stack")]
		ViewFlashcardsByStack
	}
}
