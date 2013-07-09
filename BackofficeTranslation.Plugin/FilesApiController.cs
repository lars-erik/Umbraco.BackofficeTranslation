using System;
using System.Collections.Generic;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Umbraco.BackofficeTranslation.Plugin
{
	[PluginController("BackofficeTranslation")]
	public class FilesApiController : UmbracoApiController, IFilesController
	{
		private readonly FilesController controller;

		public FilesApiController()
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			controller = new FilesController();
		}

		public IEnumerable<TranslationFile> GetAllFiles()
		{
			return controller.GetAllFiles();
		}

		public void Create(string fileName)
		{
			controller.Create(fileName);
		}

		public IEnumerable<TranslationFile> GetPotentialFiles()
		{
			return controller.GetPotentialFiles();
		}
	}
}
