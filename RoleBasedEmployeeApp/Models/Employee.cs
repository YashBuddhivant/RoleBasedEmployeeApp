using System.ComponentModel.DataAnnotations;

namespace RoleBasedEmployeeApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10)]
        public string Gender { get; set; } = "";

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(100)]
        public string Designation { get; set; } = "";

        [Required(ErrorMessage = "City is required")]
        [StringLength(100)]
        public string City { get; set; } = "";

        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        public DateTime? JoinDate { get; set; }

        // Not a table column: filled from a JOIN when listing employees
        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }
    }
}