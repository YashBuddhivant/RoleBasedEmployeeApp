using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp
{
    public class EmployeeDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        public List<Employee> GetAllEmployees()
        {
            List<Employee> empList = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllEmployees", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    empList.Add(MapEmployee(reader));
                }
            }

            return empList;
        }

        public Employee? GetEmployeeById(int id)
        {
            Employee? emp = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployeeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", id);

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    emp = MapEmployee(reader);
                }
            }

            return emp;
        }

        public void AddEmployee(Employee emp)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                AddEmployeeParameters(cmd, emp);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(Employee emp)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                AddEmployeeParameters(cmd, emp);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spDeleteEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Shared by Add and Update, so the parameter list is written once
        private static void AddEmployeeParameters(SqlCommand cmd, Employee emp)
        {
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Gender", emp.Gender);
            cmd.Parameters.AddWithValue("@Age", emp.Age);
            cmd.Parameters.AddWithValue("@DepartmentId", (object?)emp.DepartmentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Designation", emp.Designation);
            cmd.Parameters.AddWithValue("@City", emp.City);
            cmd.Parameters.AddWithValue("@JoinDate", (object?)emp.JoinDate ?? DBNull.Value);
        }

        // Turns the current row of the reader into an Employee
        private static Employee MapEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                Name = reader["Name"].ToString() ?? "",
                Gender = reader["Gender"].ToString() ?? "",
                Age = Convert.ToInt32(reader["Age"]),
                DepartmentId = reader["DepartmentId"] == DBNull.Value
                    ? null : Convert.ToInt32(reader["DepartmentId"]),
                DepartmentName = reader["DepartmentName"] == DBNull.Value
                    ? null : reader["DepartmentName"].ToString(),
                Designation = reader["Designation"].ToString() ?? "",
                City = reader["City"].ToString() ?? "",
                JoinDate = reader["JoinDate"] == DBNull.Value
                    ? null : Convert.ToDateTime(reader["JoinDate"])
            };
        }
        public List<SelectListItem> GetDepartments()
        {
            List<SelectListItem> list = new();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllDepartments", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new SelectListItem(
                        reader["DepartmentName"].ToString(),
                        reader["DepartmentId"].ToString()));
                }
            }
            return list;
        }
    }
}