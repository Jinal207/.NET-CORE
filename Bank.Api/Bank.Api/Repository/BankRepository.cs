using Bank.Api.Data;
using Bank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Repository
{
    public class BankRepository : IBankRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BankRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BankDetails>> GetAllAsync()
        {
            return await _dbContext.BankDetails.ToListAsync();
        }

        public async Task<BankDetails?> GetByIdAsync(Guid accountId)
        {
            return await _dbContext.BankDetails.FindAsync(accountId);
        }

        public async Task AddAsync(BankDetails bankDetails)
        {
            await _dbContext.BankDetails.AddAsync(bankDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(BankDetails bankDetails)
        {
            _dbContext.BankDetails.Update(bankDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid accountId)
        {
            var bankDetails = await _dbContext.BankDetails.FindAsync(accountId);
            if (bankDetails != null)
            {
                _dbContext.BankDetails.Remove(bankDetails);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BankDetails>> SearchAsync(string searchText)
        {
            return await _dbContext.BankDetails
                .Where(item =>
                    item.AccountId.ToString().Contains(searchText) ||
                    item.AccountName.Contains(searchText) ||
                    item.Balance.ToString().Contains(searchText) ||
                    item.CreatedDate.ToString().Contains(searchText)
                )
                .ToListAsync();
        }
    }
}
