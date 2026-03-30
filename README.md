# SiPolicyEngine

Cross-platform CLI tool for converting Windows Code Integrity (CI) policy files between XML and binary CIP formats.

Extracted from the [AppControl Manager](https://github.com/HotCakeX/Harden-Windows-Security) SiPolicy module — no WinUI or GUI dependencies required. Runs on Windows, Linux, and macOS.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Windows**: CI Policy schema validation works automatically via `C:\Windows\schemas\CodeIntegrity\cipolicy.xsd`
- **Linux/macOS**: Schema validation is skipped automatically (or supply your own via `--schema-path`)

## Build

```bash
git clone git@github.com:anubhavg-icpl/SiPolicyEngine.git
cd SiPolicyEngine
dotnet build
```

### Publish as single native executable (AOT)

**Windows:**
```bash
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r win-arm64
```

**Linux:**
```bash
dotnet publish -c Release -r linux-x64
dotnet publish -c Release -r linux-arm64
```

**macOS:**
```bash
dotnet publish -c Release -r osx-x64
dotnet publish -c Release -r osx-arm64
```

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -r linux-x64 -o /app

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0
COPY --from=build /app /app
ENTRYPOINT ["/app/SiPolicyEngine"]
```

```bash
docker build -t sipolicyengine .
docker run --rm -v $(pwd):/data sipolicyengine xml2cip /data/MyPolicy.xml /data/MyPolicy.cip
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

SiPolicyEngine xml2cip --schema-path /path/to/cipolicy.xsd MyPolicy.xml
# Use custom schema path (useful on Linux/macOS)
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

## Platform Notes

| Platform | Schema Validation | Binary Conversion |
|---|---|---|
| Windows | Automatic (uses system XSD) | Full support |
| Linux | Skipped unless `--schema-path` provided | Full support |
| macOS | Skipped unless `--schema-path` provided | Full support |

The binary conversion engine is pure C# with no platform-specific APIs — it works identically on all platforms.

## Project Structure

```
SiPolicyEngine/
├── Program.cs                  # CLI entry point
├── SiPolicyEngine.csproj       # .NET 10 cross-platform project
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
    ├── CiPolicyTest.cs         # XML schema validation (auto-skips on non-Windows)
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
