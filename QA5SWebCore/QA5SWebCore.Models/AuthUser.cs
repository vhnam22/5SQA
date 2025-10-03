using System;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;
using QA5SWebCore.Utilities.Enums;

namespace QA5SWebCore.Models;

public class AuthUser : AuditableEntity
{
	[Required]
	public string FullName { get; set; }

	public string Email { get; set; }

	public DateTimeOffset? BirthDate { get; set; }

	[Required]
	public string Gender { get; set; }

	public string PhoneNumber { get; set; }

	public string Address { get; set; }

	public string JobTitle { get; set; }

	[Required]
	public string Position { get; set; }

	public Guid? DepartmentId { get; set; }

	public virtual MetadataValue Department { get; set; }

	public string ImageUrl { get; set; }

	[Required]
	public string Username { get; set; }

	[Required]
	public string Password { get; set; }

	public string LoginAt { get; set; }

	public DateTimeOffset? TimeLogin { get; set; }

	public string RefreshToken { get; set; }

	public RoleWeb Role { get; set; }

	public bool? IsEmail { get; set; }
}
