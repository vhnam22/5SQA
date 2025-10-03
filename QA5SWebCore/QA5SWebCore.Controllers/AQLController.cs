using System;
using System.Collections.Generic;
using System.Linq;
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
public class AQLController : ControllerBase
{
	private readonly IAQLService _service;

	public AQLController(IAQLService service)
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
	public async Task<ResponseDto> Save([FromBody] AQLViewModel model)
	{
		return await _service.Save(model);
	}

	[HttpPost("{idfrom}/{idto}")]
	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		return await _service.Move(idfrom, idto);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<AQLViewModel> models)
	{
		return await _service.SaveList(models);
	}

	[HttpPost]
	public async Task<ResponseDto> DeleteList([FromBody] IEnumerable<Guid> ids)
	{
		foreach (Guid id in ids)
		{
			await _service.Delete(id);
		}
		return new ResponseDto
		{
			Data = ids,
			Count = ids.Count()
		};
	}

	[HttpPost]
	public async Task<ResponseDto> Samples([FromBody] AQLDto model)
	{
		return await _service.Samples(model);
	}
}
