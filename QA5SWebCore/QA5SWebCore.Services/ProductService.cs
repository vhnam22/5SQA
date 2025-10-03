using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

public class ProductService : IProductService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _hostingEnvironment;

	public ProductService(IUnitOfWork uow, IWebHostEnvironment hostingEnvironment)
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
			responseDto.Data = await _uow.GetRepository<Product>().FindByAsync<ProductViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Product>().CountAsync(args.Predicate, args.PredicateParameters);
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
			Product att = (await _uow.GetRepository<Product>().FindByIdAsync(id)) ?? throw new Exception($"Can't find product with id: {id}");
			IEnumerable<Measurement> entities = await _uow.GetRepository<Measurement>().FindByAsync((Measurement x) => x.ProductId.Equals(id), "");
			_uow.GetRepository<Measurement>().Delete(entities);
			_uow.GetRepository<Product>().Delete(att);
			await _uow.CommitAsync();
			if (!(await _uow.GetRepository<Product>().AnyAsync((Product x) => !string.IsNullOrEmpty(x.ImageUrl) && x.ImageUrl == att.ImageUrl)))
			{
				string text = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
				Directory.CreateDirectory(text);
				if (!string.IsNullOrEmpty(att.ImageUrl))
				{
					string path = Path.Combine(text, att.ImageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
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

	public async Task<ResponseDto> Save(ProductViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Product();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Product att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<Product>().GetSingleAsync((Product x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && ((object)x.GroupId).Equals((object?)model.GroupId), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = new Product();
				Mapper.Map(model, att);
				Product product = await _uow.GetRepository<Product>().GetSingleAsync((Product x) => ((object)x.GroupId).Equals((object?)model.GroupId), "Sort DESC, Created DESC");
				if (product != null)
				{
					att.Sort = product.Sort + 1;
				}
				_uow.GetRepository<Product>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<Product>().GetSingleAsync((Product x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && ((object)x.GroupId).Equals((object?)model.GroupId) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = await _uow.GetRepository<Product>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find product with id: {model.Id}");
				}
				model.Sort = att.Sort;
				Mapper.Map(model, att);
				_uow.GetRepository<Product>().Update(att);
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

	public async Task<ResponseDto> SaveList(IEnumerable<ProductViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (models == null)
			{
				throw new Exception("Models is null");
			}
			List<Product> anons = new List<Product>();
			foreach (var item in models.Select((ProductViewModel Value, int Index) => new { Value, Index }))
			{
				ProductViewModel model = item.Value;
				int index = item.Index + 1;
				string Message = string.Empty;
				if (string.IsNullOrEmpty(model.Code))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Code is empty;" : " Code is empty;");
				}
				else
				{
					if (await _uow.GetRepository<Product>().GetSingleAsync((Product x) => x.Code.Equals(model.Code), "") != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
					Product product = anons.FirstOrDefault((Product x) => x.Code.Equals(model.Code));
					if (product != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
				}
				if (string.IsNullOrEmpty(model.Name))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Name is empty;" : " Name is empty;");
				}
				if (!model.SampleMax.HasValue)
				{
					Message += ((Message == string.Empty) ? $"No {index}: Maximum sample is empty;" : " Maximum sample is empty;");
				}
				else
				{
					model.SampleMax = (model.SampleMax.Equals(0) ? new int?(1) : model.SampleMax);
				}
				if (!model.Cavity.HasValue)
				{
					Message += ((Message == string.Empty) ? $"No {index}: Cavity quantity is empty;" : " Cavity quantity is empty;");
				}
				else
				{
					model.Cavity = (model.Cavity.Equals(0) ? new int?(1) : model.Cavity);
				}
				anons.Add(Mapper.Map<Product>(model));
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
			_uow.GetRepository<Product>().Add(anons);
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

	public async Task<ResponseDto> UpdateImage(Guid id, IFormFile file)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Product att = await _uow.GetRepository<Product>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find product with id: {id}");
			}
			string text = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
			Directory.CreateDirectory(text);
			if (!string.IsNullOrEmpty(att.ImageUrl))
			{
				string path = Path.Combine(text, att.ImageUrl);
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
				att.ImageUrl = fileUniqueName;
			}
			else
			{
				att.ImageUrl = null;
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

	public async Task<ResponseDto> Copy(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Product att = (await _uow.GetRepository<Product>().FindByIdAsync(id)) ?? throw new Exception($"Can't find product with id: {id}");
			att.Code += "_copy";
			_uow.GetRepository<Product>().Add(att);
			foreach (Measurement item in await _uow.GetRepository<Measurement>().FindByAsync((Measurement x) => x.ProductId.Equals(id), "Sort, Created"))
			{
				item.ProductId = att.Id;
				_uow.GetRepository<Measurement>().Add(item);
			}
			foreach (TemplateOther item2 in await _uow.GetRepository<TemplateOther>().FindByAsync((TemplateOther x) => x.ProductId.Equals(id), ""))
			{
				item2.ProductId = att.Id;
				_uow.GetRepository<TemplateOther>().Add(item2);
			}
			foreach (AQL item3 in await _uow.GetRepository<AQL>().FindByAsync((AQL x) => x.ProductId.Equals(id), ""))
			{
				item3.ProductId = att.Id;
				_uow.GetRepository<AQL>().Add(item3);
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

	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (await _uow.GetRepository<Product>().FindByIdAsync(idfrom) == null)
			{
				throw new Exception($"Can't find product with idform: {idfrom}");
			}
			Product measfrom = await _uow.GetRepository<Product>().FindByIdAsync(idfrom);
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(idto);
			if (measfrom == null || product == null)
			{
				throw new Exception($"Can't find product with idform: {idfrom} or idto: {idto}");
			}
			_ = string.Empty;
			string allAsync = ((measfrom.Sort <= product.Sort) ? $"Update Products Set Sort = Sort - 1 Where Sort > {measfrom.Sort} And Sort <= {product.Sort} And GroupId = '{measfrom.GroupId}'" : $"Update Products Set sort = Sort + 1 Where Sort < {measfrom.Sort} And Sort >= {product.Sort} And GroupId = '{measfrom.GroupId}'");
			_uow.GetRepository<Product>().SetAllAsync(allAsync);
			measfrom.Sort = product.Sort;
			_uow.GetRepository<Product>().Update(measfrom);
			await _uow.CommitAsync();
			res.Data = Mapper.Map<ProductViewModel>(measfrom);
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
}
