using System;
using System.Collections.Generic;
using CompanyEmployee.Entities.Models;

namespace CompanyEmployee.Contracts
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
    }
}