using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.GetEmployees());
        }

        [HttpPost]
        public JsonResult AddEmployee(Employee employee)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.AddEmployee(employee));
        }

        [HttpPut]
        public JsonResult UpdateEmployee(Employee employee)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.UpdateEmployee(employee));
        }

        [HttpDelete]
        public JsonResult DeleteEmployee(Employee employee)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.DeleteEmployee(employee));
        }

        [Route("AddEmployeePhoto")]
        [HttpPost]
        public JsonResult AddEmployeePhoto()
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.AddEmployeePhoto());
        }
    }
}
