using System;

namespace _5SQA_Read_UWave.Models;

public class ToolResultDto
{
	public Guid? MachineId { get; set; }

	public string Result { get; set; }

	public string Unit { get; set; }
}
