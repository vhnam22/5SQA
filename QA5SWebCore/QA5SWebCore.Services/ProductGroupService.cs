using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class ProductGroupService : IProductGroupService
{
	private readonly IUnitOfWork _uow;

	private readonly IWebHostEnvironment _host;

	private readonly IMapper _mapper;

	public ProductGroupService(IUnitOfWork uow, IWebHostEnvironment host, IMapper mapper)
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
			responseDto.Data = await _uow.GetRepository<ProductGroup>().FindByAsync<ProductGroupViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<ProductGroup>().CountAsync(args.Predicate, args.PredicateParameters);
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
			ProductGroup att = (await _uow.GetRepository<ProductGroup>().FindByIdAsync(id)) ?? throw new Exception($"Can't find product group with id: {id}");
			IEnumerable<Product> products = await _uow.GetRepository<Product>().FindByAsync((Product x) => ((object)x.GroupId).Equals((object?)id), "");
			foreach (Product product in products)
			{
				IEnumerable<Measurement> entities = await _uow.GetRepository<Measurement>().FindByAsync((Measurement x) => x.ProductId.Equals(product.Id), "");
				_uow.GetRepository<Measurement>().Delete(entities);
			}
			_uow.GetRepository<Product>().Delete(products);
			_uow.GetRepository<ProductGroup>().Delete(att);
			await _uow.CommitAsync();
			foreach (Product item in products)
			{
				if (!string.IsNullOrEmpty(item.ImageUrl))
				{
					string text = Path.Combine(_host.WebRootPath, "ProductImage");
					Directory.CreateDirectory(text);
					string path = Path.Combine(text, item.ImageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
			}
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

	public async Task<ResponseDto> Save(ProductGroupViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new ProductGroup();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			ProductGroup att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<ProductGroup>().GetSingleAsync((ProductGroup x) => x.Code.Equals(model.Code), "") != null)
				{
					throw new Exception("Code already exist");
				}
				att = new ProductGroup();
				_mapper.Map(model, att);
				_uow.GetRepository<ProductGroup>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<ProductGroup>().GetSingleAsync((ProductGroup x) => x.Code.Equals(model.Code) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Code already exist");
				}
				att = (await _uow.GetRepository<ProductGroup>().FindByIdAsync(model.Id)) ?? throw new Exception($"Can't find product group with id: {model.Id}");
				_mapper.Map(model, att);
				_uow.GetRepository<ProductGroup>().Update(att);
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

	public async Task<ResponseDto> Copy(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			ProductGroup att = (await _uow.GetRepository<ProductGroup>().FindByIdAsync(id)) ?? throw new Exception($"Can't find product group with id: {id}");
			att.Code += "_copy";
			_uow.GetRepository<ProductGroup>().Add(att);
			foreach (Product pro in await _uow.GetRepository<Product>().FindByAsync((Product x) => ((object)x.GroupId).Equals((object?)id), "Sort, Created"))
			{
				Guid proId = pro.Id;
				pro.GroupId = att.Id;
				_uow.GetRepository<Product>().Add(pro);
				foreach (Measurement item in await _uow.GetRepository<Measurement>().FindByAsync((Measurement x) => x.ProductId.Equals(proId), "Sort, Created"))
				{
					item.ProductId = pro.Id;
					_uow.GetRepository<Measurement>().Add(item);
				}
				foreach (TemplateOther item2 in await _uow.GetRepository<TemplateOther>().FindByAsync((TemplateOther x) => x.ProductId.Equals(proId), ""))
				{
					item2.ProductId = pro.Id;
					_uow.GetRepository<TemplateOther>().Add(item2);
				}
				foreach (AQL item3 in await _uow.GetRepository<AQL>().FindByAsync((AQL x) => x.ProductId.Equals(proId), ""))
				{
					item3.ProductId = pro.Id;
					_uow.GetRepository<AQL>().Add(item3);
				}
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
