namespace Workiom.Data.Repositories
{
    using System;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workiom.Core.Contact;
    using Workiom.Data.Context;
    using System.Linq;

    public class ContactRepository : IContactRepository
    {
        private readonly WorkiomDbContext _context;

        public ContactRepository(WorkiomDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts.Find(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(string id)
        {
            return await _context.Contacts.Find(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task AddContactAsync(Contact contact)
        {
            await _context.Contacts.InsertOneAsync(contact);
        }

        public async Task<IEnumerable<Contact>> GetContactsByListOfIdsAsync(ICollection<string> ids)
        {
            var filter = Builders<Contact>.Filter.And(
                Builders<Contact>.Filter.Where(x => !x.IsDeleted),
                Builders<Contact>.Filter.In(x => x.Id, ids));

            return await _context.Contacts.Find(filter).ToListAsync();
        }

        public async Task DeleteContactAsync(Contact contact)
        {
            var filter = Builders<Contact>.Filter.Eq(nameof(contact.Id), contact.Id);
            var deletedDocument = Builders<Contact>.Update
                .Set(nameof(contact.DeletionDate), contact.DeletionDate)
                .Set(nameof(contact.IsDeleted), contact.IsDeleted);
            await _context.Contacts.UpdateOneAsync(filter, deletedDocument);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            var filter = Builders<Contact>.Filter.Eq(nameof(contact.Id), contact.Id);
            var updatedDocument = Builders<Contact>.Update
                .Set(nameof(contact.CompanyIds), contact.CompanyIds);
            await _context.Contacts.UpdateOneAsync(filter, updatedDocument);
        }
    }
}
