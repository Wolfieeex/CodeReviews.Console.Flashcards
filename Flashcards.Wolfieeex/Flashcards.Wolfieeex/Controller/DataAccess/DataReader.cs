using Dapper;
using Flashcards.Wolfieeex.Model;
using static Flashcards.Wolfieeex.Model.Enums.ReportSettingsEnums;
using Microsoft.Data.SqlClient;

namespace Flashcards.Wolfieeex.Controller.DataAccess;

internal class DataReader : DbConnectionProvider
{
	internal List<StudySessionDTO> GetStudySessionData()
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();

		string sql = @"
            SELECT s.Name AS StackName, ss.Date, ss.Questions, ss.CorrectAnswers, ss.Percentage, ss.Time
            FROM StudySessions ss
            INNER JOIN Stacks s ON ss.StackId = s.Id
			ORDER BY Date";

		return connection.Query<StudySessionDTO>(sql).ToList();
	}

	internal IEnumerable<Stack> GetAllStacks()
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		return connection.Query<Stack>("SELECT * FROM Stacks");
	}

	internal IEnumerable<Flashcard> GetAllFlashcards(int stackId)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		return connection.Query<Flashcard>(
			"SELECT * FROM Flashcards WHERE StackId = @StackId",
			new
			{
				StackId = stackId
			});
	}

	internal IEnumerable<Flashcard> GetAllFlashcards()
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		return connection.Query<Flashcard>("SELECT * FROM Flashcards");
	}

	internal string GetStackName(int stackId)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		return connection.QueryFirstOrDefault<Stack>(
			"SELECT * FROM Stacks WHERE Id = @StackId",
			new
			{
				StackId = stackId
			})?.Name;
	}

	internal Flashcard GetFlashcard(int stackId, int flashcardId)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		return connection.QueryFirstOrDefault<Flashcard>(
			"SELECT * FROM Flashcards WHERE Id = @FlashcardId AND StackId = @StackId",
			new
			{
				FlashcardId = flashcardId,
				StackId = stackId
			});
	}

	internal string GetFlashcardName(int stackId, int flashcardId)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		return connection.QueryFirstOrDefault<Flashcard>(
			"SELECT * FROM Flashcards WHERE StackId = @StackId AND Id = @FlashcardId",
			new
			{
				StackId = stackId,
				FlashcardId = flashcardId
			})?.Question;
	}

	internal List<List<ReportRow>> ReportToUser(ReportSettings reportSettings)
	{
		var sessions = GetStudySessionData();

		using (var connection = new SqlConnection(ConnectionString))
		{
			connection.Open();

			List<int> years = new();
			foreach (var session in sessions)
			{
				if (!years.Contains(session.Date.Year))
					years.Add(session.Date.Year);
			}

			string reportType = reportSettings.type != ReportType.StudyCount ? "AVG(Percentage)" : "COUNT(*)";
			string period = reportSettings.period switch
			{
				PeriodOptions.ByWeek => "WEEK",
				PeriodOptions.ByMonth => "MONTH",
				PeriodOptions.ByQuarter => "QUARTER",
				PeriodOptions.ByYear => "YEAR",
				_ => throw new ArgumentOutOfRangeException("Period is given in an incorrect format.")
			};

			List<List<ReportRow>> rows = new List<List<ReportRow>>();
			foreach (var year in years)
			{
				string command = @$"SET DATEFIRST 1;
								SELECT DATEPART(YEAR, Date) AS Year, DATENAME({period}, Date) AS Period, {reportType} AS Value
								FROM StudySessions
								WHERE DATEPART(YEAR, Date) = {year}
								GROUP BY DATEPART(YEAR, Date), DATEPART({period}, Date), DATENAME({period}, Date)
								ORDER BY DATEPART({period}, Date)";

				List<ReportRow> reader = connection.Query<ReportRow>(command).ToList();
				rows.Add(reader);
			}
			return rows;
		}
	}
}
