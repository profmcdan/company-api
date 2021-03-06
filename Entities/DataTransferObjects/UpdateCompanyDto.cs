using System.Collections.Generic;

namespace CompanyEmployee.Entities.DataTransferObjects
{
    public class UpdateCompanyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        
        public IEnumerable<CreateEmployeeDto> Employees { get; set; }

    }
}