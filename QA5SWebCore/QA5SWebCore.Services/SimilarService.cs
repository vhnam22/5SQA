using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class SimilarService : ISimilarService
{
	private readonly IUnitOfWork _uow;

	public SimilarService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> Gets(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<SimilarViewModel> similars = new List<SimilarViewModel>();
			Similar att = await _uow.GetRepository<Similar>().GetSingleAsync((Similar x) => x.Value.Contains(((object)id).ToString()), "");
			if (att != null && !string.IsNullOrEmpty(att.Value))
			{
				string[] array = att.Value.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (!(text == id.ToString()))
					{
						Product product = await _uow.GetRepository<Product>().FindByIdAsync(Guid.Parse(text));
						if (product != null)
						{
							similars.Add(new SimilarViewModel
							{
								Id = att.Id,
								Created = att.Created,
								CreatedBy = att.CreatedBy,
								Modified = att.Modified,
								ModifiedBy = att.ModifiedBy,
								IsActivated = att.IsActivated,
								Value = att.Value,
								ProductId = product.Id,
								ProductCode = product.Code,
								ProductName = product.Name
							});
						}
					}
				}
			}
			res.Data = similars;
			res.Count = similars.Count;
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

	public async Task<ResponseDto> Save(SimilarViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Similar();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			if (await _uow.GetRepository<Similar>().GetSingleAsync((Similar x) => x.Value.Contains(((object)model.ProductId).ToString()), "") != null)
			{
				throw new Exception("This product was created similarly");
			}
			Similar att = await _uow.GetRepository<Similar>().GetSingleAsync((Similar x) => x.Value.Contains(((object)model.Id).ToString()), "");
			if (att == null)
			{
				string value = $"{model.Id};{model.ProductId};";
				att = new Similar
				{
					Value = value
				};
				_uow.GetRepository<Similar>().Add(att);
			}
			else
			{
				string text = att.Value;
				if (!text.Contains($"{model.ProductId};"))
				{
					text += $"{model.ProductId};";
				}
				att.Value = text;
				_uow.GetRepository<Similar>().Update(att);
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

	public async Task<ResponseDto> Delete(SimilarViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Similar();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Similar att = await _uow.GetRepository<Similar>().GetSingleAsync((Similar x) => x.Value.Contains(((object)model.Id).ToString()), "");
			if (att == null)
			{
				throw new Exception($"Can't find similar with id: {model.Id}");
			}
			string text = att.Value.Replace($"{model.ProductId};", "");
			if (text == $"{model.Id};")
			{
				text = null;
			}
			if (string.IsNullOrEmpty(text))
			{
				_uow.GetRepository<Similar>().Delete(att);
			}
			else
			{
				att.Value = text;
				_uow.GetRepository<Similar>().Update(att);
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
}
