using System;

namespace QA5SWebCore.ViewModels;

public class MachineForToolViewModel
{
	public string Name { get; set; }

	public string MachineTypeName { get; set; }

	public bool? IsHasTool { get; set; }

	public Guid? Id { get; set; }
}
