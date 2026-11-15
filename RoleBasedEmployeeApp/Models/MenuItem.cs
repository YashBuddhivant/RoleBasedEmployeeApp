namespace RoleBasedEmployeeApp.Models
{
    public class MenuItem
    {
        public int MenuId { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string? Icon { get; set; }
        public int SortOrder { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}