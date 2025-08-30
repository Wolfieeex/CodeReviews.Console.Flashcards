using Spectre.Console;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class ViewStudySessions
{
	internal static void ViewStudyHistory()
	{
		var dataAccess = new DataAccess();
		var sessions = dataAccess.GetStudySessionData();

		var table = new Table();

		table.AddColumn("Date");
		table.AddColumn("Stack");
		table.AddColumn("Result");
		table.AddColumn("Percentage");
		table.AddColumn("Duration");

		foreach (var session in sessions)
		{
			table.AddRow(session.Date.ToShortDateString(), session.StackName, $"{session.CorrectAnswers} out of {session.Questions}", $"{session.Percentage}%", session.Time.ToString());
		}

		AnsiConsole.Write(table);
	}
}
