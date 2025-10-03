using System;

namespace QA5SWebCore.Utilities.Dtos;

public class History
{
	public DateTimeOffset Created { get; set; }

	public string Value { get; set; }

	public string CreatedBy { get; set; }

	public string MachineName { get; set; }
}
