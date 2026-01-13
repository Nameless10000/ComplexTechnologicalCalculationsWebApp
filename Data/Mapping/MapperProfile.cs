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
        CreateMap<RequestData, Request>()
            .ForMember(x => x.CastIron, opt => opt.MapFrom(s => s.Iron))
            .ForMember(x => x.InputCoke, opt => opt.MapFrom(s => s.Coke))
            .ReverseMap()
            .ForMember(x => x.Iron, opt => opt.MapFrom(s => s.CastIron))
            .ForMember(x => x.Coke, opt => opt.MapFrom(s => s.InputCoke))
            .ForMember(x => x.User, opt => opt.Ignore());
        CreateMap<ResponseData, Response>().ReverseMap();
        CreateMap<InputChargeComponentsForCalc, ChargeComponent>().ReverseMap();
        CreateMap<InputCokeForCalcs, InputCoke>().ReverseMap();
        CreateMap<InputCastIronForCalc, CastIron>().ReverseMap();
        CreateMap<InputSlagForCalc, Slag>().ReverseMap();
        #endregion
    }
}