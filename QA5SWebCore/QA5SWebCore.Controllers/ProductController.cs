using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ProductController : ControllerBase
{
	private readonly IProductService _product;

	public ProductController(IProductService product)
	{
		_product = product;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _product.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _product.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] ProductViewModel model)
	{
		return await _product.Save(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<ProductViewModel> models)
	{
		return await _product.SaveList(models);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> UpdateImage(Guid id, IFormFile file)
	{
		return await _product.UpdateImage(id, file);
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> Copy(Guid id)
	{
		return await _product.Copy(id);
	}

	[HttpPost("{idfrom}/{idto}")]
	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		return await _product.Move(idfrom, idto);
	}
}
