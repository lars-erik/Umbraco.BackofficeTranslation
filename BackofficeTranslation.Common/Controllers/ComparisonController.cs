using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Common.Controllers
{
	public interface IComparisonController
	{
		TranslationSetComparison GetComparison(string sourceName, string translationName);
	}

	public class ComparisonController : IComparisonController
	{
		private readonly ITranslationSetRepository setRepository;

		public ComparisonController()
		{
			setRepository = Factory.Instance.Create<ITranslationSetRepository>();
		}

		public TranslationSetComparison GetComparison(string sourceName, string translationName)
		{
			var source = setRepository.Get(sourceName);
			var translation = setRepository.Get(translationName);
			return new TranslationSetComparison(source, translation);
		}
	}
}
