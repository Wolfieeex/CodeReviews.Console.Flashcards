using Flashcards.Wolfieeex.Controller.DataAccess;
using Flashcards.Wolfieeex.Model;
using Flashcards.Wolfieeex.Model.Enums;
using Spectre.Console;
using System.Globalization;
using static Flashcards.Wolfieeex.Model.Enums.MultiInputMenuEnums;
using static Flashcards.Wolfieeex.Model.Enums.ReportSettingsEnums;

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
		var rows = dataAccess.ReportToUser(reportSettings);

		Table reportTable = new Table();
		reportTable.BorderStyle = new Style(foreground: menuColors.Important2Color);
		reportTable.ShowRowSeparators = true;
		reportTable.Title("Report");


		// The extract function will not work for months, which are the actual names of the months.
		if (reportSettings.display == ReportSettingsEnums.DisplayOptions.HideEmptyColumns)
		{
			List<int> columnIds = new();
			ExtractColumnIds(rows, ref columnIds);
			switch (reportSettings.period)
			{
				case ReportSettingsEnums.PeriodOptions.ByYear:
					reportTable.AddColumns(new string[]{ "Year", "Value"});
					break;
				case ReportSettingsEnums.PeriodOptions.ByQuarter:
					foreach (var id in columnIds)
					{
						reportTable.AddColumn("Quarter " + id);
					}
					break;
				case ReportSettingsEnums.PeriodOptions.ByMonth:
					foreach (var id in columnIds)
					{
						reportTable.AddColumn("Quarter " + id);
					}
					break;
				case ReportSettingsEnums.PeriodOptions.ByWeek:
					foreach (var id in columnIds)
					{
						reportTable.AddColumn("Week " + id);
					}
					break;
			}
		}
		else
		{
			switch (reportSettings.period)
			{
				case ReportSettingsEnums.PeriodOptions.ByYear:
					reportTable.AddColumns(new string[] { "Year", "Value" });
					break;
				case ReportSettingsEnums.PeriodOptions.ByQuarter:
					for (int i = 1; i <= 4; i++)
					{
						reportTable.AddColumn("Quarter " + i.ToString());
					}
					break;
				case ReportSettingsEnums.PeriodOptions.ByMonth:
					string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
					reportTable.AddColumn("Year");
					reportTable.AddColumns(names);
					break;
				case ReportSettingsEnums.PeriodOptions.ByWeek:
					for (int i = 1; i <= 53; i++)
					{
						reportTable.AddColumn("Week " + i.ToString());
					}
					break;
			}
		}

		foreach (var rowList in rows)
		{
			foreach (var row in rowList)
			{

			}
		}

		Console.ReadKey();
		Console.Clear();
	}

	private static void ExtractColumnIds(List<List<ReportRow>> rows, ref List<int> columnIds)
	{
		foreach (var rowList in rows)
		{
			foreach (var row in rowList)
			{
				if (!columnIds.Contains(Convert.ToInt32(row.Period)))
				{
					columnIds.Add(Convert.ToInt32(row.Period));
				}
			}
		}
	}
}
