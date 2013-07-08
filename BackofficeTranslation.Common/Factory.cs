using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Common
{
	public abstract class Factory
	{
		public static Factory Instance { get; set; }

		public abstract T Create<T>();
	}
}
