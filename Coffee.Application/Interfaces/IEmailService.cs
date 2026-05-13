using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(OrderDto order, string recipientEmail);
    }
}
