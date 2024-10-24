using Domain.Interfaces;

namespace WebAppCrud.Exceptions
{
    public class InternalServerError : Exception, IErrorCode
    {
        public InternalServerError(string message) : base(message) { }

        public int ErrorCode => 500;
    }
}
