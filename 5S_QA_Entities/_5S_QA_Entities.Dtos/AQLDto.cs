using System;

namespace _5S_QA_Entities.Dtos;

public class AQLDto
{
	public Guid? ProductId { get; set; }

	public Guid? RequestId { get; set; }

	public int? InputQuantity { get; set; }
}
