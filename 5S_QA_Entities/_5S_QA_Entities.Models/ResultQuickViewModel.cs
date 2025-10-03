namespace _5S_QA_Entities.Models;

public class ResultQuickViewModel
{
	public string Name { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public double? LSL { get; set; }

	public double? USL { get; set; }

	public int? Cavity { get; set; }

	public string Result { get; set; }

	public string Judge { get; set; }

	public string History { get; set; }

	public string MeasurementUnit { get; set; }
}
