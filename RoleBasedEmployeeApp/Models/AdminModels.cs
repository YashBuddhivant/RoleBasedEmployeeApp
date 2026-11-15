using System.ComponentModel.DataAnnotations;

namespace RoleBasedEmployeeApp.Models
{
    public class UserRow
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string RoleName { get; set; } = "";
        public bool IsActive { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Select a role")]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
    }

    public class RoleItem
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = "";
    }

    // one row of the checkbox grid: reuses your Permission class
    public class PermissionRow : Permission
    {
        public int MenuId { get; set; }
        public string Title { get; set; } = "";
    }

    public class PermissionMatrixViewModel
    {
        public int RoleId { get; set; }
        public List<PermissionRow> Rows { get; set; } = new();
    }

    public class SettingItem
    {
        public string SettingKey { get; set; } = "";
        public string? SettingValue { get; set; }
    }
}