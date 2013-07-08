using System;
using System.Linq;
using Umbraco.Web;

static internal class SecurityHelper
{
	public static void ThrowIfNotDeveloper(UmbracoContext umbracoContext)
	{
		if (umbracoContext.UmbracoUser.GetApplications().All(a => a.alias != "developer"))
			throw new UnauthorizedAccessException();
	}
}