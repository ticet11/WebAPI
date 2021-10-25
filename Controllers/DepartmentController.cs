using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.GetDepartments());
        }

        [HttpPost]
        public JsonResult AddDepartment(Department department)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.AddDepartment(department));
        }

        [HttpPut]
        public JsonResult UpdateDepartment(Department department)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.UpdateDepartmentName(department));
        }

        [HttpDelete]
        public JsonResult DeleteDepartment(Department department)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.DeleteDepartment(department));
        }
    }
}
