using System.Collections.Generic;

namespace _5S_QA_Entities.Dtos;

public class ConfigDto
{
	public string Table { get; set; }

	public Dictionary<string, string> Value { get; set; }
}
