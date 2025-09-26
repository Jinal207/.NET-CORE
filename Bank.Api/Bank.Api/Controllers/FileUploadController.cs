using Bank.Api.Data;
using FileUploadExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public FileUploadController(ApplicationDbContext db)
        {
            _dbContext = db;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("No file selected.");

            // Allowed extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            long maxFileSize = 5 * 1024 * 1024; // 5 MB

            var extension = Path.GetExtension(model.File.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return BadRequest($"File type not allowed: {model.File.FileName}");

            if (model.File.Length > maxFileSize)
                return BadRequest($"File too large (max 5 MB): {model.File.FileName}");

            using (var ms = new MemoryStream()) 
            {
                await model.File.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                model.FileBase64 = Convert.ToBase64String(fileBytes);
            }

            model.FileName = model.File.FileName;
            model.ContentType = model.File.ContentType;

            _dbContext.UploadedFiles.Add(model);
            await _dbContext.SaveChangesAsync();

            return Ok(new { model.Id, model.FileName, model.ContentType });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileBase64(int id)
        {
            var file = await _dbContext.UploadedFiles.FindAsync(id);
            if (file == null)
                return NotFound();

            // Return Base64 string
            return Ok(new
            {
                file.Id,
                file.FileName,
                file.ContentType,
                Base64Data = file.FileBase64
            });
        }
    }
}
