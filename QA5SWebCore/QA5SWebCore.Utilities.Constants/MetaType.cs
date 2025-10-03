using System.Collections.Generic;

namespace QA5SWebCore.Utilities.Constants;

public static class MetaType
{
	public const string MachineSign = "MAC-";

	public const string MeasurementSign = "MEAS-";

	public const string TemplateSign = "TEMP-";

	public const string RequestSign = "REQ-";

	public const string DepartmentSign = "DEP-";

	public const string MachineTypeSign = "MTYPE-";

	public const string FactorySign = "FAC-";

	public const string ImportantSign = "IMP-";

	public const string UnitSign = "UNIT-";

	public const string StageSign = "STA-";

	public const string TypeSign = "TYPE-";

	public const string RankSign = "RANK-";

	public const string LineSign = "LINE-";

	public const string ConfigSign = "CFG-";

	public const string TypeCalSign = "TYPECAL-";

	public const string DepartmentId = "55630EBA-6A11-4001-B161-9AE77ACCA43D";

	public const string MachineTypeId = "438D7052-25F3-4342-ED0C-08D7E9C5C77D";

	public const string FactoryId = "5EB07BDB-9086-4BC5-A02B-5D6E9CFCD476";

	public const string ImportantId = "6042BF53-9411-47D4-9BD6-F8AB7BABB663";

	public const string UnitId = "7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D";

	public const string StageId = "11C5FD56-AD45-4457-8DC9-6C8D9F6673A1";

	public const string TypeId = "AC5FA813-C9EE-4805-A850-30A5EA5AB0A1";

	public const string RankId = "EEA65E86-D458-4919-82F7-3DCA0475695D";

	public const string LineId = "AC5FA814-C9EE-4807-A851-30A5EA5AB0A2";

	public const string ConfigId = "3FCB0099-A290-46A6-A2C4-1934C6328B9D";

	public const string TypeCalId = "AC5FA815-C9EE-4807-A852-30A5EA5AB0A3";

	public const string DirFileFinish = "DirFile";

	public const string Admin = "Admin";

	public const string Male = "Male";

	public const string Female = "Female";

	public const string Staff = "Staff";

	public const string Leader = "Leader";

	public const string Manager = "Manager";

	public const string Supervisor = "Supervisor";

	public const string TYPE = "TYPE";

	public const string IME = "IME";

	public const string System = "System";

	public const string Client = "Client";

	public const string Tablet = "Tablet";

	public const string InUse = "In Use";

	public const string NotUse = "Not Use";

	public const string Repairing = "Repairing";

	public const string Detail = "Detail";

	public const string Chart = "Chart";

	public const string Special = "Special";

	public const string UnActivated = "Unactivated";

	public const string Activated = "Activated";

	public const string Reject = "Rejected";

	public const string Complete = "Completed";

	public const string Checked = "Checked";

	public const string Approved = "Approved";

	public const string ManualInput = "Manual Input";

	public const string Formuler = "Formuler";

	public const string Degrees = "°";

	public const string Minutes = "'";

	public const string Seconds = "\"";

	public const string ACCEPTABLE = "ACCEPTABLE";

	public const string STANDARD = "STANDARD";

	public const string OK = "OK";

	public const string OKUpper = "OK+";

	public const string OKLower = "OK-";

	public const string NG = "NG";

	public const string NGUpper = "NG+";

	public const string NGLower = "NG-";

	public const string GO = "GO";

	public const string NOGO = "NOGO";

	public const string IN = "IN";

	public const string STOP = "STOP";

	public const string RANK5 = "RANK5";

	public const string Product = "Product";

	public const string Sample = "Sample";

	public const string Plan = "Plan";

	public const string All = "All";

	public const string Daily = "Daily";

	public static List<string> IsDate = new List<string>
	{
		"PRODUCEDATE]]", "DATEFROM]]", "DATETO]]", "DATE]]", "COMPLETED]]", "CHECKED]]", "APPROVED]]", "CREATED]]", "MODIFIED]]", "[[TIME#",
		"[[DATE#"
	};

	public static List<string> IsImage = new List<string> { "[[COMPLETEDBY]]", "[[CHECKEDBY]]", "[[APPROVEDBY]]" };

	public static List<string> lstFilter = new List<string> { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "IsActivated" };

	public static List<string> lstAdd = new List<string> { "Standard", "NG", "OK+", "OK-" };

	public const string Vertical = "V";

	public const string Horizontal = "H";

	public const int ELEMENT = 7;

	public const string gbPosition = "gbPosition";

	public const string gbAbnormal = "gbAbnormal";

	public const string gbOKNG = "gbOKNG";

	public const string gbRequestStatus = "gbRequestStatus";

	public const string cbManager = "cbManager";

	public const string cbLeader = "gbOKNG";

	public const string cbStaff = "cbStaff";

	public const string cbDepartment = "cbDepartment";

	public const string cbAbnormalActive = "cbAbnormalActive";

	public const string cbXtb1 = "cbXtb1";

	public const string cbXtb2 = "cbXtb2";

	public const string cbXtb3 = "cbXtb3";

	public const string cbXtb4 = "cbXtb4";

	public const string cbXtb5 = "cbXtb5";

	public const string cbXtb6 = "cbXtb6";

	public const string cbXtb7 = "cbXtb7";

	public const string cbXtb8 = "cbXtb8";

	public const string cbR1 = "cbR1";

	public const string cbR2 = "cbR2";

	public const string cbR3 = "cbR3";

	public const string cbR4 = "cbR4";

	public const string cbOKNGActive = "cbOKNGActive";

	public const string cbChecked = "cbChecked";

	public const string cbCompleted = "cbCompleted";
}
