using System;

namespace _5S_QA_Entities.Models;

public class RequestQuickViewModel
{
	public string Code { get; set; }

	public string Name { get; set; }

	public int? Sample { get; set; }

	public string Status { get; set; }

	public Guid? Id { get; set; }
}
