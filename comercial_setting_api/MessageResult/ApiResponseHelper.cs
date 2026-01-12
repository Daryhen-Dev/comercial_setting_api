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
        public static MessageResult<T> SuccessResponseEmpty<T>(string message = "Operación exitosa sin datos.")
        {
            return new MessageResult<T>
            {
                Success = true,
                Message = message,
                Data = default
            };
        }

        public static MessageResult<T> CustomResponse<T>(T data, bool state,  string message )
        {
            return new MessageResult<T>
            {
                Success = state,
                Message = message,
                Data = data
            };
        }

        public static MessageResult<T> ResponseToken<T>(T data, string token, bool state, string message)
        {
            return new MessageResult<T>
            {
                Success = state,
                Message = message,
                Data = data,
                Token = token
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
