using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Umbraco.BackofficeTranslation.Common;
using Umbraco.BackofficeTranslation.Common.Controllers;
using Umbraco.BackofficeTranslation.Common.Core;
using Umbraco.BackofficeTranslation.Common.Exceptions;

namespace Umbraco.BackofficeTranslation.Tests.Common.Controllers
{
	[TestFixture]
	public class FilesControllerTests
	{
		private FilesController controller;
		private readonly string nynorskPath = TestResources.CreatePath("nn_no");
		private ITranslationSetRepository repository;

		[SetUp]
		public void SetUp()
		{
			Factory.Instance = new TestFactory();
			if (File.Exists(nynorskPath))
				File.Delete(nynorskPath);
			controller = new FilesController();
			repository = Factory.Instance.Create<ITranslationSetRepository>();
		}

		[Test]
		public void GetAllFiles_ReturnsAllFiles()
		{
			var files = controller.GetAllFiles();
			Assert.AreEqual(20, files.Count());
		}

		[Test]
		public void GetPotentialFiles_ReturnsAllCulturesButExistingFiles()
		{
			var existing = controller.GetAllFiles();
			var potential = controller.GetPotentialFiles().ToList();

			Assert.AreNotEqual(0, potential.Count());
			Assert.AreEqual(1, potential.Count(f => f.Name == "nn_no"));
			Assert.IsTrue(potential.All(p => existing.All(e => e.Name != p.Name)));
		}

		[Test]
		public void CreateFile_CreatesEmptyTranslationFile()
		{
			controller.Create("nn_no");
			Assert.IsTrue(File.Exists(nynorskPath));
			var set = repository.Get("nn_no");
			Assert.AreEqual(0, set.Areas.Count);
			Assert.AreEqual("Norwegian, Nynorsk (Norway)", set.InternalName);
			Assert.AreEqual("nn_no", set.Key);
			Process.Start("iexplore.exe", nynorskPath);
		}

		[Test]
		[ExpectedException(typeof(FileAlreadyExistsException))]
		public void CreateFile_WhenFileExists_ThrowsFileExists()
		{
			controller.Create("no");
		}
	}
}
