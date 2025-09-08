using Dapper;
using Flashcards.Wolfieeex.Model;
using Microsoft.Data.SqlClient;

namespace Flashcards.Wolfieeex.Controller.DataAccess;

internal class DataWriter : DbConnectionProvider
{
	internal void CreateTables()
	{
		try
		{
			using var conn = new SqlConnection(ConnectionString);
			conn.Open();

			string createStackTableSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                CREATE TABLE Stacks (
                    Id INT IDENTITY(1,1) NOT NULL,
                    Name NVARCHAR(30) NOT NULL UNIQUE,
                    PRIMARY KEY (Id)
                );";
			conn.Execute(createStackTableSql);

			string createFlashcardTableSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                CREATE TABLE Flashcards (
                    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    Question NVARCHAR(30) NOT NULL,
                    Answer NVARCHAR(30) NOT NULL,
                    StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(Id) ON DELETE CASCADE ON UPDATE CASCADE
                );";
			conn.Execute(createFlashcardTableSql);

			string createStudySessionTableSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                CREATE TABLE StudySessions (
                    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    Questions INT NOT NULL,
                    Date DATETIME NOT NULL,
                    CorrectAnswers INT NOT NULL,
                    Percentage AS (CorrectAnswers * 100) / Questions PERSISTED,
                    Time TIME NOT NULL,
                    StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(Id) ON DELETE CASCADE ON UPDATE CASCADE
                );";
			conn.Execute(createStudySessionTableSql);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error creating tables: {ex.Message}");
		}
	}

	internal void InsertStack(Stack stack)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		string insertQuery = "INSERT INTO Stacks (Name) VALUES (@Name)";
		connection.Execute(insertQuery, new
		{
			stack.Name
		});
	}

	internal void InsertFlashcard(Flashcard flashcard)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		string insertQuery = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
		connection.Execute(insertQuery, new
		{
			flashcard.Question,
			flashcard.Answer,
			flashcard.StackId
		});
	}

	internal void InsertStudySession(StudySession session)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		string insertQuery = @"
            INSERT INTO StudySessions (Questions, CorrectAnswers, StackId, Time, Date)
            VALUES (@Questions, @CorrectAnswers, @StackId, @Time, @Date)";
		connection.Execute(insertQuery, new
		{
			session.Questions,
			session.CorrectAnswers,
			session.StackId,
			session.Time,
			session.Date
		});
	}

	internal void UpdateStack(Stack stack)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		string updateQuery = "UPDATE Stacks SET Name = @Name WHERE Id = @Id";
		connection.Execute(updateQuery, new
		{
			stack.Name,
			stack.Id
		});
	}

	internal void UpdateFlashcard(int flashcardId, Dictionary<string, object> updateProperties)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();

		string updateQuery = "UPDATE Flashcards SET ";
		var parameters = new DynamicParameters();

		foreach (var kvp in updateProperties)
		{
			updateQuery += $"{kvp.Key} = @{kvp.Key}, ";
			parameters.Add(kvp.Key, kvp.Value);
		}

		updateQuery = updateQuery.TrimEnd(',', ' ') + " WHERE Id = @Id";
		parameters.Add("Id", flashcardId);

		connection.Execute(updateQuery, parameters);
	}

	internal void DeleteStack(int id)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		connection.Execute("DELETE FROM Stacks WHERE Id = @Id", new
		{
			Id = id
		});
	}

	internal void DeleteFlashcard(int id)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		connection.Execute("DELETE FROM Flashcards WHERE Id = @Id", new
		{
			Id = id
		});
	}

	internal void DeleteTables()
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		connection.Execute("DROP TABLE StudySessions");
		connection.Execute("DROP TABLE Flashcards");
		connection.Execute("DROP TABLE Stacks");
	}

	internal void BulkInsertRecords(List<Stack> stacks, List<Flashcard> flashcards)
	{
		using var connection = new SqlConnection(ConnectionString);
		connection.Open();
		using var transaction = connection.BeginTransaction();

		try
		{
			connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", stacks, transaction: transaction);
			connection.Execute("INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)", flashcards, transaction: transaction);
			transaction.Commit();
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}
}
