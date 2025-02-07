﻿using MediatR;

namespace WebAppCrud.Behaviors
{
	public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
		public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
			=> _logger = logger;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handing {typeof(TRequest).Name}");

			var response = await next();

			_logger.LogInformation($"Handing {typeof(TResponse).Name}");

			return response;
		}
	}
}
