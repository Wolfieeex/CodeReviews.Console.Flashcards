using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Wolfieeex.Model;

internal class MultiInputMenuEnums
{
	public enum InsertFlashcardSelection
	{
		[Display(Name = "Confirm and insert a new Flashcard!")]
		[EnumLabelSpecialLabel(SpecialLabels.Confirm)]
		Confirm,

		[Display(Name = "Return to previous menu")]
		[EnumLabelSpecialLabel(SpecialLabels.Quit)]
		ReturnToPreviousMenu,

		[Description("Construct a question for your flashcard: ")]
		[Display(Name = "Choose Question")]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		ChooseQuestion,

		[Description("What's the answer for your question?: ")]
		[Display(Name = "Choose Answer")]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		ChooseAnswer,

		[Description("Choose which stack it belongs to: ")]
		[Display(Name = "Choose Stack")]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		ChooseStack,
	}

	public enum UpdateFlashcardSelection
	{
		[Display(Name = "Confirm and insert a new Flashcard!")]
		[EnumLabelSpecialLabel(SpecialLabels.Confirm)]
		Confirm,

		[Display(Name = "Return to previous menu")]
		[EnumLabelSpecialLabel(SpecialLabels.Quit)]
		ReturnToPreviousMenu,

		[Display(Name = "Update Question")]
		UpdateQuestion,

		[Display(Name = "Update Answer")]
		UpdateAnswer,

		[Display(Name = "Update Stack")]
		UpdateStack,
	}
}
