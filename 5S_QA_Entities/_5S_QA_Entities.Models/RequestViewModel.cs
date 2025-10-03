using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class RequestViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public Guid? GroupId { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductStage { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public string ProductDescription { get; set; }

	public string ProductDepartment { get; set; }

	public string ProductImageUrl { get; set; }

	public int? ProductCavity { get; set; }

	public DateTimeOffset? Date { get; set; }

	public int? Quantity { get; set; }

	public string Lot { get; set; }

	public string Line { get; set; }

	public string Intention { get; set; }

	public DateTimeOffset? InputDate { get; set; }

	public string Supplier { get; set; }

	public int? Sample { get; set; }

	public string Type { get; set; }

	public string Status { get; set; }

	public string Judgement { get; set; }

	public string Link { get; set; }

	public string CompletedBy { get; set; }

	public DateTimeOffset? Completed { get; set; }

	public string CheckedBy { get; set; }

	public DateTimeOffset? Checked { get; set; }

	public string ApprovedBy { get; set; }

	public DateTimeOffset? Approved { get; set; }

	public int? TotalComment { get; set; }
}
