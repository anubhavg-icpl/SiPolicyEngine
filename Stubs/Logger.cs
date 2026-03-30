namespace SiPolicyEngine.Stubs;

/// <summary>
/// Minimal logger stub — writes to stderr for CLI usage.
/// </summary>
public static class Logger
{
	public static void Write(string message)
	{
		Console.Error.WriteLine(message);
	}
}
