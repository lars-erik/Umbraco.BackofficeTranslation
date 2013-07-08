using System.Globalization;
using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Tests.Common.Core
{
	public class TranslationSetComparisonTests
	{
		private TranslationSet source;
		private TranslationSet translation;
		private TranslationSetComparison comparison;

		[SetUp]
		public void SetUp()
		{
			source = new TranslationSet(new TranslationFile("a", "A"), CultureInfo.InvariantCulture);
			translation = new TranslationSet(new TranslationFile("b", "B"), CultureInfo.InvariantCulture);
		}

		[Test]
		public void SourceAndTranslationFiles_ReturnsFileHeaders()
		{
			Compare();
			Assert.AreEqual(source.File, comparison.Source);
			Assert.AreEqual(translation.File, comparison.Translation);
		}

		[Test]
		public void EqualAreas_HasTranslationStatesForItemsInBothFiles()
		{
			CreateEqualAreaWithDifferences();

			Compare();

			var comparedArea = comparison.FindArea("a");
			Assert.AreEqual(TranslationState.Different, comparedArea.FindItem("a").State);
			Assert.AreEqual(TranslationState.Different, comparedArea.FindItem("nullTranslation").State);
			Assert.AreEqual(TranslationState.Equal, comparedArea.FindItem("b").State);
			Assert.AreEqual(TranslationState.New, comparedArea.FindItem("new").State);
			Assert.AreEqual(TranslationState.Obsolete, comparedArea.FindItem("obsolete").State);
		}

		[Test]
		public void EqualArea_HasCountOfItemStates()
		{
			CreateEqualAreaWithDifferences();
			Compare();

			var comparedArea = comparison.FindArea("a");
			Assert.AreEqual(2, comparedArea.States[TranslationState.Different]);
			Assert.AreEqual(1, comparedArea.States[TranslationState.Equal]);
			Assert.AreEqual(1, comparedArea.States[TranslationState.New]);
			Assert.AreEqual(1, comparedArea.States[TranslationState.Obsolete]);
		}

		[Test]
		public void SourceAreaDoesntExist_ReturnsComparisonWithObsoleteAreaAndObsoleteItems()
		{
			var obsoleteArea = new Area("a");
			obsoleteArea.Items.Add(new TranslationItem("obsolete", "a"));
			translation.Areas.Add(obsoleteArea);

			Compare();

			Assert.AreEqual(TranslationState.Obsolete, comparison.FindArea("a").State);
			Assert.AreEqual(TranslationState.Obsolete, comparison.FindArea("a").FindItem("obsolete").State);
		}

		[Test]
		public void TranslationAreaDoesntExist_ReturnsComparisonWithNewAreaAndNewItems()
		{
			var sourceArea = new Area("a");
			sourceArea.Items.Add(new TranslationItem("new", "a"));
			source.Areas.Add(sourceArea);

			Compare();

			Assert.AreEqual(TranslationState.New, comparison.FindArea("a").State);
			Assert.AreEqual(TranslationState.New, comparison.FindArea("a").FindItem("new").State);
		}

		private void Compare()
		{
			comparison = new TranslationSetComparison(source, translation);
		}

		private void CreateEqualAreaWithDifferences()
		{
			source.Areas.Add(new Area("a"));
			translation.Areas.Add(new Area("a"));

			var sourceArea = source.FindArea("a");
			var translationArea = translation.FindArea("a");

			sourceArea.Items.Add(new TranslationItem("a", "a"));
			sourceArea.Items.Add(new TranslationItem("b", "c"));
			sourceArea.Items.Add(new TranslationItem("nullTranslation", "c"));
			sourceArea.Items.Add(new TranslationItem("new", "d"));
			translationArea.Items.Add(new TranslationItem("a", "b"));
			translationArea.Items.Add(new TranslationItem("b", "c"));
			translationArea.Items.Add(new TranslationItem("nullTranslation", null));
			translationArea.Items.Add(new TranslationItem("obsolete", "d"));
		}
	}
}
