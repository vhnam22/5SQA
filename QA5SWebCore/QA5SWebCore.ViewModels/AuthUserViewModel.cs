using System;
using QA5SWebCore.Data.Abstracts;
using QA5SWebCore.Utilities.Enums;

namespace QA5SWebCore.ViewModels;

public class AuthUserViewModel : AuditableEntity
{
	public string FullName { get; set; }

	public string Email { get; set; }

	public DateTimeOffset? BirthDate { get; set; }

	public string Gender { get; set; }

	public string PhoneNumber { get; set; }

	public string Address { get; set; }

	public string JobTitle { get; set; }

	public string Position { get; set; }

	public Guid? DepartmentId { get; set; }

	public string DepartmentName { get; set; }

	public string ImageUrl { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public DateTimeOffset? TimeLogin { get; set; }

	public string LoginAt { get; set; }

	public string RefreshToken { get; set; }

	public RoleWeb? Role { get; set; }

	public bool? IsEmail { get; set; }
}
