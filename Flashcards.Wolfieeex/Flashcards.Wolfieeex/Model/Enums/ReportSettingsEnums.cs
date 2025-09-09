using System.ComponentModel;

namespace Flashcards.Wolfieeex.Model.Enums
{
	internal class ReportSettingsEnums
	{
		public enum DisplayOptions
		{
			[Description("Return to Report Menu")]
			ReturnToReportMenu,

			[Description("Hide empty columns")]
			HideEmptyColumns,

			[Description("Display all columns")]
			DisplayAllColumns
		}

		public enum PeriodOptions
		{
			[Description("Return to Report Menu")]
			ReturnToReportMenu,

			[Description("By week")]
			ByWeek,

			[Description("By month")]
			ByMonth,

			[Description("By quarter")]
			ByQuarter,

			[Description("By year")]
			ByYear,
		}

		public enum ReportType
		{
			[Description("Return to Report Menu")]
			ReturnToReportMenu,

			[Description("Report number of study sessions")]
			StudyCount,

			[Description("Report average scores")]
			AverageScore
		}
	}
}
