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

		List<StudySession> sessions = new();
		long startTick = new DateTime(2023, 01, 01).Ticks;
		long endTick = DateTime.Now.Ticks;
		int numberOfSeeds = 500;
		Random random = new Random();

		for (int i = 0; i < 500; i++)
		{
			int stackId;
			DateTime randomDate;
			int questions;
			int correctAnswers;
			int percentage;
			TimeSpan timeSpan;

			stackId = random.Next(1, 6);

			double tNumber = random.NextDouble();
			tNumber = Math.Pow(tNumber, 0.3);
			long lerpedTickValue = (long)Math.Round(startTick + (endTick - startTick) * tNumber);
			randomDate = new DateTime(lerpedTickValue);

			questions = random.Next(1, 4);
			correctAnswers = random.Next(0, questions + 1);

			int seconds = random.Next(15, 61);
			timeSpan = TimeSpan.FromSeconds(seconds);

			StudySession studySession = new StudySession()
			{
				StackId = stackId,
				Date = randomDate,
				Questions = questions,
				CorrectAnswers = correctAnswers,
				Time = timeSpan
			};
			sessions.Add(studySession);
		}
		dataAccess.BulkInsertSessions(sessions);
	}
}
