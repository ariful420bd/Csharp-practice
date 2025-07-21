// Controllers/CustomersController.cs
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
    public class CustomersController : ControllerBase
    {
        private readonly string _connectionString;

        public CustomersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // POST: api/Customers
        // Used to add a new customer record to the database.
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is null.");
            }

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // SQL INSERT statement for the Customer table.
                    string sql = @"INSERT INTO Customer (Name, ContactNo, Address, Reference)
                                   VALUES (@Name, @ContactNo, @Address, @Reference);";
                    await db.ExecuteAsync(sql, customer);
                }
                return Ok("Customer added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}