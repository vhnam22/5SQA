using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class MachineController : ControllerBase
{
	private readonly IMachineService _machine;

	public MachineController(IMachineService machine)
	{
		_machine = machine;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> NoAuthorGets([FromBody] QueryArgs args)
	{
		return await _machine.Gets(args);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> GetForTools(Guid id, [FromBody] QueryArgs args)
	{
		return await _machine.GetForTools(id, args);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _machine.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _machine.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] MachineViewModel model)
	{
		return await _machine.Save(model);
	}

	[HttpGet("{ime}")]
	public async Task<ResponseDto> GetLiences(string ime)
	{
		return await _machine.GetLiences(ime);
	}
}
