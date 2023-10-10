using SmartCardService.Models;

namespace SmartCardService.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);

    }
}
