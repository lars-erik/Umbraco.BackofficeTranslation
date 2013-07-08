using System;
using System.Linq;
using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Tests.Common.Core
{
	[TestFixture]
	public class TranslationFileRepositoryTests
	{
		private TranslationFileRepository repository;

		[SetUp]
		public void SetUp()
		{
			var path = TestResources.TranslationFilesPath;
			repository = new TranslationFileRepository(path);
		}

		[Test]
		public void GetAll_ReturnsListOfAllFilesWithCultureName()
		{
			var files = repository.GetAll().ToList();
			Assert.AreNotEqual(0, files.Count());
			Assert.AreEqual("English", files.Single(f => f.Name == "en").EnglishName);
			foreach (var file in files)
			{
				Console.WriteLine("{0,-6}{1}", file.Name, file.EnglishName);
			}
		}
	}
}
