namespace Workiom.Core.Contact
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(string id);
        Task AddContactAsync(Contact contact);
        Task<IEnumerable<Contact>> GetContactsByListOfIdsAsync(ICollection<string> ids);
        Task DeleteContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
    }
}
