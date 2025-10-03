using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class AQLViewModel : AuditableEntity
{
	public bool? IsSelect { get; set; }

	public string Type { get; set; }

	public int? InputQuantity { get; set; }

	public int? Sample { get; set; }

	public Guid? ProductId { get; set; }
}
