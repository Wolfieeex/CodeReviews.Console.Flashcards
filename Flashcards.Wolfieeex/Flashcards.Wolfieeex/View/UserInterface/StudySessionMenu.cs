using Spectre.Console;
using Flashcards.Wolfieeex.Model;
using Flashcards.Wolfieeex.Controller;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class StudySessionMenu : Menu
{
	DataAccess dataAccess = new DataAccess();
	public StudySessionMenu(Color color) : base(color) { }

	public override void DisplayMenu()
	{
		bool menuRunning = true;
		if (menuRunning)
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

				StudySession studySession = new StudySession();
				studySession.Questions = shuffledFlashcards.Count();
				studySession.Date = DateTime.Now;
				studySession.StackId = stackId;
				studySession.CorrectAnswers = 0;

				while (shuffledFlashcards.Count() > 0)
				{
					Flashcard flashc = shuffledFlashcards[0];

					Console.Clear();
					string input = "";
					Input.ValidateInput(ref input, $"Translate [#{menuColors.Important2Color.ToHex()}]{flashc.Question}[/]:"
						, InputValidationEnums.ValidationType.AnyNonBlank, menuColors, InputValidationEnums.BackOptions.Exit);

					if (input.ToLower() == "e")
					{
						Console.Clear();
						if (!AnsiConsole.Confirm("Are you sure you want to end your study session? Your progress will still be added to the study history table:"))
						{
							Console.Clear();
							continue;
						}
						else
						{
							// Finish session here
							return;
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


					shuffledFlashcards.RemoveAt(0);
				}

				//	Give score and update session to history
				stackSelected = false;
			}
		}
	}
}
