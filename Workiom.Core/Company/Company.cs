namespace Workiom.Core.Company
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Contact;

    public class Company : DomainEntity
    {
        [Required]
        public string Name { get; private set; }
        public int NumOfEmployees { get; private set; }
        public ICollection<string> ContactIds { get; set; }

        public static Company New(string name, int numOfEmpolyees)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new Company
            {
                Name = name,
                NumOfEmployees = numOfEmpolyees,
                CreationDate = DateTime.Now
            };
        }

        public void AddContact(string contactId)
        {
            if (ContactIds is null)
            {
                ContactIds = new List<string>();
                ContactIds.Add(contactId);
            }
            else
            {
                ContactIds.Add(contactId);
            }
        }

        public void Delete()
        {
            DeletionDate = DateTime.Now;
            IsDeleted = true;
        }
    }
}
