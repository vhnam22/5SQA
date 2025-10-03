using System.ComponentModel.DataAnnotations;

namespace QA5SWebCore.Utilities.Enums;

public enum RoleWeb
{
	[Display(Name = "None")]
	None,
	[Display(Name = "SuperAdministrator")]
	SuperAdministrator,
	[Display(Name = "Administrator")]
	Administrator,
	[Display(Name = "Member")]
	Member
}
