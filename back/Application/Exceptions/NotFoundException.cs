using Application.Common.MediatR;

namespace Application.Exceptions
{
    public class NotFoundException: AppException
    {
        public NotFoundException():base("Requested data not found.")
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
