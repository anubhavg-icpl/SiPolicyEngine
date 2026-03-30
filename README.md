# SiPolicyEngine

Standalone CLI tool for converting Windows Code Integrity (CI) policy files between XML and binary CIP formats.

Extracted from the [AppControl Manager](https://github.com/HotCakeX/Harden-Windows-Security) SiPolicy module — no WinUI or GUI dependencies required.

## Requirements

- Windows 10 22H2+ / Windows 11
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- CI Policy schema file at `C:\Windows\schemas\CodeIntegrity\cipolicy.xsd` (ships with Windows)

## Build

```bash
git clone git@github.com:anubhavg-icpl/SiPolicyEngine.git
cd SiPolicyEngine
dotnet build
```

### Publish as single native executable (AOT)

```bash
dotnet publish -c Release -r win-x64
# Output: bin/Release/net10.0-windows10.0.26100.0/win-x64/publish/SiPolicyEngine.exe
```

For ARM64:

```bash
dotnet publish -c Release -r win-arm64
```

## Usage

### Convert XML to CIP (binary)

```bash
SiPolicyEngine xml2cip MyPolicy.xml
# Output: MyPolicy.cip

SiPolicyEngine xml2cip MyPolicy.xml CustomOutput.cip
# Explicit output path

SiPolicyEngine xml2cip --skip-validation MyPolicy.xml
# Skip XML schema validation

SiPolicyEngine xml2cip --schema-path "D:\schemas\cipolicy.xsd" MyPolicy.xml
# Use custom schema path
```

### Convert CIP (binary) to XML

```bash
SiPolicyEngine cip2xml MyPolicy.cip
# Output: MyPolicy.xml

SiPolicyEngine cip2xml MyPolicy.cip CustomOutput.xml
# Explicit output path
```

### Options

| Flag | Description |
|---|---|
| `-i`, `--input` | Input file path |
| `-o`, `--output` | Output file path (defaults to input with swapped extension) |
| `--skip-validation` | Skip XML schema validation (xml2cip only) |
| `--schema-path` | Custom CI policy schema XSD path |
| `-h`, `--help` | Show help |

## Project Structure

```
SiPolicyEngine/
├── Program.cs                  # CLI entry point
├── SiPolicyEngine.csproj       # .NET 10 project file
├── GlobalUsings.cs             # Global using directives
├── SiPolicy/                   # Core conversion engine (from AppControl Manager)
│   ├── Management.cs           # Entry points: ConvertXMLToBinary, SavePolicyToFile
│   ├── BinaryOpsForward.cs     # XML → binary CIP serialization
│   ├── BinaryOpsReverse.cs     # Binary CIP → XML deserialization
│   ├── CustomDeserialization.cs # XML file → SiPolicy object
│   ├── CustomSerialization.cs  # SiPolicy object → XML document
│   ├── SiPolicy.cs            # Data model (all policy types and enums)
│   ├── Helper.cs              # Version conversion, sorting, utilities
│   ├── ApplicationManifest.cs  # AppManifest data model
│   └── CustomAppManifestLogics.cs # AppManifest serialization
└── Stubs/                      # Lightweight replacements for WinUI dependencies
    ├── GlobalVars.cs           # Config, schema path, namespace constants
    ├── CiPolicyTest.cs         # XML schema validation
    ├── Logger.cs               # stderr logging
    └── SecHttpClient.cs        # HTTP client for manifest URIs
```

## How It Works

1. **XML → SiPolicy object** — `CustomDeserialization` parses and validates the XML against the CI schema
2. **SiPolicy object → binary CIP** — `BinaryOpsForward` serializes into the binary format (version 9) with header, option flags, and sections 0–9
3. **Binary CIP → SiPolicy object** — `BinaryOpsReverse` parses the binary (handles both signed PKCS#7 and unsigned)
4. **SiPolicy object → XML** — `CustomSerialization` writes back to XML document

All serialization is hand-rolled (no reflection) and compatible with Native AOT compilation.

## License

MIT — see the original [AppControl Manager license](https://github.com/HotCakeX/Harden-Windows-Security/blob/main/LICENSE).
