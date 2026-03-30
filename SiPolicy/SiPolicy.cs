// MIT License
//
// Copyright (c) 2023-Present - Violet Hansen - (aka HotCakeX on GitHub) - Email Address: spynetgirl@outlook.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// See here for more information: https://github.com/HotCakeX/Harden-Windows-Security/blob/main/LICENSE
//

// . "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\xsd.exe" "C:\Windows\schemas\CodeIntegrity\cipolicy.xsd" /classes /namespace:AppControlManager.SiPolicy /language:CS

using System.Collections.Generic;

namespace SiPolicyEngine.SiPolicy;

public sealed class MacrosMacro(string id, string value)
{
	public string Id => id;

	public string Value => value;
}

public sealed class AppSetting(List<string>? value, string? name)
{
	public List<string>? Value => value;

	public string? Name => name;
}

public sealed class AppRoot(List<AppSetting>? setting, string manifest)
{
	public List<AppSetting>? Setting => setting;

	public string Manifest => manifest;
}

public sealed class AppSettingRegion(List<AppRoot>? app)
{
	public List<AppRoot>? App => app;
}

public readonly struct RuleType(OptionType item)
{
	public OptionType Item => item;
}

public enum OptionType : uint
{
	EnabledUMCI = 4U,
	EnabledBootMenuProtection = 8U,
	EnabledIntelligentSecurityGraphAuthorization = 16U,
	EnabledInvalidateEAsonReboot = 32U,
	RequiredWHQL = 128U,
	EnabledDeveloperModeDynamicCodeTrust = 256U,
	EnabledAllowSupplementalPolicies = 1024U,
	DisabledRuntimeFilePathRuleProtection = 2048U,
	EnabledRevokedExpiredAsUnsigned = 8192U,
	EnabledAuditMode = 65536U,
	DisabledFlightSigning = 131072U,
	EnabledInheritDefaultPolicy = 262144U,
	EnabledUnsignedSystemIntegrityPolicy = 524288U,
	EnabledDynamicCodeSecurity = 1048576U,
	RequiredEVSigners = 2097152U,
	EnabledBootAuditOnFailure = 4194304U,
	EnabledAdvancedBootOptionsMenu = 8388608U,
	DisabledScriptEnforcement = 16777216U,
	RequiredEnforceStoreApplications = 33554432U,
	EnabledSecureSettingPolicy = 67108864U,
	EnabledManagedInstaller = 134217728U,
	EnabledUpdatePolicyNoReboot = 268435456U,
	EnabledConditionalWindowsLockdownPolicy = 536870912U,
	DisabledDefaultWindowsCertificateRemapping
}

public sealed class SettingValueType(object item)
{
	/// Can only hold Binary/Boolean/DWord/String
	public object Item { get; set; } = item;
}

public sealed class Setting(SettingValueType value, string provider, string key, string valueName)
{
	public SettingValueType Value { get; set; } = value;

	public string Provider => provider;

	public string Key => key;

	public string ValueName => valueName;
}

public sealed class CertEKU(string id)
{
	public string ID => id;
}

public sealed class CertOemID(string value)
{
	public string Value => value;
}

public sealed class CertPublisher(string value)
{
	public string Value => value;
}

public sealed class CertIssuer(string value)
{
	public string Value => value;
}

public readonly struct CertRoot(CertEnumType type, ReadOnlyMemory<byte> value)
{
	public CertEnumType Type => type;

	/// <summary>
	/// Holds hexBinary
	/// </summary>
	public ReadOnlyMemory<byte> Value => value;
}

public enum CertEnumType
{
	TBS,
	Wellknown,
}

public sealed class ProductSigners
{
	public AllowedSigners? AllowedSigners { get; set; }

	public DeniedSigners? DeniedSigners { get; set; }

	public FileRulesRef? FileRulesRef { get; set; }
}

public sealed class AllowedSigners(List<AllowedSigner> allowedSigner)
{
	public List<AllowedSigner> AllowedSigner => allowedSigner;

	public string? Workaround { get; set; }
}

