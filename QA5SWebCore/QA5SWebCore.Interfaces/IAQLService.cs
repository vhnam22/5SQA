using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IAQLService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(AQLViewModel model);

	Task<ResponseDto> Move(Guid idfrom, Guid idto);

	Task<ResponseDto> SaveList(IEnumerable<AQLViewModel> models);

	Task<ResponseDto> Samples(AQLDto model);
}
