using System.Text.Json;

namespace TripsEntityFramework
{
    public class ApiResponse<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}