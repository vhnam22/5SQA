using System;

namespace QA5SWebCore.Utilities.Dtos;

public class AQLDto
{
	public Guid? ProductId { get; set; }

	public Guid? RequestId { get; set; }

	public int? InputQuantity { get; set; }
}
