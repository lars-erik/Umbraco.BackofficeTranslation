using System.Web.Mvc;

namespace Umbraco.BackofficeTranslation.Web.Areas.BackofficeTranslation
{
	public class BackofficeTranslationAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "BackofficeTranslation";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"BackofficeTranslation_default",
				"BackofficeTranslation/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
