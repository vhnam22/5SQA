using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Constants;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class TemplateService : ITemplateService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _hostingEnvironment;

	public TemplateService(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
	{
		_uow = unitOfWork;
		_hostingEnvironment = hostingEnvironment;
	}

	public async Task<ResponseDto> GetProductTemplates(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			Product product = (await _uow.GetRepository<Product>().FindByIdAsync(id)) ?? throw new Exception($"Can't find product with ID: {id}");
			Template data = (await _uow.GetRepository<Template>().FindByIdAsync(product.TemplateId.Value)) ?? throw new Exception("This product hasn't template");
			res.Data = data;
			res.Count = 1;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> GetAllProductTemplates(Guid id)
	{
		ResponseDto res = new ResponseDto();
		List<Template> temps = new List<Template>();
		try
		{
			ProductViewModel productViewModel = (await _uow.GetRepository<Product>().FindByIdAsync<ProductViewModel>(id)) ?? throw new Exception($"Can't find product with ID: {id}");
			if (productViewModel.TemplateId.HasValue)
			{
				temps.Add(new Template
				{
					Id = productViewModel.TemplateId.Value,
					Name = productViewModel.TemplateName
				});
			}
			foreach (TemplateOtherViewModel item in await _uow.GetRepository<TemplateOther>().FindByAsync<TemplateOtherViewModel>((TemplateOther x) => x.ProductId == id))
			{
				temps.Add(new Template
				{
					Id = item.TemplateId.Value,
					Name = item.TemplateName
				});
			}
			res.Data = temps;
			res.Count = temps.Count;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> GetPlanTemplates(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			Plan plan = await _uow.GetRepository<Plan>().FindByIdAsync(id);
			if (plan == null)
			{
				throw new Exception($"Can't find plan with ID: {id}");
			}
			Template template = await _uow.GetRepository<Template>().FindByIdAsync(plan.TemplateId.Value);
			if (template == null)
			{
				throw new Exception("This product hasn't template");
			}
			res.Data = template;
			res.Count = 1;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Template>().FindByAsync<TemplateViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Template>().CountAsync(args.Predicate, args.PredicateParameters);
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> Delete(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Template att = await _uow.GetRepository<Template>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find template with id: {id}");
			}
			_uow.GetRepository<Template>().Delete(att);
			await _uow.CommitAsync();
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.TemplateUrl))
			{
				string path = Path.Combine(text, att.TemplateUrl);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			res.Data = att;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> Save(TemplateViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Template();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Template att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<Template>().GetSingleAsync((Template x) => x.Code.Equals(model.Code) || x.Name.Equals(model.Name), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = new Template();
				Mapper.Map(model, att);
				_uow.GetRepository<Template>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<Template>().GetSingleAsync((Template x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = await _uow.GetRepository<Template>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find template with id: {model.Id}");
				}
				model.TemplateData = att.TemplateData;
				Mapper.Map(model, att);
				_uow.GetRepository<Template>().Update(att);
			}
			await _uow.CommitAsync();
			res.Data = att;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> SaveList(IEnumerable<TemplateViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (models == null)
			{
				throw new Exception("Models is null");
			}
			List<Template> anons = new List<Template>();
			foreach (var item in models.Select((TemplateViewModel Value, int Index) => new { Value, Index }))
			{
				TemplateViewModel model = item.Value;
				int index = item.Index + 1;
				string Message = string.Empty;
				if (string.IsNullOrEmpty(model.Code))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Code is empty;" : " Code is empty;");
				}
				else if (!model.Code.StartsWith("TEMP-"))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Code incorrect;" : " Code incorrect;");
				}
				else
				{
					if (await _uow.GetRepository<Template>().GetSingleAsync((Template x) => x.Code.Equals(model.Code), "") != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
					Template template = anons.FirstOrDefault((Template x) => x.Code.Equals(model.Code));
					if (template != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
				}
				if (string.IsNullOrEmpty(model.Name))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Name is empty;" : " Name is empty;");
				}
				else
				{
					if (await _uow.GetRepository<Template>().GetSingleAsync((Template x) => x.Name.Equals(model.Name), "") != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Name already exist;" : " Name already exist;");
					}
					Template template2 = anons.FirstOrDefault((Template x) => x.Code.Equals(model.Code));
					if (template2 != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Name already exist;" : " Name already exist;");
					}
				}
				if (!model.Limit.HasValue)
				{
					Message += ((Message == string.Empty) ? $"No {index}: Limit is empty;" : " Limit is empty;");
				}
				else
				{
					model.Limit = (model.Limit.Equals(0) ? new int?(7) : model.Limit);
				}
				if (string.IsNullOrEmpty(model.Type))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Type is empty;" : " Type is empty;");
				}
				else if (!model.Type.Equals("Detail") && !model.Type.Equals("Chart") && !model.Type.Equals("Special"))
				{
					Message += ((Message == string.Empty) ? string.Format("No {0}: Type isn't '{1}', '{2}', '{3}';", index, "Detail", "Chart", "Special") : " Type isn't 'Detail', 'Chart', 'Special';");
				}
				anons.Add(Mapper.Map<Template>(model));
				if (!string.IsNullOrEmpty(Message))
				{
					res.Messages.Add(new ResponseMessage
					{
						Code = "Error",
						Message = Message
					});
				}
			}
			if (res.Messages.Count > 0)
			{
				return res;
			}
			_uow.GetRepository<Template>().Add(anons);
			await _uow.CommitAsync();
			res.Data = anons;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> UpdateExcel(Guid id, IFormFile file)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Template template = (await _uow.GetRepository<Template>().FindByIdAsync(id)) ?? throw new Exception($"Can't find template with id: {id}");
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(template.TemplateUrl))
			{
				string path = Path.Combine(text, template.TemplateUrl);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			if (file.Length != 0L && !string.IsNullOrEmpty(file.FileName))
			{
				string text2 = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
				string path2 = Path.Combine(text, text2);
				using Stream _stream = file.OpenReadStream();
				List<ExportMappingDto> list = new List<ExportMappingDto>();
				using (ExcelPackage excelPackage = new ExcelPackage(_stream))
				{
					if (excelPackage.Workbook.Worksheets.Count < 1)
					{
						throw new Exception("File excel import is incorect format");
					}
					foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
					{
						if (worksheet.Dimension == null)
						{
							throw new Exception("Sheet excel import is null");
						}
						ExcelRange excelRange = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
						if (!excelRange.Any())
						{
							throw new Exception("Content of sheet excel import is null");
						}
						foreach (ExcelRangeBase item2 in excelRange)
						{
							if (item2.Value == null)
							{
								continue;
							}
							string text3 = item2.Value.ToString().Trim().ToUpper();
							if (text3.StartsWith("[[") && text3.EndsWith("]]"))
							{
								ExportMappingDto item = new ExportMappingDto(item2.Address, text3, worksheet.Name);
								if (!template.Type.Equals("Chart"))
								{
									list.Add(item);
								}
								else if (string.IsNullOrEmpty(item2.Formula))
								{
									list.Add(item);
								}
							}
						}
					}
				}
				if (list.Count < 1)
				{
					throw new Exception("No configuration found in the file");
				}
				using FileStream stream = new FileStream(path2, FileMode.Create);
				template.TemplateUrl = text2;
				template.TemplateData = System.Text.Json.JsonSerializer.Serialize(list);
				await file.CopyToAsync(stream);
				await _uow.CommitAsync();
			}
			else
			{
				template.TemplateUrl = null;
				template.TemplateData = null;
				await _uow.CommitAsync();
			}
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<CustomerFile> DownloadExcel(Guid id)
	{
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
		Directory.CreateDirectory(sourcePath);
		Template template = await _uow.GetRepository<Template>().FindByIdAsync(id);
		if (template == null)
		{
			throw new Exception($"Can't find template with id: {id}");
		}
		if (string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("Template hasn't file");
		}
		string path = Path.Combine(sourcePath, template.TemplateUrl);
		byte[] fileContents = File.ReadAllBytes(path);
		return new CustomerFile(fileContents, template.Name + Path.GetExtension(path));
	}

	public async Task<CustomerFile> Export(IEnumerable<ExportDto> models)
	{
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
		string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFileExport");
		Directory.CreateDirectory(sourcePath);
		Directory.CreateDirectory(folderPath);
		MetadataValue cfg = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name == "DirFile", "");
		List<string> listFileName = new List<string>();
		foreach (ExportDto model2 in models)
		{
			ExportDto model = model2;
			RequestViewModel request = (await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(model.Id)) ?? throw new Exception($"Can't find request with id: {model.Id}");
			if (model.Page.Equals("All"))
			{
				switch (model.Type)
				{
				case "Product":
					if (await _uow.GetRepository<Template>().CountAsync((Template x) => ((object)x.Id).Equals((object?)model.TemplateId) && !string.IsNullOrEmpty(x.TemplateUrl)) > 0)
					{
						List<string> list = listFileName;
						list.AddRange(await ExportOneFile(request, model.Type, string.Empty, sourcePath, folderPath, isSample: false, model.TemplateId, cfg?.Description));
					}
					break;
				case "Plan":
					foreach (Plan item in await _uow.GetRepository<Plan>().FindByAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId) && !string.IsNullOrEmpty(x.Template.TemplateUrl), "Sort"))
					{
						List<string> list = listFileName;
						list.AddRange(await ExportOneFile(request, item.Id.ToString(), string.Empty, sourcePath, folderPath, isSample: false, null, cfg?.Description));
					}
					break;
				case "Sample":
					if ((await _uow.GetRepository<Plan>().CountAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId))).Equals(0))
					{
						if (await _uow.GetRepository<Template>().CountAsync((Template x) => ((object)x.Id).Equals((object?)model.TemplateId) && !string.IsNullOrEmpty(x.TemplateUrl)) > 0)
						{
							List<string> list = listFileName;
							list.AddRange(await ExportOneFile(request, "Product", string.Empty, sourcePath, folderPath, isSample: true, model.TemplateId, cfg?.Description));
						}
						break;
					}
					foreach (Plan item2 in await _uow.GetRepository<Plan>().FindByAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId) && !string.IsNullOrEmpty(x.Template.TemplateUrl), "Sort"))
					{
						List<string> list = listFileName;
						list.AddRange(await ExportOneFile(request, item2.Id.ToString(), string.Empty, sourcePath, folderPath, isSample: true, null, cfg?.Description));
					}
					break;
				case "All":
					if ((await _uow.GetRepository<Plan>().CountAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId))).Equals(0))
					{
						if (await _uow.GetRepository<Template>().CountAsync((Template x) => ((object)x.Id).Equals((object?)model.TemplateId) && !string.IsNullOrEmpty(x.TemplateUrl)) > 0)
						{
							List<string> list = listFileName;
							list.AddRange(await ExportOneFile(request, "Product", string.Empty, sourcePath, folderPath, isSample: false, model.TemplateId, cfg?.Description));
						}
						break;
					}
					foreach (Plan item3 in await _uow.GetRepository<Plan>().FindByAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId) && !string.IsNullOrEmpty(x.Template.TemplateUrl), "Sort"))
					{
						List<string> list = listFileName;
						list.AddRange(await ExportOneFile(request, item3.Id.ToString(), string.Empty, sourcePath, folderPath, isSample: false, null, cfg?.Description));
					}
					break;
				default:
				{
					Guid idplan = Guid.Parse(model.Type);
					if (await _uow.GetRepository<Plan>().CountAsync((Plan x) => x.Id.Equals(idplan) && !string.IsNullOrEmpty(x.Template.TemplateUrl)) > 0)
					{
						List<string> list = listFileName;
						list.AddRange(await ExportOneFile(request, model.Type, string.Empty, sourcePath, folderPath, isSample: false, null, cfg?.Description));
					}
					break;
				}
				}
				continue;
			}
			string type = model.Type;
			if (type == "Sample")
			{
				if ((await _uow.GetRepository<Plan>().CountAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId))).Equals(0))
				{
					List<string> list = listFileName;
					list.AddRange(await ExportOneFile(request, "Product", model.Page, sourcePath, folderPath, isSample: true, model.TemplateId, cfg?.Description));
					continue;
				}
				foreach (Plan item4 in await _uow.GetRepository<Plan>().FindByAsync((Plan x) => ((object)x.ProductId).Equals((object?)request.ProductId) && !string.IsNullOrEmpty(x.Template.TemplateUrl), "Sort"))
				{
					List<string> list = listFileName;
					list.AddRange(await ExportOneFile(request, item4.Id.ToString(), model.Page, sourcePath, folderPath, isSample: true, null, cfg?.Description));
				}
			}
			else
			{
				List<string> list = listFileName;
				list.AddRange(await ExportOneFile(request, model.Type, model.Page, sourcePath, folderPath, isSample: false, model.TemplateId, cfg?.Description));
			}
		}
		return CalFiles(listFileName, folderPath, models.First().Name);
	}

	public async Task<CustomerFile> ExportExcelChart(Guid? templateid, IEnumerable<ResultFullViewModel> models)
	{
		Regex pattern = new Regex("[>():]");
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
		string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFileExport");
		Directory.CreateDirectory(sourcePath);
		Directory.CreateDirectory(folderPath);
		_ = string.Empty;
		List<string> listFileName = new List<string>();
		List<PropertyInfo> propMeass = new List<PropertyInfo>(typeof(ChartViewModel).GetProperties());
		IEnumerable<IGrouping<Guid?, ResultFullViewModel>> groupAlls = from x in models
			group x by x.MeasurementId;
		Template template = await _uow.GetRepository<Template>().FindByIdAsync(templateid.Value);
		if (template == null || string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("File template for chart is emtry");
		}
		ExportMappingDto formatResult = null;
		List<ExportMappingDto> formatMeass = new List<ExportMappingDto>();
		List<ExportMappingDto> list = JsonConvert.DeserializeObject<List<ExportMappingDto>>(template.TemplateData);
		foreach (ExportMappingDto item in list)
		{
			if (item.Value.StartsWith("[[RESULT#") && item.Value.EndsWith("]]"))
			{
				if (formatResult == null)
				{
					formatResult = item;
				}
			}
			else
			{
				formatMeass.Add(item);
			}
		}
		if (formatResult == null)
		{
			throw new Exception("Template isn't correct format");
		}
		string[] array = formatResult.Value.Split("#");
		if (array.Length < 2)
		{
			throw new Exception("Template isn't correct format");
		}
		string mOrientation = array[1].Replace("]]", "");
		string sourceFile = Path.Combine(sourcePath, template.TemplateUrl);
		Product product = await _uow.GetRepository<Product>().FindByIdAsync(models.ToList().FirstOrDefault().ProductId.Value);
		foreach (IGrouping<Guid?, ResultFullViewModel> group in groupAlls)
		{
			string text = pattern.Replace(group.First().Name + "_" + template.Name, "");
			string destFile = Path.Combine(folderPath, text + Path.GetExtension(template.TemplateUrl));
			File.Copy(sourceFile, destFile, overwrite: true);
			FileInfo newFile = new FileInfo(destFile);
			using (ExcelPackage pck = new ExcelPackage(newFile))
			{
				ExcelWorksheet ws = pck.Workbook.Worksheets["Data"];
				int _colpage = ws.Cells[formatResult.CellAddress].Start.Column;
				int _rowpage = ws.Cells[formatResult.CellAddress].Start.Row;
				ChartViewModel chartViewModel = await _uow.GetRepository<Measurement>().FindByIdAsync<ChartViewModel>(group.Key.Value);
				IEnumerable<IGrouping<Guid?, ResultFullViewModel>> source = from x in @group
					group x by x.RequestId;
				chartViewModel.DateFrom = group.FirstOrDefault().RequestDate;
				chartViewModel.DateTo = group.ToList()[group.Count() - 1].RequestDate;
				foreach (var item2 in source.Select((IGrouping<Guid?, ResultFullViewModel> value, int index) => new { index, value }))
				{
					foreach (ResultFullViewModel item3 in item2.value)
					{
						int? num = item3.Cavity - 1 + (item3.Sample - 1) * product.Cavity;
						if (mOrientation == "V")
						{
							ws.Cells[_rowpage + item2.index, _colpage + num.Value].Value = double.Parse(item3.Result);
						}
						else
						{
							ws.Cells[_rowpage + num.Value, _colpage + item2.index].Value = double.Parse(item3.Result);
						}
					}
				}
				foreach (ExportMappingDto format in formatMeass)
				{
					new object();
					PropertyInfo propertyInfo = propMeass.Where((PropertyInfo x) => format.Value.StartsWith("[[" + x.Name.ToUpper() + "]]")).FirstOrDefault();
					dynamic val = ((!(propertyInfo == null)) ? propertyInfo.GetValue(chartViewModel, null) : null);
					if ((object)val != null)
					{
						string text2 = MetaType.IsDate.Find((string x) => format.Value.Contains(x));
						if (text2 != null)
						{
							val = DateTime.Parse(val.DateTime.ToShortDateString());
						}
					}
					ws.Cells[format.CellAddress].Value = (object)val;
				}
				pck.Save();
			}
			listFileName.Add(destFile);
		}
		return CalFiles(listFileName, folderPath, pattern.Replace(product.Code + "_" + template.Name, ""));
	}

	public async Task<CustomerFile> ExportAllOnFile(IEnumerable<ExportDto> models)
	{
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
		string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFileExport");
		Directory.CreateDirectory(sourcePath);
		Directory.CreateDirectory(folderPath);
		MetadataValue cfg = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name == "DirFile", "");
		List<string> listFileName = new List<string>();
		List<RequestResultViewModel> resultAlls = new List<RequestResultViewModel>();
		foreach (ExportDto model in models)
		{
			resultAlls.AddRange(await _uow.GetRepository<RequestResult>().FindByAsync<RequestResultViewModel>((RequestResult x) => x.RequestId.Equals(model.Id) && !string.IsNullOrEmpty(x.Result), "Sample"));
		}
		List<string> list = listFileName;
		list.AddRange(await ExportOneFile(resultAlls, sourcePath, folderPath, models.First().TemplateId, cfg?.Description));
		return CalFiles(listFileName, folderPath, models.First().Name);
	}

	public async Task<CustomerFile> ExportForProduct(IEnumerable<ExportDto> models)
	{
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
		string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFileExport");
		Directory.CreateDirectory(sourcePath);
		Directory.CreateDirectory(folderPath);
		MetadataValue cfg = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name == "DirFile", "");
		List<string> listFileName = new List<string>();
		List<RequestResultViewModel> resultAlls = new List<RequestResultViewModel>();
		foreach (ExportDto model in models)
		{
			resultAlls.AddRange(await _uow.GetRepository<RequestResult>().FindByAsync<RequestResultViewModel>((RequestResult x) => x.RequestId.Equals(model.Id) && !string.IsNullOrEmpty(x.Result), "Sample"));
		}
		IEnumerable<IGrouping<string, RequestResultViewModel>> enumerable = from x in resultAlls
			group x by $"{x.ProductSort}#{x.RequestId}";
		foreach (IGrouping<string, RequestResultViewModel> item in enumerable)
		{
			List<string> list = listFileName;
			list.AddRange(await ExportOneFile(item.ToList(), sourcePath, folderPath, models.First().TemplateId, cfg?.Description));
		}
		return CalFiles(listFileName, models.First().Name);
	}

	public async Task<List<string>> ExportOneFile(RequestViewModel request, string type, string page, string sourcePath, string folderPath, bool isSample = false, Guid? templateId = null, string password = null)
	{
		_ = Guid.Empty;
		Guid? idplan = null;
		List<PlanDetail> plandetails = new List<PlanDetail>();
		Guid templateid;
		if (type == "Product")
		{
			templateid = templateId.Value;
		}
		else
		{
			idplan = Guid.Parse(type);
			Plan plan = await _uow.GetRepository<Plan>().FindByIdAsync(idplan.Value);
			templateid = (plan.TemplateId.HasValue ? plan.TemplateId.Value : Guid.Empty);
			plandetails = (await _uow.GetRepository<PlanDetail>().FindByAsync((PlanDetail x) => x.PlanId.Equals(plan.Id), "")).ToList();
		}
		Template template = (await _uow.GetRepository<Template>().FindByIdAsync(templateid)) ?? throw new Exception("This product hasn't template");
		if (string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("Template for this product hasn't file");
		}
		RequestStatusViewModel requestStatus = (await _uow.GetRepository<RequestStatus>().FindByAsync<RequestStatusViewModel>((RequestStatus x) => x.RequestId.Equals(request.Id) && x.PlanId == idplan)).FirstOrDefault();
		IEnumerable<RequestResultViewModel> source = await _uow.GetRepository<RequestResult>().FindByAsync<RequestResultViewModel>((RequestResult x) => x.RequestId.Equals(request.Id) && !string.IsNullOrEmpty(x.Result), "Sample");
		string sourceFile = Path.Combine(sourcePath, template.TemplateUrl);
		int limit = template.Limit;
		if (isSample)
		{
			limit = 1;
		}
		if (idplan.HasValue)
		{
			source = source.Where((RequestResultViewModel x) => plandetails.Any((PlanDetail c) => c.MeasurementId.Equals(x.MeasurementId)));
		}
		IEnumerable<IGrouping<int?, RequestResultViewModel>> groupResultAlls = from x in source
			group x by x.Sample;
		int mRow = (groupResultAlls.Count().Equals(0) ? 1 : groupResultAlls.Count());
		int pages = ((mRow % limit == 0) ? (mRow / limit) : (mRow / limit + 1));
		List<PropertyInfo> typeRequests = new List<PropertyInfo>(typeof(RequestViewModel).GetProperties());
		List<PropertyInfo> typeStatuss = new List<PropertyInfo>(typeof(RequestStatusViewModel).GetProperties());
		List<ExportMappingDto> fieldMappings = JsonConvert.DeserializeObject<List<ExportMappingDto>>(template.TemplateData);
		List<string> tempLst = new List<string>();
		double num2 = default(double);
		for (int i = 0; i < pages; i++)
		{
			string value = $"{i + 1}";
			if (!string.IsNullOrEmpty(page) && !page.Equals(value))
			{
				continue;
			}
			int skip = i * limit;
			Dictionary<string, object> props = new Dictionary<string, object>();
			List<string> formats = new List<string>();
			_ = string.Empty;
			string judgement = "OK";
			foreach (PropertyInfo item in typeRequests)
			{
				object value2 = item.GetValue(request, null);
				if (value2 != null)
				{
					props.Add("[[" + item.Name.ToUpper() + "]]", value2);
				}
			}
			foreach (PropertyInfo item2 in typeStatuss)
			{
				if (requestStatus != null)
				{
					object value3 = item2.GetValue(requestStatus, null);
					if (value3 != null)
					{
						props.Add("[[" + item2.Name.ToUpper() + "#STATUS]]", value3);
					}
					continue;
				}
				break;
			}
			foreach (var item3 in (await _uow.GetRepository<Comment>().FindByAsync<CommentViewModel>((Comment x) => x.RequestId.Equals(request.Id), "Created")).Select((CommentViewModel value8, int index) => new
			{
				index = index,
				value = value8
			}))
			{
				if (!string.IsNullOrEmpty(item3.value.Content))
				{
					props.Add($"[[COMMENT#{item3.index + 1}]]", item3.value.Content);
				}
			}
			string key;
			for (int num = 0; num < limit; num++)
			{
				if (skip + num + 1 > mRow)
				{
					continue;
				}
				string text = "OK";
				if (groupResultAlls.Count().Equals(0))
				{
					break;
				}
				IGrouping<int?, RequestResultViewModel> grouping = groupResultAlls.ToList()[skip + num];
				bool flag = true;
				foreach (RequestResultViewModel item4 in grouping)
				{
					if (string.IsNullOrEmpty(item4.Result))
					{
						continue;
					}
					key = "[[OPERATOR]]";
					if (props.ContainsKey(key))
					{
						dynamic val = props[key];
						if ((!val.Contains(item4.StaffName)))
						{
							props[key] += ", " + item4.StaffName;
						}
					}
					else
					{
						props.Add(key, item4.StaffName);
					}
					if (flag)
					{
						flag = false;
						key = $"[[OPERATOR#{num + 1}]]";
						props.Add(key, item4.StaffName);
						key = $"[[TIME#{num + 1}]]";
						props.Add(key, item4.Modified);
					}
					key = $"[[{item4.MeasurementCode.ToUpper()}#{item4.Cavity}#{num + 1}]]";
					if (item4.MeasurementUnit == "°" && double.TryParse(item4.Result, out var result))
					{
						props.Add(key, ConvertDoubleToDegree(result));
					}
					else
					{
						props.Add(key, item4.Result);
					}
					string text2 = $"[[{item4.MeasurementCode.ToUpper()}#{item4.Cavity}#JUD{num + 1}]]";
					props.Add(text2, item4.Judge);
					string text3 = $"[[{item4.MeasurementCode.ToUpper()}#{item4.Cavity}#JUD]]";
					if (!props.ContainsKey(text3))
					{
						props.Add(text3, item4.Judge);
					}
					if (item4.Judge.Contains("NG"))
					{
						formats.Add(key);
						formats.Add(text2);
						text = "NG";
						judgement = "NG";
						if (!formats.Contains(text3))
						{
							formats.Add(text3);
						}
						props[text3] = "NG";
					}
					if (item4.Judge.Contains("OK"))
					{
						string key2 = "[[" + item4.MeasurementCode.ToUpper() + "#OK]]";
						if (props.ContainsKey(key2))
						{
							props.TryGetValue(key2, out dynamic value4);
							props[key2] = (object)(value4 + 1);
						}
						else
						{
							props.Add(key2, 1);
						}
					}
					else
					{
						string text4 = "[[" + item4.MeasurementCode.ToUpper() + "#NG]]";
						if (props.ContainsKey(text4))
						{
							props.TryGetValue(text4, out dynamic value5);
							props[text4] = (object)(value5 + 1);
						}
						else
						{
							props.Add(text4, 1);
							formats.Add(text4);
						}
					}
					string key3 = "[[" + item4.MeasurementCode.ToUpper() + "#TOOL]]";
					if (!props.ContainsKey(key3))
					{
						props.Add(key3, item4.MachineName);
					}
					string key4 = "[[" + item4.MeasurementCode.ToUpper() + "#JUD#OK]]";
					string text5 = "[[" + item4.MeasurementCode.ToUpper() + "#JUD#NG]]";
					if (!props.ContainsKey(key4))
					{
						props.Add(key4, "OK");
					}
					if (!props.ContainsKey(text5))
					{
						props.Add(text5, null);
					}
					if (item4.Judge.Contains("NG"))
					{
						props[key4] = null;
						props[text5] = "NG";
						if (!formats.Contains(text5))
						{
							formats.Add(text5);
						}
					}
				}
				props.Add($"[[SP#{num + 1}]]", skip + num + 1);
				key = $"[[JUD#{num + 1}]]";
				if (grouping.Count() > 0)
				{
					props.Add(key, text);
				}
				if (text.Equals("NG"))
				{
					formats.Add(key);
				}
			}
			key = "[[JUDORG]]";
			props.Add(key, judgement);
			if (judgement.Equals("NG"))
			{
				formats.Add(key);
			}
			string arg = string.Format("{0}_Page {1}_", request.Name.Replace("\\", "").Replace("/", ""), i + 1);
			string text6 = Path.Combine(folderPath, $"{arg}{Guid.NewGuid()}{Path.GetExtension(template.TemplateUrl)}");
			File.Copy(sourceFile, text6, overwrite: true);
			FileInfo newFile = new FileInfo(text6);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				IEnumerable<IGrouping<string, ExportMappingDto>> enumerable = from x in fieldMappings
					group x by x.Sheet;
				foreach (IGrouping<string, ExportMappingDto> item5 in enumerable)
				{
					ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[item5.Key];
					if (excelWorksheet == null)
					{
						excelWorksheet = excelPackage.Workbook.Worksheets[0];
					}
					foreach (ExportMappingDto fieldMapping in item5)
					{
						props.TryGetValue(fieldMapping.Value, out dynamic value6);
						if (fieldMapping.Value.StartsWith("[[DATE#"))
						{
							props.TryGetValue("[[DATE]]", out value6);
						}
						string text7 = MetaType.IsDate.Find((string x) => fieldMapping.Value.Contains(x));
						if (text7 == null)
						{
							if (fieldMapping.Value.StartsWith("[[TYPE#"))
							{
								string value7 = Regex.Match(fieldMapping.Value, "(?<=#).*(?=\\].)").Value;
								if (value7 == request.Type.ToUpper())
								{
									value6 = "√";
								}
							}
							else if (fieldMapping.Value == "[[JUDORG#OK]]" && judgement == "OK")
							{
								value6 = "√";
							}
							else if (fieldMapping.Value == "[[JUDORG#NG]]" && judgement == "NG")
							{
								value6 = "√";
							}
							if (double.TryParse(value6?.ToString(), out num2))
							{
								excelWorksheet.Cells[fieldMapping.CellAddress].Value = num2;
							}
							else
							{
								excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)(value6 ?? null);
							}
						}
						else if (value6 != null && text7.Contains("[[DATE#"))
						{
							switch (fieldMapping.Value)
							{
							case "[[DATE#YEAR]]":
								value6 = value6.DateTime.ToString("yyyy");
								break;
							case "[[DATE#MONTH]]":
								value6 = value6.DateTime.ToString("MM");
								break;
							case "[[DATE#DAY]]":
								value6 = value6.DateTime.ToString("dd");
								break;
							}
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)(value6 ?? null);
						}
						else if (text7.Contains("[[TIME#"))
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)((value6 == null) ? null : value6.DateTime);
						}
						else
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)((value6 == null) ? null : DateTime.Parse(value6.DateTime.ToShortDateString()));
						}
						string text8 = formats.Find((string x) => x.Equals(fieldMapping.Value));
						if (text8 != null)
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Style.Font.Color.SetColor(Color.Red);
						}
					}
					if (!string.IsNullOrEmpty(password))
					{
						excelWorksheet.Protection.SetPassword(password);
						excelWorksheet.Protection.IsProtected = true;
						excelWorksheet.Protection.AllowSelectUnlockedCells = false;
						excelWorksheet.Protection.AllowSelectLockedCells = false;
					}
				}
				excelPackage.Save();
			}
			tempLst.Add(text6);
		}
		return tempLst;
	}

	private async Task<List<string>> ExportOneFile(List<RequestResultViewModel> resultAlls, string sourcePath, string folderPath, Guid? templateId = null, string password = null)
	{
		RequestViewModel request = (await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(resultAlls.First().RequestId.Value)) ?? throw new Exception($"Can't find request with id: {resultAlls.First().RequestId}");
		Guid? guid = templateId;
		if (!guid.HasValue || guid == Guid.Empty)
		{
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId.Value);
			guid = (product.TemplateId.HasValue ? product.TemplateId.Value : Guid.Empty);
		}
		Template template = (await _uow.GetRepository<Template>().FindByIdAsync(guid.Value)) ?? throw new Exception("This product hasn't template");
		if (string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("Template for this product hasn't file");
		}
		string sourceFile = Path.Combine(sourcePath, template.TemplateUrl);
		int limit = template.Limit;
		IEnumerable<IGrouping<string, RequestResultViewModel>> groupResultAlls = from x in resultAlls
			group x by $"{x.RequestId}#{x.Sample}";
		int mRow = (groupResultAlls.Count().Equals(0) ? 1 : groupResultAlls.Count());
		int pages = ((mRow % limit == 0) ? (mRow / limit) : (mRow / limit + 1));
		List<PropertyInfo> typeRequests = new List<PropertyInfo>(typeof(RequestViewModel).GetProperties());
		List<ExportMappingDto> fieldMappings = JsonConvert.DeserializeObject<List<ExportMappingDto>>(template.TemplateData);
		List<string> tempLst = new List<string>();
		double num2 = default(double);
		for (int i = 0; i < pages; i++)
		{
			int skip = i * limit;
			Dictionary<string, object> props = new Dictionary<string, object>();
			List<string> formats = new List<string>();
			_ = string.Empty;
			string judgement = "OK";
			foreach (PropertyInfo item in typeRequests)
			{
				object value = item.GetValue(request, null);
				if (value != null)
				{
					props.Add("[[" + item.Name.ToUpper() + "]]", value);
				}
			}
			foreach (var item2 in (await _uow.GetRepository<Comment>().FindByAsync<CommentViewModel>((Comment x) => x.RequestId.Equals(request.Id), "Created")).Select((CommentViewModel value6, int index) => new
			{
				index = index,
				value = value6
			}))
			{
				if (!string.IsNullOrEmpty(item2.value.Content))
				{
					props.Add($"[[COMMENT#{item2.index + 1}]]", item2.value.Content);
				}
			}
			string key;
			for (int num = 0; num < limit; num++)
			{
				if (skip + num + 1 > mRow)
				{
					continue;
				}
				string text = "OK";
				if (groupResultAlls.Count().Equals(0))
				{
					break;
				}
				IGrouping<string, RequestResultViewModel> grouping = groupResultAlls.ToList()[skip + num];
				bool flag = true;
				foreach (RequestResultViewModel item3 in grouping)
				{
					if (string.IsNullOrEmpty(item3.Result))
					{
						continue;
					}
					key = "[[OPERATOR]]";
					if (props.ContainsKey(key))
					{
						dynamic val = props[key];
						if ((!val.Contains(item3.StaffName)))
						{
							props[key] += ", " + item3.StaffName;
						}
					}
					else
					{
						props.Add(key, item3.StaffName);
					}
					if (flag)
					{
						flag = false;
						key = $"[[OPERATOR#{num + 1}]]";
						props.Add(key, item3.StaffName);
						key = $"[[TIME#{num + 1}]]";
						props.Add(key, item3.Modified);
					}
					key = $"[[{item3.MeasurementCode.ToUpper()}#{item3.Cavity}#{num + 1}]]";
					if (item3.MeasurementUnit == "°" && double.TryParse(item3.Result, out var result))
					{
						props.Add(key, ConvertDoubleToDegree(result));
					}
					else
					{
						props.Add(key, item3.Result);
					}
					string text2 = $"[[{item3.MeasurementCode.ToUpper()}#{item3.Cavity}#JUD{num + 1}]]";
					props.Add(text2, item3.Judge);
					string text3 = $"[[{item3.MeasurementCode.ToUpper()}#{item3.Cavity}#JUD]]";
					if (!props.ContainsKey(text3))
					{
						props.Add(text3, item3.Judge);
					}
					if (item3.Judge.Contains("NG"))
					{
						formats.Add(key);
						formats.Add(text2);
						text = "NG";
						judgement = "NG";
						if (!formats.Contains(text3))
						{
							formats.Add(text3);
						}
						props[text3] = "NG";
					}
					if (item3.Judge.Contains("OK"))
					{
						string key2 = "[[" + item3.MeasurementCode.ToUpper() + "#OK]]";
						if (props.ContainsKey(key2))
						{
							props.TryGetValue(key2, out dynamic value2);
							props[key2] = (object)(value2 + 1);
						}
						else
						{
							props.Add(key2, 1);
						}
					}
					else
					{
						string text4 = "[[" + item3.MeasurementCode.ToUpper() + "#NG]]";
						if (props.ContainsKey(text4))
						{
							props.TryGetValue(text4, out dynamic value3);
							props[text4] = (object)(value3 + 1);
						}
						else
						{
							props.Add(text4, 1);
							formats.Add(text4);
						}
					}
					string key3 = "[[" + item3.MeasurementCode.ToUpper() + "#TOOL]]";
					if (!props.ContainsKey(key3))
					{
						props.Add(key3, item3.MachineName);
					}
					string key4 = "[[" + item3.MeasurementCode.ToUpper() + "#JUD#OK]]";
					string text5 = "[[" + item3.MeasurementCode.ToUpper() + "#JUD#NG]]";
					if (!props.ContainsKey(key4))
					{
						props.Add(key4, "OK");
					}
					if (!props.ContainsKey(text5))
					{
						props.Add(text5, null);
					}
					if (item3.Judge.Contains("NG"))
					{
						props[key4] = null;
						props[text5] = "NG";
						if (!formats.Contains(text5))
						{
							formats.Add(text5);
						}
					}
				}
				props.Add($"[[SP#{num + 1}]]", skip + num + 1);
				key = $"[[JUD#{num + 1}]]";
				if (grouping.Count() > 0)
				{
					props.Add(key, text);
				}
				if (text.Equals("NG"))
				{
					formats.Add(key);
				}
			}
			key = "[[JUDORG]]";
			props.Add(key, judgement);
			if (judgement.Equals("NG"))
			{
				formats.Add(key);
			}
			string text6 = string.Format("{0}_Page {1}", request.Name.Replace("\\", "").Replace("/", ""), i + 1);
			string text7 = Path.Combine(folderPath, text6 + Path.GetExtension(template.TemplateUrl));
			File.Copy(sourceFile, text7, overwrite: true);
			FileInfo newFile = new FileInfo(text7);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				IEnumerable<IGrouping<string, ExportMappingDto>> enumerable = from x in fieldMappings
					group x by x.Sheet;
				foreach (IGrouping<string, ExportMappingDto> item4 in enumerable)
				{
					ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[item4.Key];
					if (excelWorksheet == null)
					{
						excelWorksheet = excelPackage.Workbook.Worksheets[0];
					}
					foreach (ExportMappingDto fieldMapping in item4)
					{
						props.TryGetValue(fieldMapping.Value, out dynamic value4);
						if (fieldMapping.Value.StartsWith("[[DATE#"))
						{
							props.TryGetValue("[[DATE]]", out value4);
						}
						string text8 = MetaType.IsDate.Find((string x) => fieldMapping.Value.Contains(x));
						if (text8 == null)
						{
							if (fieldMapping.Value.StartsWith("[[TYPE#"))
							{
								string value5 = Regex.Match(fieldMapping.Value, "(?<=#).*(?=\\].)").Value;
								if (value5 == request.Type.ToUpper())
								{
									value4 = "√";
								}
							}
							else if (fieldMapping.Value == "[[JUDORG#OK]]" && judgement == "OK")
							{
								value4 = "√";
							}
							else if (fieldMapping.Value == "[[JUDORG#NG]]" && judgement == "NG")
							{
								value4 = "√";
							}
							if (double.TryParse(value4?.ToString(), out num2))
							{
								excelWorksheet.Cells[fieldMapping.CellAddress].Value = num2;
							}
							else
							{
								excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)(value4 ?? null);
							}
						}
						else if (value4 != null && text8.Contains("[[DATE#"))
						{
							switch (fieldMapping.Value)
							{
							case "[[DATE#YEAR]]":
								value4 = value4.DateTime.ToString("yyyy");
								break;
							case "[[DATE#MONTH]]":
								value4 = value4.DateTime.ToString("MM");
								break;
							case "[[DATE#DAY]]":
								value4 = value4.DateTime.ToString("dd");
								break;
							}
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)(value4 ?? null);
						}
						else if (text8.Contains("[[TIME#"))
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)((value4 == null) ? null : value4.DateTime);
						}
						else
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)((value4 == null) ? null : DateTime.Parse(value4.DateTime.ToShortDateString()));
						}
						string text9 = formats.Find((string x) => x.Equals(fieldMapping.Value));
						if (text9 != null)
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Style.Font.Color.SetColor(Color.Red);
						}
					}
					if (!string.IsNullOrEmpty(password))
					{
						excelWorksheet.Protection.SetPassword(password);
						excelWorksheet.Protection.IsProtected = true;
						excelWorksheet.Protection.AllowSelectUnlockedCells = false;
						excelWorksheet.Protection.AllowSelectLockedCells = false;
					}
				}
				excelPackage.Save();
			}
			tempLst.Add(text7);
		}
		return tempLst;
	}

	private async Task<List<string>> ExportOneFile(List<RequestResultViewModel> resultAlls, string sourcePath, string folderPath, int? lastSort, Guid? templateId = null)
	{
		RequestViewModel request = (await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(resultAlls.First().RequestId.Value)) ?? throw new Exception($"Can't find request with id: {resultAlls.First().RequestId}");
		Guid? guid = templateId;
		if (!guid.HasValue || guid == Guid.Empty)
		{
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId.Value);
			guid = (product.TemplateId.HasValue ? product.TemplateId.Value : Guid.Empty);
		}
		Template template = (await _uow.GetRepository<Template>().FindByIdAsync(guid.Value)) ?? throw new Exception("This product hasn't template");
		if (string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("Template for this product hasn't file");
		}
		string sourceFile = Path.Combine(sourcePath, template.TemplateUrl);
		int limit = template.Limit;
		IEnumerable<IGrouping<string, RequestResultViewModel>> groupResultAlls = from x in resultAlls
			group x by $"{x.RequestId}#{x.Sample}";
		int? pages = ((lastSort % limit != 0) ? (lastSort / limit + 1) : (lastSort / limit));
		List<PropertyInfo> typeRequests = new List<PropertyInfo>(typeof(RequestViewModel).GetProperties());
		List<ExportMappingDto> fieldMappings = JsonConvert.DeserializeObject<List<ExportMappingDto>>(template.TemplateData);
		List<string> tempLst = new List<string>();
		int i;
		double num3 = default(double);
		for (i = 0; i < pages; i++)
		{
			int skip = i * limit;
			Dictionary<string, object> props = new Dictionary<string, object>();
			List<string> formats = new List<string>();
			_ = string.Empty;
			string judgement = "OK";
			foreach (PropertyInfo item in typeRequests)
			{
				object value = item.GetValue(request, null);
				if (value != null)
				{
					props.Add("[[" + item.Name.ToUpper() + "]]", value);
				}
			}
			foreach (var item2 in (await _uow.GetRepository<Comment>().FindByAsync<CommentViewModel>((Comment x) => x.RequestId.Equals(request.Id), "Created")).Select((CommentViewModel value5, int index) => new
			{
				index = index,
				value = value5
			}))
			{
				if (!string.IsNullOrEmpty(item2.value.Content))
				{
					props.Add($"[[COMMENT#{item2.index + 1}]]", item2.value.Content);
				}
			}
			IEnumerable<RequestResultViewModel> source = resultAlls.Where((RequestResultViewModel x) => x.Sample > i * limit && x.Sample <= (i + 1) * limit);
			IEnumerable<IGrouping<string, RequestResultViewModel>> enumerable = from x in source
				group x by $"{x.MeasurementCode}#{x.Cavity}";
			string text;
			foreach (IGrouping<string, RequestResultViewModel> item3 in enumerable)
			{
				string value2 = "OK";
				text = "[[" + item3.Key.ToUpper() + "#JUD]]";
				int num = item3.Count((RequestResultViewModel x) => x.Judge.Contains("NG"));
				if (num > 0)
				{
					value2 = "NG";
					formats.Add(text);
				}
				props.Add(text, value2);
			}
			for (int num2 = 0; num2 < limit; num2++)
			{
				if (!(skip + num2 + 1 <= lastSort))
				{
					continue;
				}
				if (groupResultAlls.Count().Equals(0))
				{
					break;
				}
				IGrouping<string, RequestResultViewModel> grouping = null;
				foreach (IGrouping<string, RequestResultViewModel> item4 in groupResultAlls)
				{
					if (item4.Last().Sort == skip + num2 + 1)
					{
						grouping = item4;
						break;
					}
				}
				if (grouping == null || grouping.Count() == 0)
				{
					continue;
				}
				string text2 = "OK";
				bool flag = true;
				foreach (RequestResultViewModel item5 in grouping)
				{
					if (string.IsNullOrEmpty(item5.Result))
					{
						continue;
					}
					text = "[[OPERATOR]]";
					if (props.ContainsKey(text))
					{
						dynamic val = props[text];
						if ((!val.Contains(item5.StaffName)))
						{
							props[text] += ", " + item5.StaffName;
						}
					}
					else
					{
						props.Add(text, item5.StaffName);
					}
					if (flag)
					{
						flag = false;
						text = $"[[OPERATOR#{num2 + 1}]]";
						props.Add(text, item5.StaffName);
						text = $"[[TIME#{num2 + 1}]]";
						props.Add(text, item5.Modified);
					}
					text = $"[[{item5.MeasurementCode.ToUpper()}#{item5.Cavity}#{num2 + 1}]]";
					if (item5.MeasurementUnit == "°" && double.TryParse(item5.Result, out var result))
					{
						props.Add(text, ConvertDoubleToDegree(result));
					}
					else
					{
						props.Add(text, item5.Result);
					}
					string text3 = $"[[{item5.MeasurementCode.ToUpper()}#{item5.Cavity}#JUD{num2 + 1}]]";
					props.Add(text3, item5.Judge);
					if (item5.Judge.Contains("NG"))
					{
						formats.Add(text);
						formats.Add(text3);
						text2 = "NG";
						judgement = "NG";
					}
				}
				props.Add($"[[SP#{num2 + 1}]]", skip + num2 + 1);
				text = $"[[JUD#{num2 + 1}]]";
				if (grouping.Count() > 0)
				{
					props.Add(text, text2);
				}
				if (text2.Equals("NG"))
				{
					formats.Add(text);
				}
			}
			text = "[[JUDORG]]";
			props.Add(text, judgement);
			if (judgement.Equals("NG"))
			{
				formats.Add(text);
			}
			string text4 = string.Format("{0}_Page {1}", request.Name.Replace("\\", "").Replace("/", ""), i + 1);
			string text5 = Path.Combine(folderPath, text4 + Path.GetExtension(template.TemplateUrl));
			File.Copy(sourceFile, text5, overwrite: true);
			FileInfo newFile = new FileInfo(text5);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Data"];
				foreach (ExportMappingDto fieldMapping in fieldMappings)
				{
					props.TryGetValue(fieldMapping.Value, out dynamic value3);
					string text6 = MetaType.IsDate.Find((string x) => fieldMapping.Value.Contains(x));
					if (text6 == null)
					{
						if (fieldMapping.Value.StartsWith("[[TYPE#"))
						{
							string value4 = Regex.Match(fieldMapping.Value, "(?<=#).*(?=\\].)").Value;
							if (value4 == request.Type.ToUpper())
							{
								value3 = "√";
							}
						}
						else if (fieldMapping.Value == "[[JUDORG#OK]]" && judgement == "OK")
						{
							value3 = "√";
						}
						else if (fieldMapping.Value == "[[JUDORG#NG]]" && judgement == "NG")
						{
							value3 = "√";
						}
						if (double.TryParse(value3?.ToString(), out num3))
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = num3;
						}
						else
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)(value3 ?? null);
						}
					}
					else
					{
						excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)((value3 == null) ? null : DateTime.Parse(value3.DateTime.ToShortDateString()));
						if (value3 != null && text6.Contains("[[TIME#"))
						{
							int num4 = value3.DateTime.Hour;
							excelWorksheet.Cells[fieldMapping.CellAddress].Style.Fill.PatternType = ExcelFillStyle.Solid;
							if (num4 > 7 && num4 < 20)
							{
								excelWorksheet.Cells[fieldMapping.CellAddress].Style.Fill.BackgroundColor.SetColor(Color.LimeGreen);
							}
							else
							{
								excelWorksheet.Cells[fieldMapping.CellAddress].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
							}
						}
					}
					string text7 = formats.Find((string x) => x.Equals(fieldMapping.Value));
					if (text7 != null)
					{
						excelWorksheet.Cells[fieldMapping.CellAddress].Style.Font.Color.SetColor(Color.Red);
					}
				}
				excelPackage.Save();
			}
			tempLst.Add(text5);
		}
		return tempLst;
	}

	private async Task<string> ExportOneByOneFile(List<RequestResultViewModel> results, string sourcePath, string folderPath, int? lastSort, Guid? templateId = null)
	{
		RequestViewModel request = (await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(results.First().RequestId.Value)) ?? throw new Exception($"Can't find request with id: {results.First().RequestId}");
		Guid? guid = templateId;
		if (!guid.HasValue || guid == Guid.Empty)
		{
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId.Value);
			guid = (product.TemplateId.HasValue ? product.TemplateId.Value : Guid.Empty);
		}
		Template template = (await _uow.GetRepository<Template>().FindByIdAsync(guid.Value)) ?? throw new Exception("This product hasn't template");
		if (string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("Template for this product hasn't file");
		}
		string sourceFile = Path.Combine(sourcePath, template.TemplateUrl);
		List<PropertyInfo> list = new List<PropertyInfo>(typeof(RequestViewModel).GetProperties());
		List<ExportMappingDto> fieldMappings = JsonConvert.DeserializeObject<List<ExportMappingDto>>(template.TemplateData);
		Dictionary<string, object> props = new Dictionary<string, object>();
		List<string> formats = new List<string>();
		_ = string.Empty;
		string judgement = "OK";
		foreach (PropertyInfo item in list)
		{
			object value = item.GetValue(request, null);
			if (value != null)
			{
				props.Add("[[" + item.Name.ToUpper() + "]]", value);
			}
		}
		foreach (var item2 in (await _uow.GetRepository<Comment>().FindByAsync<CommentViewModel>((Comment x) => x.RequestId.Equals(request.Id), "Created")).Select((CommentViewModel value5, int index) => new
		{
			index = index,
			value = value5
		}))
		{
			if (!string.IsNullOrEmpty(item2.value.Content))
			{
				props.Add($"[[COMMENT#{item2.index + 1}]]", item2.value.Content);
			}
		}
		IEnumerable<IGrouping<string, RequestResultViewModel>> enumerable = from x in results
			group x by $"{x.MeasurementCode}#{x.Cavity}";
		string text;
		foreach (IGrouping<string, RequestResultViewModel> item3 in enumerable)
		{
			string value2 = "OK";
			text = "[[" + item3.Key.ToUpper() + "#JUD]]";
			int num = item3.Count((RequestResultViewModel x) => x.Judge.Contains("NG"));
			if (num > 0)
			{
				value2 = "NG";
				formats.Add(text);
			}
			props.Add(text, value2);
		}
		if (results == null || results.Count() == 0)
		{
			return null;
		}
		string text2 = "OK";
		bool flag = true;
		foreach (RequestResultViewModel result3 in results)
		{
			if (string.IsNullOrEmpty(result3.Result))
			{
				continue;
			}
			text = "[[OPERATOR]]";
			if (props.ContainsKey(text))
			{
				dynamic val = props[text];
				if ((!val.Contains(result3.StaffName)))
				{
					props[text] += ", " + result3.StaffName;
				}
			}
			else
			{
				props.Add(text, result3.StaffName);
			}
			if (flag)
			{
				flag = false;
				text = "[[OPERATOR#1]]";
				props.Add(text, result3.StaffName);
				text = "[[TIME#1]]";
				props.Add(text, result3.Modified);
			}
			text = $"[[{result3.MeasurementCode.ToUpper()}#{result3.Cavity}#1]]";
			if (result3.MeasurementUnit == "°" && double.TryParse(result3.Result, out var result))
			{
				props.Add(text, ConvertDoubleToDegree(result));
			}
			else
			{
				props.Add(text, result3.Result);
			}
			string text3 = $"[[{result3.MeasurementCode.ToUpper()}#{result3.Cavity}#JUD1]]";
			props.Add(text3, result3.Judge);
			if (result3.Judge.Contains("NG"))
			{
				formats.Add(text);
				formats.Add(text3);
				text2 = "NG";
				judgement = "NG";
			}
		}
		props.Add("[[SP#1]]", lastSort);
		text = "[[JUD#1]]";
		if (results.Count() > 0)
		{
			props.Add(text, text2);
		}
		if (text2.Equals("NG"))
		{
			formats.Add(text);
		}
		text = "[[JUDORG]]";
		props.Add(text, judgement);
		if (judgement.Equals("NG"))
		{
			formats.Add(text);
		}
		string text4 = string.Format("{0}_Sample {1}", request.Name.Replace("\\", "").Replace("/", ""), lastSort);
		string text5 = Path.Combine(folderPath, text4 + Path.GetExtension(template.TemplateUrl));
		File.Copy(sourceFile, text5, overwrite: true);
		FileInfo newFile = new FileInfo(text5);
		using (ExcelPackage excelPackage = new ExcelPackage(newFile))
		{
			ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Data"];
			foreach (ExportMappingDto fieldMapping in fieldMappings)
			{
				props.TryGetValue(fieldMapping.Value, out dynamic value3);
				string text6 = MetaType.IsDate.Find((string x) => fieldMapping.Value.Contains(x));
				if (text6 == null)
				{
					if (fieldMapping.Value.StartsWith("[[TYPE#"))
					{
						string value4 = Regex.Match(fieldMapping.Value, "(?<=#).*(?=\\].)").Value;
						if (value4 == request.Type.ToUpper())
						{
							text2 = "√";
						}
					}
					else if (fieldMapping.Value == "[[JUDORG#OK]]" && judgement == "OK")
					{
						text2 = "√";
					}
					else if (fieldMapping.Value == "[[JUDORG#NG]]" && judgement == "NG")
					{
						text2 = "√";
					}
					if (double.TryParse(text2?.ToString(), out var result2))
					{
						excelWorksheet.Cells[fieldMapping.CellAddress].Value = result2;
					}
					else
					{
						excelWorksheet.Cells[fieldMapping.CellAddress].Value = text2 ?? null;
					}
				}
				else
				{
					excelWorksheet.Cells[fieldMapping.CellAddress].Value = (object)((value3 == null) ? null : DateTime.Parse(value3.DateTime.ToShortDateString()));
					if (value3 != null && text6.Contains("[[TIME#"))
					{
						int num2 = value3.DateTime.Hour;
						excelWorksheet.Cells[fieldMapping.CellAddress].Style.Fill.PatternType = ExcelFillStyle.Solid;
						if (num2 > 7 && num2 < 20)
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Style.Fill.BackgroundColor.SetColor(Color.LimeGreen);
						}
						else
						{
							excelWorksheet.Cells[fieldMapping.CellAddress].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
						}
					}
				}
				string text7 = formats.Find((string x) => x.Equals(fieldMapping.Value));
				if (text7 != null)
				{
					excelWorksheet.Cells[fieldMapping.CellAddress].Style.Font.Color.SetColor(Color.Red);
				}
			}
			excelPackage.Save();
		}
		return text5;
	}

	private CustomerFile CalFiles(List<string> listFileName, string folderPath, string fileName)
	{
		if (listFileName.Count.Equals(0))
		{
			throw new Exception("Files is emtry");
		}
		string text;
		if (listFileName.Count.Equals(1))
		{
			text = listFileName[0];
		}
		else
		{
			text = Path.Combine(folderPath, $"{fileName}_{Guid.NewGuid()}.zip");
			using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(text)))
			{
				zipOutputStream.SetLevel(9);
				byte[] array = new byte[16384];
				for (int i = 0; i < listFileName.Count; i++)
				{
					ZipEntry zipEntry = new ZipEntry(Path.GetFileName(listFileName[i]));
					zipEntry.DateTime = DateTime.Now;
					zipEntry.IsUnicodeText = true;
					zipOutputStream.PutNextEntry(zipEntry);
					using FileStream fileStream = File.OpenRead(listFileName[i]);
					int num;
					do
					{
						num = fileStream.Read(array, 0, array.Length);
						zipOutputStream.Write(array, 0, num);
					}
					while (num > 0);
				}
				zipOutputStream.Finish();
				zipOutputStream.Flush();
				zipOutputStream.Close();
			}
			listFileName.Add(text);
		}
		byte[] fileContents = File.ReadAllBytes(text);
		for (int j = 0; j < listFileName.Count; j++)
		{
			if (File.Exists(listFileName[j]))
			{
				File.Delete(listFileName[j]);
			}
		}
		return new CustomerFile(fileContents, Path.GetFileName(text));
	}

	private CustomerFile CalFiles(List<string> listFileName, string fileName)
	{
		if (listFileName.Count.Equals(0))
		{
			throw new Exception("Files is emtry");
		}
		string text;
		if (listFileName.Count.Equals(1))
		{
			text = listFileName[0];
		}
		else
		{
			text = listFileName[0];
			FileInfo newFile = new FileInfo(text);
			using ExcelPackage excelPackage = new ExcelPackage(newFile);
			for (int i = 1; i < listFileName.Count; i++)
			{
				FileInfo newFile2 = new FileInfo(listFileName[i]);
				using ExcelPackage excelPackage2 = new ExcelPackage(newFile2);
				foreach (ExcelWorksheet worksheet in excelPackage2.Workbook.Worksheets)
				{
					try
					{
						string name = worksheet.Name;
						int num = 1;
						while (excelPackage.Workbook.Worksheets[name] != null)
						{
							name = $"{worksheet.Name} ({num})";
							num++;
						}
						excelPackage.Workbook.Worksheets.Add(name, worksheet);
					}
					catch
					{
					}
				}
			}
			excelPackage.Save();
		}
		byte[] fileContents = File.ReadAllBytes(text);
		for (int j = 0; j < listFileName.Count; j++)
		{
			if (File.Exists(listFileName[j]))
			{
				File.Delete(listFileName[j]);
			}
		}
		return new CustomerFile(fileContents, fileName + Path.GetExtension(text));
	}

	private string ConvertDoubleToDegree(double value)
	{
		int num = (int)value;
		double num2 = Math.Abs(value - (double)num) * 60.0;
		int num3 = (int)num2;
		double a = (num2 - (double)num3) * 60.0;
		int num4 = (int)Math.Round(a);
		if (num4 == 60)
		{
			num4 = 0;
			num3++;
		}
		if (num3 == 60)
		{
			num3 = 0;
			num++;
		}
		return string.Format("{0}{1}{2}{3}{4}", (num == 0 && value < 0.0) ? "-" : "", num, "°", (num3 == 0) ? "" : string.Format("{0}{1}", num3, "'"), (num4 == 0) ? "" : string.Format("{0}{1}", num4, "\""));
	}
}
