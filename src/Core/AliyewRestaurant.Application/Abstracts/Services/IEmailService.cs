namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string body);
}
