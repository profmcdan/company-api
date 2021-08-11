using System;
using System.Collections.Generic;
using System.Linq;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.Models;

namespace CompanyEmployee.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
        {
            return FindByCondition(e => 
                e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.Name);
        }

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            return FindByCondition(e => 
                    e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges: false)
                .SingleOrDefault();
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