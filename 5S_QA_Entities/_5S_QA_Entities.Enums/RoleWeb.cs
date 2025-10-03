using System.ComponentModel.DataAnnotations;

namespace _5S_QA_Entities.Enums;

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
