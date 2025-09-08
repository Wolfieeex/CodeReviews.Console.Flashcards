using Spectre.Console;
using Flashcards.Wolfieeex.Model;
using Flashcards.Wolfieeex.Controller;
using Flashcards.Wolfieeex.Controller.DataAccess;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class StudySessionMenu : Menu
{
	DataAccessor dataAccess = new DataAccessor();
	StudySession studySession = new();
	public StudySessionMenu(Color color) : base(color) { }

	public override void DisplayMenu()
	{
		bool menuRunning = true;
		while (menuRunning)
		{
			Console.Clear();
			int stackId = StacksMenu.ChooseStack("Select stack you want to study:", menuColors.TitleColor,
				$"You don't have any stacks you can study at the moment:");
			if (stackId == -1)
				return;

			bool stackSelected = true;
			while (stackSelected)
			{
				Console.Clear();
				var flashcards = dataAccess.GetAllFlashcards(stackId);
				if (flashcards.ToList().Count() == 0)
				{
					Console.Clear();
					AnsiConsole.Markup($"At the moment stack [#{menuColors.TitleColor.ToHex()}]{dataAccess.GetStackName(stackId)}[/] is empty. Add some flashcards to it to start studying. " +
						$"Press any button to select another stack: ");
					Console.ReadKey();
					Console.Clear();
					stackSelected = false;

				}

				Random random = new Random();
				var shuffledFlashcards = flashcards.OrderBy(x => random.Next()).ToList();

				studySession = new StudySession();
				studySession.Questions = shuffledFlashcards.Count();
				studySession.Date = DateTime.Now;
				studySession.StackId = stackId;
				studySession.CorrectAnswers = 0;
				int finishedCards = 0;

				while (shuffledFlashcards.Count() > 0)
				{
					Flashcard flashc = shuffledFlashcards[0];

					Console.Clear();
					string input = "";
					Input.ValidateInput(ref input, $"Translate [#{menuColors.Important2Color.ToHex()}]{flashc.Question}[/]:"
						, InputValidationEnums.ValidationType.AnyNonBlank, menuColors, InputValidationEnums.BackOptions.Exit);

					if (input.ToLower() == "")
					{
						Console.Clear();
						string anyProgress = finishedCards > 0 ? " Your progress will still be added to the study history table" : "";
						if (!AnsiConsole.Confirm($"Are you sure you want to end your study session?{anyProgress}:"))
						{
							Console.Clear();
							continue;
						}
						else
						{
							if (finishedCards == 0)
							{
								Console.Clear();
								stackSelected = false;
								break;
							}
							studySession.Questions = finishedCards;
							FinishSession(finishedCards, false);
							stackSelected = false;
							break;
						}
					}

					if (string.Equals(input.Trim(), flashc.Answer, StringComparison.OrdinalIgnoreCase))
					{
						studySession.CorrectAnswers++;
						AnsiConsole.Markup($"Well done! This is a correct answer! Press any key to continue: ");
						Console.ReadKey();
						Console.Clear();	
					}
					else
					{
						AnsiConsole.Markup($"Unfortunately, [#{menuColors.NegativeColor.ToHex()}]{flashc.Answer}[/] is the correct answer. Press any key to continue: ");
						Console.ReadKey();
						Console.Clear();
					}

					finishedCards++;
					shuffledFlashcards.RemoveAt(0);
				}

				if (stackSelected)
				{
					FinishSession(studySession.Questions, true);
					stackSelected = false;
				}
			}
		}
	}

	private void FinishSession(int questions, bool isFullSession)
	{
		Console.Clear();

		studySession.Time = DateTime.Now - studySession.Date;

		dataAccess.InsertStudySession(studySession);
		AnsiConsole.Markup($"You have finished your [#{menuColors.Important1Color.ToHex()}]{dataAccess.GetStackName(studySession.StackId)}[/] study session {(isFullSession ? "" : "early ")}" +
			$"and you scored [#{menuColors.Important3Color.ToHex()}]{studySession.CorrectAnswers}/{questions} ({Math.Round((double)studySession.CorrectAnswers / questions * 10000) / 100}% correct)[/]. Press any key to return to the previous menu: ");
		Console.ReadKey();
		Console.Clear();
	}
}
