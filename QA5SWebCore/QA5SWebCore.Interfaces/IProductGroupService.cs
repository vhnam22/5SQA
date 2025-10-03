using System;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IProductGroupService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(ProductGroupViewModel model);

	Task<ResponseDto> Copy(Guid id);
}
