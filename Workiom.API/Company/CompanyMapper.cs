namespace Workiom.API.Company
{
    using AutoMapper;
    using Workiom.Web.Models.Company;
    using Core.Company;

    public class CompanyMapper : Profile
    {
        public CompanyMapper()
        {
            CreateMap<Company, CompanyOutputModel>()
                .ForMember(dest => dest.Contacts, opt => opt.Ignore());
        }
    }
}
