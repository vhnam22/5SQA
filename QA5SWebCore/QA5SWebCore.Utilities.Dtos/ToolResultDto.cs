using System;

namespace QA5SWebCore.Utilities.Dtos;

public class ToolResultDto
{
	public Guid MachineId { get; set; }

	public string MachineName { get; set; }

	public string Result { get; set; }

	public string Unit { get; set; }
}
