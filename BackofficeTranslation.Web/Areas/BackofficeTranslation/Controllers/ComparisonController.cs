using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;
using Umbraco.BackofficeTranslation.Common.Web;

namespace Umbraco.BackofficeTranslation.Web.Areas.BackofficeTranslation.Controllers
{
    public class ComparisonController : ApiController, IComparisonController
    {
		private readonly IComparisonController controller;

	    public ComparisonController()
	    {
			controller = new Common.Controllers.ComparisonController();
	    }

		TranslationSetComparison IComparisonController.GetComparison(string sourceName, string translationName)
		{
			return controller.GetComparison(sourceName, translationName);
		}

		public HttpResponseMessage GetComparison(string sourceName, string translationName)
		{
			return JsonResponse(((IComparisonController)this).GetComparison(sourceName, translationName));
		}

	    private HttpResponseMessage JsonResponse(TranslationSetComparison obj)
	    {
		    return new HttpResponseMessage
			    {
				    Content = JsonContent(obj)
			    };
	    }

	    private StringContent JsonContent(TranslationSetComparison obj)
	    {
		    return JsonContent(obj, new JsonConverter[] { new TranslationStateConverter() });
	    }

	    private StringContent JsonContent(object obj, JsonConverter[] converters)
		{
			return new StringContent(JsonConvert.SerializeObject(obj, converters), Encoding.UTF8, "application/json");
		}
	}
}
