using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface ICalibrationService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(CalibrationViewModel model);

	Task<ResponseDto> UpdateFile(Guid id, IFormFile file);

	Task<CustomerFile> DownloadFile(Guid id);
}
