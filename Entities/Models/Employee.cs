using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyEmployee.Entities.Models
{
    public class Employee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Employee name is required")]
        [MaxLength(30, ErrorMessage = "Maximum lenght for Name is 30 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        
        [Required(ErrorMessage = "Position name is required")]
        public string Position { get; set; }
        
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}