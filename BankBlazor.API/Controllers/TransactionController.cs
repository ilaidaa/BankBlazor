using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBlazor.API.Controllers
{
    [Route("api/[controller]")] // Själva adressen vart jag ska hitta dvs endpoint
    [ApiController] // Visar att det är en controller
    public class TransactionController : ControllerBase  // ärver från ControllerBase
    {
    }
}
