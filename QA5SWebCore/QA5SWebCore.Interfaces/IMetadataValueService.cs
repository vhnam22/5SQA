using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IMetadataValueService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(MetadataValueViewModel model);

	Task<ResponseDto> SaveRanks(IEnumerable<MetadataValueViewModel> models);

	Task<ResponseDto> GetDecimals(QueryArgs args);
}
