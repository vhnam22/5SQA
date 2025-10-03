using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IAuthUserService
{
	Task<AuthUserViewModel> Login(LoginDto model);

	Task UpdateToken(string token, Guid userid);

	Task<ResponseDto> Logout(Guid id);

	Task<ResponseDto> ChangeMyPassword(ChangeMyPasswordDto model);

	Task<ResponseDto> ResetPassword(ResetPasswordDto model);

	Task<ResponseDto> UpdateImage(Guid id, IFormFile file);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(AuthUserViewModel model);

	ResponseDto GetFuntions();
}
