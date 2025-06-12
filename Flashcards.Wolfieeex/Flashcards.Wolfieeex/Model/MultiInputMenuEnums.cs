using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Wolfieeex.Model;

internal class MultiInputMenuEnums
{
	public enum InsertFlashcardSelection
	{
		[Display(Name = "Confirm and insert a new Flashcard!")]
		Confirm,

		[Display(Name = "Return to previous menu")]
		ReturnToPreviousMenu,

		[Description("Construct a question for your flashcard: ")]
		[Display(Name = "Choose Question")]
		ChooseQuestion,

		[Description("What's the answer for your question?: ")]
		[Display(Name = "Choose Answer")] 
		ChooseAnswer,

		[Description("Choose which stack it belongs to: ")]
		[Display(Name = "Choose Stack")]
		ChooseStack,
	}

	public enum UpdateFlashcardSelection
	{
		[Display(Name = "Confirm and insert a new Flashcard!")]
		Confirm,

		[Display(Name = "Return to previous menu")]
		ReturnToPreviousMenu,

		[Display(Name = "Update Question")]
		UpdateQuestion,

		[Display(Name = "Update Answer")]
		UpdateAnswer,

		[Display(Name = "Update Stack")]
		UpdateStack,
	}
}
