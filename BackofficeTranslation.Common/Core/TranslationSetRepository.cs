using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Umbraco.BackofficeTranslation.Common.Core
{
	public class TranslationSetRepository : ITranslationSetRepository
	{
		private readonly string path;
		private readonly XmlSerializer serializer = new XmlSerializer(typeof(TranslationSet));
		private readonly XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new[]{new XmlQualifiedName("","")});

		public TranslationSetRepository(string path)
		{
			this.path = path;
		}

		public TranslationSet Get(string name)
		{
			var filePath = Path.Combine(path, name + ".xml");
			TranslationSet translationSet;
			using (var xmlReader = XmlReader.Create(filePath))
			{
				translationSet = (TranslationSet) serializer.Deserialize(xmlReader);
			}
			translationSet.File = new TranslationFile(name, EnglishNameProvider.GetEnglishName(name));
			return translationSet;
		}

		public void Save(TranslationSet set)
		{
			var filePath = Path.Combine(path, set.File.Name + ".xml");
			using (var xmlWriter = XmlWriter.Create(filePath, new XmlWriterSettings{Indent = true}))
			{
				serializer.Serialize(xmlWriter, set, namespaces);
			}
		}
	}
}
