using Bank.Api.Models;

namespace Bank.Api.Repository
{
    public interface IBankRepository
    {
        Task<IEnumerable<BankDetails>> GetAllAsync();
        Task<BankDetails?> GetByIdAsync(Guid accountId);
        Task AddAsync(BankDetails bankDetails);
        Task UpdateAsync(BankDetails bankDetails);
        Task DeleteAsync(Guid accountId);
        Task<IEnumerable<BankDetails>> SearchAsync(string searchText);
    }
}
