using Domain.Models;
using MediatR;
using WebAppCrud.Notifications;

namespace WebAppCrud.Events
{
    public class ApplicationLifetimeEvents
    {
        private readonly IMediator _mediator;

        public ApplicationLifetimeEvents(
            IHostApplicationLifetime appLifetime,
            IMediator mediator)
        {
            appLifetime.ApplicationStarted.Register(() => LogAppEvent("Application started."));
            appLifetime.ApplicationStopping.Register(() => LogAppEvent("Application is being closed."));
            appLifetime.ApplicationStopped.Register(() => LogAppEvent("Application stoped."));
            _mediator = mediator;
        }

        private async Task LogAppEvent(string message)
        {
            LoggerNotification notification = new()
            {
                Message = message,
                NotificationType = NotificationType.AppEvent
            };
            await _mediator.Publish(notification);
        }
    }

}
