using System;

namespace _5S_QA_Entities.Models;

public class ProductQuickViewModel
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string GroupCodeName { get; set; }

	public string ImageUrl { get; set; }

	public int? TotalMeas { get; set; }

	public Guid? Id { get; set; }
}
