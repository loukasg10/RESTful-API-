using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MyApiProject.Models;
using MyApiProject.Data;
using MyApiProject.DTOs;


namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDataBase _context;
        public DepartmentController(AppDataBase context)
        {
            _context = context;
        }

        [HttpGet]   //GET--> departments
        public ActionResult<IEnumerable<DepartmentDTO>> GetDepartments()
        {
            var departments = _context.Departments
                .Select(d => new DepartmentDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    OfficeLocation = d.OfficeLocation
                })
                .ToList();
            return Ok(new { message = "Departments found successfully.", data = departments });
        }
        [HttpGet("departmentsDropdown")] //GET--> departments/{id}
        public ActionResult<IEnumerable<DepartmentDropdownDTO>> GetDepDropdown()
        {
            var departments = _context.Departments
                .Select(d => new DepartmentDropdownDTO
                {
                    Name = d.Name,

                })
                .ToList();
            return Ok(new { message = "Departments dropdown found successfully.", data = departments });
        }

        [HttpGet("{id}")] //GET--> departments/{id}
        public ActionResult<DepartmentDTO> GetDepartment(int id)
        {
            var department = _context.Departments
                .Where(d => d.Id == id)
                .Select(department => new DepartmentDTO
                {
                    Id = department.Id,
                    Name = department.Name,
                    OfficeLocation = department.OfficeLocation
                })
                .FirstOrDefault();
            if (department == null)
                return NotFound(new { message = "Department not found." });
            return Ok(new { message = "Department found successfully.", data = department });
        }
        [HttpPost] //POST--> departments
        public ActionResult<DepartmentDTO> CreateNewDepartment(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();

            var departmentDTO = new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,
                OfficeLocation = department.OfficeLocation
            };
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, new { message = "Department created successfully.", data = departmentDTO });
        }

        [HttpPut("{id}")] //PUT--> departments/{id}
        public ActionResult<DepartmentDTO> UpdateDepartment(int id, Department department)
        {
            if (id != department.Id)
                return BadRequest(new { message = "Department ID mismatch." });

            var existingDep = _context.Departments.Find(id);
            if (existingDep == null)
                return NotFound(new { message = "Department not found." });

            existingDep.Name = department.Name;
            existingDep.OfficeLocation = department.OfficeLocation;
            _context.SaveChanges();

            return Ok(new { message = "Department updated successfully.", data = existingDep });

        }
        [HttpDelete("{id}")] //DELETE--> departments/{id}
        public ActionResult<DepartmentDTO> DeleteDepartment(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
                return NotFound(new { message = "Department not found." });
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return Ok(new { message = "Department deleted successfully." });
        }

        [HttpGet("{id}/employees")] //GET--> departments/{id}/employees
        public ActionResult<IEnumerable<EmployeeDTO>> GetDepartmentEmployees(int id)
        {
            var depExist = _context.Departments.Any(d => d.Id == id);
            if (!depExist)
                return NotFound(new { message = "Department not found." });

            var employees = _context.Employees
                .Where(e => e.DepartmentId == id)
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Salary = e.salary,
                    DepartmentId = e.DepartmentId
                })
                .ToList();
            return Ok(new { message = "Employees found successfully.", data = employees });
        }
    }
}