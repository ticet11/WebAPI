using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
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
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.GetEmployees());
        }

        [HttpPost]
        public JsonResult AddEmployee(Employee employee)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.AddEmployee(employee));
        }

        [HttpPut]
        public JsonResult UpdateEmployee(Employee employee)
        {
            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.UpdateEmployee(employee));
        }

        [HttpDelete]
        public JsonResult DeleteEmployee(Employee employee)
        {
            if (employee.PhotoFile != "default.png")
            {
                int dotIndex = employee.PhotoFile.IndexOf('.');
                string photoFileName = employee.PhotoFile.Substring(0, dotIndex);
                string empPhotoPath = _environment.ContentRootPath + "/EmployeePhotos/";
                DirectoryInfo dirInfo = new DirectoryInfo(empPhotoPath);
                FileInfo[] filesList = dirInfo.GetFiles($"{photoFileName}*");
                if (filesList.Length > 0)
                {
                    foreach (FileInfo file in filesList)
                    {
                        file.Delete();
                    }
                }
            }

            StoredProcs storedProcs = new StoredProcs(_configuration);
            return new JsonResult(storedProcs.DeleteEmployee(employee));
        }

        [Route("AddEmployeePhoto")]
        [HttpPost]
        public JsonResult AddEmployeePhoto()
        {
            try
            {
                IFormCollection httpRequest = _httpContextAccessor.HttpContext.Request.Form;
                IFormFile postedFile = httpRequest.Files[0];
                string fileName = postedFile.Name;
                string newFileName = postedFile.FileName;
                string empPhotoPath = _environment.ContentRootPath + "/EmployeePhotos/";
                string physicalPath = empPhotoPath + newFileName;

                if (fileName != "default.png" && newFileName != "default.png")
                {
                    int dotIndex = postedFile.FileName.LastIndexOf('.');
                    DirectoryInfo dirInfo = new DirectoryInfo(empPhotoPath);
                    FileInfo[] filesList = dirInfo.GetFiles($"{newFileName.Substring(0, dotIndex)}*");
                    if (filesList.Length > 0)
                    {
                        foreach (FileInfo file in filesList)
                        {
                            file.Delete();
                        }
                    }

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                }

                return new JsonResult(fileName);
            }
            catch
            {
                return new JsonResult("default.png");
            }

        }
    }
}
