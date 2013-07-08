using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Umbraco.BackofficeTranslation.Plugin
{
	[PluginController("BackofficeTranslation")]
	public class TranslationApiController : UmbracoApiController, ITranslationController
	{
		private readonly TranslationController controller;

		public TranslationApiController()
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			controller = new TranslationController();
		}

		public void PutTranslation(UpdateTranslationCommand cmd)
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			controller.PutTranslation(cmd);
		}

		public void DeleteObsolete(string sourceName, string translationName)
		{
			SecurityHelper.ThrowIfNotDeveloper(UmbracoContext);
			controller.DeleteObsolete(sourceName, translationName);
		}
	}
}
