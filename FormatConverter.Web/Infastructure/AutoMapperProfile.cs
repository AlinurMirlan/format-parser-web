using AutoMapper;
using FormatConverter.Library;
using FormatConverter.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace FormatConverter.Web.Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        base.CreateMap<RegistrationVm, IdentityUser>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(registrationModel => registrationModel.Email));
        base.CreateMap<IFormatConverter, ConverterVm>()
            .ForMember(f => f.ConverterClassName, opt => opt.MapFrom(formatProvider => formatProvider.GetType().Name));
    }
}
