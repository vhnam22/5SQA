using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IRequestService
{
	Task<ResponseDto> Gets(Guid id);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(RequestViewModel model);

	Task<ResponseDto> UpdateFile(Guid id, IFormFile file);

	Task<CustomerFile> DownloadFile(Guid id);

	Task<ResponseDto> SaveSample(RequestViewModel model);

	Task<ResponseDto> SaveList(IEnumerable<RequestViewModel> models);

	Task<ResponseDto> Active(ActiveRequestDto model);

	Task<ResponseDto> SaveListResult(Guid id, IEnumerable<Guid> ids);

	Task<ResponseDto> UpdateStatistics();

	Task<ResponseDto> GetCpks(Guid id);
}
