using System;
using System.Collections.Generic;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Plan : AuditableEntity
{
	public Guid ProductId { get; set; }

	public virtual Product Product { get; set; }

	public Guid StageId { get; set; }

	public virtual MetadataValue Stage { get; set; }

	public Guid? TemplateId { get; set; }

	public virtual Template Template { get; set; }

	public virtual ICollection<PlanDetail> PlanDetails { get; set; }

	public int Sort { get; set; }

	public Plan()
	{
		PlanDetails = new HashSet<PlanDetail>();
	}
}
