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
public class ToolController : ControllerBase
{
	private readonly IToolService _tool;

	public ToolController(IToolService tool)
	{
		_tool = tool;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _tool.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] ToolViewModel model)
	{
		return await _tool.Save(model);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> Result([FromBody] ToolResultDto model)
	{
		return await _tool.Result(model);
	}
}
