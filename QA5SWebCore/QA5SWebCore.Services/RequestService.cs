using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Sockets;
using QA5SWebCore.Utilities.Constants;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Enums;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class RequestService : IRequestService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _hostingEnvironment;

	protected readonly IActionContextAccessor _ctxAccessor;

	private readonly WebSocketMessageHandler _socket;

	private readonly IEmailService _email;

	private readonly ITemplateService _template;

	private readonly IConfiguration _configuration;

	private readonly IMapper _mapper;

	private string IdentityName
	{
		get
		{
			try
			{
				if (_ctxAccessor.ActionContext.HttpContext.User != null)
				{
					return _ctxAccessor.ActionContext.HttpContext.User.Identity.Name;
				}
				throw new Exception();
			}
			catch
			{
				return null;
			}
		}
	}

	public RequestService(IUnitOfWork uow, IEmailService email, ITemplateService template, IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IActionContextAccessor ctxAccessor, WebSocketMessageHandler socket, IMapper mapper)
	{
		_uow = uow;
		_hostingEnvironment = hostingEnvironment;
		_ctxAccessor = ctxAccessor;
		_email = email;
		_template = template;
		_socket = socket;
		_configuration = configuration;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			res.Count = ((await _uow.GetRepository<RequestResult>().GetSingleAsync((RequestResult x) => x.RequestId.Equals(id) && x.Judge.Contains("NG"), "") != null) ? 1 : 0);
		}
		catch
		{
			res.Count = 0;
		}
		return res;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Request>().FindByAsync<RequestViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Request>().CountAsync(args.Predicate, args.PredicateParameters);
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
			Request att = (await _uow.GetRepository<Request>().FindByIdAsync(id)) ?? throw new Exception($"Can't find request with id: {id}");
			if (!att.Status.Contains("Activated") && !att.Status.Contains("Rejected"))
			{
				throw new Exception("Status is the " + att.Status + ". Can't delete this request");
			}
			_uow.GetRepository<Request>().Delete(att);
			await _uow.CommitAsync();
			res.Data = att;
			res.Count = 1;
			string message = JsonConvert.SerializeObject(att);
			await _socket.SendMessageToAll(message);
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

	public async Task<ResponseDto> Save(RequestViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Request();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Request att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<Request>().GetSingleAsync((Request x) => x.Name.Equals(model.Name) && x.Type == model.Type, "") != null)
				{
					throw new Exception("Name already exist");
				}
				att = new Request();
				_mapper.Map(model, att);
				att.Status = "Activated";
				att.Date = DateTime.Parse(att.Date.DateTime.ToShortDateString());
				_uow.GetRepository<Request>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<Request>().GetSingleAsync((Request x) => x.Name.Equals(model.Name) && x.Type == model.Type && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Name already exist");
				}
				att = await _uow.GetRepository<Request>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find request with id: {model.Id}");
				}
				model.Status = att.Status;
				model.Code = att.Code;
				_mapper.Map(model, att);
				att.Date = DateTime.Parse(att.Date.DateTime.ToShortDateString());
				_uow.GetRepository<Request>().Update(att);
			}
			await _uow.CommitAsync();
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(att.Id);
			res.Count = 1;
			string message = JsonConvert.SerializeObject(res.Data);
			await _socket.SendMessageToAll(message);
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

	public async Task<ResponseDto> UpdateFile(Guid id, IFormFile file)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			Request att = (await _uow.GetRepository<Request>().FindByIdAsync(id)) ?? throw new Exception($"Can't find request with id: {id}");
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "ResultFiles");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.Link))
			{
				string path = Path.Combine(text, att.Link);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			if (!file.Length.Equals(0L) && !string.IsNullOrEmpty(file.FileName))
			{
				string fileUniqueName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
				string path2 = Path.Combine(text, fileUniqueName);
				using FileStream stream = new FileStream(path2, FileMode.Create);
				await file.CopyToAsync(stream);
				att.Link = fileUniqueName;
			}
			else
			{
				att.Link = null;
			}
			await _uow.CommitAsync();
			res.Data = att;
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

	public async Task<CustomerFile> DownloadFile(Guid id)
	{
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "ResultFiles");
		string destPath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFileExport");
		Directory.CreateDirectory(sourcePath);
		Directory.CreateDirectory(destPath);
		RequestViewModel request = (await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(id)) ?? throw new Exception($"Can't find request with id: {id}");
		if (string.IsNullOrEmpty(request.Link))
		{
			throw new Exception("Request hasn't file");
		}
		Product product = (await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId.Value)) ?? throw new Exception($"Can't find product with id: {request.ProductId}");
		Template template = (await _uow.GetRepository<Template>().FindByIdAsync(product.TemplateId.Value)) ?? throw new Exception($"Can't find template with id: {product.TemplateId}");
		if (string.IsNullOrEmpty(template.TemplateUrl))
		{
			throw new Exception("Template hasn't file");
		}
		List<ExportMappingDto> source = JsonConvert.DeserializeObject<List<ExportMappingDto>>(template.TemplateData);
		List<PropertyInfo> list = new List<PropertyInfo>(typeof(RequestViewModel).GetProperties());
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (PropertyInfo item in list)
		{
			object value = item.GetValue(request, null);
			if (value != null)
			{
				dictionary.Add("[[" + item.Name.ToUpper() + "]]", value);
			}
		}
		string sourceFileName = Path.Combine(sourcePath, request.Link);
		string text = Path.Combine(destPath, request.Link);
		File.Copy(sourceFileName, text, overwrite: true);
		FileInfo newFile = new FileInfo(text);
		using (ExcelPackage excelPackage = new ExcelPackage(newFile))
		{
			IEnumerable<IGrouping<string, ExportMappingDto>> enumerable = from x in source
				group x by x.Sheet;
			foreach (IGrouping<string, ExportMappingDto> item2 in enumerable)
			{
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[item2.Key] ?? excelPackage.Workbook.Worksheets[0];
				foreach (ExportMappingDto item3 in item2)
				{
					dictionary.TryGetValue(item3.Value, out dynamic value2);
					if (MetaType.IsImage.Contains(item3.Value) && value2 != null)
					{
						string text2 = Path.Combine(_hostingEnvironment.WebRootPath, "AuthUserImage");
						Directory.CreateDirectory(text2);
						string filename = Path.Combine(text2, "condau.png");
						Bitmap bitmap = new Bitmap(filename);
						if (bitmap != null)
						{
							string mergedAddress = GetMergedAddress(excelWorksheet.Cells[item3.CellAddress]);
							int column = excelWorksheet.Cells[mergedAddress].Start.Column;
							int column2 = excelWorksheet.Cells[mergedAddress].End.Column;
							int row = excelWorksheet.Cells[mergedAddress].Start.Row;
							int row2 = excelWorksheet.Cells[mergedAddress].End.Row;
							ExcelPicture excelPicture = excelWorksheet.Drawings.AddPicture(Guid.NewGuid().ToString(), bitmap);
							excelPicture.From.Column = column - 1;
							excelPicture.From.Row = row - 1;
							int cellHeigh = GetCellHeigh(excelWorksheet, row, row2);
							int cellWidth = GetCellWidth(excelWorksheet, column, column2);
							excelPicture.SetSize(cellWidth, cellHeigh);
						}
						dictionary.TryGetValue(item3.Value.Replace("BY", ""), out dynamic value3);
						value2 = $"\r\n{(object?)value2}\r\n{(object?)value3.DateTime.ToShortDateString()}";
						excelWorksheet.Cells[item3.CellAddress].Value = (object)(value2 ?? null);
					}
				}
			}
			excelPackage.Save();
		}
		byte[] fileContents = File.ReadAllBytes(text);
		return new CustomerFile(fileContents, request.Name + Path.GetExtension(text));
	}

	public async Task<ResponseDto> SaveSample(RequestViewModel model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Request att = (await _uow.GetRepository<Request>().FindByIdAsync(model.Id)) ?? throw new Exception($"Can't find request with id: {model.Id}");
			att.Sample = model.Sample.Value;
			_uow.GetRepository<Request>().Update(att);
			await _uow.CommitAsync();
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(att.Id);
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

	public async Task<ResponseDto> SaveList(IEnumerable<RequestViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (models == null)
			{
				throw new Exception("Models is null");
			}
			List<Request> anons = new List<Request>();
			foreach (var item in models.Select((RequestViewModel Value, int Index) => new { Value, Index }))
			{
				RequestViewModel model = item.Value;
				int index = item.Index + 1;
				string Message = string.Empty;
				if (string.IsNullOrEmpty(model.Code))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Code is empty;" : " Code is empty;");
				}
				else
				{
					if (await _uow.GetRepository<Request>().GetSingleAsync((Request x) => x.Code.Equals(model.Code), "") != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
					Request request = anons.FirstOrDefault((Request x) => x.Code.Equals(model.Code));
					if (request != null)
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
					if (await _uow.GetRepository<Request>().GetSingleAsync((Request x) => x.Name.Equals(model.Name), "") != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Name already exist;" : " Name already exist;");
					}
					Request request2 = anons.FirstOrDefault((Request x) => x.Name.Equals(model.Name));
					if (request2 != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Name already exist;" : " Name already exist;");
					}
				}
				int samplemax = 1;
				if (string.IsNullOrEmpty(model.ProductCode))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Product code is empty;" : " Product code is empty;");
				}
				else
				{
					Product product = await _uow.GetRepository<Product>().GetSingleAsync((Product x) => x.Code.Equals(model.ProductCode), "");
					if (product == null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Product don't exist;" : " Product don't exist;");
					}
					else
					{
						model.ProductId = product.Id;
						samplemax = product.SampleMax;
					}
				}
				if (!model.Sample.HasValue)
				{
					Message += ((Message == string.Empty) ? $"No {index}: Sample is empty;" : " Sample is empty;");
				}
				else
				{
					model.Sample = (model.Sample.Equals(0) ? new int?(1) : model.Sample);
					if (model.Sample > samplemax)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Sample > maximum sample;" : " Sample > maximum sample;");
					}
				}
				if (string.IsNullOrEmpty(model.Status))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Status is empty;" : " Status is empty;");
				}
				else if (!model.Status.Equals("Unactivated") && !model.Status.Equals("Activated"))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Status incorrect;" : " Status incorrect;");
				}
				anons.Add(_mapper.Map<Request>(model));
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
			_uow.GetRepository<Request>().Add(anons);
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

	public async Task<ResponseDto> Active(ActiveRequestDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Request att = (await _uow.GetRepository<Request>().FindByIdAsync(model.Id)) ?? throw new Exception($"Can't find request with id: {model.Id}");
			_ = att.Status;
			att.Status = model.Status;
			switch (model.Status)
			{
			case "Rejected":
				await addAllRequestStatusForRejectAsync(att);
				await addAllRequestStatisticForRejectAsync(att);
				break;
			case "Completed":
			{
				att.Completed = DateTimeOffset.Now;
				att.CompletedBy = IdentityName ?? null;
				await addAllRequestStatusAsync(att);
				await addRequestStatisticAsync(att);
				if (model.Judgement == "RANK5")
				{
					att.Judgement = model.Judgement;
				}
				else
				{
					att.Judgement = ((await getRequestStatusNG(att.Id)) ? "NG" : "OK");
				}
				EmailDto email = await createdEmailDto(att);
				await _email.SendEmailAsync(email);
				email = await SendEmailDtoForChangeRequestStatus(att);
				await _email.SendEmailAsync(email);
				break;
			}
			case "Checked":
			{
				att.Judgement = model.Judgement;
				att.Checked = DateTimeOffset.Now;
				att.CheckedBy = IdentityName ?? null;
				EmailDto email = await SendEmailDtoForChangeRequestStatus(att);
				await _email.SendEmailAsync(email);
				break;
			}
			case "Approved":
				att.Judgement = model.Judgement;
				att.Approved = DateTimeOffset.Now;
				att.ApprovedBy = IdentityName ?? null;
				break;
			}
			_uow.GetRepository<Request>().Update(att);
			await _uow.CommitAsync();
			res.Data = att;
			if (model.Status.Equals("Completed"))
			{
				await CreateFile(att);
			}
			att.Product = null;
			string message = JsonConvert.SerializeObject(att);
			await _socket.SendMessageToAll(message);
			if (model.Status.Equals("Approved"))
			{
				await SaveFileFinishAsync(att);
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

	public async Task<ResponseDto> SaveListResult(Guid id, IEnumerable<Guid> ids)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (ids == null)
			{
				throw new Exception("List id is null");
			}
			IEnumerable<RequestResult> entities = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(id), "");
			_uow.GetRepository<RequestResult>().Delete(entities);
			List<RequestResult> results = new List<RequestResult>();
			foreach (Guid id2 in ids)
			{
				RequestResultViewModel requestResultViewModel = await _uow.GetRepository<Measurement>().FindByIdAsync<RequestResultViewModel>(id2);
				if (requestResultViewModel != null)
				{
					requestResultViewModel.Id = Guid.Empty;
					requestResultViewModel.RequestId = id;
					requestResultViewModel.Sample = 1;
					results.Add(_mapper.Map<RequestResult>(requestResultViewModel));
				}
			}
			_uow.GetRepository<RequestResult>().Add(results);
			await _uow.CommitAsync();
			res.Data = results;
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

	public async Task<ResponseDto> UpdateStatistics()
	{
		ResponseDto res = new ResponseDto();
		try
		{
			IEnumerable<Request> requests = await _uow.GetRepository<Request>().FindByAsync((Request x) => x.Status.Equals("Completed") || x.Status.Equals("Checked") || x.Status.Equals("Approved"), "");
			foreach (Request item in requests)
			{
				await addRequestStatisticAsync(item);
			}
			res.Count = requests.Count();
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

	private async Task addAllRequestStatusAsync(Request request)
	{
		Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId);
		int i = 1;
		while (true)
		{
			if (i > request.Sample)
			{
				break;
			}
			List<Plan> list = (await _uow.GetRepository<Plan>().FindByAsync((Plan x) => x.ProductId.Equals(request.ProductId), "Sort, Created")).ToList();
			if (list.Count.Equals(0))
			{
				list.Add(new Plan());
			}
			foreach (Plan plan in list)
			{
				IEnumerable<RequestResult> results = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(request.Id) && x.Sample.Equals(i) && !string.IsNullOrEmpty(x.Result), "");
				int ok;
				int ng;
				int empty;
				if (plan.ProductId == Guid.Empty)
				{
					IEnumerable<Measurement> meass = await _uow.GetRepository<Measurement>().FindByAsync((Measurement x) => x.ProductId.Equals(request.ProductId), "");
					ok = results.Where((RequestResult x) => meass.Any((Measurement y) => y.Id.Equals(x.MeasurementId)) && x.Judge.Contains("OK")).Count();
					ng = results.Where((RequestResult x) => meass.Any((Measurement y) => y.Id.Equals(x.MeasurementId)) && x.Judge.Contains("NG")).Count();
					empty = meass.Count() * product.Cavity - ok - ng;
				}
				else
				{
					IEnumerable<PlanDetail> _meass = await _uow.GetRepository<PlanDetail>().FindByAsync((PlanDetail x) => x.PlanId.Equals(plan.Id), "");
					ok = results.Where((RequestResult x) => _meass.Any((PlanDetail y) => y.MeasurementId.Equals(x.MeasurementId)) && x.Judge.Contains("OK")).Count();
					ng = results.Where((RequestResult x) => _meass.Any((PlanDetail y) => y.MeasurementId.Equals(x.MeasurementId)) && x.Judge.Contains("NG")).Count();
					empty = _meass.Count() * product.Cavity - ok - ng;
				}
				Guid? planid = null;
				if (plan.Id != Guid.Empty)
				{
					planid = plan.Id;
				}
				string status = "OK";
				if (ng > 0)
				{
					status = "NG";
				}
				RequestStatus requestStatus = await _uow.GetRepository<RequestStatus>().GetSingleAsync((RequestStatus x) => x.RequestId.Equals(request.Id) && ((object)x.PlanId).Equals((object?)planid) && x.Sample.Equals(i), "");
				if (requestStatus == null)
				{
					requestStatus = new RequestStatus
					{
						RequestId = request.Id,
						PlanId = planid,
						Sample = i,
						OK = ok,
						NG = ng,
						Empty = empty,
						Status = status,
						Completed = DateTimeOffset.Now,
						CompletedBy = request.CompletedBy,
						Checked = DateTimeOffset.Now,
						CheckedBy = request.CompletedBy,
						Approved = DateTimeOffset.Now,
						ApprovedBy = request.CompletedBy
					};
					_uow.GetRepository<RequestStatus>().Add(requestStatus);
				}
				else
				{
					requestStatus.OK = ok;
					requestStatus.NG = ng;
					requestStatus.Empty = empty;
					requestStatus.Status = status;
					requestStatus.Completed = DateTimeOffset.Now;
					requestStatus.CompletedBy = request.CompletedBy;
					requestStatus.Checked = DateTimeOffset.Now;
					requestStatus.CheckedBy = request.CompletedBy;
					requestStatus.Approved = DateTimeOffset.Now;
					requestStatus.ApprovedBy = request.CompletedBy;
					_uow.GetRepository<RequestStatus>().Update(requestStatus);
				}
			}
			i++;
		}
		await _uow.CommitAsync();
	}

	private async Task addAllRequestStatusForRejectAsync(Request request)
	{
		IEnumerable<RequestStatus> enumerable = await _uow.GetRepository<RequestStatus>().FindByAsync((RequestStatus x) => x.RequestId.Equals(request.Id), "");
		foreach (RequestStatus item in enumerable)
		{
			item.Status = "Rejected";
		}
		_uow.GetRepository<RequestStatus>().Update(enumerable);
		await _uow.CommitAsync();
	}

	private async Task addRequestStatisticAsync(Request request)
	{
		ProductViewModel product = await _uow.GetRepository<Product>().FindByIdAsync<ProductViewModel>(request.ProductId);
		IEnumerable<RequestResult> results = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(request.Id) && !string.IsNullOrEmpty(x.Result), "");
		int resultNG = results.Count((RequestResult x) => x.Judge.Contains("NG"));
		IEnumerable<IGrouping<string, RequestResult>> enumerable = from x in results
			group x by $"{x.Sample}_{x.Cavity}";
		int num = 0;
		foreach (IGrouping<string, RequestResult> item in enumerable)
		{
			int num2 = item.Count((RequestResult x) => x.Judge.Contains("NG"));
			if (num2 > 0)
			{
				num++;
			}
		}
		RequestStatistic statistic = new RequestStatistic
		{
			RequestId = request.Id,
			ResultAll = results.Count(),
			MeasAll = product.TotalMeas.Value,
			ResultNG = resultNG,
			SampleNG = num
		};
		RequestStatistic requestStatistic = await _uow.GetRepository<RequestStatistic>().GetSingleAsync((RequestStatistic x) => x.RequestId.Equals(request.Id), "");
		if (requestStatistic == null)
		{
			_uow.GetRepository<RequestStatistic>().Add(statistic);
		}
		else
		{
			requestStatistic.ResultAll = statistic.ResultAll;
			requestStatistic.MeasAll = statistic.MeasAll;
			requestStatistic.ResultNG = statistic.ResultNG;
			requestStatistic.SampleNG = statistic.SampleNG;
			_uow.GetRepository<RequestStatistic>().Update(requestStatistic);
		}
		IEnumerable<RequestResult> source = results.Where((RequestResult x) => x.Judge.Contains("NG"));
		IEnumerable<IGrouping<Guid?, RequestResult>> enumerable2 = from x in source
			group x by x.MeasurementId;
		foreach (IGrouping<Guid?, RequestResult> item2 in enumerable2)
		{
			RequestResultStatistic _statistic = new RequestResultStatistic
			{
				RequestId = request.Id,
				MeasurementId = item2.Key.Value,
				NG = item2.Count()
			};
			RequestResultStatistic requestResultStatistic = await _uow.GetRepository<RequestResultStatistic>().GetSingleAsync((RequestResultStatistic x) => x.RequestId.Equals(request.Id) && ((object)x.MeasurementId).Equals((object?)_statistic.MeasurementId), "");
			if (requestResultStatistic == null)
			{
				_uow.GetRepository<RequestResultStatistic>().Add(_statistic);
				continue;
			}
			requestResultStatistic.NG = _statistic.NG;
			_uow.GetRepository<RequestResultStatistic>().Update(requestResultStatistic);
		}
		await _uow.CommitAsync();
	}

	private async Task addAllRequestStatisticForRejectAsync(Request request)
	{
		IEnumerable<RequestStatistic> details = await _uow.GetRepository<RequestStatistic>().FindByAsync((RequestStatistic x) => x.RequestId.Equals(request.Id), "");
		IEnumerable<RequestResultStatistic> entities = await _uow.GetRepository<RequestResultStatistic>().FindByAsync((RequestResultStatistic x) => x.RequestId.Equals(request.Id), "");
		_uow.GetRepository<RequestStatistic>().Delete(details);
		_uow.GetRepository<RequestResultStatistic>().Delete(entities);
		await _uow.CommitAsync();
	}

	private async Task<bool> getRequestStatusNG(Guid id)
	{
		return await _uow.GetRepository<RequestStatus>().AnyAsync((RequestStatus x) => x.RequestId.Equals(id) && x.NG > (int?)0);
	}

	private async Task CreateFile(Request request)
	{
		string cryptedString = _configuration["Key"];
		ConfigApp configApp = System.Text.Json.JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		if (!configApp.Funtion.Contains("QC2106"))
		{
			return;
		}
		Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId);
		if (product == null)
		{
			return;
		}
		IEnumerable<RequestResultViewModel> enumerable = await _uow.GetRepository<RequestResult>().FindByAsync<RequestResultViewModel>((RequestResult x) => x.RequestId.Equals(request.Id));
		List<PropertyInfo> list = new List<PropertyInfo>(request.GetType().GetProperties());
		string text = Path.Combine(_hostingEnvironment.WebRootPath, "BACKUP MEASUREMENT DATA", (product.Code + " (" + product.Name + ")").Replace("\\", "").Replace("/", ""));
		Directory.CreateDirectory(text);
		string path = Path.Combine(text, (request.Name + "_" + request.Type + ".txt").Replace("\\", "").Replace("/", ""));
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		using FileStream stream = File.Create(path);
		using StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
		streamWriter.WriteLine("===========================REQUEST==================================");
		foreach (PropertyInfo item in list)
		{
			streamWriter.Write(item.Name + "\t");
		}
		streamWriter.WriteLine();
		foreach (PropertyInfo item2 in list)
		{
			streamWriter.Write($"{item2.GetValue(request, null)}\t");
		}
		streamWriter.WriteLine("\r\n===========================RESULTS==================================");
		list = new List<PropertyInfo>(typeof(RequestResultViewModel).GetProperties());
		foreach (PropertyInfo item3 in list)
		{
			streamWriter.Write(item3.Name + "\t");
		}
		foreach (RequestResultViewModel item4 in enumerable)
		{
			streamWriter.WriteLine();
			foreach (PropertyInfo item5 in list)
			{
				streamWriter.Write($"{item5.GetValue(item4, null)}\t");
			}
		}
	}

	private string CreatedCol(string value)
	{
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				value = "";
			}
			if (value.Length >= 20)
			{
				return value;
			}
			int num = (20 - value.Length) / 2;
			int count = 20 - value.Length - num;
			return new string(' ', num) + value + new string(' ', count);
		}
		catch
		{
			return "";
		}
	}

	private async Task<EmailDto> createdEmailDto(Request request)
	{
		string cryptedString = _configuration["Key"];
		ConfigApp configApp = System.Text.Json.JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		if (!configApp.Funtion.Contains("QC2202"))
		{
			return null;
		}
		Email email = await _uow.GetRepository<Email>().GetSingleAsync((Email x) => x.Id != null, "");
		if (email == null || string.IsNullOrEmpty(email.Name) || string.IsNullOrEmpty(email.Password))
		{
			return null;
		}
		EmailDto temp = await CreatedToEmail(request.ProductId);
		if (string.IsNullOrEmpty(temp.ToEmail))
		{
			return null;
		}
		string abnormal = await CalAbnormal(request);
		string text = await CalOKNG(request);
		if (string.IsNullOrEmpty(abnormal) && string.IsNullOrEmpty(text))
		{
			return null;
		}
		EmailDto emailDto = new EmailDto();
		emailDto.ToEmail = temp.ToEmail;
		emailDto.CcEmail = (string.IsNullOrEmpty(temp.CcEmail) ? null : temp.CcEmail);
		emailDto.Subject = "Request [" + request.Code + "][" + request.Name + "] occurred abnormal state";
		EmailDto emailDto2 = emailDto;
		string text2 = email.Header ?? "";
		text2 = text2 + "<p>Information of request [" + request.Code + "][" + request.Name + "]:</p>";
		text2 += $"<p>&emsp;* Date: {request.Date:dd/MMM/yyyy}.</p>";
		text2 = text2 + "<p>&emsp;* Stage name: " + request.Product?.Name + ".</p>";
		if (!string.IsNullOrEmpty(abnormal))
		{
			text2 += "<p><b style=\"color: blue;\">Abnormal state information:</b></p>";
			text2 += abnormal;
		}
		if (!string.IsNullOrEmpty(text))
		{
			text2 += "<p><b style=\"color: blue;\">Measurement is defective:</b></p>";
			text2 += text;
		}
		text2 += email.Footer;
		emailDto2.Content = text2;
		return emailDto2;
	}

	private async Task<EmailDto> CreatedToEmail(Guid productid)
	{
		EmailDto emailDto = new EmailDto();
		ProductViewModel product = await _uow.GetRepository<Product>().FindByIdAsync<ProductViewModel>(productid);
		if (product == null)
		{
			return emailDto;
		}
		IEnumerable<Setting> settings = await _uow.GetRepository<Setting>().FindByAsync((Setting x) => x.Group == "gbPosition" && !x.IsChecked, "");
		IEnumerable<AuthUserViewModel> enumerable = await _uow.GetRepository<AuthUser>().FindByAsync<AuthUserViewModel>((AuthUser x) => x.IsActivated && x.IsEmail != false && !string.IsNullOrEmpty(x.Email));
		foreach (Setting item in settings)
		{
			switch (item.Name)
			{
			case "cbManager":
				enumerable = enumerable.Where((AuthUserViewModel x) => x.Role != RoleWeb.SuperAdministrator);
				break;
			case "gbOKNG":
				enumerable = enumerable.Where((AuthUserViewModel x) => x.Role != RoleWeb.Administrator);
				break;
			case "cbStaff":
				enumerable = enumerable.Where((AuthUserViewModel x) => x.Role != RoleWeb.Member && x.Role != RoleWeb.None);
				break;
			}
		}
		List<AuthUserViewModel> list = new List<AuthUserViewModel>();
		if (!settings.Any((Setting x) => x.Name == "cbDepartment"))
		{
			foreach (AuthUserViewModel item2 in enumerable)
			{
				if ((!(item2.JobTitle != "Supervisor") || !(item2.JobTitle != "Manager") || !(item2.DepartmentId != product.DepartmentId)) && product.DepartmentName.StartsWith(item2.DepartmentName))
				{
					list.Add(item2);
				}
			}
		}
		else
		{
			list.AddRange(enumerable);
		}
		foreach (AuthUserViewModel item3 in list)
		{
			if (string.IsNullOrEmpty(emailDto.ToEmail))
			{
				emailDto.ToEmail = item3.Email;
			}
			else if (!emailDto.ToEmail.Contains(item3.Email))
			{
				if (string.IsNullOrEmpty(emailDto.CcEmail))
				{
					emailDto.CcEmail += item3.Email;
				}
				else if (!emailDto.CcEmail.Contains(item3.Email))
				{
					emailDto.CcEmail = emailDto.CcEmail + ";" + item3.Email;
				}
			}
		}
		return emailDto;
	}

	private async Task<string> CalAbnormal(Request request)
	{
		string content = string.Empty;
		QueryArgs queryArgs = new QueryArgs();
		queryArgs.Page = 1;
		queryArgs.Limit = 29;
		queryArgs.Order = "Date DESC, Created DESC";
		queryArgs.Predicate = "Id!=@0 && ProductId=@1 && Date<=@2 && !Status.Contains(@3) && !Status.Contains(@4)";
		object[] predicateParameters = new string[5]
		{
			request.Id.ToString(),
			request.ProductId.ToString(),
			request.Date.ToString("yyyy/MM/dd"),
			"Activated",
			"Rejected"
		};
		queryArgs.PredicateParameters = predicateParameters;
		QueryArgs queryArgs2 = queryArgs;
		List<Request> list = (await _uow.GetRepository<Request>().FindByAsync(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)).ToList();
		list.Add(request);
		List<RequestResult> allResults = new List<RequestResult>();
		foreach (Request req in list)
		{
			allResults.AddRange(await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(req.Id) && !string.IsNullOrEmpty(x.Result) && x.Measurement.ImportantId.HasValue, "Measurement.Sort"));
		}
		if (allResults.Count == 0)
		{
			return null;
		}
		IEnumerable<Setting> settings = await _uow.GetRepository<Setting>().FindByAsync((Setting x) => x.Group == "gbAbnormal" && x.IsChecked, "");
		if (settings == null || !settings.Any((Setting x) => x.Name == "cbAbnormalActive"))
		{
			return null;
		}
		List<double> list2 = allResults.Select((RequestResult x) => double.Parse(x.Result)).ToList();
		CommonCpk commonCpk = new CommonCpk
		{
			CL = list2.Average(),
			SD = mMath.CalStandardDeviation(list2)
		};
		commonCpk.Sigma1 = commonCpk.CL + commonCpk.SD;
		commonCpk.MSigma1 = commonCpk.CL - commonCpk.SD;
		commonCpk.Sigma2 = commonCpk.Sigma1 + commonCpk.SD;
		commonCpk.MSigma2 = commonCpk.MSigma1 - commonCpk.SD;
		commonCpk.Sigma3 = commonCpk.Sigma2 + commonCpk.SD;
		commonCpk.MSigma3 = commonCpk.MSigma2 - commonCpk.SD;
		commonCpk.UCL = (allResults.First().Measurement.UCL.HasValue ? allResults.First().Measurement.UCL.Value : commonCpk.Sigma3);
		commonCpk.LCL = (allResults.First().Measurement.LCL.HasValue ? allResults.First().Measurement.LCL.Value : commonCpk.MSigma3);
		IEnumerable<IGrouping<Guid?, RequestResult>> enumerable = from x in allResults
			group x by x.MeasurementId;
		foreach (IGrouping<Guid?, RequestResult> groups in enumerable)
		{
			List<DataCpk> datacpkAlls = new List<DataCpk>();
			IEnumerable<IGrouping<Guid, RequestResult>> groupRequests = from x in groups
				group x by x.RequestId;
			foreach (IGrouping<Guid, RequestResult> item in groupRequests)
			{
				IEnumerable<double> source = item.Select((RequestResult x) => double.Parse(x.Result));
				datacpkAlls.Add(new DataCpk
				{
					Average = source.Average(),
					Range = source.Max() - source.Min()
				});
			}
			commonCpk.AveR = datacpkAlls.Average((DataCpk x) => x.Range);
			Constant constant = await _uow.GetRepository<Constant>().GetSingleAsync((Constant x) => x.n.Equals(groupRequests.First().Count()), "");
			if (constant == null)
			{
				continue;
			}
			commonCpk.RUCL = constant.D4 * commonCpk.AveR;
			if (groupRequests.Last().Key.Equals(request.Id))
			{
				string text = string.Empty;
				if (settings.Any((Setting x) => x.Name == "cbXtb1") && (datacpkAlls.Last().Average > commonCpk.UCL || datacpkAlls.Last().Average < commonCpk.LCL))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;① 1 điểm nằm ngoài 3 sigma từ đường trung tâm. / 1点が領域3シグマ(UCL&LCL)を超えている。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;① 1 điểm nằm ngoài 3 sigma từ đường trung tâm. / 1点が領域3シグマ(UCL&LCL)を超えている。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb2") && datacpkAlls.Count > 2 && ((datacpkAlls[0].Average > commonCpk.Sigma2 && datacpkAlls[1].Average > commonCpk.Sigma2) || (datacpkAlls[0].Average > commonCpk.Sigma2 && datacpkAlls.Last().Average > commonCpk.Sigma2) || (datacpkAlls[1].Average > commonCpk.Sigma2 && datacpkAlls.Last().Average > commonCpk.Sigma2) || (datacpkAlls[0].Average < commonCpk.MSigma2 && datacpkAlls[1].Average < commonCpk.MSigma2) || (datacpkAlls[0].Average < commonCpk.MSigma2 && datacpkAlls.Last().Average < commonCpk.MSigma2) || (datacpkAlls[1].Average < commonCpk.MSigma2 && datacpkAlls.Last().Average < commonCpk.MSigma2)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;② 2 trong 3 điểm > 2 sigma từ đường trung tâm cùng phía. / 連続する３点中、2点（同じ側）が領域２シグマ又はそれを超えた領域にある。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;② 2 trong 3 điểm > 2 sigma từ đường trung tâm cùng phía. / 連続する３点中、2点（同じ側）が領域２シグマ又はそれを超えた領域にある。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb3") && datacpkAlls.Count > 4 && ((datacpkAlls[3].Average > commonCpk.Sigma1 && datacpkAlls[2].Average > commonCpk.Sigma1 && datacpkAlls[1].Average > commonCpk.Sigma1 && datacpkAlls[0].Average > commonCpk.Sigma1) || (datacpkAlls[3].Average > commonCpk.Sigma1 && datacpkAlls[2].Average > commonCpk.Sigma1 && datacpkAlls[1].Average > commonCpk.Sigma1 && datacpkAlls.Last().Average > commonCpk.Sigma1) || (datacpkAlls[3].Average > commonCpk.Sigma1 && datacpkAlls[2].Average > commonCpk.Sigma1 && datacpkAlls[0].Average > commonCpk.Sigma1 && datacpkAlls.Last().Average > commonCpk.Sigma1) || (datacpkAlls[3].Average > commonCpk.Sigma1 && datacpkAlls[1].Average > commonCpk.Sigma1 && datacpkAlls[0].Average > commonCpk.Sigma1 && datacpkAlls.Last().Average > commonCpk.Sigma1) || (datacpkAlls[2].Average > commonCpk.Sigma1 && datacpkAlls[1].Average > commonCpk.Sigma1 && datacpkAlls[0].Average > commonCpk.Sigma1 && datacpkAlls.Last().Average > commonCpk.Sigma1) || (datacpkAlls[3].Average < commonCpk.MSigma1 && datacpkAlls[2].Average < commonCpk.MSigma1 && datacpkAlls[1].Average < commonCpk.MSigma1 && datacpkAlls[0].Average < commonCpk.MSigma1) || (datacpkAlls[3].Average < commonCpk.MSigma1 && datacpkAlls[2].Average < commonCpk.MSigma1 && datacpkAlls[1].Average < commonCpk.MSigma1 && datacpkAlls.Last().Average < commonCpk.MSigma1) || (datacpkAlls[3].Average < commonCpk.MSigma1 && datacpkAlls[2].Average < commonCpk.MSigma1 && datacpkAlls[0].Average < commonCpk.MSigma1 && datacpkAlls.Last().Average < commonCpk.MSigma1) || (datacpkAlls[3].Average < commonCpk.MSigma1 && datacpkAlls[1].Average < commonCpk.MSigma1 && datacpkAlls[0].Average < commonCpk.MSigma1 && datacpkAlls.Last().Average < commonCpk.MSigma1) || (datacpkAlls[2].Average < commonCpk.MSigma1 && datacpkAlls[1].Average < commonCpk.MSigma1 && datacpkAlls[0].Average < commonCpk.MSigma1 && datacpkAlls.Last().Average < commonCpk.MSigma1)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;③ 4 trong 5 điểm > 1 sigma từ đường trung tâm cùng phía. / 連続する5点中、4点（同じ側）が領域２シグマ又はそれを超えた領域にある。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;③ 4 trong 5 điểm > 1 sigma từ đường trung tâm cùng phía. / 連続する5点中、4点（同じ側）が領域２シグマ又はそれを超えた領域にある。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb4") && datacpkAlls.Count > 5 && ((datacpkAlls[4].Average > datacpkAlls[3].Average && datacpkAlls[3].Average > datacpkAlls[2].Average && datacpkAlls[2].Average > datacpkAlls[1].Average && datacpkAlls[1].Average > datacpkAlls[0].Average && datacpkAlls[0].Average > datacpkAlls.Last().Average) || (datacpkAlls[4].Average < datacpkAlls[3].Average && datacpkAlls[3].Average < datacpkAlls[2].Average && datacpkAlls[2].Average < datacpkAlls[1].Average && datacpkAlls[1].Average < datacpkAlls[0].Average && datacpkAlls[0].Average < datacpkAlls.Last().Average)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;④ 6 điểm liên tục ,tất cả cùng tăng hay giảm. / ６点が増加又は、減少している。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;④ 6 điểm liên tục ,tất cả cùng tăng hay giảm. / ６点が増加又は、減少している。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb5") && datacpkAlls.Count > 6 && ((datacpkAlls[5].Average > commonCpk.CL && datacpkAlls[4].Average > commonCpk.CL && datacpkAlls[3].Average > commonCpk.CL && datacpkAlls[2].Average > commonCpk.CL && datacpkAlls[1].Average > commonCpk.CL && datacpkAlls[0].Average > commonCpk.CL && datacpkAlls.Last().Average > commonCpk.CL) || (datacpkAlls[5].Average < commonCpk.CL && datacpkAlls[4].Average < commonCpk.CL && datacpkAlls[3].Average < commonCpk.CL && datacpkAlls[2].Average < commonCpk.CL && datacpkAlls[1].Average < commonCpk.CL && datacpkAlls[0].Average < commonCpk.CL && datacpkAlls.Last().Average < commonCpk.CL)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;⑤ 7 điểm liên tiếp ở cùng 1 phía của đường trung tâm. / ７点が中心値に対して同じ側にある。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;⑤ 7 điểm liên tiếp ở cùng 1 phía của đường trung tâm. / ７点が中心値に対して同じ側にある。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb6") && datacpkAlls.Count > 7 && (datacpkAlls[6].Average > commonCpk.Sigma1 || datacpkAlls[6].Average < commonCpk.MSigma1) && (datacpkAlls[5].Average > commonCpk.Sigma1 || datacpkAlls[5].Average < commonCpk.MSigma1) && (datacpkAlls[4].Average > commonCpk.Sigma1 || datacpkAlls[4].Average < commonCpk.MSigma1) && (datacpkAlls[3].Average > commonCpk.Sigma1 || datacpkAlls[3].Average < commonCpk.MSigma1) && (datacpkAlls[2].Average > commonCpk.Sigma1 || datacpkAlls[2].Average < commonCpk.MSigma1) && (datacpkAlls[1].Average > commonCpk.Sigma1 || datacpkAlls[1].Average < commonCpk.MSigma1) && (datacpkAlls[0].Average > commonCpk.Sigma1 || datacpkAlls[0].Average < commonCpk.MSigma1) && (datacpkAlls.Last().Average > commonCpk.Sigma1 || datacpkAlls.Last().Average < commonCpk.MSigma1))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;⑥ 8 điểm liên tục>1 sigma từ đường trung tâm phí bất kì. / 連続する8点が領域1シグマを超えた領域にある。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;⑥ 8 điểm liên tục>1 sigma từ đường trung tâm phí bất kì. / 連続する8点が領域1シグマを超えた領域にある。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb7") && datacpkAlls.Count > 13 && ((datacpkAlls[12].Average > datacpkAlls[11].Average && datacpkAlls[11].Average < datacpkAlls[10].Average && datacpkAlls[10].Average > datacpkAlls[9].Average && datacpkAlls[9].Average < datacpkAlls[8].Average && datacpkAlls[8].Average > datacpkAlls[7].Average && datacpkAlls[7].Average < datacpkAlls[6].Average && datacpkAlls[6].Average > datacpkAlls[5].Average && datacpkAlls[5].Average < datacpkAlls[4].Average && datacpkAlls[4].Average > datacpkAlls[3].Average && datacpkAlls[3].Average < datacpkAlls[2].Average && datacpkAlls[2].Average > datacpkAlls[1].Average && datacpkAlls[1].Average < datacpkAlls[0].Average && datacpkAlls[0].Average > datacpkAlls.Last().Average) || (datacpkAlls[12].Average < datacpkAlls[11].Average && datacpkAlls[11].Average > datacpkAlls[10].Average && datacpkAlls[10].Average < datacpkAlls[9].Average && datacpkAlls[9].Average > datacpkAlls[8].Average && datacpkAlls[8].Average < datacpkAlls[7].Average && datacpkAlls[7].Average > datacpkAlls[6].Average && datacpkAlls[6].Average < datacpkAlls[5].Average && datacpkAlls[5].Average > datacpkAlls[4].Average && datacpkAlls[4].Average < datacpkAlls[3].Average && datacpkAlls[3].Average > datacpkAlls[2].Average && datacpkAlls[2].Average < datacpkAlls[1].Average && datacpkAlls[1].Average > datacpkAlls[0].Average && datacpkAlls[0].Average < datacpkAlls.Last().Average)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;⑦ 14 điểm liên tục thay đổi lên xuống-lên xuống. / 14の点が交互に増減している。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;⑦ 14 điểm liên tục thay đổi lên xuống-lên xuống. / 14の点が交互に増減している。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbXtb8") && datacpkAlls.Count > 14 && datacpkAlls[13].Average < commonCpk.Sigma1 && datacpkAlls[13].Average > commonCpk.MSigma1 && datacpkAlls[12].Average < commonCpk.Sigma1 && datacpkAlls[12].Average > commonCpk.MSigma1 && datacpkAlls[11].Average < commonCpk.Sigma1 && datacpkAlls[11].Average > commonCpk.MSigma1 && datacpkAlls[10].Average < commonCpk.Sigma1 && datacpkAlls[10].Average > commonCpk.MSigma1 && datacpkAlls[9].Average < commonCpk.Sigma1 && datacpkAlls[9].Average > commonCpk.MSigma1 && datacpkAlls[8].Average < commonCpk.Sigma1 && datacpkAlls[8].Average > commonCpk.MSigma1 && datacpkAlls[7].Average < commonCpk.Sigma1 && datacpkAlls[7].Average > commonCpk.MSigma1 && datacpkAlls[6].Average < commonCpk.Sigma1 && datacpkAlls[6].Average > commonCpk.MSigma1 && datacpkAlls[5].Average < commonCpk.Sigma1 && datacpkAlls[5].Average > commonCpk.MSigma1 && datacpkAlls[4].Average < commonCpk.Sigma1 && datacpkAlls[4].Average > commonCpk.MSigma1 && datacpkAlls[3].Average < commonCpk.Sigma1 && datacpkAlls[3].Average > commonCpk.MSigma1 && datacpkAlls[2].Average < commonCpk.Sigma1 && datacpkAlls[2].Average > commonCpk.MSigma1 && datacpkAlls[1].Average < commonCpk.Sigma1 && datacpkAlls[1].Average > commonCpk.MSigma1 && datacpkAlls[0].Average < commonCpk.Sigma1 && datacpkAlls[0].Average > commonCpk.MSigma1 && datacpkAlls.Last().Average < commonCpk.Sigma1 && datacpkAlls.Last().Average > commonCpk.MSigma1)
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;⑧ 15 điểm liên tục trong khoảng 1 sigma từ đường trung tâm phía bất kì. / 連続する15点が領域１シグマに存在する。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;⑧ 15 điểm liên tục trong khoảng 1 sigma từ đường trung tâm phía bất kì. / 連続する15点が領域１シグマに存在する。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbR1") && datacpkAlls.Last().Range > commonCpk.RUCL)
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;MR: ① 1 điểm nằm ngoài 3 sigma từ đường trung tâm. / 1点が領域3シグマ(UCL&LCL)を超えている。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;MR: ① 1 điểm nằm ngoài 3 sigma từ đường trung tâm. / 1点が領域3シグマ(UCL&LCL)を超えている。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbR2") && datacpkAlls.Count > 5 && ((datacpkAlls[4].Range > datacpkAlls[3].Range && datacpkAlls[3].Range > datacpkAlls[2].Range && datacpkAlls[2].Range > datacpkAlls[1].Range && datacpkAlls[1].Range > datacpkAlls[0].Range && datacpkAlls[0].Range > datacpkAlls.Last().Range) || (datacpkAlls[4].Range < datacpkAlls[3].Range && datacpkAlls[3].Range < datacpkAlls[2].Range && datacpkAlls[2].Range < datacpkAlls[1].Range && datacpkAlls[1].Range < datacpkAlls[0].Range && datacpkAlls[0].Range < datacpkAlls.Last().Range)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;MR: ④ 6 điểm liên tục ,tất cả cùng tăng hay giảm. / ６点が増加又は、減少している。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;MR: ④ 6 điểm liên tục ,tất cả cùng tăng hay giảm. / ６点が増加又は、減少している。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbR3") && datacpkAlls.Count > 6 && ((datacpkAlls[5].Range > commonCpk.AveR && datacpkAlls[4].Range > commonCpk.AveR && datacpkAlls[3].Range > commonCpk.AveR && datacpkAlls[2].Range > commonCpk.AveR && datacpkAlls[1].Range > commonCpk.AveR && datacpkAlls[0].Range > commonCpk.AveR && datacpkAlls.Last().Range > commonCpk.AveR) || (datacpkAlls[5].Range < commonCpk.AveR && datacpkAlls[4].Range < commonCpk.AveR && datacpkAlls[3].Range < commonCpk.AveR && datacpkAlls[2].Range < commonCpk.AveR && datacpkAlls[1].Range < commonCpk.AveR && datacpkAlls[0].Range < commonCpk.AveR && datacpkAlls.Last().Range < commonCpk.AveR)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;MR: ⑤ 7 điểm liên tiếp ở cùng 1 phía của đường trung tâm. / ７点が中心値に対して同じ側にある。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;MR: ⑤ 7 điểm liên tiếp ở cùng 1 phía của đường trung tâm. / ７点が中心値に対して同じ側にある。</p>"));
				}
				if (settings.Any((Setting x) => x.Name == "cbR4") && datacpkAlls.Count > 13 && ((datacpkAlls[12].Range > datacpkAlls[11].Range && datacpkAlls[11].Range < datacpkAlls[10].Range && datacpkAlls[10].Range > datacpkAlls[9].Range && datacpkAlls[9].Range < datacpkAlls[8].Range && datacpkAlls[8].Range > datacpkAlls[7].Range && datacpkAlls[7].Range < datacpkAlls[6].Range && datacpkAlls[6].Range > datacpkAlls[5].Range && datacpkAlls[5].Range < datacpkAlls[4].Range && datacpkAlls[4].Range > datacpkAlls[3].Range && datacpkAlls[3].Range < datacpkAlls[2].Range && datacpkAlls[2].Range > datacpkAlls[1].Range && datacpkAlls[1].Range < datacpkAlls[0].Range && datacpkAlls[0].Range > datacpkAlls.Last().Range) || (datacpkAlls[12].Range < datacpkAlls[11].Range && datacpkAlls[11].Range > datacpkAlls[10].Range && datacpkAlls[10].Range < datacpkAlls[9].Range && datacpkAlls[9].Range > datacpkAlls[8].Range && datacpkAlls[8].Range < datacpkAlls[7].Range && datacpkAlls[7].Range > datacpkAlls[6].Range && datacpkAlls[6].Range < datacpkAlls[5].Range && datacpkAlls[5].Range > datacpkAlls[4].Range && datacpkAlls[4].Range < datacpkAlls[3].Range && datacpkAlls[3].Range > datacpkAlls[2].Range && datacpkAlls[2].Range < datacpkAlls[1].Range && datacpkAlls[1].Range > datacpkAlls[0].Range && datacpkAlls[0].Range < datacpkAlls.Last().Range)))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "<p>&emsp;&emsp;MR: ⑦ 14 điểm liên tục thay đổi lên xuống-lên xuống. / 14の点が交互に増減している。</p>") : (text + "<p>&emsp;* Item no. [" + groups.First().Measurement.Name + "]:</p><p>&emsp;&emsp;MR: ⑦ 14 điểm liên tục thay đổi lên xuống-lên xuống. / 14の点が交互に増減している。</p>"));
				}
				if (!string.IsNullOrEmpty(text))
				{
					content += text;
				}
			}
		}
		return content;
	}

	private async Task<string> CalOKNG(Request request)
	{
		string content = string.Empty;
		IEnumerable<RequestResult> results = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(request.Id) && !string.IsNullOrEmpty(x.Result) && x.Judge.Contains("NG"), "Measurement.Sort, Sample, Cavity");
		IEnumerable<Setting> enumerable = await _uow.GetRepository<Setting>().FindByAsync((Setting x) => x.Group == "gbOKNG" && x.IsChecked, "");
		if (enumerable == null || !enumerable.Any((Setting x) => x.Name == "cbOKNGActive"))
		{
			return null;
		}
		IEnumerable<IGrouping<Guid?, RequestResult>> enumerable2 = from x in results
			group x by x.MeasurementId;
		foreach (IGrouping<Guid?, RequestResult> item in enumerable2)
		{
			string text = $"<p>&emsp;* Item no. [{item.First().Name}] # {item.First().Value} ({item.First().Upper}/{item.First().Lower}):</p>";
			foreach (RequestResult item2 in item)
			{
				text += $"<p>&emsp;&emsp;- Sample: {item2.Sample}, Cavity: {item2.Cavity}, Result: {item2.Result}, Tool: {item2.MachineName}, Inspector: {item2.StaffName}, Time: {item2.Modified}.</p>";
			}
			content += text;
		}
		return content;
	}

	private string GetMergedAddress(ExcelRange range)
	{
		if (range.Merge)
		{
			int mergeCellId = range.Worksheet.GetMergeCellId(range.Start.Row, range.Start.Column);
			return range.Worksheet.MergedCells[mergeCellId - 1];
		}
		return range.Address;
	}

	private int GetCellHeigh(ExcelWorksheet ws, int start, int end)
	{
		int num = 0;
		for (int i = start; i <= end; i++)
		{
			num += (int)(ws.Row(i).Height / 0.75);
		}
		return num;
	}

	private int GetCellWidth(ExcelWorksheet ws, int start, int end)
	{
		int num = 0;
		decimal maxFontWidth = ws.Workbook.MaxFontWidth;
		for (int i = start; i <= end; i++)
		{
			num += (int)decimal.Truncate((256m * (decimal)ws.Column(i).Width + decimal.Truncate(128m / maxFontWidth)) / 256m * maxFontWidth);
		}
		return num;
	}

	public async Task<ResponseDto> GetCpks(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<CpkViewModel> cpks = new List<CpkViewModel>();
			IEnumerable<RequestResult> source = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(id) && !string.IsNullOrEmpty(x.Result) && x.Measurement.ImportantId != null, "Measurement.Sort");
			for (int num = 0; num < source.Count() - 1; num++)
			{
				string value = Regex.Match(source.ElementAt(num).Name, "^.*(?=\\d)").Value;
				if (!value.ToUpper().Contains("LENGTH") && !value.ToUpper().Contains("WIDTH") && !value.ToUpper().Contains("THICKNESS"))
				{
					continue;
				}
				for (int num2 = num + 1; num2 < source.Count(); num2++)
				{
					string value2 = Regex.Match(source.ElementAt(num2).Name, "^.*(?=\\d)").Value;
					if (value == value2)
					{
						source.ElementAt(num).Name = value;
						source.ElementAt(num2).MeasurementId = source.ElementAt(num).MeasurementId;
						source.ElementAt(num2).Name = source.ElementAt(num).Name;
						source.ElementAt(num2).Value = source.ElementAt(num).Value;
						source.ElementAt(num2).Upper = source.ElementAt(num).Upper;
						source.ElementAt(num2).Lower = source.ElementAt(num).Lower;
					}
				}
			}
			IEnumerable<IGrouping<Guid?, RequestResult>> enumerable = from x in source
				group x by x.MeasurementId;
			foreach (IGrouping<Guid?, RequestResult> item in enumerable)
			{
				List<RequestResult> source2 = item.ToList();
				double.TryParse(source2.First().Value, out var result);
				double? upper = source2.First().Upper;
				double? lower = source2.First().Lower;
				double? num3 = result + upper;
				double? num4 = result + lower;
				double? num5 = (num3 + num4) / 2.0;
				double? num6 = (upper - lower) / 2.0;
				List<double> list = source2.Select((RequestResult x) => double.Parse(x.Result)).ToList();
				double num7 = list.Average();
				double num8 = mMath.CalStandardDeviation(list);
				double? obj = (num3 - num4) / (6.0 * num8);
				double? num9 = Math.Round(((1.0 - Math.Abs((num5 - num7).Value) / num6) * obj).Value, 4);
				string rank = "RANK1";
				if (num9 < 0.67)
				{
					rank = "RANK5";
				}
				else if (num9 <= 1.0)
				{
					rank = "RANK4";
				}
				else if (num9 <= 1.3)
				{
					rank = "RANK3";
				}
				else if (num9 < 1.7)
				{
					rank = "RANK2";
				}
				if (num9.Equals(double.NaN))
				{
					rank = string.Empty;
					num9 = null;
				}
				cpks.Add(new CpkViewModel
				{
					Id = item.Key,
					Name = source2.First().Name,
					Cpk = num9,
					Rank = rank
				});
			}
			res.Data = cpks;
			res.Count = cpks.Count();
		}
		catch
		{
			res.Count = 0;
		}
		return res;
	}

	private async Task SaveFileFinishAsync(Request request)
	{
		string cryptedString = _configuration["Key"];
		ConfigApp configApp = System.Text.Json.JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		if (!configApp.Funtion.Contains("QC2210"))
		{
			return;
		}
		MetadataValue cfg = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name == "DirFile" && x.IsActivated, "");
		if (cfg == null || string.IsNullOrEmpty(cfg.Value) || !Directory.Exists(cfg.Value))
		{
			return;
		}
		Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId);
		if (product == null || !product.TemplateId.HasValue)
		{
			return;
		}
		Template template = await _uow.GetRepository<Template>().FindByIdAsync(product.TemplateId.Value);
		if (template == null || string.IsNullOrEmpty(template.TemplateUrl))
		{
			return;
		}
		string text = Path.Combine(_hostingEnvironment.WebRootPath, "TemplateExcel");
		string text2 = Path.Combine(_hostingEnvironment.WebRootPath, "TempFileExport");
		Directory.CreateDirectory(text);
		Directory.CreateDirectory(text2);
		List<string> listFileName = new List<string>();
		RequestViewModel requestViewModel = new RequestViewModel();
		_mapper.Map(request, requestViewModel);
		List<string> list = listFileName;
		list.AddRange(await _template.ExportOneFile(requestViewModel, "Product", string.Empty, text, text2, isSample: false, template.Id, cfg.Description));
		if (listFileName.Count.Equals(0))
		{
			return;
		}
		text2 = Path.Combine(cfg.Value, product.Code.Trim());
		Directory.CreateDirectory(text2);
		if (listFileName.Count.Equals(1))
		{
			string destFileName = Path.Combine(text2, request.Name + Path.GetExtension(listFileName.First()));
			File.Copy(listFileName.First(), destFileName, overwrite: true);
		}
		else
		{
			string destFileName = Path.Combine(text2, request.Name + ".zip");
			using ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(destFileName));
			zipOutputStream.SetLevel(9);
			byte[] array = new byte[16384];
			foreach (string item in listFileName)
			{
				ZipEntry entry = new ZipEntry(Path.GetFileName(item))
				{
					DateTime = DateTime.Now,
					IsUnicodeText = true
				};
				zipOutputStream.PutNextEntry(entry);
				using FileStream fileStream = File.OpenRead(item);
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
		foreach (string item2 in listFileName)
		{
			if (File.Exists(item2))
			{
				File.Delete(item2);
			}
		}
	}

	private async Task<EmailDto> SendEmailDtoForChangeRequestStatus(Request request)
	{
		string cryptedString = _configuration["Key"];
		ConfigApp configApp = System.Text.Json.JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		if (!configApp.Funtion.Contains("QC2202"))
		{
			return null;
		}
		Email email = await _uow.GetRepository<Email>().GetSingleAsync((Email x) => x.Id != null, "");
		if (email == null || string.IsNullOrEmpty(email.Name) || string.IsNullOrEmpty(email.Password))
		{
			return null;
		}
		EmailDto emailDto = await CreatedToEmailForChangeRequestStatus(request.ProductId, request.Status);
		if (string.IsNullOrEmpty(emailDto.ToEmail))
		{
			return null;
		}
		EmailDto emailDto2 = new EmailDto();
		emailDto2.ToEmail = emailDto.ToEmail;
		emailDto2.CcEmail = (string.IsNullOrEmpty(emailDto.CcEmail) ? null : emailDto.CcEmail);
		emailDto2.Subject = "Request [" + request.Code + "][" + request.Name + "] need approval";
		EmailDto emailDto3 = emailDto2;
		string text = email.Header ?? "";
		text = text + "<p>Information of request [" + request.Code + "][" + request.Name + "]:</p>";
		text += $"<p>&emsp;* Date: {request.Date:dd/MMM/yyyy}.&emsp;* Status: {request.Status}.</p>";
		text += $"<p>&emsp;* Lot no.: {request.Lot}.&emsp;* Line: {request.Line}.&emsp;* Quantity: {request.Quantity}.</p>";
		text += $"<p>&emsp;* Intention: {request.Intention}.&emsp;* Sample: {request.Sample}.";
		text = text + "&emsp;* Judgement: <b style=\"color: " + (request.Judgement.Contains("OK") ? "blue" : "red") + ";\">" + request.Judgement + ".</b></p>";
		text += email.Footer;
		emailDto3.Content = text;
		return emailDto3;
	}

	private async Task<EmailDto> CreatedToEmailForChangeRequestStatus(Guid productid, string status)
	{
		EmailDto emailDto = new EmailDto();
		string posName = "cbCompleted";
		if (status == "Checked")
		{
			posName = "cbChecked";
		}
		Setting setting = await _uow.GetRepository<Setting>().GetSingleAsync((Setting x) => x.Group == "gbRequestStatus" && x.IsChecked && x.Name == posName, "");
		if (setting == null)
		{
			return emailDto;
		}
		ProductViewModel product = await _uow.GetRepository<Product>().FindByIdAsync<ProductViewModel>(productid);
		if (product == null)
		{
			return emailDto;
		}
		IEnumerable<AuthUserViewModel> enumerable = await _uow.GetRepository<AuthUser>().FindByAsync<AuthUserViewModel>((AuthUser x) => x.IsActivated && x.IsEmail != false && !string.IsNullOrEmpty(x.Email));
		string name = setting.Name;
		if (!(name == "cbChecked"))
		{
			if (name == "cbCompleted")
			{
				enumerable = enumerable.Where((AuthUserViewModel x) => x.Role == RoleWeb.Administrator);
			}
		}
		else
		{
			enumerable = enumerable.Where((AuthUserViewModel x) => x.Role == RoleWeb.SuperAdministrator);
		}
		List<AuthUserViewModel> list = new List<AuthUserViewModel>();
		foreach (AuthUserViewModel item in enumerable)
		{
			if ((!(item.JobTitle != "Supervisor") || !(item.JobTitle != "Manager") || !(item.DepartmentId != product.DepartmentId)) && product.DepartmentName.StartsWith(item.DepartmentName))
			{
				list.Add(item);
			}
		}
		foreach (AuthUserViewModel item2 in list)
		{
			if (string.IsNullOrEmpty(emailDto.ToEmail))
			{
				emailDto.ToEmail = item2.Email;
			}
			else if (!emailDto.ToEmail.Contains(item2.Email))
			{
				if (string.IsNullOrEmpty(emailDto.CcEmail))
				{
					emailDto.CcEmail += item2.Email;
				}
				else if (!emailDto.CcEmail.Contains(item2.Email))
				{
					emailDto.CcEmail = emailDto.CcEmail + ";" + item2.Email;
				}
			}
		}
		return emailDto;
	}
}
