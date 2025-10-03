namespace _5S_QA_Entities.Dtos;

public class ExportMappingDto
{
	public string CellAddress { get; set; }

	public string Value { get; set; }

	public string Sheet { get; set; }

	public ExportMappingDto()
	{
	}

	public ExportMappingDto(string cellAddress, string value, string sheet)
	{
		CellAddress = cellAddress;
		Value = value;
		Sheet = sheet;
	}
}
