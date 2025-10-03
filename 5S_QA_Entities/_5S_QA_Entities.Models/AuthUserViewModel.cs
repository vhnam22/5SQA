using System;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Enums;

namespace _5S_QA_Entities.Models;

public class AuthUserViewModel : AuditableEntity
{
	public string Username { get; set; }

	public string FullName { get; set; }

	public string Email { get; set; }

	public bool? IsEmail { get; set; }

	public DateTime? BirthDate { get; set; }

	public string Gender { get; set; }

	public string PhoneNumber { get; set; }

	public string Address { get; set; }

	public string JobTitle { get; set; }

	public string Position { get; set; }

	public Guid? DepartmentId { get; set; }

	public string DepartmentName { get; set; }

	public string ImageUrl { get; set; }

	public string LoginAt { get; set; }

	public DateTimeOffset? TimeLogin { get; set; }

	public RoleWeb Role { get; set; }

	public string Token { get; set; }
}
