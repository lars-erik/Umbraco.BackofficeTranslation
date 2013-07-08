using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Tests.Common.Controllers
{
	[TestFixture]
	public class ComparisonControllerTests
	{
		private ComparisonController controller;

		[SetUp]
		public void SetUp()
		{
			Factory.Instance = new TestFactory();
			controller = new ComparisonController();
		}

		[Test]
		public void GetComparison_ReturnsComparison()
		{
			var result = controller.GetComparison("en", "no");
			Assert.AreEqual("en", result.Source.Name);
			Assert.AreEqual("no", result.Translation.Name);
		}
	}
}
