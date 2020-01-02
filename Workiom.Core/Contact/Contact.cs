namespace Workiom.Core.Contact
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contact : DomainEntity
    {
        [Required]
        public string Name { get; private set; }
        public ICollection<string> CompanyIds { get; private set; }

        public static Contact New(string name, string companyId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (companyId is null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            var contact = new Contact
            {
                Name = name,
                CreationDate = DateTime.Now
            };

            contact.CompanyIds = new List<string>();

            contact.CompanyIds.Add(companyId);
            return contact;
        }

        public void AddCompany(string companyId)
        {
            if (companyId is null)
            {
                CompanyIds = new List<string>();
                CompanyIds.Add(companyId);
            }
            else
            {
                CompanyIds.Add(companyId);
            }
        }

        public void Delete()
        {
            DeletionDate = DateTime.Now;
            IsDeleted = true;
        }
    }
}
