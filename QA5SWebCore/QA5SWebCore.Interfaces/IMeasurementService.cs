using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IMeasurementService
{
	Task<ResponseDto> Gets(Guid productid, Guid planid);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Delete(Guid id);

	Task<ResponseDto> Save(MeasurementViewModel model);

	Task<ResponseDto> SaveList(IEnumerable<MeasurementViewModel> models);

	Task<ResponseDto> Move(Guid idfrom, Guid idto);

	Task<ResponseDto> Gets(Guid id, IEnumerable<string> lstmachinetype);

	Task<ResponseDto> SaveCoordinate(MeasurementDto model);
}
