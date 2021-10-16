using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class StoredProcs
    {
        private readonly IConfiguration _configuration;

        public StoredProcs(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JsonResult GetDepartments()
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS GetDepartments";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE PROCEDURE GetDepartments() " +
                                  "BEGIN " +
                                  "SELECT * FROM `departments`; " +
                                  "END";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
            conn.Close();

            try
            {
                DataTable table = new DataTable();
                MySqlDataReader myReader;

                conn.Open();
                using (MySqlCommand myCommand = new MySqlCommand("GetDepartments", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(table);
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }

        public JsonResult AddDepartment(Department department)
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS AddDepartment";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"CREATE PROCEDURE AddDepartment() " +
                                  $"BEGIN INSERT INTO `departments` " +
                                  $"VALUES(null, {department.DepartmentName}); " +
                                  $"END";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
            conn.Close();

            try
            {
                DataTable table = new DataTable();
                MySqlDataReader myReader;

                conn.Open();
                using (MySqlCommand myCommand = new MySqlCommand("AddDepartment", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(new { success = $"Added {department} to Departments Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }


        }
    }
}
