using System.ComponentModel.DataAnnotations;

namespace CompanyEmployee.Entities.DataTransferObjects
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [MaxLength(30, ErrorMessage = "Maximum lenght for Name is 30 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Age name is required")]
        public int Age { get; set; }
        
        [Required(ErrorMessage = "Position name is required")]
        [MaxLength(20, ErrorMessage = "Position lenght for Name is 20 characters")]
        public string Position { get; set; }
    }
}