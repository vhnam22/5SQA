using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class RequestStatisticViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public DateTimeOffset? RequestDate { get; set; }

	public int? RequestSample { get; set; }

	public string RequestType { get; set; }

	public string ProductCode { get; set; }

	public int? ProductCavity { get; set; }

	public int? MeasAll { get; set; }

	public int? ResultNG { get; set; }

	public int? ResultAll { get; set; }

	public int? SampleNG { get; set; }
}
