using System;

namespace _5S_QA_Entities.Models;

public class RequestStatusQuickViewModel
{
	public bool? IsSelect { get; set; }

	public string RequestName { get; set; }

	public int? Sample { get; set; }

	public string PlanName { get; set; }

	public int? OK { get; set; }

	public int? NG { get; set; }

	public int? Empty { get; set; }

	public string Status { get; set; }

	public Guid? Id { get; set; }
}
