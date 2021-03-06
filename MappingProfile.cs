using AutoMapper;
using CompanyEmployee.Entities.DataTransferObjects;
using CompanyEmployee.Entities.Models;

namespace CompanyEmployee
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress, 
                    opt => opt.MapFrom(x 
                        => string.Join(' ', x.Address, x.Country)));

            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateCompanyDto, Company>();
            CreateMap<UpdateEmployeeDto, Employee>().ReverseMap();
            CreateMap<RegisterUserDto, User>();
        }
    }
}