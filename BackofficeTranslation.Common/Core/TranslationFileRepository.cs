using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Umbraco.BackofficeTranslation.Common.Core
{
	public class TranslationFileRepository : ITranslationFileRepository
	{
		private readonly string path;

		public TranslationFileRepository(string path)
		{
			this.path = path;
		}

		public IEnumerable<TranslationFile> GetAll()
		{
			return Directory.GetFiles(path)
			                .Select(CreateTranslationFile);
		}

		private static TranslationFile CreateTranslationFile(string filePath)
		{
			var name = Path.GetFileNameWithoutExtension(filePath);
			var englishName = EnglishNameProvider.GetEnglishName(name);
			return new TranslationFile(name, englishName);
		}
	}
}
