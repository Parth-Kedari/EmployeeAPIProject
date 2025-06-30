using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();

            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeedto)
        {
            if (addEmployeedto == null || string.IsNullOrEmpty(addEmployeedto.Name) || string.IsNullOrEmpty(addEmployeedto.Email))
            {
                return BadRequest("Invalid employee data.");
            }
            var employeeEntity = new Employee
            {
                Name = addEmployeedto.Name,
                Email = addEmployeedto.Email,
                Phone = addEmployeedto.Phone,
                Salary = addEmployeedto.Salary
            };
            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetAllEmployees), new { id = employeeEntity.Id }, employeeEntity);
        }

        [HttpPut] 
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, AddEmployeeDto addEmployeedto)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null) {
                return NotFound("Invalid Employee Data");
            }

            employee.Name = addEmployeedto.Name;
            employee.Email = addEmployeedto.Email;
            employee.Phone = addEmployeedto.Phone;
            employee.Salary = addEmployeedto.Salary;

            dbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id) { 
            var employee = dbContext.Employees.Find(id);
            if (employee == null) {
                return NotFound("Employee not found");
            }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok($"Employee with ID {id} having Name {employee.Name} deleted successfully.");
        }
    }
}
