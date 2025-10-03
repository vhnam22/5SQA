using System.Collections.Generic;

namespace _5SQA_Config_UWave.Constants;

public class Constant
{
	public const string APIMachineGets = "/api/Machine/NoAuthorGets";

	public const string InUse = "In Use";

	public const string NotUse = "Not Use";

	public const string Repairing = "Repairing";

	public static List<string> dgvContent = new List<string> { "ChanelId", "Code", "name", "Model", "Serial", "FactoryName", "MachineTypeName", "Status" };

	public static List<string> dgvFooter = new List<string> { "Created", "Modified", "CreatedBy", "ModifiedBy", "IsActivated" };

	public const string FILECONFIG = "Config.ini";

	public static string PATHCONFIG = "C:\\5SQA_UWave_Config";

	public const string KEY = "A&A Technology";
}
