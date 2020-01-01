namespace Workiom.Web.Models.Company
{
    using System;
    using System.Collections.Generic;
    using Workiom.Web.Models.Contact;

    public class CompanyOutputModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int NumOfEmployees { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ContactOutputModel> Contacts { get; set; }
    }
}
