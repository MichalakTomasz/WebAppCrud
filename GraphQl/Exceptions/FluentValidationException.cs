using HotChocolate.Resolvers;

namespace WebAppCrud.GraphQl.Exceptions
{
	public class FluentValidationException : DomainException
	{
        public FluentValidationException() { }
		public FluentValidationException(string message) : base(message) { }
        public FluentValidationException(string message, Exception? exception) : base(message, exception) { }
    }
}
