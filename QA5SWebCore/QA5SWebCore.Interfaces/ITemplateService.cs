using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface ITemplateService
{
	Task<ResponseDto> GetProductTemplates(Guid id);

	Task<ResponseDto> GetAllProductTemplates(Guid id);

	Task<ResponseDto> GetPlanTemplates(Guid id);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(TemplateViewModel model);

	Task<ResponseDto> SaveList(IEnumerable<TemplateViewModel> models);

	Task<ResponseDto> UpdateExcel(Guid id, IFormFile file);

	Task<CustomerFile> DownloadExcel(Guid id);

	Task<CustomerFile> Export(IEnumerable<ExportDto> models);

	Task<CustomerFile> ExportExcelChart(Guid? templateid, IEnumerable<ResultFullViewModel> models);

	Task<CustomerFile> ExportAllOnFile(IEnumerable<ExportDto> models);

	Task<CustomerFile> ExportForProduct(IEnumerable<ExportDto> models);

	Task<List<string>> ExportOneFile(RequestViewModel request, string type, string page, string sourcePath, string folderPath, bool isSample = false, Guid? templateId = null, string password = null);
}
