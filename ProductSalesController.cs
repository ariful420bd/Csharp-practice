// Controllers/ProductSalesController.cs
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
    public class ProductSalesController : ControllerBase
    {
        private readonly string _connectionString;

        public ProductSalesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // POST: api/ProductSales
        // Used to add a new product sales record to the database.
        [HttpPost]
        public async Task<IActionResult> AddProductSales([FromBody] ProductSales sales)
        {
            if (sales == null)
            {
                return BadRequest("Product sales data is null.");
            }

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // SQL INSERT statement for the ProductSales table.
                    string sql = @"INSERT INTO ProductSales (ProductID, Category, ProductName, Quantity, Unit, Remarks)
                                   VALUES (@ProductID, @Category, @ProductName, @Quantity, @Unit, @Remarks);";
                    await db.ExecuteAsync(sql, sales);
                }
                return Ok("Product sales added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product sales: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}