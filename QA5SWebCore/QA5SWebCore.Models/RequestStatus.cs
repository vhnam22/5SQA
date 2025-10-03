using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class RequestStatus : AuditableEntity
{
	public Guid RequestId { get; set; }

	public virtual Request Request { get; set; }

	public Guid? PlanId { get; set; }

	public virtual Plan Plan { get; set; }

	public int Sample { get; set; }

	public int? OK { get; set; }

	public int? NG { get; set; }

	public int? Empty { get; set; }

	public string Status { get; set; }

	public string CompletedBy { get; set; }

	public DateTimeOffset? Completed { get; set; }

	public string CheckedBy { get; set; }

	public DateTimeOffset? Checked { get; set; }

	public string ApprovedBy { get; set; }

	public DateTimeOffset? Approved { get; set; }
}
