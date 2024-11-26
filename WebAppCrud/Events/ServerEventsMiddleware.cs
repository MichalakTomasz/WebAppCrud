using MediatR;
using WebAppCrud.Notifications;

namespace WebAppCrud.Events
{
    public class ServerEventsMiddleware 
    { 
        private readonly RequestDelegate _next; 
        private readonly IMediator _mediator;

        public ServerEventsMiddleware(
            RequestDelegate next, 
            ILogger<ServerEventsMiddleware> logger,
            IMediator mediator) 
        { 
            _next = next;
            _mediator = mediator;
        } 
        
        public async Task InvokeAsync(HttpContext context) 
        { 
            try 
            {
                await _next(context); 
            } catch (Exception ex) 
            {
                LoggerNotification notification = new()
                {
                    NotificationType = Domain.Models.NotificationType.Exception,
                    Message = "ApplicationError",
                    ErrorMessage = $"exception: {ex.Message}, inner exception: {ex.InnerException}"
                };
                await _mediator.Publish(notification);
            } 
        } 
    }
}
