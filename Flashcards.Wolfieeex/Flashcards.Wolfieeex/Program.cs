using Flashcards.Wolfieeex.View.UserInterface;

var DataAccess = new DataAccess();
DataAccess.CreateTables();

MainMenu mainMenu = new();
mainMenu.DisplayMenu();
