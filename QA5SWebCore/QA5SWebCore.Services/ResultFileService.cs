using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class ResultFileService : IResultFileService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _host;

	private readonly IMapper _mapper;

	public ResultFileService(IUnitOfWork uow, IWebHostEnvironment host, IMapper mapper)
	{
		_uow = uow;
		_host = host;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<ResultFile>().FindByAsync<ResultFileViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<ResultFile>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Save(ResultFileViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new ResultFile();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			ResultFile att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<ResultFile>().GetSingleAsync((ResultFile x) => ((object)x.RequestId).Equals((object?)model.RequestId), "") != null)
				{
					throw new Exception("File result already exist");
				}
				att = new ResultFile();
				_mapper.Map(model, att);
				_uow.GetRepository<ResultFile>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<ResultFile>().GetSingleAsync((ResultFile x) => ((object)x.RequestId).Equals((object?)model.RequestId) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("File result already exist");
				}
				att = await _uow.GetRepository<ResultFile>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find file result with id: {model.Id}");
				}
				_mapper.Map(model, att);
				_uow.GetRepository<ResultFile>().Update(att);
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

	public async Task<ResponseDto> Delete(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			ResultFile att = await _uow.GetRepository<ResultFile>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find file result with id: {id}");
			}
			_uow.GetRepository<ResultFile>().Delete(att);
			await _uow.CommitAsync();
			string text = Path.Combine(_host.WebRootPath, "ResultFile");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.Link))
			{
				string path = Path.Combine(text, att.Link);
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

	public async Task<ResponseDto> UploadFile(Guid id, IFormFile file)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			ResultFile att = await _uow.GetRepository<ResultFile>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find file result with id: {id}");
			}
			string text = Path.Combine(_host.WebRootPath, "ResultFile");
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
				string fileUniqueName = Path.GetFileName(file.FileName) ?? "";
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
}
