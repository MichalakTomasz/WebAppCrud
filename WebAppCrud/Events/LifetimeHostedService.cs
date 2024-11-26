namespace WebAppCrud.Events
{
    public class LifetimeHostedService : IHostedService
    {
        private readonly ApplicationLifetimeEvents _lifetimeEvents;

        public LifetimeHostedService(ApplicationLifetimeEvents lifetimeEvents)
        {
            _lifetimeEvents = lifetimeEvents;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // This will force the construction of ApplicationLifetimeEvents.
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
