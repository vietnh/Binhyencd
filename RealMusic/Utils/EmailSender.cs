using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using RealMusic.Db;
using System;

namespace RealMusic.Utils
{
    public static class EmailSender
    {
        const string RealMusicEmail = "nhieuhan90@gmail.com";
        const string SenderEmail = "cdbinhyen@gmail.com";
        const string SenderPassword = "25HangChuoi";

        public static void CreateOrderEmail(Order order, IMapStore data)
        {
            data.RefreshTransaction();
            var note = string.IsNullOrEmpty(order.Note) ? "Không có" : order.Note;
            var timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var createdDate = TimeZoneInfo.ConvertTimeFromUtc(order.CreatedDate, timezoneInfo);

            var builder = new StringBuilder();

            builder.Append("THÔNG TIN ĐƠN HÀNG:").AppendLine();
            builder.Append($"   Mã số đơn hàng: {order.OrderNumber}").AppendLine();
            builder.Append($"   Địa chỉ: {order.Address}").AppendLine();
            builder.Append($"   Số điện thoại: {order.Phone}").AppendLine();
            builder.Append($"   Ghi chú: {note}").AppendLine();
            builder.Append($"   Ngày tạo: {createdDate.ToShortDateString()} {createdDate.ToString("HH:mm")}").AppendLine().AppendLine();
            builder.Append("THÔNG TIN ĐĨA:").AppendLine();

            foreach (var itemId in order.OrderItemIds)
            {
                OrderItem orderItem;
                if (!data.TryGetOrderItem(itemId, out orderItem))
                    return;
                Album album;
                if (!data.TryGetAlbum(orderItem.AlbumId, out album))
                    return;

                builder.Append($"   - {orderItem.Quantity} {album.DiscType.GetStringValue()} {album.Name}");
                builder.AppendLine();
            }
            builder.AppendLine().Append($"   Tổng số: {order.TotalPrice.ToString("N0")} VNĐ").AppendLine();

            var mail = new MailMessage(SenderEmail, RealMusicEmail)
            {
                Subject = "Một đơn đặt hàng vừa được tạo trên Real Music",
                Body = builder.ToString(),
                BodyEncoding = Encoding.Unicode,
                SubjectEncoding = Encoding.Unicode
            };

            using (var client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(SenderEmail, SenderPassword)
            })
            {
                client.Send(mail);
            }       
        }
    }
}
