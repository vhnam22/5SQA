using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class TemplateController : ControllerBase
{
	private readonly ITemplateService _template;

	private readonly IUnitOfWork _uow;

	public TemplateController(ITemplateService template, IUnitOfWork uow)
	{
		_template = template;
		_uow = uow;
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> GetProductTemplates(Guid id)
	{
		return await _template.GetProductTemplates(id);
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> GetAllProductTemplates(Guid id)
	{
		return await _template.GetAllProductTemplates(id);
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> GetPlanTemplates(Guid id)
	{
		return await _template.GetPlanTemplates(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _template.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _template.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] TemplateViewModel model)
	{
		return await _template.Save(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<TemplateViewModel> models)
	{
		return await _template.SaveList(models);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> UpdateExcel(Guid id, IFormFile file)
	{
		return await _template.UpdateExcel(id, file);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> DownloadExcel(Guid id)
	{
		try
		{
			CustomerFile customerFile = await _template.DownloadExcel(id);
			return File(customerFile.FileContents, "application/octet-stream", customerFile.FileName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Export([FromBody] IEnumerable<ExportDto> models)
	{
		try
		{
			if (models == null)
			{
				throw new Exception("List model is null");
			}
			CustomerFile customerFile = await _template.Export(models);
			Regex regex = new Regex("[>():]");
			string fileDownloadName = models.First().Name + "_" + regex.Replace(models.First().Type, "") + "_" + models.First().Page + Path.GetExtension(customerFile.FileName);
			return File(customerFile.FileContents, "application/octet-stream", fileDownloadName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("{id}")]
	public async Task<IActionResult> ExportExcelChart(Guid id, [FromBody] IEnumerable<ResultFullViewModel> models)
	{
		try
		{
			if (models == null)
			{
				throw new Exception("List result is null");
			}
			CustomerFile customerFile = await _template.ExportExcelChart(id, models);
			return File(customerFile.FileContents, "application/octet-stream", customerFile.FileName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost]
	public async Task<IActionResult> ExportAllOnFile([FromBody] IEnumerable<ExportDto> models)
	{
		try
		{
			if (models == null)
			{
				throw new Exception("List model is null");
			}
			CustomerFile customerFile = await _template.ExportAllOnFile(models);
			return File(customerFile.FileContents, "application/octet-stream", customerFile.FileName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost]
	public async Task<IActionResult> ExportForProduct([FromBody] IEnumerable<ExportDto> models)
	{
		try
		{
			if (models == null)
			{
				throw new Exception("List model is null");
			}
			CustomerFile customerFile = await _template.ExportForProduct(models);
			return File(customerFile.FileContents, "application/octet-stream", customerFile.FileName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}
