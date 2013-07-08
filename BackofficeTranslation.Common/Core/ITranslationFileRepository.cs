using System.Collections.Generic;

namespace Umbraco.BackofficeTranslation.Common.Core
{
	public interface ITranslationFileRepository
	{
		IEnumerable<TranslationFile> GetAll();
	}
}