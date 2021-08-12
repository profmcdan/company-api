using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyEmployee.Entities.Models;
using CompanyEmployee.Entities.RequestFeatures;

namespace CompanyEmployee.Contracts
{
    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges);
        Task<Company> GetCompanyAsync(Guid id, bool trackChanges);
        void CreateCompany(Company company);
        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCompany(Company company);
    }
}