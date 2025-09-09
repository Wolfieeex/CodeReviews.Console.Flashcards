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
            SELECT s.Name as StackName, ss.Date, ss.Questions, ss.CorrectAnswers, ss.Percentage, ss.Time
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

	internal void ReportToUser(ReportSettings reportSettings)
	{
		var sessions = GetStudySessionData();

		using (var connection = new SqlConnection(ConnectionString))
		{
			connection.Open();

			string reportType = reportSettings.type == ReportType.StudyCount ? "AVG(Percentage)" : "COUNT(*)";

			string command = @$"SELECT {reportType} 
								FROM StudySessions
								GROUP BY Date";

			var reader = connection.Query(command);
			string someting = "";
		}
	}
}
