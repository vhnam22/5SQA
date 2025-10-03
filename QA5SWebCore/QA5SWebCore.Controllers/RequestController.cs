using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class RequestController : ControllerBase
{
	private readonly IRequestService _request;

	public RequestController(IRequestService request)
	{
		_request = request;
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> Gets(Guid id)
	{
		return await _request.Gets(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _request.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _request.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] RequestViewModel model)
	{
		return await _request.Save(model);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> UpdateFile(Guid id, IFormFile file)
	{
		return await _request.UpdateFile(id, file);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> DownloadFile(Guid id)
	{
		try
		{
			CustomerFile customerFile = await _request.DownloadFile(id);
			return File(customerFile.FileContents, "application/octet-stream", customerFile.FileName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost]
	public async Task<ResponseDto> SaveSample([FromBody] RequestViewModel model)
	{
		return await _request.SaveSample(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<RequestViewModel> models)
	{
		return await _request.SaveList(models);
	}

	[HttpPost]
	public async Task<ResponseDto> Active([FromBody] ActiveRequestDto model)
	{
		return await _request.Active(model);
	}

	[HttpPost]
	public async Task<ResponseDto> ActiveList([FromBody] IEnumerable<ActiveRequestDto> models)
	{
		ResponseDto result = new ResponseDto();
		foreach (ActiveRequestDto model in models)
		{
			result = await _request.Active(model);
		}
		return result;
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> SaveListResult(Guid id, [FromBody] IEnumerable<Guid> ids)
	{
		return await _request.SaveListResult(id, ids);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> UpdateStatistics()
	{
		return await _request.UpdateStatistics();
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> GetCpks(Guid id)
	{
		return await _request.GetCpks(id);
	}
}
