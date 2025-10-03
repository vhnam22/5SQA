using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.Extensions.Configuration;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class RequestResultController : ControllerBase
{
	private readonly IRequestResultService _result;

	private readonly IUnitOfWork _uow;

	private int digit;

	public RequestResultController(IRequestResultService result, IUnitOfWork uow, IConfiguration configuration)
	{
		_result = result;
		_uow = uow;
		string text = configuration["Round"];
		digit = (string.IsNullOrEmpty(text) ? 4 : (int.TryParse(text, out var result2) ? result2 : 4));
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> Gets(Guid id, [FromBody] IEnumerable<string> lstmachinetype)
	{
		return await _result.Gets(id, lstmachinetype);
	}

	[HttpPost("{id}/{sample}")]
	public async Task<ResponseDto> Gets(Guid id, int sample, [FromBody] IEnumerable<string> lstmachinetype)
	{
		return await _result.Gets(id, sample, lstmachinetype);
	}

	[HttpGet("{id}/{idmeas}")]
	public async Task<ResponseDto> GetForMeass(Guid id, Guid idmeas)
	{
		return await _result.Gets(id, idmeas);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _result.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] RequestResultViewModel model)
	{
		ResponseDto res = new ResponseDto();
		if (model == null)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = "Model is null"
			});
			return res;
		}
		double result3;
		if (!string.IsNullOrEmpty(model.Result))
		{
			model.Result = model.Result.TrimEnd('.');
			RequestResultViewModel requestResultViewModel = model;
			string result2;
			if (!double.TryParse(model.Result, out var result))
			{
				result2 = model.Result;
			}
			else
			{
				result3 = Math.Round(result, digit, MidpointRounding.AwayFromZero);
				result2 = result3.ToString();
			}
			requestResultViewModel.Result = result2;
		}
		Request request = await _uow.GetRepository<Request>().FindByIdAsync(model.RequestId.Value);
		if (request == null)
		{
			res.Messages.Add(new ResponseMessage
			{
				Message = $"Can't find request with id: {model.RequestId}"
			});
			return res;
		}
		FormulaViewModel modelorigin = await _uow.GetRepository<Measurement>().FindByIdAsync<FormulaViewModel>(model.MeasurementId.Value);
		if (modelorigin == null)
		{
			res.Messages.Add(new ResponseMessage
			{
				Message = $"Can't find measurement with id: {model.MeasurementId}"
			});
			return res;
		}
		if (!string.IsNullOrEmpty(model.Result) && !string.IsNullOrEmpty(modelorigin.Unit) && !double.TryParse(model.Result, out result3))
		{
			res.Messages.Add(new ResponseMessage
			{
				Message = "The result format is incorrect"
			});
			return res;
		}
		IEnumerable<FormulaViewModel> lstmodel = await _uow.GetRepository<Measurement>().FindByAsync<FormulaViewModel>((Measurement x) => x.ProductId.Equals(request.ProductId) && !string.IsNullOrEmpty(x.Formula));
		List<FormulaViewModel> lstmodel2 = new List<FormulaViewModel> { modelorigin };
		lstmodel2.AddRange(relationFormula(lstmodel, modelorigin.Code).Distinct());
		foreach (FormulaViewModel meas in lstmodel2)
		{
			meas.Id = Guid.Empty;
			meas.MachineName = "Formuler";
			meas.StaffName = model.StaffName;
			meas.Modified = DateTimeOffset.Now;
			meas.Sample = model.Sample;
			meas.Cavity = model.Cavity;
			FormulaViewModel formulaViewModel = await _uow.GetRepository<RequestResult>().GetSingleAsync<FormulaViewModel>((RequestResult x) => ((object)x.MeasurementId).Equals((object?)meas.MeasurementId) && x.RequestId.Equals(request.Id) && x.Sample.Equals(model.Sample) && x.Cavity.Equals(model.Cavity));
			if (formulaViewModel != null)
			{
				meas.Result = formulaViewModel.Result;
				meas.ResultOld = formulaViewModel.ResultOld;
				meas.ResultOrigin = formulaViewModel.ResultOrigin;
				meas.Value = formulaViewModel.Value;
				meas.ValueOld = formulaViewModel.ValueOld;
				meas.UpperOld = formulaViewModel.UpperOld;
				meas.Upper = formulaViewModel.Upper;
				meas.LowerOld = formulaViewModel.LowerOld;
				meas.Lower = formulaViewModel.Lower;
				meas.Id = formulaViewModel.Id;
				meas.MachineName = formulaViewModel.MachineName;
				meas.MachineNameOld = formulaViewModel.MachineNameOld;
				meas.StaffName = formulaViewModel.StaffName;
				meas.History = formulaViewModel.History;
				meas.Modified = formulaViewModel.Modified;
			}
			if (!meas.MeasurementId.Equals(model.MeasurementId))
			{
				continue;
			}
			if (model.MachineName.Contains("Manual Input"))
			{
				if (string.IsNullOrEmpty(model.Result))
				{
					if (!string.IsNullOrEmpty(meas.ResultOld))
					{
						meas.ResultOrigin = model.Result;
					}
				}
				else if (!model.Result.Equals(meas.ResultOld))
				{
					meas.ResultOrigin = model.Result;
				}
			}
			else
			{
				meas.ResultOrigin = model.Result;
			}
			meas.MachineName = model.MachineName;
			meas.StaffName = model.StaffName;
		}
		lstmodel2 = await getListFormula(lstmodel2, request);
		return await _result.Save(lstmodel2, model, digit);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<RequestResultViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		if (models == null)
		{
			res.Messages.Add(new ResponseMessage
			{
				Message = "Models is null"
			});
			return res;
		}
		List<object> anons = new List<object>();
		foreach (RequestResultViewModel model in models)
		{
			anons.Add((await Save(model)).Data);
		}
		res.Data = anons;
		return res;
	}

	[HttpGet("{idrequest}/{idplan}")]
	public async Task<ResponseDto> GetSampleHasResults(Guid idrequest, Guid idplan)
	{
		return await _result.GetSampleHasResults(idrequest, idplan);
	}

	[HttpPost]
	public async Task<ResponseDto> GetsForChart([FromBody] ResultForChartDto dto)
	{
		return await _result.GetsForChart(dto);
	}

	[HttpPost]
	public async Task<ResponseDto> GetForStatistics([FromBody] StatisticDetailDto dto)
	{
		return await _result.GetForStatistics(dto);
	}

	private async Task<List<FormulaViewModel>> getListFormula(List<FormulaViewModel> lstmodel, Request request)
	{
		int digit = 4;
		foreach (FormulaViewModel _model in lstmodel)
		{
			string tempResultOrigin = _model.ResultOrigin;
			if (_model.ResultOrigin != null)
			{
				_model.ResultOrigin = _model.ResultOrigin.Replace("-", "");
			}
			FormulaViewModel att = new FormulaViewModel
			{
				Value = _model.ValueOrigin,
				Upper = _model.UpperOrigin,
				Lower = _model.LowerOrigin,
				Result = _model.ResultOrigin
			};
			if (!string.IsNullOrEmpty(_model.Formula) && !string.IsNullOrEmpty(_model.Unit))
			{
				List<string> list = (from x in _model.Formula.Split(';')
					where !x.Trim().Equals("")
					select x).ToList();
				if (!list.Count.Equals(0))
				{
					foreach (string item in list)
					{
						if (item.Contains("[UnAbs]"))
						{
							att.Result = tempResultOrigin;
							_model.ResultOrigin = tempResultOrigin;
							_model.IsChangeByFormula = true;
							continue;
						}
						List<string> formula = (from x in item.Split('=')
							where !x.Trim().Equals("")
							select x).ToList();
						if (!formula.Count.Equals(2))
						{
							continue;
						}
						string calformula = await calFormula(lstmodel, _model, request, formula[1]);
						IEnumerable<string> enumerable = from x in calformula.Split('[')
							select (x.Split(']').Length == 0) ? "" : x.Split(']')[0] into x
							where !string.IsNullOrEmpty(x.Trim()) && !x.Contains("(")
							select x;
						foreach (string _formula in enumerable)
						{
							string text = await getParameter(_model, lstmodel, request, _formula);
							if (!string.IsNullOrEmpty(text))
							{
								calformula = calformula.Replace("[" + _formula + "]", text ?? "");
								continue;
							}
							break;
						}
						try
						{
							object obj = await CSharpScript.EvaluateAsync(calformula);
							switch (formula[0].Trim())
							{
							case "[VALUE]":
								att.Value = Math.Round(double.Parse(obj.ToString()), digit, MidpointRounding.AwayFromZero).ToString();
								break;
							case "[UPPER]":
								att.Upper = Math.Round(double.Parse(obj.ToString()), digit, MidpointRounding.AwayFromZero);
								break;
							case "[LOWER]":
								att.Lower = Math.Round(double.Parse(obj.ToString()), digit, MidpointRounding.AwayFromZero);
								break;
							case "[RESULT]":
								att.Result = Math.Round(double.Parse(obj.ToString()), digit, MidpointRounding.AwayFromZero).ToString();
								_model.IsChangeByFormula = true;
								break;
							case "[WARNUPPER]":
								_model.WarnUpper = Math.Round(double.Parse(obj.ToString()), digit, MidpointRounding.AwayFromZero);
								break;
							case "[WARNLOWER]":
								_model.WarnLower = Math.Round(double.Parse(obj.ToString()), digit, MidpointRounding.AwayFromZero);
								break;
							}
						}
						catch
						{
							switch (formula[0].Trim())
							{
							case "[VALUE]":
								att.Value = _model.ValueOrigin;
								break;
							case "[UPPER]":
								att.Upper = _model.UpperOrigin;
								break;
							case "[LOWER]":
								att.Lower = _model.LowerOrigin;
								break;
							case "[RESULT]":
								att.Result = null;
								break;
							case "[WARNUPPER]":
								_model.WarnUpper = null;
								break;
							case "[WARNLOWER]":
								_model.WarnLower = null;
								break;
							}
						}
					}
				}
			}
			if (_model.ResultOld != att.Result)
			{
				_model.Result = att.Result;
				_model.IsChange = true;
			}
			if (_model.ValueOld != att.Value)
			{
				_model.Value = att.Value;
				_model.IsChangeOther = true;
			}
			if (_model.UpperOld != att.Upper)
			{
				_model.Upper = att.Upper;
				_model.IsChangeOther = true;
			}
			if (_model.LowerOld != att.Lower)
			{
				_model.Lower = att.Lower;
				_model.IsChangeOther = true;
			}
		}
		return lstmodel;
	}

	private async Task<string> getParameter(FormulaViewModel model, List<FormulaViewModel> lstmodel, Request request, string name)
	{
		string[] lstname = name.Split('#');
		string result = string.Empty;
		if (lstname.Length > 1)
		{
			FormulaViewModel formulaViewModel = lstmodel.Where((FormulaViewModel x) => x.Code.Equals(lstname[0])).FirstOrDefault();
			if (formulaViewModel == null)
			{
				formulaViewModel = await _uow.GetRepository<RequestResult>().GetSingleAsync<FormulaViewModel>((RequestResult x) => x.Measurement.Code.Equals(lstname[0]) && x.RequestId.Equals(request.Id) && x.Sample.Equals(model.Sample) && x.Cavity.Equals(model.Cavity));
			}
			if (formulaViewModel != null)
			{
				switch (lstname[1])
				{
				case "RESULT":
					result = ((formulaViewModel.ResultOrigin != null) ? formulaViewModel.ResultOrigin : string.Empty);
					break;
				case "VALUE":
					result = ((formulaViewModel.ValueOrigin != null) ? formulaViewModel.ValueOrigin : string.Empty);
					break;
				case "UPPER":
					result = (formulaViewModel.UpperOrigin.HasValue ? formulaViewModel.UpperOrigin.ToString() : string.Empty);
					break;
				case "LOWER":
					result = (formulaViewModel.LowerOrigin.HasValue ? formulaViewModel.LowerOrigin.ToString() : string.Empty);
					break;
				}
			}
		}
		else
		{
			switch (lstname[0])
			{
			case "RESULT":
				result = ((model.ResultOrigin != null) ? model.ResultOrigin : string.Empty);
				break;
			case "VALUE":
				result = ((model.ValueOrigin != null) ? model.ValueOrigin : string.Empty);
				break;
			case "UPPER":
				result = (model.UpperOrigin.HasValue ? model.UpperOrigin.ToString() : string.Empty);
				break;
			case "LOWER":
				result = (model.LowerOrigin.HasValue ? model.LowerOrigin.ToString() : string.Empty);
				break;
			}
		}
		return result;
	}

	private async Task<string> calFormula(List<FormulaViewModel> lstmodel, FormulaViewModel model, Request request, string formula)
	{
		string illogical = string.Empty;
		bool isFinish = true;
		string result = formula;
		IEnumerable<string> enumerable = from x in formula.Split('[')
			select (x.Split(']').Length == 0) ? "" : x.Split(']')[0] into x
			where !x.Trim().Equals("") && !x.ToLower().Contains("system.math")
			select x;
		foreach (string _formula in enumerable)
		{
			string[] lstname = _formula.Split('#');
			if (lstname.Length <= 1)
			{
				continue;
			}
			FormulaViewModel formulaViewModel = lstmodel.Where((FormulaViewModel x) => x.Code.Equals(lstname[0])).FirstOrDefault();
			if (formulaViewModel == null)
			{
				formulaViewModel = await _uow.GetRepository<Measurement>().GetSingleAsync<FormulaViewModel>((Measurement x) => x.Code.Equals(lstname[0]) && x.ProductId.Equals(request.ProductId));
			}
			if (formulaViewModel == null)
			{
				continue;
			}
			if (formulaViewModel.Code.Equals(model.Code))
			{
				result = result.Replace(formulaViewModel.Code + "#", "");
			}
			else
			{
				if (string.IsNullOrEmpty(formulaViewModel.Formula))
				{
					continue;
				}
				IEnumerable<string> enumerable2 = from x in formulaViewModel.Formula.Split(';')
					where !x.Trim().Equals("")
					select x;
				foreach (string item in enumerable2)
				{
					List<string> list = (from x in item.Split("=")
						where !x.Trim().Equals("")
						select x).ToList();
					if (list.Count <= 1)
					{
						continue;
					}
					if (list[1].Contains("[VALUE]"))
					{
						list[1] = list[1].Replace("[VALUE]", "[" + formulaViewModel.Code + "#VALUE]");
						illogical = "VALUE";
					}
					if (list[1].Contains("[UPPER]"))
					{
						list[1] = list[1].Replace("[UPPER]", "[" + formulaViewModel.Code + "#UPPER]");
						illogical = "UPPER";
					}
					if (list[1].Contains("[LOWER]"))
					{
						list[1] = list[1].Replace("[LOWER]", "[" + formulaViewModel.Code + "#LOWER]");
						illogical = "LOWER";
					}
					if (list[1].Contains("[RESULT]"))
					{
						list[1] = list[1].Replace("[RESULT]", "[" + formulaViewModel.Code + "#RESULT]");
						illogical = "RESULT";
					}
					if (list[0].Contains("[" + lstname[1] + "]"))
					{
						result = result.Replace("[" + _formula + "]", list[1] ?? "");
						if (!illogical.Contains(lstname[1]))
						{
							isFinish = false;
						}
					}
				}
			}
		}
		if (!isFinish)
		{
			result = await calFormula(lstmodel, model, request, result);
		}
		return result;
	}

	private List<FormulaViewModel> relationFormula(IEnumerable<FormulaViewModel> lstmodel, string code)
	{
		List<FormulaViewModel> list = new List<FormulaViewModel>();
		IEnumerable<FormulaViewModel> enumerable = lstmodel.Where((FormulaViewModel x) => !string.IsNullOrEmpty(x.Formula) && x.Formula.Contains("[" + code + "#"));
		foreach (FormulaViewModel item in enumerable)
		{
			list.Add(item);
			list.AddRange(relationFormula(lstmodel, item.Code));
		}
		return list;
	}
}
