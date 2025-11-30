namespace TendalProject.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }
        protected Result(bool isSuccess, Error? error = null)
        {
            if (isSuccess && error != null || !isSuccess && error == null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true);
        public static Result Failure(Error error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }
        protected internal Result(T value) : base(true) => Value = value;
        protected internal Result(Error error) : base(false, error) { }

        public static Result<T> Success(T value) => new(value);
        public new static Result<T> Failure(Error error) => new(error);
    }
}
