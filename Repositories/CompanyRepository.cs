using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.Models;
using CompanyEmployee.Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployee.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters,
            bool trackChanges)
        {
            var companies = await FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .Skip((companyParameters.PageNumber - 1) * companyParameters.PageSize)
                .Take(companyParameters.PageSize)
                .ToListAsync();
            
            var count = await FindAll(trackChanges: false).CountAsync();
            return PagedList<Company>.ToPagedList(companies, companyParameters.PageNumber, 
                companyParameters.PageSize, count); 
        }
        

        public async Task<Company> GetCompanyAsync(Guid id, bool trackChanges) => 
            await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void CreateCompany(Company company) => 
            Create(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => 
            await FindByCondition(x => ids.Contains(x.Id), trackChanges: false).ToListAsync();

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }
    }
}