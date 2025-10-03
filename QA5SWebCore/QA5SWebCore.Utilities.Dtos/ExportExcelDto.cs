using System;
using System.Collections.Generic;

namespace QA5SWebCore.Utilities.Dtos;

public class ExportExcelDto
{
	public Guid ProductId { get; set; }

	public Guid ExportTemplateId { get; set; }

	public Guid LineId { get; set; }

	public List<Guid> RequestId { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }
}
