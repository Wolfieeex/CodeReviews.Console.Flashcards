using Flashcards.Wolfieeex.Controller.DataAccess;
using Flashcards.Wolfieeex.Controller.ProgramSetup;
using Flashcards.Wolfieeex.View.UserInterface;

var DataAccessor = new DataAccessor();
DataAccessor.DeleteTables();
DataAccessor.CreateTables();
SeedData.SeedRecords();
MainMenu mainMenu = new();
mainMenu.DisplayMenu();
