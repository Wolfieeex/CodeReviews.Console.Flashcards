namespace Flashcards.Wolfieeex.Model.Enums;

internal class InputValidationEnums
{
	public enum ValidationType
	{
		Any,
		AnyNonBlank,
		Text,
		Integer,
		DateTime,
		TimeSpan
	}

	public enum BackOptions
	{
		None,
		Blank,
		Exit,
		ExitBlank,
	}
}
