using System.Collections.Generic;

namespace _5SQA_Config_UWave.Dtos;

public class QueryArgs
{
	public string Predicate { get; set; }

	public ICollection<object> PredicateParameters { get; set; }

	public string Order { get; set; }

	public int? Page { get; set; }

	public int? Limit { get; set; }
}
