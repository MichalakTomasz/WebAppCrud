using Domain.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace WebAppCrud.Notifications
{
	public class LoggerNotificationHandler : INotificationHandler<LoggerNotification>
	{
		private readonly ILogger<LoggerNotificationHandler> _logger;

		public LoggerNotificationHandler(ILogger<LoggerNotificationHandler> logger)
        {
			_logger = logger;
		}
        public Task Handle(LoggerNotification notification, CancellationToken cancellationToken)
		{
			switch (notification.NotificationType)
			{
				case NotificationType.Information:
					_logger.LogInformation(notification.Message);
					break;
				case NotificationType.Warning:
					_logger.LogWarning(notification.Message);
					break;
				case NotificationType.Error:
					_logger.LogError(notification.Message);
					break;
				default:
					_logger.LogInformation($"EventType: {notification.NotificationType}, Message: {notification.Message}{GetIpMessage(notification.Ip)}{GetUserIdMessage(notification.UserId)}{GetErrorMessage(notification.ErrorMessage)}");
					break;
			}

			return Task.CompletedTask;
		}

		private string GetIpMessage(string ip)
			=> ip.IsNullOrEmpty() ? string.Empty: $"{Environment.NewLine}IP: {ip}";

		private string GetUserIdMessage(int? userId)
			=> userId == default ? string.Empty : $"{Environment.NewLine} User Id: {userId}";

		private string GetErrorMessage(string errorMessage)
			=> errorMessage.IsNullOrEmpty() ? string.Empty : $"{Environment.NewLine} Error message: {errorMessage}";
	}
}
