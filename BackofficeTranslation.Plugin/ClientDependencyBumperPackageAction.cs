using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using umbraco;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using umbraco.interfaces;
using helper = umbraco.cms.businesslogic.packager.standardPackageActions.helper;

namespace Umbraco.BackofficeTranslation.Plugin
{
	public class ClientDependencyBumperPackageAction : IPackageAction
	{
		public bool Execute(string packageName, XmlNode xmlData)
		{
			var config = new ClientDependencyConfiguration();
			config.IncreaseVersionNumber();
			return true;
		}

		public string Alias()
		{
			return "ClientDependencyBump";
		}

		public bool Undo(string packageName, XmlNode xmlData)
		{
			return false;
		}

		public XmlNode SampleXml()
		{
			return helper.parseStringToXmlNode("<Action runat=\"install\" undo=\"false\" alias=\"ClientDependencyBump\"/>");
		}
	}

	internal class ClientDependencyConfiguration
	{
		private readonly string fileName;

		public ClientDependencyConfiguration()
		{
			fileName = IOHelper.MapPath(string.Format("{0}/ClientDependency.config", SystemDirectories.Config));
		}

		/// <summary>
		/// Increases the version number in ClientDependency.config by 1
		/// </summary>
		internal bool IncreaseVersionNumber()
		{
			try
			{
				var clientDependencyConfigXml = XDocument.Load(fileName, LoadOptions.PreserveWhitespace);
				if (clientDependencyConfigXml.Root != null)
				{

					var versionAttribute = clientDependencyConfigXml.Root.Attribute("version");

					int oldVersion;
					int.TryParse(versionAttribute.Value, out oldVersion);
					var newVersion = oldVersion + 1;

					versionAttribute.SetValue(newVersion);
					clientDependencyConfigXml.Save(fileName, SaveOptions.DisableFormatting);

					LogHelper.Info<ClientDependencyConfiguration>(string.Format("Updated version number from {0} to {1}", oldVersion,
						newVersion));
					return true;
				}
			}
			catch (Exception ex)
			{
				LogHelper.Error<ClientDependencyConfiguration>("Couldn't update ClientDependency version number", ex);
			}

			return false;
		}
	}
}
