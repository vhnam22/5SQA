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
public class ProductGroupController : ControllerBase
{
	private readonly IProductGroupService _service;

	public ProductGroupController(IProductGroupService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _service.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _service.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] ProductGroupViewModel model)
	{
		return await _service.Save(model);
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> Copy(Guid id)
	{
		return await _service.Copy(id);
	}
}
