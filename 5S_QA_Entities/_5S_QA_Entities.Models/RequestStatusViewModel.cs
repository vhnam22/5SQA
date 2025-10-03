using System;

namespace _5S_QA_Entities.Models;

public class RequestStatusViewModel
{
	public int? Sample { get; set; }

	public Guid? PlanId { get; set; }

	public string PlanName { get; set; }

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
