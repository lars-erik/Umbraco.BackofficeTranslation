using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Common.Controllers
{
	public interface ITranslationController
	{
		void PutTranslation(UpdateTranslationCommand cmd);
		void DeleteObsolete(string sourceName, string translationName);
	}

	public class TranslationController : ITranslationController
	{
		private readonly ITranslationSetRepository setRepository;

		public TranslationController()
		{
			this.setRepository = Factory.Instance.Create<ITranslationSetRepository>();
		}

		public void PutTranslation(UpdateTranslationCommand cmd)
		{
			var set = setRepository.Get(cmd.File);
			var area = set.FindArea(cmd.Area);
			if (area == null)
			{
				area = new Area(cmd.Area);
				set.Areas.Add(area);
			}
			var item = area.FindItem(cmd.Item);
			if (item == null)
			{
				item = new TranslationItem(cmd.Item, cmd.NewValue);
				area.Items.Add(item);
			}
			item.Value = cmd.NewValue;
			setRepository.Save(set);
		}

		public void DeleteObsolete(string sourceName, string translationName)
		{
			var sourceSet = setRepository.Get(sourceName);
			var translationSet = setRepository.Get(translationName);
			var comparison = new TranslationSetComparison(sourceSet, translationSet);
			for (var areaIndex = comparison.Areas.Count - 1; areaIndex >= 0; areaIndex--)
			{
				var area = comparison.Areas[areaIndex];
				for (var itemIndex = area.Items.Count - 1; itemIndex >= 0; itemIndex--)
				{
					var item = area.Items[itemIndex];
					if (item.State == TranslationState.Obsolete)
					{
						var translatedItem = translationSet.FindArea(area.Key).FindItem(item.Key);
						translationSet.FindArea(area.Key).Items.Remove(translatedItem);
					}
				}
				if (area.State == TranslationState.Obsolete)
				{
					var translatedArea = translationSet.FindArea(area.Key);
					translationSet.Areas.Remove(translatedArea);
				}
			}
			setRepository.Save(translationSet);
		}
	}
}
