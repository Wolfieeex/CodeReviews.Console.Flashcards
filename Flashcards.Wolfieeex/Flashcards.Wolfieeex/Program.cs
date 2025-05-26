using Flashcards.Wolfieeex.Controller.ProgramSetup;
using Flashcards.Wolfieeex.View.UserInterface;

var DataAccess = new DataAccess();
DataAccess.DeleteTables();
DataAccess.CreateTables();
SeedData.SeedRecords();
MainMenu mainMenu = new();
mainMenu.DisplayMenu();
