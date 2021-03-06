﻿namespace Workiom.API.Company
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Workiom.Core.Company;
    using Workiom.Web.Models;
    using Workiom.Web.Models.Company;
    using Workiom.Core.Contact;
    using AutoMapper;
    using Workiom.Web.Models.Contact;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IContactRepository _contactRepository;

        public CompanyController(ICompanyRepository companyRepository, IContactRepository contactRepository)
        {
            _companyRepository = companyRepository;
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllCompaniesAsync();

            var result = Mapper.Map<List<CompanyOutputModel>>(companies);

            int idx = 0;
            foreach (var item in companies)
            {
                if (item.ContactIds != null)
                {
                    var contacts = await _contactRepository.GetContactsByListOfIdsAsync(item.ContactIds);
                    result[idx].Contacts = Mapper.Map<List<ContactOutputModel>>(contacts);
                }
                idx++;
            }
            return Ok(ResponseResult.SucceededWithData(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Company Id can't be empty."));
            }

            var company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "Company isn't found."));
            }

            var result = Mapper.Map<CompanyOutputModel>(company);

            if (company.ContactIds != null)
            {
                var contacts = await _contactRepository.GetContactsByListOfIdsAsync(company.ContactIds);
                result.Contacts = Mapper.Map<List<ContactOutputModel>>(contacts);
            }

            return Ok(ResponseResult.SucceededWithData(result));
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyAsync(CompanyInputModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Company name can't be empty."));
            }

            var company = Company.New(model.Name, model.NumOfEmpolyees);

            await _companyRepository.AddCompanyAsync(company);

            var result = Mapper.Map<CompanyOutputModel>(company);

            return Ok(ResponseResult.SucceededWithData(result));
        }

        [HttpPut("{id}/AddContact")]
        public async Task<IActionResult> AddContactToCompanyAsync(string id, AddContactCompanyInputModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Company Id can't be empty."));
            }

            var company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "Company isn't found."));
            }

            if (string.IsNullOrEmpty(model.ContactId))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Contact Id can't be empty."));
            }

            var contact = await _contactRepository.GetContactByIdAsync(model.ContactId);

            if (contact is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "Contact isn't found."));
            }

            var isExist = company.ContactIds != null ? company.ContactIds.FirstOrDefault(x => x == model.ContactId) : null;

            if (isExist != null)
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.Error, "Contact is already exist."));
            }

            company.AddContact(model.ContactId);
            contact.AddCompany(company.Id);

            await _companyRepository.UpdateCompanyAsync(company);
            await _contactRepository.UpdateContactAsync(contact);

            return Ok(ResponseResult.Succeeded());
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseResult.Failed(ErrorCode.ValidationError, "Company Id can't be empty."));
            }

            var company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company is null)
            {
                return NotFound(ResponseResult.Failed(ErrorCode.Error, "Company isn't found."));
            }

            company.Delete();

            await _companyRepository.DeleteCompanyAsync(company);

            return Ok(ResponseResult.Succeeded());
        }
    }
}
