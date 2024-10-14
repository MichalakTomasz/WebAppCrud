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
			catch (FluentValidationException exception) 
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
			catch(DomainException ex)
			{
				ErrorBuilder.New().SetMessage(ex.Message);
			}
		}
	}
}
