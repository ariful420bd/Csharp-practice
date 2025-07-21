// Controllers/ProductPurchasesController.cs
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
    public class ProductPurchasesController : ControllerBase
    {
        private readonly string _connectionString;

        public ProductPurchasesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // POST: api/ProductPurchases
        // Used to add a new product purchase record to the database.
        [HttpPost]
        public async Task<IActionResult> AddProductPurchase([FromBody] ProductPurchase purchase)
        {
            if (purchase == null)
            {
                return BadRequest("Product purchase data is null.");
            }

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // SQL INSERT statement for the ProductPurchase table.
                    string sql = @"INSERT INTO ProductPurchase (ProductID, Category, ProductName, Quantity, Unit, Remarks)
                                   VALUES (@ProductID, @Category, @ProductName, @Quantity, @Unit, @Remarks);";
                    await db.ExecuteAsync(sql, purchase);
                }
                return Ok("Product purchase added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product purchase: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}