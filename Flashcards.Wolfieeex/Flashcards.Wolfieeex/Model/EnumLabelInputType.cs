using System.Collections;

namespace Flashcards.Wolfieeex.Model
{
	public enum InputType
	{
		Question,
		Selection,
		None
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class EnumLabelInputType : Attribute
	{
		public InputType Label { get; }
		public EnumLabelInputType(InputType label)
		{
			Label = label;
		}
	}
}
