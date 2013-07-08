using System;
using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common.Core;

namespace Umbraco.BackofficeTranslation.Tests.Common.Core
{
	[TestFixture]
	public class EnglishNameProviderTests
	{
		[Test]
		[TestCase("en", "English")]
		[TestCase("en_us", "English (United States)")]
		[TestCase("no", "Norwegian")]
		[TestCase("", "Invariant Language (Invariant Country)")]
		public void GetEnglishName_ReturnsEnglishNameOfCulture(string fileName, string expectedEnglishName)
		{
			var englishName = EnglishNameProvider.GetEnglishName(fileName);
			Assert.AreEqual(expectedEnglishName, englishName);
		}

		[Test]
		public void GetEnglishName_ReturnsUnknownForInvalidCultures()
		{
			var unknownName = EnglishNameProvider.GetEnglishName("fail");
			Assert.AreEqual(String.Format("Unknown English Name ({0})", "fail"), unknownName);
		}

		[Test]
		public void GetEnglishName_ReturnsUnknownNullForNull()
		{
			var unknownName = EnglishNameProvider.GetEnglishName(null);
			Assert.AreEqual("Unknown English Name (null)", unknownName);
		}
	}
}
