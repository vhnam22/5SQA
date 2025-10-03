using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class MeasurementService : IMeasurementService
{
	private readonly IUnitOfWork _uow;

	public MeasurementService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> Gets(Guid productid, Guid planid)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<MeasurementPlanViewModel> anons = new List<MeasurementPlanViewModel>();
			foreach (MeasurementPlanViewModel meas in await _uow.GetRepository<Measurement>().FindByAsync<MeasurementPlanViewModel>((Measurement x) => x.ProductId.Equals(productid), "Sort, Created"))
			{
				PlanDetail planDetail = await _uow.GetRepository<PlanDetail>().GetSingleAsync((PlanDetail x) => ((object)x.MeasurementId).Equals((object?)meas.Id) && x.PlanId.Equals(planid), "");
				meas.IsSelect = ((planid.Equals(Guid.Empty) || planDetail != null) ? true : false);
				if (await _uow.GetRepository<PlanDetail>().GetSingleAsync((PlanDetail x) => ((object)x.MeasurementId).Equals((object?)meas.Id) && !x.PlanId.Equals(planid), "") == null)
				{
					anons.Add(meas);
				}
			}
			res.Data = anons;
			res.Count = anons.Count();
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
			responseDto.Data = await _uow.GetRepository<Measurement>().FindByAsync<MeasurementViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Measurement>().CountAsync(args.Predicate, args.PredicateParameters);
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
			Measurement att = await _uow.GetRepository<Measurement>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find measurement with id: {id}");
			}
			_uow.GetRepository<Measurement>().Delete(att);
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

	public async Task<ResponseDto> Save(MeasurementViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Measurement();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Measurement att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<Measurement>().GetSingleAsync((Measurement x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && ((object)x.ProductId).Equals((object?)model.ProductId), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = new Measurement();
				Mapper.Map(model, att);
				Measurement measurement = await _uow.GetRepository<Measurement>().GetSingleAsync((Measurement x) => ((object)x.ProductId).Equals((object?)model.ProductId), "Sort DESC, Created DESC");
				if (measurement != null)
				{
					att.Sort = measurement.Sort + 1;
				}
				_uow.GetRepository<Measurement>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<Measurement>().GetSingleAsync((Measurement x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && ((object)x.ProductId).Equals((object?)model.ProductId) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = await _uow.GetRepository<Measurement>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find measurement with id: {model.Id}");
				}
				model.Sort = att.Sort;
				Mapper.Map(model, att);
				_uow.GetRepository<Measurement>().Update(att);
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

	public async Task<ResponseDto> SaveList(IEnumerable<MeasurementViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (models == null)
			{
				throw new Exception("Models is null");
			}
			Product pro = await _uow.GetRepository<Product>().FindByIdAsync(models.First().ProductId.Value);
			if (pro == null)
			{
				throw new Exception($"Can't find product with id: {models.First().ProductId}");
			}
			int sort = -1;
			Measurement measurement = await _uow.GetRepository<Measurement>().GetSingleAsync((Measurement x) => x.ProductId.Equals(pro.Id), "Sort DESC, Created DESC");
			if (measurement != null)
			{
				sort = measurement.Sort;
			}
			List<Measurement> anons = new List<Measurement>();
			foreach (var item in models.Select((MeasurementViewModel Value, int Index) => new { Value, Index }))
			{
				MeasurementViewModel model = item.Value;
				int index = item.Index + 1;
				string Message = string.Empty;
				if (string.IsNullOrEmpty(model.Code))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Code is empty;" : " Code is empty;");
				}
				else if (!model.Code.StartsWith("MEAS-"))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Code incorrect;" : " Code incorrect;");
				}
				else
				{
					if (await _uow.GetRepository<Measurement>().GetSingleAsync((Measurement x) => x.Code.Equals(model.Code) && x.ProductId.Equals(pro.Id), "") != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
					Measurement measurement2 = anons.FirstOrDefault((Measurement x) => x.Code.Equals(model.Code));
					if (measurement2 != null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Code already exist;" : " Code already exist;");
					}
				}
				if (string.IsNullOrEmpty(model.Name))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Name is empty;" : " Name is empty;");
				}
				if (!string.IsNullOrEmpty(model.ImportantName))
				{
					MetadataValue metadataValue = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name.Equals(model.ImportantName) && x.TypeId.Equals(new Guid("6042BF53-9411-47D4-9BD6-F8AB7BABB663")), "");
					if (metadataValue == null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Can't find important with name: {model.ImportantName};" : (" Can't find important with name: " + model.ImportantName + ";"));
					}
					else
					{
						model.ImportantId = metadataValue.Id;
					}
				}
				double result;
				if (string.IsNullOrEmpty(model.Value))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Value is empty;" : " Value is empty;");
				}
				else if (double.TryParse(model.Value, out result))
				{
					if (string.IsNullOrEmpty(model.UnitName))
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is float, so unit can't empty;" : " Type value is float, so unit can't empty;");
					}
					else
					{
						MetadataValue metadataValue2 = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name.Equals(model.UnitName) && x.TypeId.Equals(new Guid("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D")), "");
						if (metadataValue2 == null)
						{
							Message += ((Message == string.Empty) ? $"No {index}: Can't find unit with name: {model.UnitName};" : (" Can't find unit with name: " + model.UnitName + ";"));
						}
						else
						{
							model.UnitId = metadataValue2.Id;
						}
					}
					if (!model.Upper.HasValue)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is float, so upper can't empty;" : " Type value is float, so upper can't empty;");
					}
					if (!model.Lower.HasValue)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is float, so lower can't empty;" : " Type value is float, so lower can't empty;");
					}
					if (!string.IsNullOrEmpty(model.Formula))
					{
						string text = checkFormula(model, models);
						if (!string.IsNullOrEmpty(text) && text.Contains("Error: "))
						{
							Message += ((Message == string.Empty) ? $"No {index}: Formula incorrect;" : " Formula incorrect;");
						}
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(model.ImportantName))
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is string, so important is empty;" : " Type value is string, so important is empty;");
					}
					if (!string.IsNullOrEmpty(model.UnitName))
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is string, so unit is empty;" : " Type value is string, so unit is empty;");
					}
					if (model.Upper.HasValue)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is string, so upper is empty;" : " Type value is string, so upper is empty;");
					}
					if (model.Lower.HasValue)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is string, so lower is empty;" : " Type value is string, so lower is empty;");
					}
					if (!string.IsNullOrEmpty(model.Formula))
					{
						Message += ((Message == string.Empty) ? $"No {index}: Type value is string, so formula is empty;" : " Type value is string, so formula is empty;");
					}
				}
				if (string.IsNullOrEmpty(model.MachineTypeName))
				{
					Message += ((Message == string.Empty) ? $"No {index}: Machine type is empty;" : " Machine type is empty;");
				}
				else
				{
					MetadataValue metadataValue3 = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Name.Equals(model.MachineTypeName) && x.TypeId.Equals(new Guid("438D7052-25F3-4342-ED0C-08D7E9C5C77D")), "");
					if (metadataValue3 == null)
					{
						Message += ((Message == string.Empty) ? $"No {index}: Can't find machine type with name: {model.MachineTypeName};" : (" Can't find machine type with name: " + model.MachineTypeName + ";"));
					}
					else
					{
						model.MachineTypeId = metadataValue3.Id;
					}
				}
				model.Sort = sort + index;
				anons.Add(Mapper.Map<Measurement>(model));
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
			_uow.GetRepository<Measurement>().Add(anons);
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

	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (await _uow.GetRepository<Measurement>().FindByIdAsync(idfrom) == null)
			{
				throw new Exception($"Can't find measurement with idform: {idfrom}");
			}
			Measurement measfrom = await _uow.GetRepository<Measurement>().FindByIdAsync(idfrom);
			Measurement measurement = await _uow.GetRepository<Measurement>().FindByIdAsync(idto);
			if (measfrom == null || measurement == null)
			{
				throw new Exception($"Can't find measurement with idform: {idfrom} or idto: {idto}");
			}
			_ = string.Empty;
			string allAsync = ((measfrom.Sort <= measurement.Sort) ? $"Update Measurements Set Sort = Sort - 1 Where Sort > {measfrom.Sort} And Sort <= {measurement.Sort} And ProductId = '{measfrom.ProductId}'" : $"Update Measurements Set sort = Sort + 1 Where Sort < {measfrom.Sort} And Sort >= {measurement.Sort} And ProductId = '{measfrom.ProductId}'");
			_uow.GetRepository<Measurement>().SetAllAsync(allAsync);
			measfrom.Sort = measurement.Sort;
			_uow.GetRepository<Measurement>().Update(measfrom);
			await _uow.CommitAsync();
			res.Data = Mapper.Map<MeasurementViewModel>(measfrom);
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

	public async Task<ResponseDto> Gets(Guid id, IEnumerable<string> lstmachinetype)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			IEnumerable<ResultFullViewModel> enumerable = await _uow.GetRepository<Measurement>().FindByAsync<ResultFullViewModel>((Measurement x) => x.ProductId.Equals(id), "Sort, Created");
			if (lstmachinetype != null && lstmachinetype.Count() > 0)
			{
				enumerable = enumerable.Where((ResultFullViewModel x) => lstmachinetype.Any((string y) => y.Equals(x.MachineTypeName)));
			}
			res.Data = enumerable;
			res.Count = enumerable.Count();
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

	public async Task<ResponseDto> SaveCoordinate(MeasurementDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			if (model.Id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Measurement att = await _uow.GetRepository<Measurement>().FindByIdAsync(model.Id);
			if (att == null)
			{
				throw new Exception($"Can't find measurement with id: {model.Id}");
			}
			att.Coordinate = model.Coordinate;
			_uow.GetRepository<Measurement>().Update(att);
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

	private string checkFormula(MeasurementViewModel model, IEnumerable<MeasurementViewModel> models)
	{
		string text = string.Empty;
		string text2 = model.Formula.Replace(Environment.NewLine, "").Replace("\n", "").Trim();
		if (!string.IsNullOrEmpty(text2))
		{
			List<string> list = (from x in text2.Split(';')
				where !x.Trim().Equals("")
				select x).ToList();
			if (list.Count != 0)
			{
				foreach (string item in list)
				{
					if (item.Equals("[UnAbs]"))
					{
						text += "[RESULT] = 0; ";
						continue;
					}
					List<string> list2 = (from x in item.Split('=')
						where !x.Trim().Equals("")
						select x).ToList();
					if (list2.Count().Equals(2))
					{
						IEnumerable<string> enumerable = from x in list2[1].Split('[')
							select (x.Split(']').Length == 0) ? "" : x.Split(']')[0] into x
							where !x.Trim().Equals("") && !x.ToLower().Contains("system.math")
							select x;
						foreach (string item2 in enumerable)
						{
							string parameterAsync = getParameterAsync(item2.Trim(), models, model.Value);
							if (string.IsNullOrEmpty(parameterAsync))
							{
								break;
							}
							list2[1] = list2[1].Replace("[" + item2 + "]", parameterAsync ?? "");
						}
						try
						{
							Task<object> task = CSharpScript.EvaluateAsync(list2[1]);
							string text3 = list2[0].Trim();
							text = ((!text3.Equals("[RESULT]") && !text3.Equals("[VALUE]") && !text3.Equals("[UPPER]") && !text3.Equals("[LOWER]") && !text3.Equals("[WARNUPPER]") && !text3.Equals("[WARNLOWER]")) ? (text + "Error: Return type result different [RESULT], [VALUE], [UPPER], [LOWER], [WARNUPPER], [WARNLOWER]; ") : (text + $"{text3} = {task.Result}; "));
						}
						catch (Exception ex)
						{
							text = text + "Error: " + ex.Message + "; ";
						}
					}
					else
					{
						text += "Error: Command format incorrect (ex: [UPPER]=Formula); ";
					}
				}
			}
			else
			{
				text += "0";
			}
		}
		else
		{
			text += "0";
		}
		return text;
	}

	private string getParameterAsync(string name, IEnumerable<MeasurementViewModel> models, string value)
	{
		string[] lstname = name.Split('#');
		string result = string.Empty;
		if (lstname.Length > 1)
		{
			MeasurementViewModel[] array = models.Where((MeasurementViewModel r) => r.Code.Equals(lstname[0])).ToArray();
			if (array.Count() > 0)
			{
				switch (lstname[1])
				{
				case "RESULT":
					result = array[0].Value;
					break;
				case "VALUE":
					result = array[0].Value;
					break;
				case "UPPER":
					result = array[0].Upper.ToString();
					break;
				case "LOWER":
					result = array[0].Lower.ToString();
					break;
				}
			}
		}
		else
		{
			switch (lstname[0])
			{
			case "RESULT":
				result = value;
				break;
			case "VALUE":
				result = value;
				break;
			case "UPPER":
				result = value;
				break;
			case "LOWER":
				result = value;
				break;
			}
		}
		return result;
	}
}
