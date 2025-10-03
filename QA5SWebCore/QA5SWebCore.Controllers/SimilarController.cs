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
public class SimilarController : ControllerBase
{
	private readonly ISimilarService _similar;

	public SimilarController(ISimilarService similar)
	{
		_similar = similar;
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> Gets(Guid id)
	{
		return await _similar.Gets(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] SimilarViewModel model)
	{
		return await _similar.Save(model);
	}

	[HttpPost]
	public async Task<ResponseDto> Delete([FromBody] SimilarViewModel model)
	{
		return await _similar.Delete(model);
	}
}
