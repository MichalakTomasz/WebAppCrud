using Domain.Models;
using MediatR;

namespace WebAppCrud.Notifications
{
	public class LoggerNotification : INotification
	{
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public int? UserId { get; set; }
        public string ErrorMessage { get; set; }
        public string Ip { get; set; }
    }
}
