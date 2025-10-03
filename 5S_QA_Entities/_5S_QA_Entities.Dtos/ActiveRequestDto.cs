using System;

namespace _5S_QA_Entities.Dtos;

public class ActiveRequestDto
{
	public Guid? Id { get; set; }

	public string Judgement { get; set; }

	public string Status { get; set; }
}
