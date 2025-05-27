using Service.Models.EmailModels;

namespace Service.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(CancellationToken cancellationToken, EmailModel emailRequest);
    }
}