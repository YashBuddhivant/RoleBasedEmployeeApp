using System.Data;
using Microsoft.Data.SqlClient;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp
{
    public class UserDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        public User? GetUserByName(string username)
        {
            User? user = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetUserByUsername", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString() ?? "",
                        FullName = reader["FullName"].ToString() ?? "",
                        PasswordHash = reader["PasswordHash"].ToString() ?? "",
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        RoleId = Convert.ToInt32(reader["RoleId"]),
                        RoleName = reader["RoleName"].ToString() ?? ""
                    };
                }
            }

            return user;
        }
    }
}