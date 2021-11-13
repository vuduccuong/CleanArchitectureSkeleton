namespace Application.Common.Models
{
    public class ResultBase<TResponse>
    {
        public bool IsSuccess { get; set; }
        public TResponse Value { get; set; }
        public string Error { get; set; }

        public static ResultBase<TResponse> Success(TResponse value) => new() { IsSuccess = true, Value = value };

        public static ResultBase<TResponse> Failure(string error) => new() { IsSuccess = false, Error = error };
    }
}
