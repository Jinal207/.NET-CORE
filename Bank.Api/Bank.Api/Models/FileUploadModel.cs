using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileUploadExample.Models
{
    public class FileUploadModel
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        [Required]
        public IFormFile File { get; set; }

        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        // Store as Base64 string instead of byte[]
        public string? FileBase64 { get; set; }
    }
}
