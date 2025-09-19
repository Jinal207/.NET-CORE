using System.ComponentModel.DataAnnotations;

namespace Bank.Api.Models
{
    public class updateBankDetailsDTO
    {
        [Required]
        public string AccountName { get; set; } = string.Empty;

        public double Balance { get; set; } = 0.0;  // Public so EF can map it

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
