using System;

namespace _5S_QA_Entities.Models;

public class CpkViewModel
{
	public Guid? Id { get; set; }

	public string Name { get; set; }

	public double? Cpk { get; set; }

	public string Rank { get; set; }
}
