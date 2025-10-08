using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class AuthUserService : IAuthUserService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _hostingEnvironment;

	private readonly IHttpContextAccessor _httpContextAccessor;

	private readonly IConfiguration _configuration;

	private readonly IMapper _mapper;

	public AuthUserService(IUnitOfWork uow, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IMapper mapper)
	{
		_uow = uow;
		_hostingEnvironment = hostingEnvironment;
		_httpContextAccessor = httpContextAccessor;
		_configuration = configuration;
		_mapper = mapper;
	}

	public async Task<AuthUserViewModel> Login(LoginDto model)
	{
		try
		{
			string passwordHash = Encryptor.GenerateSaltedSHA1(model.Password);
			AuthUserViewModel att = await _uow.GetRepository<AuthUser>().GetSingleAsync<AuthUserViewModel>((AuthUser x) => x.Password.Equals(passwordHash) && x.Username.Equals(model.Username));
			if (att == null)
			{
				throw new Exception("Username or password incorrect");
			}
			if (!att.IsActivated)
			{
				throw new Exception("Authuser unactivated");
			}
			StringValues stringValues = _httpContextAccessor.HttpContext.Request.Headers["TYPE"];
			AuthUserViewModel result;
			if (!stringValues.Count.Equals(0))
			{
				if (stringValues.ToString().Equals("System"))
				{
					if (att.Username.ToLower().Equals("Admin".ToLower()))
					{
						result = att;
					}
					else
					{
						if (!(await IsLimitUpperUser(att)))
						{
							throw new Exception("Login limit exceeded");
						}
						result = att;
					}
				}
				else
				{
					if (!IsLimitUpperIME())
					{
						throw new Exception("IME incorrect");
					}
					result = att;
				}
			}
			else
			{
				if (!att.Username.ToLower().Equals("Admin".ToLower()))
				{
					throw new Exception("Structure incorrect");
				}
				result = att;
			}
			return result;
		}
		catch
		{
			throw;
		}
	}

	public async Task UpdateToken(string token, Guid userid)
	{
		_ = 1;
		try
		{
			AuthUser authUser = await _uow.GetRepository<AuthUser>().FindByIdAsync(userid);
			if (authUser == null)
			{
				throw new Exception($"Can't find account with id: {userid}");
			}
			authUser.RefreshToken = token;
			if (!authUser.Username.ToLower().Equals("Admin".ToLower()))
			{
				authUser.TimeLogin = DateTimeOffset.Now;
				StringValues stringValues = _httpContextAccessor.HttpContext.Request.Headers["TYPE"];
				if (stringValues.Count != 0)
				{
					authUser.LoginAt = stringValues.ToString();
				}
			}
			await _uow.CommitAsync();
		}
		catch
		{
			throw;
		}
	}

	public async Task<ResponseDto> Logout(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			AuthUser att = await _uow.GetRepository<AuthUser>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find account with id: {id}");
			}
			att.TimeLogin = null;
			att.RefreshToken = null;
			att.LoginAt = null;
			await _uow.CommitAsync();
			res.Data = att;
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

	public async Task<ResponseDto> ChangeMyPassword(ChangeMyPasswordDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			AuthUser att = await _uow.GetRepository<AuthUser>().GetSingleAsync((AuthUser x) => x.Username.Equals(model.Username), "");
			if (att == null)
			{
				throw new Exception("Username incorrect");
			}
			if (!Encryptor.GenerateSaltedSHA1(model.OldPassword).Equals(att.Password))
			{
				throw new Exception("Old password incorrect");
			}
			if (!model.ConfirmNewPassword.Equals(model.NewPassword))
			{
				throw new Exception("New password and confirm new password different");
			}
			if (string.IsNullOrEmpty(model.NewPassword))
			{
				throw new Exception("New password is empty");
			}
			if (model.NewPassword.Length < 8)
			{
				throw new Exception("New password has length < 8");
			}
			att.Password = Encryptor.GenerateSaltedSHA1(model.NewPassword);
			await _uow.CommitAsync();
			res.Data = att;
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

	public async Task<ResponseDto> ResetPassword(ResetPasswordDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			_ = model.UserId;
			AuthUser att = await _uow.GetRepository<AuthUser>().FindByIdAsync(model.UserId);
			if (att == null)
			{
				throw new Exception($"Can't find authuser with id: {model.UserId}");
			}
			att.Password = Encryptor.GenerateSaltedSHA1("user@123");
			await _uow.CommitAsync();
			res.Data = att;
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

	public async Task<ResponseDto> UpdateImage(Guid id, IFormFile file)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			AuthUser att = await _uow.GetRepository<AuthUser>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find authuser with id: {id}");
			}
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "AuthUserImage");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.ImageUrl))
			{
				string path = Path.Combine(text, att.ImageUrl);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			if (!file.Length.Equals(0L) && !string.IsNullOrEmpty(file.FileName))
			{
				string fileUniqueName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
				string path2 = Path.Combine(text, fileUniqueName);
				using FileStream stream = new FileStream(path2, FileMode.Create);
				await file.CopyToAsync(stream);
				att.ImageUrl = fileUniqueName;
			}
			else
			{
				att.ImageUrl = null;
			}
			await _uow.CommitAsync();
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

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<AuthUser>().FindByAsync<AuthUserViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<AuthUser>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Delete(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			AuthUser att = await _uow.GetRepository<AuthUser>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find authuser with id: {id}");
			}
			_uow.GetRepository<AuthUser>().Delete(att);
			await _uow.CommitAsync();
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "AuthUserImage");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.ImageUrl))
			{
				string path = Path.Combine(text, att.ImageUrl);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			res.Data = att;
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

	public async Task<ResponseDto> Save(AuthUserViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new AuthUser();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			AuthUser att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<AuthUser>().GetSingleAsync((AuthUser x) => x.Username.Equals(model.Username), "") != null)
				{
					throw new Exception("Username already exist");
				}
				att = new AuthUser();
				model.Password = Encryptor.GenerateSaltedSHA1("user@123");
				_mapper.Map(model, att);
				_uow.GetRepository<AuthUser>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<AuthUser>().GetSingleAsync((AuthUser x) => x.Username.Equals(model.Username) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Username already exist");
				}
				att = await _uow.GetRepository<AuthUser>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find authuser with id: {model.Id}");
				}
				model.Password = att.Password;
				model.LoginAt = att.LoginAt;
				model.TimeLogin = att.TimeLogin;
				model.RefreshToken = att.RefreshToken;
				_mapper.Map(model, att);
				_uow.GetRepository<AuthUser>().Update(att);
			}
			await _uow.CommitAsync();
			res.Data = att;
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

	public ResponseDto GetFuntions()
	{
		string empty = string.Empty;
		ResponseDto responseDto = new ResponseDto
		{
			Data = empty,
			Count = 1
		};
		try
		{
			string cryptedString = _configuration["Key"];
			empty = JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes")).Funtion;
			responseDto.Data = empty;
			responseDto.Count = 1;
		}
		catch (Exception ex)
		{
			responseDto.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return responseDto;
	}

	private async Task<bool> IsLimitUpperUser(AuthUserViewModel user)
	{
		string cryptedString = _configuration["Key"];
		ConfigApp conf = JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		if (await _uow.GetRepository<AuthUser>().CountAsync((AuthUser x) => !x.Id.Equals(user.Id) && !x.Username.Equals("Admin") && x.LoginAt.Equals("System")) >= conf.NumUserSystem)
		{
			return false;
		}
		return true;
	}

	private bool IsLimitUpperIME()
	{
		//string cryptedString = _configuration["Key"];
		//ConfigApp configApp = JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		//if (_httpContextAccessor.HttpContext.Request.Headers["IME"].Count.Equals(0) || configApp.ImeList == null || !configApp.ImeList.Split(";").Contains(_httpContextAccessor.HttpContext.Request.Headers["IME"].ToString()))
		//{
		//	return false;
		//}
		return true;
	}
}
