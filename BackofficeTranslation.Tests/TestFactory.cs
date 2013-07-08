using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Tests
{
	public class TestFactory : Factory
	{
		public override T Create<T>()
		{
			if (typeof (T) == typeof (ITranslationSetRepository))
				return (T)(object)(new TranslationSetRepository(TestResources.TranslationFilesPath));
			return (T)(object)(new TranslationFileRepository(TestResources.TranslationFilesPath));
		}
	}
}