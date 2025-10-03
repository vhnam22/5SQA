using System;

namespace QA5SWebCore.Utilities.Dtos;

public class GetResultDto
{
	public Guid ProductId { get; set; }

	public DateTimeOffset Date { get; set; }

	public string Mold { get; set; }

	public string Shift { get; set; }
}
