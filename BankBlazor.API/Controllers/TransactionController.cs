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




        // INSÄTTNING
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
                Type = "Credit",
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












        // UTTAG
        // POST: api/transaction/debit
        [HttpPost("debit")]
        public async Task<ActionResult> Withdraw(int accountId, decimal amount)
        {
            // Skapa account variabel från Accounts tabellen med hjälp av _Context åsen ge första matchande värdet av AccountID som matas in
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);

            // Om kontot inte finns dv.s null
            if (account == null)
            {
                return NotFound("Kontot hittades inte.");
            }

            // Om det inte finns tillräckligt med pengar
            if (account.Balance < amount)
            {

                return BadRequest("Otillräckligt saldo.");
            }

            // Uppdatera saldo
            account.Balance -= amount;

            // Skapa en ny transaction objekt 
            var transaction = new Transaction
            {
                AccountId = accountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Debit",
                Operation = "Uttag",
                Amount = amount,
                Balance = account.Balance,
                Symbol = "",
                Bank = "BankBlazor",
            };

            // Lägg till den nya transaktionen och spara i databasen
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Meddelande till användare
            return Ok("Uttag genomförd.");
        }








        // TRANSFER - TRANSAKTION MELLAN TVÅ KONTON
        [HttpPost("Transfer")]
        public async Task<ActionResult> Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            // Först hantera det som händer om summan är under eller lika med 0 inget ska kunna överföras 
            if (amount <= 0)
            {
                return BadRequest("Beloppet måste vara större än 0.");
            }

            // Skapa variabel för konto som ska ta emot och som ska överföra
            var fromAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == fromAccountId);
            var toAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == toAccountId);

            // Kolla om kontorna ens finns
            if (fromAccount == null || toAccount == null)
            { 
            return NotFound("Ett eller båda konton hittades inte.");
            }

            // Kolla om kontot som ska skicka pengar har tillräckligt med pengar att skicka 
            if (fromAccount.Balance < amount)
            {
                return BadRequest("Otillräckligt saldo på kontot.");
            }

            // Dra pengar från avsändare
            fromAccount.Balance -= amount;

            // Skapa och registrera debit-transaktion (från avsändare) pengar går UT från konto
            var debitTransaction = new Transaction
            {
                AccountId = fromAccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Debit",
                Operation = "Överföring till konto",
                Amount = -amount,
                Balance = fromAccount.Balance,
                Symbol = "Transfer",
                Bank = "",
                Account = toAccountId.ToString()
            };
            // Lägg till transaktionen i databasen
            _context.Transactions.Add(debitTransaction);
            // Lägg till pengar på mottagarens konto
            toAccount.Balance += amount;



            // Skapa och registrera credit-transaktion (till mottagare) pengar GÅR IN i kontot
            var creditTransaction = new Transaction
            {
                AccountId = toAccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Credit",
                Operation = "Överföring från konto",
                Amount = amount,
                Balance = toAccount.Balance,
                Symbol = "Transfer",
                Bank = "",
                Account = fromAccountId.ToString()
            };
            // Lägg till transaktionen i databasen i tabell Transactions
            _context.Transactions.Add(creditTransaction);

            // Spara allt i databasen
            await _context.SaveChangesAsync();

            // Skicka meddelande till mottagaren
            return Ok("Överföring genomförd.");
        }

    }
}
