using System.ComponentModel.DataAnnotations;

namespace _5S_QA_Entities.Enums;

public enum FormType
{
	[Display(Name = "VIEW")]
	VIEW = 1,
	[Display(Name = "ADD")]
	ADD,
	[Display(Name = "EDIT")]
	EDIT,
	[Display(Name = "DEPARTMENT")]
	DEPARTMENT,
	[Display(Name = "FACTORY")]
	FACTORY,
	[Display(Name = "MACHINETYPE")]
	MACHINETYPE,
	[Display(Name = "IMPORTANT")]
	IMPORTANT,
	[Display(Name = "UNIT")]
	UNIT,
	[Display(Name = "STAGE")]
	STAGE,
	[Display(Name = "TYPE")]
	TYPE,
	[Display(Name = "LINE")]
	LINE,
	[Display(Name = "TYPECAL")]
	TYPECAL
}
