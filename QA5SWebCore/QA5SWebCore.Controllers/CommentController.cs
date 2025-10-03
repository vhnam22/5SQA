using System;
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
public class CommentController : ControllerBase
{
	private readonly ICommentService _comment;

	public CommentController(ICommentService comment)
	{
		_comment = comment;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _comment.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _comment.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] CommentViewModel model)
	{
		return await _comment.Save(model);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> UpdateFile(Guid id, IFormFile file)
	{
		return await _comment.UpdateFile(id, file);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> DownloadFile(Guid id)
	{
		try
		{
			CustomerFile customerFile = await _comment.DownloadFile(id);
			return File(customerFile.FileContents, "application/octet-stream", customerFile.FileName);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}
