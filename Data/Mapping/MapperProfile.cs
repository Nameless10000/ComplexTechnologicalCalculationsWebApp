using AutoMapper;
using BaseLib.SlagMode.Models;
using Core.Models.SlagMode;

namespace Data.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Регистрация проекций

        #region Projections

        #endregion

        // Регистрация маппов

        #region Mapps
        CreateMap<RequestData, Request>().ReverseMap()
            .ForMember(x => x.User, opt => opt.Ignore());
        CreateMap<ResponseData, Response>().ReverseMap();
        CreateMap<InputChargeComponentsForCalc, ChargeComponent>().ReverseMap();
        CreateMap<InputCokeForCalcs, InputCoke>().ReverseMap();
        CreateMap<InputCastIronForCalc, CastIron>().ReverseMap();
        CreateMap<InputSlagForCalc, Slag>().ReverseMap();
        #endregion
    }
}