using System.IO;

namespace SiPolicyEngine.Stubs;

/// <summary>
/// Minimal stub for GlobalVars — replaces AppControlManager's localization and config.
/// </summary>
public static class GlobalVars
{
	/// <summary>
	/// The SiPolicy XML namespace.
	/// </summary>
	public const string SiPolicyNamespace = "urn:schemas-microsoft-com:sipolicy";

	/// <summary>
	/// Path to the Code Integrity schema XSD file on the local system.
	/// </summary>
	public static readonly string CISchemaPath = Path.Combine(
		Environment.GetEnvironmentVariable("SystemDrive") + @"\",
		"Windows", "schemas", "CodeIntegrity", "cipolicy.xsd");

	/// <summary>
	/// Stub settings object.
	/// </summary>
	public static readonly SettingsStub Settings = new();

	/// <summary>
	/// Returns the key itself as the string (no localization needed for CLI).
	/// </summary>
	public static string GetStr(string key) => key;
}

/// <summary>
/// Minimal settings stub to satisfy CiPolicyTest's fallback schema path check.
/// </summary>
public sealed class SettingsStub
{
	public string? CiPolicySchemaPath { get; set; }
}
