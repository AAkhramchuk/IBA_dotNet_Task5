namespace Domain.Entities.Wrappers
{
    /// <summary>
    /// Responsed parameters for the HTTP request output
    /// </summary>
    /// <typeparam name="T">Any data type</typeparam>
    public class Response<T>
    {
        // Data payload
        public T? Data { get; set; }
        // Request succeeded or not
        public bool Succeeded { get; set; }
        // Array of errors if any
        public string[]? Errors { get; set; }
        // Any message
        public string? Message { get; set; }

        /// <summary>
        /// Succeeded empty response
        /// </summary>
        public Response()
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = default;
        }
        /// <summary>
        /// Succeeded response with any data
        /// </summary>
        /// <param name="data">Data payload</param>
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
    }
}
