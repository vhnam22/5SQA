using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class ToolViewModel : AuditableEntity
{
	public Guid? TabletId { get; set; }

	public string TabletCode { get; set; }

	public string TabletName { get; set; }

	public Guid? MachineId { get; set; }

	public string MachineCode { get; set; }

	public string MachineName { get; set; }

	public string MachineTypeName { get; set; }
}
