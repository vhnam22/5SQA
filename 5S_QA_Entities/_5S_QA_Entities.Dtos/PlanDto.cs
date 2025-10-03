using System;
using System.Collections.Generic;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Dtos;

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
