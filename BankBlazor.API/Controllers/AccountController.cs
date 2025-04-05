using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankBlazor.API.Models;

namespace BankBlazor.API.Controllers
{
    // Controller som ska visa  konto och transaktioner som tillhör konto och pengar

    [Route("api/[controller]")]             //Vart jag kan hitta den
    [ApiController]                         // Säger : "Detta är en API Controller
    public class AccountController : ControllerBase
    {
        private readonly BankContext _context; // Privat så bara klassen ska använda
                                               // readonly menar att den bara kan sättas en gång
                                               // BankContext är min koppling till databasen
                                               // _context Variabeln som används i resten av klassen för att komma åt databasen

        public AccountController(BankContext context) // Den här konstruktorn är hjärtat i hur controller får tillgång till databasen.
        {
            _context = context;
        }

        // GET: api/Account/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id) // GetAccount namnet på metoden som ska ta emot id
                                                                    // sjäva metoden är allts som finns inom klammerparenteserna nedan
        {
            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountId == id);
            //    await: Väntar på att databasen ska svara (utan att låsa systemet under tiden)
            //   _context.Accounts: Går till tabellen / entiteten Accounts i databasen.
            //   .Include(a => a.Transactions): Hämtar även alla transaktioner kopplade till kontot. (Så man får med både kontot och dess transaktioner.)
            //   .FirstOrDefaultAsync(...): Hämtar det första kontot som har det här id, eller null om inget hittas.

            if (account == null)
            {
                return NotFound(); // 404
            }
                

            return account;
        }
    }
}
