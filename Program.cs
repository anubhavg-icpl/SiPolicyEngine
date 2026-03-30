using System;
using System.IO;
using SiPolicyEngine.SiPolicy;
using SiPolicyEngine.Stubs;

namespace SiPolicyEngine;

public static class Program
{
	public static int Main(string[] args)
	{
		if (args.Length == 0 || args[0] is "-h" or "--help")
		{
			PrintUsage();
			return 0;
		}

		string command = args[0].ToLowerInvariant();

		try
		{
			return command switch
			{
				"xml2cip" => ConvertXmlToCip(args),
				"cip2xml" => ConvertCipToXml(args),
				_ => Error($"Unknown command: '{command}'. Use --help for usage.")
			};
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"Error: {ex.Message}");
			return 1;
		}
	}

	private static int ConvertXmlToCip(string[] args)
	{
		string? inputPath = null;
		string? outputPath = null;
		bool skipValidation = false;
		string? schemaPath = null;

		for (int i = 1; i < args.Length; i++)
		{
			switch (args[i])
			{
				case "-i" or "--input":
					inputPath = args[++i];
					break;
				case "-o" or "--output":
					outputPath = args[++i];
					break;
				case "--skip-validation":
					skipValidation = true;
					break;
				case "--schema-path":
					schemaPath = args[++i];
					break;
				default:
					if (inputPath is null)
						inputPath = args[i];
					else if (outputPath is null)
						outputPath = args[i];
					break;
			}
		}

		if (string.IsNullOrEmpty(inputPath))
			return Error("Missing input XML file. Usage: SiPolicyEngine xml2cip <input.xml> [output.cip]");

		if (!File.Exists(inputPath))
			return Error($"Input file not found: {inputPath}");

		// Default output: same name with .cip extension
		outputPath ??= Path.ChangeExtension(inputPath, ".cip");

		if (!string.IsNullOrEmpty(schemaPath))
			GlobalVars.Settings.CiPolicySchemaPath = schemaPath;

		Console.WriteLine($"Converting: {inputPath}");
		Console.WriteLine($"Output:     {outputPath}");

		if (skipValidation)
		{
			// Deserialize without schema validation by passing XmlDocument directly
			System.Xml.XmlDocument xmlDoc = new();
			xmlDoc.Load(inputPath);
			SiPolicyEngine.SiPolicy.SiPolicy policyObj = CustomDeserialization.DeserializeSiPolicy(null, xmlDoc);
			using FileStream fs = new(outputPath, FileMode.Create, FileAccess.ReadWrite);
			BinaryOpsForward.ConvertPolicyToBinary(policyObj, fs);
		}
		else
		{
			Management.ConvertXMLToBinary(inputPath, outputPath);
		}

		FileInfo fi = new(outputPath);
		Console.WriteLine($"Done! Output size: {fi.Length:N0} bytes");
		return 0;
	}

	private static int ConvertCipToXml(string[] args)
	{
		string? inputPath = null;
		string? outputPath = null;

		for (int i = 1; i < args.Length; i++)
		{
			switch (args[i])
			{
				case "-i" or "--input":
					inputPath = args[++i];
					break;
				case "-o" or "--output":
					outputPath = args[++i];
					break;
				default:
					if (inputPath is null)
						inputPath = args[i];
					else if (outputPath is null)
						outputPath = args[i];
					break;
			}
		}

		if (string.IsNullOrEmpty(inputPath))
			return Error("Missing input CIP file. Usage: SiPolicyEngine cip2xml <input.cip> [output.xml]");

		if (!File.Exists(inputPath))
			return Error($"Input file not found: {inputPath}");

		outputPath ??= Path.ChangeExtension(inputPath, ".xml");

		Console.WriteLine($"Converting: {inputPath}");
		Console.WriteLine($"Output:     {outputPath}");

		SiPolicyEngine.SiPolicy.SiPolicy policy = BinaryOpsReverse.ConvertBinaryToXmlFile(inputPath);
		Management.SavePolicyToFile(policy, outputPath);

		FileInfo fi = new(outputPath);
		Console.WriteLine($"Done! Output size: {fi.Length:N0} bytes");
		return 0;
	}

	private static int Error(string message)
	{
		Console.Error.WriteLine(message);
		return 1;
	}

	private static void PrintUsage()
	{
		Console.WriteLine("""
		SiPolicyEngine - Code Integrity Policy Converter

		Usage:
		  SiPolicyEngine xml2cip <input.xml> [output.cip]  Convert XML policy to binary CIP
		  SiPolicyEngine cip2xml <input.cip> [output.xml]  Convert binary CIP to XML policy

		Options:
		  -i, --input <path>       Input file path
		  -o, --output <path>      Output file path (default: input with changed extension)
		  --skip-validation        Skip XML schema validation (xml2cip only)
		  --schema-path <path>     Custom CI policy schema XSD path
		  -h, --help               Show this help

		Examples:
		  SiPolicyEngine xml2cip MyPolicy.xml
		  SiPolicyEngine xml2cip MyPolicy.xml MyPolicy.cip
		  SiPolicyEngine xml2cip --input MyPolicy.xml --output out.cip --skip-validation
		  SiPolicyEngine cip2xml MyPolicy.cip MyPolicy.xml
		""");
	}
}
