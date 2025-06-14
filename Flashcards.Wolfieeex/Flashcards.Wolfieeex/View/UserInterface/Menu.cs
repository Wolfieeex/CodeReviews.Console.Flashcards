namespace Flashcards.Wolfieeex.View.UserInterface;

using Flashcards.Wolfieeex.Model;
using Spectre.Console;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public abstract class Menu
{
	public struct MenuColors
	{
		public Color TitleColor;
		public Color SelectorsColor;
		public Color Important1Color;
		public Color Important2Color;
		public Color Important3Color;
		public Color UserInputColor;
		public Color PositiveColor;
		public Color NegativeColor;
	}
	Color _menuColor;
	protected Color MenuColor
	{
		get => _menuColor; set
		{
			_menuColor = value;
			menuColors.TitleColor = value;
			menuColors.SelectorsColor = value.Blend(Color.White, 0.2f);
			menuColors.Important1Color = value.Blend(Color.Orange1, 0.2f);
			menuColors.Important2Color = value.Blend(Color.Magenta1, 0.2f);
			menuColors.Important3Color = value.Blend(Color.Pink1, 0.2f);
			menuColors.UserInputColor = value.Blend(Color.Yellow, 0.2f);
			menuColors.PositiveColor = value.Blend(Color.Green, 0.2f);
			menuColors.NegativeColor = value.Blend(Color.Red, 0.2f);
		}
	}
	protected MenuColors menuColors;

	protected readonly Style style;

	public Menu(Color color)
	{
		MenuColor = color;
		style = new Style(decoration: Decoration.RapidBlink, foreground: menuColors.UserInputColor);
	}

	public abstract void DisplayMenu();

	protected string GetDisplayName(Enum value)
	{
		if (value == null)
			throw new ArgumentNullException($"Your \"Get Display Name\" argument was null.");

		var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
		if (memberInfo != null)
		{
			var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>(false);

			if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
				return displayAttribute.Name;
		}
		return value.ToString();
	}

	protected string GetDescription(Enum value)
	{
		if (value == null)
			throw new ArgumentNullException($"Your \"Get Description\" argument was null.");

		var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
		if (memberInfo != null)
		{
			var displayAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>(false);

			if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Description))
				return displayAttribute.Description;
		}
		return value.ToString();
	}

	protected SpecialLabels GetSpecialLabel(Enum value)
	{
		if (value == null)
			throw new ArgumentNullException($"Your \"Get Description\" argument was null.");

		var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
		if (memberInfo != null)
		{
			var displayAttribute = memberInfo.GetCustomAttribute<EnumLabelSpecialLabel>(false);

			if (displayAttribute != null)
				return displayAttribute.Label;
		}
		return SpecialLabels.None;
	}
}
