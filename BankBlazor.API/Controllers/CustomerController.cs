using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankBlazor.API.Models;

namespace BankBlazor.API.Controllers
{
    // Controller som sköter Kunder

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly BankContext _context;

        public CustomerController(BankContext context)
        {
            _context = context;
        }

        // GET: api/customer Ska hämta alla kunder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() // Hämtar listan av kunder
        {
            return await _context.Customers.ToListAsync(); // returnerar listan av kunder
        }

        // GET: api/customer/1 ska hämta kund efter ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            // Kolla i Databasen första bästa som matchar
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            // Hantera om det inte finns
            if (customer == null)
            {
                return NotFound(); //  404 Not Found.
            }
              
            // returnera customer om det finns
            return customer;
        }
    }
}
