using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Http;
using Umbraco.BackofficeTranslation.Common.Core;
using Umbraco.BackofficeTranslation.Common.Exceptions;

namespace Umbraco.BackofficeTranslation.Common.Controllers
{
	public interface IFilesController
	{
		IEnumerable<TranslationFile> GetAllFiles();
		void Create(string fileName);
		IEnumerable<TranslationFile> GetPotentialFiles();
	}

	public class FilesController : IFilesController
	{
		private readonly ITranslationFileRepository repository;
		private readonly ITranslationSetRepository setRepository;

		public FilesController()
		{
			repository = Factory.Instance.Create<ITranslationFileRepository>();
			setRepository = Factory.Instance.Create<ITranslationSetRepository>();
		}

		public IEnumerable<TranslationFile> GetAllFiles()
		{
			return repository.GetAll().OrderBy(f => f.EnglishName);
		}

		public void Create(string fileName)
		{
			if (repository.GetAll().Any(f => f.Name == fileName))
				throw new FileAlreadyExistsException(fileName);

			var culture = CultureInfo.GetCultureInfo(CultureName(fileName));
			var file = new TranslationFile(fileName, culture.EnglishName);
			var set = new TranslationSet(file, culture);
			setRepository.Save(set);
		}

		public IEnumerable<TranslationFile> GetPotentialFiles()
		{
			var allSpecific = CultureInfo.GetCultures(CultureTypes.AllCultures);
			var existing = repository.GetAll();
			return allSpecific
				.Where(s => existing.All(e => e.Name != UmbracoName(s.Name)))
				.Select(s => new TranslationFile(UmbracoName(s.Name), s.EnglishName))
				.OrderBy(s => s.EnglishName);
		}

		private static string CultureName(string fileName)
		{
			return fileName.Replace("_", "-");
		}

		private static string UmbracoName(string cultureName)
		{
			return cultureName.Replace("-", "_").ToLower();
		}
	}
}
