namespace comercial_setting_api.MessageResult
{
    public static class ApiResponseHelper
    {
        public static MessageResult<T> SuccessResponse<T>(T data, string message = "Operación exitosa.")
        {
            return new MessageResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static MessageResult<T> ErrorResponse<T>(string message)
        {
            return new MessageResult<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
