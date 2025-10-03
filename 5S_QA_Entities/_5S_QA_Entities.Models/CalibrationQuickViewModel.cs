using System;

namespace _5S_QA_Entities.Models;

public class CalibrationQuickViewModel
{
	public string CalibrationNo { get; set; }

	public string TypeName { get; set; }

	public string Company { get; set; }

	public string Staff { get; set; }

	public DateTimeOffset? CalDate { get; set; }

	public DateTimeOffset? ExpDate { get; set; }

	public int? Period { get; set; }

	public Guid? Id { get; set; }
}
