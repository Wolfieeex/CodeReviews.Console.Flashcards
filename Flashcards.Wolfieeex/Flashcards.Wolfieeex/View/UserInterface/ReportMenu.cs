using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using static Flashcards.Wolfieeex.Model.Enums.ReportSettingsEnums;
using static Flashcards.Wolfieeex.Model.Enums.MultiInputMenuEnums;
using Flashcards.Wolfieeex.Controller.DataAccess;
using Flashcards.Wolfieeex.Model.Enums;

namespace Flashcards.Wolfieeex.View.UserInterface;

internal class ReportMenu : MultiInputMenu
{
	public ReportMenu(Color color) : base(color)
	{
		_selectionType = typeof(MultiInputMenuEnums.ReportingMenuOptions);
	}
	DataAccessor dataAccess = new DataAccessor();
	private ReportSettings reportSettings = new ReportSettings();

	protected override void MenuRunningLoop()
	{
		Console.Clear();

		var sessions = dataAccess.GetStudySessionData();

		if (sessions.Count == 0)
		{
			Console.WriteLine("Unfortunately, at this time it is not possible to run your report. There are no study sessions recorded yet.\n" +
				"Press any button to return to a previous menu: ");
			Console.ReadKey();
			Console.Clear();
			return;
		}

		inputs.Add(ReportingMenuOptions.Display, GetDescription(DisplayOptions.HideEmptyColumns));
		reportSettings.display = DisplayOptions.HideEmptyColumns;

		bool mainLoop = true;
		while (mainLoop)
		{
			var userInput = AnsiConsole.Prompt(new SelectionPrompt<Enum>()
				.Title("Please select how you would like to run your report: ")
				.AddChoices(GenerateOptions())
				.UseConverter(s => SmartOptionConverter(s))
				.HighlightStyle(style)
				.WrapAround());

			Enum enumInput;
			string reportOption;
			switch (userInput)
			{
				case MultiInputMenuEnums.ReportingMenuOptions.Confirm:
					RunReport();
					break;
				case MultiInputMenuEnums.ReportingMenuOptions.Return:
					Console.Clear();
					return;
				case MultiInputMenuEnums.ReportingMenuOptions.ReportOutput:
					ReportOptionSelector(ReportingMenuOptions.ReportOutput, typeof(ReportType));
					break;
				case MultiInputMenuEnums.ReportingMenuOptions.PeriodSelection:
					ReportOptionSelector(ReportingMenuOptions.PeriodSelection, typeof(PeriodOptions));
					break;
				case MultiInputMenuEnums.ReportingMenuOptions.Display:
					ReportOptionSelector(ReportingMenuOptions.Display, typeof(DisplayOptions));
					break;
			}
		}
	}

	private void ReportOptionSelector(Enum reportSetting, Type selection)
	{
		Console.Clear();

		var selectionValues = Enum.GetValues(selection).Cast<Enum>().ToList();

		var selectorInput = AnsiConsole.Prompt(new SelectionPrompt<Enum>()
			.WrapAround()
			.HighlightStyle(style)
			.AddChoices(selectionValues)
			.UseConverter(s => GetDescription(s))
			.Title(GetDescription(reportSetting)));

		if (Convert.ToInt32(selectorInput) == 0) return;

		if (inputs.ContainsKey(reportSetting))
		{
			inputs.Remove(reportSetting);
		}
		inputs.Add(reportSetting, GetDescription(selectorInput));

		switch (reportSetting)
		{
			case ReportingMenuOptions.ReportOutput:
				reportSettings.type = (ReportType)selectorInput;
				break;
			case ReportingMenuOptions.Display:
				reportSettings.display = (DisplayOptions)selectorInput;
				break;
			case ReportingMenuOptions.PeriodSelection:
				reportSettings.period = (PeriodOptions)selectorInput;
				break;
		}
	}

	private void RunReport()
	{
		dataAccess.ReportToUser(reportSettings);
	}
}
