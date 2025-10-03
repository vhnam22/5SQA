using System.Collections.Generic;

namespace _5S_QA_Entities.Models;

public class StatisticViewModel
{
	public string Name { get; set; }

	public Dictionary<string, double> Values { get; set; }
}
