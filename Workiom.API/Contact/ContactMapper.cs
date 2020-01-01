namespace Workiom.API.Contact
{
    using AutoMapper;
    using Workiom.Web.Models.Contact;
    using Core.Contact;

    public class ContactMapper : Profile
    {
        public ContactMapper()
        {
            CreateMap<Contact, ContactOutputModel>();
        }
    }
}
