using System;
using System.Collections.Generic;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Utilities.Dtos;

public class PlanDto : AuditableEntity
{
	public Guid? ProductId { get; set; }

	public Guid? StageId { get; set; }

	public Guid? TemplateId { get; set; }

	public List<Guid> MeasurementIds { get; set; }

	public PlanDto()
	{
		MeasurementIds = new List<Guid>();
	}
}
