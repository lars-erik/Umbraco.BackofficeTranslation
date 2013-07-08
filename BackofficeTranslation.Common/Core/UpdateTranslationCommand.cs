namespace Umbraco.BackofficeTranslation.Common.Core
{
	public class UpdateTranslationCommand
	{
		public string File { get; set; }
		public string Area { get; set; }
		public string Item { get; set; }
		public string NewValue { get; set; }
	}
}
