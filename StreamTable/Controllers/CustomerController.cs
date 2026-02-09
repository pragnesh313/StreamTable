using StreamTable.Data;
using StreamTable.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // Remove try-catch to allow yield return in async iterator
            var query = context.Customers.AsNoTracking().AsAsyncEnumerable();

            //2.Iterate and yield each row
            await foreach (var customer in query)
            {
                // Optional: simulate your slow response
                await Task.Delay(10);

                yield return customer;
            }

        }
    }
}