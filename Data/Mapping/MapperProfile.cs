using AutoMapper;
using BaseLib.AglomMode.Models;
using BaseLib.SlagMode.Models;
using Console;
using Console.DTO;
using Core.Models.AglomMode;
using Core.Models.SlagMode;
using System.ComponentModel;

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

        CreateMap<AglomRequestData, AglomRequestDB>().ReverseMap();
        CreateMap<AglomResponseData, AglomResponseDB>().ReverseMap();
        CreateMap<CalcLES, CalcLESDB>().ReverseMap();
        CreateMap<Cocksick, CocksickDB>().ReverseMap();
        CreateMap<END, ENDDB>().ReverseMap();
        CreateMap<FluxAdditions, FluxAdditionsDB>().ReverseMap();
        CreateMap<Shihta, ShihtaDB>().ReverseMap();
        CreateMap<ShihtaComponent, ShihtaComponentDB>().ReverseMap();
        CreateMap<StartEnter, StartEnterDB>().ReverseMap();
        CreateMap<ZolaOfCocksick, ZolaOfCocksickDB>().ReverseMap();
        CreateMap<ComponentInfo, ComponentInfoDB>().ReverseMap();

        #endregion
    }
}