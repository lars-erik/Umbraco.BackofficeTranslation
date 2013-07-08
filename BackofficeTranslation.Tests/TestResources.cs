using System;
using System.IO;

namespace Umbraco.BackofficeTranslation.Tests
{
	static internal class TestResources
	{
		public static string TranslationFilesPath
		{
			get { return Path.Combine(Environment.CurrentDirectory, @"..\..\TranslationFiles"); }
		}

		public static string TestFilePath
		{
			get { return CreatePath("test"); }
		}

		public static string CreatePath(string name)
		{
			return Path.Combine(TranslationFilesPath, name + ".xml");
		}
	}
}