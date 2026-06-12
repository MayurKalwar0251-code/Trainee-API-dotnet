public class ServiceResult<T>
{
    public bool Success {get; set;}
    public string Message {get; set;} = string.Empty;
    public int? Status {get; set;}
    public T? Data {get; set;}

    public static ServiceResult<T> Ok(T data,string message = "Success",int status = 200)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Message = message,
            Status = status,
            Data = data
        };
    }

    public static ServiceResult<T> Fail(string message, int status = 400)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Message = message,
            Status = status!
        }; 
    }
}