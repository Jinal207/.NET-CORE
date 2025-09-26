using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

public class OtpService
{
    private readonly string _smtpServer = "smtp.gmail.com"; // Change if needed
    private readonly int _smtpPort = 465; // SSL port
    private readonly string _smtpUser = "chodavadiyaar@gmail.com"; // Your SMTP email
    private readonly string _smtpPass = "your_email_password_or_app_password"; // SMTP password or app password

    public async Task SendOtpAsync(string toEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("MyApp", _smtpUser));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Your OTP Code";
        message.Body = new TextPart("plain")
        {
            Text = $"Your OTP code is: {otp}. It is valid for 5 minutes."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, true);
        await client.AuthenticateAsync(_smtpUser, _smtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public string GenerateOtp(int length = 6)
    {
        var random = new Random();
        var otp = "";
        for (int i = 0; i < length; i++)
            otp += random.Next(0, 10).ToString();
        return otp;
    }
}
