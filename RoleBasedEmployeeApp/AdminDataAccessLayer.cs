using System.Data;
using Microsoft.Data.SqlClient;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp
{
    public class AdminDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        // ---------- Users ----------
        public List<UserRow> GetUsers()
        {
            List<UserRow> list = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new UserRow
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString() ?? "",
                        FullName = reader["FullName"].ToString() ?? "",
                        RoleName = reader["RoleName"].ToString() ?? "",
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            return list;
        }

        public void AddUser(string username, string fullName, string passwordHash, int roleId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@RoleId", roleId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ---------- Roles ----------
        public List<RoleItem> GetRoles()
        {
            List<RoleItem> list = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new RoleItem
                    {
                        RoleId = Convert.ToInt32(reader["RoleId"]),
                        RoleName = reader["RoleName"].ToString() ?? ""
                    });
                }
            }
            return list;
        }

        // ---------- Permission matrix ----------
        public List<PermissionRow> GetPermissionMatrix(int roleId)
        {
            List<PermissionRow> list = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetPermissionMatrix", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", roleId);

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new PermissionRow
                    {
                        MenuId = Convert.ToInt32(reader["MenuId"]),
                        Title = reader["Title"].ToString() ?? "",
                        CanView = Convert.ToBoolean(reader["CanView"]),
                        CanAdd = Convert.ToBoolean(reader["CanAdd"]),
                        CanEdit = Convert.ToBoolean(reader["CanEdit"]),
                        CanDelete = Convert.ToBoolean(reader["CanDelete"])
                    });
                }
            }
            return list;
        }

        public void SavePermission(int roleId, PermissionRow row)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spSavePermission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@MenuId", row.MenuId);
                cmd.Parameters.AddWithValue("@CanView", row.CanView);
                cmd.Parameters.AddWithValue("@CanAdd", row.CanAdd);
                cmd.Parameters.AddWithValue("@CanEdit", row.CanEdit);
                cmd.Parameters.AddWithValue("@CanDelete", row.CanDelete);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ---------- Settings ----------
        public List<SettingItem> GetSettings()
        {
            List<SettingItem> list = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new SettingItem
                    {
                        SettingKey = reader["SettingKey"].ToString() ?? "",
                        SettingValue = reader["SettingValue"].ToString()
                    });
                }
            }
            return list;
        }

        public void UpdateSetting(string key, string value)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettingKey", key);
                cmd.Parameters.AddWithValue("@SettingValue", value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}