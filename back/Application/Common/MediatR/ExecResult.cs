namespace Application.Common.MediatR
{
    public class ExecResult<T> : ExecResult
    {
        public T? Result { get; set; }
    }

    public class ExecResult
    {
        public AppException? Error { get; set; }

        public static ExecResult<UResult> FromError<UResult>(AppException error) => new ExecResult<UResult>
        {
            Error = error
        };

        public static ExecResult FromError(AppException error) => new ExecResult
        {
            Error = error
        };

        public static ExecResult Success() => new ExecResult{};

        public static ExecResult<U> Success<U>(U result) => new ExecResult<U>
        {
            Result = result
        };
    }

}
