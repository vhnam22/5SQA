using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IProductService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(ProductViewModel model);

	Task<ResponseDto> SaveList(IEnumerable<ProductViewModel> models);

	Task<ResponseDto> UpdateImage(Guid id, IFormFile file);

	Task<ResponseDto> Copy(Guid id);

	Task<ResponseDto> Move(Guid idfrom, Guid idto);
}
