using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class ViewStudySessions : Menu
{
	public ViewStudySessions(Color color) : base(color) { }
	public override void DisplayMenu()
	{
		var dataAccess = new DataAccess();
		var sessions = dataAccess.GetStudySessionData();

		Console.Clear();

		if (sessions.Count() == 0)
		{
			Console.Write("Unfortunately, you don't have any study sessions recorded at that time. Go practice some first and then come back to view them. " +
				"Press any key to return to the main menu: ");
			Console.ReadKey();
			Console.Clear();
			return;
		}

		var table = new Table();

		table.AddColumn("Date");
		table.AddColumn("Stack");
		table.AddColumn("Result");
		table.AddColumn("Percentage");
		table.AddColumn("Duration");

		table.ShowRowSeparators = true;
		table.BorderStyle = new Style(foreground: menuColors.PositiveColor);
		table.Centered();


		foreach (var session in sessions)
		{
			table.AddRow(session.Date.ToShortDateString(), session.StackName, $"{session.CorrectAnswers} out of {session.Questions}", $"{session.Percentage}%", session.Time.ToString(@"hh\:mm\:ss"));
		}

		AnsiConsole.Write(table);
		string displayText = "Your table is being displayed above. Press any key to return to the previous menu: ";
		int textLength = displayText.Length;
		int screenWidth = Console.WindowWidth;
		Console.SetCursorPosition((screenWidth - textLength) / 2, Console.CursorTop + 1);
		Console.Write(displayText);
		Console.ReadKey();
		Console.Clear();
	}
}
