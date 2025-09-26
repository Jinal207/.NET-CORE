using System.ComponentModel.DataAnnotations;

namespace Bank.Api.Models
{
    public class UserAccounts
    {
        [Key]
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
