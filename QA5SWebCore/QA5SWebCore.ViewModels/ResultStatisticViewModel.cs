using System;

namespace QA5SWebCore.ViewModels;

public class ResultStatisticViewModel
{
	public Guid? RequestId { get; set; }

	public DateTimeOffset? RequestDate { get; set; }

	public int? RequestSample { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductCode { get; set; }

	public Guid? MeasurementId { get; set; }

	public int? Sample { get; set; }

	public int? Cavity { get; set; }

	public string Judge { get; set; }
}
