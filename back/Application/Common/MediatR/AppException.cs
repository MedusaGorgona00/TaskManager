namespace Application.Common.MediatR
{
    public class AppException: Exception
    {
        public AppException(string message):base(message)
        {

        }
    }
}
