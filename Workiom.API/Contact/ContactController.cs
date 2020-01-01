namespace Workiom.API.Contact
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Workiom.Core.Company;
    using Workiom.Core.Contact;
    using Workiom.Web.Models;
    using Workiom.Web.Models.Contact;

    [Route("api/contacts")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICompanyRepository _companyRepository;

        public ContactController(IContactRepository contactRepository, ICompanyRepository companyRepository)
        {
            _contactRepository = contactRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContactsAsync()
        {
            var contacts = await _contactRepository.GetAllContactsAsync();

            var result = Mapper.Map<List<ContactOutputModel>>(contacts);

            return Ok(ResponseResult.SucceededWithData(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Contact Id can't be empty."));
            }

            var contact = await _contactRepository.GetContactByIdAsync(id);

            if (contact is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "no hit information"));
            }

            var result = Mapper.Map<ContactOutputModel>(contact);

            return Ok(ResponseResult.SucceededWithData(result));
        }

        [HttpPost]
        public async Task<IActionResult> AddContactyAsync(ContactInputModel model)
        {
            if (string.IsNullOrEmpty(model.CompanyId))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Company id can't be empty."));
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Contact name can't be empty."));
            }

            var company = await _companyRepository.GetCompanyByIdAsync(model.CompanyId);

            if (company is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "Company not found"));
            }

            var contact = Contact.New(model.Name, company.Id);
            await _contactRepository.AddContactAsync(contact);
            company.AddContact(contact.Id);
            await _companyRepository.UpdateCompanyAsync(company);

            var result = Mapper.Map<ContactOutputModel>(contact);

            return Ok(ResponseResult.SucceededWithData(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Contact Id can't be empty."));
            }

            var contact = await _contactRepository.GetContactByIdAsync(id);

            if (contact is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "no hit information"));
            }

            contact.Delete();

            await _contactRepository.DeleteContactAsync(contact);

            return Ok(ResponseResult.Succeeded());
        }
    }
}