using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class RequestResultStatisticViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public string RequestType { get; set; }

	public string ProductCode { get; set; }

	public Guid? MeasurementId { get; set; }

	public string MeasurementName { get; set; }

	public int? NG { get; set; }
}
