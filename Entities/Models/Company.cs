using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyEmployee.Entities.Models
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(60, ErrorMessage = "Maximum length for Name is 60 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Address name is required")]
        [MaxLength(250, ErrorMessage = "Maximum length for Address is 250 characters")]
        public string Address { get; set; }

        public string Country { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}