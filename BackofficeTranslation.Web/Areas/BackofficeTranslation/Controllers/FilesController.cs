using System.Collections.Generic;
using System.Web.Http;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Web.Areas.BackofficeTranslation.Controllers
{
	public class FilesController : ApiController, IFilesController
	{
		private readonly IFilesController controller;

		public FilesController()
		{
			controller = new Common.Controllers.FilesController();
		}

		public IEnumerable<TranslationFile> GetAllFiles()
		{
			return controller.GetAllFiles();
		}

		public IEnumerable<TranslationFile> GetPotentialFiles()
		{
			return controller.GetPotentialFiles();
		}

		public void Create(string cultureName)
		{
			controller.Create(cultureName);
		}
	}
}