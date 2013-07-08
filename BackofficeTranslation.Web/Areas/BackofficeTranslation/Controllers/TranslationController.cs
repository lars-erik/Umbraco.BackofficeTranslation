using System.Web.Http;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Web.Areas.BackofficeTranslation.Controllers
{
	public class TranslationController : ApiController, ITranslationController
	{
		private readonly ITranslationController controller;

		public TranslationController()
		{
			controller = new Common.Controllers.TranslationController();
		}

		public void PutTranslation(UpdateTranslationCommand cmd)
		{
			controller.PutTranslation(cmd);
		}

		public void DeleteObsolete(string sourceName, string translationName)
		{
			controller.DeleteObsolete(sourceName, translationName);
		}
	}
}