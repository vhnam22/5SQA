using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IResultFileService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Save(ResultFileViewModel model);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> UploadFile(Guid id, IFormFile file);
}
