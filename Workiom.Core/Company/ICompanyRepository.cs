namespace Workiom.Core.Company
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(string id);
        Task AddCompanyAsync(Company company);
        Task UpdateCompanyAsync(Company company);
        Task DeleteCompanyAsync(Company company);
    }
}
