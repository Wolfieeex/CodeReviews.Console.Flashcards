using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
}
