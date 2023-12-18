using AutoMapper;
using LibraryServices.Domain.DataTransferObjects.Dynamo;
using LibraryServices.Domain.DataTransferObjects.FamilyLibrary;
using LibraryServices.Domain.DataTransferObjects.FamilyParameter;
using LibraryServices.Domain.DataTransferObjects.Identity;
using LibraryServices.Domain.Models.Dynamo;
using LibraryServices.Domain.Models.FamilyLibrary;
using LibraryServices.Domain.Models.FamilyParameter;
using LibraryServices.Domain.Models.Identity;
using LibraryServices.Infrastructure.Sercurity;

namespace LibraryServices.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FamilyCategory, FamilyCategoryDTO>();
            CreateMap<FamilyCategory, FamilyCategoryBasicDTO>();
            CreateMap<FamilySymbol, FamilySymbolDTO>();
            CreateMap<Family, FamilyDetailDTO>();


            CreateMap<Package, PackageDTO>();
            CreateMap<PackageVersion, PackageVersionDTO>();

            CreateMap<User, UserDTO>();
            CreateMap<UserCreationDTO, User>().ForMember(u => u.Salt,
                    options => { options.MapFrom((ud, u) => { return u.Salt = Guid.NewGuid().ToString("N"); }); })
                .ForMember(u => u.Password,
                    options => { options.MapFrom((ud, u) => ud.Password!.MD5Encrypt32(u.Salt!)); });

            CreateMap<ParameterGroup, ParameterGroupDTO>();

            CreateMap<Parameter, ParameterDTO>();

            CreateMap<DisplayUnitType, DisplayUnitTypeDTO>();

            CreateMap<ParameterUnitType, UnitTypeDTO>();

            CreateMap<ParameterType, ParameterTypeDTO>();

            CreateMap<ParameterDefinition, ParameterDefinitionDTO>();

            CreateMap<ParameterCreationDTO, Parameter>();

            CreateMap<ParameterDefinitionCreationDTO, ParameterDefinition>();
        }
    }
}
