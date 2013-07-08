using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Umbraco.BackofficeTranslation.Common.Core
{
	[XmlRoot("language")]
	public class TranslationSet
	{
		[XmlAttribute("alias")]
		public string Key { get; set; }

		[XmlAttribute("intName")]
		public string InternalName { get; set; }

		[XmlAttribute("localName")]
		public string LocalName { get; set; }

		[XmlAttribute("lcid")]
		public string LCID { get; set; }

		[XmlAttribute("culture")]
		public string CultureName { get; set; }

		[XmlElement("creator")]
		public Creator Creator { get; set; }

		[XmlElement("area")]
		public List<Area> Areas { get; set; }

		[XmlIgnore]
		public TranslationFile File { get; internal set; }

		public TranslationSet()
		{
			Areas = new List<Area>();
		}

		public TranslationSet(TranslationFile file, CultureInfo culture)
			: this()
		{
			File = file;
			Key = file.Name;
			InternalName = culture.EnglishName;
			LocalName = culture.DisplayName;
			LCID = Convert.ToString(culture.LCID);
			CultureName = culture.Name;
		}

		public Area FindArea(string key)
		{
			return Areas.SingleOrDefault(a => a.Key == key);
		}
	}

	public class Creator
	{
		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("link")]
		public string Link { get; set; }
	}

	public class Area
	{
		[XmlAttribute("alias")]
		public string Key { get; set; }

		[XmlElement("key")]
		public List<TranslationItem> Items { get; set; }

		public Area()
		{
			Items = new List<TranslationItem>();
		}

		public Area(string key)
			: this()
		{
			Key = key;
		}

		public TranslationItem FindItem(string key)
		{
			return Items.SingleOrDefault(i => i.Key == key);
		}

		public string FindValue(string key)
		{
			var item = FindItem(key);
			if (item == null)
				return String.Empty;
			return item.Value ?? String.Empty;
		}
	}

	public class TranslationItem
	{
		[XmlAttribute("alias")]
		public string Key { get; set; }

		[XmlText]
		public string Value { get; set; }

		public TranslationItem()
		{
		}

		public TranslationItem(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}