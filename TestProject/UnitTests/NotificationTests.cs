using Moq;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Notifications;
using Domain.Models;
using System;
using WebAppCrud.Notifications;

namespace TestProject.UnitTests
{
    public class NotificationTests
    {
        [Theory]
        [InlineData(NotificationType.Information, LogLevel.Information, "info")]
        [InlineData(NotificationType.Warning, LogLevel.Warning, "warning")]
        [InlineData(NotificationType.Error, LogLevel.Error, "error")]
        public async Task NotificationTestPassed(NotificationType notificationType, LogLevel logLevel, string message)
        {
            var loggerMock = new Mock<ILogger<LoggerNotificationHandler>>();
            LoggerNotification loggerNotification = new()
            {
                Message = message,
                NotificationType = notificationType
            };

            LoggerNotificationHandler loggerNotificationHandler = new(loggerMock.Object);
            await loggerNotificationHandler.Handle(loggerNotification, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
