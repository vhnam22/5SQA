using System;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IMachineService
{
	Task<ResponseDto> GetForTools(Guid idtablet, QueryArgs args);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(MachineViewModel model);

	Task<ResponseDto> GetLiences(string ime);
}
