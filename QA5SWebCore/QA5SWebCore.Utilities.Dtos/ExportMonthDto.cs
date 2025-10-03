using System;
using System.Collections.Generic;

namespace QA5SWebCore.Utilities.Dtos;

public class ExportMonthDto
{
	public Guid ProductId { get; set; }

	public DateTimeOffset? StartDate { get; set; }

	public DateTimeOffset? EndDate { get; set; }

	public List<string> Supplier { get; set; }
}
