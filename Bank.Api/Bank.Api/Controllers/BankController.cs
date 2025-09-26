using Azure;
using Bank.Api.Data;
using Bank.Api.Models;
using Bank.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [LogActionFilterClass]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankController(ILogger<BankController> _logger, IBankRepository _bankRepository) : ControllerBase
    {
        // SECOND WAY TO INJECT......
        //private readonly ApplicationDbContext _dbContext;
        //private readonly ILogger<BankController> _logger;

        //// Constructor for dependency injection
        //public BankController(ApplicationDbContext dbContext, ILogger<BankController> logger)
        //{
        //    _dbContext = dbContext;
        //    _logger = logger;
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllBankDetails()
        {
            var allBankDetails = await _bankRepository.GetAllAsync();
            return Ok(allBankDetails);
        }

        [HttpGet]
        [Route("{accountId:guid}")]
        public async Task<IActionResult> GetBankDetailsById(Guid accountId)
        {
            var bankDetails = await _bankRepository.GetByIdAsync(accountId);
            if (bankDetails == null) return NotFound();
            return Ok(bankDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddBankDetails(BankDetailsDTO dto)
        {
            var bankDetails = new BankDetails
            {
                AccountName = dto.AccountName,
                Balance = dto.Balance
            };

            await _bankRepository.AddAsync(bankDetails);
            return Ok(bankDetails);
        }

        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult> UpdateBankDetails(Guid accountId, updateBankDetailsDTO dto)
        {
            var bankDetails = await _bankRepository.GetByIdAsync(accountId);
            if (bankDetails == null) return NotFound();

            bankDetails.AccountName = dto.AccountName;
            bankDetails.Balance = dto.Balance;

            await _bankRepository.UpdateAsync(bankDetails);
            return Ok(bankDetails);
        }

        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteBankDetails(Guid accountId)
        {
            await _bankRepository.DeleteAsync(accountId);
            return Ok("Deleted");
        }

        [HttpGet("search/{searchText}")]
        public async Task<IActionResult> GetFilteredData(string searchText)
        {
            var results = await _bankRepository.SearchAsync(searchText);
            return Ok(results);
        }

        [HttpPatch("{accountId:guid}")]
        public async Task<IActionResult> PatchBankDetails(Guid accountId, JsonPatchDocument<BankDetails> patchDoc)
        {
            if (patchDoc == null) return BadRequest("Document is null");

            var bankDetails = await _bankRepository.GetByIdAsync(accountId);
            if (bankDetails == null) return NotFound();

            patchDoc.ApplyTo(bankDetails, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _bankRepository.UpdateAsync(bankDetails);
            return Ok(bankDetails);
        }


        [HttpGet("hidden-action")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test-logging")]
        public IActionResult TestLogging()
        {
            _logger.LogInformation("TestLogging endpoint called."); // Info
            _logger.LogWarning("This is a warning example."); // Warning
            _logger.LogError("This is an error example."); // Error

            try
            {
                int x = 0;
                int y = 10 / x; // will throw
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred in TestLogging."); // Exception logging
            }

            return Ok("Logging test completed!");
        }
    }
}


