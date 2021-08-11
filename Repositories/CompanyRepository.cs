using System;
using System.Collections.Generic;
using System.Linq;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.Models;

namespace CompanyEmployee.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
            FindAll(trackChanges).OrderBy(c => c.Name).ToList();

        public Company GetCompany(Guid id, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();

        public void CreateCompany(Company company) => 
            Create(company);

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id), trackChanges: false);
        }
    }
}