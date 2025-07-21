// Controllers/EmployeesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ArifTestApi.Models;

namespace ArifTestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly string _connectionString;

        public EmployeesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // POST: api/Employees
        // Used to add a new employee record to the database.
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is null.");
            }

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // SQL INSERT statement for the Employee table.
                    // EmployeeID is IDENTITY, so it's not included in the INSERT columns.
                    string sql = @"INSERT INTO Employee (FirstName, LastName, Designation, Post, ContactNo, District, PresentAddress, PermanentAddress, EducationQualification, Remarks)
                                   VALUES (@FirstName, @LastName, @Designation, @Post, @ContactNo, @District, @PresentAddress, @PermanentAddress, @EducationQualification, @Remarks);";
                    await db.ExecuteAsync(sql, employee);
                }
                return Ok("Employee added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.
                Console.WriteLine($"Error adding employee: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}