public sealed class AllowedSigner(string signerId, List<ExceptDenyRule>? exceptDenyRule)
{
	public List<ExceptDenyRule>? ExceptDenyRule => exceptDenyRule;

	public string SignerId => signerId;
}

public sealed class ExceptDenyRule(string denyRuleID)
{
	public string DenyRuleID => denyRuleID;
}

public sealed class DeniedSigners(List<DeniedSigner> deniedSigner)
{
	public List<DeniedSigner> DeniedSigner => deniedSigner;

	public string? Workaround { get; set; }
}

public sealed class DeniedSigner(string signerId, List<ExceptAllowRule>? exceptAllowRule)
{
	public List<ExceptAllowRule>? ExceptAllowRule => exceptAllowRule;

	public string SignerId => signerId;
}

public sealed class ExceptAllowRule(string allowRuleID)
{
	public string AllowRuleID => allowRuleID;
}

public sealed class FileRulesRef(List<FileRuleRef> fileRuleRef)
{
	public List<FileRuleRef> FileRuleRef => fileRuleRef;

	public string? Workaround { get; set; }
}

public sealed class FileRuleRef(string ruleID)
{
	public string RuleID => ruleID;
}

public sealed class TestSigners
{
	public AllowedSigners? AllowedSigners { get; set; }

	public DeniedSigners? DeniedSigners { get; set; }

	public FileRulesRef? FileRulesRef { get; set; }
}

public sealed class TestSigningSigners
{
	public AllowedSigners? AllowedSigners { get; set; }

	public DeniedSigners? DeniedSigners { get; set; }

	public FileRulesRef? FileRulesRef { get; set; }
}

public sealed class AppIDTag(string key, string value)
{
	public string Key => key;

	public string Value => value;
}

public sealed class AppIDTags
{
	public List<AppIDTag>? AppIDTag { get; set; }

	public bool? EnforceDLL { get; set; }
}

public sealed class FileAttribRef(string ruleID)
{
	public string RuleID { get; set; } = ruleID;
}

public sealed class EKU(string id, ReadOnlyMemory<byte> value, string? friendlyName)
{
	public string ID => id;

	/// <summary>
	/// Holds hexBinary
	/// </summary>
	public ReadOnlyMemory<byte> Value => value;

	public string? FriendlyName => friendlyName;
}

public sealed class Allow(string id)
{
	public string ID => id;

	public string? FriendlyName { get; set; }

	public string? FileName { get; set; }

	public string? InternalName { get; set; }

	public string? FileDescription { get; set; }

	public string? ProductName { get; set; }

	public string? PackageFamilyName { get; set; }

	public string? PackageVersion { get; set; }

	public string? MinimumFileVersion { get; set; }

	public string? MaximumFileVersion { get; set; }

	/// <summary>
	/// Holds hexBinary
	/// </summary>
	public ReadOnlyMemory<byte> Hash { get; set; }

	public string? AppIDs { get; set; }

	public string? FilePath { get; set; }

	public string? RequireHotpatchID { get; set; }

	public uint? MinimumHotpatchSequence { get; set; }

	public uint? MaximumHotpatchSequence { get; set; }
}

public sealed class Deny(string id)
{
	public string ID => id;

	public string? FriendlyName { get; set; }

	public string? FileName { get; set; }

	public string? InternalName { get; set; }

	public string? FileDescription { get; set; }

	public string? ProductName { get; set; }

	public string? PackageFamilyName { get; set; }

	public string? PackageVersion { get; set; }

	public string? MinimumFileVersion { get; set; }

	public string? MaximumFileVersion { get; set; }

	/// <summary>
	/// Holds hexBinary
	/// </summary>
	public ReadOnlyMemory<byte> Hash { get; set; }

	public string? AppIDs { get; set; }

	public string? FilePath { get; set; }
}

public sealed class FileAttrib(string id)
{
	public string ID => id;

	public string? FriendlyName { get; set; }

	public string? FileName { get; set; }

	public string? InternalName { get; set; }

	public string? FileDescription { get; set; }

	public string? ProductName { get; set; }

	public string? PackageFamilyName { get; set; }

