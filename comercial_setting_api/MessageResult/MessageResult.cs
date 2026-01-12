namespace comercial_setting_api.MessageResult
{
    public class MessageResult<T>
    {
        public bool Success { get; set; }      
        public required string Message { get; set; }    
        public T? Data { get; set; }
        public string? Token { get; set; }

    }
}
