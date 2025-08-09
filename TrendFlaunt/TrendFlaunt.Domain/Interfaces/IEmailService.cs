using MimeKit;
using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Interfaces;

public interface IEmailService
{
    void SendEmail(MessageResponse message);
}
