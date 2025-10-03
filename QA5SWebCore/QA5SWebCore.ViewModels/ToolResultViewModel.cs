using System;

namespace QA5SWebCore.ViewModels;

public class ToolResultViewModel
{
	public Guid? TabletId { get; set; }

	public Guid? MachineId { get; set; }

	public string MachineName { get; set; }

	public string MachineTypeName { get; set; }

	public string Result { get; set; }

	public string Unit { get; set; }
}
