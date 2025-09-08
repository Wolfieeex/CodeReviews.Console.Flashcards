using Microsoft.Extensions.Configuration;

namespace Flashcards.Wolfieeex.Controller.DataAccess;

internal abstract class DbConnectionProvider
{
	protected readonly string ConnectionString;

	protected DbConnectionProvider()
	{
		IConfiguration configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
	}
}
