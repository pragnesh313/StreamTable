using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StreamTable.Data;
using StreamTable.Models;
using System.Data;

namespace StreamTable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CustomerController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async IAsyncEnumerable<Customer> StreamCustomers()
        {
            var query = context.Customers.AsNoTracking().AsAsyncEnumerable();

            await foreach (var customer in query)
            {
                await Task.Delay(10);
                yield return customer;
            }

        }
        [HttpGet("sql")]
        public async IAsyncEnumerable<Customer> StreamCustomersSQL()
        {
            const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=StreamTableDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            const string sql = @" SELECT * FROM Customers";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult | CommandBehavior.CloseConnection);
            while (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    Id = reader["Id"] as int? ?? 0,
                    Contact = reader["Contact"] as string ?? "",
                    FirstName = reader["FirstName"] as string ?? "",
                    LastName = reader["LastName"] as string ?? "",
                    Email = reader["Email"] as string ?? "",
                    DateOfBirth = reader["DateOfBirth"] as DateTime? ?? default
                };
                yield return customer;
            }
        }

    }
}