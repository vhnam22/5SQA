using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace _5S_QA_Client.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.13.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Username
	{
		get
		{
			return (string)this["Username"];
		}
		set
		{
			this["Username"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Password
	{
		get
		{
			return (string)this["Password"];
		}
		set
		{
			this["Password"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool RememberPassword
	{
		get
		{
			return (bool)this["RememberPassword"];
		}
		set
		{
			this["RememberPassword"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("50")]
	public int limitLogin
	{
		get
		{
			return (int)this["limitLogin"];
		}
		set
		{
			this["limitLogin"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool IsVertical
	{
		get
		{
			return (bool)this["IsVertical"];
		}
		set
		{
			this["IsVertical"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool IsSameTool
	{
		get
		{
			return (bool)this["IsSameTool"];
		}
		set
		{
			this["IsSameTool"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("rbEN")]
	public string Language
	{
		get
		{
			return (string)this["Language"];
		}
		set
		{
			this["Language"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int Camera
	{
		get
		{
			return (int)this["Camera"];
		}
		set
		{
			this["Camera"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool IsShowChart
	{
		get
		{
			return (bool)this["IsShowChart"];
		}
		set
		{
			this["IsShowChart"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("1;2;3;4;5;6;9;10;11;12;13;14;15;16")]
	public string Headers
	{
		get
		{
			return (string)this["Headers"];
		}
		set
		{
			this["Headers"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("1;2;3;4;5;6;9;10;11;12;13;14;15;16")]
	public string CompleteHeaders
	{
		get
		{
			return (string)this["CompleteHeaders"];
		}
		set
		{
			this["CompleteHeaders"] = value;
		}
	}
}
