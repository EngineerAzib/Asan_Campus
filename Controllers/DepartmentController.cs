using Asan_Campus.Model;
using Asan_Campus.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using static Asan_Campus.Model.InputClass;
using static Asan_Campus.Model.OutputClass; 

namespace Asan_Campus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly ImplementationSevices _service;
        public DepartmentController(ApplicationDbContext context, ImplementationSevices service )
        {
            _context = context;
            _service = service;
        }

        [HttpGet("GetDepartment")]
        public IActionResult GetDepartment()
        {
            
            var res = _context.Departments.Select(department => new
            {
                id = department.Id,
                Name = department.DepartmentName,
                TotalStudents = department.MaleStudent+ department.FemaleStudent,
                GenderStats = new
                {
                    Boys = new
                    {
                        Percentage = _service.getPercentage(department.MaleStudent, department.MaleStudent + department.FemaleStudent),
                        Count = department.MaleStudent
                    },
                   
                    Girls = new
                    {
                        Percentage = _service.getPercentage(department.FemaleStudent, department.MaleStudent + department.FemaleStudent),
                        Count = department.FemaleStudent
                    }
                }
            }).ToList();

            return Ok(res);
        }
        [HttpGet("GetDepartmentList")]

        public IActionResult GetDepartmentList()
        {
            var res = _context.Departments.Select(x => new
            {
                id = x.Id,
                departmentName = x.DepartmentCode + "-" + x.DepartmentName
            });
            return Ok(res);
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment(DepartmentDT0 request)
        {
            var department = new Department()
            {

                DepartmentName = request.DepartmentName,
                DepartmentCode=request.DepartmentCode,
                MaleStudent=request.MaleStudent,
                FemaleStudent=request.FemaleStudent
            };
            _context.Add(department);
            _context.SaveChanges();
            return Ok("Add Department");
        }
        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment(Department request,int id)
        {
            var depId = _context.Departments.Find(id);
            depId.DepartmentName=request.DepartmentName;
            depId.MaleStudent = request.MaleStudent;
            depId.FemaleStudent = request.FemaleStudent;
            _context.SaveChanges();
            return Ok("Update Student");


        }
        [HttpDelete("DeleteDepartment")]
        public IActionResult DeleteDepartment (int id)
        {
            var depId = _context.Departments.Find(id);
            _context.Remove(depId);
            _context.SaveChanges();
            return Ok("Delete Successfully");
        }
        



    }
}
