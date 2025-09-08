using Flashcards.Wolfieeex.Model;
using Flashcards.Wolfieeex.Controller.DataAccess;

namespace Flashcards.Wolfieeex.Controller.ProgramSetup;

internal class SeedData
{
	internal static void SeedRecords()
	{
		List<Stack> stacks = new()
		{
			new Stack { Name = "French" },
			new Stack { Name = "Italian" },
			new Stack { Name = "Norwegian" },
			new Stack { Name = "German" },
			new Stack { Name = "Spanish" }
		};

		List<Flashcard> flashcards = new()
		{
			new Flashcard { StackId = 1, Question = "Oui", Answer = "Yes" },
			new Flashcard { StackId = 2, Question = "Ciao", Answer = "Hi" },
			new Flashcard { StackId = 3, Question = "Ja", Answer = "Yes" },
			new Flashcard { StackId = 4, Question = "Nein", Answer = "No" },
			new Flashcard { StackId = 5, Question = "Hola", Answer = "Hi" },
			new Flashcard { StackId = 1, Question = "Bonjour", Answer = "Good morning" },
			new Flashcard { StackId = 2, Question = "Buongiorno", Answer = "Good morning" },
			new Flashcard { StackId = 3, Question = "God morgen", Answer = "Good morning" },
			new Flashcard { StackId = 4, Question = "Guten Morgen", Answer = "Good morning" },
			new Flashcard { StackId = 5, Question = "Buenos días", Answer = "Good morning" },
			new Flashcard { StackId = 1, Question = "Merci", Answer = "Thank you" },
			new Flashcard { StackId = 2, Question = "Grazie", Answer = "Thank you" },
			new Flashcard { StackId = 3, Question = "Takk", Answer = "Thank you" },
			new Flashcard { StackId = 4, Question = "Danke", Answer = "Thank you" },
			new Flashcard { StackId = 5, Question = "Gracias", Answer = "Thank you" }
		};

		var dataAccess = new DataAccessor();
		dataAccess.BulkInsertRecords(stacks, flashcards);
	}
}
