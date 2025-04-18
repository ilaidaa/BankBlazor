﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankBlazor.API.Models;

namespace BankBlazor.API.Controllers
{
    // Controller som sköter Kunder, dvs visar alla kunder men kan också visa en kund om man skriver in dess id

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
     
        private readonly BankContext _context; // Privat så bara klassen ska använda
                                               // readonly menar att den bara kan sättas en gång
                                               // BankContext är min koppling till databasen
                                               // _context Variabeln som används i resten av klassen för att komma åt databasen

        public CustomerController(BankContext context) // Den här konstruktorn är hjärtat i hur controller får tillgång till databasen.
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
        public async Task<ActionResult<Customer>> GetCustomer(int id)  // Meyoden getCustomer skapas den tar emot id
                                                                       // sjäva metoden är allts som finns inom klammerparenteserna nedan
        {
            // Kolla i Databasen första bästa som matchar
            var customer = await _context.Customers
                .Include(c => c.Dispositions) // DENNA RAD LAS TILL EFTER, för att utan dessa så får man bara tillbaka kunden, inte konton relaterade till kunden
        .ThenInclude(d => d.Account) // DENNA RAD LAS TILL EFTER
                                     // Kund -> disposition -> konto. Därför måste vi ha med Dispostion för det är mellanhandel
                                     // Säger "Jag vill ha med de Dispositions som hör till den här kunden"
                                     // ThenInclude säger "För varje sån länk (disposition) som vi har hämtat, hämta även det konto som den pekar på"

                .FirstOrDefaultAsync(c => c.CustomerId == id);
            //    await: Väntar på att databasen ska svara (utan att låsa systemet under tiden)
            //   _context.Accounts: Går till tabellen / entiteten Accounts i databasen.
            //   .FirstOrDefaultAsync(...): Hämtar det första kunden som har det här id, eller null om inget hittas.

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
