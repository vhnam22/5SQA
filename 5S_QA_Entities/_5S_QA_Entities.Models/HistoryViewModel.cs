using System;

namespace _5S_QA_Entities.Models;

public class HistoryViewModel
{
	public string Value { get; set; }

	public string CreatedBy { get; set; }

	public string MachineName { get; set; }

	public DateTimeOffset? Created { get; set; }
}
