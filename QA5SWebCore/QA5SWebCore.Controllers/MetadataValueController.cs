using System;
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
public class MetadataValueController : ControllerBase
{
	private readonly IMetadataValueService _meta;

	public MetadataValueController(IMetadataValueService meta)
	{
		_meta = meta;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> NoAuthorGets([FromBody] QueryArgs args)
	{
		return await _meta.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _meta.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> GetDecimals([FromBody] QueryArgs args)
	{
		return await _meta.GetDecimals(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _meta.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] MetadataValueViewModel model)
	{
		return await _meta.Save(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveRanks([FromBody] IEnumerable<MetadataValueViewModel> models)
	{
		return await _meta.SaveRanks(models);
	}
}
