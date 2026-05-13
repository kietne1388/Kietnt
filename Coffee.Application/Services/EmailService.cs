using System.Net;
using System.Net.Mail;
using System.Text;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FastFood.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOrderConfirmationAsync(OrderDto order, string recipientEmail)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var mailServer = emailSettings["MailServer"] ?? "smtp.gmail.com";
            var mailPort = int.Parse(emailSettings["MailPort"] ?? "587");
            var senderEmail = emailSettings["SenderEmail"] ?? "";
            var senderPassword = emailSettings["SenderPassword"] ?? "";
            var senderName = "Thư Từ FastFood";

            if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
                return;

            var subject = $"FastFood - Xác Nhận Đơn Hàng #{order.OrderCode}";
            var body = BuildOrderEmailBody(order);

            using var smtpClient = new SmtpClient(mailServer, mailPort)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(recipientEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log error but don't fail the order
                Console.WriteLine($"[EmailService] Failed to send email: {ex.Message}");
            }
        }

        private string BuildOrderEmailBody(OrderDto order)
        {
            var sb = new StringBuilder();
            sb.Append(@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 20px; }
        .container { max-width: 600px; margin: 0 auto; background: white; border-radius: 16px; overflow: hidden; box-shadow: 0 5px 30px rgba(0,0,0,0.12); }
        .header { background: linear-gradient(135deg, #C62828, #E53935); color: white; padding: 36px 30px; text-align: center; }
        .header h1 { margin: 0; font-size: 26px; font-weight: 700; letter-spacing: 0.5px; }
        .header p { margin: 8px 0 0; opacity: 0.88; font-size: 15px; }
        .divider-bar { height: 4px; background: linear-gradient(90deg, #E53935, #FF7043, #E53935); }
        .content { padding: 30px; }
        .order-info { background: #FFF5F5; border-left: 4px solid #E53935; border-radius: 8px; padding: 18px 20px; margin-bottom: 24px; }
        .order-info p { margin: 6px 0; color: #7B1A1A; font-size: 14px; }
        .order-info p strong { color: #C62828; min-width: 110px; display: inline-block; }
        .section-title { color: #1F2937; font-size: 16px; font-weight: 700; margin: 0 0 12px; padding-bottom: 8px; border-bottom: 2px solid #F3F4F6; }
        table { width: 100%; border-collapse: collapse; margin: 0; }
        th { background: #F9FAFB; padding: 11px 12px; text-align: left; font-weight: 700; color: #374151; font-size: 13px; border-bottom: 2px solid #E5E7EB; }
        th.text-right { text-align: right; }
        td { padding: 12px; border-bottom: 1px solid #F3F4F6; color: #4B5563; font-size: 14px; }
        .text-right { text-align: right; }
        .total-row td { background: #FFF5F5; font-weight: 700; color: #C62828; font-size: 16px; padding: 14px 12px; border-bottom: none; }
        .badge { display: inline-block; background: #E53935; color: white; padding: 2px 7px; border-radius: 4px; font-size: 11px; font-weight: 600; margin-right: 6px; letter-spacing: 0.3px; }
        .footer { background: #1F2937; color: #9CA3AF; padding: 24px 20px; text-align: center; font-size: 13px; line-height: 1.8; }
        .footer strong { color: #F4F4F4; }
        .footer a { color: #EF4444; text-decoration: none; }
        .footer .hotline { color: #D1D5DB; font-size: 13px; margin-top: 4px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Thư Từ FastFood</h1>
            <p>Xác nhận đơn hàng thành công!</p>
        </div>
        <div class='divider-bar'></div>
        <div class='content'>
            <div class='order-info'>
                <p><strong>Mã đơn hàng:</strong> " + order.OrderCode + @"</p>
                <p><strong>Ngày đặt:</strong> " + order.CreatedAt.ToString("dd/MM/yyyy HH:mm") + @"</p>
                <p><strong>Khách hàng:</strong> " + order.UserName + @"</p>
                <p><strong>Địa chỉ:</strong> " + order.Address + @"</p>
            </div>

            <p class='section-title'>Chi tiết đơn hàng</p>
            <table>
                <thead>
                    <tr>
                        <th>Sản phẩm</th>
                        <th class='text-right'>Đơn giá</th>
                        <th class='text-right'>SL</th>
                        <th class='text-right'>Thành tiền</th>
                    </tr>
                </thead>
                <tbody>");

            foreach (var item in order.OrderItems)
            {
                var isCombo = item.ComboId.HasValue && item.ComboId > 0;
                var badge = isCombo ? "<span class='badge'>COMBO</span>" : "";
                sb.Append($@"
                    <tr>
                        <td>{badge}{item.ProductName}</td>
                        <td class='text-right'>{item.Price:N0}đ</td>
                        <td class='text-right'>{item.Quantity}</td>
                        <td class='text-right'>{item.Subtotal:N0}đ</td>
                    </tr>");
            }

            sb.Append($@"
                    <tr class='total-row'>
                        <td colspan='3'><strong>Tổng cộng</strong></td>
                        <td class='text-right'><strong>{order.TotalAmount:N0}đ</strong></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class='footer'>
            <p>Cảm ơn bạn đã đặt hàng tại <strong>FastFood</strong>!</p>
            <p>Đơn hàng sẽ được giao trong vòng 30 phút.</p>
            <p class='hotline'>Hotline: 1900-FASTFOOD &nbsp;|&nbsp; <a href='mailto:{_configuration.GetSection("EmailSettings")["SenderEmail"]}'>{_configuration.GetSection("EmailSettings")["SenderEmail"]}</a></p>
        </div>
    </div>
</body>
</html>");

            return sb.ToString();
        }
    }
}
