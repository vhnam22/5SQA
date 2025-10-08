using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class CalibrationService : ICalibrationService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _hostingEnvironment;

	private readonly IConfiguration _configuration;

	private readonly IMapper _mapper;

	public CalibrationService(IUnitOfWork uow, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IMapper mapper)
	{
		_uow = uow;
		_hostingEnvironment = hostingEnvironment;
		_configuration = configuration;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Calibration>().FindByAsync<CalibrationViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Calibration>().CountAsync(args.Predicate, args.PredicateParameters);
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
			Calibration att = await _uow.GetRepository<Calibration>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find calibration with id: {id}");
			}
			_uow.GetRepository<Calibration>().Delete(att);
			await _uow.CommitAsync();
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "CalibrationFile");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.File))
			{
				string path = Path.Combine(text, att.File);
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

	public async Task<ResponseDto> Save(CalibrationViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Calibration();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			string cryptedString = _configuration["Key"];
			ConfigApp configApp = JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
			if (!configApp.Funtion.Contains("QC2209"))
			{
				throw new Exception("Unlicensed");
			}
			Calibration att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<Calibration>().GetSingleAsync((Calibration x) => x.CalibrationNo.Equals(model.CalibrationNo) && ((object)x.MachineId).Equals((object?)model.MachineId), "") != null)
				{
					throw new Exception("Calibration no. already exist");
				}
				att = new Calibration();
				_mapper.Map(model, att);
				_uow.GetRepository<Calibration>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<Calibration>().GetSingleAsync((Calibration x) => x.CalibrationNo.Equals(model.CalibrationNo) && ((object)x.MachineId).Equals((object?)model.MachineId) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Calibration no. already exist");
				}
				att = await _uow.GetRepository<Calibration>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find calibration with id: {model.Id}");
				}
				_mapper.Map(model, att);
				_uow.GetRepository<Calibration>().Update(att);
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

	public async Task<ResponseDto> UpdateFile(Guid id, IFormFile file)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Calibration att = await _uow.GetRepository<Calibration>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find calibration with id: {id}");
			}
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "CalibrationFile");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.File))
			{
				string path = Path.Combine(text, att.File);
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
				att.File = fileUniqueName;
			}
			else
			{
				att.File = null;
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

	public async Task<CustomerFile> DownloadFile(Guid id)
	{
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "CalibrationFile");
		Directory.CreateDirectory(sourcePath);
		Calibration calibration = (await _uow.GetRepository<Calibration>().FindByIdAsync(id)) ?? throw new Exception($"Can't find calibration with id: {id}");
		if (string.IsNullOrEmpty(calibration.File))
		{
			throw new Exception("Calibration hasn't file");
		}
		string path = Path.Combine(sourcePath, calibration.File);
		byte[] fileContents = File.ReadAllBytes(path);
		return new CustomerFile(fileContents, calibration.File);
	}
}
