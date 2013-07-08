using System.Globalization;
using System.IO;
using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Core;
using Umbraco.BackofficeTranslation.Common.Controllers;

namespace Umbraco.BackofficeTranslation.Tests.Common.Controllers
{
	[TestFixture]
	public class TranslationControllerTests
	{
		private TranslationController controller;
		private TranslationSetRepository setRepository;

		[SetUp]
		public void SetUp()
		{
			Factory.Instance = new TestFactory();
			if (File.Exists(TestResources.TestFilePath))
				File.Delete(TestResources.TestFilePath);

			setRepository = new TranslationSetRepository(TestResources.TranslationFilesPath);
			controller = new TranslationController();
		}

		[Test]
		public void PutTranslation_ChangesTranslationItemAndSaves()
		{
			var cmd = new UpdateTranslationCommand
			{
				File = "test",
				Area = "area",
				Item = "item",
				NewValue = "new value"
			};

			var set = new TranslationSet(new TranslationFile("test", "Test"), CultureInfo.InvariantCulture);
			var area = new Area("area");
			area.Items.Add(new TranslationItem("item", "old value"));
			set.Areas.Add(area);
			setRepository.Save(set);

			controller.PutTranslation(cmd);

			var loadedSet = setRepository.Get("test");
			Assert.AreEqual("new value", loadedSet.FindArea("area").FindItem("item").Value);
		}

		[Test]
		public void PutTranslation_WhenNewItem_InsertsTranslationItemAndSaves()
		{
			var cmd = new UpdateTranslationCommand
			{
				File = "test",
				Area = "area",
				Item = "newitem",
				NewValue = "new value"
			};

			var set = new TranslationSet(new TranslationFile("test", "Test"), CultureInfo.InvariantCulture);
			var area = new Area("area");
			area.Items.Add(new TranslationItem("item", "old value"));
			set.Areas.Add(area);
			setRepository.Save(set);

			controller.PutTranslation(cmd);

			var loadedSet = setRepository.Get("test");
			Assert.AreEqual("new value", loadedSet.FindArea("area").FindItem("newitem").Value);
		}

		[Test]
		public void DeleteObsolete_DeletesObsoleteAreasAndItems()
		{
			var source = new TranslationSet(new TranslationFile("source", "Source"), CultureInfo.InvariantCulture);
			var translation = new TranslationSet(new TranslationFile("test", "Test"), CultureInfo.InvariantCulture);
			var sourceArea = new Area("a");
			var translatedArea = new Area("a");
	
			sourceArea.Items.Add(new TranslationItem("a", "a"));
			source.Areas.Add(sourceArea);

			translatedArea.Items.Add(new TranslationItem("a", "a"));
			translatedArea.Items.Add(new TranslationItem("obsoleteItem1", "a"));
			translation.Areas.Add(translatedArea);

			var obsoleteArea = new Area("obsolete");
			obsoleteArea.Items.Add(new TranslationItem("obsoleteItem2", "abc"));
			translation.Areas.Add(obsoleteArea);

			setRepository.Save(source);
			setRepository.Save(translation);

			controller.DeleteObsolete("source", "test");

			var loadedSet = setRepository.Get("test");
			Assert.AreEqual(1, loadedSet.Areas.Count);
			Assert.AreEqual(1, loadedSet.FindArea("a").Items.Count);
			Assert.IsNotNull(loadedSet.FindArea("a").FindItem("a"));
		}
	}
}
