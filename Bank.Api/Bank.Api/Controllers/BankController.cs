using Bank.Api.Data;
using Bank.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public BankController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult getAllBankDetails()
        {
            var allBankDetails = dbContext.BankDetails.ToList();
            return Ok(allBankDetails);
        }

        [HttpPost]
        public IActionResult addBankDetails(BankDetailsDTO bankDetailsDTO)
        {
            var bankDetails = new BankDetails()
            {
                AccountName = bankDetailsDTO.AccountName,
                Balance = bankDetailsDTO.Balance,
            };
            dbContext.BankDetails.Add(bankDetails);
            dbContext.SaveChanges();
            return Ok(bankDetails);
        }

        [HttpGet]
        [Route("{AccountId:guid}")]
        public IActionResult getBankDetailsById(Guid AccountId)
        {
            var fethedData = dbContext.BankDetails.Find(AccountId);
            if (fethedData is null)
            {
                return NotFound();
            }
            return Ok(fethedData);
        }

        [HttpPut]
        [Route("{AccountId:guid}")]
        public IActionResult updateBankDetails(Guid AccountId, updateBankDetailsDTO updateBankDetails)
        {
            var fetchedData = dbContext.BankDetails.Find(AccountId);
            if (fetchedData is null)
            {
                return NotFound();
            }
            fetchedData.AccountName = updateBankDetails.AccountName;
            fetchedData.Balance = updateBankDetails.Balance;
            dbContext.SaveChanges();
            return Ok(updateBankDetails);
        }

        [HttpDelete]
        [Route("{AccountId:guid}")]
        public IActionResult deleteBankDetails(Guid AccountId)
        {
            var fetchedData = dbContext.BankDetails.Find(AccountId);
            if (fetchedData is null)
            {
                return NotFound();
            }
            dbContext.BankDetails.Remove(fetchedData);
            dbContext.SaveChanges();
            return Ok("Deleted");
        }
    }

}

// [FromRoute] - Binds values from the URL route parameters to action method parameters
// [FromQuery] - Binds values from the query string (?key=value) to action method parameters
// [FromBody] - Binds values from the request body (usually JSON) to complex objects or parameters
// [FromForm] - Binds values from form data (x-www-form-urlencoded or multipart/form-data)
// [FromHeader] - Binds values from HTTP headers to parameters
// [FromServices] - Injects registered services from dependency injection into action method parameters
// Complex type binding - Automatically maps JSON or query string to object properties
// List/Array binding - Supports binding arrays or lists from JSON body or query string
// Model binding is automatic - ASP.NET Core automatically converts types and populates parameters
// Works with simple and complex types - int, string, Guid, objects, nested objects, lists
// Integration with validation - Works seamlessly with Data Annotations and ModelState validation

