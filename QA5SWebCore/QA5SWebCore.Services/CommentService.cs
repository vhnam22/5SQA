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
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class CommentService : ICommentService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _hostingEnvironment;

	public CommentService(IUnitOfWork uow, IWebHostEnvironment hostingEnvironment)
	{
		_uow = uow;
		_hostingEnvironment = hostingEnvironment;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Comment>().FindByAsync<CommentViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Comment>().CountAsync(args.Predicate, args.PredicateParameters);
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
			Comment att = await _uow.GetRepository<Comment>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find comment with id: {id}");
			}
			_uow.GetRepository<Comment>().Delete(att);
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

	public async Task<ResponseDto> Save(CommentViewModel model)
	{
		ResponseDto res = new ResponseDto();
		Comment att = new Comment();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			if (model.Id.Equals(Guid.Empty))
			{
				Mapper.Map(model, att);
				_uow.GetRepository<Comment>().Add(att);
			}
			else
			{
				att = await _uow.GetRepository<Comment>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find comment with id: {model.Id}");
				}
				model.RequestId = att.RequestId;
				Mapper.Map(model, att);
				_uow.GetRepository<Comment>().Update(att);
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
			Comment att = (await _uow.GetRepository<Comment>().FindByIdAsync(id)) ?? throw new Exception($"Can't find comment with id: {id}");
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "CommentFiles");
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
		string sourcePath = Path.Combine(_hostingEnvironment.WebRootPath, "CommentFiles");
		Directory.CreateDirectory(sourcePath);
		Comment comment = (await _uow.GetRepository<Comment>().FindByIdAsync(id)) ?? throw new Exception($"Can't find comment with id: {id}");
		if (string.IsNullOrEmpty(comment.Link))
		{
			throw new Exception("Comment hasn't file");
		}
		string path = Path.Combine(sourcePath, comment.Link);
		byte[] fileContents = File.ReadAllBytes(path);
		return new CustomerFile(fileContents, comment.Link);
	}
}
