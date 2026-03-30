using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SiPolicyEngine.Stubs;

/// <summary>
/// Validates CI policy XML files against the Code Integrity schema.
/// Extracted from AppControlManager.Main.CiPolicyTest.
/// </summary>
public static class CiPolicyTest
{
	public static void TestCiPolicy(string xmlFilePath)
	{
		string schemaPath = GlobalVars.CISchemaPath;

		if (!File.Exists(schemaPath))
		{
			schemaPath = string.IsNullOrEmpty(GlobalVars.Settings.CiPolicySchemaPath) || !File.Exists(GlobalVars.Settings.CiPolicySchemaPath)
				? throw new FileNotFoundException($"CI Schema not found at: {schemaPath}. Use --skip-validation or set --schema-path.", schemaPath)
				: GlobalVars.Settings.CiPolicySchemaPath;
		}

		if (!File.Exists(xmlFilePath))
		{
			throw new FileNotFoundException($"XML file not found: {xmlFilePath}", xmlFilePath);
		}

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
