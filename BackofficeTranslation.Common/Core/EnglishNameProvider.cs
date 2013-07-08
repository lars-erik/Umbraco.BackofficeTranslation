using System;
using System.Globalization;

namespace Umbraco.BackofficeTranslation.Common.Core
{
	static internal class EnglishNameProvider
	{
		internal static string GetEnglishName(string name)
		{
			if (name == null)
				return UnknownEnglishName("null");
			try
			{
				name = name.Replace("_", "-");
				var cultureInfo = CultureInfo.GetCultureInfo(name);
				return cultureInfo.EnglishName;
			}
			catch(CultureNotFoundException)
			{
				return UnknownEnglishName(name);
			}
		}

		private static string UnknownEnglishName(string name)
		{
			return String.Format("Unknown English Name ({0})", name);
		}
	}
}