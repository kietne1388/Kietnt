namespace FastFood.Models.Common
{
    public class ApiResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ApiResponseModel<T> Success(T data, string message = "Success")
        {
            return new ApiResponseModel<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseModel<T> Fail(string message, List<string>? errors = null)
        {
            return new ApiResponseModel<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new()
            };
        }
    }
}
