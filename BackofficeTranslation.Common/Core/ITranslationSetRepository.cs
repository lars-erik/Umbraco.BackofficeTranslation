namespace Umbraco.BackofficeTranslation.Common.Core
{
	public interface ITranslationSetRepository
	{
		TranslationSet Get(string name);
		void Save(TranslationSet set);
	}
}