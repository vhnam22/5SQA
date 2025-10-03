using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class MachineViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Model { get; set; }

	public string Serial { get; set; }

	public Guid? FactoryId { get; set; }

	public string FactoryName { get; set; }

	public Guid? MachineTypeId { get; set; }

	public string MachineTypeName { get; set; }

	public string Status { get; set; }

	public int? Mark { get; set; }

	public DateTimeOffset? ExpDate { get; set; }
}
