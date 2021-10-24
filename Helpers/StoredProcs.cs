using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class StoredProcs
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public StoredProcs(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }

        #region Departments
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
                                  $"BEGIN " +
                                  $"INSERT INTO `departments` " +
                                  $"VALUES(null, '{department.DepartmentName}'); " +
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

                return new JsonResult(new { success = $"Added {department.DepartmentName} to Departments Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }

        public JsonResult UpdateDepartmentName(Department department)
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS UpdateDepartment";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"CREATE PROCEDURE UpdateDepartment() " +
                                  $"BEGIN UPDATE `departments` SET " +
                                  $"DepartmentName = '{department.DepartmentName}' " +
                                  $"WHERE DepartmentId = {department.ID}; " +
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
                using (MySqlCommand myCommand = new MySqlCommand("UpdateDepartment", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(new { success = $"Updated {department.ID} to {department.DepartmentName} in Departments Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }

        public JsonResult DeleteDepartment(Department department)
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS DeleteDepartment";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"CREATE PROCEDURE DeleteDepartment() " +
                                  $"BEGIN DELETE FROM `departments` " +
                                  $"WHERE DepartmentId = {department.ID}; " +
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
                using (MySqlCommand myCommand = new MySqlCommand("DeleteDepartment", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(new { success = $"Deleted {department.DepartmentName} from Departments Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }
        #endregion Departments

        #region Employees
        public JsonResult GetEmployees()
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS GetEmployees";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE PROCEDURE GetEmployees() " +
                                  "BEGIN " +
                                  "SELECT employeeID, employeeName, employeeDepartment, employeeJoinDate, " +
                                  "employeePhotoFile " +
                                  "FROM `employees`; " +
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
                using (MySqlCommand myCommand = new MySqlCommand("GetEmployees", conn))
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

        public JsonResult AddEmployee(Employee employee)
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS AddEmployee";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"CREATE PROCEDURE AddEmployee() " +
                                  $"BEGIN " +
                                  $"INSERT INTO `employees` " +
                                  $"(employeeID, employeeName, employeeDepartment, employeeJoinDate, employeePhotoFile)" +
                                  $"VALUES(" +
                                  $"null, " +
                                  $"'{employee.Name}', " +
                                  $"'{employee.Department}', " +
                                  $"'{employee.JoinDate}', " +
                                  $"'{employee.PhotoFile}' " +
                                  $"); " +
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
                using (MySqlCommand myCommand = new MySqlCommand("AddEmployee", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(new { success = $"Added {employee.Name} to Employees Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }

        public JsonResult UpdateEmployee(Employee employee)
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS UpdateEmployee";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"CREATE PROCEDURE UpdateEmployee() " +
                                  $"BEGIN UPDATE `employees` SET " +
                                  $"employeeName = '{employee.Name}', " +
                                  $"employeeDepartment = '{employee.Department}', " +
                                  $"employeeJoinDate = '{employee.JoinDate}', " +
                                  $"employeePhotoFile = '{employee.PhotoFile}' " +
                                  $"WHERE employeeID = {employee.ID}; " +
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
                using (MySqlCommand myCommand = new MySqlCommand("UpdateEmployee", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(new { success = $"Updated Employee #{employee.ID} in Employees Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }

        public JsonResult DeleteEmployee(Employee employee)
        {
            MySqlConnection conn = new();
            string MySqlDataSource = _configuration["EmployeeAppCon"];
            conn.ConnectionString = MySqlDataSource;
            MySqlCommand cmd = new();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DROP PROCEDURE IF EXISTS DeleteEmployee";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"CREATE PROCEDURE DeleteEmployee() " +
                                  $"BEGIN DELETE FROM `employees` " +
                                  $"WHERE employeeId = {employee.ID}; " +
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
                using (MySqlCommand myCommand = new MySqlCommand("DeleteEmployee", conn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    conn.Close();
                }

                return new JsonResult(new { success = $"Deleted {employee.Name} from Employees Table." });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { error = $"Error {ex.Number}: {ex.Message}" });
            }
        }

        public JsonResult AddEmployeePhoto()
        {
            try
            {
                IFormCollection httpRequest = _contextAccessor.HttpContext.Request.Form;
                IFormFile postedFile = httpRequest.Files[0];
                string newFileName = Regex.Replace(httpRequest["employeeName"], @"\s+", String.Empty);
                string empID = httpRequest["employeeID"];
                string fileName = postedFile.FileName;
                int dotIndex = fileName.LastIndexOf('.');
                string newFileExt = fileName.Substring(dotIndex);
                var physicalPath = _environment.ContentRootPath + "/EmployeePhotos/" + empID + "_" + newFileName + newFileExt;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch
            {
                return new JsonResult("anonymous.png");
            }

        }
        #endregion Employees
    }
}
