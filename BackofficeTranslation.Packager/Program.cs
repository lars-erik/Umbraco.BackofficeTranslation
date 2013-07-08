using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using ICSharpCode.SharpZipLib.Zip;
using Umbraco.BackofficeTranslation.Common.Core;

namespace BackofficeTranslation.Packager
{
	class Program
	{
		private const string PackageGuid = "F8CB7939-29B4-409A-8D14-87104059D3E6";

		static void Main(string[] args)
		{
			var solutionDir = args.Length == 1 ? args[0] : Path.Combine(Environment.CurrentDirectory, @"..\..\..\");
			solutionDir = solutionDir.Trim();
			if (solutionDir.EndsWith("\""))
				solutionDir = solutionDir.Replace("\"", "\\");
			var packagePath = Path.Combine(solutionDir, "Package");
			var packageSourcePath = Path.Combine(packagePath, "Source");
			var guidPath = Path.Combine(packageSourcePath, PackageGuid);
#if DEBUG
			var packagerPath = Path.Combine(solutionDir, @"BackofficeTranslation.Packager\bin\debug");
#else
			var packagerPath = Path.Combine(solutionDir, @"BackofficeTranslation.Packager\bin\release");
#endif
			var webPath = Path.Combine(solutionDir, "BackofficeTranslation.Web");
			var packageDocPath = Path.Combine(solutionDir, "package.xml");
			var packageDocTargetPath = Path.Combine(guidPath, "package.xml");
			var versionNumber = typeof(TranslationSet).Assembly.GetName().Version.ToString();
			var zipPath = Path.Combine(packagePath, String.Format("Backoffice_Translation_{0}.zip", versionNumber));
			var waited = false;

			Console.WriteLine("-- Creating package --");
			Console.WriteLine("Using folder '{0}'", packagePath);

			if (Directory.Exists(packagePath))
			{
				Console.WriteLine("Deleting folder '{0}'", packagePath);
				Directory.Delete(packagePath, true);
				while (Directory.Exists(packagePath))
				{
					Console.Write(".");
					waited = true;
				}
				if (waited)
					Console.WriteLine();
			}

			Console.WriteLine("Creating folder '{0}'", packagePath);
			Directory.CreateDirectory(packagePath);

			Console.WriteLine("Creating folder '{0}'", packageSourcePath);
			Directory.CreateDirectory(packageSourcePath);

			Console.WriteLine("Creating folder '{0}'", guidPath);
			Directory.CreateDirectory(guidPath);

			var doc = XDocument.Load(packageDocPath);
			foreach (var fileNode in Descendants(doc, "file"))
			{
				var orgPath = Descendants(fileNode, "orgPath").Single().Value.Substring(1).Replace("/", "\\");
				var orgName = Descendants(fileNode, "orgName").Single().Value;
				var parentPath = orgPath.Contains("bin") ? packagerPath : Path.Combine(webPath, orgPath);
				var sourcePath = Path.Combine(parentPath, orgName);
				var targetPath = Path.Combine(guidPath, orgName);
				Console.WriteLine("Copying file '{0}'", sourcePath);
				File.Copy(sourcePath, targetPath);
				File.SetAttributes(targetPath, FileAttributes.Normal);
			}
			Descendants(doc, "version").Single().Value = versionNumber;

			Console.WriteLine("Saving package manifest to '{0}", packageDocTargetPath);
			doc.Save(packageDocTargetPath);

			Console.WriteLine("Zipping to {0}", zipPath);

			var fastZip = new FastZip();
			fastZip.CreateZip(zipPath, packageSourcePath, true, null);
		}

		private static IEnumerable<XElement> Descendants(XContainer doc, string nodeName)
		{
			return doc.DescendantNodes().OfType<XElement>().Where(n => n.Name == nodeName);
		}
	}
}
