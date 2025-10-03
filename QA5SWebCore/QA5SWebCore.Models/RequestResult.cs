using System;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class RequestResult : AuditableEntity
{
	public Guid RequestId { get; set; }

	public virtual Request Request { get; set; }

	public Guid? MeasurementId { get; set; }

	public virtual Measurement Measurement { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public string Result { get; set; }

	public string ResultOrigin { get; set; }

	public string Judge { get; set; }

	public string MachineName { get; set; }

	public string StaffName { get; set; }

	public string History { get; set; }

	public int Sample { get; set; }

	public int Cavity { get; set; }
}
