using System;

namespace QA5SWebCore.ViewModels;

public class CpkViewModel
{
	public Guid? Id { get; set; }

	public string Name { get; set; }

	public double? Cpk { get; set; }

	public string Rank { get; set; }
}
