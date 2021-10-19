
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentController(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.GetDepartments());
        }

        [HttpPost]
        public JsonResult AddDepartment(Department department)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.AddDepartment(department));
        }

        [HttpPut]
        public JsonResult UpdateDepartment(Department department)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.UpdateDepartmentName(department));
        }

        [HttpDelete]
        public JsonResult DeleteDepartment(Department department)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration, _environment, _httpContextAccessor);
            return new JsonResult(storedProcs.DeleteDepartment(department));
        }
    }
}
