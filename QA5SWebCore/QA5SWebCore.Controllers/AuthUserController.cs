using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class AuthUserController : ControllerBase
{
	private readonly IAuthUserService _auth;

	public AuthUserController(IAuthUserService auth)
	{
		_auth = auth;
	}

	private IEnumerable<Claim> GetValidClaims(AuthUserViewModel user)
	{
		IdentityOptions identityOptions = new IdentityOptions();
		List<Claim> list = new List<Claim>();
		list.Add(new Claim("sub", user.Username));
		list.Add(new Claim("jti", user.Id.ToString()));
		list.Add(new Claim("UserId", user.Id.ToString()));
		list.Add(new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()));
		list.Add(new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.FullName));
		list.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", typeof(AuthUser).Name));
		list.Add(new Claim(identityOptions.ClaimsIdentity.RoleClaimType, user.Role.ToString()));
		return list;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> Login([FromBody] LoginDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			AuthUserViewModel authUserViewModel = await _auth.Login(model);
			if (authUserViewModel == null)
			{
				throw new Exception("Username or password incorrect");
			}
			IEnumerable<Claim> validClaims = GetValidClaims(authUserViewModel);
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0B5E65FDBBBC4FB18946D4A9CCDBAB4A0B5E65FDBBBC4FB18946D4A9CCDBAB4A0B5E65FDBBBC4FB18946D4A9CCDBAB4A"));
			SigningCredentials signingCredentials = new SigningCredentials(key, "HS256");
			JwtSecurityToken token = new JwtSecurityToken("WebCore", "WebCore", validClaims, null, DateTime.Now.AddDays(1.0), signingCredentials);
			string token2 = new JwtSecurityTokenHandler().WriteToken(token);
			res.Data = new
			{
				Id = authUserViewModel.Id,
				Username = authUserViewModel.Username,
				FullName = authUserViewModel.FullName,
				ImageUrl = authUserViewModel.ImageUrl,
				JobTitle = authUserViewModel.JobTitle,
				DepartmentId = authUserViewModel.DepartmentId,
				DepartmentName = authUserViewModel.DepartmentName,
				Role = authUserViewModel.Role,
				Token = token2
			};
			await _auth.UpdateToken(token2, authUserViewModel.Id);
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> Logout(Guid id)
	{
		return await _auth.Logout(id);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> ChangeMyPassword([FromBody] ChangeMyPasswordDto model)
	{
		return await _auth.ChangeMyPassword(model);
	}

	[HttpPost]
	public async Task<ResponseDto> ResetPassword([FromBody] ResetPasswordDto model)
	{
		return await _auth.ResetPassword(model);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> UpdateImage(Guid id, IFormFile file)
	{
		return await _auth.UpdateImage(id, file);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _auth.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _auth.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] AuthUserViewModel model)
	{
		return await _auth.Save(model);
	}

	[HttpGet]
	public ResponseDto GetFuntions()
	{
		return _auth.GetFuntions();
	}
}
