using System;
using System.Collections.Generic;
using CompanyEmployee.Entities.Models;

namespace CompanyEmployee.Contracts
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompany(Guid id, bool trackChanges);
    }
}