using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Flashcards.Wolfieeex.Model;

internal class DataAccess
{
    IConfiguration configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json")
        .Build();

    private string ConnectionString;

    public DataAccess()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    internal void CreateTables()
    {
        try
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string createStackTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
					CREATE TABLE Stacks (
						Id INT IDENTITY(1,1) NOT NULL,
						Name NVARCHAR(30) NOT NULL UNIQUE,
						PRIMARY KEY (Id)
					);";
                conn.Execute(createStackTableSql);

                string createFlashcardTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
					CREATE TABLE Flashcards (
					Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
					Question NVARCHAR(30) NOT NULL,
					Answer NVARCHAR(30) NOT NULL,
					StackId INT NOT NULL
						FOREIGN KEY
						REFERENCES Stacks(Id)
						ON DELETE CASCADE
						ON UPDATE CASCADE
					);";
                conn.Execute(createFlashcardTableSql);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An Error ocurred whilst creating datatables: {ex.Message}");
        }
    }

    internal void InsertStack(Stack stack)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO Stacks (Name) VALUES (@Name)";
                connection.Execute(insertQuery, new { stack.Name });
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"There was a problem while inserting a stack: {ex.Message}");
        }
    }

    internal IEnumerable<Stack> GetAllStacks()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM stacks";
                var records = connection.Query<Stack>(selectQuery);
                return records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem while retrieving the stacks: {ex.Message}");
            return new List<Stack>();
        }
    }

    internal void InsertFlashcard(Flashcard flashcard)
    {
        try
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string insertQuery = @"
                INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
                conn.Execute(insertQuery, new { flashcard.Question, flashcard.Answer, flashcard.StackId });
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem while inserting a flashcard: {ex.Message}");
        }
    }
}
