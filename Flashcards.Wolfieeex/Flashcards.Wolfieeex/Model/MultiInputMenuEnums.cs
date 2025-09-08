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
		[Display(Name = "Confirm and update Flashcard")]
		[EnumLabelSpecialLabel(SpecialLabels.Confirm)]
		Confirm,

		[Display(Name = "Return to previous menu")]
		[EnumLabelSpecialLabel(SpecialLabels.Quit)]
		ReturnToPreviousMenu,

		[Description("Select a new question for your flashcard: ")]
		[Display(Name = "Update Question")]
		[EnumLabelSpecialLabel(SpecialLabels.OneOf)]
		UpdateQuestion,

		[Description("Select a new answer for your flashcard: ")]
		[Display(Name = "Update Answer")]
		[EnumLabelSpecialLabel(SpecialLabels.OneOf)]
		UpdateAnswer,

		[Description("Select a new name for your flashcard")]
		[Display(Name = "Update Stack")]
		[EnumLabelSpecialLabel(SpecialLabels.OneOf)]
		UpdateStack,
	}

	public enum ReportingMenuOptions
	{
		[Display(Name = "Confirm")]
		[EnumLabelSpecialLabel(SpecialLabels.Confirm)]
		Confirm,

		[Display(Name = "Return to the main menu")]
		[EnumLabelSpecialLabel(SpecialLabels.Quit)]
		Return,

		[Description("Select what do you want to report:")]
		[Display(Name = "")]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		ReportOutput,

		[Description("Select by which period you want to display your data: ")]
		[Display(Name = "Report average percentage of your sessions")]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		PeriodSelection,

		[Description("Do you want to hide columns with no data?: ")]
		[Display(Name = "Display Settings")]
		[EnumLabelSpecialLabel(SpecialLabels.NonOptional)]
		Display,
	}
}
