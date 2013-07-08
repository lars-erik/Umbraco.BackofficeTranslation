namespace Umbraco.BackofficeTranslation.Common.Core
{
	public class TranslationFile
	{
		public string Name { get; private set; }
		public string EnglishName { get; private set; }

		public TranslationFile(string name, string englishName)
		{
			Name = name;
			EnglishName = englishName;
		}
	}
}