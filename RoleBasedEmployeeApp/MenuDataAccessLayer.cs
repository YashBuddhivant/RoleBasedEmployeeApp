using System.Data;
using Microsoft.Data.SqlClient;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp
{
    public class MenuDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        public List<MenuItem> GetMenusByRole(int roleId)
        {
            List<MenuItem> menus = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetMenusByRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", roleId);

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    menus.Add(new MenuItem
                    {
                        MenuId = Convert.ToInt32(reader["MenuId"]),
                        Title = reader["Title"].ToString() ?? "",
                        Url = reader["Url"].ToString() ?? "",
                        Icon = reader["Icon"] == DBNull.Value ? null : reader["Icon"].ToString(),
                        SortOrder = Convert.ToInt32(reader["SortOrder"]),
                        CanView = Convert.ToBoolean(reader["CanView"]),
                        CanAdd = Convert.ToBoolean(reader["CanAdd"]),
                        CanEdit = Convert.ToBoolean(reader["CanEdit"]),
                        CanDelete = Convert.ToBoolean(reader["CanDelete"])
                    });
                }
            }

            return menus;
        }

        public Permission GetPermission(int roleId, string menuUrl)
        {
            Permission perm = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetPermission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@MenuUrl", menuUrl);

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    perm.CanView = Convert.ToBoolean(reader["CanView"]);
                    perm.CanAdd = Convert.ToBoolean(reader["CanAdd"]);
                    perm.CanEdit = Convert.ToBoolean(reader["CanEdit"]);
                    perm.CanDelete = Convert.ToBoolean(reader["CanDelete"]);
                }
            }

            return perm;
        }
    }
}