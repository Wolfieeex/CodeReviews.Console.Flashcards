using System.ComponentModel;

namespace Flashcards.Wolfieeex.Model;

internal class MultiInputMenuEnums
{
	public enum InsertFlashcardSelection
	{
		[Description("Return to previous menu")]
		ReturnToPreviousMenu,
		[Description("Choose Question")]
		UpdateQuestion,
		[Description("Choose Answer")] 
		UpdateStack,
		[Description("Choose Stack")]
		ChooseStack,
	}

	public enum UpdateFlashcardSelection
	{
		[Description("Return to previous menu")]
		ReturnToPreviousMenu,
		[Description("Update Question")]
		UpdateQuestion,
		[Description("Update Answer")]
		UpdateAnswer,
		[Description("Update Stack")]
		UpdateStack,
	}
}
