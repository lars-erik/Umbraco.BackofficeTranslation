using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Web;
using Umbraco.Core;

namespace Umbraco.BackofficeTranslation.Plugin
{
	public class PluginRegistrationHandler : ApplicationEventHandler
	{
		protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			Factory.Instance = new WebFactory(umbracoApplication.Context);
		}
	}
}
