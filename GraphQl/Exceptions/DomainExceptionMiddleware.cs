using HotChocolate.Resolvers;

namespace WebAppCrud.GraphQl.Exceptions
{
	public class DomainExceptionMiddleware : DomainException
	{
		private readonly FieldDelegate _next;

		public DomainExceptionMiddleware(FieldDelegate next)
			=> _next = next;

		public async Task InvokeAsync(IMiddlewareContext context)
		{
			try
			{
				await _next(context);
			}
			catch (DomainException exception) { }
		}
	}
}
