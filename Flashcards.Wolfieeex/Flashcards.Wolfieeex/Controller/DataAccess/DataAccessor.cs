using Flashcards.Wolfieeex.Model;

namespace Flashcards.Wolfieeex.Controller.DataAccess;

internal class DataAccessor
{
	private readonly DataReader _reader = new DataReader();
	private readonly DataWriter _writer = new DataWriter();

	internal List<StudySessionDTO> GetStudySessionData() => _reader.GetStudySessionData();
	internal IEnumerable<Stack> GetAllStacks() => _reader.GetAllStacks();
	internal IEnumerable<Flashcard> GetAllFlashcards(int stackId) => _reader.GetAllFlashcards(stackId);
	internal IEnumerable<Flashcard> GetAllFlashcards() => _reader.GetAllFlashcards();
	internal string GetStackName(int stackId) => _reader.GetStackName(stackId);
	internal Flashcard GetFlashcard(int stackId, int flashcardId) => _reader.GetFlashcard(stackId, flashcardId);
	internal string GetFlashcardName(int stackId, int flashcardId) => _reader.GetFlashcardName(stackId, flashcardId);
	internal List<dynamic> ReportToUser(ReportSettings settings) => _reader.ReportToUser(settings);

	internal void CreateTables() => _writer.CreateTables();
	internal void InsertStack(Stack stack) => _writer.InsertStack(stack);
	internal void InsertFlashcard(Flashcard flashcard) => _writer.InsertFlashcard(flashcard);
	internal void UpdateStack(Stack stack) => _writer.UpdateStack(stack);
	internal void UpdateFlashcard(int flashcardId, Dictionary<string, object> updateProperties) =>
		_writer.UpdateFlashcard(flashcardId, updateProperties);
	internal void InsertStudySession(StudySession session) => _writer.InsertStudySession(session);
	internal void DeleteStack(int id) => _writer.DeleteStack(id);
	internal void DeleteFlashcard(int id) => _writer.DeleteFlashcard(id);
	internal void DeleteTables() => _writer.DeleteTables();
	internal void BulkInsertRecords(List<Stack> stacks, List<Flashcard> flashcards) =>
		_writer.BulkInsertRecords(stacks, flashcards);
	internal void BulkInsertSessions(List<StudySession> sessions) => _writer.BulkInsertSessions(sessions);
}
