namespace RoleBasedEmployeeApp
{
    public static class ConnectionString
    {
        private static string cs = "Server=localhost\\SQLEXPRESS; Database=RoleAdminDb; Trusted_Connection=True; TrustServerCertificate=True;";
        public static string dbcs { get => cs; }
    }
}
