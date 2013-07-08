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
			controller = new FilesController();
		}

		public IEnumerable<TranslationFile> GetAllFiles()
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			return controller.GetAllFiles();
		}

		public void Create(string fileName)
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			controller.Create(fileName);
		}

		public IEnumerable<TranslationFile> GetPotentialFiles()
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			return controller.GetPotentialFiles();
		}
	}
}
