using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployee.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges)
        {
            return await FindByCondition(e => 
                e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            return await FindByCondition(e => 
                    e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges: false)
                .SingleOrDefaultAsync();
        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }
    }
}