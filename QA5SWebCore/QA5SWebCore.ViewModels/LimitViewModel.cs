using System;

namespace QA5SWebCore.ViewModels;

public class LimitViewModel
{
	public Guid? MeasurementId { get; set; }

	public string Type { get; set; }

	public int? Sample { get; set; }
}
