using Domain.Interfaces;

namespace WebAppCrud.Exceptions
{
    public class NotFoundException : Exception, IErrorCode
    {
        public NotFoundException(string message) : base(message) { }

        public int ErrorCode => 404;
    }
}
