using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // dessa ska man tydligen ha med
using BankBlazor.API.Models; // Den också



// Sköter insättning, uppdaterar kontot med ny balance och sparar i databasen

namespace BankBlazor.API.Controllers
{
    [Route("api/[controller]")] // Själva adressen vart jag ska hitta dvs endpoint
    [ApiController] // Visar att det är en controller
    public class TransactionController : ControllerBase  //  skapar en controller, alltså en klass som hanterar API-anrop.
                                                         //  : ControllerBase betyder att klassen ärver funktioner från.NET:s egna controllerklass.


    {
        private readonly BankContext _context; // Du skapar bara en variabel _context av BankContext objektet här du DEKLARERAR INTE. För det ska göras i konstruktorn

        public TransactionController(BankContext context) // Konstruktor som gör så att vi kan använda _context och connecta t databasen i klassen
        {
            _context = context;
        }


        // POST: api/transaction/deposit
        [HttpPost("credit")] 
        // HttpPost: Den här metoden ska anropas med en HTTP POST-förfrågan (istället för t.ex. GET, PUT eller DELETE).
        // "deposit": Det är den sista delen av URL:en som används för att nå just den här metoden.
        public async Task<ActionResult> Deposit(int accountId, decimal amount) // Task lovar att resturnera och ActionResult är en klass inbyggd i C#
                                                                               // tar emot 2 värden en accountId och den andra amount
                                                                              // använd inte double för den är snabb men inte lika säker som decimal
                                                                              // Tydligen används det mest i bankappar E handel å sånt
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            // var account = jag skapar en variabel för det kontot jag hittar eller för null värdet om jag inte hittar
            // _context.Accounts = här använder jag min _context variabel som är kopplad till databasen och pekar på Accounts tabellen
            // FirstOrDefault = är den vanliga LINQ metoden men för asynkrontkod och den säger:
            // antingen returnera första matchade kontot i Accounts tabellen med id:et som matades eller ge en null

            if (account == null)
            {
                return NotFound("Kontot hittades inte."); // Hade det varit tom i parentesen hade jag fått 404 error
            }
               

            account.Balance += amount;

            var transaction = new Transaction // Transaction finns i models mappen och kom när jag ladda ner databasen
                                              // Alla propertys i klammerparentesen måste vara med för de fanns i Transaction klassen
            {
                AccountId = accountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Deposit",
                Operation = "Insättning",
                Amount = amount,
                Balance = account.Balance,
                Symbol = "",
                Bank = "BankBlazor",
               
            };

            _context.Transactions.Add(transaction);  // Lägger till den nya transaktionen i Transations klassen
            await _context.SaveChangesAsync(); // Sparar alla men i Assynk verison. Precis som vanliga men lägg bara till await och Async

            return Ok("Insättning genomförd."); //Meddelande till användaren
        }
    }
}
