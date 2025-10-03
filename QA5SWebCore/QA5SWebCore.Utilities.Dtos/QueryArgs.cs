namespace QA5SWebCore.Utilities.Dtos;

public class QueryArgs
{
	public string Predicate { get; set; }

	public object[] PredicateParameters { get; set; }

	public string Order { get; set; }

	public int Page { get; set; }

	public int Limit { get; set; }
}
