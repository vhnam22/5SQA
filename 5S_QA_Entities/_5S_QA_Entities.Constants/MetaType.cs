using System.Collections.Generic;

namespace _5S_QA_Entities.Constants;

public static class MetaType
{
	public const string VN = "rdbVN";

	public const string EN = "rdbEN";

	public const string JP = "rdbJP";

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

	public const string Default = "Kiểm tra đầu vào";

	public const string UnActivated = "Unactivated";

	public const string Activated = "Activated";

	public const string Rejected = "Rejected";

	public const string Completed = "Completed";

	public const string Checked = "Checked";

	public const string Approved = "Approved";

	public static object[] lstStatus = new object[5] { "Activated", "Rejected", "Completed", "Checked", "Approved" };

	public const string Supervisor = "Supervisor";

	public const string Foreman = "Foreman";

	public const string SubLeader = "Sub-Leader";

	public const string Inspector = "Inspector";

	public static object[] lstJobTitle = new object[6] { "Manager", "Supervisor", "Foreman", "Leader", "Sub-Leader", "Inspector" };

	public const string ManualInput = "Manual Input";

	public const string Formuler = "Formuler";

	public const string Degrees = "°";

	public const string Minutes = "'";

	public const string Seconds = "\"";

	public const string ACCEPTABLE = "ACCEPTABLE";

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

	public const string Vertical = "V";

	public const string Horizontal = "H";

	public static List<string> IsDate = new List<string> { "ProduceDate", "DateFrom", "DateTo", "Date", "RequestDate", "Completed", "Checked", "Approved", "Created", "Modified" };

	public const string Confirm = "Confirm";

	public const string ImportFromExcel = "Import from excel";

	public static List<string> dgvContent = new List<string>
	{
		"FullName", "Username", "Email", "IsEmail", "BirthDate", "Gender", "PhoneNumber", "Address", "JobTitle", "Position",
		"DepartmentName", "LoginAt", "TimeLogin", "MachineTypeName", "Code", "name", "Model", "Serial", "FactoryName", "Status",
		"Description", "ImportantName", "Value", "UnitName", "Upper", "Lower", "Formula", "TemplateName", "ProductCode", "ProductName",
		"StageName", "Date", "LSL", "USL", "LCL", "UCL", "ProductStage", "Cavity", "Sample", "SampleMax",
		"Special", "Lot", "DeliveryDate", "Quantity", "Detail", "Coordinate", "Customer", "ExportDate", "OK", "NG",
		"Remain", "Type", "Line", "Intention", "CalibrationNo", "Staff", "CalDate", "ExpDate", "Period", "File",
		"AssemblyDate", "Link", "Mark", "Company", "TypeName", "RevisionHistory", "Version", "ReleaseDate", "InputDate", "Supplier"
	};

	public static List<string> dgvOther = new List<string> { "ImageUrl", "TemplateUrl", "ProductImageUrl", "Link" };

	public static List<string> dgvFooter = new List<string>
	{
		"PreparedBy", "Judgement", "CompletedBy", "Completed", "CheckedBy", "Checked", "ApprovedBy", "Approved", "Created", "Modified",
		"CreatedBy", "ModifiedBy", "IsActivated"
	};

	public static List<string> mPanelOther = new List<string>
	{
		"Code", "name", "Description", "Result", "Judge", "MachineName", "StaffName", "Content", "Link", "Created",
		"Modified", "CreatedBy", "ModifiedBy", "IsActivated", "ProductCode", "ProductName"
	};

    public static List<string> mPanelMapper = new List<string> { "Code", "name", "Mapper", "Created", "Modified", "CreatedBy", "ModifiedBy", "IsActivated" };

    public static List<string> mPanelGroup = new List<string>
	{
		"Code", "name", "Description", "Limit", "Type", "TemplateUrl", "Created", "Modified", "CreatedBy", "ModifiedBy",
		"IsActivated"
	};

	public static List<string> mPanelSimilar = new List<string> { "ProductCode", "ProductName", "TemplateName", "TemplateDescription", "Created", "Modified", "CreatedBy", "ModifiedBy", "IsActivated" };

	public static List<string> mPanelAQL = new List<string> { "Type", "InputQuantity", "Sample", "Created", "Modified", "CreatedBy", "ModifiedBy", "IsActivated" };

	public static string[] TemplateHeaders = new string[6] { "No", "Code", "Name", "Description", "Limit", "Type" };

	public static string[] ProductHeaders = new string[6] { "No", "Code", "Name", "Description", "Cavity", "SampleMax" };

	public static string[] MeasurementHeaders = new string[10] { "No", "Code", "Name", "ImportantName", "Value", "UnitName", "Upper", "Lower", "Formula", "MachineTypeName" };

	public static string[] RequestHeaders = new string[17]
	{
		"No", "ProductCode", "OrderNumber", "OrderQuantity", "TotalQuantity", "MaterialType", "MaterialCode", "MaterialName", "MaterialInvoice", "MaterialChange",
		"ProduceNo", "Customer", "PoNumber", "Information", "Note", "Sample", "Status"
	};

	public static string[] OrderHeaders = new string[8] { "Stt", "Tên KH", "Tên hàng", "Số bản vẽ", "Kensaless", "Mã số sx", "Số lượng", "Ghi chú" };

	public static string[] AQLHeaders = new string[4] { "No", "Type", "InputQuantity", "Sample" };

	public static int LIMIT = 10;

	public static int OFFSET = 13;

	public static int MERGE = 2;

	public static int CAVITY = 1;

	public static List<string> MacTypeSpecial = new List<string> { "RG", "SG", "NG2", "PG", "I" };

	public static List<string> UnitSpecial = new List<string> { "°" };

	public static List<string> NameSpecial = new List<string>
	{
		"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
		"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
		"U", "V", "W", "X", "Y", "Z"
	};

	public static List<string> CharSpecial = new List<string> { "↓", "a", "R", "Ø", "D", "C" };

	public static List<string> MachineMapper = new List<string> { "K", "SM" };

	public static List<string> SignAddNew = new List<string> { "R", "Ø", "C", "M" };

	public static List<string> MacTypeAddNew = new List<string> { "4", "C面ｹﾞｰｼﾞ" };

	public static Dictionary<string, string> Mapper = new Dictionary<string, string>
	{
		{ "Ø", "PHI" },
		{ "PCD", "PCD" },
		{ "R", "R" },
		{ "C", "C" },
		{ "Ʇ", "VG" },
		{ "//", "SS" },
		{ "⊚", "DT" },
		{ "°", "GOC" }
	};

	public const string InforVertical = "InforVertical";

	public const string InforHorizontal = "InforHorizontal";

	public const string InforVerticalRevert = "InforVerticalRevert";

	public const string InforHorizontalRevert = "InforHorizontalRevert";

	public const string InforCurrent = "InforCurrent";

	public const string Title = "Title";

	public const string Type = "Type";

	public const string INCOMING_INSPECTION = "INCOMING INSPECTION";

	public const string PROCESS_DATA_INSPECTION = "PROCESS DATA INSPECTION";

	public const string OTHER = "Hình vẽ:";
}
