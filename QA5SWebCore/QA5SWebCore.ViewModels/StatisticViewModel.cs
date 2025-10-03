using System.Collections.Generic;

namespace QA5SWebCore.ViewModels;

public class StatisticViewModel
{
	public string Name { get; set; }

	public Dictionary<string, double> Values { get; set; }
}
