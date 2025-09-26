using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OtpController : ControllerBase
{
    private readonly OtpService _otpService = new OtpService();

    [HttpPost("send")]
    public async Task<IActionResult> SendOtp([FromForm] string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email is required.");

        var otp = _otpService.GenerateOtp();

        await _otpService.SendOtpAsync(email, otp);

        // You should store OTP in cache or DB for verification (omitted for simplicity)
        return Ok(new { Message = "OTP sent successfully", Otp = otp });
    }
}
