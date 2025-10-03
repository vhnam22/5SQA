using System.Collections.Generic;
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
public class ResultRankController : ControllerBase
{
	private readonly IResultRankService _service;

	public ResultRankController(IResultRankService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _service.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<ResultRankViewModel> models)
	{
		return await _service.SaveList(models);
	}
}
