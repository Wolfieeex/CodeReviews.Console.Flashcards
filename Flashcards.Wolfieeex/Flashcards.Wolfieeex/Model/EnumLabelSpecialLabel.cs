namespace Flashcards.Wolfieeex.Model
{
	public enum SpecialLabels
	{
		Quit,
		Confirm,
		NonOptional,
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
