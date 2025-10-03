using System;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IPlanDetailService
{
	Task<ResponseDto> Gets(Guid productid, Guid requestid);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(PlanDetailViewModel model);
}
