namespace Workiom.Data.Repositories
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workiom.Core.Company;
    using Workiom.Data.Context;
    using System.Linq;
    using MongoDB.Bson;

    public class CompanyRepository : ICompanyRepository
    {
        private readonly WorkiomDbContext _context;

        public CompanyRepository(WorkiomDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            var filter = Builders<Company>.Filter.Where(x => !x.IsDeleted);

            return await _context.Companies.Find(filter)
                .ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(string id)
        {
            return await _context.Companies
           .Find(x => x.Id == id && !x.IsDeleted)
           .FirstOrDefaultAsync();
        }

        public async Task AddCompanyAsync(Company company)
        {
            await _context.Companies.InsertOneAsync(company);
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            var filter = Builders<Company>.Filter.Where(x => x.Id == company.Id && !x.IsDeleted);
            var updatedDocument = Builders<Company>.Update
                .Set(nameof(company.ContactIds), company.ContactIds);
            await _context.Companies.UpdateOneAsync(filter, updatedDocument);
        }

        public async Task DeleteCompanyAsync(Company company)
        {
            var filter = Builders<Company>.Filter.Eq(nameof(company.Id), company.Id);
            var deletedDocument = Builders<Company>.Update
                .Set(nameof(company.DeletionDate), company.DeletionDate)
                .Set(nameof(company.IsDeleted), company.IsDeleted);
            await _context.Companies.UpdateOneAsync(filter, deletedDocument);
        }
    }
}
