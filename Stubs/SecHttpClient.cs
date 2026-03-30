using System.Net.Http;

namespace SiPolicyEngine.Stubs;

/// <summary>
/// Minimal HTTP client stub for AppManifest URI loading.
/// </summary>
public static class SecHttpClient
{
	public static readonly HttpClient Instance = new();
}
