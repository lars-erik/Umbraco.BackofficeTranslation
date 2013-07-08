using System;
using System.Collections.Generic;
using System.Linq;

namespace Umbraco.BackofficeTranslation.Common.Core
{
	//public class TranslationSetComparer
	//{
	//	private readonly TranslationSet source;
	//	private readonly TranslationSet translation;

	//	public static TranslationSetComparison Compare(TranslationSet source, TranslationSet translation)
	//	{
	//		var comparer = new TranslationSetComparer(source, translation);
	//		return comparer.CreateComparison();
	//	}

	//	private TranslationSetComparer(TranslationSet source, TranslationSet translation)
	//	{
	//		this.source = source;
	//		this.translation = translation;
	//	}

	//	private TranslationSetComparison CreateComparison()
	//	{
	//		var comparison = new TranslationSetComparison(source.File, translation.File);

	//		comparison.Areas.AddRange(
	//			source.Areas.Select(a => a.Key)
	//			.Union(translation.Areas.Select(t => t.Key))
	//			.Distinct()
	//			.Select(key => new AreaComparison(key))
	//		);

	//		foreach (var areaComparison in comparison.Areas)
	//		{
	//			var sourceArea = source.FindArea(areaComparison.Key);
	//			var translationArea = translation.FindArea(areaComparison.Key);
	//			if (sourceArea == null)
	//			{
	//				areaComparison.State = TranslationState.Obsolete;
	//				continue;
	//			}
				
	//			if (translationArea == null)
	//				throw new NotImplementedException();

	//			areaComparison.Items.AddRange(
	//				sourceArea.Items.Select(i => i.Key)
	//					.Union(translationArea.Items.Select(i => i.Key))
	//					.Distinct()
	//					.Select(key => CreateItemComparison(key, sourceArea, translationArea))
	//			);
	//		}


	//		return comparison;
	//	}

	//	private TranslationComparison CreateItemComparison(string key, Area sourceArea, Area translationArea)
	//	{
	//		var sourceItem = sourceArea.FindItem(key);
	//		var translatedItem = translationArea.FindItem(key);
	//		if (sourceItem == null || translatedItem == null)
	//			throw new NotImplementedException();
	//		var sourceValue = sourceItem.Value;
	//		var translatedValue = translatedItem.Value;
	//		return new TranslationComparison(
	//			key,
	//			sourceValue,
	//			translatedValue,
	//			Compare(sourceValue, translatedValue)
	//			);
	//	}

	//	private TranslationState Compare(string sourceValue, string translatedValue)
	//	{
	//		if (sourceValue != translatedValue)
	//			return TranslationState.Different;
	//		return TranslationState.Equal;
	//	}
	//}

	public class TranslationSetComparison
	{
		private TranslationSet translationSet;
		private TranslationSet sourceSet;
		private List<AreaComparison> areas;

		public List<AreaComparison> Areas
		{
			get
			{
				if (areas == null)
					CompareAreas();
				return areas;
			}
		}

		public TranslationFile Source
		{
			get { return sourceSet.File; }
		}

		public TranslationFile Translation
		{
			get { return translationSet.File; }
		}

		public TranslationSetComparison(TranslationSet sourceSet, TranslationSet translationSet)
		{
			this.sourceSet = sourceSet;
			this.translationSet = translationSet;
		}

		public AreaComparison FindArea(string key)
		{
			return Areas.SingleOrDefault(a => a.Key == key);
		}

		private void CompareAreas()
		{
			areas = sourceSet.Areas.Select(a => a.Key)
			                 .Union(translationSet.Areas.Select(a => a.Key))
			                 .Distinct()
			                 .Select(key => new AreaComparison(key, sourceSet.FindArea(key), translationSet.FindArea(key)))
							 .ToList();
		}
	}

	public class AreaComparison
	{
		private List<TranslationComparison> items;
		private Area sourceArea;
		private Area translationArea;
		public string Key { get; private set; }

		public List<TranslationComparison> Items
		{
			get
			{
				if (items == null)
					items = CompareItems();
				return items;
			}
		}

		public TranslationState State { get; set; }

		public Dictionary<TranslationState, int> States
		{
			get { return Items.GroupBy(i => i.State).ToDictionary(g => g.Key, g => g.Count()); }
		}

		public TranslationComparison FindItem(string key)
		{
			return Items.SingleOrDefault(i => i.Key == key);
		}

		public AreaComparison(string key, Area sourceArea, Area translationArea)
		{
			this.sourceArea = sourceArea;
			this.translationArea = translationArea;
			Key = key;
			State = GetState();
		}

		private TranslationState GetState()
		{
			if (sourceArea == null)
				return TranslationState.Obsolete;
			if (translationArea == null)
				return TranslationState.New;
			return TranslationState.Equal;
		}

		private List<TranslationComparison> CompareItems()
		{
			if (State == TranslationState.Obsolete)
				return translationArea.Items.Select(i => new TranslationComparison(i.Key, null, i)).ToList();
			if (State == TranslationState.New)
				return sourceArea.Items.Select(i => new TranslationComparison(i.Key, i, null)).ToList();
			return sourceArea.Items.Select(i => i.Key)
						.Union(translationArea.Items.Select(i => i.Key))
						.Distinct()
						.Select(key => new TranslationComparison(key, sourceArea.FindItem(key), translationArea.FindItem(key)))
						.ToList();
		}
	}

	public class TranslationComparison
	{
		private readonly TranslationItem source;
		private readonly TranslationItem translation;
		public TranslationState State { get; private set; }
		public string Key { get; private set; }

		public string SourceValue
		{
			get { return source.GetValue(); }
		}

		public string TargetValue
		{
			get { return translation.GetValue(); }
		}

		public TranslationComparison(string key, TranslationItem source, TranslationItem translation)
		{
			Key = key;
			this.source = source;
			this.translation = translation;
			State = GetState();
		}

		private TranslationState GetState()
		{
			if (source == null)
				return TranslationState.Obsolete;
			if (translation == null)
				return TranslationState.New;
			if (source.GetValue() != translation.GetValue())
				return TranslationState.Different;
			return TranslationState.Equal;
		}
	}

	public enum TranslationState
	{
		Equal,
		Different,
		Obsolete,
		New
	}

	internal static class TranslationItemExtensions
	{
		public static string GetValue(this TranslationItem item)
		{
			return item == null || item.Value == null ? String.Empty : item.Value;
		}
	}
}
