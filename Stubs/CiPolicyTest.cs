using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SiPolicyEngine.Stubs;

/// <summary>
/// Validates CI policy XML files against the Code Integrity schema.
/// On Linux/macOS, validation is skipped if no schema path is configured.
/// </summary>
public static class CiPolicyTest
{
	public static void TestCiPolicy(string xmlFilePath)
	{
		if (!File.Exists(xmlFilePath))
		{
			throw new FileNotFoundException($"XML file not found: {xmlFilePath}", xmlFilePath);
		}

		// Resolve schema path
		string schemaPath = GlobalVars.CISchemaPath;

		if (string.IsNullOrEmpty(schemaPath) || !File.Exists(schemaPath))
		{
			if (!string.IsNullOrEmpty(GlobalVars.Settings.CiPolicySchemaPath) && File.Exists(GlobalVars.Settings.CiPolicySchemaPath))
			{
				schemaPath = GlobalVars.Settings.CiPolicySchemaPath;
			}
			else
			{
				// No schema available — skip validation (expected on Linux/macOS)
				return;
			}
		}

		// Validate XML against schema
		XmlReaderSettings settings = new();
		_ = settings.Schemas.Add(null, schemaPath);
		settings.ValidationType = ValidationType.Schema;
		settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

		settings.ValidationEventHandler += (sender, args) =>
		{
			throw new XmlSchemaValidationException(
				$"XML validation error in '{xmlFilePath}': {args.Message}");
		};

		XmlDocument xmlDoc = new();
		xmlDoc.Load(xmlFilePath);

		using XmlReader reader = XmlReader.Create(new StringReader(xmlDoc.OuterXml), settings);
		while (reader.Read()) { }
	}
}
