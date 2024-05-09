
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Contracts.Responses;

namespace WebApi.Dependencies;

public static class AutoMapperDependencyInjection
{
    public static IServiceCollection AgregarAutoMapper(this IServiceCollection services)
    {
        // Auto Mapper Configurations
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        var mapper = mappingConfig.CreateMapper();

        return services.AddSingleton(mapper);
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonaRequest, PersonaEntity>()
            .ForMember(dest => dest.IdPersona, opt => opt.Ignore()); // Ignora el mapeo del campo IdPersona
        CreateMap<PersonaEntity, PersonaResponse>().ReverseMap();
        CreateMap<ClienteRequest, ClienteEntity>()
            .ForMember(dest => dest.IdCliente, opt => opt.Ignore()); // Ignora el mapeo del campo IdPersona
        CreateMap<ClienteEntity, ClienteResponse>().ReverseMap();
        CreateMap<PersonaUpdateDTO, PersonaRequest>().ReverseMap();
        CreateMap<PersonaUpdateDTO, PersonaEntity>()
            .ForMember(dest => dest.IdPersona, opt => opt.Ignore()).ReverseMap(); // Ignora el mapeo del campo IdPersona
        CreateMap<ClienteUpdateDTO, ClienteRequest>()
            .ReverseMap(); ;
        CreateMap<ClienteUpdateDTO, ClienteEntity>()
            .ForMember(dest => dest.IdCliente, opt => opt.Ignore())
            .ReverseMap(); // Ignora el mapeo del campo IdPersona
        CreateMap<ClienteUpdateDTO, ClienteUpdateRequest>()
            .ReverseMap();
        CreateMap<ClienteEntity, ClienteUpdateRequest>()
           .ReverseMap();
        CreateMap<CuentaEntity, CuentaRequest>().ReverseMap();
        CreateMap<MovimientosEntity, MovimientosRequest>().ReverseMap();
    }
}