	public string? PackageVersion { get; set; }

	public string? MinimumFileVersion { get; set; }

	public string? MaximumFileVersion { get; set; }

	/// <summary>
	/// Holds hexBinary
	/// </summary>
	public ReadOnlyMemory<byte> Hash { get; set; }

	public string? AppIDs { get; set; }

	public string? FilePath { get; set; }
}

public sealed class FileRule(string id, RuleTypeType type)
{
	public string ID => id;

	public RuleTypeType Type => type;

	public string? FriendlyName { get; set; }

	public string? FileName { get; set; }

	public string? InternalName { get; set; }

	public string? FileDescription { get; set; }

	public string? ProductName { get; set; }

	public string? PackageFamilyName { get; set; }

	public string? PackageVersion { get; set; }

	public string? MinimumFileVersion { get; set; }

	public string? MaximumFileVersion { get; set; }

	/// <summary>
	/// Holds hexBinary
	/// </summary>
	public ReadOnlyMemory<byte> Hash { get; set; }

	public string? AppIDs { get; set; }

	public string? FilePath { get; set; }
}

public enum RuleTypeType
{
	Match,
	Exclude,
	Attribute
}

public sealed class UpdatePolicySigner(string signerID)
{
	public string SignerId => signerID;
}

public sealed class SupplementalPolicySigner(string signerID)
{
	public string SignerId => signerID;
}

public sealed class CiSigner(string signerID)
{
	public string SignerId => signerID;
}

public sealed class Signer(string id, string name, CertRoot certRoot)
{
	public CertRoot CertRoot => certRoot;

	public List<CertEKU>? CertEKU { get; set; }

	public CertIssuer? CertIssuer { get; set; }

	public CertPublisher? CertPublisher { get; set; }

	public CertOemID? CertOemID { get; set; }

	public List<FileAttribRef>? FileAttribRef { get; set; }

	public string Name { get; set; } = name;

	public string ID { get; set; } = id;

	public DateTime? SignTimeAfter { get; set; }
}

public sealed class SigningScenario(string id, byte value, ProductSigners productSigners)
{
	public ProductSigners ProductSigners => productSigners;

	public TestSigners? TestSigners { get; set; }

	public TestSigningSigners? TestSigningSigners { get; set; }

	public AppIDTags? AppIDTags { get; set; }

	public string ID => id;

	public string? FriendlyName { get; set; }

	public byte Value => value;

	public string? InheritedScenarios { get; set; }

	public ushort? MinimumHashAlgorithm { get; set; }
}

public sealed class SiPolicy(
	string versionEx,
	string platformID,
	string policyID,
	string basePolicyID,
	List<RuleType> rules,
	PolicyType policyType)
{
	public string VersionEx { get; set; } = versionEx;

	public string? PolicyTypeID { get; set; }

	public string PlatformID => platformID;

	public string PolicyID { get; set; } = policyID;

	public string BasePolicyID { get; set; } = basePolicyID;

	public List<RuleType> Rules => rules;

	public List<EKU>? EKUs { get; set; }

	/// Can only hold the following types:
	/// <see cref="Allow"/>
	/// <see cref="Deny"/>
	/// <see cref="FileAttrib"/>
	/// <see cref="FileRule"/>
	public List<object>? FileRules { get; set; }

	public List<Signer>? Signers { get; set; }

	public List<SigningScenario>? SigningScenarios { get; set; }

	public List<UpdatePolicySigner>? UpdatePolicySigners { get; set; }

	public List<CiSigner>? CiSigners { get; set; }

	public uint? HvciOptions { get; set; }

	public List<Setting>? Settings { get; set; }

	public List<MacrosMacro>? Macros { get; set; }

	public List<SupplementalPolicySigner>? SupplementalPolicySigners { get; set; }

	public AppSettingRegion? AppSettings { get; set; }

	public string? FriendlyName { get; set; }

	public PolicyType PolicyType { get; set; } = policyType;
}

public enum PolicyType : int
{
	BasePolicy = 0,
	SupplementalPolicy,
	AppIDTaggingPolicy,
}
