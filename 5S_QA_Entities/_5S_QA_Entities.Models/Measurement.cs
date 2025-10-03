namespace _5S_QA_Entities.Models;

public class Measurement
{
	public string Temp { get; set; }

	public string Name { get; set; }

	public string Important { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public string Tolerance { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public string USL { get; set; }

	public string USLFormula { get; set; }

	public string LSL { get; set; }

	public string LSLFormula { get; set; }

	public string MachineType { get; set; }

	public string Type { get; set; }

	public string Formula { get; set; }

	public string Mapper { get; set; }
}
