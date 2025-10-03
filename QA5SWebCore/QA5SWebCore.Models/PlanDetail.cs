using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class PlanDetail : AuditableEntity
{
	public Guid PlanId { get; set; }

	public virtual Plan Plan { get; set; }

	public Guid? MeasurementId { get; set; }

	public virtual Measurement Measurement { get; set; }
}
