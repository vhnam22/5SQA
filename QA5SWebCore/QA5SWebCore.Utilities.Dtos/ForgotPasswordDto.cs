using System;

namespace QA5SWebCore.Utilities.Dtos;

public class ForgotPasswordDto
{
	public string Email { get; set; }

	public string PasswordForReset { get; set; }

	public Guid? Id { get; set; }
}
