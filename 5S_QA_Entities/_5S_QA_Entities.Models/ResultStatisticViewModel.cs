using System;

namespace _5S_QA_Entities.Models;

public class ResultStatisticViewModel
{
	public string RequestName { get; set; }

	public DateTimeOffset? Date { get; set; }

	public string Lot { get; set; }

	public string Line { get; set; }

	public string Type { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public int? Cavity { get; set; }

	public int? Sample { get; set; }

	public string Result { get; set; }

	public string Judge { get; set; }
}
