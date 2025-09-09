namespace Flashcards.Wolfieeex.Model.Enums
{
	public enum SpecialLabels
	{
		Quit,
		Confirm,
		NonOptional,
		OneOf,
		None
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class EnumLabelSpecialLabel : Attribute
	{
		public SpecialLabels Label { get; }
		public EnumLabelSpecialLabel(SpecialLabels label)
		{
			Label = label;
		}
	}
}
