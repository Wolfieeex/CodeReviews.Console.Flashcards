using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Wolfieeex.Model;

internal class MultiInputMenuEnums
{
	public enum InsertFlashcardSelection
	{
		[EnumLabelSpecialLabel(SpecialLabels.Confirm)]
		[Display(Name = "Confirm and insert a new Flashcard!")]
		Confirm,

		[EnumLabelSpecialLabel(SpecialLabels.Quit)]
		[Display(Name = "Return to previous menu")]
		ReturnToPreviousMenu,

		[EnumLabelInputType(InputType.Question)]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		[Description("Construct a question for your flashcard: ")]
		[Display(Name = "Choose Question")]
		ChooseQuestion,

		[EnumLabelInputType(InputType.Question)]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		[Description("What's the answer for your question?: ")]
		[Display(Name = "Choose Answer")] 
		ChooseAnswer,

		[EnumLabelInputType(InputType.Selection)]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		[Description("Choose which stack it belongs to: ")]
		[Display(Name = "Choose Stack")]
		ChooseStack,
	}

	public enum UpdateFlashcardSelection
	{
		[EnumLabelSpecialLabel(SpecialLabels.Confirm)]
		[Display(Name = "Confirm and insert a new Flashcard!")]
		Confirm,

		[EnumLabelSpecialLabel(SpecialLabels.Quit)]
		[Display(Name = "Return to previous menu")]
		ReturnToPreviousMenu,

		[EnumLabelInputType(InputType.Question)]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		[Display(Name = "Update Question")]
		UpdateQuestion,

		[EnumLabelInputType(InputType.Question)]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		[Display(Name = "Update Answer")]
		UpdateAnswer,

		[EnumLabelInputType(InputType.Selection, )]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		[Display(Name = "Update Stack")]
		UpdateStack,
	}
}
