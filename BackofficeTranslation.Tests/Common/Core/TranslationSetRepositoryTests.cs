using System.Diagnostics;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Tests.Common.Core
{
	[TestFixture]
	public class TranslationSetRepositoryTests
	{
		private TranslationSetRepository repository;

		private bool openTheFileInIe = true;

		[SetUp]
		public void SetUp()
		{
			Factory.Instance = new TestFactory();
			if (File.Exists(TestResources.TestFilePath))
				File.Delete(TestResources.TestFilePath);
			repository = new TranslationSetRepository(TestResources.TranslationFilesPath);
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void Get_ReturnsTranslationFileWithAllContent()
		{
			var set = repository.Get("en");
			Assert.AreEqual("en", set.File.Name);
			Assert.AreEqual("English", set.File.EnglishName);
			Assert.AreEqual("en", set.Key);
			Assert.AreEqual("English (UK)", set.InternalName);
			Assert.AreEqual("en-GB", set.CultureName);
			Assert.AreEqual("umbraco", set.Creator.Name);
			Assert.AreEqual("http://umbraco.org", set.Creator.Link);
			Assert.AreNotEqual(0, set.Areas.Count);
			Assert.IsNotNull(set.FindArea("actions"));
			Assert.AreNotEqual(0, set.FindArea("actions").Items.Count);
			Assert.AreEqual("Culture and Hostnames", set.FindArea("actions").FindItem("assignDomain").Value);
			Assert.AreEqual("Domains", set.FindArea("assignDomain").FindItem("setDomains").Value);
		}

		[Test]
		public void Save_WritesTranslationFileWithAllContent_ManualCheck()
		{
			var set = new TranslationSet(new TranslationFile("test", "Test file"), CultureInfo.GetCultureInfo("nn-NO"));
			set.Areas.Add(new Area{Key = "testAreaKey"});
			set.FindArea("testAreaKey").Items.Add(new TranslationItem("testTranslationKey", "Test value"));
			repository.Save(set);
			Assert.IsTrue(File.Exists(TestResources.TestFilePath));
			if (openTheFileInIe)
				Process.Start("iexplore.exe", TestResources.TestFilePath);
		}
	}
}
