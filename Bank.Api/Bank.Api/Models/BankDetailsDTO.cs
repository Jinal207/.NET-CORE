using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Api.Models
{
    public class BankDetailsDTO
    {

        [Required(ErrorMessage = "AccountName is required")]
        [StringLength(10, ErrorMessage = "AccountName cannot be longer than 10 characters")]

        public string AccountName { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be non-negative")]
        public double Balance { get; set; } = 0.0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}