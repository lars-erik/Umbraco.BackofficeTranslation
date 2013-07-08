using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbraco.BackofficeTranslation.Common.Exceptions
{
	public class FileAlreadyExistsException : Exception
	{
		public FileAlreadyExistsException(string name)
			: base(String.Format("File '{0}' already exists", name))
		{
		}
	}
}
