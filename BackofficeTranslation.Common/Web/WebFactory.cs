using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Common.Web
{
	public class WebFactory : Factory
	{
		readonly Dictionary<Type, Func<object>> factoryMethods = new Dictionary<Type, Func<object>>
			{
				{ typeof(ITranslationFileRepository), CreateFileRepository },
				{ typeof(ITranslationSetRepository), CreateSetRepository }
			};

		private readonly string fullPath;

		public override T Create<T>()
		{
			return (T)factoryMethods[typeof(T)]();
		}

		public WebFactory(HttpContext context)
		{
			var path = ConfigurationManager.AppSettings["BackofficeTranslator:TranslationDirectory"] ??
				"~/umbraco/config/lang";
			fullPath = context.Server.MapPath(path);
		}

		private static object CreateSetRepository()
		{
			return new TranslationSetRepository(FullPath);
		}

		private static object CreateFileRepository()
		{
			return new TranslationFileRepository(FullPath);
		}

		private static string FullPath
		{
			get { return ((WebFactory) Instance).fullPath; }
		}
	}
}