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
    [Route("employees")]

    public class EmployeeController : ControllerBase
    {
        private readonly AppDataBase _context;
        public EmployeeController(AppDataBase context)
        {
            _context = context;
        }
        [HttpGet]   //GET--> employees
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            var employees = _context.Employees
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


        [HttpGet("{id}")] //GET--> employees/{id}
        public ActionResult<EmployeeDTO> GetEmployee(int id)
        {
            var employee = _context.Employees
                .Where(e => e.Id == id)
                .Select(employee => new EmployeeDTO
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Salary = employee.salary,
                    DepartmentId = employee.DepartmentId
                })
                .FirstOrDefault();
            if (employee == null)
                return NotFound(new { message = "Employee not found." });
            return Ok(new { message = "Employee found successfully.", data = employee });
        }

        [HttpPost] //POST--> employees
        public ActionResult<EmployeeDTO> CreateNewEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();

            var employeeDTo = new EmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Salary = employee.salary,
                DepartmentId = employee.DepartmentId
            };

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, new

            {
                message = "Employee created successfully.",
                data = employeeDTo
            });
        }
        [HttpPut("{id}")] //PUT--> employees/{id}
        public ActionResult<EmployeeDTO> UpdateEmployee(int id, Employee update_employee)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();


            employee.FirstName = update_employee.FirstName;
            employee.LastName = update_employee.LastName;
            employee.Email = update_employee.Email;
            employee.salary = update_employee.salary;
            employee.DepartmentId = update_employee.DepartmentId;
            _context.SaveChanges();

            return Ok(new { message = "Employee updated successfully." });
        }

        [HttpDelete("{id}")] //DELETE--> employees/{id}
        public ActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.FirstOrDefault(employee => employee.Id == id);
            if (employee == null)
                return NotFound(new { message = "Employee not found." });
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return Ok(new { message = "Employee deleted successfully." });
        }

        [HttpGet("highest-salary")] //GET--> employees/highest-salary
        public ActionResult<IEnumerable<EmployeeDTO>> GetHighestSalary([FromQuery] int top = 2)

        {
            var employees = _context.Employees
                .OrderByDescending(e => e.salary)
                .Take(top)
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
        [HttpGet("average-salary/{departmentId}")] //GET--> employees/average-salary/{departmentId}
        public ActionResult GetAverageSalary(int departmentId)
        {
            var empInDep = _context.Employees
                .Where(e => e.DepartmentId == departmentId);

            if (!empInDep.Any())
                return NotFound(new { message = $"No employees found in department {departmentId}" });

            var avgSalary = empInDep.Average(e => e.salary);
            return Ok(new { message = $"Average salary in department {departmentId} is {avgSalary}" });

        }
        [HttpGet("salary-range")]
        public ActionResult<IEnumerable<EmployeeDTO>> GetSalaryRange([FromQuery] decimal min, [FromQuery] decimal max)
        {
            if (min > max || min < 0 || max < 0)
                return BadRequest(new { message = "Invalid salary range." });

            var employees = _context.Employees
                .Where(e => e.salary >= min && e.salary <= max)
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
            return Ok(new { message = "Range found successfully.", data = employees });
        }


    }
}