using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Common.Web
{
	public class TranslationStateConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue("\"" + value.ToString().ToLower() + "\"");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TranslationState);
		}
	}
}
