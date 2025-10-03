using System;

namespace _5S_QA_Entities.Dtos;

public class ExportMonthDto
{
	public Guid? ProductId { get; set; }

	public string Mold { get; set; }

	public DateTimeOffset? StartDate { get; set; }

	public DateTimeOffset? EndDate { get; set; }
}
